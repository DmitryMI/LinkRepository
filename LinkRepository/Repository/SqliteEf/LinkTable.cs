using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkRepository.Repository.SqliteEf
{
    [Table("LinkTable")]
    public class LinkTable : ILinkTableRow
    {
        [Column("CreatedTimestamp")]
        public DateTime CreatedTimestamp { get; }

        [Column("ModifiedTimestamp")]
        public DateTime ModifiedTimestamp { get; set; }

        [Column("Index")]
        [Key]
        public int Index { get; }

        [Column("Uri")]
        public string Uri { get; set; }

        [Column("Genre")]
        public string Genre { get; set; }

        [Column("Score")]
        public int Score { get; set; }

        [Column("Comment")]
        public string Comment { get; set; }

        [Column("IsAvailable")]
        public bool IsAvailable { get; set; }

        [Column("IsLoaded")]
        public bool IsLoaded { get; set; }

        [Column("ThumbnailBytes")]
        public byte[] ThumbnailBytes { get; set; }
    }
}
