using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class OrderDelivery
    {
        public string UniqueCode { get; }
        public string Description { get; }
        public decimal Price { get; } 
        public IReadOnlyDictionary<string, string> Parametrs { get; }
        public OrderDelivery(string uniqueCode,
                             string description,
                             decimal amount,
                             IReadOnlyDictionary<string, string> parametrs)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentException(nameof(uniqueCode));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(nameof(description));

            if (parametrs == null)
                throw new ArgumentNullException(nameof(parametrs));

            UniqueCode = uniqueCode;
            Description = description;
            Price = amount;
            Parametrs = parametrs;
        }
    }
}
