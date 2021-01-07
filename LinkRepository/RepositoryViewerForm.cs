using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkRepository.Repository;

namespace LinkRepository
{
    public partial class RepositoryViewerForm : Form
    {
        private IRepository _repository;
        public RepositoryViewerForm(IRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            _repository.Load();
            ExitFocusMode();
            UpdateDataView();
            ResizeTableView();
        }

        private void UpdateDataView()
        {
            LinkTableView.Rows.Clear();
            for (var index = 0; index < _repository.Count; index++)
            {
                var row = _repository[index];
                DataGridViewRow viewRow = new DataGridViewRow();
                DataGridViewCell rowNumberCell = new DataGridViewTextBoxCell();
                rowNumberCell.Value = index;
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
                DataGridViewCell thumbnailCell = new DataGridViewImageCell();
                // TODO Set image
                viewRow.Cells.Add(rowNumberCell);
                viewRow.Cells.Add(indexCell);
                viewRow.Cells.Add(uriCell);
                viewRow.Cells.Add(genreCell);
                viewRow.Cells.Add(scoreCell);
                viewRow.Cells.Add(commentCell);
                viewRow.Cells.Add(isAvailableCell);
                viewRow.Cells.Add(isLoadedCell);
                viewRow.Cells.Add(thumbnailCell);

                LinkTableView.Rows.Add(viewRow);
            }
        }

        private void DeleteRow(DataGridViewRow viewRow)
        {
            //LinkTableView.Rows.Remove(viewRow);
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

        private void EnterFocusMode(DataGridViewRow viewRow)
        {
            UriBox.Enabled = true;
            GenreBox.Enabled = true;
            CommentBox.Enabled = true;
            IsAvailableBox.Enabled = true;
            IsLoadedBox.Enabled = true;
            SaveChangesButton.Enabled = true;
            ScoreBox.Enabled = true;

            int rowNumber = (int)viewRow.Cells[0].Value;
            var row = _repository[rowNumber];

            UriBox.Text = row.Uri;
            GenreBox.Text = row.Genre;
            CommentBox.Text = row.Comment;
            IsAvailableBox.Checked = row.IsAvailable;
            IsLoadedBox.Checked = row.IsLoaded;
            ScoreBox.Value = row.Score;
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
            
            UriBox.Clear();
            GenreBox.Clear();
            CommentBox.Clear();
            ScoreBox.Value = 0;
            IsAvailableBox.Checked = false;
            IsLoadedBox.Checked = false;
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
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            if (LinkTableView.SelectedRows.Count != 1)
            {
                return;
            }
            SaveChangesToRow(LinkTableView.SelectedRows[0]);
            _repository.Save();
            UpdateDataView();
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
    }
}
