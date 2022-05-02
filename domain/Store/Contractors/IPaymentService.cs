using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public interface IPaymentService
    {
        string UniqueCode { get; }

        string Title { get; }

        public Form CreateForm(Order order);

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value);

        OrderPayment GetPayment(Form form);
    }
}
