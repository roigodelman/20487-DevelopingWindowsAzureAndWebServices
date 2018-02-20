using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BlueYonder.Companion.Client.Common;
using BlueYonder.Companion.Client.DataModel;
using BlueYonder.Companion.Client.Helpers;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;

namespace BlueYonder.Companion.Client.ViewModels
{
    public class OptionsViewModel : ViewModel
    {
        private readonly ICollection<LanguageItem> _languageItems;
        public IEnumerable<LanguageItem> LanguageItems
        {
            get { return _languageItems; }
        }

        private LanguageItem _selectedLanguageItem;
        public LanguageItem SelectedLanguageItem
        {
            get { return _selectedLanguageItem; }
            set { SetProperty(ref _selectedLanguageItem, value); }
        }

        public DelegateCommand PurchaseAppCommand { get; set; }
        public DelegateCommand PurchaseMediaFeatureCommand { get; set; }

        public OptionsViewModel()
        {
            _languageItems = new ObservableCollection<LanguageItem>();

            PurchaseAppCommand = new DelegateCommand(PurchaseApp, CanExecutePurchaseApp);
            PurchaseMediaFeatureCommand = new DelegateCommand(PurchaseMediaFeature, CanExecutePurchaseMediaFeatureCommand);

        }

        public override void Initialize(Frame frame)
        {
            base.Initialize(frame);

            LoadLanguages();

            PropertyChanged += OptionsViewModel_PropertyChanged;
        }

        private void AddLanguageItem(string displayName, string languageTag)
        {
            var languageItem = new LanguageItem
            {
                DisplayName = displayName,
                LanguageTag = languageTag
            };
            _languageItems.Add(languageItem);
        }

        private void LoadLanguages()
        {
            // First show the default setting
            AddLanguageItem(ResourceHelper.ResourceLoader.GetString("LanguagePreferences"), string.Empty);

            // If there are other languages the user speaks that aren't the default, show them first
            if (ApplicationLanguages.PrimaryLanguageOverride != string.Empty || ApplicationLanguages.Languages.Count > 1)
            {
                for (var i = 0; i < ApplicationLanguages.Languages.Count; i++)
                {
                    if ((ApplicationLanguages.PrimaryLanguageOverride == string.Empty && i != 0) || (ApplicationLanguages.PrimaryLanguageOverride != string.Empty && i != 1))
                    {
                        var language = new Language(ApplicationLanguages.Languages[i]);
                        AddLanguageItem(language.DisplayName, language.LanguageTag);
                    }
                }
                AddLanguageItem("——————", string.Empty);
            }

            // Finally, add the rest of the languages the app supports
            var manifestLanguageObjects = new List<Language>();
            foreach (var languageTag in ApplicationLanguages.ManifestLanguages)
            {
                manifestLanguageObjects.Add(new Language(languageTag));
            }
            IEnumerable<Language> orderedManifestLanguageObjects = manifestLanguageObjects.OrderBy(lang => lang.DisplayName);
            foreach (var language in orderedManifestLanguageObjects)
            {
                AddLanguageItem(language.DisplayName, language.LanguageTag);
            }

            SelectedLanguageItem = _languageItems.FirstOrDefault(l => l.LanguageTag == ApplicationLanguages.PrimaryLanguageOverride);
        }

        private async void OptionsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedLanguageItem")
            {
                // Set the persistent application language override
                ApplicationLanguages.PrimaryLanguageOverride = SelectedLanguageItem.LanguageTag;

                var msg = new Windows.UI.Popups.MessageDialog(ResourceHelper.ResourceLoader.GetString("ChangeLanguageMessage"));
                await msg.ShowAsync();
            }
        }

        private async void PurchaseApp(object obj)
        {
			// TODO: Module 12: Exercise 1: Task 3.2: Implement the PurchaseApp method
            await LicenseManager.Instance.PurchaseAppAsync();
        }

        // TODO: Module 12: Exercise 1: Task 3.2: Implement the CanExecutePurchaseApp method
        private bool CanExecutePurchaseApp(object obj)
        {
		 // TODO: Module 12: Exercise 1: Task 3.2: Implement the CanExecutePurchaseApp method
            return LicenseManager.Instance.IsTrialLicense;
        }


        private async void PurchaseMediaFeature(object obj)
        {
        // TODO: Module 12: Exercise 2: Task 1.3: Implement the PurchaseMediaFeature method
            await LicenseManager.Instance.PurchaseMediaFeatureAsync();
        }


        private bool CanExecutePurchaseMediaFeatureCommand(object obj)
        {
        // TODO: Module 12: Exercise 2: Task 1.3: Implement the CanExecutePurchaseMediaFeature method
            return !LicenseManager.Instance.IsMediaFeatureEnabled;
        }
    }
}
