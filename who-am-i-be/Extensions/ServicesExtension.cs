using who_am_i_be.Interfaces;
using who_am_i_be.Services;

namespace who_am_i_be.Extensions
{
    public static class ServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenEmitterService, TokenEmitterService>();
        }
    }
}