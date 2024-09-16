using System;
using System.Threading.Tasks;
using Firebase.Auth;
using OrdenaYaVersion1.Views.AccesoApp;
using OrdenaYaVersion1.Models;
using Xamarin.Forms;
using OrdenaYaVersion1.Conexion;
using System.Text.RegularExpressions;

namespace OrdenaYaVersion1.ViewModels
{
	public class RegisterVistaModelo : BaseVistaModelo
	{
        #region Atributos
        private string email;
        private string clave;
        private string confirmClave;
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
        public string entryConfirmPassword
        {
            get { return confirmClave; }
            set { SetValue(ref confirmClave, value); }
        }
        #endregion

        #region Command
        public Command RegisterCommand { get; }
        #endregion

        #region Metodo
        public async Task RegisterUsuario()
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(clave) || string.IsNullOrEmpty(confirmClave))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, rellena todos los campos.", "Aceptar");
                return;
            }

            if (!IsValidPassword(clave))
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "La contraseña debe tener al menos 8 caracteres, una letra mayúscula, un número y un carácter especial.", "Aceptar");
                return;
            }

            if (clave != confirmClave)
            {
                await App.Current.MainPage.DisplayAlert("Advertencia", "Las contraseñas no coinciden.", "Aceptar");
                return;
            }

            if (!IsValidEmail(email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El formato del correo electrónico no es válido.", "Aceptar");
                return;
            }

            var objusuario = new UserModel()
            {
                EmailField = email,
                PasswordField = clave
            };

            try
            {
                var autenticacion = new FirebaseAuthProvider(new FirebaseConfig(DBConn.WepApyAuthentication));
                var authuser = await autenticacion.CreateUserWithEmailAndPasswordAsync(objusuario.EmailField.ToString(), objusuario.PasswordField.ToString());
                string obternertoken = authuser.FirebaseToken;

                await Application.Current.MainPage.DisplayAlert("Éxito", "Registro exitoso, serás redirigido al inicio de sesión.", "Aceptar");

                var Propiedades_NavigationPage = new NavigationPage(new LoginPage());
                Application.Current.MainPage = Propiedades_NavigationPage;

            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El correo electrónico ya está en uso. Por favor, inicia sesión o utiliza otro correo.", "Aceptar");
            }
        }


        private bool IsValidPassword(string password)
        {

            //La contraseña debe tener al menos 8 caracteres, una letra mayúscula, un número y un carácter especial
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,}$");

            return regex.IsMatch(password);
        }

        private bool IsValidEmail(string email)
        {
            // Validar el formato del correo electrónico
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        #endregion

        #region Constructor
        public RegisterVistaModelo(INavigation navegar)
        {
            Navigation = navegar;
            RegisterCommand = new Command(async () => await RegisterUsuario());
        }
        #endregion
    }
}

