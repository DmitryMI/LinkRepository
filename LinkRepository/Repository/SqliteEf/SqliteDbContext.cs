using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkRepository.Repository.SqliteEf
{
    class SqliteDbContext : DbContext, IRepository
    {
        public SqliteDbContext(string dbPath) :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                    {DataSource = dbPath, ForeignKeys = true}.ConnectionString
            }, true)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<LinkTable>().Property(t => t.CreatedTimestampShadow).HasColumnName("CreatedTimestamp");
        }

        public DbSet<LinkTable> LinkTables { get; set; }
        public IEnumerator<ILinkTableRow> GetEnumerator()
        {
            return LinkTables.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ILinkTableRow item)
        {
            if (item == null)
            {
                return;
            }
            LinkTables.Add((LinkTable) item);
        }

        public void Clear()
        {
            for (int i = 0; i < LinkTables.Count(); i++)
            {
                LinkTables.Remove(LinkTables.First());
            }
        }

        public bool Contains(ILinkTableRow item)
        {
            return LinkTables.Contains(item);
        }

        public void CopyTo(ILinkTableRow[] array, int arrayIndex)
        {
            LinkTable[] rowArray = LinkTables.ToArray();
            Array.Copy(rowArray, 0, array, 0, rowArray.Length);
        }

        public bool Remove(ILinkTableRow item)
        {
            if (item == null)
            {
                return false;
            }
            LinkTables.Remove((LinkTable) item);
            return true;
        }

        public int Count => LinkTables.Count();
        public bool IsReadOnly => false;
        public int IndexOf(ILinkTableRow item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ILinkTableRow item)
        {
            if (item == null)
            {
                return;
            }
            LinkTables.Add((LinkTable)item);
        }

        public void RemoveAt(int index)
        {
            var element = LinkTables.Where(e => e.LinkIndex == index);
            LinkTables.Remove(element.First());
        }

        public ILinkTableRow this[int index]
        {
            get => LinkTables.First(e => e.LinkIndex == index);
            set
            {
                RemoveAt(index);
                LinkTables.Add((LinkTable)value);
            }
        }

        public void Load()
        {
            
        }

        public void Save()
        {
            SaveChanges();
        }

        public bool HasUnsavedChanges => ChangeTracker.HasChanges();
        public ILinkTableRow CreateLinkTableRow()
        {
            return LinkTables.Create();
        }

        public void OpenRepository()
        {
            
        }
    }
}
