using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_SheetsEditor
{
	internal class MusicList
	{
		public ObservableCollection<Music> Songs = new ObservableCollection<Music>();

		public string GetTitleArtist(int id)
		{
            if (id < Songs.Count && id >= 0)
                return $"[ID: {id}] {Songs[id].SongArtist}   /   {Songs[id].Title}";
            else
                return "[INVALID SONG]";
		}

		public static MusicList ParseCSV(string[] data)
		{
            MusicList result = new MusicList();

			foreach(string entry in data)
			{
                //Line is a comment, skip parsing
                if (entry.StartsWith("//")) continue;

                //Line ends the sheet, stop parsing
                if (entry == "EOF") break;

                Music song = Music.ParseData(entry);

				result.Songs.Add(song);
			}

			return result;
		}

        public static string[] CreateCSV(MusicList musicList)
        {
            List<string> result = new List<string>();

            result.Add($"// MusicInfoData : {DateTime.Today.ToString("yyyy-MM-dd")}");


			foreach (Music song in musicList.Songs)
            {
                result.Add(
                    $"{song.ID}\uFF5C" +
                    $"{song.Title}\uFF5C" +
                    $"{song.SortTitle}\uFF5C" +
                    $"{song.Game}\uFF5C" +
                    $"{song.Category}\uFF5C" +
                    $"{song.File}\uFF5C" +
                    $"{song.Movie}\uFF5C" +
                    $"{song.Unknown1}\uFF5C" +
                    $"{song.MinimumBPM.ToString(new CultureInfo(App.LocaleName) { NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "," } })}\uFF5C" +
                    $"{song.MaximumBPM.ToString(new CultureInfo(App.LocaleName) { NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "," } })}\uFF5C" +
                    $"{song.DifficultyLight}\uFF5C" +
                    $"{song.DifficultyMedium}\uFF5C" +
                    $"{song.DifficultyBeast}\uFF5C" +
                    $"{song.DifficultyNightmare}\uFF5C" +
                    $"{song.SongArtist}\uFF5C" +
                    $"{song.MovieArtist}\uFF5C" +
                    $"{song.IllustrationArtist}\uFF5C" +
                    $"{song.Unknown2}\uFF5C" +
                    $"{song.Unknown3}\uFF5C" +
                    $"{song.Unknown4}\uFF5C" +
                    $"{song.License}\uFF5C" +
                    $"{song.UnlockingMethod}\uFF5C" +
                    $"{song.Update}\uFF5C" +
                    $"{song.Unknown6}\uFF5C" +
                    $"{song.EventHandler}\uFF5C" +
                    $"{song.Unknown7}\uFF5C" +
                    $"{song.MovieRegion}\uFF5C" +
                    $"{song.MovieReplacement}\uFF5C" +
                    $"{song.Series}\uFF5C" +
                    $"{song.AnimeSubtitle}"
                );
            }

            result.Add("EOF");

            return result.ToArray();
        }

        public class Music
        {
            /// <summary>
            /// ID of the song in the list.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Song title.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Song title written in full-width katagana. Used for sorting in game.
            /// </summary>
            public string SortTitle { get; set; }

            /// <summary>
            /// Which game this song made its appearance in. Either 1st or 2nd.
            /// </summary>
            public string Game { get; set; }

            /// <summary>
            /// Gategory/genre of the song.<br/><br/>
            /// ANIME - Anime songs.<br/>
            /// EXITTUNES - EXIT TUNES songs (mostly VOCALOID).<br/>
            /// TOHO - Touhou Project songs.<br/>
            /// KDE - Konami Original songs.<br/>
            /// OTHER - Variety.<br/>
            /// </summary>
            public string Category { get; set; }

            public int DisplayedCategory
            {
                set
                {
                    Category = value switch
                    {
                        0 => "ANIME",
                        1 => "EXITTUNES",
                        2 => "TOHO",
                        3 => "KDE",
                        4 => "OTHER",
                        _ => ""
                    };
                }
                get
                {
                    return Category switch
                    {
                        "ANIME" => 0,
                        "EXITTUNES" => 1,
                        "TOHO" => 2,
                        "KDE" => 3,
                        "OTHER" => 4,
                        _ => -1,
                    };
                }
            }

            /// <summary>
            /// Name of the chart files, excluding difficulty and extension.
            /// </summary>
            public string File { get; set; }

            /// <summary>
            /// Name of the background video file WITH its file extension.
            /// </summary>
            public string Movie { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unknown1 { get; set; }

            /// <summary>
            /// Lowest BPM of the song.
            /// </summary>
            public float MinimumBPM { get; set; }

            /// <summary>
            /// Highest BPM of the song.
            /// </summary>
            public float MaximumBPM { get; set; }

            public string DisplayedBPM
            {
                get
                {
                    if (MinimumBPM == MaximumBPM)
                        return MinimumBPM.ToString();
                    else
                        return $"{MinimumBPM} - {MaximumBPM}";
                }
            }

            /// <summary>
            /// Numerical difficulty of the song's LIGHT chart.<br/><br/>
            /// -1 to disable it.<br/>
            /// 99 for Kami.<br/>
            /// Difficulties 9 and 10 can have + or - signs added.
            /// </summary>
            public string DifficultyLight { get; set; }

            public string DisplayedDifficultyLight
            {
                get
                {
                    return DifficultyLight switch
                    {
                        "-1" => "",
                        "99" => "\u795E",
                        _ => DifficultyLight,
                    };
                }
            }

            public int DisplayedIndexDifficultyLight
            {
                set
                {
                    DifficultyLight = value switch
                    {
                        0 => "-1",
                        1 => "1",
                        2 => "2",
                        3 => "3",
                        4 => "4",
                        5 => "5",
                        6 => "6",
                        7 => "7",
                        8 => "8",
                        9 => "9-",
                        10 => "9",
                        11 => "9+",
                        12 => "10-",
                        13 => "10",
                        14 => "10+",
                        15 => "\u795E",
                        _ => "-1"
                    };
                }
                get
                {
                    return DifficultyLight switch
                    {
                        "-1" => 0,
                        "1" => 1,
                        "2" => 2,
                        "3" => 3,
                        "4" => 4,
                        "5" => 5,
                        "6" => 6,
                        "7" => 7,
                        "8" => 8,
                        "9-" => 9,
                        "9" => 10,
                        "9+" => 11,
                        "10-" => 12,
                        "10" => 13,
                        "10+" => 14,
                        "99" => 15,
                        _ => 0,
                    };
                }
            }

            /// <summary>
            /// Numerical difficulty of the song's MEDIUM chart.<br/><br/>
            /// -1 to disable it.<br/>
            /// 99 for Kami.<br/>
            /// Difficulties 9 and 10 can have + or - signs added.
            /// </summary>
			public string DifficultyMedium { get; set; }

            public string DisplayedDifficultyMedium
            {
                get
                {
                    return DifficultyMedium switch
                    {
                        "-1" => "",
                        "99" => "\u795E",
                        _ => DifficultyMedium,
                    };
                }
            }

            public int DisplayedIndexDifficultyMedium
            {
                set
                {
                    DifficultyMedium = value switch
                    {
                        0 => "-1",
                        1 => "1",
                        2 => "2",
                        3 => "3",
                        4 => "4",
                        5 => "5",
                        6 => "6",
                        7 => "7",
                        8 => "8",
                        9 => "9-",
                        10 => "9",
                        11 => "9+",
                        12 => "10-",
                        13 => "10",
                        14 => "10+",
                        15 => "\u795E",
                        _ => "-1"
                    };
                }
                get
                {
                    return DifficultyMedium switch
                    {
                        "-1" => 0,
                        "1" => 1,
                        "2" => 2,
                        "3" => 3,
                        "4" => 4,
                        "5" => 5,
                        "6" => 6,
                        "7" => 7,
                        "8" => 8,
                        "9-" => 9,
                        "9" => 10,
                        "9+" => 11,
                        "10-" => 12,
                        "10" => 13,
                        "10+" => 14,
                        "99" => 15,
                        _ => 0,
                    };
                }
            }

            /// <summary>
            /// Numerical difficulty of the song's BEAST chart.<br/><br/>
            /// -1 to disable it.<br/>
            /// 99 for Kami.<br/>
            /// Difficulties 9 and 10 can have + or - signs added.
            /// </summary>
			public string DifficultyBeast { get; set; }

            public string DisplayedDifficultyBeast
            {
                get
                {
                    return DifficultyBeast switch
                    {
                        "-1" => "",
                        "99" => "\u795E",
                        _ => DifficultyBeast,
                    };
                }
            }

            public int DisplayedIndexDifficultyBeast
            {
                set
                {
                    DifficultyBeast = value switch
                    {
                        0 => "-1",
                        1 => "1",
                        2 => "2",
                        3 => "3",
                        4 => "4",
                        5 => "5",
                        6 => "6",
                        7 => "7",
                        8 => "8",
                        9 => "9-",
                        10 => "9",
                        11 => "9+",
                        12 => "10-",
                        13 => "10",
                        14 => "10+",
                        15 => "\u795E",
                        _ => "-1"
                    };
                }
                get
                {
                    return DifficultyBeast switch
                    {
                        "-1" => 0,
                        "1" => 1,
                        "2" => 2,
                        "3" => 3,
                        "4" => 4,
                        "5" => 5,
                        "6" => 6,
                        "7" => 7,
                        "8" => 8,
                        "9-" => 9,
                        "9" => 10,
                        "9+" => 11,
                        "10-" => 12,
                        "10" => 13,
                        "10+" => 14,
                        "99" => 15,
                        _ => 0,
                    };
                }
            }

            /// <summary>
            /// Numerical difficulty of the song's NIGHTMARE chart.<br/><br/>
            /// -1 to disable it.<br/>
            /// 99 for Kami.<br/>
            /// Difficulties 9 and 10 can have + or - signs added.
            /// </summary>
			public string DifficultyNightmare { get; set; }

            public string DisplayedDifficultyNightmare
            {
                get
                {
                    return DifficultyNightmare switch
                    {
                        "-1" => "",
                        "99" => "\u795E",
                        _ => DifficultyNightmare,
                    };
                }
            }

            public int DisplayedIndexDifficultyNightmare
            {
                set
                {
                    DifficultyNightmare = value switch
                    {
                        0 => "-1",
                        1 => "1",
                        2 => "2",
                        3 => "3",
                        4 => "4",
                        5 => "5",
                        6 => "6",
                        7 => "7",
                        8 => "8",
                        9 => "9-",
                        10 => "9",
                        11 => "9+",
                        12 => "10-",
                        13 => "10",
                        14 => "10+",
                        15 => "\u795E",
                        _ => "-1"
                    };
                }
                get
                {
                    return DifficultyNightmare switch
                    {
                        "-1" => 0,
                        "1" => 1,
                        "2" => 2,
                        "3" => 3,
                        "4" => 4,
                        "5" => 5,
                        "6" => 6,
                        "7" => 7,
                        "8" => 8,
                        "9-" => 9,
                        "9" => 10,
                        "9+" => 11,
                        "10-" => 12,
                        "10" => 13,
                        "10+" => 14,
                        "99" => 15,
                        _ => 0,
                    };
                }
            }

            /// <summary>
            /// Artist of the song.
            /// </summary>
            public string SongArtist { get; set; }

            /// <summary>
            /// Artist of the background video.
            /// </summary>
            public string MovieArtist { get; set; }

            /// <summary>
            /// Artist of the illustration.
            /// </summary>
            public string IllustrationArtist { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public string Unknown2 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
			public string Unknown3 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
			public string Unknown4 { get; set; }

            /// <summary>
            /// License name.
            /// </summary>
            public string License { get; set; }

            /// <summary>
            /// Song's unlocking method.<br/><br/>
            /// UNLOCKED makes the song always available.<br/>
			/// DISABLED hides the song entirely.<br/>
			/// SP is used for events and other conditions.<br/>
			/// MAIL is BeatStream 1 only, used for Takahashi-san's Lab system.
            /// </summary>
            public string UnlockingMethod { get; set; }

            /// <summary>
            /// Update ID.
            /// </summary>
			public int Update { get; set; }

            /// <summary>
            /// Unknown. Values other than 0 only found on BEAST CRISIS songs.
            /// </summary>
			public int Unknown6 { get; set; }

            /// <summary>
            /// Internal event name.
            /// </summary>
            public string EventHandler { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
			public int Unknown7 { get; set; }

            /// <summary>
            /// Allowed region for background videos.<br/><br/>
			/// ALL allows video to be played in any region.<br/>
			/// JAPAN allows video to be played only in J region and instead uses MovieReplacement in other regions.
            /// </summary>
            public string MovieRegion { get; set; }

            /// <summary>
            /// Name of the replacement background video file WITH its file extension.
            /// </summary>
            public string MovieReplacement { get; set; }

            /// <summary>
            /// Series name. Seems to only be used in BeatStream 1.<br/><br/>
			/// NO - no series associated.<br/>
			/// BST - BeatStream.<br/>
			/// PRIM - Prim.<br/>
			/// HINA_B - Hinata Bitter Sweets (HinaBita).<br/>
			/// MIKA_G - MikaGura.<br/>
			/// ETO_T - Etotama.<br/>
			/// NOGAME - No Game - No Life.<br/>
			/// SEIREI - Sword Dance.<br/>
			/// MACHINE - Unbreakable Machine-doll.<br/>
			/// FALCOM - Falcom.<br/>
            /// </summary>
            public string Series { get; set; }

            /// <summary>
            /// Song subtitle. Only displayed for ANIME category songs.
            /// </summary>
            public string AnimeSubtitle { get; set; }

            /// <summary>
            /// Parses input
            /// </summary>
            /// <param name="data"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public static Music ParseData(string data, int id = -1)
            {
                string[] entry = data.Split('\uFF5C');

                Music song = new Music()
                {
                    ID = id == -1 ? int.Parse(entry[0]) : id,
                    Title = entry[1],
                    SortTitle = entry[2],
                    Game = entry[3],
                    Category = entry[4],
                    File = entry[5],
                    Movie = entry[6],
                    Unknown1 = entry[7] == "" ? 0 : int.Parse(entry[7]),
                    MinimumBPM = float.Parse(entry[8], new CultureInfo(App.LocaleName) { NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." } }),
                    MaximumBPM = float.Parse(entry[9], new CultureInfo(App.LocaleName) { NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." } }),
                    DifficultyLight = entry[10],
                    DifficultyMedium = entry[11],
                    DifficultyBeast = entry[12],
                    DifficultyNightmare = entry[13],
                    SongArtist = entry[14],
                    MovieArtist = entry[15],
                    IllustrationArtist = entry[16],
                    Unknown2 = entry[17],
                    Unknown3 = entry[18],
                    Unknown4 = entry[19],
                    License = entry[20],
                    UnlockingMethod = entry[21],
                    Update = int.Parse(entry[22]),
                    Unknown6 = int.Parse(entry[23]),
                    EventHandler = entry[24],
                    Unknown7 = int.Parse(entry[25]),
                    MovieRegion = entry[26],
                    MovieReplacement = entry[27],
                    Series = entry[28],
                    AnimeSubtitle = entry[29],
                };

                return song;
            }
        }
	}
}
