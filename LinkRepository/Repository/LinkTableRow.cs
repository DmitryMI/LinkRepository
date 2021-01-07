using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkRepository.Repository
{
    public class LinkTableRow
    {
        private bool _isModified = false;
        private bool _isNewRow = false;
        private int _index;
        private IModificationReporter _modificationReporter;
        private string _uri;
        private string _genre;
        private int _score;
        private string _comment;
        private bool _isAvailable;
        private bool _isLoaded;
        private byte[] _thumbnailBytes;

        internal LinkTableRow(int index, bool isNewRow, IModificationReporter modificationReporter = null)
        {
            _index = index;
            _isNewRow = isNewRow;
            _modificationReporter = modificationReporter;
        }

        internal void ResetModifiedFlag()
        {
            _isModified = false;
        }

        internal void ResetNewRowFlag()
        {
            _isNewRow = false;
        }

        public bool IsNewRow => _isNewRow;

        public bool IsModified => _isModified;

        public int Index
        {
            get => _index;
        }

        public string Uri
        {
            get => _uri;
            set { _uri = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }

        public int Score
        {
            get => _score;
            set { _score = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }

        public string Comment
        {
            get => _comment;
            set { _comment = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set { _isLoaded = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }

        public byte[] ThumbnailBytes
        {
            get => _thumbnailBytes;
            set { _thumbnailBytes = value; _isModified = true; _modificationReporter?.ReportModification(this); }
        }
    }
}
