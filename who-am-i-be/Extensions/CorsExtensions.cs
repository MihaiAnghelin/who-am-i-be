namespace who_am_i_be.Extensions
{
    public static class CorsExtension
    {
        private const string CorsPolicyName = "EnableCORS";

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000",
                                "http://192.168.100.35:3000",
                                "https://who-am-i.mihaianghelin.ro",
                                "http://192.168.100.58:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        public static void UseCors(this WebApplication app)
        {
            app.UseCors(CorsPolicyName);
        }
    }
}