using System.Configuration;
using System.Data;
using System.Windows;

namespace BST_SheetsEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Apparently Windows 8.1 (and maybe earlier versions as well) uses "ja-JP" for Japanese locale name
        // while Windows 10 uses "jp-JP"
        // no clue about Windows 11, I don't use this piece of shit
        // neither about Windows 7. OS is fine, but I grew up with 8
        //
        // but I digress
        public static string LocaleName = Environment.OSVersion.Version.Major < 10 ? "ja-JP" : "jp-JP";
    }

}
