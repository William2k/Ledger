using Data.Interfaces;
using Data.Models;
using System.Data;

namespace Data;

public class Repository : IRepository
{
    private readonly Dictionary<Guid, Account> Accounts = new();

    private readonly Dictionary<Guid, Payment> Payments = new();

    public Account AddOrUpdateAccount(Account account)
    {
        if (!account.Id.HasValue)
        {
            account.Created = DateTime.Now;
            account.IsActive = true;

            do
            {
                account.Id = Guid.NewGuid();
            } while (Accounts.ContainsKey(account.Id.Value));
        }

        account.Modified = DateTime.Now;
        Accounts[account.Id.Value] = account;

        return account;
    }

    public Account GetAccount(Guid accountId) => Accounts[accountId];

    public Account GetAccountByOwner(string owner)
    {
        return Accounts.FirstOrDefault(x => x.Value.Owner == owner).Value;
    }

    public Payment AddOrUpdatePayment(Payment payment)
    {
        payment.Date = DateTime.Now;

        if (!payment.Id.HasValue)
        {
            payment.IsActive = true;

            do
            {
                payment.Id = Guid.NewGuid();
            } while (Payments.ContainsKey(payment.Id.Value));
        }

        Payments[payment.Id.Value] = payment;

        return payment;
    }

    public Payment GetPayment(Guid paymentId) => Payments[paymentId];

    public bool DeletePayment(Guid paymentId) => Payments.Remove(paymentId);

    public IEnumerable<Payment> GetPaymentsByDates(DateTime startDate, DateTime endDate) => 
        Payments.Select(s => s.Value).Where(x => x.Date >= startDate && x.Date <= endDate);
}