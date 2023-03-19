using ETicaretAPI.Application.Repositories.BasketItem;
using ETicaretAPI.Persistince.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Repositories.Concrete.BasketItem
{
    public class BasketItemReadRepository : ReadRepository<Domain.Entities.BasketItem>, IBasketItemReadRepository
    {
        public BasketItemReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
