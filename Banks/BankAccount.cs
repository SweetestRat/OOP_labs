using System;

namespace Banks
{
    public partial class Manager
    {
        public partial class Bank
        {
            public enum AccountType
            {
                DEBIT,
                CREDIT,
                DEPOSIT
            }

            public enum TransactionType
            {
                WITHDRAW,
                REFILL,
                TRANSFER
            }

            // фабрика
            
            public abstract class BankAccount 
            {
                protected Guid id = Guid.NewGuid();
                public Guid Id => id;

                protected Bank bank;
                public Bank Bank => bank;
                protected double money;

                public double Money
                {
                    get => money;
                    set => money = value;
                }

                protected double monthMoney = 0;

                protected Client client;
                protected AccountType accountType;

                public abstract void Withdraw(double _money);
                public abstract void Refill(double _money);
                public abstract void Transfer(double _money, BankAccount _accountTo);
                public abstract void update();
            }

            class Factory
            {
                public static Debit createDebit(Bank bank, Client client)
                {
                    return new Debit(0, bank, client);
                }

                public static Credit createCredit(Bank bank, Client client)
                {
                    return new Credit(bank, client);
                }

                public static Deposit createDeposit(Bank bank, DateTime datetime, Client client)
                {
                    return new Deposit(0, bank, datetime, client);
                }
            }

            public class Debit : BankAccount
            {
                public Debit(double _money, Bank _bank, Client _client)
                {
                    money = _money;
                    bank = _bank;
                    client = _client;
                    accountType = AccountType.DEBIT;
                }

                public override void Withdraw(double _money)
                {
                    if (!(_money <= money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Withdraw transaction = new Withdraw(_money, this);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }
                    else
                    {
                        throw new Exception(
                            $"Transaction is unavailable. On your account: {money}. Or you're a doubtful client :)");
                    }
                }

                public override void Refill(double _money)
                {
                    Refill transaction = new Refill(_money, this);
                    transaction.Execute();
                    bank.Transactions.Add(transaction.Id, transaction);
                }

                public override void Transfer(double _money, BankAccount _accountTo)
                {
                    if (!(_money <= money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Transfer transaction = new Transfer(_money, _money, this, _accountTo);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }
                    else
                    {
                        throw new Exception(
                            "Transaction is unavailable. Not enough money. Or you're a doubtful client :)");
                    }
                }

                public override void update()
                {
                    monthMoney += money * bank.Terms.debitPercent;

                    if (Manager.GetManager().Date.Day == 1)
                    {
                        money += monthMoney;
                        monthMoney = 0;
                    }
                }
            }

            public class Credit : BankAccount
            {
                public Credit(Bank _bank, Client _client)
                {
                    money = bank.Terms.credit.Item1;
                    bank = _bank;
                    client = _client;
                    accountType = AccountType.CREDIT;
                }

                public override void Withdraw(double _money)
                {
                    if (!(_money <= money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Withdraw transaction = new Withdraw(_money, this);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }
                    else
                    {
                        throw new Exception(
                            $"Transaction is unavailable. On your account: {bank.Terms.credit.Item1}. Or you're a doubtful client :)");
                    }
                }

                public override void Refill(double _money)
                {
                    Refill transaction = new Refill(_money, this);
                    transaction.Execute();
                    bank.Transactions.Add(transaction.Id, transaction);
                }

                public override void Transfer(double _money, BankAccount _accountTo)
                {
                    if (!(_money <= money) || !(bank.Terms.credit.Item1 <= _money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Transfer transaction = new Transfer(_money, _money, this, _accountTo);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }
                    else if (!(_money + _money * bank.Terms.credit.Item2 <= money) || !(bank.Terms.credit.Item1 > _money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Transfer transaction = new Transfer(_money + _money * bank.Terms.credit.Item2, _money, this,
                            _accountTo);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }

                }

                public override void update()
                {
                    // вроде ничего
                }
            }

            public class Deposit : BankAccount
            {
                private DateTime dateTime;

                public Deposit(double _money, Bank _bank, DateTime _dateTime, Client _client)
                {
                    money = _money;
                    bank = _bank;
                    dateTime = _dateTime;
                    client = _client;
                    accountType = AccountType.DEPOSIT;
                }

                public override void Withdraw(double _money)
                {
                    if (Manager.GetManager().Date <= dateTime || !(_money <= money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Withdraw transaction = new Withdraw(_money, this);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }
                    else
                    {
                        throw new Exception(
                            $"Transaction is unavailable. The function is not yet available or not enough money. On your account: {money}. Or you're a doubtful client :)");
                    }
                }

                public override void Refill(double _money)
                {
                    Refill transaction = new Refill(_money, this);
                    transaction.Execute();
                    bank.Transactions.Add(transaction.Id, transaction);
                }

                public override void Transfer(double _money, BankAccount _accountTo)
                {
                    if (Manager.GetManager().Date <= dateTime || !(_money <= money) || !client.DoubtfulClient || _money <= bank.Terms.limit)
                    {
                        Transfer transaction = new Transfer(_money, _money, this, _accountTo);
                        transaction.Execute();
                        bank.Transactions.Add(transaction.Id, transaction);
                    }
                    else
                    {
                        throw new Exception(
                            "Transaction is unavailable. The function is not yet available or not enough money. Or you're a doubtful client :)");
                    }
                }

                public override void update()
                {
                    if (money <= bank.Terms.deposit.Item1)
                    {
                        monthMoney += money * bank.Terms.deposit.Item2;
                    }
                    else if (money >= bank.Terms.deposit.Item4)
                    {
                        monthMoney += money * bank.Terms.deposit.Item5;
                    }
                    else
                    {
                        monthMoney += money * bank.Terms.deposit.Item3;
                    }

                    if (Manager.GetManager().Date.Day == 1)
                    {
                        money += monthMoney;
                        monthMoney = 0;
                    }
                }
            }
        }
    }
}