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
		int songAssignmentListener = -1;

		public MainWindow()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			InitializeComponent();
		}

		private void miOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog()
			{
				Filter = "BeatStream Sheet files|*.csv",
			};

			if ((bool)dialog.ShowDialog())
			{
				switch (tcSheet.SelectedIndex)
				{
					case 0:
						musicList = MusicList.ParseCSV(File.ReadAllLines(dialog.FileName));
						lvMusicList.ItemsSource = musicList.Songs;
						break;
					case 1:
						if (musicList.Songs.Count == 0 && MessageBox.Show("Your Music List is currently empty.\nAre you sure you want to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
							break;
						hackerList = HackerList.ParseCSV(File.ReadAllLines(dialog.FileName, Encoding.GetEncoding("Shift-JIS")), musicList);
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
				Unknown5 = 0,
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
				(hackerList.Entries[i] as HackerList.Chain).ID = i;
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
				Unknown5 = 0,
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
			cbHackerSong.ItemsSource = null;

			songNames.Clear();
			foreach (MusicList.Music m in musicList.Songs)
			{
				songNames.Add(m.Title);
			}

			cbHackerSong.ItemsSource = songNames;
		}

		private void AssignSong(object sender, MouseButtonEventArgs e)
		{
			switch(tcSheet.SelectedIndex)
			{
				case 1:
					MusicList.Music selectedSong = (MusicList.Music)lvHackerMusicList.SelectedItem;
					HackerList.Chain selectedChain = (HackerList.Chain)lvHackerList.SelectedItem;
					switch (songAssignmentListener)
					{
						case 0:
							selectedChain.UnlockedSong = selectedSong.ID;
							break;
						case 1:
							selectedChain.Lock1_SongID = selectedSong.ID;
							break;
						case 2:
							selectedChain.Lock2_SongID = selectedSong.ID;
							break;
						case 3:
							selectedChain.Lock3_SongID = selectedSong.ID;
							break;
						case 4:
							selectedChain.Lock4_SongID = selectedSong.ID;
							break;
						case 5:
							selectedChain.Lock5_SongID = selectedSong.ID;
							break;
					}
					break;
			}
		}

		private void btHackerUnlockedSong_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}