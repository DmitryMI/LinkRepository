using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LinkRepository.Preferences;
using LinkRepository.Repository;
using LinkRepository.Utils;

namespace LinkRepository
{
    public partial class RepositoryViewerForm : Form
    {
        private readonly IRepository _repository;
        private PreferencesContainer _preferences;
        public RepositoryViewerForm(IRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            _repository.Load();
            ExitFocusMode();
            UpdateDataView();
            LoadPreferences();
            ResizeTableView();
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

        private void SaveColumnsToPreferences()
        {
            _preferences.VisibleColumns.Clear();
            foreach (DataGridViewColumn column in LinkTableView.Columns)
            {
                if (column.Visible)
                {
                    _preferences.VisibleColumns.Add(column.Name);
                }
            }
        }

        private void LoadPreferences()
        {
            _preferences = new PreferencesContainer("LinkRepository.xml");
            if (_preferences.Deserialize())
            {
                LoadColumnsFromPreferences();
            }
        }

        private void SavePreferences()
        {
            SaveColumnsToPreferences();
            _preferences.Serialize();
        }

        private int GetRowRepositoryNumber(DataGridViewRow viewRow)
        {
            return (int) viewRow.Cells[0].Value;
        }

        private void CreateViewRow(DataGridViewRow viewRow, int repositoryNumber)
        {
            viewRow.Cells.Clear();
            var row = _repository[repositoryNumber];
            DataGridViewCell rowNumberCell = new DataGridViewTextBoxCell();
            rowNumberCell.Value = repositoryNumber;
            DataGridViewCell indexCell = new DataGridViewTextBoxCell();
            indexCell.Value = row.Index;
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

            viewRow.Cells.Add(rowNumberCell);
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
        }

        private Image GetPreviewImage(byte[] sourceImageData)
        {
            Image sourceImage = ImageUtils.ImageFromBytes(sourceImageData);
            Image preview = ImageUtils.ResizeImage(sourceImage, 128, 128);
            return preview;
        }

        private void UpdateViewRow(DataGridViewRow viewRow)
        {
            int repositoryNumber = GetRowRepositoryNumber(viewRow);
            var rowData = GetRowData(viewRow);

            viewRow.Cells[0].Value = repositoryNumber;
            viewRow.Cells[RepositoryConstants.IndexColumnIndex + 1].Value = rowData.Index;
            viewRow.Cells[RepositoryConstants.UriColumnIndex + 1].Value = rowData.Uri;
            viewRow.Cells[RepositoryConstants.GenreColumnIndex + 1].Value = rowData.Genre;
            viewRow.Cells[RepositoryConstants.ScoreColumnIndex + 1].Value = rowData.Score;
            viewRow.Cells[RepositoryConstants.CommentColumnIndex + 1].Value = rowData.Comment;
            viewRow.Cells[RepositoryConstants.IsAvailableColumnIndex + 1].Value = rowData.IsAvailable;
            viewRow.Cells[RepositoryConstants.IsLoadedColumnIndex + 1].Value = rowData.IsLoaded;
            viewRow.Cells[RepositoryConstants.CreatedColumnIndex + 1].Value = rowData.CreatedTimestamp;
            viewRow.Cells[RepositoryConstants.ModifiedColumnIndex + 1].Value = rowData.ModifiedTimestamp;
            viewRow.Cells[RepositoryConstants.ThumbnailColumnIndex + 1].Value = ImageUtils.ImageFromBytes(rowData.ThumbnailBytes);

            if (LinkTableView.SelectedRows.Contains(viewRow))
            {
                EnterFocusMode(viewRow);
            }
        }

        private void UpdateDataView()
        {
            LinkTableView.Rows.Clear();
            for (var index = 0; index < _repository.Count; index++)
            {
                DataGridViewRow viewRow = new DataGridViewRow();
                CreateViewRow(viewRow, index);
                LinkTableView.Rows.Add(viewRow);
            }
        }

        private void DeleteRow(DataGridViewRow viewRow)
        {
            int rowNumber = (int)viewRow.Cells[0].Value;
            _repository.RemoveAt(rowNumber);
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
            UpdateDataView();
        }

        private LinkTableRow GetRowData(DataGridViewRow viewRow)
        {
            int rowNumber = (int)viewRow.Cells[0].Value;
            var row = _repository[rowNumber];
            return row;
        }

        private void EnterFocusMode(DataGridViewRow viewRow)
        {
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
            //ThumbnailBox.Image = (Image)viewRow.Cells[RepositoryConstants.ThumbnailColumnIndex + 1].Value;
            ThumbnailBox.Image = ImageUtils.ImageFromBytes(row.ThumbnailBytes);
        }
    

        private void ExitFocusMode()
        {
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
            if (_repository.HasUnsavedChanges)
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
            UpdateDataView();
            int repositoryIndex = row.Index;

            var viewRows =
                from DataGridViewRow tableRow in LinkTableView.Rows
                where (int)tableRow.Cells[1].Value == repositoryIndex
                select tableRow;

            var newViewRow = viewRows.First();
            LinkTableView.ClearSelection();
            newViewRow.Selected = true;
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
            LinkTableRow rowData = GetRowData(viewRow);
            int rowNumber = GetRowRepositoryNumber(viewRow);
            DataGridViewCell cell = viewRow.Cells[e.ColumnIndex];
            if (e.ColumnIndex == RepositoryConstants.IsAvailableColumnIndex + 1)
            {
                var checkboxCell = (DataGridViewCheckBoxCell)cell;
                bool value = !(bool)checkboxCell.Value;
                rowData.IsAvailable = value;
                UpdateViewRow(viewRow);
                _repository.Save();
            }
            else if (e.ColumnIndex == RepositoryConstants.IsLoadedColumnIndex + 1)
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
            if (Clipboard.ContainsImage())
            {
                ThumbnailBox.Image = Clipboard.GetImage();
            }
        }

        private void RepositoryViewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SavePreferences();
        }
    }
}
