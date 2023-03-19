using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.Basket;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistince.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Repositories.Concrete.Basket
{
    public class BasketWriteRepository : WriteRepository<Domain.Entities.Basket>, IBasketWriteRepository
    {
        public BasketWriteRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
