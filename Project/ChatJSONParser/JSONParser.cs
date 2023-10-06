using System.Text.Json;
using Newtonsoft.Json;
using static Project.Models.ChatModel;

namespace Project.Chat
{
    public class JSONParser
    {
        public static List<T> ReadFromJSON<T>(string filePath)
        {
            CreateJSONFileIfNotExists(filePath); // create file if it doesn't exist

            List<T> items = new List<T>();
            using (FileStream file = File.OpenRead(filePath)) // Opens file and closes it when "using" is done
            {
                items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(file)!;
            }
            return items;
        }

        public static void AddJSONMessage(
            MessageData messageData,
            string path,
            List<MessageData> messages
        ) // Adds a message to the JSON file
        {
            var jsonData = System.IO.File.ReadAllText(path);
            messages.Clear();
            messages =
                JsonConvert.DeserializeObject<List<MessageData>>(jsonData)
                ?? new List<MessageData>(); // Possible generic type for task 7

            messages.Add(messageData);

            jsonData = JsonConvert.SerializeObject(messages);
            System.IO.File.WriteAllText(path, jsonData);
        }

        private static void CreateJSONFileIfNotExists(string filePath) // Creates a JSON file if it doesn't exist
        {
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    fs.Close();
                    File.WriteAllText(filePath, "[]");
                }
            }
        }
    }
}
