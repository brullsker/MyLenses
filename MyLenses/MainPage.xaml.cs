using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Popups;
using Microsoft.Toolkit;
using Microsoft.Toolkit.Extensions;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.System;
using Windows.Globalization;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace MyLenses
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DateTime dtExpDate = new DateTime();

        public MainPage()
        {
            this.InitializeComponent();
            LoadingControl.Visibility = Visibility.Visible;
            if (Settings.Default.Theme == "Light") ThemeBox.SelectedIndex = 0;
            if (Settings.Default.Theme == "Dark") ThemeBox.SelectedIndex = 1;
            if (Settings.Default.LeftLensOperator == 0) LeftLensOperatorTextBloc.Text = "+";
            if (Settings.Default.LeftLensOperator == 1) LeftLensOperatorTextBloc.Text = "-";
            if (Settings.Default.RightLensOperator == 0) RightLensOperatorTextBloc.Text = "+";
            if (Settings.Default.RightLensOperator == 1) RightLensOperatorTextBloc.Text = "-";
            if (Settings.Default.DateFormat == "MM/dd/yyyy") DateFormatBox.SelectedIndex = 0;
            if (Settings.Default.DateFormat == "dd.MM.yyyy") DateFormatBox.SelectedIndex = 1;
            if (Settings.Default.Language == "en-US") LangBox.SelectedIndex = 0;
            if (Settings.Default.Language == "de-DE") LangBox.SelectedIndex = 1;

            About_AppTitleText.Text = Package.Current.DisplayName.ToString();
            About_AppVersionText.Text = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsEditSplitView.IsPaneOpen == false)
            {
#if WINDWS_PHONE
                await MainGrid.Fade(value: 0f, duration: 100, delay: 0, easingType: EasingType.Linear).StartAsync();
                SettingsGrid.Visibility = Visibility.Collapsed;
                EditGrid.Opacity = 100;
                EditGrid.Visibility = Visibility.Visible;
                SettingsEditSplitView.IsPaneOpen = true;
                await MainGrid.Fade(value: 0.5f, duration: 250, delay: 350, easingType: EasingType.Linear).StartAsync();
                EditButton.IsChecked = true;
#else
                SettingsGrid.Visibility = Visibility.Collapsed;
                EditGrid.Opacity = 100;
                EditGrid.Visibility = Visibility.Visible;
                SettingsEditSplitView.IsPaneOpen = true;
                await MainGrid.Fade(value: 0.5f, duration: 250, easingType: EasingType.Linear).StartAsync();
                EditButton.IsChecked = true;
#endif
            }
            else if (SettingsEditSplitView.IsPaneOpen == true)
            {
                if (SettingsGrid.Visibility == Visibility.Visible)
                {
                    SettingsButton.IsChecked = false;
                    AboutToggleButton.IsChecked = false;
                    EditGrid.Opacity = 0;
                    await SettingsGrid.Fade(value: 0f, duration: 100, delay: 0, easingType: EasingType.Linear).StartAsync();
                    SettingsGrid.Visibility = Visibility.Collapsed;
                    EditGrid.Visibility = Visibility.Visible;
                    await EditGrid.Fade(value: 1f, duration: 100, delay: 0, easingType: EasingType.Linear).StartAsync();
                }
                else Grid_Tapped(sender, e);
            }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (SettingsEditSplitView.IsPaneOpen == false)
            {
#if WINDOWS_PHONE
                await MainGrid.Fade(value: 0f, duration: 100, delay: 0, easingType: EasingType.Linear).StartAsync();
                EditGrid.Visibility = Visibility.Collapsed;
                SettingsGrid.Opacity = 100;
                SettingsGrid.Visibility = Visibility.Visible;
                SettingsEditSplitView.IsPaneOpen = true;
                await MainGrid.Fade(value: 0.5f, duration: 250, delay: 350, easingType: EasingType.Linear).StartAsync();
                SettingsButton.IsChecked = true;
#else
                EditGrid.Visibility = Visibility.Collapsed;
                SettingsGrid.Opacity = 100;
                SettingsGrid.Visibility = Visibility.Visible;
                SettingsEditSplitView.IsPaneOpen = true;
                await MainGrid.Fade(value: 0.5f, duration: 250, easingType: EasingType.Linear).StartAsync();
                SettingsButton.IsChecked = true;
#endif
            }
            else if (SettingsEditSplitView.IsPaneOpen == true)
            {
                if (EditGrid.Visibility == Visibility.Visible)
                {
                    EditButton.IsChecked = false;
                    SettingsGrid.Opacity = 0;
                    await EditGrid.Fade(value: 0f, duration: 100, delay: 0, easingType: EasingType.Linear).StartAsync();
                    EditGrid.Visibility = Visibility.Collapsed;
                    SettingsGrid.Visibility = Visibility.Visible;
                    await SettingsGrid.Fade(value: 1f, duration: 100, delay: 0, easingType: EasingType.Linear).StartAsync();
                }
                else Grid_Tapped(sender, e);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeBox.SelectedIndex == 0) MyLenses.Settings.Default.Theme = "Light";
            if (ThemeBox.SelectedIndex == 1) Settings.Default.Theme = "Dark";
        }

        private async void Grid_Tapped(object sender, RoutedEventArgs e)
        {
            if (SettingsEditSplitView.IsPaneOpen == true)
            {
#if WINDOWS_PHONE
                await MainGrid.Fade(value: 0f, duration: 50, delay: 0, easingType: EasingType.Linear).StartAsync();
                SettingsEditSplitView.IsPaneOpen = false;
                EditButton.IsChecked = false;
                SettingsButton.IsChecked = false;
                await MainGrid.Fade(value: 1f, duration: 250, delay: 0, easingType: EasingType.Linear).StartAsync();
                EditButton.IsChecked = false;
                SettingsButton.IsChecked = false;
#else
                SettingsEditSplitView.IsPaneOpen = false;
                EditButton.IsChecked = false;
                SettingsButton.IsChecked = false;
                await MainGrid.Fade(value: 1f, duration: 250, delay: 0, easingType: EasingType.Linear).StartAsync();
                EditButton.IsChecked = false;
                SettingsButton.IsChecked = false;
#endif
                AboutToggleButton.IsChecked = false;
            }
        }

        private async void EditLens_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double addDays = new double();
            if (EditLens_Type.SelectedIndex == 0) addDays = 1;
            if (EditLens_Type.SelectedIndex == 1) addDays = 7;
            if (EditLens_Type.SelectedIndex == 2) addDays = 14;
            if (EditLens_Type.SelectedIndex == 3) addDays = 30;
            if (EditLens_Type.SelectedIndex == 4) addDays = 365;
            
            dtExpDate = DateTime.Now;
            dtExpDate = dtExpDate.AddDays(addDays);

            EditLens_ExpDate_TextBlock.Text = dtExpDate.ToString(Settings.Default.DateFormat);
        }

        private async void AcceptChangesButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.LensType = EditLens_Type.SelectedIndex;
            if (EditLens_Type.SelectedIndex == 0) Settings.Default.LensTypeName = EditLens_Type_1Day.Content.ToString();
            if (EditLens_Type.SelectedIndex == 1) Settings.Default.LensTypeName = EditLens_Type_7Day.Content.ToString();
            if (EditLens_Type.SelectedIndex == 2) Settings.Default.LensTypeName = EditLens_Type_14Day.Content.ToString();
            if (EditLens_Type.SelectedIndex == 3) Settings.Default.LensTypeName = EditLens_Type_30Day.Content.ToString();
            if (EditLens_Type.SelectedIndex == 4) Settings.Default.LensTypeName = EditLens_Type_365Day.Content.ToString();
            Settings.Default.LensExpDate = EditLens_ExpDate_TextBlock.Text;
            Settings.Default.LeftLensOperator = EditLens_LeftLens_Operator.SelectedIndex;
            Settings.Default.LeftLens = EditLens_LeftLens_Value.Text;
            Settings.Default.RightLensOperator = EditLens_RightLens_Operator.SelectedIndex;
            Settings.Default.RighttLens = EditLens_RightLens_Value.Text;

            // Create appointment
            var appointment = new Windows.ApplicationModel.Appointments.Appointment();
            var date = dtExpDate.Date;
            var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, timeZoneOffset);
            appointment.StartTime = startTime;

            appointment.Subject = AppointmentSubjectText.Text;
            appointment.AllDay = true;
            appointment.Reminder = TimeSpan.FromHours(0);

            string appointmenID = await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, MainPage.GetElementRect(sender as FrameworkElement));
            
            Grid_Tapped(sender, e);
            ContentDialog messageDialog = new ContentDialog();
            messageDialog.Title = SuccessTextTemplate.Text;
            if (appointmenID != string.Empty) { messageDialog.Content = EditSuccessTextTemplate.Text + ", " + AppointmentReminderSet.Text; messageDialog.PrimaryButtonText = CloseTemplate.Text; }
            else { messageDialog.Content = EditSuccessTextTemplate.Text + ", " + AppointmentNoReminder.Text; messageDialog.PrimaryButtonText = CloseTemplate.Text; }
            await messageDialog.ShowAsync();
            Frame.Navigate(typeof(MainPage));
        }

        private async void DiscardChangesButton_Click(object sender, RoutedEventArgs e)
        {
            EditLens_Type.SelectedIndex = Settings.Default.LensType;
            EditLens_ExpDate_TextBlock.Text = Settings.Default.LensExpDate;
            EditLens_LeftLens_Operator.SelectedIndex = Settings.Default.LeftLensOperator;
            EditLens_LeftLens_Value.Text = Settings.Default.LeftLens;
            EditLens_RightLens_Operator.SelectedIndex = Settings.Default.RightLensOperator;
            EditLens_RightLens_Value.Text = Settings.Default.RighttLens;
            Grid_Tapped(sender, e);
            ContentDialog messageDialog = new ContentDialog();
            messageDialog.Title = SuccessTextTemplate.Text; messageDialog.Content = DiscardSuccessTextTemplate.Text; messageDialog.PrimaryButtonText = CloseTemplate.Text;
            await messageDialog.ShowAsync();
        }

        public static Rect GetElementRect(Windows.UI.Xaml.FrameworkElement element)
        {
            Windows.UI.Xaml.Media.GeneralTransform transform = element.TransformToVisual(null);
            Point point = transform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private async void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SettingsEditSplitView.OpenPaneLength = SettingsEditSplitView.OpenPaneLength + 310;
            AboutSplitView.IsPaneOpen = true;
            await AboutButton.Rotate(value: 180, duration: 125, centerX: 34, centerY: 19, delay: 0, easingType: EasingType.Linear).StartAsync();
        }

        private async void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsEditSplitView.OpenPaneLength = SettingsEditSplitView.OpenPaneLength - 310;
            AboutSplitView.IsPaneOpen = false;
            await AboutButton.Rotate(value: 0, duration: 125, centerX: 34, centerY: 19, delay: 0, easingType: EasingType.Linear).StartAsync();
        }

        private void AboutSplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            AboutToggleButton.IsChecked = false;
        }

        private void DateFormatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateFormatBox.SelectedIndex == 0) Settings.Default.DateFormat = "MM/dd/yyyy";
            if (DateFormatBox.SelectedIndex == 1) Settings.Default.DateFormat = "dd.MM.yyyy";
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (LangBox.SelectedIndex == 0) Settings.Default.Language = "en-US";
            if (LangBox.SelectedIndex == 1) Settings.Default.Language = "de-DE";
        }

        private async void DevContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(DevEmailBtn))
            {
                Uri uri = new Uri("mailto:brullsker@outlook.de");
                await Launcher.LaunchUriAsync(uri);
            }
            if (sender.Equals(DevTgBtn))
            {
                Uri uri = new Uri("https://t.me/brullsker");
                await Launcher.LaunchUriAsync(uri);
                
            }
            if (sender.Equals(DevTwitterBtn))
            {
                Uri uri = new Uri("https://t.co/brullsker");
                await Launcher.LaunchUriAsync(uri);
            }
        }
    }
}
