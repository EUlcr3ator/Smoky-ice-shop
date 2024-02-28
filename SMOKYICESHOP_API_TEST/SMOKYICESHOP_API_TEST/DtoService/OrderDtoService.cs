using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SMOKYICESHOP_API_TEST.DTO.Orders;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public class OrderDtoService
    {
        private readonly DefaultDtoService _defaultDtoService;

        public OrderDtoService(DefaultDtoService defaultDtoService)
        {
            _defaultDtoService = defaultDtoService;
        }

        public OrderDTO CreateOrderDto(Order entity)
        {
            OrderDTO dto = new OrderDTO
            {
                Id = entity.Id,
                ClientID = entity.ClientId,
                OrderStatus = entity.OrderStatus.NameUkr,
                PaymentMethod = entity.DeliveryInfo.PaymentMethod.NameUkr,
                CreationDate = entity.CreationDate,
                ClientFirstName = entity.DeliveryInfo.FirstName,
                ClientLastName = entity.DeliveryInfo.LastName,
                Comment = entity.DeliveryInfo.Comment,
                DeliveryRegion = entity.DeliveryInfo.DeliveryRegion,
                DeliveryCity = entity.DeliveryInfo.DeliveryCity,
                PostOfficeNumber = entity.DeliveryInfo.PostOfficeNumber,
                PhoneNumber = entity.DeliveryInfo.PhoneNumber,
                TotalPrice = entity.TotalPrice,
                Goods = entity.OrderHasGoods.Select(x => new OrderGoodDTO
                {
                    Price = x.Price,
                    Count = x.ProductCount,
                    Good = _defaultDtoService.CreateDefaultGood(x.Good)
                })
            };

            return dto;
        }

        public IQueryable<Order> IncludeForOrder(IQueryable<Order> orders)
        {
            return orders
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryInfo)
                    .ThenInclude(x => x.PaymentMethod)
                .Include(x => x.OrderHasGoods)
                    .ThenInclude(x => x.Good);
        }

        public IQueryable<CartHasGood> IncludeForCart(IQueryable<CartHasGood> cartHasGoods)
        {
            return cartHasGoods.Include(x => x.Good);
        }

        public OrderHasGood CreateOrderGood(CartHasGood cartGood)
        {
            return new OrderHasGood
            {
                GoodId = cartGood.GoodId,
                ProductCount = cartGood.ProductCount,
                Price = cartGood.Good.Price
            };
        }

        public OrderHasGood CreateOrderGood(Good good, byte count)
        {
            return new OrderHasGood
            {
                GoodId = good.Id,
                ProductCount = count,
                Price = good.Price
            };
        }

        public Order CreateOrderEntity(CreateOrderDTO dto, IEnumerable<CartHasGood> cart, Guid clientId, Guid statusId)
        {
            Order order = BaseCreateOrderEntity(dto, clientId, statusId);

            IEnumerable<OrderHasGood> goods = cart.Select(x => CreateOrderGood(x));

            order.TotalPrice = (short)goods.Sum(x => x.Price);
            order.OrderHasGoods.AddRange(goods);

            return order;
        }

        public Order CreateOrderEntity(CreateOrderWithoutClientDTO dto, IEnumerable<OrderHasGood> goods, Guid statusId)
        {
            Order order = BaseCreateOrderEntity(dto, null, statusId);

            order.TotalPrice = (short)goods.Sum(x => x.Price);
            order.OrderHasGoods.AddRange(goods);

            return order;
        }

        private Order BaseCreateOrderEntity(IDeliveryInfo dto, Guid? clientId, Guid statusId)
        {
            return new Order
            {
                ClientId = clientId,
                OrderStatusId = statusId,
                CreationDate = DateTime.Now,

                DeliveryInfo = new DeliveryInfo
                {
                    DeliveryMethodId = dto.DeliveryID,
                    PaymentMethodId = dto.PaymentID,
                    PhoneNumber = dto.PhoneNumber,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DeliveryRegion = dto.DeliveryRegion,
                    DeliveryCity = dto.DeliveryCity,
                    PostOfficeNumber = dto.PostOfficeNumber,
                    Comment = dto.Comment
                }
            };
        }
    }
}
