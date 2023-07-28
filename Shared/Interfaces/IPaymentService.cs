using Data.Models;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Account AddOrUpdateAccount(Account account);
        Account MakePayment(Account account, Payment payment);
    }
}