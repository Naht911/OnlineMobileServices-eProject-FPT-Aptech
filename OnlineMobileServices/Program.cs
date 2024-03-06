namespace OnlineMobileServices_FE
{
    public class Program
    {
        public static string ApiUrl = "http://localhost:8001/api";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var port = builder.Configuration.GetValue<int>("Port", 8000);
            builder.WebHost.UseUrls("http://localhost:" + port);


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Add services to the container.
            builder.Services.AddHttpContextAccessor(); // Add this line
            //Bật session
            builder.Services.AddDistributedMemoryCache();
            
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
