namespace SupermercadoVirtual
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Añadir servicios al contenedor
            builder.Services.AddControllersWithViews();

            // Añadir sesiones al contenedor de servicios
            builder.Services.AddSession(); // Añadir soporte para sesiones

            var app = builder.Build();

            // Configurar el pipeline de manejo de solicitudes HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Usar sesiones antes de la autorización y el enrutamiento
            app.UseSession(); // Habilitar el uso de sesiones

            app.UseAuthorization();

            // Mapear rutas de controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Rutas para el TiendaController
            app.MapControllerRoute(
                name: "tienda",
                pattern: "tienda/{action=Index}/{id?}",
                defaults: new { controller = "Tienda" });

            app.Run();
        }
    }
}