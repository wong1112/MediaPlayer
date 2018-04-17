using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MediaPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;

            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mp3");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            }
        }

        private void button_1_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3"));
        }

        private async void button_2_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3");
            var myMusics = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
            var myMusic = myMusics.SaveFolder;
            var fileName = Path.GetFileName(uri.LocalPath);
            try
            {
                StorageFile musicFile = await myMusic.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.FailIfExists);
                if (musicFile != null)
                {
                    Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

                    IBuffer buffer;
                    try
                    {
                        buffer = await httpClient.GetBufferAsync(uri);
                    }
                    catch (Exception ex)
                    {
                        text.Text = "download fail";
                        return;
                    }

                    await FileIO.WriteBufferAsync(musicFile, buffer);
                    text.Text = "download complete";
                }
            }
            catch (Exception ex)
            {
                text.Text = "file exist";
            }
        }
    }
}