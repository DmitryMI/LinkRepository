using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace LinkRepository.Repository.Sqlite
{
    public class SqliteEncryptedRepository : SqliteRepository
    {
        private string _password;
        public SqliteEncryptedRepository(string dbPath, string password) : base(dbPath)
        {
            _password = password;
        }

        public override void OpenRepository()
        {
            var csBuilder = new SqliteConnectionStringBuilder();
            csBuilder.Password = _password;
            csBuilder.DataSource = DbPath;
            csBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
            string cs = csBuilder.ToString();
            SqliteConnection = new SqliteConnection(cs);
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_dynamic_cdecl());
        }
    }
}
