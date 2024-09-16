using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OrdenaYaVersion1.ViewModels;
using Xamarin.Forms;

namespace OrdenaYaVersion1.Views.AccesoApp
{	
	public partial class RegisterPage : ContentPage
	{	
		public RegisterPage ()
		{
			InitializeComponent ();
            BindingContext = new RegisterVistaModelo(Navigation);
        }

        private async void NavToLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}

