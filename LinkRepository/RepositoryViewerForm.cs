using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LinkRepository.Preferences;
using LinkRepository.Repository;
using LinkRepository.Repository.Sqlite;
using LinkRepository.Utils;

namespace LinkRepository
{
    public partial class RepositoryViewerForm : Form
    {
        private IRepository _repository;
        private IRepositoryProvider _repositoryProvider;
        private PreferencesContainer _preferences;
        private bool _ignoreFocusModeChanges = true;

        public RepositoryViewerForm(IRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            _repository.Load();
            OnRepositoryLoaded();
        }

        public RepositoryViewerForm(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            InitializeComponent();
            _repositoryProvider.OnRepositoryLoadedEvent += OnRepositoryProvided;
        }

        private void OnRepositoryProvided(IRepositoryProvider provider, IRepository repository)
        {
            _repository = repository;
            _repositoryProvider.ReportRepositoryProvided();
            OnRepositoryLoaded();
        }

        private void OnRepositoryLoaded()
        {
            ExitFocusMode();
            UpdateDataView();
            LoadPreferences();
            ResizeTableView();
            SetupDefaultSorting();

            LinkTableView.ClearSelection();
        }

        private void SetupDefaultSorting()
        {
            var column = LinkTableView.Columns[RepositoryConstants.ScoreColumnIndex];
            if (column == null)
            {
                return;
            }
            LinkTableView.Sort(column, ListSortDirection.Descending);
        }

        private void LoadColumnsFromPreferences()
        {
            foreach (DataGridViewColumn column in LinkTableView.Columns)
            {
                column.Visible = false;
            }
            foreach (var columnName in _preferences.VisibleColumns)
            {
                var column = LinkTableView.Columns[columnName];
                if (column != null)
                {
                    column.Visible = true;
                }
            }
        }

        private void LoadTableFontSizeFromPreferences()
        {
            Font font = new Font(LinkTableView.Font.FontFamily, _preferences.LinkTableFontSize, LinkTableView.Font.Style);
            LinkTableView.Font = font;
        }

        private void SaveColumnsToPreferences()
        {
            if (_preferences != null)
            {
                var list = new List<string>();
                foreach (DataGridViewColumn column in LinkTableView.Columns)
                {
                    if (column.Visible)
                    {
                        list.Add(column.Name);
                    }
                }

                _preferences.VisibleColumns = list.ToArray();
            }
        }

        private void SaveTableFontSizeToPreferences()
        {
            if (_preferences != null)
            {
                _preferences.LinkTableFontSize = LinkTableView.Font.Size;
            }
        }

        private void LoadPreferences()
        {
            _preferences = new PreferencesContainer("LinkRepository.xml");
            if (_preferences.Deserialize())
            {
                LoadColumnsFromPreferences();
            }
            LoadTableFontSizeFromPreferences();
        }

        private void SavePreferences()
        {
            if (_preferences != null)
            {
                SaveColumnsToPreferences();
                SaveTableFontSizeToPreferences();
                _preferences.Serialize();
            }
        }

        private int GetRowRepositoryNumber(DataGridViewRow viewRow)
        {
            return (int) viewRow.Cells[0].Value;
        }

