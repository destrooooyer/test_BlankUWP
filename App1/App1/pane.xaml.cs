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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace App1
{
    public sealed partial class pane : UserControl
    {
        public pane()
        {
            this.InitializeComponent();
            if (Window.Current.Bounds.Width>650)
            {
                //pane_lv.Items.RemoveAt(0);
            }
            else
            {
                
            }
        }

        private void pane_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(pane_lv.SelectedIndex==1)
                ((Frame)Window.Current.Content).Navigate(typeof(MainPage));


            if (pane_lv.SelectedIndex == 0)
                ((Frame)Window.Current.Content).Navigate(typeof(today));

            if (pane_lv.SelectedIndex == 2)
                ((Frame)Window.Current.Content).Navigate(typeof(settings));

            splitview.IsPaneOpen = false;
        }
    }
}
