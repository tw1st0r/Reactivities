using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Middlewares
{
    public class MigrationMiddleware : IMiddleware
    {
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;
        public MigrationMiddleware(DataContext dataContext, ILogger<MigrationMiddleware> logger)
        {
            _logger = logger;
            _dataContext = dataContext;            
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                //var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                await _dataContext.Database.MigrateAsync();
                await Seed.SeedData(_dataContext);
               _logger.LogInformation($">>>>>>>>>>>>>>>>>>>>>>>>>> Request path {context.Request.Path}. Performed migration and db seeding");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during migration");
            }

            await next(context);
        }
    }
}