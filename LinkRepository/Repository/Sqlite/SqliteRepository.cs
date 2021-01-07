﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace LinkRepository.Repository.Sqlite
{
    public class SqliteRepository : IRepository, IModificationReporter
    {
        private readonly SqliteConnection _sqliteConnection;
        private List<LinkTableRow> _rowList;
        private List<LinkTableRow> _removedList = new List<LinkTableRow>();

        private bool _hasUnsavedModifications = false;
        public SqliteRepository(string dbPath)
        {            
            var csBuilder = new SqliteConnectionStringBuilder();
            csBuilder.DataSource = dbPath;
            csBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
            string cs = csBuilder.ToString();
            _sqliteConnection = new SqliteConnection(cs);
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_dynamic_cdecl());
        }

        public void Dispose()
        {
            _sqliteConnection.Close();
            _sqliteConnection.Dispose();
        }

        public IEnumerator<LinkTableRow> GetEnumerator()
        {
            return _rowList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _rowList).GetEnumerator();
        }

        public void Add(LinkTableRow item)
        {
            _hasUnsavedModifications = true;
            _rowList.Add(item);
        }

        public void Clear()
        {
            _hasUnsavedModifications = true;
            _removedList.AddRange(_rowList);
            _rowList.Clear();
        }

        public bool Contains(LinkTableRow item)
        {
            return _rowList.Contains(item);
        }

        public void CopyTo(LinkTableRow[] array, int arrayIndex)
        {
            _rowList.CopyTo(array, arrayIndex);
        }

        public bool Remove(LinkTableRow item)
        {
            _hasUnsavedModifications = true;
            _removedList.Add(item);
            return _rowList.Remove(item);
        }

        public int Count => _rowList.Count;

        public bool IsReadOnly => false;

        public int IndexOf(LinkTableRow item)
        {
            return _rowList.IndexOf(item);
        }

        public void Insert(int index, LinkTableRow item)
        {
            _hasUnsavedModifications = true;
            _rowList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _hasUnsavedModifications = true;
            _removedList.Add(_rowList[index]);
            _rowList.RemoveAt(index);
        }

        public LinkTableRow this[int index]
        {
            get => _rowList[index];
            set => _rowList[index] = value;
        }

        private void CreateLinkTable()
        {
            
            string cmdString = "CREATE TABLE IF NOT EXISTS LinkTable" +
                "(\n" +
                    "LinkIndex INTEGER PRIMARY KEY,\n" +
                    "Uri TEXT,\n" +
                    "Genre TEXT,\n" +
                    "Score INTEGER," +
                    "Comment TEXT,\n" +
                    "IsAvailable INTEGER,\n" +
                    "IsLoaded INTEGER,\n" +
                    "CreatedTimestamp INTEGER,\n"+
                    "ModifiedTimestamp INTEGER,\n"+
                    "ThumbnailBytes BLOB\n" +
                 ");";
            
            //string cmdString =
            //    "CREATE TABLE IF NOT EXISTS LinkTable (\r\n\tIndex INTEGER PRIMARY KEY,\r\n\tfirst_name TEXT NOT NULL,\r\n\tlast_name TEXT NOT NULL,\r\n\temail TEXT NOT NULL UNIQUE,\r\n\tphone TEXT NOT NULL UNIQUE\r\n);";
            _sqliteConnection.Open();
            using (var cmd = new SqliteCommand(cmdString, _sqliteConnection))
            {
                cmd.ExecuteNonQuery();
            }
            _sqliteConnection.Close();
        }

        private void ReadLinkTable()
        {
            // TODO Replace with strict SELECT
            string stm = "SELECT * FROM LinkTable";
            _sqliteConnection.Open();
            var cmd = new SqliteCommand(stm, _sqliteConnection);
            int erroneousRowsCount = 0;
            using (SqliteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    bool resetModifiedFlag = true;
                    DateTime createDateTime = DateTime.Now;
                    
                    if (!rdr.IsDBNull(7))
                    {
                        long createdTimestamp = rdr.GetInt64(7);
                        createDateTime = DateTime.FromBinary(createdTimestamp);
                    }
                    else
                    {
                        resetModifiedFlag = false;
                        erroneousRowsCount++;
                    }

                    LinkTableRow row = new LinkTableRow(rdr.GetInt32(0), false, createDateTime, this)
                    {
                        Uri = rdr.GetString(1),
                        Genre = rdr.GetString(2),
                        Score = rdr.GetInt32(3),
                        Comment = rdr.GetString(4),
                        IsAvailable = rdr.GetInt32(5) != 0,
                        IsLoaded = rdr.GetInt32(6) != 0,
                        ThumbnailBytes = null
                    };

                    if (!rdr.IsDBNull(8))
                    {
                        long modifiedTimestamp = rdr.GetInt64(8);
                        DateTime modifiedDateTime = DateTime.FromBinary(modifiedTimestamp);
                        row.ModifiedTimestamp = modifiedDateTime;
                    }
                    else
                    {
                        DateTime modifiedDateTime = DateTime.Now;
                        row.ModifiedTimestamp = modifiedDateTime;
                        resetModifiedFlag = false;
                        erroneousRowsCount++;
                    }
                    // TODO Thumbnail
                    if (resetModifiedFlag)
                    {
                        row.ResetModifiedFlag();
                    }

                    _rowList.Add(row);
                    Debug.WriteLine($"Row: {row.Index}, {row.IsAvailable}, {row.IsLoaded}");
                }
            }
            _sqliteConnection.Close();
            if (erroneousRowsCount == 0)
            {
                _hasUnsavedModifications = false;
            }
        }

        public void Load()
        {
            _rowList = new List<LinkTableRow>();
            CreateLinkTable();
            ReadLinkTable();
        }

        public void Save()
        {
            _sqliteConnection.Open();
            int rowsModified = 0;
            string deleteCommandString = "DELETE FROM LinkTable\r\nWHERE LinkIndex = @linkIndex;";
            
            using (SqliteCommand command = new SqliteCommand(deleteCommandString, _sqliteConnection))
            {
                foreach (var removedRow in _removedList)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@linkIndex", removedRow.Index);
                    command.Prepare();
                    rowsModified += command.ExecuteNonQuery();
                }
            }
            _removedList.Clear();

            string insertCommandString =
                $"INSERT INTO LinkTable(LinkIndex, Uri, Genre, Score, Comment, IsAvailable, IsLoaded, CreatedTimestamp, ModifiedTimestamp)" +
                $"VALUES(@index, @uri, @genre, @score, @comment, @isAvailable, @isLoaded, @createdTimestamp, @modifiedTimestamp)";
            SqliteCommand insertCommand = new SqliteCommand(insertCommandString, _sqliteConnection);

            string updateCommandString =
                $"UPDATE LinkTable\n" +
                "SET\n" +
                $"Uri = @uri,\n" +
                $"Genre = @genre,\n" +
                $"Score = @score,\n" +
                $"Comment = @comment,\n" +
                $"IsAvailable = @isAvailable,\n" +
                $"IsLoaded = @isLoaded,\n" +
                $"CreatedTimestamp = @createdTimestamp,\n" +
                $"ModifiedTimestamp = @modifiedTimestamp\n" +
                $"WHERE LinkIndex = @index";
            SqliteCommand updateCommand = new SqliteCommand(updateCommandString, _sqliteConnection);

            foreach (var row in _rowList)
            {
                if (!row.IsModified)
                {
                    continue;
                }

                if (row.IsNewRow)
                {
                    insertCommand.Parameters.Clear();
                    insertCommand.Parameters.AddWithValue("@index", row.Index);
                    insertCommand.Parameters.AddWithValue("@uri", row.Uri);
                    insertCommand.Parameters.AddWithValue("@genre", row.Genre);
                    insertCommand.Parameters.AddWithValue("@score", row.Score);
                    insertCommand.Parameters.AddWithValue("@comment", row.Comment);
                    insertCommand.Parameters.AddWithValue("@isAvailable", SqliteUtils.BoolToInt(row.IsAvailable));
                    insertCommand.Parameters.AddWithValue("@isLoaded", SqliteUtils.BoolToInt(row.IsLoaded));
                    insertCommand.Parameters.AddWithValue("@createdTimestamp", row.CreatedTimestamp.ToBinary());
                    insertCommand.Parameters.AddWithValue("@modifiedTimestamp", row.ModifiedTimestamp.ToBinary());

                    rowsModified += insertCommand.ExecuteNonQuery();
                    
                    row.ResetNewRowFlag();
                }
                else
                {
                    updateCommand.Parameters.Clear();
                    updateCommand.Parameters.AddWithValue("@index", row.Index);
                    updateCommand.Parameters.AddWithValue("@uri", row.Uri);
                    updateCommand.Parameters.AddWithValue("@genre", row.Genre);
                    updateCommand.Parameters.AddWithValue("@score", row.Score);
                    updateCommand.Parameters.AddWithValue("@comment", row.Comment);
                    updateCommand.Parameters.AddWithValue("@isAvailable", SqliteUtils.BoolToInt(row.IsAvailable));
                    updateCommand.Parameters.AddWithValue("@isLoaded", SqliteUtils.BoolToInt(row.IsLoaded));
                    updateCommand.Parameters.AddWithValue("@createdTimestamp", row.CreatedTimestamp.ToBinary());
                    updateCommand.Parameters.AddWithValue("@modifiedTimestamp", row.ModifiedTimestamp.ToBinary());
                    rowsModified += updateCommand.ExecuteNonQuery();
                }

                row.ResetModifiedFlag();
            }

            insertCommand.Dispose();
            updateCommand.Dispose();

            Debug.WriteLine($"Rows modified: {rowsModified}");
            _sqliteConnection.Close();
            _hasUnsavedModifications = false;
        }

        public bool HasUnsavedChanges => _hasUnsavedModifications;

        public LinkTableRow CreateLinkTableRow()
        {
            if (_rowList == null)
            {
                throw new InvalidOperationException("Call Load first");
            }

            int index = 0;
            if (_rowList.Count != 0)
            {
                index = _rowList.Last().Index + 1;
            }

            var row = new LinkTableRow(index, true, DateTime.Now,this);
            _rowList.Add(row);
            return row;
        }

        public void ReportModification(object sender)
        {
            _hasUnsavedModifications = true;
        }
    }
}
