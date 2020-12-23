using System;
using System.Collections.Generic;

namespace Banks
{
    public partial class Manager
    {
        public partial class Bank
        {
            public struct TERMS
            {
                public double debitPercent;
                public (int, double, double, int, double) deposit;
                public (int, double) credit; // limit + commission
                public int limit; // для сомнительных пользователей
            }

            private string Name;
            private TERMS terms;
            public TERMS Terms => terms;

            List<Guid> transactionID = new List<Guid>();
            public List<Guid> TransactionId => transactionID;

            Dictionary<Guid, Transaction> _transactions = new Dictionary<Guid, Transaction>();
            public Dictionary<Guid, Transaction> Transactions => _transactions;

            private Dictionary<Guid, BankAccount> accounts = new Dictionary<Guid, BankAccount>();
            public Dictionary<Guid, BankAccount> Accounts => accounts;

            private Dictionary<Guid, Client> idClient = new Dictionary<Guid, Client>();

            public void update() // Move time in accounts from Manager.MoveTime
            {
                foreach (var account in accounts)
                {
                    account.Value.update();
                }
            }

            public Bank(string name, double _debitPercent, (int, double, double, int, double) _depositPercent,
                (int, double) _credit, int _limit)
            {
                Name = name;
                terms.debitPercent = _debitPercent / (DateTime.IsLeapYear(Manager.GetManager().Date.Year) ? 366 : 365) /
                                     100;
                terms.deposit = _depositPercent;
                terms.credit = _credit;
                terms.limit = _limit;
                GetManager().Banks.Add(this);
            }

            public void UndoTransaction(Guid id)
            {
                Transactions[id].Undo();
                Transactions.Remove(id);
                transactionID.Remove(id);
            }
        }
    }
}
