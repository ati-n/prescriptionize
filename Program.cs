using PrescribeAPI.Models;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Connect to Postgres
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<PrescriptionContext>(opt => opt.UseInMemoryDatabase("PrescriptionList"));
builder.Services.AddDbContext<PrescriptionContext>(opt => opt.UseNpgsql(connectionString));


/**
 * Helps capture database-related exceptions along with
 * possible actions to resolve those in the HTML response format.
 */
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// CORS Middleware handles cross-origin requests.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://example.com",
                                              "http://localhost:7021")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();