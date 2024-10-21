using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BST_SheetsEditor.HackerList;
using static BST_SheetsEditor.MusicList;

namespace BST_SheetsEditor
{
	class HackerList
	{
		public ObservableCollection<Chain> Entries = new ObservableCollection<Chain>();

		public void UpdateDisplayedHeaders(MusicList musicList)
		{
			foreach (Chain entry in Entries)
				entry.UpdateDisplayedHeader(musicList);
		}

		public static HackerList ParseCSV(string[] data, MusicList musicList)
		{
			HackerList result = new HackerList();

			for (int i = 0; i < data.Length; i++)
			{
				//Line is a comment, skip parsing
				if (data[i].StartsWith("//")) continue;
				//Line ends the sheet, stop parsing
				if (data[i] == "EOF") break;

				string[] entryData = data[i].Split('\uFF5C');

				Chain chain = new Chain()
				{
					ID = int.Parse(entryData[0]),
					Update = int.Parse(entryData[1]),
					UnlockedSong = int.Parse(entryData[2]),
					HackerLevel = int.Parse(entryData[3]),
					Lock1_SongID = int.Parse(entryData[4]),
					Lock1_SongDifficulty = entryData[5],
					Lock1_CompletionMethod = entryData[6],
					Lock1_Goal = int.Parse(entryData[7]),
					Lock2_SongID = int.Parse(entryData[8]),
					Lock2_SongDifficulty = entryData[9],
					Lock2_CompletionMethod = entryData[10],
					Lock2_Goal = int.Parse(entryData[11]),
					Lock3_SongID = int.Parse(entryData[12]),
					Lock3_SongDifficulty = entryData[13],
					Lock3_CompletionMethod = entryData[14],
					Lock3_Goal = int.Parse(entryData[15]),
					Lock4_SongID = int.Parse(entryData[16]),
					Lock4_SongDifficulty = entryData[17],
					Lock4_CompletionMethod = entryData[18],
					Lock4_Goal = int.Parse(entryData[19]),
					Lock5_SongID = int.Parse(entryData[20]),
					Lock5_SongDifficulty = entryData[21],
					Lock5_CompletionMethod = entryData[22],
					Lock5_Goal = int.Parse(entryData[23]),
				};

				chain.DisplayedUnlockedSong = musicList.GetTitleArtist(chain.UnlockedSong);
				chain.DisplayedUnlockedDifficulty = chain.UnlockedSong == -1 ? "" : musicList.Songs[chain.UnlockedSong].DisplayedDifficultyNightmare;

				result.Entries.Add(chain);
			}

			return result;
		}

        public static string[] CreateCSV(HackerList hackerList)
        {
            List<string> result = new List<string>();

            result.Add($"// BeastHacker : {DateTime.Today.ToString("yyyy-MM-dd")}");


            foreach (Chain chain in hackerList.Entries)
            {
                result.Add(
                    $"{chain.ID}\uFF5C" +
                    $"{chain.Update}\uFF5C" +
                    $"{chain.UnlockedSong}\uFF5C" +
                    $"{chain.HackerLevel}\uFF5C" +
                    $"{chain.Lock1_SongID}\uFF5C" +
                    $"{chain.Lock1_SongDifficulty}\uFF5C" +
                    $"{chain.Lock1_CompletionMethod}\uFF5C" +
                    $"{chain.Lock1_Goal}\uFF5C" +
                    $"{chain.Lock2_SongID}\uFF5C" +
                    $"{chain.Lock2_SongDifficulty}\uFF5C" +
                    $"{chain.Lock2_CompletionMethod}\uFF5C" +
                    $"{chain.Lock2_Goal}\uFF5C" +
                    $"{chain.Lock3_SongID}\uFF5C" +
                    $"{chain.Lock3_SongDifficulty}\uFF5C" +
                    $"{chain.Lock3_CompletionMethod}\uFF5C" +
                    $"{chain.Lock3_Goal}\uFF5C" +
                    $"{chain.Lock4_SongID}\uFF5C" +
                    $"{chain.Lock4_SongDifficulty}\uFF5C" +
                    $"{chain.Lock4_CompletionMethod}\uFF5C" +
                    $"{chain.Lock4_Goal}\uFF5C" +
                    $"{chain.Lock5_SongID}\uFF5C" +
                    $"{chain.Lock5_SongDifficulty}\uFF5C" +
                    $"{chain.Lock5_CompletionMethod}\uFF5C" +
                    $"{chain.Lock5_Goal}\uFF5C"
                );
            }

            result.Add("EOF");

            return result.ToArray();
        }

        public class Chain
		{
			/// <summary>
			/// ID of this unlock chain in the list.
			/// </summary>
			public int ID { get; set; }

			/// <summary>
			/// Update ID.
			/// </summary>
			public int Update { get; set; }

