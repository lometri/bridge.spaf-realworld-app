﻿using System;
using System.Linq;
using System.Reflection;
using Bridge;
using Bridge.Ioc;
using Bridge.Messenger;
using Bridge.Navigation;
using Bridge.Spaf.Attributes;
using realworld.spaf.Services;
using realworld.spaf.Services.impl;
using realworld.spaf.ViewModels;

namespace Bridge.Spaf
{
    public class SpafApp
    {
        public static IIoc Container;

        public static async void Main()
        {
            #if TEST
            return;
            #endif
            
            #if !DEBUG
            NavigationUtility.VirtualDirectory = "realworld.spaf"; //  virtual dir for release environment
            #endif

            Container = new BridgeIoc();
            ContainerConfig(); // config container
            var mainVm = Container.Resolve<MainViewModel>();
            await mainVm.Start();
            
            Container.Resolve<INavigator>().InitNavigation(); // init navigation
        }

        private static void ContainerConfig()
        {
            // navigator
            Container.RegisterSingleInstance<INavigator, BridgeNavigatorWithRouting>();
            Container.RegisterSingleInstance<IBrowserHistoryManager, QueryParameterNavigationHistory>();
            Container.Register<INavigatorConfigurator, CustomRoutesConfig>(); 

            // messenger
            Container.RegisterSingleInstance<IMessenger, Messenger.Messenger>();

            // viewmodels
            RegisterAllViewModels();

            // register custom resource, services..
            Container.RegisterSingleInstance<ISettings, Settings>();
            Container.RegisterSingleInstance<IUserService, UserService>();

            Container.Register<IArticleResources,ArticleResources>();
            Container.Register<IUserResources,UserResources>();
            Container.Register<IFeedResources,FeedResources>();
            Container.Register<IProfileResources,ProfileResources>();
            
            Container.Register<IRepository,LocalStorageRepository>();
            Container.Register<ISettingsResources,SettingsResources>();

        }

        #region PAGES IDS
        // static pages id


        public static string HomeId => "home";
        public static string LoginId => "login";
        public static string RegisterId => "register";
        public static string ProfileId => "profile";
        public static string SettingsId => "settings";
        public static string EditArticleId => "editArticle";
        public static string ArticleId => "article";

        #endregion

        #region MESSAGES
        // messenger helper for global messages and messages ids

        public static class Messages
        {
            public class GlobalSender { };

            public static GlobalSender Sender = new GlobalSender();

            public static string LoginDone => "LoginDone";

        }


        #endregion

        /// <summary>
        /// Register all types that end with "viewmodel".
        /// You can register a viewmode as Singlr Instance adding "SingleInstanceAttribute" to the class
        /// </summary>
        private static void RegisterAllViewModels()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(w => w.Name.ToLower().EndsWith("viewmodel")).ToList();

            types.ForEach(f =>
            {
                var attributes = f.GetCustomAttributes(typeof(SingleInstanceAttribute), true);

                if (attributes.Any())
                    Container.RegisterSingleInstance(f);
                else
                    Container.Register(f);
            });

        }
    }
}
