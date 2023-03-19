using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Basket
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemQueryRequest, List<GetBasketItemQueryResponse>>
    {
        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemQueryResponse>> Handle(GetBasketItemQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemAsync();

            return basketItems.Select(ba => new GetBasketItemQueryResponse
            {
                BasketItemId = ba.Id.ToString(),
                Name = ba.Product.Name,
                Price = ba.Product.Price,
                Quantity = ba.Quantity,
            }).ToList();
        }
    }
}
