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
            string dbPath = null;
            
            if (args.Length != 0)
            {
                dbPath = args[0];
            }

            //PasswordInputForm passwordInputForm = new PasswordInputForm();
            RepositoryManagerForm repositoryManager = new RepositoryManagerForm(null, dbPath);
            RepositoryViewerForm viewerForm = new RepositoryViewerForm(repositoryManager);
            Application.Run(viewerForm);
        }
    }
}