        private void DecorateRow(DataGridViewRow viewRow)
        {
            bool isAvailable = (bool) viewRow.Cells[RepositoryConstants.IsAvailableColumnIndex].Value;
            if (!isAvailable)
            {
                viewRow.DefaultCellStyle.BackColor = Color.Red;
            }
            else
            {
                viewRow.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void CreateViewRow(DataGridViewRow viewRow, ILinkTableRow row)
        {
            viewRow.Cells.Clear();
            DataGridViewCell indexCell = new DataGridViewTextBoxCell();
            indexCell.Value = row.LinkIndex;
            DataGridViewCell uriCell = new DataGridViewTextBoxCell();
            uriCell.Value = row.Uri;
            DataGridViewCell genreCell = new DataGridViewTextBoxCell();
            genreCell.Value = row.Genre;
            DataGridViewCell scoreCell = new DataGridViewTextBoxCell();
            scoreCell.Value = row.Score;
            DataGridViewCell commentCell = new DataGridViewTextBoxCell();
            commentCell.Value = row.Comment;
            DataGridViewCell isAvailableCell = new DataGridViewCheckBoxCell();
            isAvailableCell.Value = row.IsAvailable;
            DataGridViewCell isLoadedCell = new DataGridViewCheckBoxCell();
            isLoadedCell.Value = row.IsLoaded;

            DataGridViewCell createdCell = new DataGridViewTextBoxCell();
            createdCell.Value = row.CreatedTimestamp;
            DataGridViewCell modifiedCell = new DataGridViewTextBoxCell();
            modifiedCell.Value = row.ModifiedTimestamp;

            DataGridViewCell thumbnailCell = new DataGridViewImageCell();
            Image preview = GetPreviewImage(row.ThumbnailBytes);
            thumbnailCell.Value = preview;

            viewRow.Cells.Add(indexCell);
            viewRow.Cells.Add(uriCell);
            viewRow.Cells.Add(genreCell);
            viewRow.Cells.Add(scoreCell);
            viewRow.Cells.Add(commentCell);
            viewRow.Cells.Add(isAvailableCell);
            viewRow.Cells.Add(isLoadedCell);
            viewRow.Cells.Add(createdCell);
            viewRow.Cells.Add(modifiedCell);
            viewRow.Cells.Add(thumbnailCell);

            DecorateRow(viewRow);
        }

        private Image GetPreviewImage(byte[] sourceImageData)
        {
            Image sourceImage = ImageUtils.ImageFromBytes(sourceImageData);
            Image preview = ImageUtils.ResizeImage(sourceImage, 160, 90);
            return preview;
        }

        private void UpdateViewRow(DataGridViewRow viewRow)
        {
            _ignoreFocusModeChanges = true;
            //int repositoryNumber = GetRowRepositoryNumber(viewRow);
            var rowData = GetRowData(viewRow);

            viewRow.Cells[RepositoryConstants.IndexColumnIndex ].Value = rowData.LinkIndex;
            viewRow.Cells[RepositoryConstants.UriColumnIndex ].Value = rowData.Uri;
            viewRow.Cells[RepositoryConstants.GenreColumnIndex ].Value = rowData.Genre;
            viewRow.Cells[RepositoryConstants.ScoreColumnIndex ].Value = rowData.Score;
            viewRow.Cells[RepositoryConstants.CommentColumnIndex ].Value = rowData.Comment;
            viewRow.Cells[RepositoryConstants.IsAvailableColumnIndex ].Value = rowData.IsAvailable;
            viewRow.Cells[RepositoryConstants.IsLoadedColumnIndex ].Value = rowData.IsLoaded;
            viewRow.Cells[RepositoryConstants.CreatedColumnIndex ].Value = rowData.CreatedTimestamp;
            viewRow.Cells[RepositoryConstants.ModifiedColumnIndex ].Value = rowData.ModifiedTimestamp;
            viewRow.Cells[RepositoryConstants.ThumbnailColumnIndex ].Value = GetPreviewImage(rowData.ThumbnailBytes);
            DecorateRow(viewRow);
            if (LinkTableView.SelectedRows.Contains(viewRow))
            {
                EnterFocusMode(viewRow);
            }

            _ignoreFocusModeChanges = false;
        }

        private void UpdateDataView()
        {
            LinkTableView.Rows.Clear();
            //for (var index = 0; index < _repository.Count; index++)
            foreach (var row in _repository)
            {
                DataGridViewRow viewRow = new DataGridViewRow();
                CreateViewRow(viewRow, row);
                LinkTableView.Rows.Add(viewRow);
            }
        }

        private void DeleteRow(DataGridViewRow viewRow)
        {
            int rowNumber = (int)viewRow.Cells[0].Value;
            _repository.RemoveAt(rowNumber);
            LinkTableView.Rows.Remove(viewRow);
        }

        private void DeleteRows(IEnumerable<DataGridViewRow> viewRows)
        {
            foreach (var viewRow in viewRows)
            {
                DeleteRow(viewRow);
            }
            UpdateDataView();
        }

        private void DeleteSelectedRows()
        {
            foreach (DataGridViewRow row in LinkTableView.SelectedRows)
            {
                DeleteRow(row);
            }
            //UpdateDataView();
        }

        private ILinkTableRow GetRowData(DataGridViewRow viewRow)
        {
            int rowNumber = (int)viewRow.Cells[0].Value;
            var row = _repository[rowNumber];
            return row;
        }

        private void EnterFocusMode(DataGridViewRow viewRow)
        {
            _ignoreFocusModeChanges = true;
            UriBox.Enabled = true;
            GenreBox.Enabled = true;
            CommentBox.Enabled = true;
            IsAvailableBox.Enabled = true;
            IsLoadedBox.Enabled = true;
            SaveChangesButton.Enabled = true;
            ScoreBox.Enabled = true;
            ThumbnailBox.Enabled = true;

            var row = GetRowData(viewRow);

            UriBox.Text = row.Uri;
            GenreBox.Text = row.Genre;
            CommentBox.Text = row.Comment;
            IsAvailableBox.Checked = row.IsAvailable;
            IsLoadedBox.Checked = row.IsLoaded;
            ScoreBox.Value = row.Score;
            ThumbnailBox.Image = ImageUtils.ImageFromBytes(row.ThumbnailBytes);
            _ignoreFocusModeChanges = false;
        }
    

        private void ExitFocusMode()
        {
            _ignoreFocusModeChanges = true;

            UriBox.Enabled = false;
            GenreBox.Enabled = false;
            CommentBox.Enabled = false;
            IsAvailableBox.Enabled = false;
            IsLoadedBox.Enabled = false;
            SaveChangesButton.Enabled = false;
            ScoreBox.Enabled = false;
            ThumbnailBox.Image = null;
            
            UriBox.Clear();
            GenreBox.Clear();
            CommentBox.Clear();
            ScoreBox.Value = 0;
            IsAvailableBox.Checked = false;
            IsLoadedBox.Checked = false;
            ThumbnailBox.Enabled = false;

            _ignoreFocusModeChanges = false;
        }

        private void LinkTableView_SelectionChanged(object sender, EventArgs e)
        {
            if (LinkTableView.SelectedRows.Count == 1)
            {
                EnterFocusMode(LinkTableView.SelectedRows[0]);
            }
            else if(LinkTableView.SelectedRows.Count == 0)
            {
                ExitFocusMode();
            }
            else
            {
                ExitFocusMode();
            }
        }

        private void ResizeTableView()
        {
            int width = Width - LinkTableView.Location.X - UriBox.Width - 50;
            int height = Height - LinkTableView.Location.Y - 50;

            LinkTableView.Width = width;
            LinkTableView.Height = height;
        }

        private void RepositoryViewerForm_ResizeEnd(object sender, EventArgs e)
        {
            ResizeTableView();
        }

        private void DeleteSelectedButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedRows();
        }


        private void RepositoryViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_repository != null && _repository.HasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show("Save changes?", "Exiting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        e.Cancel = false;
                        _repository.Save();
                        break;
                    case DialogResult.No:
                        e.Cancel = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void NewRowButton_Click(object sender, EventArgs e)
        {
            var row = _repository.CreateLinkTableRow();

            DataGridViewRow viewRow = new DataGridViewRow();
            CreateViewRow(viewRow, row);
            LinkTableView.Rows.Add(viewRow);

            LinkTableView.ClearSelection();
            viewRow.Selected = true;
            LinkTableView.FirstDisplayedScrollingRowIndex = LinkTableView.SelectedRows[0].Index;
        }

        private void SaveChangesToRow(DataGridViewRow viewRow)
        {
            int rowNumber = (int)viewRow.Cells[0].Value;
            var row = _repository[rowNumber];

            row.Uri = UriBox.Text;
            row.Genre = GenreBox.Text;
            row.Comment = CommentBox.Text;
            row.IsAvailable = IsAvailableBox.Checked;
            row.IsLoaded = IsLoadedBox.Checked;
            row.Score = (int)ScoreBox.Value;
            row.ModifiedTimestamp = DateTime.Now;
            row.ThumbnailBytes = ImageUtils.ImageToBytes(ThumbnailBox.Image);
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            if (LinkTableView.SelectedRows.Count != 1)
            {
                return;
            }
            SaveChangesToRow(LinkTableView.SelectedRows[0]);
            _repository.Save();
            UpdateViewRow(LinkTableView.SelectedRows[0]);
            //UpdateDataView();
        }

        private void UriBox_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(UriBox.Text);
        }

        private void GenerateLinkTableHeadersMenuStrip()
        {
            LinkTableHeadersMenuStrip.Items.Clear();
            foreach (DataGridViewColumn column in LinkTableView.Columns)
            {
                var columnName = column.Name;
                var columnText = column.HeaderText;
                ToolStripMenuItem item = new ToolStripMenuItem(columnText, null, LinkTableHeadersMenuItemClicked);
                item.Name = columnName;
                item.Checked = column.Visible;
                item.CheckOnClick = true;
                LinkTableHeadersMenuStrip.Items.Add(item);
            }
        }

        private void LinkTableHeadersMenuItemClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            string columnName = item.Name;
            DataGridViewColumn column = LinkTableView.Columns[columnName];
            if (column == null)
            {
                return;
            }
            column.Visible = item.Checked;
        }

