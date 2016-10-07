﻿using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class settings : Page
    {
        public settings()
        {
            this.InitializeComponent();
            pane.pane_lv.SelectedIndex = 2;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pane.splitview.IsPaneOpen = !pane.splitview.IsPaneOpen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (global.File == null)
                chooseXls.Content = "未选择课表，请选择课表";
            else
                chooseXls.Content = "已打开 " + global.File.Name;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await global.openfile();
            if (global.File == null)
                chooseXls.Content = "未选择课表，请选择课表";
            else
                chooseXls.Content = "已打开 " + global.File.Name;
        }
    }
}
