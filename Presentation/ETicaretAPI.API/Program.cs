using ETicaretAPI.Application;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastucture;
using ETicaretAPI.Infrastucture.Filters;
using ETicaretAPI.Infrastucture.Services.Storage.Local;
using ETicaretAPI.Persistince;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistinceService();

builder.Services.AddAuthentication();
builder.Services.AddHttpClient();

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddStorage<LocalStorage>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
));
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()
     ).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Olu�turulacak token de�erinin hangi sitelerin kullan�ca��n� belirledi�imiz de�erdir.
            ValidateIssuer = true, // Olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r.
            ValidateLifetime = true, // Olu�turulan token de�erinin s�resini kontrol edecek olan do�rulamad�r.
            ValidateIssuerSigningKey = true,
            // �retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden security
            //key verisinin do�rulanmas�d�r.

            ValidAudience = builder.Configuration["Token:Audince"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))

        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
