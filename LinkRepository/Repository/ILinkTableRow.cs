using System;

namespace LinkRepository.Repository
{
    public interface ILinkTableRow
    {
        DateTime CreatedTimestamp { get; }

        DateTime ModifiedTimestamp { get; set; }

        int Index { get; }

        string Uri { get; set; }

        string Genre { get; set; }

        int Score { get; set; }

        string Comment { get; set; }
        bool IsAvailable { get; set; }

        bool IsLoaded { get; set; }
        byte[] ThumbnailBytes { get; set; }
    }
}