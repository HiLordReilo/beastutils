using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_SheetsEditor
{
    public class CourseList
    {
        public ObservableCollection<Entry> Entries { get; set; } = new ObservableCollection<Entry>();

        public class Entry
        {
            /// <summary>
            /// Course title.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// ID of the Update this course was added in.
            /// </summary>
            public int UpdateID { get; set; }

            /// <summary>
            /// ID of the 1st in the course.
            /// </summary>
            public int Song1_ID { get; set; }

            /// <summary>
            /// ID of the 2nd in the course.
            /// </summary>
            public int Song2_ID { get; set; }

            /// <summary>
            /// ID of the 3rd in the course.
            /// </summary>
            public int Song3_ID { get; set; }

            /// <summary>
            /// ID of the 4th in the course.
            /// </summary>
            public int Song4_ID { get; set; }

            /// <summary>
            /// Difficulty of the song used in the unlock chain.<br/>
            /// Can be L, M, B or NIGHT
            /// </summary>
            public string Song1_Difficulty { get; set; }

            public int DisplayedIndex_Song1_Difficulty
            {
                set
                {
                    Song1_Difficulty = value switch
                    {
                        0 => "L",
                        1 => "M",
                        2 => "B",
                        3 => "NIGHT",
                        _ => "L"
                    };
                }
                get
                {
                    return Song1_Difficulty switch
                    {
                        "L" => 0,
                        "M" => 1,
                        "B" => 2,
                        "NIGHT" => 3,
                        _ => -1,
                    };
                }
            }

            /// <summary>
            /// Difficulty of the song used in the unlock chain.<br/>
            /// Can be L, M, B or NIGHT
            /// </summary>
            public string Song2_Difficulty { get; set; }

            public int DisplayedIndex_Song2_Difficulty
            {
                set
                {
                    Song2_Difficulty = value switch
                    {
                        0 => "L",
                        1 => "M",
                        2 => "B",
                        3 => "NIGHT",
                        _ => "L"
                    };
                }
                get
                {
                    return Song2_Difficulty switch
                    {
                        "L" => 0,
                        "M" => 1,
                        "B" => 2,
                        "NIGHT" => 3,
                        _ => -1,
                    };
                }
            }

            /// <summary>
            /// Difficulty of the song used in the unlock chain.<br/>
            /// Can be L, M, B or NIGHT
            /// </summary>
            public string Song3_Difficulty { get; set; }

            public int DisplayedIndex_Song3_Difficulty
            {
                set
                {
                    Song3_Difficulty = value switch
                    {
                        0 => "L",
                        1 => "M",
                        2 => "B",
                        3 => "NIGHT",
                        _ => "L"
                    };
                }
                get
                {
                    return Song3_Difficulty switch
                    {
                        "L" => 0,
                        "M" => 1,
                        "B" => 2,
                        "NIGHT" => 3,
                        _ => -1,
                    };
                }
            }

            /// <summary>
            /// Difficulty of the song used in the unlock chain.<br/>
            /// Can be L, M, B or NIGHT
            /// </summary>
            public string Song4_Difficulty { get; set; }

            public int DisplayedIndex_Song4_Difficulty
            {
                set
                {
                    Song4_Difficulty = value switch
                    {
                        0 => "L",
                        1 => "M",
                        2 => "B",
                        3 => "NIGHT",
                        _ => "L"
                    };
                }
                get
                {
                    return Song4_Difficulty switch
                    {
                        "L" => 0,
                        "M" => 1,
                        "B" => 2,
                        "NIGHT" => 3,
                        _ => -1,
                    };
                }
            }

            /// <summary>
            /// Name of the big icon image.
            /// </summary>
            public string BigIconName { get; set; }

            /// <summary>
            /// Name of the small icon image.
            /// </summary>
            public string SmallIconName { get; set; }

            /// <summary>
            /// Type of the course. Currently only known possible value is RANK
            /// </summary>
            public string Type { get; set; }
            
            /// <summary>
            /// Reward player gets for completing the course.<br/>
            /// If CourseType is set to RANK, player will get assigned a new rank
            /// </summary>
            public int Reward { get; set; }

            public string DisplayedReward
            {
                get
                {
                    if (Reward <= 0)
                        return "No reward";

                    switch(Type)
                    {
                        case "RANK":
                            if (Reward == 16)
                                return "Beast Rank \u795E";
                            else
                                return $"Beast Rank {Reward}";
                        default:
                            return Reward.ToString();
                    }
                }
            }

            public static Entry ParseData(string[] entry)
            {
                Entry course = new Entry()
                {
                    Title = entry[0],
                    UpdateID = int.Parse(entry[1]),
                    Song1_ID = int.Parse(entry[2]),
                    Song2_ID = int.Parse(entry[3]),
                    Song3_ID = int.Parse(entry[4]),
                    Song4_ID = int.Parse(entry[5]),
                    Song1_Difficulty = entry[6],
                    Song2_Difficulty = entry[7],
                    Song3_Difficulty = entry[8],
                    Song4_Difficulty = entry[9],
                    BigIconName = entry[10],
                    SmallIconName = entry[11],
                    Type = entry[12],
                    Reward = int.Parse(entry[13])
                };

                return course;
            }
        }

        public static CourseList ParseCSV(string data)
        {
            CourseList result = new CourseList();

            string[] splitData;
            splitData = data.Split('\n', '\uFF5C');

            List<string> builtEntry = new List<string>();
            foreach (string dataSeg in splitData)
            {
                // Line is a header, skip parsing
                if (dataSeg.StartsWith("// CourseInfoData")) continue;
                // Line is empty, skip parsing to avoid problems
                if (string.IsNullOrEmpty(dataSeg)) continue;
                // Line ends the sheet, stop parsing
                if (dataSeg.StartsWith("EOF")) break;

                builtEntry.Add(dataSeg);

                if(builtEntry.Count == 14)
                {
                    Entry newEntry = Entry.ParseData(builtEntry.ToArray());

                    result.Entries.Add(newEntry);

                    builtEntry.Clear();
                }
            }

            return result;
        }

        public static string CreateCSV(CourseList courseList)
        {
            string result = $"// CourseInfoData : {DateTime.Today.ToString("yyyy-MM-dd")}\n";

            foreach (Entry course in courseList.Entries)
            {
                result +=   // KONMAI did really weird thing here, where they mix line breaks and pipe characters as separators
                            // It seems that song IDs and their difficulties are separated by line breaks individually,
                            // but whole info chunks and entries are separated by pipes.
                            //
                            // john komani was smoking some really hard shit while making this game eh? why make file formats so fucked up....
                    $"{course.Title}\uFF5C" +
                    $"{course.UpdateID}\uFF5C" +
                    // Song IDs
                    $"{course.Song1_ID}\n" +
                    $"{course.Song2_ID}\n" +
                    $"{course.Song3_ID}\n" +
                    $"{course.Song4_ID}\uFF5C" +
                    // Song diffs
                    $"{course.Song1_Difficulty}\n" +
                    $"{course.Song2_Difficulty}\n" +
                    $"{course.Song3_Difficulty}\n" +
                    $"{course.Song4_Difficulty}\uFF5C" +
                    //Other stuff
                    $"{course.BigIconName}\uFF5C" +
                    $"{course.SmallIconName}\uFF5C" +
                    $"{course.Type}\uFF5C" +
                    $"{course.Reward}\uFF5C";
            }

            result += "EOF";

            return result;
        }
    }
}
