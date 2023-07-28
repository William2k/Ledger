using Data.Models;

namespace Data.Interfaces;

public interface IRepository
{
    Account AddOrUpdateAccount(Account account);
    Payment AddOrUpdatePayment(Payment payment);
    bool DeletePayment(Guid paymentId);
    Account GetAccount(Guid accountId);
    Account GetAccountByOwner(string owner);
    Payment GetPayment(Guid paymentId);
    IEnumerable<Payment> GetPaymentsByDates(DateTime startDate, DateTime endDate);
}