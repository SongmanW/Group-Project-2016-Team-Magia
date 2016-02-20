using System.Text.RegularExpressions;

namespace GDLibrary.Utility
{
    public class StringUtility
    {
        //parse a file name from a path + name string
        public static string ParseNameFromPath(string path)
        {
            return Regex.Match(path, @"[^\\/]*$").Value;
        }
    }
}
