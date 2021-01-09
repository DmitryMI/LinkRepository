namespace LinkRepository.Repository.Sqlite
{
    public interface IModificationReporter
    {
        void ReportModification(object sender);
    }
}