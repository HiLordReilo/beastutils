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

        enum SaveState { SKIP, SUCCESS, FAIL }

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

            if (!tbSavePath.Text.Contains("data_mods"))
            {
                if (MessageBox.Show("Selected folder is not considered to be a mod.\n" +
                    "It is generally not recommended to modify game files directly.\n" +
                    "Press OK if you wish to continue anyway.",
                    "BeStISE did not find \"data_mods\" in the path", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Cancel)
                    return;
            }

            Directory.CreateDirectory(tbSavePath.Text + "/data2/sound/");
            Directory.CreateDirectory(tbSavePath.Text + "/data2/others/");

            // Save states.
            // 0 = Skipped
            // 1 = Saved
            // 255 = Failed
            SaveState ssML = SaveState.SKIP;
            SaveState ssHL = SaveState.SKIP;
            SaveState ssCoL = SaveState.SKIP;
            SaveState ssCrL = SaveState.SKIP;
            SaveState ssChL = SaveState.SKIP;
            SaveState ssHTS = SaveState.SKIP;
            bool anyFail = false;

            // MusicList
            if (cbMusicList.IsChecked == true)
                try
                {
                    File.WriteAllLines(tbSavePath.Text + Util.SheetPaths["BST2_MusicList"], MusicList.CreateCSV(_musicList), Encoding.GetEncoding("UTF-16"));
                    ssML = SaveState.SUCCESS;
                }
                catch
                {
                    ssML = SaveState.FAIL;
                    anyFail = true;
                }
            // HackerList
            if (cbHackerList.IsChecked == true)
                try
                {
                    File.WriteAllLines(tbSavePath.Text + Util.SheetPaths["BST2_HackerList"], HackerList.CreateCSV(_hackerList), Encoding.GetEncoding("Shift-JIS"));
                    ssHL = SaveState.SUCCESS;
                }
                catch
                {
                    ssHL = SaveState.FAIL;
                    anyFail = true;
                }
            // CourseList
            if (cbCourseList.IsChecked == true)
                try
                {
                    File.WriteAllText(tbSavePath.Text + Util.SheetPaths["BST2_CourseList"], CourseList.CreateCSV(_courseList), Encoding.GetEncoding("Shift-JIS"));
                    ssCoL = SaveState.SUCCESS;
                }
                catch
                {
                    ssCoL = SaveState.FAIL;
                    anyFail = true;
                }
            // HIGH-TENSION Sheet
            if (cbHTSheet.IsChecked == true)
                try
                {
                    File.WriteAllLines(tbSavePath.Text + Util.SheetPaths["BST2_HTSheet"], HTSheet.CreateTXT(_htSheet), Encoding.GetEncoding("Shift-JIS"));
                    ssHTS = SaveState.SUCCESS;
                }
                catch
                {
                    ssHTS = SaveState.FAIL;
                    anyFail = true;
                }

            MessageBox.Show("Package have been saved to\n" + tbSavePath.Text + "\n\n" +
                $"- Song List (musiclist.csv): \t\t{ssML}\n" +
                $"- BEAST HACKER List (hacker_list.csv): \t{ssHL}\n" +
                $"- Course Mode List (courselist.csv): \t{ssCoL}\n" +
                $"- BEAST CRISIS List (crysislist.csv): \t{ssCrL}\n" +
                $"- Character List (chara_list.csv): \t\t{ssChL}\n" +
                $"- HIGH TENSION Sheet (ht_sheat.txt): \t{ssHTS}" + (anyFail ? "\n\nIt appears some files failed to save. Make sure they are not used by other programs while saving." : ""),
                "Complete.",
                MessageBoxButton.OK,
                anyFail ? MessageBoxImage.Warning : MessageBoxImage.Information);

            Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
