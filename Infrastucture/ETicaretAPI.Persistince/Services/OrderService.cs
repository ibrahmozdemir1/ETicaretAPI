using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Dtos.Order;
using ETicaretAPI.Application.Repositories.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task CreateOrder(CreateOrder createOrder)
        {
            await _orderWriteRepository.AddAsync(new()
            {
                Adress = createOrder.Adress,
                Id = Guid.Parse(createOrder.BasketId),
                Description= createOrder.Description,
            });
        }
    }
}
