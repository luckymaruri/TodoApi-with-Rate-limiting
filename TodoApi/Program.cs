
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Models;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            
            /*  𝗥𝗮𝘁𝗲 𝗟𝗶𝗺𝗶𝘁𝗶𝗻𝗴 𝗠𝗶𝗱𝗱𝗹𝗲𝘄𝗮𝗿𝗲 - 𝑻𝒐𝒌𝒆𝒏 𝑩𝒖𝒄𝒌𝒆𝒕 𝗪𝗶𝗻𝗱𝗼𝘄
                If your application is using .NET 7 (or higher), a rate limiting middleware is available out of the box. It provides a way to apply rate limiting to your web application and API endpoints.

                Here's an example where:
                - You are given 100 requests every minute
                - Fill by 10 tokens each 1 minute */

            builder.Services.AddRateLimiter(rateLimiterOption => {
                rateLimiterOption.AddTokenBucketLimiter("token", options =>
                {
                    options.TokenLimit = 100;
                    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 5;
                    options.ReplenishmentPeriod=TimeSpan.FromMinutes(1);
                    options.TokensPerPeriod = 10;
                    options.AutoReplenishment = true;   
                });
            } );
            //---------------------------------

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
        }
    }
}
