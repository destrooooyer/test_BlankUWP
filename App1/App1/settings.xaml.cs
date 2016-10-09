using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
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
            int open_s = await global.openfile();
            if (open_s == -1)
            {
                Toast.Label = "文件被占用或无权访问";
                Toast.Show();
            }
            else if (open_s == -2)
            {
                Toast.Label = "文件读取过程出错";
                Toast.Show();
            }
            else if (open_s == -3)
                chooseXls.Content = "未选择课表，请选择课表";
            else if (global.File == null)
                chooseXls.Content = "未选择课表，请选择课表";
            else
            {
                chooseXls.Content = "已打开 " + global.File.Name;
                Toast.Label = "已打开 " + global.File.Name;
                Toast.Show();
            }
                
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DateTimeOffset? d = date.Date;
            long first_monday = d.HasValue ? d.Value.ToUnixTimeSeconds() : 0;
            global.setSetting("first_monday", first_monday.ToString());
            global.saveSetting();

            global.setSetting("firstMondayFileTime", date.Date.Value.Date.ToFileTime().ToString());
            global.saveSetting();
        }

    }
}
