using System;
using System.Collections.Generic;
using OrdenaYaVersion1.ViewModels;
using Xamarin.Forms;

namespace OrdenaYaVersion1.Views.AccesoApp
{	
	public partial class LoginPage : ContentPage
	{	
		public LoginPage ()
		{
			InitializeComponent();
            BindingContext = new LoginVistaModelo(Navigation);
		}

        private async void NavToRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}

