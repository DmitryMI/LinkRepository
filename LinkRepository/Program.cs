using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkRepository.Repository;
using LinkRepository.Repository.Sqlite;

namespace LinkRepository
{
    static class Program
    {
        private static Random _rnd = new Random();

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IRepository repository = new SqliteRepository("data.db");
            repository.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RepositoryViewerForm(repository));
        }

        static void AddRandomRow(IRepository repository)
        {
            var row = repository.CreateLinkTableRow();
            row.Uri = $"www.google{_rnd.Next()}.com";
            row.Genre = $"Bullshit {_rnd.Next()}";
            row.Comment = $"See genre {_rnd.Next()} ";
            row.Score = _rnd.Next(0, 100);
            row.IsAvailable = _rnd.Next(0, 2) != 0;
            row.IsLoaded = _rnd.Next(0, 2) != 0;
        }
    }
}
