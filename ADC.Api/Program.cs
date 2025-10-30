using ADC.Infraestructure;
using ADC.Persistence;
using ADC.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfraestrure();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UsePersistence();

app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
