using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.Basket;
using ETicaretAPI.Application.Repositories.BasketItem;
using ETicaretAPI.Application.Repositories.Orders;
using ETicaretAPI.Application.ViewModels.Baskets;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;

        public BasketService(IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager,
            IOrderReadRepository orderReadRepository,
            IBasketItemWriteRepository basketItemWriteRepository,
            IBasketWriteRepository basketWriteRepository,
            IBasketItemReadRepository basketItemReadRepository,
            IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket> GetTargetBasket()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
               AppUser? appUser = await _userManager.Users
                    .Include(b => b.Baskets)
                    .FirstOrDefaultAsync(u => u.UserName == username);

                var _basket = from basket in appUser.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order,
                              };

                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                {
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else 
                {
                    appUser.Baskets.Add(targetBasket);
                }

                await _basketWriteRepository.SaveAsync();

                return targetBasket;
            }
            throw new Exception("Beklenmeyen bir hata oluştu");
        }

        public async Task AddItemToAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await GetTargetBasket();

            if(basket != null)
            {
                var _basketItem = await _basketItemReadRepository.GetSingleAsync(p => p.BasketId == basket.Id && p.ProductId ==
                Guid.Parse(basketItem.ProductId));

                if(_basketItem != null)
                {
                    _basketItem.Quantity++;
                }
                else
                {
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity,
                    });
                }
            }

            throw new NotImplementedException();
        }

        public async Task<List<BasketItem>> GetBasketItemAsync()
        {
            Basket? basket = await GetTargetBasket();

            Basket? result = _basketReadRepository.Table
                .Include(b => b.BasketItems)
                .ThenInclude(b => b.Product)
                .FirstOrDefault(b => b.Id == basket.Id);

            return result.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);

            if(basketItem != null)
            {
                _basketItemWriteRepository.Delete(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(WM_Update_BasketItem basketItem)
        {
            BasketItem? basketItems = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);

            if(basketItems != null )
            {
                basketItems.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }
    }
}
