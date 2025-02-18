﻿using Core;
using Core.Interfaces;
using Data;
using Data.Interfaces;
using Data.Models;
using LedgerApp;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        //setup our DI
        var serviceProvider = SetupDI();

        var paymentService = serviceProvider.GetService<IPaymentService>();

        if(paymentService == null) 
            throw new ArgumentNullException(nameof(paymentService));

        App.Start(paymentService);
    }

    private static ServiceProvider SetupDI() => new ServiceCollection()
            .AddSingleton<IRepository, Repository>()
            .AddSingleton<IPaymentService, PaymentService>()
            .BuildServiceProvider();
}