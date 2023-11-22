using Microsoft.Extensions.Configuration;

namespace BookStoreAPI.Core.Configuration
{
    public static class EmailConfiguration
    {
        // Email adlı xüsusiyyət, e-poçt göndərməkdə istifadə olunacaq e-poçt ünvanını təyin edir
        public static string Email { get => GetConfiguration().GetSection("EmailSettings:Email").Value; }
        // Password adlı xüsusiyyət, e-poçt göndərməkdə istifadə olunacaq şifrəni təyin edir
        public static string Password { get => GetConfiguration().GetSection("EmailSettings:Password").Value; }
        // Host adlı xüsusiyyət, e-poçt göndərməkdə istifadə olunacaq host ünvanını təyin edir
        public static string Host { get => GetConfiguration().GetSection("EmailSettings:Host").Value; }
        // Port adlı xüsusiyyət, e-poçt göndərməkdə istifadə olunacaq port nömrəsini təyin edir
        public static int Port { get => Convert.ToInt32(GetConfiguration().GetSection("EmailSettings:Port").Value); }

        // Konfiqurasiyani əldə etmək üçün istifadə olunan xüsusi metod
        private static ConfigurationManager GetConfiguration()
        {
            // Konfiqurasiya obyekti yaratmaq
            ConfigurationManager configurationManager = new();
            // Tətbiqin əsas qovluğunu təyin edib əlavə etmək
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BookStoreAPI.BooksApi"));
            // appsettings.json faylindan konfiqurasiya parametrlərini əldə etmək
            configurationManager.AddJsonFile("appsettings.json");
            // Oluşdurulmuş konfiqurasiya obyektini cagirmaq
            return configurationManager;
        }
    }
}
