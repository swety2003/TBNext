using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TB.Shared.Helpers
{
    public static class ClassHelper
    {
        public static readonly DependencyProperty ClassProperty =
            DependencyProperty.RegisterAttached("Class", typeof(string), typeof(ClassHelper), new UIPropertyMetadata(null));

        public static void SetClass(DependencyObject element, string value)
        {
            element.SetValue(ClassProperty, value);
        }

        public static string GetClass(DependencyObject element)
        {
            return (string)element.GetValue(ClassProperty);
        }
    }

}
