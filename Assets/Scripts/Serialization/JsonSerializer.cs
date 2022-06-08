using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Utility
{
    public static class JsonSerializer 
    {
        public static void SaveJson(string jsonString, string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(jsonString);
            }
        }

        public static string LoadJson(string filePath)
        {
            if(File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string jsonString = reader.ReadToEnd();

                    return jsonString;
                }
            }
            return null;
        }
    }
}