        private void LinkTableView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            var pos = LinkTableView.PointToScreen(
                LinkTableView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
            pos.X += e.X;
            pos.Y += e.Y;

            GenerateLinkTableHeadersMenuStrip();
            LinkTableHeadersMenuStrip.Show(pos);
        }

        private void LinkTableView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            DataGridViewRow viewRow = LinkTableView.Rows[e.RowIndex];
            ILinkTableRow rowData = GetRowData(viewRow);
            int rowNumber = GetRowRepositoryNumber(viewRow);
            DataGridViewCell cell = viewRow.Cells[e.ColumnIndex];
            if (e.ColumnIndex == RepositoryConstants.IsAvailableColumnIndex )
            {
                var checkboxCell = (DataGridViewCheckBoxCell)cell;
                bool value = !(bool)checkboxCell.Value;
                rowData.IsAvailable = value;
                UpdateViewRow(viewRow);
                _repository.Save();
            }
            else if (e.ColumnIndex == RepositoryConstants.IsLoadedColumnIndex )
            {
                var checkboxCell = (DataGridViewCheckBoxCell)cell;
                bool value = !(bool)checkboxCell.Value;
                rowData.IsLoaded = value;
                UpdateViewRow(viewRow);
                _repository.Save();
            }
        }

        private void ThumbnailBox_DoubleClick(object sender, EventArgs e)
        {
            if (LinkTableView.SelectedRows.Count != 1)
            {
                return;
            }
            if (Clipboard.ContainsImage())
            {
                Image image = Clipboard.GetImage();               
                if (image.Height > _preferences.ThumbnailMaxHeight)
                {
                    double aspectRatio = (double)image.Width / image.Height;
                    int targetWidth = (int)(_preferences.ThumbnailMaxHeight * aspectRatio);
                    image = ImageUtils.ResizeImage(image, targetWidth, _preferences.ThumbnailMaxHeight);
                }
                
                ThumbnailBox.Image = image;
                SaveChangesToRow(LinkTableView.SelectedRows[0]);
                UpdateViewRow(LinkTableView.SelectedRows[0]);
            }
        }

