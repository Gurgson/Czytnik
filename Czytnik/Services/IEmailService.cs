using System.Threading.Tasks;

namespace Czytnik.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }
}
