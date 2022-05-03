namespace Store
{
    public class OrderPayment
    {
        public string UniqueCode { get; }
        public string Description { get; }
        public IReadOnlyDictionary<string, string> Parametrs { get; }
        public OrderPayment(string uniqueCode,
                             string description,
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
            Parametrs = parametrs;
        }
    }
}
