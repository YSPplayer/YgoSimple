using System;
using System.IO;
using UnityEngine;

namespace TCGame.Client
{
    public class Log
    {
        private static string default_path = $"{Application.dataPath}/Resources/Log/log.txt";

        public static void WriteLog(string message,string path = "")
        {
            if (path.Equals("")) path = default_path;
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"{currentTime} - {message}";
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(logEntry);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

        }



    }

}