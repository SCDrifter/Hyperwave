using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hyperwave
{
    static class Extensions
    {


        public static object GetObjectAtPoint<ItemContainer>(this ItemsControl control, Point p)
        where ItemContainer : DependencyObject
        {
            // ItemContainer - can be ListViewItem, or TreeViewItem and so on(depends on control)
            ItemContainer obj = GetContainerAtPoint<ItemContainer>(control, p);
            if (obj == null)
                return null;

            return control.ItemContainerGenerator.ItemFromContainer(obj);
        }

        public static ItemContainer GetContainerAtPoint<ItemContainer>(this ItemsControl control, Point p)
        where ItemContainer : DependencyObject
        {
            HitTestResult result = VisualTreeHelper.HitTest(control, p);
            DependencyObject obj = result.VisualHit;

            while (VisualTreeHelper.GetParent(obj) != null && !(obj is ItemContainer))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            // Will return null if not found
            return obj as ItemContainer;
        }

        public static Uri ImageUrlToUri(this string text,UriKind type)
        {
            Uri uri;
            if (text.StartsWith("@://"))
                uri = new Uri(text.Substring(3), UriKind.Relative);
            else
                uri = new Uri(text, type);

            return uri;
        }

        public static Uri SoundUrlToUri(this string text, UriKind type)
        {
            Uri uri;
            if (text.StartsWith("@://"))
                uri = new Uri(text.Substring(4), UriKind.Relative);
            else
                uri = new Uri(text, type);

            return uri;
        }
    }
}
