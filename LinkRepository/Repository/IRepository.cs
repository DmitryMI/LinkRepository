using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
