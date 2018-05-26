using Autofac;
using FlashcardsManager.Desktop.IocContainer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FlashcardsManager.Core.EF;

namespace FlashcardsManager.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //EnsureDbExists();
            var builder = new ContainerBuilder();
            builder.RegisterAll();
            var container = builder.Build();

            AuthWindow auth = container.Resolve<AuthWindow>();
            MainWindow app = container.Resolve<MainWindow>();

            auth.ShowDialog();
            app.Show();
            //if(!auth.IsAuthorized)
            //    this.Shutdown();
        }

        //private static void EnsureDbExists()
        //{
        //    AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        //    var dbContext = new AppDbContextFactory().Create();
        //    Database.SetInitializer(
        //        new MigrateDatabaseToLatestVersion<AppDbContext, Core.Migrations.Configuration>(
        //            "DefaultConnection"));
        //    dbContext.Database.Initialize(false);
        //}
    }
}
