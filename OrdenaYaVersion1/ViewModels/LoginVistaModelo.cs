using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrdenaYaVersion1.Models;
using Firebase.Auth;
using OrdenaYaVersion1.Conexion;

namespace OrdenaYaVersion1.ViewModels
{
	public class LoginVistaModelo : BaseVistaModelo
	{
        #region Atributos
        private string email;
        private string clave;
        #endregion

        #region Propiedades
        public string entryEmail
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        public string entryPassword
        {
            get { return clave; }
            set { SetValue(ref clave, value); }
        }

        #endregion

        #region Command
        public Command LoginCommand { get; }
        #endregion

        #region Metodo
        public async Task LoginUsuario()
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(clave))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, rellena todos los campos.", "Aceptar");
                return;
            }

            var objusuario = new UserModel()
            {
                EmailField = email,
                PasswordField = clave,
            };
            try
            {

                var autenticacion = new FirebaseAuthProvider(new FirebaseConfig(DBConn.WepApyAuthentication));
                var authuser = await autenticacion.SignInWithEmailAndPasswordAsync(objusuario.EmailField.ToString(), objusuario.PasswordField.ToString());
                string obternertoken = authuser.FirebaseToken;

                //Guardar el token en Application.Current.Properties
                Application.Current.Properties["FirebaseToken"] = obternertoken;

                await Application.Current.MainPage.DisplayAlert("Éxito", "Inicio de sesión correcto, serás redirigido a la página principal.", "Aceptar");


                var Propiedades_NavigationPage = new NavigationPage(new MainPage());
                App.Current.MainPage = Propiedades_NavigationPage;

            }
            catch (Exception)
            {

                await Application.Current.MainPage.DisplayAlert("Advertencia", "Los datos introducidos son incorrectos o el usuario se encuentra inactivo.", "Aceptar");
            }
        }
        #endregion

        #region Constructor
        public LoginVistaModelo(INavigation navegar)
        {
            Navigation = navegar;
            LoginCommand = new Command(async () => await LoginUsuario());

        }
        #endregion
    }
}

