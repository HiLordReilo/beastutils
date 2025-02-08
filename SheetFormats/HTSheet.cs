using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_SheetsEditor
{
    public class HTSheet
    {
        public ObservableCollection<Entry> Entries { get; set; } = new ObservableCollection<Entry>();

        public class Entry
        {
            /// <summary>
            /// Name of the song.
            /// </summary>
            public string SongName { get; set; }
            /// <summary>
            /// Determines if HIGH TENSION effect is enabled in the song
            /// </summary>
            public bool IsEnabled { get; set; }
            /// <summary>
            /// ParticleMotion file for the Tap note
            /// </summary>
            public string PM_Tap { get; set; }
            /// <summary>
            /// Texture file for the Tap note
            /// </summary>
            public string TEX_Tap { get; set; }
            /// <summary>
            /// ParticleMotion file for the Hold note
            /// </summary>
            public string PM_Hold { get; set; }
            /// <summary>
            /// Texture file for the Hold note
            /// </summary>
            public string TEX_Hold { get; set; }
            /// <summary>
            /// ParticleMotion file for the Ripple note
            /// </summary>
            public string PM_Ripple { get; set; }
            /// <summary>
            /// Texture file for the Ripple note
            /// </summary>
            public string TEX_Ripple { get; set; }
            /// <summary>
            /// ParticleMotion file for the Slash note
            /// </summary>
            public string PM_Slash { get; set; }
            /// <summary>
            /// Texture file for the Slash note
            /// </summary>
            public string TEX_Slash { get; set; }
            /// <summary>
            /// ParticleMotion file for the Stream note
            /// </summary>
            public string PM_Stream { get; set; }
            /// <summary>
            /// Texture file for the Stream note
            /// </summary>
            public string TEX_Stream { get; set; }

            public static Entry ParseData(string data)
            {
                string[] entry;
                
                // Because Discord replaces Tabs (\t) with 4 spaces, we should check which way we should parse the data
                // thanks discord really cool
                if(data.Contains('\t'))
                    entry = data.Split('\t');
                else
                    entry = data.Split("    ");

                Entry song = new Entry()
                {
                    SongName = entry[0],
                    IsEnabled = entry[1] == "ON",
                    PM_Tap = entry[2],
                    TEX_Tap = entry[3],
                    PM_Hold = entry[4],
                    TEX_Hold = entry[5],
                    PM_Ripple = entry[6],
                    TEX_Ripple = entry[7],
                    PM_Slash = entry[8],
                    TEX_Slash = entry[9],
                    PM_Stream = entry[10],
                    TEX_Stream = entry[11]
                };

                return song;
            }
        }

        public static HTSheet ParseTXT(string[] data)
        {
            HTSheet result = new HTSheet();

            foreach (string entry in data)
            {
                // Line is a header, skip parsing
                if (entry.StartsWith("楽曲名")) continue;
                // Line is empty, skip parsing to avoid problems
                if (string.IsNullOrEmpty(entry)) continue;

                Entry newEntry = Entry.ParseData(entry);

                result.Entries.Add(newEntry);
            }

            return result;
        }

        public static string[] CreateTXT(HTSheet htSheet)
        {
            List<string> result = new List<string>();

            result.Add("楽曲名\tON/OFF\tNOMAL_PM\tNOMAL_TEX\tLONG_PM\tLONG_TEX\tRIPPLE_PM\tRIPPLE_TEX\tSLASH_PM\tSLASH_TEX\tSTREAM_PM\tSTREAM_TEX");

            foreach (Entry e in htSheet.Entries)
            {
                result.Add($"{e.SongName}\t" +
                    $"{(e.IsEnabled ? "ON" : "OFF")}\t" +
                    $"{e.PM_Tap}\t" +
                    $"{e.TEX_Tap}\t" +
                    $"{e.PM_Hold}\t" +
                    $"{e.TEX_Hold}\t" +
                    $"{e.PM_Ripple}\t" +
                    $"{e.TEX_Ripple}\t" +
                    $"{e.PM_Slash}\t" +
                    $"{e.TEX_Slash}\t" +
                    $"{e.PM_Stream}\t" +
                    $"{e.TEX_Stream}");
            }

            return result.ToArray();
        }
    }
}
