using System;

namespace Shop
{
    public class Product
    {
        private string prodname;
        public string prodName 
        { 
            get => prodname; 
            set => prodname = value;
        }

        private Guid prodid;
        public Guid prodId
        {
            get => prodid;
            set => prodid = value;
        }
        
        private int prodprice;
        public int prodPrice
        {
            get => prodprice;
            set => prodprice = value;
        }
        
        private int prodamount;
        public int prodAmount
        {
            get => prodamount;
            set => prodamount = value;
        }
        
        public Product(string _prodName)
        {
            prodName = _prodName;
            prodId = Guid.NewGuid();

            // return prodId;
        }
    }
}
