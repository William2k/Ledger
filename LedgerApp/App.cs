using Core.Interfaces;
using Data;
using Data.Models;

namespace LedgerApp;

internal static class App
{
    private static Account? Account;
    private static IPaymentService _paymentService;

    private static void Run()
    {
        Console.WriteLine("Lerger started");
        Console.WriteLine("Enter name:");

        var name = Console.ReadLine();

        if (name == null)
            throw new ArgumentNullException(nameof(name));

        Account = _paymentService.AddOrUpdateAccount(new Account { Owner = name });

        Console.WriteLine($"You are now logged in as: {name}");

        AccountCheck();

        Console.WriteLine($"Closing balance is: £{Account.Balance}");
        Console.WriteLine("App closing");
    }

    internal static void Start(IPaymentService paymentService)
    {
        _paymentService = paymentService;

        Run();
    }

    private static void AccountCheck()
    {
        if(Account ==  null) 
            throw new ArgumentNullException(nameof(Account));

        Console.WriteLine($"Account owner: {Account.Owner}");
        Console.WriteLine($"Current balance is: £{Account.Balance}");
        Console.WriteLine($"Last modified: {Account.Modified}");

        Console.WriteLine("Do you want to add a payment? [Y or N]");
        var yn = Console.ReadLine()?.ToUpper().First();

        if (yn == 'Y')
            AddPayments();
    }

    private static void AddPayments()
    {
        Console.WriteLine("Add Payment");
        Console.WriteLine("Other Entity?");

        var otherEntity = Console.ReadLine();

        Console.WriteLine("Payment amount?");

        var amount = decimal.Parse(Console.ReadLine());

        Console.WriteLine($"Payment direction? [Outbound = 0, Inbound = 1]");

        var directionInput = Console.ReadLine();

        var direction = (Direction)int.Parse(directionInput);

        var payment = new Payment
        {
            OtherEntity = otherEntity,
            Amount = amount,
            Direction = direction,
            AccountId = Account.Id.Value
        };


        if (!ProcessPayment(payment))
        {
            AddPayments();
            return;
        }
            
        Console.WriteLine($"Payment Added, new balance is: £{Account.Balance}");

        Console.WriteLine("Add another payment? [Y or N]");
        var yn = Console.ReadLine()?.ToUpper().First();

        if (yn == 'Y')
            AddPayments();
    }

    private static bool ProcessPayment(Payment payment)
    {
        try
        {
            Account = _paymentService.MakePayment(Account, payment);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(ProcessPayment)} failed: {ex.Message}");
            Console.WriteLine("Please try again");
            return false;
        }
    }
}
