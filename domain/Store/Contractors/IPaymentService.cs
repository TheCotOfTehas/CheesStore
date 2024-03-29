﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public interface IPaymentService
    {
        string Name { get; }

        string Title { get; }

        public Form FirstForm(Order order);

        public Form NextForm(int step, IReadOnlyDictionary<string, string> value);

        OrderPayment GetPayment(Form form);
    }
}
