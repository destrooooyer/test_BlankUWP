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
            pane.pane_lv.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pane.splitview.IsPaneOpen = !pane.splitview.IsPaneOpen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            initGrid();
            //             if (global.File == null)
            //                 temp_message.Text = "没有打开文件，滚去设置";
            //             else
            //                 temp_message.Text = "已打开 " + global.File.Name;
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
                for (int i = 0; i < 6; i++)
                {
                    int j = Convert.ToInt32(DateTime.Now.DayOfWeek) - 1;
                    if (j < 0) j = 6;
                    TextBlock block = new TextBlock();
                    block.HorizontalAlignment = HorizontalAlignment.Center;
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
                                    block.Text += "【" + global.Subjects[i, j].Name[k] + "】\n";

                                    block.Text += global.Subjects[i, j].Location[k]+" ";
                                    block.Text += global.Subjects[i, j].WeekBegin[k];
                                    block.Text += "~" + global.Subjects[i, j].WeekEnd[k];
                                    block.Text += global.Subjects[i, j].IsDanShuangZhou[k] == 1 ? "单" : global.Subjects[i, j].IsDanShuangZhou[k] == 2 ? "双" : "";
                                    block.Text += "周";
                                    block.Text += "\n 教师：" + global.Subjects[i, j].Teacher[k];
                                    block.Name = global.Subjects[i, j].Name[k];
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        block.Text = global.res[i, j];
                    }
                    block.Padding = new Thickness(10);
                    block.TextWrapping = TextWrapping.Wrap;
                    block.MinHeight = 80;

                    block.Tapped += new TappedEventHandler((object sender, TappedRoutedEventArgs e) =>
                    {
                        TextBlock tb = (TextBlock)sender;
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(note), tb.Name);
                    });

                    grid1.Children.Add(block);
                    Grid.SetRow(block, i);
                    //temp_message.Text = ""+ str_id[i,j] + " "+ System.Text.Encoding.Unicode.GetString(strs[str_id[i, j]]);

                    Border bd = new Border();
                    grid1.Children.Add(bd);
                    Grid.SetRow(bd, i);
                }

            }
            else
            {
                Toast.Label = "没有选择课表，请前往设置页";
                Toast.Show();
            }

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
    }
}
