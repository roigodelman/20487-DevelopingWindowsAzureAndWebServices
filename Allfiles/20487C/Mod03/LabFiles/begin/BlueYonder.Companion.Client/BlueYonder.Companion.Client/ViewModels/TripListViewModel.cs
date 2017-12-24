using BlueYonder.Companion.Client.Common;
using BlueYonder.Companion.Client.DataModel;
using BlueYonder.Companion.Client.Helpers;
using BlueYonder.Companion.Client.Views.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;

namespace BlueYonder.Companion.Client.ViewModels
{
    public class TripListViewModel : ViewModel
    {
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

        private ObservableCollection<TripCategory> _tripCategories;
        public ObservableCollection<TripCategory> TripCategories
        {
            get { return this._tripCategories; }
            set { this.SetProperty(ref this._tripCategories, value); }
        }

        private Reservation _currentTrip;
        public Reservation CurrentTrip
        {
            get { return this._currentTrip; }
            set { this.SetProperty(ref this._currentTrip, value); }
        }

        private bool _working;
        public bool Working
        {
            get { return this._working; }
            set { this.SetProperty(ref this._working, value); }
        }

        private WeatherForecast _weather;
        public WeatherForecast Weather
        {
            get { return this._weather; }
            set { this.SetProperty(ref this._weather, value); }
        }

        private bool _isLoginCommandVisible;
        public bool IsLoginCommandVisible
        {
            get { return this._isLoginCommandVisible; }
            set { this.SetProperty(ref this._isLoginCommandVisible, value); }
        }

        private bool _isTripDataVisible;
        public bool IsTripDataVisible
        {
            get { return this._isTripDataVisible; }
            set { this.SetProperty(ref this._isTripDataVisible, value); }
        }

        private bool _isGlanceVisible;
        public bool IsGlanceVisible
        {
            get { return this._isGlanceVisible; }
            set { this.SetProperty(ref this._isGlanceVisible, value); }
        }

        private TravelerInfoViewModel _travelerInfo;
        public TravelerInfoViewModel TravelerInfo
        {
            get { return this._travelerInfo; }
            set { this.SetProperty(ref this._travelerInfo, value); }
        }

        private bool _forceRefresh;
        public bool ForceRefresh
        {
            get { return _forceRefresh; }
            set { this.SetProperty(ref this._forceRefresh, value); }
        }

        private async void InitCurrentTrip(bool isLoggedIn)
        {
            var currentTripCategory = TripCategories.FirstOrDefault(t => t.Type == CategoryType.CurrentTrip);
            var currentTrip =
                !isLoggedIn || currentTripCategory == null || currentTripCategory.Items.Count == 0
                    ? null
                    : currentTripCategory.Items[0];

            WeatherForecast weather = null;
            if (currentTrip != null)
            {
                var flightInfo = currentTrip.DepartureFlight.FlightInfo;
                var locationId = flightInfo.Flight.Destination.LocationId;
                var departure = flightInfo.Departure ?? DateTime.Now;

                weather = await _data.GetWeatherForecastByIdAsync(locationId, departure);

                ApplicationData.Current.LocalSettings.Values["locationId"] = locationId;
                ApplicationData.Current.LocalSettings.Values["locationName"] = flightInfo.Flight.Source.City;
                ApplicationData.Current.LocalSettings.Values["departure"] = String.Format("{0:dd/MM/yyyy}", departure);
            }

            CurrentTrip = currentTrip;
            Weather = weather;
            IsGlanceVisible = currentTrip != null;
        }

        private readonly DataManager _data;

        public TripListViewModel()
        {
            this._tripCategories = new ObservableCollection<TripCategory>();
            this._data = new DataManager();

            this.TravelerInfo = new TravelerInfoViewModel();

            IsLoginCommandVisible = true;
            IsTripDataVisible = false;
            IsGlanceVisible = false;

            SearchCommand = new DelegateCommand(ShowSearch);
            LoginCommand = new DelegateCommand(Login);
            LogoutCommand = new DelegateCommand(Logout);
        }

        public override async void Initialize(Frame frame)
        {
            base.Initialize(frame);

            TravelerInfo.Initialize(frame);

            var isLoggedIn = UserAuth.Instance.IsLoggedIn;

            IsTripDataVisible = isLoggedIn;
            IsLoginCommandVisible = !isLoggedIn;

            await LoadTrips(isLoggedIn);
        }

        private async Task LoadTrips(bool isLoggedIn)
        {
            if (isLoggedIn)
            {
                var cacheItem = await CacheManager.Load<ObservableCollection<TripCategory>>(CacheType.Categories);
                if (cacheItem != null && cacheItem.Data != null)
                {
                    TripCategories = (ObservableCollection<TripCategory>) cacheItem.Data;
                }

                if (cacheItem == null || cacheItem.Data == null || cacheItem.RequireRefresh)
                {
                    await GetTravelerFlightsFromServer(ForceRefresh);
                }
            }
            InitCurrentTrip(isLoggedIn);
        }

        private async Task GetTravelerFlightsFromServer(bool forceRefresh)
        {
            Working = true;
            TripCategories = await ReservationDataFetcher.Instance.GetCategoriesAsync(forceRefresh);
            Working = false;
        }

        private void ShowSearch(object parameter)
        {
            SearchPane.GetForCurrentView().Show();
        }

        private async void Login(object parameter)
        {
        }

        private void Logout(object parameter)
        {
        }
    }
}
