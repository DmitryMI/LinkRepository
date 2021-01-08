using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkRepository.Preferences;

namespace LinkRepository
{
    public partial class PreferencesEditorForm : Form
    {
        private readonly PreferencesContainer _preferencesContainer;
        public PreferencesEditorForm(PreferencesContainer preferencesContainer)
        {
            _preferencesContainer = preferencesContainer;
            InitializeComponent();

            UpdateUi();
        }

        private void UpdateUi()
        {
            LinkTableFontSizeBox.Value = (decimal)_preferencesContainer.LinkTableFontSize;
        }

        private void StoreUi()
        {
            _preferencesContainer.LinkTableFontSize = (float)LinkTableFontSizeBox.Value;
        }

        private void LinkTableFontSizeBox_ValueChanged(object sender, EventArgs e)
        {
            StoreUi();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            StoreUi();
            Close();
        }
    }
}
