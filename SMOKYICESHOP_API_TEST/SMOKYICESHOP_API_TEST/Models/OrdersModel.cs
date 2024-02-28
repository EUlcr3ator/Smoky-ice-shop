using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.Orders;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class OrdersModel
    {
        private readonly SmokyIceDbContext _dbcontext;
        private readonly OrderDtoService _orderDtoService;

        public OrdersModel(SmokyIceDbContext dbcontext, OrderDtoService orderDtoService)
        {
            _dbcontext = dbcontext;
            _orderDtoService = orderDtoService;
        }

        public OrderDTO GetOrder(Guid orderId)
        {
            Order order = _orderDtoService.IncludeForOrder(_dbcontext.Orders).First(x => x.Id == orderId);
            return _orderDtoService.CreateOrderDto(order);
        }

        public IEnumerable<OrderDTO> GetOrderHistory(Guid clientId)
        {
            return _orderDtoService.IncludeForOrder(_dbcontext.Orders)
                .Where(x => x.ClientId == clientId)
                .Select(x => _orderDtoService.CreateOrderDto(x))
                .ToList();
        }

        public IEnumerable<OrderDTO> GetOrdersByStatusId(Guid statusId)
        {
            return _dbcontext.Orders
                .Where(x => x.OrderStatusId == statusId)
                .Select(x => _orderDtoService.CreateOrderDto(x))
                .ToList();
        }

        public void UpdateOrderStatus(Guid orderID, Guid statusId)
        {
            Order order = _dbcontext.Orders.First(x => x.Id == orderID);
            order.OrderStatusId = statusId;
            _dbcontext.SaveChanges();
        }

        public Guid CreateOrder(CreateOrderDTO orderDto, Guid clientId)
        {
            var cart = _dbcontext.CartHasGoods
                .Include(x => x.Good)
                .Where(x => x.CartId == clientId)
                .ToList();

            OrderStatus status = _dbcontext.OrderStatuses.First(x => x.Name == "NewOrder");

            if (cart.Count == 0)
                throw new InvalidOperationException();

            Order order = _orderDtoService.CreateOrderEntity(orderDto, cart, clientId, status.Id);
            _dbcontext.Orders.Add(order);
            _dbcontext.CartHasGoods.RemoveRange(cart);
            _dbcontext.SaveChanges();

            return order.Id;
        }

        public Guid CreateOrder(CreateOrderWithoutClientDTO orderDto)
        {
            var cart = new List<OrderHasGood>();

            foreach (var item in orderDto.Goods)
            {
                Good good = _dbcontext.Goods.First(x => x.Id == item.Id);
                cart.Add(_orderDtoService.CreateOrderGood(good, item.Count));
            }

            OrderStatus status = _dbcontext.OrderStatuses.First(x => x.Name == "NewOrder");

            if (cart.Count == 0)
                throw new InvalidOperationException();

            Order order = _orderDtoService.CreateOrderEntity(orderDto, cart, status.Id);
            _dbcontext.Orders.Add(order);
            _dbcontext.SaveChanges();

            return order.Id;
        }
    }
}
