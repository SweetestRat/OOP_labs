using System;
using System.Linq;

namespace Banks
{
    class Program
    {
        static void Main()
        {
            // фабрика аккаунтов
            // строитель клиентов
            // команда транзакций
            
            Manager manager = Manager.GetManager();
            Manager.Bank Tinkoff = new Manager.Bank("Tinkoff", 3.65, (50000, 0.3, 3.5, 100000, 0.4), (20000, 3.65), 5000);
            
            Manager.Bank.Client SweetestRat = new Manager.Bank.Client();
            SweetestRat.SetName("Gilde Vlada").SetAddress("Ul. Kurortnaya, 8").SetPassport("1234 567890");
            Manager.Bank.Client danyaffff = new Manager.Bank.Client();
            danyaffff.SetName("K Danya");

            var SweetestRatDepositAccount1 = SweetestRat.CreateAccount(new Manager.Bank.Client.Account(new Manager.Bank.Client.Account.DepositAccount(new DateTime(2020, 12, 31))), Tinkoff); // -sr
            SweetestRatDepositAccount1.Refill(40000); // 1
            Console.WriteLine($"Количество транзакций: {Tinkoff.Transactions.Count}");
            SweetestRatDepositAccount1.Withdraw(10000); // 2
            Console.WriteLine($"Количество транзакций после выполнения еще одной транзакции: {Tinkoff.Transactions.Count}");
            
            Tinkoff.UndoTransaction(Tinkoff.TransactionId.Last());
            Console.WriteLine($"Количество транзакций после отмены последней транзакции: {Tinkoff.Transactions.Count}"); // 1

            var danyaffffDebitAccount1 = danyaffff.CreateAccount(new Manager.Bank.Client.Account(new Manager.Bank.Client.Account.DebitAccount()), Tinkoff);
            danyaffffDebitAccount1.Refill(50000);
            
            danyaffffDebitAccount1.Transfer(4000, SweetestRatDepositAccount1); 
            // теперь у Дани 46000
            // а у Влады 44000 (40000 - 10000 + (тк отмена) 10000)

            Console.WriteLine($"Количество денег на счету у Дани: {danyaffffDebitAccount1.Money}"); // начисление процента
            manager.MoveTime(new DateTime(2021, 2, 2));
            Console.WriteLine("Начисление процентов привело к такой сумме:" + "{0:0.##}", danyaffffDebitAccount1.Money);
        }
    }
}