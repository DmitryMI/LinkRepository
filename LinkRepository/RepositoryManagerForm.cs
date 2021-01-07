using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkRepository.Repository;
using LinkRepository.Repository.Sqlite;

namespace LinkRepository
{
    public partial class RepositoryManagerForm : Form
    {
        public RepositoryManagerForm()
        {
            InitializeComponent();
        }

        private void OnRepositoryViewerClosed(object sender, EventArgs args)
        {
            Close();
        }

        private void InvokeRepositoryViewer(string repositoryPath)
        {
            IRepository repository = null;
            bool ok = false; 
            try
            {
                repository = new SqliteRepository(repositoryPath);
                repository.Load();
                ok = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading selected repository", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (repository != null && ok == true)
            {
                this.Hide();
                RepositoryViewerForm viewer = new RepositoryViewerForm(repository);
                viewer.Closed += OnRepositoryViewerClosed;
                viewer.Show();
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
            InvokeRepositoryViewer(fileName);
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
            InvokeRepositoryViewer(fileName);
        }
    }
}
