using MediatR;
using TheatreBookingSystem.Events;
using TheatreBookingSystem.Infrastructure;
using TheatreBookingSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient<INotificationHandler<SeatBookedDomainEvent>, SeatBookedDomainEventHandler>();

builder.Services.AddScoped<IEventStore>(sp =>
{
	var config = sp.GetRequiredService<IConfiguration>();
	var connectionString = config["AzureBlobStorage:ConnectionString"];
	var containerName = config["AzureBlobStorage:ContainerName"];

	return new AzureBlobEventStore(connectionString, containerName);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
