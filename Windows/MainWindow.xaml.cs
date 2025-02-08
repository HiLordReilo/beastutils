using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.Encodings;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BST_SheetsEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MusicList musicList = new MusicList();
		HackerList hackerList = new HackerList();
		CourseList courseList = new CourseList();
		HTSheet htSheet = new HTSheet();

		bool isMusicListLoaded = false;
		bool isHackerListLoaded = false;
		bool isCourseListLoaded = false;
		bool isHTSheetLoaded = false;

		string packagePath = string.Empty;

		ObservableCollection<string> songNames = new ObservableCollection<string>();

		public MainWindow()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			InitializeComponent();
		}

		private void miOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Filter = "All files|*.*|BeatStream Sheet files|*.csv|BeatStream HIGH-TENSION TIME sheet files|*.txt",
			};

            string fileFull = string.Empty;
            string[] fileLines = new string[0];

            if ((bool)dialog.ShowDialog())
			{
				fileFull = File.ReadAllText(dialog.FileName, Encoding.GetEncoding("Shift-JIS"));
				fileLines = fileFull.Split('\n');

                if (fileLines[0].StartsWith("// MusicInfoData")) tcSheet.SelectedIndex = 0;
				if (fileLines[0].StartsWith("// BeastHacker")) tcSheet.SelectedIndex = 1;
				if (fileLines[0].StartsWith("// CourseInfoData")) tcSheet.SelectedIndex = 2;
				if (fileLines[0].StartsWith("// CharacterData")) tcSheet.SelectedIndex = 4;
				if (fileLines[0].StartsWith("楽曲名")) tcSheet.SelectedIndex = 5;

                switch (tcSheet.SelectedIndex)
				{
					case 0:
						musicList = MusicList.ParseCSV(fileLines);
						lvMusicList.ItemsSource = musicList.Songs;
                        lvHackerMusicList.ItemsSource = musicList.Songs;
						lvCourseMusicList.ItemsSource = musicList.Songs;

						isMusicListLoaded = true;
						MusicList_Refresh(sender, e);
                        break;
					case 1:
						if (musicList.Songs.Count == 0 && MessageBox.Show("Your Music List is currently empty.\nAre you sure you want to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
							break;
						hackerList = HackerList.ParseCSV(fileLines, musicList);
						lvHackerList.ItemsSource = hackerList.Entries;
                        
						isHackerListLoaded = true;
						HackerList_Refresh(sender, e);
                        break;
					case 2:
                        if (musicList.Songs.Count == 0 && MessageBox.Show("Your Music List is currently empty.\nAre you sure you want to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            break;

                        courseList = CourseList.ParseCSV(fileFull);
                        lvCourseList.ItemsSource = courseList.Entries;

						isCourseListLoaded = true;
                        HackerList_Refresh(sender, e);
                        break;
					case 5:
                        if (musicList.Songs.Count == 0 && MessageBox.Show("Your Music List is currently empty.\nAre you sure you want to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            break;
                        htSheet = HTSheet.ParseTXT(fileLines);
                        lvHTSheet.ItemsSource = htSheet.Entries;

						isHTSheetLoaded = true;
                        HTSheet_Refresh(sender, e);
                        break;
				}
			}
		}

		private void miOpenAll_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog()
			{
				IsFolderPicker = true,
				Title = "Select root folder of a game or a mod."
			};

			string[] file = new string[0];

			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				bool isMod = dialog.FileName.Contains("data_mods");

				if(!isMod)
				{
					if (MessageBox.Show("Selected folder is not considered to be a mod.\n" +
						"It is generally not recommended to modify game files directly.\n" +
						"Press OK if you wish to continue anyway.",
						"Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
						return;
                }

				// Reset all lists
				musicList = new MusicList();
				hackerList = new HackerList();
				musicList = new MusicList();

				// Scan for data sheets
				// BeatStream AnimTribe
				{
                    // MusicList
                    if (File.Exists(dialog.FileName + Util.SheetPaths["BST2_MusicList"]))
                    {
                        file = File.ReadAllLines(dialog.FileName + Util.SheetPaths["BST2_MusicList"], Encoding.GetEncoding("Shift-JIS"));

                        musicList = MusicList.ParseCSV(file);

                        lvMusicList.ItemsSource = musicList.Songs;
                        lvHackerMusicList.ItemsSource = musicList.Songs;
                        lvCourseMusicList.ItemsSource = musicList.Songs;

						isMusicListLoaded = true;
                        MusicList_Refresh(sender, e);
                    }
					else
					{
                        switch (MessageBox.Show("Selected folder does not contain musiclist.csv.\n" +
                            "It is recommended to load it first, so songs would appear in BEAST HACKER, Course Mode and HIGH TENSTION tabs.\n" +
                            "Would you like to locate musiclist.csv manually?.\n" +
							"\n" +
							"- Click Yes to open file selector.\n" +
							"- Click No to continue without MusicList.\n" +
							"- Click Cancel to cancel loading altogether.",
                            "musiclist.csv not found", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
						{
							case MessageBoxResult.Yes:
								tcSheet.SelectedIndex = 0;
								miOpen_Click(sender, e);
								break;
							case MessageBoxResult.No:
								// Do nothing.
								break;
							case MessageBoxResult.Cancel:
								return;
						}
                    }
                    // HackerList
                    if (File.Exists(dialog.FileName + Util.SheetPaths["BST2_HackerList"]))
                    {
                        file = File.ReadAllLines(dialog.FileName + Util.SheetPaths["BST2_HackerList"], Encoding.GetEncoding("Shift-JIS"));

                        hackerList = HackerList.ParseCSV(file, musicList);

                        lvHackerList.ItemsSource = hackerList.Entries;

                        isHackerListLoaded = true;
                        HackerList_Refresh(sender, e);
                    }
                    // CourseList
                    if (File.Exists(dialog.FileName + Util.SheetPaths["BST2_CourseList"]))
                    {
                        string _file = File.ReadAllText(dialog.FileName + Util.SheetPaths["BST2_CourseList"], Encoding.GetEncoding("Shift-JIS"));

                        courseList = CourseList.ParseCSV(_file);

                        lvCourseList.ItemsSource = courseList.Entries;

						isCourseListLoaded = true;
                        CourseList_Refresh(sender, e);
                    }
                    // CrisisList
                    if (File.Exists(dialog.FileName + Util.SheetPaths["BST2_CrisisList"]))
                    {
                        file = File.ReadAllLines(dialog.FileName + Util.SheetPaths["BST2_CrisisList"], Encoding.GetEncoding("Shift-JIS"));

						// TODO: Parse list
                    }
                    // CharaList
                    if (File.Exists(dialog.FileName + Util.SheetPaths["BST2_CharaList"]))
                    {
                        file = File.ReadAllLines(dialog.FileName + Util.SheetPaths["BST2_CharaList"], Encoding.GetEncoding("Shift-JIS"));

						// TODO: Parse list
                    }
                    // HTSheet
                    if (File.Exists(dialog.FileName + Util.SheetPaths["BST2_HTSheet"]))
                    {
                        file = File.ReadAllLines(dialog.FileName + Util.SheetPaths["BST2_HTSheet"], Encoding.GetEncoding("Shift-JIS"));

                        htSheet = HTSheet.ParseTXT(file);

                        lvHTSheet.ItemsSource = htSheet.Entries;

						isHTSheetLoaded = true;
                        HTSheet_Refresh(sender, e);
                    }
                }

				if(isMusicListLoaded ||
					isHackerListLoaded ||
					isCourseListLoaded ||
					isHTSheetLoaded) packagePath = dialog.FileName;

				MessageBox.Show("Loaded files:\n" +
					$"- Song List (musiclist.csv): \t\t{(isMusicListLoaded ? "LOADED" : "NOT FOUND")}\n"+
					$"- BEAST HACKER List (hacker_list.csv): \t{(isHackerListLoaded ? "LOADED" : "NOT FOUND")}\n"+
					$"- Course Mode List (courselist.csv): \t{(isCourseListLoaded ? "LOADED" : "NOT FOUND")}\n"+
					//$"- BEAST CRISIS List (crysislist.csv): \t{(isMusicListLoaded ? "LOADED" : "NOT FOUND")}\n"+
					//$"- Character List (chara_list.csv): \t\t{(isMusicListLoaded ? "LOADED" : "NOT FOUND")}\n"+
					$"- BEAST CRISIS List (crysislist.csv): \tNOT SUPPORTED\n"+
					$"- Character List (chara_list.csv): \t\tNOT SUPPORTED\n"+
					$"- HIGH TENSION Sheet (ht_sheat.txt): \t{(isHTSheetLoaded ? "LOADED" : "NOT FOUND")}\n",
					"Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
		}

		private void miSaveAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog()
			{
				Filter = "BeatStream Sheet files|*.csv|BeatStream HIGH-TENSION TIME sheet files|*.txt|All files|*.*",
			};
			
			if (tcSheet.SelectedIndex == 5) dialog.FilterIndex = 1;

            if ((bool)dialog.ShowDialog())
			{
				switch(tcSheet.SelectedIndex)
				{
					// Music List
					case 0:
						File.WriteAllLines(dialog.FileName, MusicList.CreateCSV(musicList), Encoding.GetEncoding("UTF-16"));
						break;
					// Hacker List
					case 1:
						File.WriteAllLines(dialog.FileName, HackerList.CreateCSV(hackerList), Encoding.GetEncoding("Shift-JIS"));
						break;
					// Course List
					case 2:
						File.WriteAllText(dialog.FileName, CourseList.CreateCSV(courseList), Encoding.GetEncoding("Shift-JIS"));
						break;
					// HIGH TENSION Sheet
					case 5:
						File.WriteAllLines(dialog.FileName, HTSheet.CreateTXT(htSheet), Encoding.GetEncoding("Shift-JIS"));
						break;
				}
			}

			MessageBox.Show("Sheet have been saved to\n" + dialog.FileName, "All good!", MessageBoxButton.OK, MessageBoxImage.Information);
		}

        private void miSaveAll_Click(object sender, RoutedEventArgs e)
		{
			new SavePack(packagePath, musicList, hackerList, courseList, htSheet).ShowDialog();
		}

        private void bQuotationBracketCopy_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText("\u300C\u300D");
		}

		private void MusicList_Refresh(object sender, RoutedEventArgs e)
		{
			lvMusicList.ItemsSource = null;
			lvMusicList.ItemsSource = musicList.Songs;
			lvHackerMusicList.ItemsSource = null;
            lvHackerMusicList.ItemsSource = musicList.Songs;

			UpdateSongNames(sender, e);
        }

		private void bReindexAndRefresh_MusicList_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < musicList.Songs.Count; i++)
			{
				musicList.Songs[i].ID = i;
			}
			MusicList_Refresh(sender, e);
		}

		private void bRemoveEntry_MusicList_Click(object sender, RoutedEventArgs e)
		{
			if(lvMusicList.SelectedIndex > -1)
			{
				musicList.Songs.RemoveAt(lvMusicList.SelectedIndex);
				MusicList_Refresh(sender, e);
			}
		}

		private void bAddEntry_MusicList_Click(object sender, RoutedEventArgs e)
		{
			MusicList.Music newSong = new MusicList.Music()
			{
				ID = lvMusicList.SelectedIndex > -1 ? lvMusicList.SelectedIndex + 1 : musicList.Songs.Count,
				DifficultyLight = "-1",
				DifficultyMedium = "-1",
				DifficultyBeast = "-1",
				DifficultyNightmare = "-1",
				Game = "2nd",
				Category = "OTHER",
				Unknown1 = 0,
				Unknown2 = "-",
				Unknown3 = "-",
				Unknown4 = "-",
				License = "-",
				UnlockingMethod = "UNLOCKED",
				Update = 18,
				Unknown6 = 0,
				EventHandler = "-",
				Unknown7 = 0,
				MovieRegion = "ALL",
				MovieReplacement = "-",
				Series = "NO",
			};

			if (lvMusicList.SelectedIndex > -1)
				musicList.Songs.Insert(lvMusicList.SelectedIndex + 1, newSong);
			else
				musicList.Songs.Add(newSong);

			MusicList_Refresh(sender, e);
		}

		private void bClear_MusicList_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete all entries from the Music List?\nThis action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				musicList.Songs.Clear();
				MusicList_Refresh(sender, e);
			}
		}

		private void HackerList_Refresh(object sender, RoutedEventArgs e)
		{
			hackerList.UpdateDisplayedHeaders(musicList);
			lvHackerList.ItemsSource = null;
			lvHackerList.ItemsSource = hackerList.Entries;
		}

		private void bReindexAndRefresh_HackerList_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < hackerList.Entries.Count; i++)
			{
				hackerList.Entries[i].ID = i;
			}

			HackerList_Refresh(sender, e);
		}

		private void bRemoveEntry_HackerList_Click(object sender, RoutedEventArgs e)
		{
			if(lvHackerList.SelectedIndex > -1)
			{
				hackerList.Entries.RemoveAt(lvHackerList.SelectedIndex);
				HackerList_Refresh(sender, e);
			}
		}

		private void bAddEntry_HackerList_Click(object sender, RoutedEventArgs e)
		{
			HackerList.Chain newChain = new HackerList.Chain()
			{
				ID = lvMusicList.SelectedIndex > -1 ? lvMusicList.SelectedIndex + 1 : musicList.Songs.Count,
				UnlockedSong = -1,
				Lock1_SongID = -1,
				Lock1_SongDifficulty = "-",
				Lock1_CompletionMethod = "-",
				Lock1_Goal = -1,
				Lock2_SongID = -1,
				Lock2_SongDifficulty = "-",
				Lock2_CompletionMethod = "-",
				Lock2_Goal = -1,
				Lock3_SongID = -1,
				Lock3_SongDifficulty = "-",
				Lock3_CompletionMethod = "-",
				Lock3_Goal = -1,
				Lock4_SongID = -1,
				Lock4_SongDifficulty = "-",
				Lock4_CompletionMethod = "-",
				Lock4_Goal = -1,
				Lock5_SongID = -1,
				Lock5_SongDifficulty = "-",
				Lock5_CompletionMethod = "-",
				Lock5_Goal = -1,
				Update = 10,
				HackerLevel = 1,
			};

			if (lvHackerList.SelectedIndex > -1)
				hackerList.Entries.Insert(lvHackerList.SelectedIndex + 1, newChain);
			else
                hackerList.Entries.Add(newChain);

			HackerList_Refresh(sender, e);
		}

		private void bClear_HackerList_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete all entries from the Hacker List?\nThis action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				hackerList.Entries.Clear();
				HackerList_Refresh(sender, e);
			}
		}

		private void CourseList_Refresh(object sender, RoutedEventArgs e)
		{
			lvCourseList.ItemsSource = null;
			lvCourseList.ItemsSource = courseList.Entries;
		}

		private void bRemoveEntry_CourseList_Click(object sender, RoutedEventArgs e)
		{
			if(lvCourseList.SelectedIndex > -1)
			{
				courseList.Entries.RemoveAt(lvCourseList.SelectedIndex);
                CourseList_Refresh(sender, e);
			}
		}

		private void bAddEntry_CourseList_Click(object sender, RoutedEventArgs e)
		{
			CourseList.Entry newCourse = new CourseList.Entry()
			{
                Title = "New Course!",
                UpdateID = 4,
				Song1_ID = 0,
				Song2_ID = 0,
				Song3_ID = 0,
				Song4_ID = 0,
				Song1_Difficulty = "L",
				Song2_Difficulty = "L",
				Song3_Difficulty = "L",
				Song4_Difficulty = "L",
				BigIconName = "rank_01_big",
				SmallIconName = "rank_01",
				Type = "RANK",
				Reward = 1
			};

			if (lvCourseList.SelectedIndex > -1)
				courseList.Entries.Insert(lvCourseList.SelectedIndex + 1, newCourse);
			else
                courseList.Entries.Add(newCourse);

            CourseList_Refresh(sender, e);
		}

		private void bClear_CourseList_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete all entries from the Course List?\nThis action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				courseList.Entries.Clear();
				CourseList_Refresh(sender, e);
			}
		}

        private void HTSheet_Refresh(object sender, RoutedEventArgs e)
        {
            lvHTSheet.ItemsSource = null;
            lvHTSheet.ItemsSource = htSheet.Entries;

            UpdateSongNames(sender, e);
			cbHTSongName.ItemsSource = songNames;
        }

        private void bRemoveEntry_HTSheet_Click(object sender, RoutedEventArgs e)
        {
            if (lvHTSheet.SelectedIndex > -1)
            {
                htSheet.Entries.RemoveAt(lvHTSheet.SelectedIndex);
                HTSheet_Refresh(sender, e);
            }
        }

        private void bAddEntry_HTSheet_Click(object sender, RoutedEventArgs e)
        {
            HTSheet.Entry newEntry = new HTSheet.Entry()
            {
				SongName = "",
				IsEnabled = false,
				PM_Tap = "fire_hit.pm",
				TEX_Tap = "fire.dds",
				PM_Hold = "fire_hit.pm",
				TEX_Hold = "fire.dds",
				PM_Ripple = "fire_hit.pm",
				TEX_Ripple = "fire.dds",
				PM_Slash = "fire_hit.pm",
				TEX_Slash = "fire.dds",
				PM_Stream = "fire_hit.pm",
				TEX_Stream = "fire.dds",
            };

            if (lvHTSheet.SelectedIndex > -1)
                htSheet.Entries.Insert(lvHTSheet.SelectedIndex + 1, newEntry);
            else
                htSheet.Entries.Add(newEntry);

            HTSheet_Refresh(sender, e);
        }

        private void bClear_HTSheet_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all entries from the HIGH TENSION Sheet?\nThis action is irreversible!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                htSheet.Entries.Clear();
                HTSheet_Refresh(sender, e);
            }
        }

        private void tcSheet_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(e.Source == sender)
			{
				MusicList_Refresh(sender, e);
				HackerList_Refresh(sender, e);
			}
		}

		private void UpdateSongNames(object sender, RoutedEventArgs e)
		{
			songNames.Clear();
			foreach (MusicList.Music m in musicList.Songs)
			{
				songNames.Add(m.Title);
			}

            cbHTSongName.ItemsSource = songNames;
        }

        private void AssignSong(object sender, RoutedEventArgs e)
		{
			switch(tcSheet.SelectedIndex)
			{
				case 1:
					MusicList.Music selectedHackerSong = (MusicList.Music)lvHackerMusicList.SelectedItem;
					switch (((Button)sender).Name)
					{
						case "btHackerUnlockedSong":
							tbHackerUnlockedSong.Value = selectedHackerSong.ID;
							((HackerList.Chain)lvHackerList.SelectedItem).DisplayedUnlockedSong = musicList.GetTitleArtist(selectedHackerSong.ID);
							break;
						case "btHackerLock1":
							tbHackerLock1.Value = selectedHackerSong.ID;
							break;
						case "btHackerLock2":
							tbHackerLock2.Value = selectedHackerSong.ID;
							break;
                        case "btHackerLock3":
							tbHackerLock3.Value = selectedHackerSong.ID;
							break;
                        case "btHackerLock4":
							tbHackerLock4.Value = selectedHackerSong.ID;
							break;
                        case "btHackerLock5":
							tbHackerLock5.Value = selectedHackerSong.ID;
							break;
						case "btHackerLock1Clear":
							tbHackerLock1.Value = -1;
							cbHackerLock1Diff.SelectedIndex = -1;
							cbHackerLock1Method.SelectedIndex = -1;
							cbHackerLock1Goal.Value = -1;
							break;
						case "btHackerLock2Clear":
							tbHackerLock2.Value = -1;
							cbHackerLock2Diff.SelectedIndex = -1;
							cbHackerLock2Method.SelectedIndex = -1;
							cbHackerLock2Goal.Value = -1;
							break;
						case "btHackerLock3Clear":
							tbHackerLock3.Value = -1;
							cbHackerLock3Diff.SelectedIndex = -1;
							cbHackerLock3Method.SelectedIndex = -1;
							cbHackerLock3Goal.Value = -1;
							break;
						case "btHackerLock4Clear":
							tbHackerLock4.Value = -1;
							cbHackerLock4Diff.SelectedIndex = -1;
							cbHackerLock4Method.SelectedIndex = -1;
							cbHackerLock4Goal.Value = -1;
							break;
						case "btHackerLock5Clear":
							tbHackerLock5.Value = -1;
							cbHackerLock5Diff.SelectedIndex = -1;
							cbHackerLock5Method.SelectedIndex = -1;
							cbHackerLock5Goal.Value = -1;
							break;
                    }
                    break;
				case 2:
                    MusicList.Music selectedCourseSong = (MusicList.Music)lvCourseMusicList.SelectedItem;
                    switch (((Button)sender).Name)
                    {
                        case "btCourseSong1":
                            tbCourseSong1.Value = selectedCourseSong.ID;
                            break;
                        case "btCourseSong2":
                            tbCourseSong2.Value = selectedCourseSong.ID;
                            break;
                        case "btCourseSong3":
                            tbCourseSong3.Value = selectedCourseSong.ID;
                            break;
                        case "btCourseSong4":
                            tbCourseSong4.Value = selectedCourseSong.ID;
                            break;
                    }
                    break;
            }

			UpdateAssignedSongs(sender, null);
        }

		private void ChangeAssignedSong(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			Xceed.Wpf.Toolkit.IntegerUpDown spin = (Xceed.Wpf.Toolkit.IntegerUpDown)sender;

			switch(tcSheet.SelectedIndex)
			{
				case 1:
                    if (spin.Value != null)
                        lvHackerMusicList.SelectedIndex = (int)spin.Value;

                    lvHackerMusicList.ScrollIntoView(lvHackerMusicList.SelectedItem);
                    break;
				case 2:
                    if (spin.Value != null)
                        lvCourseMusicList.SelectedIndex = (int)spin.Value;

                    lvCourseMusicList.ScrollIntoView(lvCourseMusicList.SelectedItem);
                    break;
			}

            UpdateAssignedSongs(sender, null);
            return;
		}

        private void bAppendFromClipboard_MusicList_Click(object sender, RoutedEventArgs e)
        {
			if(Clipboard.ContainsText())
			{
				try
				{
                    MusicList.Music newSong = MusicList.Music.ParseData(Clipboard.GetText(), musicList.Songs.Count);

                    musicList.Songs.Add(newSong);

                    MusicList_Refresh(sender, e);

					lvMusicList.SelectedIndex = musicList.Songs.Count - 1;
                }
				catch
				{
                    MessageBox.Show("Clipboard does not contain MusicList entry data.", "Error parsing data.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
			else
			{
				MessageBox.Show("Clipboard does not contain MusicList entry data.", "Error parsing data.", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
        }

        private void bAppendFromClipboard_HTSheet_Click(object sender, RoutedEventArgs e)
        {
			if(Clipboard.ContainsText())
			{
				try
				{
                    HTSheet.Entry newEntry = HTSheet.Entry.ParseData(Clipboard.GetText());

                    htSheet.Entries.Add(newEntry);

                    HTSheet_Refresh(sender, e);

					lvHTSheet.SelectedIndex = htSheet.Entries.Count - 1;
                }
				catch
				{
                    MessageBox.Show("Clipboard does not contain HTSheet entry data.", "Error parsing data.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
			else
			{
				MessageBox.Show("Clipboard does not contain HTSheet entry data.", "Error parsing data.", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
        }

        private void UpdateAssignedSongs(object sender, SelectionChangedEventArgs e)
        {
			switch(tcSheet.SelectedIndex)
			{
				// HackerList
				case 1:
                    if (lvHackerList.SelectedItem == null) return;

                    HackerList.Chain chain = (HackerList.Chain)lvHackerList.SelectedItem;

					// there probably a better way to handle this, but whatever
					// Lock 1
					try
                    {
						if(chain.Lock1_SongID == -1)
						{
                            lHackerLock1Name.Content = "-";
                        }
						else
						{
                            string diff = Util.GetFullDiffName(chain.Lock1_SongDifficulty) switch
                            {
                                "LIGHT" => musicList.Songs[chain.Lock1_SongID].DisplayedDifficultyLight,
                                "MEDIUM" => musicList.Songs[chain.Lock1_SongID].DisplayedDifficultyMedium,
                                "BEAST" => musicList.Songs[chain.Lock1_SongID].DisplayedDifficultyBeast,
                                "NIGHTMARE" => musicList.Songs[chain.Lock1_SongID].DisplayedDifficultyNightmare,
                                _ => ""
                            };
                            lHackerLock1Name.Content = $"{musicList.GetTitleArtist(chain.Lock1_SongID)} [{Util.GetFullDiffName(chain.Lock1_SongDifficulty)} {diff}]";
                        }
                    }
                    catch
					{
						lHackerLock1Name.Content = "[INVALID SONG]";
                    }
					// Lock 2
					try
					{
						if (chain.Lock2_SongID == -1)
						{
							lHackerLock2Name.Content = "-";
						}
						else
						{
							string diff = Util.GetFullDiffName(chain.Lock1_SongDifficulty) switch
							{
								"LIGHT" => musicList.Songs[chain.Lock2_SongID].DisplayedDifficultyLight,
								"MEDIUM" => musicList.Songs[chain.Lock2_SongID].DisplayedDifficultyMedium,
								"BEAST" => musicList.Songs[chain.Lock2_SongID].DisplayedDifficultyBeast,
								"NIGHTMARE" => musicList.Songs[chain.Lock2_SongID].DisplayedDifficultyNightmare,
								_ => ""
							};
							lHackerLock2Name.Content = $"{musicList.GetTitleArtist(chain.Lock2_SongID)} [{Util.GetFullDiffName(chain.Lock2_SongDifficulty)} {diff}]";
						}
					}
					catch
					{
						lHackerLock2Name.Content = "[INVALID SONG]";
					}
					// Lock 3
					try
					{
						if (chain.Lock3_SongID == -1)
						{
							lHackerLock3Name.Content = "-";
						}
						else
						{
							string diff = Util.GetFullDiffName(chain.Lock1_SongDifficulty) switch
							{
								"LIGHT" => musicList.Songs[chain.Lock3_SongID].DisplayedDifficultyLight,
								"MEDIUM" => musicList.Songs[chain.Lock3_SongID].DisplayedDifficultyMedium,
								"BEAST" => musicList.Songs[chain.Lock3_SongID].DisplayedDifficultyBeast,
								"NIGHTMARE" => musicList.Songs[chain.Lock3_SongID].DisplayedDifficultyNightmare,
								_ => ""
							};
							lHackerLock3Name.Content = $"{musicList.GetTitleArtist(chain.Lock3_SongID)} [{Util.GetFullDiffName(chain.Lock3_SongDifficulty)} {diff}]";
						}
					}
					catch
					{
						lHackerLock3Name.Content = "[INVALID SONG]";
					}
					// Lock 4
					try
					{
						if (chain.Lock4_SongID == -1)
						{
							lHackerLock4Name.Content = "-";
						}
						else
						{
							string diff = Util.GetFullDiffName(chain.Lock1_SongDifficulty) switch
							{
								"LIGHT" => musicList.Songs[chain.Lock4_SongID].DisplayedDifficultyLight,
								"MEDIUM" => musicList.Songs[chain.Lock4_SongID].DisplayedDifficultyMedium,
								"BEAST" => musicList.Songs[chain.Lock4_SongID].DisplayedDifficultyBeast,
								"NIGHTMARE" => musicList.Songs[chain.Lock4_SongID].DisplayedDifficultyNightmare,
								_ => ""
							};
							lHackerLock4Name.Content = $"{musicList.GetTitleArtist(chain.Lock4_SongID)} [{Util.GetFullDiffName(chain.Lock4_SongDifficulty)} {diff}]";
						}
					}
					catch
					{
						lHackerLock4Name.Content = "[INVALID SONG]";
					}
					// Lock 5
					try
					{
						if (chain.Lock5_SongID == -1)
						{
							lHackerLock5Name.Content = "-";
						}
						else
						{
							string diff = Util.GetFullDiffName(chain.Lock1_SongDifficulty) switch
							{
								"LIGHT" => musicList.Songs[chain.Lock5_SongID].DisplayedDifficultyLight,
								"MEDIUM" => musicList.Songs[chain.Lock5_SongID].DisplayedDifficultyMedium,
								"BEAST" => musicList.Songs[chain.Lock5_SongID].DisplayedDifficultyBeast,
								"NIGHTMARE" => musicList.Songs[chain.Lock5_SongID].DisplayedDifficultyNightmare,
								_ => ""
							};
							lHackerLock5Name.Content = $"{musicList.GetTitleArtist(chain.Lock5_SongID)} [{Util.GetFullDiffName(chain.Lock5_SongDifficulty)} {diff}]";
						}
					}
					catch
					{
						lHackerLock5Name.Content = "[INVALID SONG]";
					}
                    break;
				// CourseList
				case 2:
					if (lvCourseList.SelectedItem == null) return;

					CourseList.Entry course = (CourseList.Entry)lvCourseList.SelectedItem;

					// again, there probably a better way to handle this, but whatever
					// Song 1
					try
                    {
                        string diff = Util.GetFullDiffName(course.Song1_Difficulty) switch
                        {
                            "LIGHT" => musicList.Songs[course.Song1_ID].DisplayedDifficultyLight,
                            "MEDIUM" => musicList.Songs[course.Song1_ID].DisplayedDifficultyMedium,
                            "BEAST" => musicList.Songs[course.Song1_ID].DisplayedDifficultyBeast,
                            "NIGHTMARE" => musicList.Songs[course.Song1_ID].DisplayedDifficultyNightmare,
                            _ => ""
                        };
                        lCourseSong1Name.Content = $"{musicList.GetTitleArtist(course.Song1_ID)} [{Util.GetFullDiffName(course.Song1_Difficulty)} {diff}]";
                    }
                    catch
					{
                        lCourseSong1Name.Content = "[INVALID SONG]";
                    }
                    // Song 2
                    try
                    {
                        string diff = Util.GetFullDiffName(course.Song2_Difficulty) switch
                        {
                            "LIGHT" => musicList.Songs[course.Song2_ID].DisplayedDifficultyLight,
                            "MEDIUM" => musicList.Songs[course.Song2_ID].DisplayedDifficultyMedium,
                            "BEAST" => musicList.Songs[course.Song2_ID].DisplayedDifficultyBeast,
                            "NIGHTMARE" => musicList.Songs[course.Song2_ID].DisplayedDifficultyNightmare,
                            _ => ""
                        };
                        lCourseSong2Name.Content = $"{musicList.GetTitleArtist(course.Song2_ID)} [{Util.GetFullDiffName(course.Song2_Difficulty)} {diff}]";
                    }
                    catch
					{
                        lCourseSong2Name.Content = "[INVALID SONG]";
                    }
                    // Song 3
                    try
                    {
                        string diff = Util.GetFullDiffName(course.Song3_Difficulty) switch
                        {
                            "LIGHT" => musicList.Songs[course.Song3_ID].DisplayedDifficultyLight,
                            "MEDIUM" => musicList.Songs[course.Song3_ID].DisplayedDifficultyMedium,
                            "BEAST" => musicList.Songs[course.Song3_ID].DisplayedDifficultyBeast,
                            "NIGHTMARE" => musicList.Songs[course.Song3_ID].DisplayedDifficultyNightmare,
                            _ => ""
                        };
                        lCourseSong3Name.Content = $"{musicList.GetTitleArtist(course.Song3_ID)} [{Util.GetFullDiffName(course.Song3_Difficulty)} {diff}]";
                    }
                    catch
					{
                        lCourseSong3Name.Content = "[INVALID SONG]";
                    }
                    // Song 4
                    try
                    {
						string diff = Util.GetFullDiffName(course.Song4_Difficulty) switch
                        {
							"LIGHT" => musicList.Songs[course.Song4_ID].DisplayedDifficultyLight,
							"MEDIUM" => musicList.Songs[course.Song4_ID].DisplayedDifficultyMedium,
							"BEAST" => musicList.Songs[course.Song4_ID].DisplayedDifficultyBeast,
							"NIGHTMARE" => musicList.Songs[course.Song4_ID].DisplayedDifficultyNightmare,
                            _ => ""
						};
                        lCourseSong4Name.Content = $"{musicList.GetTitleArtist(course.Song4_ID)} [{Util.GetFullDiffName(course.Song4_Difficulty)} {diff}]";
                    }
                    catch
					{
                        lCourseSong4Name.Content = "[INVALID SONG]";
                    }
                    break;
			}
        }
    }
}