namespace Winkeladvokat
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter()
        {
            this.FalseEquivalent = Visibility.Collapsed;
        }

        /// <summary>
        /// FalseEquivalent (default : Visibility.Collapsed)
        /// </summary>
        /// <value>The false equivalent.</value>
        public Visibility FalseEquivalent { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool?)value;

            if (parameter is bool)
            {
                if ((bool)parameter)
                {
                    boolValue = !boolValue;
                }
            }

            return boolValue.GetValueOrDefault() ? Visibility.Visible : this.FalseEquivalent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibilityValue = (Visibility)value;
            if (visibilityValue == Visibility.Hidden)
            {
                visibilityValue = Visibility.Collapsed;
            }

            if (parameter is bool)
            {
                if ((bool)parameter)
                {
                    visibilityValue = visibilityValue == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return visibilityValue == Visibility.Visible;
        }
    }
}
