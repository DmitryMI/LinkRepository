using System;
using System.Windows.Forms;
using LinkRepository.Repository;
using LinkRepository.Repository.Sqlite;
using LinkRepository.Repository.SqliteEf;

namespace LinkRepository
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form startingForm;
            if (args.Length != 0)
            {
                string dbPath = args[0];
                //IRepository repository = new SqliteRepository(dbPath);
                IRepository repository = new SqliteDbContext(dbPath);
                repository.Load();
                startingForm = new RepositoryViewerForm(repository);
            }
            else
            {
                startingForm = new RepositoryManagerForm();
            }


            Application.Run(startingForm);
        }
    }
}
