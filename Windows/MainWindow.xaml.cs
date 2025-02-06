using Microsoft.Win32;
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
				Filter = "BeatStream Sheet files|*.csv|BeatStream HIGH-TENSION TIME sheet files|*.txt|BeatStream Particle Motion files|*.pm|BeatStream Stream Prefabs|*.str",
			};

			string[] file = new string[0];

			if ((bool)dialog.ShowDialog())
			{
				file = File.ReadAllLines(dialog.FileName, Encoding.GetEncoding("Shift-JIS"));

				if (file[0].StartsWith("// MusicInfoData")) tcSheet.SelectedIndex = 0;
				if (file[0].StartsWith("// BeastHacker")) tcSheet.SelectedIndex = 1;
				if (file[0].StartsWith("// CharacterData")) tcSheet.SelectedIndex = 4;
				if (file[0].StartsWith("楽曲名")) tcSheet.SelectedIndex = 5;

                switch (tcSheet.SelectedIndex)
				{
					case 0:
						musicList = MusicList.ParseCSV(file);
						lvMusicList.ItemsSource = musicList.Songs;
                        lvHackerMusicList.ItemsSource = musicList.Songs;
                        break;
					case 1:
						if (musicList.Songs.Count == 0 && MessageBox.Show("Your Music List is currently empty.\nAre you sure you want to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
							break;
						hackerList = HackerList.ParseCSV(file, musicList);
						lvHackerList.ItemsSource = hackerList.Entries;
						break;
				}
			}
		}

		private void miSaveAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog()
			{
				Filter = "BeatStream Sheet files|*.csv|All files|*.*",
			};

			if((bool)dialog.ShowDialog())
			{
				switch(tcSheet.SelectedIndex)
				{
					//Music List
					case 0:
						File.WriteAllLines(dialog.FileName, MusicList.CreateCSV(musicList), Encoding.GetEncoding("UTF-16"));
						break;
					//Hacker List
					case 1:
						File.WriteAllLines(dialog.FileName, HackerList.CreateCSV(hackerList), Encoding.GetEncoding("Shift-JIS"));
						break;
				}
			}

			MessageBox.Show("Sheet have been saved to\n" + dialog.FileName, "All good!", MessageBoxButton.OK, MessageBoxImage.Information);
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

			if (lvMusicList.SelectedIndex > -1)
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

		private void tcSheet_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(e.Source == sender)
			{
				MusicList_Refresh(sender, e);
				HackerList_Refresh(sender, e);
			}
		}

		private void UpdateSongNames(object sender, SelectionChangedEventArgs e)
		{
			songNames.Clear();
			foreach (MusicList.Music m in musicList.Songs)
			{
				songNames.Add(m.Title);
			}
		}

		private void AssignSong(object sender, RoutedEventArgs e)
		{
			switch(tcSheet.SelectedIndex)
			{
				case 1:
					MusicList.Music selectedSong = (MusicList.Music)lvHackerMusicList.SelectedItem;
					HackerList.Chain selectedChain = (HackerList.Chain)lvHackerList.SelectedItem;
					switch (((Button)sender).Name)
					{
						case "btHackerUnlockedSong":
							tbHackerUnlockedSong.Value = selectedSong.ID;
							((HackerList.Chain)lvHackerList.SelectedItem).DisplayedUnlockedSong = musicList.GetTitleArtist(selectedSong.ID);
							break;
						case "btHackerLock1":
							tbHackerLock1.Value = selectedSong.ID;
							break;
						case "btHackerLock2":
							tbHackerLock2.Value = selectedSong.ID;
							break;
                        case "btHackerLock3":
							tbHackerLock3.Value = selectedSong.ID;
							break;
                        case "btHackerLock4":
							tbHackerLock4.Value = selectedSong.ID;
							break;
                        case "btHackerLock5":
							tbHackerLock5.Value = selectedSong.ID;
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
			}
		}

		private void HackerChangeSelectedSong(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			Xceed.Wpf.Toolkit.IntegerUpDown spin = (Xceed.Wpf.Toolkit.IntegerUpDown)sender;

			if (spin.Value != null)
					lvHackerMusicList.SelectedIndex = (int)spin.Value;

			lvHackerMusicList.ScrollIntoView(lvHackerMusicList.SelectedItem);
			
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

	}
}