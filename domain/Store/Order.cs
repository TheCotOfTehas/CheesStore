using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class Order
    {
        private readonly OrderDto dto;

        public int Id => dto.Id;

        public OrderItemCollection Items { get; }

        public string CellPhone
        {
            get => dto.CellPhone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CellPhone));

                dto.CellPhone = value;
            }
        }

        public OrderDelivery Delivery
        {
            get
            {
                if (dto.DeliveryUniqueCode == null)
                    return null;

                return new OrderDelivery(
                    dto.DeliveryUniqueCode,
                    dto.DeliveryDescription,
                    dto.DeliveryPrice,
                    dto.DeliveryParameters);
            }
            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Delivery));

                dto.DeliveryUniqueCode = value.UniqueCode;
                dto.DeliveryDescription = value.Description;
                dto.DeliveryPrice = value.Price;
                dto.DeliveryParameters = value.Parametrs
                                              .ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public OrderPayment Payment
        {
            get
            {
                if (dto.PaymentServiceName == null)
                    return null;

                return new OrderPayment(
                    dto.PaymentServiceName,
                    dto.PaymentDescription,
                    dto.PaymentParameters);
            }
            set
            {
                if (value == null)
                    throw new ArgumentException(nameof(Payment));

                dto.PaymentServiceName = value.UniqueCode;
                dto.PaymentDescription = value.Description;
                dto.PaymentParameters = value.Parametrs
                                             .ToDictionary(p => p.Key, p => p.Value);
            }
        }


        public decimal TotatalCount => Items.Sum(item => item.Count);

        public decimal TotalPrice => Items
            .Sum(items => items.Price * items.Count) 
            + (Delivery?.Price ?? 0m);

        public Order(OrderDto dto)
        {
            this.dto = dto;
            Items = new OrderItemCollection(dto);
        }
        public static class DtoFactory
        {
            public static OrderDto Create() => new OrderDto();
        }

        public static class Mapper
        {
            public static Order Map(OrderDto dto) => new Order(dto);

            public static OrderDto Map(Order domain) => domain.dto;
        }

        //private static void ThrowProductException(string messege, int produktId)
        //{
        //    var exception = new InvalidOperationException(messege);

        //    exception.Data["produktId"] = produktId;

        //    throw exception;
        //}
    }
}
