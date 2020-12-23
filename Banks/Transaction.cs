using System;

namespace Banks
{
    public partial class Manager
    {
        public partial class Bank
        {
            public abstract class Transaction
            {
                protected Guid id { get; set; }
                public Guid Id => id;

                protected BankAccount accountFrom;

                private TransactionType ttype;

                public TransactionType Ttype
                {
                    get => ttype;
                    set => ttype = value;
                }

                public abstract void Execute();
                public abstract void Undo();

                public void SaveID()
                {
                    accountFrom.Bank.TransactionId.Add(id);
                }
            }

            public class Withdraw : Transaction
            {
                private double money;

                public Withdraw(double _money, BankAccount _account)
                {
                    money = _money;
                    accountFrom = _account;
                    Ttype = TransactionType.WITHDRAW;
                    id = Guid.NewGuid();
                }

                public override void Execute()
                {
                    accountFrom.Money -= money;
                    SaveID();
                }

                public override void Undo()
                {
                    accountFrom.Money += money;
                }
            }

            public class Refill : Transaction
            {
                private double money;

                public Refill(double _money, BankAccount _account)
                {
                    money = _money;
                    accountFrom = _account;
                    id = Guid.NewGuid();
                }

                public override void Execute()
                {
                    accountFrom.Money += money;
                    SaveID();
                }

                public override void Undo()
                {
                    accountFrom.Money -= money;
                }
            }

            public class Transfer : Transaction
            {
                private double moneyFrom;
                private double moneyTo;
                private BankAccount accountTo;

                public Transfer(double _moneyFrom, double _moneyTo, BankAccount _accountFrom, BankAccount _accountTo)
                {
                    moneyFrom = _moneyFrom;
                    accountFrom = _accountFrom;
                    accountTo = _accountTo;
                    moneyTo = _moneyTo;
                    id = Guid.NewGuid();
                }

                public override void Execute()
                {
                    accountFrom.Money -= moneyFrom;
                    accountTo.Money += moneyTo;
                    SaveID();
                }

                public override void Undo()
                {
                    accountFrom.Money += moneyFrom;
                    accountTo.Money -= moneyTo;
                }
            }
        }
    }
}