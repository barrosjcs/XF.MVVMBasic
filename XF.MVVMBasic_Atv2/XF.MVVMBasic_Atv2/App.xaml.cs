﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.MVVMBasic.ViewModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XF.MVVMBasic_Atv2
{
    public partial class App : Application
    {
        #region ViewModels
        public static AlunoViewModel AlunoVM { get; set; }
        #endregion

        public App()
        {
            InitializeComponent();
            if (AlunoVM == null)
                AlunoVM = new AlunoViewModel();

            MainPage = new NavigationPage(new MVVMBasic.View.AlunoView()
            {
                BindingContext = AlunoVM
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
