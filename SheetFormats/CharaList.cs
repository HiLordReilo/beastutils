using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_SheetsEditor
{
    public class CharaList
    {
        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();

        public static CharaList ParseCSV(string data)
        {
            CharaList result = new CharaList();

            List<string> splitData;
            splitData = data.Split('\n').ToList();

            int exclude = 0;
            for(int i = 0; i < splitData.Count; i++)
            {
                // Line is a header, skip parsing
                if (splitData[i - exclude].StartsWith("// CharacterData"))
                {
                    splitData.RemoveAt(i - exclude);
                    exclude++;
                    continue;
                }
                // Line is a header, skip parsing
                // Line ends the sheet, stop parsing
                // Why trim? Refer to similar section in MusicList script
                if (splitData[i - exclude].Trim() == "EOF")
                {
                    splitData.RemoveAt(i - exclude);
                    break;
                }
            }

            string pureData = string.Join("\n", splitData.ToArray());
            splitData = pureData.Split('\uFF5C').ToList();

            List<string> builtEntry = new List<string>();
            foreach (string dataSeg in splitData)
            {
                builtEntry.Add(dataSeg);

                if (builtEntry.Count == 53)
                {
                    Character newEntry = Character.ParseData(builtEntry.ToArray());

                    result.Characters.Add(newEntry);

                    builtEntry.Clear();
                }
            }

            return result;
        }

        public static string CreateCSV(CharaList charaList)
        {
            List<string> contents = new List<string>();

            foreach (Character chara in charaList.Characters)
            {
                contents.Add((
                    $"{chara.Name}\uFF5C" +
                    $"{chara.UnlockMethod}\uFF5C" +
                    $"{chara.UnlockStage}\uFF5C" +
                    $"{chara.Description}\uFF5C" +
                    $"{chara.ButtonTexture}\uFF5C" +
                    $"{chara.PlateIconTexture}\uFF5C" +
                    $"{chara.MainTexture}\uFF5C" +
                    $"{chara.DefaultFaceTexture}\uFF5C" +
                    $"{chara.ResponsePoke1}\uFF5C" +
                    $"{chara.ResponsePoke2}\uFF5C" +
                    $"{chara.ResponsePoke3}\uFF5C" +
                    $"{chara.ResponsePoke4}\uFF5C" +
                    $"{chara.ResponsePoke5}\uFF5C" +
                    $"{chara.ResponsePoke6}\uFF5C" +
                    $"{chara.FacePoke1}\uFF5C" +
                    $"{chara.FacePoke2}\uFF5C" +
                    $"{chara.FacePoke3}\uFF5C" +
                    $"{chara.FacePoke4}\uFF5C" +
                    $"{chara.FacePoke5}\uFF5C" +
                    $"{chara.FacePoke6}\uFF5C" +
                    $"{chara.ResponseWin1}\uFF5C" +
                    $"{chara.ResponseWin2}\uFF5C" +
                    $"{chara.ResponseWin3}\uFF5C" +
                    $"{chara.ResponseWin4}\uFF5C" +
                    $"{chara.ResponseWin5}\uFF5C" +
                    $"{chara.ResponseWin6}\uFF5C" +
                    $"{chara.FaceWin1}\uFF5C" +
                    $"{chara.FaceWin2}\uFF5C" +
                    $"{chara.FaceWin3}\uFF5C" +
                    $"{chara.FaceWin4}\uFF5C" +
                    $"{chara.FaceWin5}\uFF5C" +
                    $"{chara.FaceWin6}\uFF5C" +
                    $"{chara.ResponseMatching1}\uFF5C" +
                    $"{chara.ResponseMatching2}\uFF5C" +
                    $"{chara.ResponseMatching3}\uFF5C" +
                    $"{chara.ResponseMatching4}\uFF5C" +
                    $"{chara.ResponseMatching5}\uFF5C" +
                    $"{chara.ResponseMatching6}\uFF5C" +
                    $"{chara.FaceMatching1}\uFF5C" +
                    $"{chara.FaceMatching2}\uFF5C" +
                    $"{chara.FaceMatching3}\uFF5C" +
                    $"{chara.FaceMatching4}\uFF5C" +
                    $"{chara.FaceMatching5}\uFF5C" +
                    $"{chara.FaceMatching6}\uFF5C" +
                    $"{chara.ResponseEnd1}\uFF5C" +
                    $"{chara.ResponseEnd2}\uFF5C" +
                    $"{chara.ResponseEnd3}\uFF5C" +
                    $"{chara.ResponseEnd4}\uFF5C" +
                    $"{chara.ResponseEnd5}\uFF5C" +
                    $"{chara.ResponseEnd6}\uFF5C" +
                    $"{chara.ColorR}\uFF5C" +
                    $"{chara.ColorG}\uFF5C" +
                    $"{chara.ColorB}").TrimEnd()  // <- Without Trimming, the list will just have extra linebreaks in-between entries.
                                                        //    The game still accepts the resulting files, but those linebreaks are just unnecessary bloat.
                                                        //    Does CRLF thing have to do anything with this? Or is it just dotnet/C# being quirky?
                );
            }

            contents.Add("EOF");

            return $"// CharacterData : {DateTime.Today.ToString("yyyy-MM-dd")}\n" + string.Join('\uFF5C', contents.ToArray());
        }

        public class Character
        {
            /// <summary>
            /// Character name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Unlock method.<br/>
            /// def is unlocked by default.
            /// </summary>
            public string UnlockMethod { get; set; }

            /// <summary>
            /// Stage of unlock chain. Used with some unlock methods, but -1 in other cases.
            /// </summary>
            public int UnlockStage { get; set; }

            /// <summary>
            /// Character description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Texture of a button in the Aibou selection window.
            /// </summary>
            public string ButtonTexture { get; set; }

            /// <summary>
            /// Texture of a character, holding the score plate in menus and gameplay during matching.
            /// </summary>
            public string PlateIconTexture { get; set; }

            /// <summary>
            /// Texture of a character on result screen. Always visible.
            /// </summary>
            public string MainTexture { get; set; }

            /// <summary>
            /// Texture of an initial face of a character on result screen.
            /// </summary>
            public string DefaultFaceTexture { get; set; }

            /// <summary>
            /// Character response to poking in menus and result screen.
            /// </summary>
            public string ResponsePoke1 { get; set; }

            /// <summary>
            /// Character response to poking in menus and result screen.
            /// </summary>
            public string ResponsePoke2 { get; set; }

            /// <summary>
            /// Character response to poking in menus and result screen.
            /// </summary>
            public string ResponsePoke3 { get; set; }

            /// <summary>
            /// Character response to poking in menus and result screen.
            /// </summary>
            public string ResponsePoke4 { get; set; }

            /// <summary>
            /// Character response to poking in menus and result screen.
            /// </summary>
            public string ResponsePoke5 { get; set; }

            /// <summary>
            /// Character response to poking in menus and result screen.
            /// </summary>
            public string ResponsePoke6 { get; set; }

            /// <summary>
            /// Texture of character's face in response to poking at result screen.
            /// </summary>
            public string FacePoke1 { get; set; }

            /// <summary>
            /// Texture of character's face in response to poking at result screen.
            /// </summary>
            public string FacePoke2 { get; set; }

            /// <summary>
            /// Texture of character's face in response to poking at result screen.
            /// </summary>
            public string FacePoke3 { get; set; }

            /// <summary>
            /// Texture of character's face in response to poking at result screen.
            /// </summary>
            public string FacePoke4 { get; set; }

            /// <summary>
            /// Texture of character's face in response to poking at result screen.
            /// </summary>
            public string FacePoke5 { get; set; }

            /// <summary>
            /// Texture of character's face in response to poking at result screen.
            /// </summary>
            public string FacePoke6 { get; set; }

            /// <summary>
            /// Character response to winning a match.
            /// </summary>
            public string ResponseWin1 { get; set; }

            /// <summary>
            /// Character response to winning a match.
            /// </summary>
            public string ResponseWin2 { get; set; }

            /// <summary>
            /// Character response to winning a match.
            /// </summary>
            public string ResponseWin3 { get; set; }

            /// <summary>
            /// Character response to winning a match.
            /// </summary>
            public string ResponseWin4 { get; set; }

            /// <summary>
            /// Character response to winning a match.
            /// </summary>
            public string ResponseWin5 { get; set; }

            /// <summary>
            /// Character response to winning a match.
            /// </summary>
            public string ResponseWin6 { get; set; }

            /// <summary>
            /// Texture of character's face in response to winning a match.
            /// </summary>
            public string FaceWin1 { get; set; }

            /// <summary>
            /// Texture of character's face in response to winning a match.
            /// </summary>
            public string FaceWin2 { get; set; }

            /// <summary>
            /// Texture of character's face in response to winning a match.
            /// </summary>
            public string FaceWin3 { get; set; }

            /// <summary>
            /// Texture of character's face in response to winning a match.
            /// </summary>
            public string FaceWin4 { get; set; }

            /// <summary>
            /// Texture of character's face in response to winning a match.
            /// </summary>
            public string FaceWin5 { get; set; }

            /// <summary>
            /// Texture of character's face in response to winning a match.
            /// </summary>
            public string FaceWin6 { get; set; }

            /// <summary>
            /// Character response to finishing a song in matching.
            /// </summary>
            public string ResponseMatching1 { get; set; }

            /// <summary>
            /// Character response to finishing a song in matching.
            /// </summary>
            public string ResponseMatching2 { get; set; }

            /// <summary>
            /// Character response to finishing a song in matching.
            /// </summary>
            public string ResponseMatching3 { get; set; }

            /// <summary>
            /// Character response to finishing a song in matching.
            /// </summary>
            public string ResponseMatching4 { get; set; }

            /// <summary>
            /// Character response to finishing a song in matching.
            /// </summary>
            public string ResponseMatching5 { get; set; }

            /// <summary>
            /// Character response to finishing a song in matching.
            /// </summary>
            public string ResponseMatching6 { get; set; }

            /// <summary>
            /// Texture of character's face in response to finishing a song in matching.
            /// </summary>
            public string FaceMatching1 { get; set; }

            /// <summary>
            /// Texture of character's face in response to finishing a song in matching.
            /// </summary>
            public string FaceMatching2 { get; set; }

            /// <summary>
            /// Texture of character's face in response to finishing a song in matching.
            /// </summary>
            public string FaceMatching3 { get; set; }

            /// <summary>
            /// Texture of character's face in response to finishing a song in matching.
            /// </summary>
            public string FaceMatching4 { get; set; }

            /// <summary>
            /// Texture of character's face in response to finishing a song in matching.
            /// </summary>
            public string FaceMatching5 { get; set; }

            /// <summary>
            /// Texture of character's face in response to finishing a song in matching.
            /// </summary>
            public string FaceMatching6 { get; set; }

            /// <summary>
            /// Character response to finishing a session.
            /// </summary>
            public string ResponseEnd1 { get; set; }

            /// <summary>
            /// Character response to finishing a session.
            /// </summary>
            public string ResponseEnd2 { get; set; }

            /// <summary>
            /// Character response to finishing a session.
            /// </summary>
            public string ResponseEnd3 { get; set; }

            /// <summary>
            /// Character response to finishing a session.
            /// </summary>
            public string ResponseEnd4 { get; set; }

            /// <summary>
            /// Character response to finishing a session.
            /// </summary>
            public string ResponseEnd5 { get; set; }

            /// <summary>
            /// Character response to finishing a session.
            /// </summary>
            public string ResponseEnd6 { get; set; }

            /// <summary>
            /// Character response window color.
            /// </summary>
            public int ColorR { get; set; }

            /// <summary>
            /// Character response window color.
            /// </summary>
            public int ColorG { get; set; }

            /// <summary>
            /// Character response window color.
            /// </summary>
            public int ColorB { get; set; }

            /// <summary>
            /// Parses input
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static Character ParseData(string[] data)
            {
                Character chara = new Character()
                {
                    Name = data[0],
                    UnlockMethod = data[1],
                    UnlockStage = int.Parse(data[2]),
                    Description = data[3],
                    ButtonTexture = data[4],
                    PlateIconTexture = data[5],
                    MainTexture = data[6],
                    DefaultFaceTexture = data[7],
                    ResponsePoke1 = data[8],
                    ResponsePoke2 = data[9],
                    ResponsePoke3 = data[10],
                    ResponsePoke4 = data[11],
                    ResponsePoke5 = data[12],
                    ResponsePoke6 = data[13],
                    FacePoke1 = data[14],
                    FacePoke2 = data[15],
                    FacePoke3 = data[16],
                    FacePoke4 = data[17],
                    FacePoke5 = data[18],
                    FacePoke6 = data[19],
                    ResponseWin1 = data[20],
                    ResponseWin2 = data[21],
                    ResponseWin3 = data[22],
                    ResponseWin4 = data[23],
                    ResponseWin5 = data[24],
                    ResponseWin6 = data[25],
                    FaceWin1 = data[26],
                    FaceWin2 = data[27],
                    FaceWin3 = data[28],
                    FaceWin4 = data[29],
                    FaceWin5 = data[30],
                    FaceWin6 = data[31],
                    ResponseMatching1 = data[32],
                    ResponseMatching2 = data[33],
                    ResponseMatching3 = data[34],
                    ResponseMatching4 = data[35],
                    ResponseMatching5 = data[36],
                    ResponseMatching6 = data[37],
                    FaceMatching1 = data[38],
                    FaceMatching2 = data[39],
                    FaceMatching3 = data[40],
                    FaceMatching4 = data[41],
                    FaceMatching5 = data[42],
                    FaceMatching6 = data[43],
                    ResponseEnd1 = data[44],
                    ResponseEnd2 = data[45],
                    ResponseEnd3 = data[46],
                    ResponseEnd4 = data[47],
                    ResponseEnd5 = data[48],
                    ResponseEnd6 = data[49],
                    ColorR = int.Parse(data[50]),
                    ColorG = int.Parse(data[51]),
                    ColorB = int.Parse(data[52]),
                };

                return chara;
            }
        }
    }
}
