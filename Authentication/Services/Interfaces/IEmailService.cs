using Authentication.Entities;

namespace Authentication.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
}
