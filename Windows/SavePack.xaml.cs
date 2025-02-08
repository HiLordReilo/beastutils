using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BST_SheetsEditor
{
    /// <summary>
    /// Логика взаимодействия для SavePack.xaml
    /// </summary>
    public partial class SavePack : Window
    {
        MusicList _musicList;
        HackerList _hackerList;
        CourseList _courseList;
        HTSheet _htSheet;


        public SavePack(string initialPath, MusicList musicList, HackerList hackerList, CourseList courseList, HTSheet htSheet)
        {
            InitializeComponent();

            tbSavePath.Text = initialPath;
            _musicList = musicList;
            _hackerList = hackerList;
            _courseList = courseList;
            _htSheet = htSheet;
            cbMusicList.IsChecked = musicList.Songs.Count > 0;
            cbHackerList.IsChecked = hackerList.Entries.Count > 0;
            cbCourseList.IsChecked = courseList.Entries.Count > 0;
            cbHTSheet.IsChecked = htSheet.Entries.Count > 0;
        }

        private void btSavePath_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Title = "Select a folder to save your sheets to."
            };

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                tbSavePath.Text = dialog.FileName;
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbSavePath.Text))
            {
                MessageBox.Show("Specify save path first!", "Save path not specified", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(!tbSavePath.Text.Contains("data_mods"))
            {
                if (MessageBox.Show("Selected folder is not considered to be a mod.\n" +
                    "It is generally not recommended to modify game files directly.\n" +
                    "Press OK if you wish to continue anyway.",
                    "BeStISE did not find \"data_mods\" in the path", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Cancel)
                    return;
            }

            Directory.CreateDirectory(tbSavePath.Text + "/data2/sound/");
            Directory.CreateDirectory(tbSavePath.Text + "/data2/others/");
            if (cbMusicList.IsChecked == true) File.WriteAllLines(tbSavePath.Text + Util.SheetPaths["BST2_MusicList"], MusicList.CreateCSV(_musicList), Encoding.GetEncoding("UTF-16"));
            if (cbHackerList.IsChecked == true) File.WriteAllLines(tbSavePath.Text + Util.SheetPaths["BST2_HackerList"], HackerList.CreateCSV(_hackerList), Encoding.GetEncoding("Shift-JIS"));
            if (cbCourseList.IsChecked == true) File.WriteAllText(tbSavePath.Text + Util.SheetPaths["BST2_CourseList"], CourseList.CreateCSV(_courseList), Encoding.GetEncoding("Shift-JIS"));
            if (cbHTSheet.IsChecked == true) File.WriteAllLines(tbSavePath.Text + Util.SheetPaths["BST2_HTSheet"], HTSheet.CreateTXT(_htSheet), Encoding.GetEncoding("Shift-JIS"));

            MessageBox.Show("Package have been saved to\n" + tbSavePath.Text, "All good!", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
