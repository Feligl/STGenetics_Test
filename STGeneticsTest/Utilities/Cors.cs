using Microsoft.AspNetCore.Cors.Infrastructure;

namespace STGeneticsTest.Utilities
{
    public class Cors
    {
        public static void CorsOptions(CorsOptions arg)
        {
            arg.AddPolicy("AllowedOrigins", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
    }
}
