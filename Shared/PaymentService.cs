using Core.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Core;

public class PaymentService : IPaymentService
{
    private readonly IRepository _repository;

    public PaymentService(IRepository repository)
    {
        _repository = repository;
    }

    public Account AddOrUpdateAccount(Account account) => _repository.AddOrUpdateAccount(account);

    public Account MakePayment(Account account, Payment payment)
    {
        ValidateAccountInfo(account);

        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        account.Balance -= payment.Amount;

        if (account.Balance < 0)
            throw new Exception("Account doesn't have enough funds for payment");

        try
        {
            payment = _repository.AddOrUpdatePayment(payment);
            account = _repository.AddOrUpdateAccount(account);
        }
        catch (Exception)
        {
            if (payment.Id.HasValue)
                _repository.DeletePayment(payment.Id.Value);

            throw new Exception("Payment could not be processed");
        }

        return account;
    }

    private void ValidateAccountInfo(Account account)
    {
        if (account == null || !account.Id.HasValue)
            throw new ArgumentNullException(nameof(account));

        var dbAccount = _repository.GetAccount(account.Id.Value);

        if (dbAccount == null || !dbAccount.IsActive || dbAccount.Modified != account.Modified)
            throw new Exception("Payment cannot be processed");
    }
}
