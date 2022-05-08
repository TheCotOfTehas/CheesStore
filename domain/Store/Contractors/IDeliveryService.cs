namespace Store.Contractors
{
    public interface IDeliveryService
    {
        string Name { get; }

        string Title { get; }

        public Form FirstForm(Order order);

        public Form NextForm(int step, IReadOnlyDictionary<string, string> value);

        public OrderDelivery GetDelivery(Form form);
    }
}
