using ETicaretAPI.Application.ViewModels.Baskets;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemAsync();
        public Task AddItemToAsync(VM_Create_BasketItem basketItem);
        public Task UpdateQuantityAsync(WM_Update_BasketItem basketItem);
        public Task RemoveBasketItemAsync(string basketItemId); 
    }
}
