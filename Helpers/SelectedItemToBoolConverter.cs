using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GardenManagement.Helpers
{
    class SelectedItemToBoolConverter : IValueConverter
    {
        //This just checks, if a UIElement has a selected item. The Binding needs to point to UIElement.SelectedIndex
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int seletedIndex = (int)value;
            if (seletedIndex >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
