using DataClasses;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Helpers
{
    public class SaveHelper
    {
        public static async Task SaveToJsonAsync<T>(T objectToSave, string folder, string fileName) where T : ISavable
        {
            if (string.IsNullOrEmpty(folder) == false && string.IsNullOrEmpty(folder)) folder = "_";
            if (string.IsNullOrEmpty(fileName) == false && string.IsNullOrEmpty(fileName)) throw new System.ArgumentNullException(nameof(fileName), "file name is null");

            string path = GetFullFolderName(folder) + "/";

            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);

            FileStream fileSteam = new FileStream(path + fileName + ".json", FileMode.Create);

            using (StreamWriter writer = new StreamWriter(fileSteam)) await writer.WriteAsync(JsonConvert.SerializeObject(objectToSave));
        }

        public static void SaveToJson<T>(T objectToSave, string folder, string fileName) where T : ISavable
        {
            if (string.IsNullOrEmpty(folder) == false && string.IsNullOrEmpty(folder)) folder = "_";
            if (string.IsNullOrEmpty(fileName) == false && string.IsNullOrEmpty(fileName)) throw new System.ArgumentNullException(nameof(fileName), "file name is null");

            string path = GetFullFolderName(folder) + "/";

            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);

            FileStream fileSteam = new FileStream(path + fileName + ".json", FileMode.Create);

            using (StreamWriter writer = new StreamWriter(fileSteam)) writer.Write(JsonConvert.SerializeObject(objectToSave));
        }

        public static async Task<T> GetStoredDataClassAsync<T>(string folder, string fileName) where T : ISavable
        {
            if (string.IsNullOrEmpty(folder) == false && string.IsNullOrEmpty(folder)) folder = "_";
            if (string.IsNullOrEmpty(fileName) == false && string.IsNullOrEmpty(fileName)) throw new System.ArgumentNullException(nameof(fileName), "file name is null");

            string file = GetFullFolderName(folder) + "/" + fileName + ".json";

            if (File.Exists(file)) using (StreamReader reader = new StreamReader(file)) return JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync()); else return default(T);
        }

        public static T GetStoredDataClass<T>(string folder, string fileName) where T : ISavable
        {
            if (string.IsNullOrEmpty(folder) == false && string.IsNullOrEmpty(folder)) folder = "_";
            if (string.IsNullOrEmpty(fileName) == false && string.IsNullOrEmpty(fileName)) throw new System.ArgumentNullException(nameof(fileName), "file name is null");

            string file = GetFullFolderName(folder) + "/" + fileName + ".json";

            if (File.Exists(file)) using (StreamReader reader = new StreamReader(file)) return JsonConvert.DeserializeObject<T>(reader.ReadToEnd()); else return default(T);
        }

        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        private static string GetFullFolderName(string folder)
        {
            return Application.persistentDataPath + "/" + folder;
        }
    }
}