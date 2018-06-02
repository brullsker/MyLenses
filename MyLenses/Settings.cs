using Windows.Storage;

namespace MyLenses
{
    public class Settings : ObservableSettings
    {
        private static Settings settings = new Settings();
        public static Settings Default
        {
            get { return settings; }
        }

        public Settings()
            : base(ApplicationData.Current.LocalSettings)
        {
        }

        [DefaultSettingValue(Value = "Dark")]
        public string Theme
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = 2)]
        public int LensType
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = "")]
        public string LensTypeName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = "")]
        public string LensExpDate
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = "")]
        public string LeftLens
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = "")]
        public string RighttLens
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = 1)]
        public int LeftLensOperator
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = 1)]
        public int RightLensOperator
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = "dd/MM/yyyy")]
        public string DateFormat
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [DefaultSettingValue(Value = "")]
        public string Language
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}