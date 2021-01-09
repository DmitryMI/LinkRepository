using System;

namespace LinkRepository
{
    public interface IPasswordProvider
    {
        event Action<IPasswordProvider, string> OnPasswordEnteredEvent;
        bool IsPasswordAvailable { get; }
        string Password { get; }
        void ReportCorrectPassword();
        void ReportWrongPassword();
    }
}