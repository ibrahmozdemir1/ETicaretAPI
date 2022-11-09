using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                   .WithMessage("Lütfen Ürün Adı Giriniz.")
                .MaximumLength(150)
                .MinimumLength(5)
                   .WithMessage("Lütfen ürün adını 5-150 karakter arasında giriniz.");
            RuleFor(c => c.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen fiyatı boş geçmeyiniz.")
                .Must(s => s >=0)
                    .WithMessage("Fiyat negatif bir değer olamaz.");
            RuleFor(c => c.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen stoğu boş geçmeyiniz.")
                .Must(s => s >= 0)
                     .WithMessage("Stok bilgisi negatif olamaz.");
        }
    }
}
