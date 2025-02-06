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
            { "StreamPrefab", "/data/stream/" },
            { "ParticleMotion", "/data2/particle/motionfile/" },
        };
    }
}
