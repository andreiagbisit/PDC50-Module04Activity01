using System.Runtime.CompilerServices;

namespace Module04Activity01
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null) 
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High
                    });
                }

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";

                    //Get Geolocation = Get Address from Lat and Long

                    var placemarks = await Geocoding.Default.GetPlacemarksAsync
                        (location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        AddressLabel.Text = $"Address: {placemark.Thoroughfare}," +
                            $"{placemark.Locality}," +
                            $"{placemark.AdminArea}," +
                            $"{placemark.PostalCode}," +
                            $"{placemark.CountryName}";
                    }

                    else
                    {
                        AddressLabel.Text = "Unable to determine the address";
                    }

                    // end of Geocoding
                }
                else
                {
                    LocationLabel.Text = "Unable to get location";
                }
            }
            catch (Exception ex)
            {
                LocationLabel.Text = $"Error: {ex.Message}";
            }
        }

        private async void OnCapturePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    // Capture a photo using MediaPicker
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null) 
                    {
                        await LoadPhotoAsync(photo);
                    }
                }

                else
                {

                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occured. {ex.Message}", "OK");
            }

        }

            private async Task LoadPhotoAsync(FileResult photo)
            {
                if (photo == null)
                return;

                Stream stream = await photo.OpenReadAsync();

                CaptureImage.Source = ImageSource.FromStream(()=>stream);

            }
    }

}
