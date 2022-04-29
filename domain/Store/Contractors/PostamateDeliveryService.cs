using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private readonly IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1","Москва" },
            {"2","Санкт-Петербург" },
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates =
            new Dictionary<string, IReadOnlyDictionary<string, string>>
            {
                {
                    "1",
                    new Dictionary<string, string>
                    {
                        {"1", "Казанский вокзал" },
                        {"2", "Охотничий ряд" },
                        {"3", "Савеловский рынок" },
                    }
                },
                {
                    "2",
                    new Dictionary<string, string>
                    {
                        {"4", "Московский вокзал" },
                        {"5", "Гостиный двор" },
                        {"6", "Петропавловская крепость" },
                    }
                }
            };

        public string UniqueCode => "Postamate";

        public string Title => "Доставка чер почтоматы в Москве и Санкт-Петербурге";

        public Form CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("Город", "city", "1", cities),
            });
        }

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> value)
        {
            if (step == 1)
            {
                if (value["city"] == "1")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Город", "city", "1"),
                        new SelectionField("Постомат", "postamate", "1", postamates["1"]),
                    });
                }
                else if (value["city"] == "2")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Город", "city", "2"),
                        new SelectionField("Постомат", "postamate", "4", postamates["4"]),
                    });
                }
                else
                    throw new InvalidOperationException("Invalid postamate");
            }
            else if (step == 2)
            {
                return new Form(UniqueCode, orderId, 3, true, new Field[]
                   {
                        new HiddenField("Город", "city", value["city"]),
                        new HiddenField("Постомат", "postamate", value["postamates"]),
                   });
            }
            else
                throw new InvalidOperationException("Invalid postamate");
        }
    }
}
