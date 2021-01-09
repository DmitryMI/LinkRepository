using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkRepository.Repository.SqliteEf
{
    [Table("LinkTable")]
    public class LinkTable : ILinkTableRow
    {
        //[Column("LinkIndex")]
        [Key]
        public int LinkIndex { get; set; }

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

        [Column("CreatedTimestamp", Order = RepositoryConstants.CreatedColumnIndex, TypeName = "INTEGER")]
        public long CreatedTimestampShadow { get; set; }

        [Column("ModifiedTimestamp", Order = RepositoryConstants.ModifiedColumnIndex, TypeName = "INTEGER")]
        public long ModifiedTimestampShadow { get; set; }

        [Column("ThumbnailBytes")]
        public byte[] ThumbnailBytes { get; set; }


        public DateTime CreatedTimestamp
        {
            get => DateTime.FromBinary(CreatedTimestampShadow);
            set => CreatedTimestampShadow = value.ToBinary();
        }

        public DateTime ModifiedTimestamp
        {
            get => DateTime.FromBinary(ModifiedTimestampShadow);
            set => ModifiedTimestampShadow = value.ToBinary();
        }

    }
}