			/// <summary>
			/// ID of the song, which NIGHTMARE chart is unlocked as a result of completing this chain.
			/// </summary>
			public int UnlockedSong { get; set; }

			public string DisplayedUnlockedSong { get; set; }
			public string DisplayedUnlockedDifficulty { get; set; }

			/// <summary>
			/// HACKER LEVEL.
			/// </summary>
			public int HackerLevel { get; set; }

			public string DisplayedHackerLevel
			{
				get 
				{
					if (HackerLevel == 10) return "[OTHER UNLOCK CONDITIONS]";

					string result = "";

					for (int i = 0; i < HackerLevel; i++)
					{
						result += '\u2B24';
					}

					return $"[{HackerLevel}] " + result.PadRight(5, '\u25CB');
				}
			}

			/// <summary>
			/// ID of the song used in the unlock chain.<br/>
			/// -1 if none.
			/// </summary>
			public int Lock1_SongID { get; set; }

			/// <summary>
			/// Difficulty of the song used in the unlock chain.<br/>
			/// Only LIGHT, MEDIUM and BEAST are used in the official sheet, but I think NIGHTMARE would work as well.
			/// </summary>
			public string Lock1_SongDifficulty { get; set; }

			public int DisplayedIndexLock1Difficulty
			{
				set
				{
					Lock1_SongDifficulty = value switch
					{
						0 => "LIGHT",
						1 => "MEDIUM",
						2 => "BEAST",
						3 => "NIGHT",
						_ => "-"
					};
				}
				get
				{
					return Lock1_SongDifficulty switch
					{
						"LIGHT" => 0,
						"MEDIUM" => 1,
						"BEAST" => 2,
						"NIGHT" => 3,
						"NIGHTMARE" => 3,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// Method of a lock completion.<br/><br/>
			/// SCORE - Achieve a score of Goal * 100000 or more.<br/>
			/// FULL - Achieve a Full Combo.
			/// </summary>
			public string Lock1_CompletionMethod { get; set; }

			public int DisplayedIndexLock1Method
			{
				set
				{
					Lock1_CompletionMethod = value switch
					{
						0 => "SCORE",
						1 => "FULL",
						_ => "-"
					};
				}
				get
				{
					return Lock1_CompletionMethod switch
					{
						"SCORE" => 0,
						"FULL" => 1,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// When Method is set to SCORE, Goal is the score needed for completion. When Method is set to FULL, Goal is set to 0.
			/// </summary>
			public int Lock1_Goal { get; set; }

			/// <summary>
			/// ID of the song used in the unlock chain.<br/>
			/// -1 if none.
			/// </summary>
			public int Lock2_SongID { get; set; }

			/// <summary>
			/// Difficulty of the song used in the unlock chain.<br/>
			/// Only LIGHT, MEDIUM and BEAST are used in the official sheet, but I think NIGHTMARE would work as well.
			/// </summary>
			public string Lock2_SongDifficulty { get; set; }

			public int DisplayedIndexLock2Difficulty
			{
				set
				{
					Lock2_SongDifficulty = value switch
					{
						0 => "LIGHT",
						1 => "MEDIUM",
						2 => "BEAST",
						3 => "NIGHT",
						_ => "-"
					};
				}
				get
				{
					return Lock2_SongDifficulty switch
					{
						"LIGHT" => 0,
						"MEDIUM" => 1,
						"BEAST" => 2,
						"NIGHT" => 3,
						"NIGHTMARE" => 3,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// Method of a lock completion.<br/><br/>
			/// SCORE - Achieve a score of Goal * 100000 or more.<br/>
			/// FULL - Achieve a Full Combo.
			/// </summary>
			public string Lock2_CompletionMethod { get; set; }

			public int DisplayedIndexLock2Method
			{
				set
				{
					Lock2_CompletionMethod = value switch
					{
						0 => "SCORE",
						1 => "FULL",
						_ => "-"
					};
				}
				get
				{
					return Lock2_CompletionMethod switch
					{
						"SCORE" => 0,
						"FULL" => 1,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// When Method is set to SCORE, Goal is the score needed for completion. When Method is set to FULL, Goal is set to 0.
			/// </summary>
			public int Lock2_Goal { get; set; }

			/// <summary>
			/// ID of the song used in the unlock chain.<br/>
			/// -1 if none.
			/// </summary>
			public int Lock3_SongID { get; set; }

			/// <summary>
			/// Difficulty of the song used in the unlock chain.<br/>
			/// Only LIGHT, MEDIUM and BEAST are used in the official sheet, but I think NIGHTMARE would work as well.
			/// </summary>
			public string Lock3_SongDifficulty { get; set; }

			public int DisplayedIndexLock3Difficulty
			{
				set
				{
					Lock3_SongDifficulty = value switch
					{
						0 => "LIGHT",
						1 => "MEDIUM",
						2 => "BEAST",
						3 => "NIGHT",
						_ => "-"
					};
				}
				get
				{
					return Lock3_SongDifficulty switch
					{
						"LIGHT" => 0,
						"MEDIUM" => 1,
						"BEAST" => 2,
						"NIGHT" => 3,
						"NIGHTMARE" => 3,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// Method of a lock completion.<br/><br/>
			/// SCORE - Achieve a score of Goal * 100000 or more.<br/>
			/// FULL - Achieve a Full Combo.
			/// </summary>
			public string Lock3_CompletionMethod { get; set; }

			public int DisplayedIndexLock3Method
			{
				set
				{
					Lock3_CompletionMethod = value switch
					{
						0 => "SCORE",
						1 => "FULL",
						_ => "-"
					};
				}
				get
				{
					return Lock3_CompletionMethod switch
					{
						"SCORE" => 0,
						"FULL" => 1,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// When Method is set to SCORE, Goal is the score needed for completion. When Method is set to FULL, Goal is set to 0.
			/// </summary>
			public int Lock3_Goal { get; set; }

			/// <summary>
			/// ID of the song used in the unlock chain.<br/>
			/// -1 if none.
			/// </summary>
			public int Lock4_SongID { get; set; }

			/// <summary>
			/// Difficulty of the song used in the unlock chain.<br/>
			/// Only LIGHT, MEDIUM and BEAST are used in the official sheet, but I think NIGHTMARE would work as well.
			/// </summary>
			public string Lock4_SongDifficulty { get; set; }

			public int DisplayedIndexLock4Difficulty
			{
				set
				{
					Lock4_SongDifficulty = value switch
					{
						0 => "LIGHT",
						1 => "MEDIUM",
						2 => "BEAST",
						3 => "NIGHT",
						_ => "-"
					};
				}
				get
				{
					return Lock4_SongDifficulty switch
					{
						"LIGHT" => 0,
						"MEDIUM" => 1,
						"BEAST" => 2,
						"NIGHT" => 3,
						"NIGHTMARE" => 3,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// Method of a lock completion.<br/><br/>
			/// SCORE - Achieve a score of Goal * 100000 or more.<br/>
			/// FULL - Achieve a Full Combo.
			/// </summary>
			public string Lock4_CompletionMethod { get; set; }

			public int DisplayedIndexLock4Method
			{
				set
				{
					Lock4_CompletionMethod = value switch
					{
						0 => "SCORE",
						1 => "FULL",
						_ => "-"
					};
				}
				get
				{
					return Lock4_CompletionMethod switch
					{
						"SCORE" => 0,
						"FULL" => 1,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// When Method is set to SCORE, Goal is the score needed for completion. When Method is set to FULL, Goal is set to 0.
			/// </summary>
			public int Lock4_Goal { get; set; }

			/// <summary>
			/// ID of the song used in the unlock chain.<br/>
			/// -1 if none.
			/// </summary>
			public int Lock5_SongID { get; set; }

			/// <summary>
			/// Difficulty of the song used in the unlock chain.<br/>
			/// Only LIGHT, MEDIUM and BEAST are used in the official sheet, but I think NIGHTMARE would work as well.
			/// </summary>
			public string Lock5_SongDifficulty { get; set; }

			public int DisplayedIndexLock5Difficulty
			{
				set
				{
					Lock5_SongDifficulty = value switch
					{
						0 => "LIGHT",
						1 => "MEDIUM",
						2 => "BEAST",
						3 => "NIGHT",
						_ => "-"
					};
				}
				get
				{
					return Lock5_SongDifficulty switch
					{
						"LIGHT" => 0,
						"MEDIUM" => 1,
						"BEAST" => 2,
						"NIGHT" => 3,
						"NIGHTMARE" => 3,
						_ => -1,
					};
				}
			}

			/// <summary>
			/// Method of a lock completion.<br/><br/>
			/// SCORE - Achieve a score of Goal * 100000 or more.<br/>
			/// FULL - Achieve a Full Combo.
			/// </summary>
			public string Lock5_CompletionMethod { get; set; }

			/// <summary>
			/// When Method is set to SCORE, Goal is the score needed for completion. When Method is set to FULL, Goal is set to 0.
			/// </summary>
			public int Lock5_Goal { get; set; }

			public int DisplayedIndexLock5Method
			{
				set
				{
					Lock5_CompletionMethod = value switch
					{
						0 => "SCORE",
						1 => "FULL",
						_ => "-"
					};
				}
				get
				{
					return Lock5_CompletionMethod switch
					{
						"SCORE" => 0,
						"FULL" => 1,
						_ => -1,
					};
				}
			}

			public void UpdateDisplayedHeader(MusicList musicList)
			{
				DisplayedUnlockedSong = musicList.GetTitleArtist(UnlockedSong);
			}
		}
	}
}
