using Microsoft.EntityFrameworkCore;
using KgyHoldingWebsite.Data;
using KgyHoldingWebsite.Application.Interfaces;
using KgyHoldingWebsite.Application.Settings;
using KgyHoldingWebsite.Application.Services;
var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KgyDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IBoardMemberService, BoardMemberService>();
builder.Services.AddScoped<ICompanyHistoryService, CompanyHistoryService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Swagger için gerekli
builder.Services.AddSwaggerGen();          // Swagger için gerekli

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapGet("/", () => "KGY Holding API is running.");
app.MapGet("/health", () => Results.Ok(new { status = "OK" }));


app.MapControllers();

app.Run();
