using elastic_search_demo.Repository;
using elastic_search_demo.RepositoryInterface;
using elastic_search_demo.Service;
using elastic_search_demo.ServiceRepository;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("books");

var client = new ElasticClient(settings);

builder.Services.AddSingleton<IElasticClient>(client);
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers();
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
