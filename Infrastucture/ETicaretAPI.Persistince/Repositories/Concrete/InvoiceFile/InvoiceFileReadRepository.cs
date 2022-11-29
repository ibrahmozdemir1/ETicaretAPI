using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistince.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Repositories.Concrete.InvoiceFile
{
    public class InvoiceFileReadRepository : ReadRepository<ETicaretAPI.Domain.Entities.InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
