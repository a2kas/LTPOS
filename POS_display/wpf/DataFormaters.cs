using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace POS_display.wpf
{
    public class DateFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                DateTime date = helpers.getXMLDateOnly(value.ToString());
                return date.ToString("yyyy-MM-dd");
            }
            catch (Exception e)
            {
                return "other";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class TypeFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                switch (value.ToString())
                {
                    case "va":
                        return "Vaistas";
                    case "mpp":
                        return "Medicininės pagalbos priemonės";
                    case "vv":
                        return "Vardinis vaistas";
                    case "ev":
                        return "Ekstemporalus vaistas";
                    case "mp":
                        return "Maisto papildas";
                    default:
                        return value.ToString();
                };
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ERecipeGenericNameFormat : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values[0].ToString() == "ev")
                    return "Gaminami vaistai";
                if (!string.IsNullOrWhiteSpace(values[1]?.ToString()))
                    return values[1].ToString();
                return values[2]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BusyToCursorConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is bool))
                return System.Windows.Input.Cursors.Arrow;

            var isBusy = (bool)value;

            if (isBusy)
                return System.Windows.Input.Cursors.Wait;
            else
                return System.Windows.Input.Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string result = value.ToString().Replace(',', '.');
                if (value.ToDecimal() == Math.Round(value.ToDecimal()) && result.IndexOf('.') > 0)
                    result = result.Substring(0, result.IndexOf('.'));

                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isVisible = value?.ToBool() ?? false;
            if (isVisible)
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UpperCaseConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class StyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Style))
            {
                throw new InvalidOperationException("The target must be a Style");
            }

            var styleProperty = parameter as string;
            if (value == null || styleProperty == null)
            {
                return null;
            }

            string styleValue = value.GetType()
                .GetProperty(styleProperty)
                .GetValue(value, null)
                .ToString();
            if (styleValue == null)
            {
                return null;
            }

            Style newStyle = (Style)Application.Current.TryFindResource(styleValue);
            return newStyle;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueBool = value?.ToBool() ?? false;
            if (valueBool)
                return "*";
            else
                return "";
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToCheapestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueBool = value?.ToBool() ?? false;
            if (valueBool)
                return "(P)";
            else
                return "";
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}