using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NguyenThanhBinhWPF.Utils
{
    public static class WindowBehaviorUtils
    {
        public static void CancelWindowClosing(this Window window, object sender, CancelEventArgs e)
        {
            e.Cancel = true; // Cancel event Closing
            window.Hide();// hide the window
            window.Owner?.Show();
        }
        public static void ShowAndSetOwnerWindow(this Window window, Window childWidow)
        {
            childWidow.Show();
            childWidow.Owner = window;
        }
        public static void OnPropertyChanged(this INotifyPropertyChanged? notifyPropertyChanged, PropertyChangedEventHandler propertyChanged, [CallerMemberName] string propertyName = "")
        {
            propertyChanged?.Invoke(notifyPropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }
}
