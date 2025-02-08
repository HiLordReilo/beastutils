using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST_SheetsEditor
{
    internal static class Util
    {
        public static Dictionary<string, string> SheetPaths = new Dictionary<string, string>()
        {
            { "BST1_MusicList", "/data/sound/musiclist.csv" },
            { "BST1_HackerList", "/data/others/hacker_list.csv" },
            { "BST1_CourseList", "/data/others/courselist.csv" },
            { "BST1_LabMailList", "/data/others/mail_list.csv" },
            { "BST1_ChallengeList", "/data/others/challenge_list.csv" },
            { "BST1_ChatEmojiList", "/data/others/chat_asci_list.csv" },
            { "BST1_ChatMessageList", "/data/others/chat_comment_list.csv" },
            { "BST1_ChatStickerList", "/data/others/chat_seal_list.csv" },
            { "BST2_MusicList", "/data2/sound/musiclist.csv" },
            { "BST2_HackerList", "/data2/others/hacker_list.csv" },
            { "BST2_CourseList", "/data2/others/courselist.csv" },
            { "BST2_CrisisList", "/data2/others/crysislist.csv" },
            { "BST2_CharaList", "/data2/others/chara_list.csv" },
            { "BST2_HTSheet", "/data2/sound/ht_sheat.txt" },
        };

        public static string GetFullDiffName(int id)
        {
            switch(id)
            {
                default:
                    return "INVALID DIFFICULTY";
                case 0:
                    return "LIGHT";
                case 1:
                    return "MEDIUM";
                case 2:
                    return "BEAST";
                case 3:
                    return "NIGHTMARE";
            }
        }

        public static string GetFullDiffName(string id)
        {
            switch(id)
            {
                default:
                    return "INVALID DIFFICULTY";
                case "L":
                    return "LIGHT";
                case "LIGHT":
                    return "LIGHT";
                case "M":
                    return "MEDIUM";
                case "MED":
                    return "MEDIUM";
                case "MEDIUM":
                    return "MEDIUM";
                case "B":
                    return "BEAST";
                case "BEAST":
                    return "BEAST";
                case "N":
                    return "NIGHTMARE";
                case "NIGHT":
                    return "NIGHTMARE";
                case "NIGHTMARE":
                    return "NIGHTMARE";
            }
        }
    }
}
