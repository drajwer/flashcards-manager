using Autofac;
using FlashcardsManager.Core.EF;
using System.Collections.Generic;
using FlashcardsManager.Desktop.ViewModels;
using FlashcardsManager.Core.UnitOfWork;
using Autofac.Core;
using FlashcardsManager.Core.Services;
using FlashcardsManager.Core.Repositories;
using FlashcardsManager.Core.Repositories.Interfaces;
using System;
using System.Net.Http;
using FlashcardsManager.Core.ApiClient;

namespace FlashcardsManager.Desktop.IocContainer
{
    public static class RegisterServicesExtensions
    {
        public static void RegisterAll(this ContainerBuilder builder)
        {
            builder.RegisterType<ApiClient>().SingleInstance();
            builder.RegisterType<HttpClient>().SingleInstance();

            builder.RegisterType<AuthWindow>();
            builder.RegisterType<MainWindow>();
            builder.RegisterTypes(typeof(CategoryViewModel), typeof(FlashcardsViewModel), typeof(UsersViewModel),
                typeof(HomeViewModel), typeof(ApplicationViewModel), typeof(UserCategorySelectionViewModel));

            builder.Register(context => new List<IPageViewModel>()
            {
                context.Resolve<HomeViewModel>(),
                context.Resolve<FlashcardsViewModel>(),
                context.Resolve<CategoryViewModel>(),
                context.Resolve<UsersViewModel>(),
                context.Resolve<UserCategorySelectionViewModel>(),
            });

        }
    }
}
