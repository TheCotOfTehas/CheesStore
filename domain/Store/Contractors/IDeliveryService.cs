﻿namespace Store.Contractors
{
    public interface IDeliveryService
    {
        string UniqueCode { get; }

        string Title { get; }

        public Form CreateForm(Order order);

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> value);

        OrderDelivery GetDelivery(Form form);
    }
}
