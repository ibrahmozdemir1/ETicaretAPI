using ETicaretAPI.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Basket
{
    public class GetBasketItemQueryRequest : IRequest<List<GetBasketItemQueryResponse>>
    {
    }
}
