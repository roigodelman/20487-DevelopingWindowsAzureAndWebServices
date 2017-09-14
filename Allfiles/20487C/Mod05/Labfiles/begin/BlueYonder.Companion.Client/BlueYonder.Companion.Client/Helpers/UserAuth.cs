using BlueYonder.Companion.Client.Common;
using BlueYonder.Companion.Client.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BlueYonder.Companion.Client.Helpers
{
    public class UserAuth : BindableBase
    {
        private readonly DataManager _data;
        private readonly Settings _settings;

        private string _userName;
        public string UserName
        {
            get { return this._userName; }
            set { this.SetProperty(ref this._userName, value); }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return this._isLoggedIn; }
            set { this.SetProperty(ref this._isLoggedIn, value); }
        }

        private bool _busy;
        public bool Busy
        {
            get { return this._busy; }
            set { this.SetProperty(ref this._busy, value); }
        }

        private Traveler _traveler;
        public Traveler Traveler
        {
            get { return this._traveler; }
            set { this.SetProperty(ref this._traveler, value); }
        }

        private UserAuth()
        {
            _settings = new Settings(SettingsType.Local);
            _data = new DataManager();
        }

        public async Task<LoginResult> Login()
        {
            if (this.IsLoggedIn)
                return new LoginResult(true);

            this.Busy = true;

            try
            {
                await VerifyConnectionExists();

                this.IsLoggedIn = await LoadTraveler();

                return new LoginResult(this.IsLoggedIn);
            }
            catch (Exception ex)
            {
                return new LoginResult(false, ex);
            }
            finally
            {
                this.Busy = false;
            }
        }

        public void Logout()
        {
            this.IsLoggedIn = false;
            _settings.Remove(Constants.TravelerId);
        }

        private async Task VerifyConnectionExists()
        {
            var errorMessage = Accessories.resourceLoader.GetString("LoginErrorNoInternetConnection");

            var isConnected = await NetworkManager.CheckInternetConnection(true, errorMessage);

            if(!isConnected)
            {
                throw new InvalidOperationException(errorMessage);
            }
        }

        private async Task<bool> LoadTraveler()
        {
            _settings.Remove(Constants.TravelerId);

            var traveler = await _data.GetTravelerAsync() ?? await _data.CreateTravelerAsync();
            if (traveler != null)
            {
                _settings.Add(Constants.TravelerId, traveler.TravelerId.ToString());
            }

            this.Traveler = traveler;

            return traveler != null;
        }

        private static UserAuth _instance;
        public static UserAuth Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserAuth();
                }
                return _instance;
            }
        }
    }
}
