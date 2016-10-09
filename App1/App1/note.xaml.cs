using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class note : Page
    {
        public note()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string title = (string)e.Parameter;

            subject.Text = title;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile note_file = await folder.TryGetItemAsync(title + ".txt") as StorageFile;
            if (note_file != null)
            {
                using (var f = new StreamReader(await note_file.OpenStreamForReadAsync()))
                {
                    editor.Text = f.ReadToEnd();
                }
            }
            else
            {
                note_file = await folder.CreateFileAsync(title + ".txt");
                using (var f = new StreamWriter(await note_file.OpenStreamForWriteAsync()))
                {
                    f.Write("");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pane.splitview.IsPaneOpen = !pane.splitview.IsPaneOpen;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            string title = subject.Text;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile note_file = await folder.TryGetItemAsync(title + ".txt") as StorageFile;
            if (note_file == null)
            {
                note_file = await folder.CreateFileAsync(title + ".txt");

            }

            using (var f = new StreamWriter(await note_file.OpenStreamForWriteAsync()))
            {
                f.Write(editor.Text);
            }

        }
    }


}
