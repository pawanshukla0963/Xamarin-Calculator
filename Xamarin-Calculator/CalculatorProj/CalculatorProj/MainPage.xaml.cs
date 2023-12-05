using CalculatorProj.Model;
using CalculatorProj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalculatorProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
	{
        public MainPageViewModel vm; 

        public MainPage(MainPageViewModel mainPageViewModel)
		{
			InitializeComponent();
            this.BindingContext = mainPageViewModel;
            vm= (MainPageViewModel)this.BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetCurrencies();
           
        }

        private void Picker_SelectedIndexChangedBase(object sender, EventArgs e)
        {
            Currency curreny = (Currency)(sender as Picker).SelectedItem;
            vm.UpdateBaseCurrency(curreny.Symbol);
        }

        private void Picker_SelectedIndexChangedTarget(object sender, EventArgs e)
        {
            Currency curreny = (Currency)(sender as Picker).SelectedItem;
            vm.UpdateTargerCurrency(curreny.Symbol);
        }
    }
}
