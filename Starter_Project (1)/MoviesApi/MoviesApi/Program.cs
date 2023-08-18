using MoviesApi.Repositories;

namespace MoviesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAllForTest",
                    policy =>
                    {
                        policy.WithOrigins("*")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddScoped<IMovieRepository, MovieRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors("AllowAllForTest");

            app.MapControllers();

            app.Run();
        }
    }
}