using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks
{
    public partial class Manager
    {
        public partial class Bank
        {
            public class Client
            {
                public class Account
                {
                    private DebitAccount debit;
                    private CreditAccount credit;
                    private DepositAccount deposit;
                    public DebitAccount Debit => debit;
                    public CreditAccount Credit => credit;
                    public DepositAccount Deposit => deposit;

                    public class DebitAccount
                    {
                    }

                    public class CreditAccount
                    {
                    }

                    public class DepositAccount
                    {
                        private DateTime depositDate;
                        public DateTime DepositDate => depositDate;

                        public DepositAccount(DateTime date)
                        {
                            depositDate = date;
                        }
                    }

                    private Account()
                    {
                        debit = null;
                        credit = null;
                        deposit = null;
                    }

                    public Account(DebitAccount _debit)
                    {
                        debit = _debit;
                        credit = null;
                        deposit = null;
                    }

                    public Account(CreditAccount _credit)
                    {
                        credit = _credit;
                        debit = null;
                        deposit = null;
                    }

                    public Account(DepositAccount _deposit)
                    {
                        deposit = _deposit;
                        debit = null;
                        credit = null;
                    }
                }

                private Guid id;

                public Guid Id
                {
                    get => id;
                    set => id = value;
                }

                private bool doubtfulClient = true;
                public bool DoubtfulClient => doubtfulClient;

                private string name;
                private string address;
                private string passport;
                private List<BankAccount> clientAccounts = new List<BankAccount>();

                // строитель 
                public Client SetName(string fname)
                {
                    name = fname;
                    id = Guid.NewGuid();
                    return this;
                }

                public Client SetAddress(string _address)
                {
                    if (name != null)
                    {
                        address = _address;
                        doubtfulClient = false;
                        return this;
                    }

                    throw new Exception("Fill in name first");
                }

                public Client SetPassport(string _passport)
                {
                    if (name != null)
                    {
                        passport = _passport;
                        doubtfulClient = false;
                        return this;
                    }

                    throw new Exception("Fill in name first");
                }

                public BankAccount CreateAccount(Account newAccount, Bank bank)
                {
                    if (newAccount.Debit != null)
                    {
                        clientAccounts.Add(Factory.createDebit(bank, this));
                    }
                    else if (newAccount.Credit != null)
                    {
                        clientAccounts.Add(Factory.createCredit(bank, this));
                    }
                    else if (newAccount.Deposit != null)
                    {
                        clientAccounts.Add(Factory.createDeposit(bank, newAccount.Deposit.DepositDate, this));
                    }

                    bank.Accounts.Add(clientAccounts.Last().Id, clientAccounts.Last());

                    return clientAccounts.Last();
                }
            }
        }
    }
}