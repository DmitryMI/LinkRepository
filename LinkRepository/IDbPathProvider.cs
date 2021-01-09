using System;
using LinkRepository.Repository;

namespace LinkRepository
{
    public interface IRepositoryProvider
    {
        IRepository Repository { get; }
        event Action<IRepositoryProvider, IRepository> OnRepositoryLoadedEvent;
        void RequestRepository();
        void ReportRepositoryProvided();
    }
}