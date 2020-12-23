using System;
using System.Collections.Generic;

namespace Banks
{
    public partial class Manager // singleton 
    {
        private DateTime date = DateTime.Now;
        public DateTime Date => date;
        
        private static Manager singletonManager = new Manager();
        private Manager() {}
        public static Manager GetManager()
        {
            return singletonManager;
        }

        private List<Bank> banks = new List<Bank>();
        public List<Bank> Banks => banks;

        public void MoveTime(DateTime nextDateTime)
        {
            int days = Convert.ToInt32(Math.Floor((nextDateTime - date).TotalDays));
            Console.WriteLine($"Количество дней начисления процентов: {days}");

            for (var i = 0; i < days; i++)
            {
                date = date.AddDays(1);
                
                foreach (var bank in banks)
                {
                    bank.update();
                }
            }
        }
    }
}