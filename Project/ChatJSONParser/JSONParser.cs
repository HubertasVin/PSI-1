using System.Text.Json;
using Newtonsoft.Json;
using static Project.Models.ChatModel;
using static Extensions.JsonExtensions;

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
            messages.AddJsonToList(path); //TODO paaiskinti kaip veikia
            /*
            Explanation: AddJsonToList method is generic type method (that means we should specify a type of the list)
            but in this instance, it takes the type of messages at compile time, so we don't need to specify it (better to specify it for readability)
            */
            messages.Add(messageData);

            jsonData = JsonConvert.SerializeObject(messages);
            System.IO.File.WriteAllText(path, jsonData);
        }

        public static void AddJSONMessageOptional(List<MessageData> messages, string user = "Guest", string message = "Empty message",
            DateTime? timestamp = null)
        {
            if (timestamp == null)
                timestamp = new DateTime(2000, 1, 1);
            var chatMessage = new MessageData
            {
                User = user,
                Message = message,
                Timestamp = timestamp.Value
            };
            AddJSONMessage(chatMessage, "src/SignalRData.json", messages);
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
