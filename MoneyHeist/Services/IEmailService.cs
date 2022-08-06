namespace MoneyHeist.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html);
    }
}
