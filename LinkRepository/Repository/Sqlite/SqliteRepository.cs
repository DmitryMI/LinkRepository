using System;
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
        private List<SqliteLinkTableRow> _rowList;
        private List<SqliteLinkTableRow> _removedList = new List<SqliteLinkTableRow>();

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

        public IEnumerator<ILinkTableRow> GetEnumerator()
        {
            return _rowList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _rowList).GetEnumerator();
        }

        public void Add(ILinkTableRow item)
        {
            _hasUnsavedModifications = true;
            _rowList.Add((SqliteLinkTableRow)item);
        }

        public void Clear()
        {
            _hasUnsavedModifications = true;
            _removedList.AddRange(_rowList);
            _rowList.Clear();
        }

        public bool Contains(ILinkTableRow item)
        {
            return _rowList.Contains(item);
        }

        public void CopyTo(ILinkTableRow[] array, int arrayIndex)
        {
            _rowList.CopyTo((SqliteLinkTableRow[])array, arrayIndex);
        }

        public bool Remove(ILinkTableRow item)
        {
            _hasUnsavedModifications = true;
            _removedList.Add((SqliteLinkTableRow)item);
            return _rowList.Remove((SqliteLinkTableRow)item);
        }

        public int Count => _rowList.Count;

        public bool IsReadOnly => false;

        public int IndexOf(ILinkTableRow item)
        {
            if (item == null)
            {
                return -1;
            }
            return item.Index;
        }

        public void Insert(int index, ILinkTableRow item)
        {
            _hasUnsavedModifications = true;
            _rowList.Remove(_rowList.First(e => e.Index == index));
            _rowList.Add((SqliteLinkTableRow)item);
        }

        public void RemoveAt(int index)
        {
            var item = _rowList.First(e => e.Index == index);
            _hasUnsavedModifications = true;
            _removedList.Add(item);
            _rowList.Remove(item);
        }

        public ILinkTableRow this[int index]
        {
            get => _rowList.First(e => e.Index == index);
            set => Insert(index, value);
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

                    SqliteLinkTableRow row = new SqliteLinkTableRow(rdr.GetInt32(0), false, createDateTime, this)
                    {
                        Uri = rdr.GetString(1),
                        Genre = rdr.GetString(2),
                        Score = rdr.GetInt32(3),
                        Comment = rdr.GetString(4),
                        IsAvailable = rdr.GetInt32(5) != 0,
                        IsLoaded = rdr.GetInt32(6) != 0,
                    };

                    if (!rdr.IsDBNull(RepositoryConstants.ModifiedColumnIndex))
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

                    if (!rdr.IsDBNull(RepositoryConstants.ThumbnailColumnIndex))
                    {
                        var dataStream = rdr.GetStream(RepositoryConstants.ThumbnailColumnIndex);
                        byte[] dataBytes = new byte[dataStream.Length];
                        dataStream.Read(dataBytes, 0, dataBytes.Length);
                        dataStream.Close();
                        dataStream.Dispose();
                        row.ThumbnailBytes = dataBytes;
                    }

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
            _rowList = new List<SqliteLinkTableRow>();
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
                    rowsModified += DeleteRow(removedRow, command);
                }
            }
            _removedList.Clear();

            string insertCommandString =
                $"INSERT INTO LinkTable(LinkIndex, Uri, Genre, Score, Comment, IsAvailable, IsLoaded, ThumbnailBytes, CreatedTimestamp, ModifiedTimestamp)" +
                $"VALUES(@index, @uri, @genre, @score, @comment, @isAvailable, @isLoaded, @thumbnailBytes, @createdTimestamp, @modifiedTimestamp)";
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
                    rowsModified += InsertRow(row, insertCommand);
                }
                else
                {
                    rowsModified += UpdateRow(row, updateCommand);
                }
            }

            insertCommand.Dispose();
            updateCommand.Dispose();

            Debug.WriteLine($"Rows modified: {rowsModified}");
            _sqliteConnection.Close();
            _hasUnsavedModifications = false;
        }

        private int InsertRow(SqliteLinkTableRow row, SqliteCommand insertCommand)
        {
            insertCommand.Parameters.Clear();
            insertCommand.Parameters.AddWithValue("@index", row.Index);
            insertCommand.Parameters.AddWithValue("@uri", row.Uri);
            insertCommand.Parameters.AddWithValue("@genre", row.Genre);
            insertCommand.Parameters.AddWithValue("@score", row.Score);
            insertCommand.Parameters.AddWithValue("@comment", row.Comment);
            insertCommand.Parameters.AddWithValue("@isAvailable", SqliteUtils.BoolToInt(row.IsAvailable));
            insertCommand.Parameters.AddWithValue("@isLoaded", SqliteUtils.BoolToInt(row.IsLoaded));
            insertCommand.Parameters.AddWithValue("@thumbnailBytes", row.ThumbnailBytes);
            insertCommand.Parameters.AddWithValue("@createdTimestamp", row.CreatedTimestamp.ToBinary());
            insertCommand.Parameters.AddWithValue("@modifiedTimestamp", row.ModifiedTimestamp.ToBinary());

            int rowsModified = insertCommand.ExecuteNonQuery();

            row.ResetNewRowFlag();
            row.ResetModifiedFlag();
            return rowsModified;
        }

        private int UpdateRow(SqliteLinkTableRow row, SqliteCommand updateCommand)
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
            int rowsModified = updateCommand.ExecuteNonQuery();

            if (row.IsThumbnailModified)
            {
                UpdateThumbnailBlob(row);
            }

            row.ResetModifiedFlag();
            return rowsModified;
        }

        private int UpdateThumbnailBlob(SqliteLinkTableRow row)
        {
            string updateCommandString =
                $"UPDATE LinkTable\n" +
                "SET\n" +
                $"ThumbnailBytes = @thumbnailBytes\n" +
                $"WHERE LinkIndex = @index";
            SqliteCommand updateCommand = new SqliteCommand(updateCommandString, _sqliteConnection);
            updateCommand.Prepare();
            updateCommand.Parameters.Clear();
            updateCommand.Parameters.AddWithValue("@index", row.Index);
            updateCommand.Parameters.AddWithValue("@thumbnailBytes", row.ThumbnailBytes);
            int rowsModified = updateCommand.ExecuteNonQuery();
            updateCommand.Dispose();
            row.ResetThumbnailModifiedFlag();
            return rowsModified;
        }

        private int DeleteRow(SqliteLinkTableRow row, SqliteCommand deleteCommand)
        {
            deleteCommand.Parameters.Clear();
            deleteCommand.Parameters.AddWithValue("@linkIndex", row.Index);
            deleteCommand.Prepare();
            int rowsModified = deleteCommand.ExecuteNonQuery();
            return rowsModified;
        }

        public bool HasUnsavedChanges => _hasUnsavedModifications;

        public ILinkTableRow CreateLinkTableRow()
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

            var row = new SqliteLinkTableRow(index, true, DateTime.Now,this);
            _rowList.Add(row);
            return row;
        }

        public void ReportModification(object sender)
        {
            _hasUnsavedModifications = true;
        }
    }
}
