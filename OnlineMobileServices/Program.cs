namespace OnlineMobileServices_FE
{
    public class Program
    {
        public static string API_POST = string.Empty;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var port = builder.Configuration.GetValue<int>("Port", 8000);
            builder.WebHost.UseUrls("http://localhost:" + port);
            API_POST = builder.Configuration.GetValue<string>("APIPort");
            if (API_POST == null)
            {
                //DỪng chương trình và báo lỗi
                Console.WriteLine("APIPort is not found in appsettings.json");
                return;
            }
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
           
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
