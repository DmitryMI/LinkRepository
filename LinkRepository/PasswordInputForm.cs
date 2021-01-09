using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkRepository.Repository;

namespace LinkRepository
{
    public partial class PasswordInputForm : Form, IPasswordProvider
    {
        public PasswordInputForm()
        {
            InitializeComponent();

            MessageLabel.Text = "";
            MessageLabel.ForeColor = Color.Black;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Password = PasswordBox.Text;
            OnPasswordEnteredEvent?.Invoke(this, Password);
        }

        public event Action<IPasswordProvider, string> OnPasswordEnteredEvent;
        public bool IsPasswordAvailable => Password != null;
        public string Password { get; set; } = null;
        public void ReportCorrectPassword()
        {
            MessageLabel.Text = "OK";
            MessageLabel.ForeColor = Color.Green;
            Close();
        }

        public void ReportWrongPassword()
        {
            MessageLabel.Text = "Wrong password";
            MessageLabel.ForeColor = Color.Red;
        }
    }
}
