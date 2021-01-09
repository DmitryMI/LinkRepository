using System;
using System.IO;
using System.Windows.Forms;
using LinkRepository.Repository;
using LinkRepository.Repository.Sqlite;

namespace LinkRepository
{
    public partial class RepositoryManagerForm : Form, IRepositoryProvider
    {
        private string _dbPath;
        public IRepository Repository { get; private set; }
        public event Action<IRepositoryProvider, IRepository> OnRepositoryLoadedEvent;
        public void RequestRepository()
        {
            if (!String.IsNullOrWhiteSpace(_dbPath) && File.Exists(_dbPath))
            {
                ProvideRepository(_dbPath);
            }
            else
            {
                Show();
            }
        }

        public void ReportRepositoryProvided()
        {
            Close();
            //Hide();
        }

        public RepositoryManagerForm(string dbPath)
        {
            InitializeComponent();
            _dbPath = dbPath;
        }

        private void ProvideRepository(string repositoryPath)
        {
            IRepository repository = null;
            bool ok = false; 
            try
            {
                repository = new SqliteRepository(repositoryPath);
                repository.OpenRepository();
                repository.Load();
                ok = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading selected repository", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (repository != null && ok)
            {
                Repository = repository;
                OnRepositoryLoadedEvent?.Invoke(this, Repository);
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "Sqlite db (*.db)|*.db|All files (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string fileName = dialog.FileName;
            ProvideRepository(fileName);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = false;
            dialog.Filter = "Sqlite db (*.db)|*.db|All files (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            string fileName = dialog.FileName;
            ProvideRepository(fileName);
        }
    }
}
