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
                                "who-am-i.mihaianghelin.ro")
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