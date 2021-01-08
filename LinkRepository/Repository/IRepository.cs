using System;
using System.Collections.Generic;

namespace LinkRepository.Repository
{
    public interface IRepository : IList<LinkTableRow>, IDisposable
    {
        void Load();
        void Save();
        bool HasUnsavedChanges { get; }
        LinkTableRow CreateLinkTableRow();
    }
}
