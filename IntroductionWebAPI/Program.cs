using Autofac;
using Autofac.Extensions.DependencyInjection;
using Introduction.Repository.Common;
using Introduction.Repository;
using Introduction.Service.Common;
using Introduction.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<CarService>().As<ICarService>();
    containerBuilder.RegisterType<CarTypeService>().As<ICarTypeService>();
    containerBuilder.RegisterType<CarRepository>().As<ICarRepository>();
    containerBuilder.RegisterType<CarTypeRepository>().As<ICarTypeRepository>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
