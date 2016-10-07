using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            pane.pane_lv.SelectedIndex = 1;
            readSaved();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            initGrid();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await global.openfile();
            initGrid();
        }

        private void initGrid()
        {

            if (global.state==1)
            {
                temp_message.Text = "已打开 " + global.File.Name;

                grid1.Children.Clear();
                grid1.RowDefinitions.Clear();
                grid1.ColumnDefinitions.Clear();
                int days = 5;
                for (int index = 5; index < 7; index++)
                {
                    for (int jndex = 0; jndex < 6; jndex++)
                    {
                        if (global.res[jndex, index].Length > 0)
                        {
                            days = index+1;
                        }
                    }
                }
                InitRows(6, grid1);
                InitColumns(days, grid1);
                String[] days_in_week = new String[] {"Mon","Tue","Wed","Thur","Fri","Sat","Sun"};
                for (int j = 0; j < days; j++)
                {
                    TextBlock block = new TextBlock();
                    block.Text = days_in_week[j];
                    block.Padding = new Thickness(10);
                    block.TextWrapping = TextWrapping.Wrap;

                    grid1.Children.Add(block);
                    Grid.SetRow(block, 0);
                    Grid.SetColumn(block, j);
                    Border bd = new Border();
                    grid1.Children.Add(bd);
                    Grid.SetRow(bd, 0);
                    Grid.SetColumn(bd, j);
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < days; j++)
                    {
                        TextBlock block = new TextBlock();

                        block.Text = global.res[i, j];
                        block.Padding = new Thickness(10);
                        block.TextWrapping = TextWrapping.Wrap;

                        grid1.Children.Add(block);
                        Grid.SetRow(block, i+1);
                        Grid.SetColumn(block, j);
                        //temp_message.Text = ""+ str_id[i,j] + " "+ System.Text.Encoding.Unicode.GetString(strs[str_id[i, j]]);

                        Border bd = new Border();
                        grid1.Children.Add(bd);
                        Grid.SetRow(bd, i+1);
                        Grid.SetColumn(bd, j);

                    }
                }
            }
            else
            {
                temp_message.Text = "没有打开文件，滚去设置";
            }
        }

        private async void readSaved()
        {
            if (global.File == null)
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile saved = await folder.TryGetItemAsync("saved.xls") as StorageFile;
                if (saved != null)
                {
                    global.File = saved;
                    await global.readXls();
                    global.state = 1;
                }
            }
            initGrid();
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
        private void InitColumns(int colCount, Grid g)
        {
            while (colCount-- > 0)
            {
                ColumnDefinition rd = new ColumnDefinition();
                rd.Width = new GridLength(1, GridUnitType.Star);
                g.ColumnDefinitions.Add(rd);
            }
        }

        private void temp_message_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pane.splitview.IsPaneOpen = !pane.splitview.IsPaneOpen;
        }
    }
}