        private void RepositoryViewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SavePreferences();
        }

        private void FocusModeValuesChanged(object sender, EventArgs args)
        {
            if (LinkTableView.SelectedRows.Count != 1)
            {
                return;
            }
            if (_ignoreFocusModeChanges)
            {
                return;
            }
            SaveChangesToRow(LinkTableView.SelectedRows[0]);
            UpdateViewRow(LinkTableView.SelectedRows[0]);
        }

        private void saveChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _repository.Save();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_repository.HasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show("Save changes?", "Exiting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        _repository.Save();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _repository.Load();
            ExitFocusMode();
            UpdateDataView();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PreferencesChangedEvent(PreferencesContainer preferencesContainer)
        {
            LoadTableFontSizeFromPreferences();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTableFontSizeToPreferences();
            PreferencesEditorForm preferencesEditor = new PreferencesEditorForm(_preferences);
            _preferences.PreferencesChangedEvent += PreferencesChangedEvent;
            preferencesEditor.ShowDialog();
            _preferences.PreferencesChangedEvent -= PreferencesChangedEvent;
        }

        private void LinkTableView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            DataGridViewRow viewRow = LinkTableView.Rows[e.RowIndex];
            DataGridViewCell cell = viewRow.Cells[e.ColumnIndex];
            if (e.ColumnIndex == RepositoryConstants.UriColumnIndex)
            {
                var uriCell = (DataGridViewTextBoxCell)cell;
                string uri = (String)uriCell.Value;
                Clipboard.SetText(uri);
            }
        }

        private void RepositoryViewerForm_Load(object sender, EventArgs e)
        {
            if (_repository == null && _repositoryProvider != null)
            {
                _repositoryProvider.RequestRepository();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_repositoryProvider == null)
            {
                MessageBox.Show("Repository provider is unavailable", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            _repositoryProvider.RequestRepository();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_repositoryProvider == null)
            {
                MessageBox.Show("Repository provider is unavailable", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            _repositoryProvider.RequestRepository();
        }

        private void shrinkAllImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in LinkTableView.Rows)
            {
                int rowNumber = (int)row.Cells[0].Value;
                DataGridViewImageCell imageCell = (DataGridViewImageCell)row.Cells["LinkTableThumbnailColumn"];
                               
                ILinkTableRow linkTableRow = _repository[rowNumber];
                if (linkTableRow.ThumbnailBytes == null)
                {
                    continue;
                }
                Image image = ImageUtils.ImageFromBytes(linkTableRow.ThumbnailBytes);
                if (image == null)
                {
                    continue;
                }

                if (image.Height > _preferences.ThumbnailMaxHeight)
                {
                    double aspectRatio = (double)image.Width / image.Height;
                    int targetWidth = (int)(_preferences.ThumbnailMaxHeight * aspectRatio);
                    //image = ImageUtils.ShrinkImage(image, new Size(targetWidth, _preferences.ThumbnailMaxHeight));
                    image = ImageUtils.ResizeImage(image, targetWidth, _preferences.ThumbnailMaxHeight);
                    imageCell.Value = image;
                    linkTableRow.ThumbnailBytes = ImageUtils.ImageToBytes(image);
                }
            }

            _repository.Save();
            UpdateDataView();
        }
    }
}
