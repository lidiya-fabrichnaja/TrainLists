using Autofac;
using Autofac.Extensions.DependencyInjection;
using TrainLists.Infrastructure.Extensions;
using TrainLists.WebApi.DependencyInjections;
using TrainLists.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtAuthorization();

builder.Services.AddControllers(x=> {
    x.Filters.Add(typeof(HttpGlobalExceptionFilter));
});

builder.Services.AddDbContext(builder.Configuration);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ServiceModule()));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.InitDbContext();

app.Run();
