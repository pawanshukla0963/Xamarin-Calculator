using CalculatorProj.Services;
using CalculatorProj.ViewModels;
using CommonServiceLocator;
using System;
using Unity;
using Unity.RegistrationByConvention;
using Unity.ServiceLocation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace CalculatorProj
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IDataService, DataService>();
            MainPage =  new MainPage(unityContainer.Resolve<MainPageViewModel>());
		}

		protected override void OnStart ()
		{


        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

    }
}
