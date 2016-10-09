using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
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
            initGrid();

            if (Window.Current.Bounds.Width > 650)
            {
                scroll.Padding = new Thickness(70, 20, 70, 50);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            pane.pane_lv.SelectedIndex = 1;
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

            if (global.File != null)
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
                            days = index + 1;
                        }
                    }
                }
                InitRows(7, grid1);
                InitColumns(days, grid1);
                String[] days_in_week = new String[] { "Mon", "Tue", "Wed", "Thur", "Fri", "Sat", "Sun" };
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


                        try
                        {
                            int week = global.getWeekOfToday();
                            for (int k = 0; k < global.Subjects[i, j].Name.Count; k++)
                            {
                                if (global.Subjects[i, j].WeekBegin[k] <= week && global.Subjects[i, j].WeekEnd[k] >= week)
                                {
                                    if (global.Subjects[i, j].IsDanShuangZhou[k] == 0 ||
                                        global.Subjects[i, j].IsDanShuangZhou[k] == 1 && week % 2 == 1 ||
                                        global.Subjects[i, j].IsDanShuangZhou[k] == 2 && week % 2 == 0)
                                    {
                                        if (k > 0) block.Text += "\n";
                                        block.Text += "【" + global.Subjects[i, j].Name[k] + "】\n";
                                        block.Text += " 地点：" + global.Subjects[i, j].Location[k] + "\n";
                                        block.Text += " 教师：" + global.Subjects[i, j].Teacher[k] + "\n";
                                        block.Text += " 上课时间：" + global.Subjects[i, j].WeekBegin[k];
                                        block.Text += "~" + global.Subjects[i, j].WeekEnd[k];

                                        block.Text += global.Subjects[i, j].IsDanShuangZhou[k] == 1 ? "单" : global.Subjects[i, j].IsDanShuangZhou[k] == 2 ? "双" : "";
                                        block.Text += "周";
                                        block.Name = global.Subjects[i, j].Name[k];
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            block.Text = global.res[i, j];
                        }


                        //block.Text = global.res[i, j];
                        block.Padding = new Thickness(10);
                        block.TextWrapping = TextWrapping.Wrap;
                        block.Tapped += new TappedEventHandler((object sender, TappedRoutedEventArgs e) =>
                        {
                            TextBlock tb = (TextBlock)sender;
                            Frame rootFrame = Window.Current.Content as Frame;
                            rootFrame.Navigate(typeof(note), tb.Name);
                        });

                        grid1.Children.Add(block);
                        Grid.SetRow(block, i + 1);
                        Grid.SetColumn(block, j);
                        //temp_message.Text = ""+ str_id[i,j] + " "+ System.Text.Encoding.Unicode.GetString(strs[str_id[i, j]]);

                        Border bd = new Border();
                        grid1.Children.Add(bd);
                        Grid.SetRow(bd, i + 1);
                        Grid.SetColumn(bd, j);

                    }
                }
            }
            else
            {
                Toast.Label = "没有选择课表，请前往设置页";
                Toast.Show();
            }
            //temp_message.Text = global.getSetting("first_monday");
            //temp_message.Text = ApplicationData.Current.LocalFolder.Path;
            try
            {
                temp_message.Text = "\n今天是开学第" + global.getWeekOfToday() + "周";
            }
            catch (System.Exception ex)
            {
                Toast.Label = "没有设置学期第一周周一的日期，请前往设置页";
                Toast.Show();
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

        private void aaa(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
