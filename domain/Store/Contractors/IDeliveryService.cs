namespace Store.Contractors
{
    public interface IDeliveryService
    {
        string UniqueCode { get; }

        string Title { get; }

        public Form CreateForm(Order order);

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> value);


    }
}
