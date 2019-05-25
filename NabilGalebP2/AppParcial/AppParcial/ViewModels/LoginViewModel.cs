using System;
using System.Collections.Generic;
using System.Text;

namespace AppParcial.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Services;
    using Xamarin.Forms;
    using global::AppParcial.Models;
    using Views;

    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        string note;
        bool isrunning;
        bool isenabled;
        ApiService apiService;
        #endregion

        #region Properties
        public string Note
        {
            get
            {
                return this.note;
            }
            set
            {
                SetValue(ref this.note, value);
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isrunning;
            }
            set
            {
                SetValue(ref this.isrunning, value);
            }

        }
        public bool IsEnabled
        {
            get
            {
                return this.isenabled;
            }
            set
            {
                SetValue(ref this.isenabled, value);
            }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(cmdLogin);
            }
        }

        private async void cmdLogin()
        {
            if (String.IsNullOrEmpty(note))
            {
                await App.Current.MainPage.DisplayAlert("Email empty",
                                "Please enter your email",
                                "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var conexion = await this.apiService.CheckConnection();
            if (!conexion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   conexion.Message,
                   "Accept");
                return;
            }

            TokenResponse token = await this.apiService.GetToken(
                  "https://notesplc.azurewebsites.net",
                  this.note);
            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   "Something was wrong, please try later.",
                   "Accept");
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "ERROR",
                   token.ErrorDescription,
                   "Accept");

                return;
            }

            MainViewModel mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;

            Application.Current.MainPage = new NavigationPage(new ProductPage());
            IsRunning = false;
            IsEnabled = true;
        }
        #endregion

        public LoginViewModel()
        {

        }
    }
}
