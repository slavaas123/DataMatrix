using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ZXing;
using ZXing.Mobile;

namespace DataMatrixSlava
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private EditText editText;
        private TextView errorTextView;
        private Button convertButton;
        private ImageView picture;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            convertButton = FindViewById<Button>(Resource.Id.convertButton);
            convertButton.Click += ConvertButton;

            picture = FindViewById<ImageView>(Resource.Id.picture);
            editText = FindViewById<EditText>(Resource.Id.editText);
            errorTextView = FindViewById<TextView>(Resource.Id.errorText);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        private void ConvertButton(object sender, EventArgs eventArgs)
        {
            if (ValidString(editText.Text) && !string.IsNullOrEmpty(editText.Text))
            {
                errorTextView.Text = "";
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.DATA_MATRIX,
                    Options = new ZXing.Datamatrix.DatamatrixEncodingOptions
                    {
                        Width = 100,
                        Height = 100,
                        Margin = 30
                    }
                };

                var image = writer.Write(editText.Text);
                picture.SetImageBitmap(image);
            }
            else
                errorTextView.Text = "Неправильный формат текста";

            var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputManager.HideSoftInputFromWindow(convertButton.WindowToken, HideSoftInputFlags.None);
        }

        private bool ValidString(string input)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
            string result = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(bytes);

            return string.Equals(input, result);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

