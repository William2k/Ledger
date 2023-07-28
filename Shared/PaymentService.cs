using Core.Interfaces;
using Data;
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

        var newAccountDetails = GetCopy(account);
        newAccountDetails.Balance = CalculateBalance(payment.Direction, account.Balance, payment.Amount);

        if (newAccountDetails.Balance < 0)
            throw new Exception("Account doesn't have enough funds for payment");

        try
        {
            payment = _repository.AddOrUpdatePayment(payment);
            account = _repository.AddOrUpdateAccount(newAccountDetails);
        }
        catch (Exception)
        {
            if (payment.Id.HasValue)
                _repository.DeletePayment(payment.Id.Value);

            throw new Exception("Payment could not be processed");
        }

        return account;
    }

    private decimal CalculateBalance(Direction direction, decimal currentBalance, decimal amount)
    {
        switch (direction)
        {
            case Direction.Outbound:
                return currentBalance -= amount;
            case Direction.Inbound:
                return currentBalance += amount;
            default:
                throw new ArgumentException($"Invalid direction: {direction}");
        }
    }

    private void ValidateAccountInfo(Account account)
    {
        if (account == null || !account.Id.HasValue)
            throw new ArgumentNullException(nameof(account));

        var dbAccount = _repository.GetAccount(account.Id.Value);

        if (dbAccount == null || !dbAccount.IsActive || dbAccount.Modified != account.Modified)
            throw new Exception("Payment cannot be processed");
    }

    private Account GetCopy(Account account) => new()
    {
        Id = account.Id,
        Balance = account.Balance,
        Created = account.Created,
        IsActive = account.IsActive,
        Modified = account.Modified,
        Owner = account.Owner,
    };
}
