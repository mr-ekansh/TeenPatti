using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialApp
{
    public class Utils
    {
        static public string REMEMBER_ME = "REMEMBER_ME";
        static public string LOGGED = "LOGGED"; //1: Email	2: Google	3:Facebook
        static public int NONE = 0;
        static public int EM = 1;
        static public int GG = 2;
        static public int FB = 3;
        static public int TW = 4;
        static public int AM = 5;
        static public int PH = 6;
        public static string GetFileExtension(string _url)
        {
            string path = _url;
            string[] splitsPath = path.Split('/');
            string[] splitsLast = splitsPath[splitsPath.Length - 1].Split('.');
            return splitsLast[splitsLast.Length - 1];
        }
    }
}
