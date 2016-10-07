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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class today : Page
    {
        public today()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pane.splitview.IsPaneOpen = !pane.splitview.IsPaneOpen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            initGrid();
            if (global.File == null)
                temp_message.Text = "没有打开文件，滚去设置";
            else
                temp_message.Text = "已打开 " + global.File.Name;
        }

        private void initGrid()
        {
            if (global.File != null)
            {
                temp_message.Text = "已打开 " + global.File.Name;

                grid1.Children.Clear();
                grid1.RowDefinitions.Clear();
                grid1.ColumnDefinitions.Clear();
                InitRows(6, grid1);
                for (int i = 2; i < 8; i++)
                {
                    int j = Convert.ToInt32(DateTime.Now.DayOfWeek) + 1;

                    TextBlock block = new TextBlock();

                    block.Text = global.Strs(global.Str_id[i, j]);
                    block.Padding = new Thickness(10);
                    block.TextWrapping = TextWrapping.Wrap;
                    block.MinHeight = 100;

                    grid1.Children.Add(block);
                    Grid.SetRow(block, i - 2);
                    //temp_message.Text = ""+ str_id[i,j] + " "+ System.Text.Encoding.Unicode.GetString(strs[str_id[i, j]]);

                    Border bd = new Border();
                    grid1.Children.Add(bd);
                    Grid.SetRow(bd, i - 2);


                }

            }
            else
            {
                temp_message.Text = "没有打开文件，滚去设置";
            }
        }

        private void InitRows(int rowCount, Grid g)
        {
            while (rowCount-- > 0)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength();
                g.RowDefinitions.Add(rd);
            }
        }
    }
}
