﻿namespace LinkRepository
{
    partial class RepositoryViewerForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LinkTableView = new System.Windows.Forms.DataGridView();
            this.LinkTableIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableUriColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableGenreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableScoreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableCommentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableIsAvailableColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LinkTableIsLoadedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LinkTableCreatedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableModifiedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkTableThumbnailColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.NewRowButton = new System.Windows.Forms.Button();
            this.UriBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GenreBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CommentBox = new System.Windows.Forms.TextBox();
            this.IsAvailableBox = new System.Windows.Forms.CheckBox();
            this.IsLoadedBox = new System.Windows.Forms.CheckBox();
            this.DeleteSelectedButton = new System.Windows.Forms.Button();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            this.ScoreBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.LinkTableHeadersMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ThumbnailBox = new System.Windows.Forms.PictureBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.repositoryViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shrinkAllImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.LinkTableView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailBox)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // LinkTableView
            // 
            this.LinkTableView.AllowUserToAddRows = false;
            this.LinkTableView.AllowUserToOrderColumns = true;
            this.LinkTableView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.LinkTableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LinkTableView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LinkTableIndexColumn,
            this.LinkTableUriColumn,
            this.LinkTableGenreColumn,
            this.LinkTableScoreColumn,
            this.LinkTableCommentColumn,
            this.LinkTableIsAvailableColumn,
            this.LinkTableIsLoadedColumn,
            this.LinkTableCreatedColumn,
            this.LinkTableModifiedColumn,
            this.LinkTableThumbnailColumn});
            this.LinkTableView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.LinkTableView.Location = new System.Drawing.Point(12, 29);
            this.LinkTableView.Name = "LinkTableView";
            this.LinkTableView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LinkTableView.Size = new System.Drawing.Size(1075, 637);
            this.LinkTableView.TabIndex = 0;
            this.LinkTableView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LinkTableView_CellContentClick);
            this.LinkTableView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LinkTableView_CellDoubleClick);
            this.LinkTableView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.LinkTableView_ColumnHeaderMouseClick);
            this.LinkTableView.SelectionChanged += new System.EventHandler(this.LinkTableView_SelectionChanged);
            // 
            // LinkTableIndexColumn
            // 
            this.LinkTableIndexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.LinkTableIndexColumn.HeaderText = "Index";
            this.LinkTableIndexColumn.Name = "LinkTableIndexColumn";
            this.LinkTableIndexColumn.ReadOnly = true;
            this.LinkTableIndexColumn.Width = 58;
            // 
            // LinkTableUriColumn
            // 
            this.LinkTableUriColumn.FillWeight = 10F;
            this.LinkTableUriColumn.HeaderText = "URI";
            this.LinkTableUriColumn.MinimumWidth = 10;
            this.LinkTableUriColumn.Name = "LinkTableUriColumn";
            this.LinkTableUriColumn.Width = 50;
            // 
            // LinkTableGenreColumn
            // 
            this.LinkTableGenreColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LinkTableGenreColumn.FillWeight = 50F;
            this.LinkTableGenreColumn.HeaderText = "Genre";
            this.LinkTableGenreColumn.MinimumWidth = 50;
            this.LinkTableGenreColumn.Name = "LinkTableGenreColumn";
            this.LinkTableGenreColumn.Width = 61;
            // 
            // LinkTableScoreColumn
            // 
            this.LinkTableScoreColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LinkTableScoreColumn.HeaderText = "Score";
            this.LinkTableScoreColumn.Name = "LinkTableScoreColumn";
            this.LinkTableScoreColumn.Width = 60;
            // 
            // LinkTableCommentColumn
            // 
            this.LinkTableCommentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LinkTableCommentColumn.FillWeight = 50F;
            this.LinkTableCommentColumn.HeaderText = "Comment";
            this.LinkTableCommentColumn.Name = "LinkTableCommentColumn";
            // 
            // LinkTableIsAvailableColumn
            // 
            this.LinkTableIsAvailableColumn.HeaderText = "Is available";
            this.LinkTableIsAvailableColumn.Name = "LinkTableIsAvailableColumn";
            // 
            // LinkTableIsLoadedColumn
            // 
            this.LinkTableIsLoadedColumn.HeaderText = "Is loaded";
            this.LinkTableIsLoadedColumn.Name = "LinkTableIsLoadedColumn";
            // 
            // LinkTableCreatedColumn
            // 
            this.LinkTableCreatedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LinkTableCreatedColumn.HeaderText = "Created";
            this.LinkTableCreatedColumn.Name = "LinkTableCreatedColumn";
            this.LinkTableCreatedColumn.Width = 69;
            // 
            // LinkTableModifiedColumn
            // 
            this.LinkTableModifiedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LinkTableModifiedColumn.HeaderText = "Modified";
            this.LinkTableModifiedColumn.Name = "LinkTableModifiedColumn";
            this.LinkTableModifiedColumn.Width = 72;
            // 
            // LinkTableThumbnailColumn
            // 
            this.LinkTableThumbnailColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LinkTableThumbnailColumn.HeaderText = "Thumbnail";
            this.LinkTableThumbnailColumn.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.LinkTableThumbnailColumn.Name = "LinkTableThumbnailColumn";
            this.LinkTableThumbnailColumn.Width = 62;
            // 
            // NewRowButton
            // 
            this.NewRowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewRowButton.Location = new System.Drawing.Point(1093, 697);
            this.NewRowButton.Name = "NewRowButton";
            this.NewRowButton.Size = new System.Drawing.Size(281, 23);
            this.NewRowButton.TabIndex = 1;
            this.NewRowButton.Text = "Add Row";
            this.NewRowButton.UseVisualStyleBackColor = true;
            this.NewRowButton.Click += new System.EventHandler(this.NewRowButton_Click);
            // 
            // UriBox
            // 
            this.UriBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UriBox.Location = new System.Drawing.Point(1093, 29);
            this.UriBox.Multiline = true;
            this.UriBox.Name = "UriBox";
            this.UriBox.Size = new System.Drawing.Size(281, 60);
            this.UriBox.TabIndex = 2;
            this.UriBox.TextChanged += new System.EventHandler(this.FocusModeValuesChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1093, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "URI:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1093, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Genre:";
            // 
            // GenreBox
            // 
            this.GenreBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenreBox.Location = new System.Drawing.Point(1093, 108);
            this.GenreBox.Name = "GenreBox";
            this.GenreBox.Size = new System.Drawing.Size(281, 20);
            this.GenreBox.TabIndex = 4;
            this.GenreBox.TextChanged += new System.EventHandler(this.FocusModeValuesChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1093, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Comment:";
            // 
            // CommentBox
            // 
            this.CommentBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentBox.Location = new System.Drawing.Point(1093, 151);
            this.CommentBox.Multiline = true;
            this.CommentBox.Name = "CommentBox";
            this.CommentBox.Size = new System.Drawing.Size(281, 60);
            this.CommentBox.TabIndex = 6;
            this.CommentBox.TextChanged += new System.EventHandler(this.FocusModeValuesChanged);
            // 
            // IsAvailableBox
            // 
            this.IsAvailableBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IsAvailableBox.AutoSize = true;
            this.IsAvailableBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IsAvailableBox.Location = new System.Drawing.Point(1096, 275);
            this.IsAvailableBox.Name = "IsAvailableBox";
            this.IsAvailableBox.Size = new System.Drawing.Size(97, 21);
            this.IsAvailableBox.TabIndex = 8;
            this.IsAvailableBox.Text = "Is available";
            this.IsAvailableBox.UseVisualStyleBackColor = true;
            this.IsAvailableBox.CheckedChanged += new System.EventHandler(this.FocusModeValuesChanged);
            // 
            // IsLoadedBox
            // 
            this.IsLoadedBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IsLoadedBox.AutoSize = true;
            this.IsLoadedBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IsLoadedBox.Location = new System.Drawing.Point(1199, 275);
            this.IsLoadedBox.Name = "IsLoadedBox";
            this.IsLoadedBox.Size = new System.Drawing.Size(84, 21);
            this.IsLoadedBox.TabIndex = 9;
            this.IsLoadedBox.Text = "Is loaded";
            this.IsLoadedBox.UseVisualStyleBackColor = true;
            this.IsLoadedBox.CheckedChanged += new System.EventHandler(this.FocusModeValuesChanged);
            // 
            // DeleteSelectedButton
            // 
            this.DeleteSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteSelectedButton.Location = new System.Drawing.Point(1093, 726);
            this.DeleteSelectedButton.Name = "DeleteSelectedButton";
            this.DeleteSelectedButton.Size = new System.Drawing.Size(281, 23);
            this.DeleteSelectedButton.TabIndex = 10;
            this.DeleteSelectedButton.Text = "Delete selected";
            this.DeleteSelectedButton.UseVisualStyleBackColor = true;
            this.DeleteSelectedButton.Click += new System.EventHandler(this.DeleteSelectedButton_Click);
            // 
            // SaveChangesButton
            // 
            this.SaveChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveChangesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SaveChangesButton.Location = new System.Drawing.Point(1093, 585);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(281, 23);
            this.SaveChangesButton.TabIndex = 11;
            this.SaveChangesButton.Text = "Save Row";
            this.SaveChangesButton.UseVisualStyleBackColor = true;
            this.SaveChangesButton.Click += new System.EventHandler(this.SaveChangesButton_Click);
            // 
            // ScoreBox
            // 
            this.ScoreBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScoreBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ScoreBox.Location = new System.Drawing.Point(1093, 235);
            this.ScoreBox.Name = "ScoreBox";
            this.ScoreBox.Size = new System.Drawing.Size(120, 26);
            this.ScoreBox.TabIndex = 12;
            this.ScoreBox.ValueChanged += new System.EventHandler(this.FocusModeValuesChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1094, 219);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Score:";
            // 
            // LinkTableHeadersMenuStrip
            // 
            this.LinkTableHeadersMenuStrip.Name = "LinkTableHeadersMenuStrip";
            this.LinkTableHeadersMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.LinkTableHeadersMenuStrip.Text = "Columns";
            // 
            // ThumbnailBox
            // 
            this.ThumbnailBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ThumbnailBox.BackColor = System.Drawing.Color.White;
            this.ThumbnailBox.Location = new System.Drawing.Point(1093, 302);
            this.ThumbnailBox.Name = "ThumbnailBox";
            this.ThumbnailBox.Size = new System.Drawing.Size(275, 275);
            this.ThumbnailBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ThumbnailBox.TabIndex = 14;
            this.ThumbnailBox.TabStop = false;
            this.ThumbnailBox.DoubleClick += new System.EventHandler(this.ThumbnailBox_DoubleClick);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.repositoryViewerToolStripMenuItem,
            this.operationsToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1384, 24);
            this.MenuStrip.TabIndex = 15;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // repositoryViewerToolStripMenuItem
            // 
            this.repositoryViewerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.saveChangesToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.repositoryViewerToolStripMenuItem.Name = "repositoryViewerToolStripMenuItem";
            this.repositoryViewerToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.repositoryViewerToolStripMenuItem.Text = "Repository viewer";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // saveChangesToolStripMenuItem
            // 
            this.saveChangesToolStripMenuItem.Name = "saveChangesToolStripMenuItem";
            this.saveChangesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveChangesToolStripMenuItem.Text = "Save changes";
            this.saveChangesToolStripMenuItem.Click += new System.EventHandler(this.saveChangesToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // operationsToolStripMenuItem
            // 
            this.operationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shrinkAllImagesToolStripMenuItem});
            this.operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
            this.operationsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.operationsToolStripMenuItem.Text = "Operations";
            // 
            // shrinkAllImagesToolStripMenuItem
            // 
            this.shrinkAllImagesToolStripMenuItem.Name = "shrinkAllImagesToolStripMenuItem";
            this.shrinkAllImagesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.shrinkAllImagesToolStripMenuItem.Text = "Shrink All Images";
            this.shrinkAllImagesToolStripMenuItem.Click += new System.EventHandler(this.shrinkAllImagesToolStripMenuItem_Click);
            // 
            // RepositoryViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 761);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.ThumbnailBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ScoreBox);
            this.Controls.Add(this.SaveChangesButton);
            this.Controls.Add(this.DeleteSelectedButton);
            this.Controls.Add(this.IsLoadedBox);
            this.Controls.Add(this.IsAvailableBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CommentBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GenreBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UriBox);
            this.Controls.Add(this.NewRowButton);
            this.Controls.Add(this.LinkTableView);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "RepositoryViewerForm";
            this.Text = "Repository viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RepositoryViewerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RepositoryViewerForm_FormClosed);
            this.Load += new System.EventHandler(this.RepositoryViewerForm_Load);
            this.ResizeEnd += new System.EventHandler(this.RepositoryViewerForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.LinkTableView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailBox)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView LinkTableView;
        private System.Windows.Forms.Button NewRowButton;
        private System.Windows.Forms.TextBox UriBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox GenreBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CommentBox;
        private System.Windows.Forms.CheckBox IsAvailableBox;
        private System.Windows.Forms.CheckBox IsLoadedBox;
        private System.Windows.Forms.Button DeleteSelectedButton;
        private System.Windows.Forms.Button SaveChangesButton;
        private System.Windows.Forms.NumericUpDown ScoreBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip LinkTableHeadersMenuStrip;
        private System.Windows.Forms.PictureBox ThumbnailBox;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem repositoryViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveChangesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableUriColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableGenreColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableScoreColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableCommentColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LinkTableIsAvailableColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LinkTableIsLoadedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableCreatedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkTableModifiedColumn;
        private System.Windows.Forms.DataGridViewImageColumn LinkTableThumbnailColumn;
        private System.Windows.Forms.ToolStripMenuItem operationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shrinkAllImagesToolStripMenuItem;
    }
}

