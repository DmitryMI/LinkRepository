using System;
using System.Collections.Generic;
using LinkRepository.Repository.Sqlite;

namespace LinkRepository.Repository
{
    public interface IRepository : IList<ILinkTableRow>, IDisposable
    {
        void Load();
        void Save();
        bool HasUnsavedChanges { get; }
        ILinkTableRow CreateLinkTableRow();
    }
}
