using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using Week1_DI.DataAccess;
using Week1_DI.Services;

namespace Week1_DI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        ServiceCollection services;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            services = new ServiceCollection();//tao be chua (dependency container)

            //cau hinh dich vu
            ConfigureServices(services);

            //xay dung provider
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            //luu thong tin vao dependency container// luu thong tin config service nao dc dung
            services.AddSingleton<ICategoryDA, CategoryDASQLServer>();
            //services.AddSingleton<ICategoryDA, CategoryDAJson>();
            services.AddSingleton<ICategoryServices, CategoryServicesVer1>();
        }
    }

}
