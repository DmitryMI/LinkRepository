using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LinkRepository.Repository;
using LinkRepository.Repository.Sqlite;

namespace LinkRepository
{
    public partial class RepositoryManagerForm : Form, IRepositoryProvider
    {
        private string _dbPath;
        private IPasswordProvider _passwordProvider;
        public IRepository Repository { get; private set; }
        public event Action<IRepositoryProvider, IRepository> OnRepositoryLoadedEvent;
        public void RequestRepository()
        {
            if (!String.IsNullOrWhiteSpace(_dbPath) && File.Exists(_dbPath))
            {
                ProvideRepository();
            }
            else
            {
                ShowSelf();
            }
        }

        private void ShowSelf()
        {
            TopMost = true;
            BringToFront();
            Show();
        }

        public void ReportRepositoryProvided()
        {
            Hide();
        }

        public RepositoryManagerForm(IPasswordProvider passwordProvider, string dbPath)
        {
            _passwordProvider = passwordProvider;
            if (_passwordProvider != null)
            {
                _passwordProvider.OnPasswordEnteredEvent += (i, p) => ProvideRepository(p);
            }
            InitializeComponent();
            _dbPath = dbPath;
        }

        private void InvokePasswordInput()
        {
            if (_passwordProvider != null)
            {
                _passwordProvider.RequestPassword();
            }
            else
            {
                ProvideRepository(null);
            }
        }


        private void ProvideRepository(string password = null)
        {
            string repoPath = _dbPath;
            _dbPath = null;
            IRepository repository = null;
            bool ok = false; 
            try
            {
                if (password == null)
                {
                    repository = new SqliteRepository(repoPath);
                }
                else
                {
                    repository = new SqliteEncryptedRepository(repoPath, password);
                }
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
                _passwordProvider?.ReportCorrectPassword();
                OnRepositoryLoadedEvent?.Invoke(this, Repository);
            }
            else
            {
                // TODO No guarantee that password is actually wrong
                _passwordProvider?.ReportWrongPassword();
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

            _dbPath = dialog.FileName;
            InvokePasswordInput();
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

            _dbPath = dialog.FileName;
            InvokePasswordInput();
        }

        private void RepositoryManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void TopMostTimer_Tick(object sender, EventArgs e)
        {
            TopMost = false;
            TopMostTimer.Stop();
        }
    }
}
