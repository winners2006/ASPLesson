using Autofac;
using Autofac.Extensions.DependencyInjection;
using ASPLesson.Abstraction;
using ASPLesson.Data;
using ASPLesson.Mapper;
using ASPLesson.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddMemoryCache(x => x.TrackStatistics = true);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
	container.RegisterType<ProductRepository>().As<IProductRepository>();
	container.RegisterType<ProductGroupRepository>().As<IProductGroupRepository>();
	container.RegisterType<StorageContext>().WithParameter("configuration", builder.Configuration).InstancePerDependency();
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
