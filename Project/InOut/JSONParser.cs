using System.Text.Json;
using Newtonsoft.Json;
using static Project.Models.InOutModel;

namespace Project.InOut
{
    public class JSONParser
    {
        // Task iterate through collections
        public static IEnumerable<MessageData> ReadMessagesFromJSON(string filePath)
        {
            CreateJSONFileIfNotExists(filePath); // create file if it doesn't exist

            using (FileStream file = File.OpenRead(filePath)) // Opens file and closes it when "using" is done
            {
                JsonElement json = JsonDocument.Parse(file).RootElement;
                if (json.ValueKind != JsonValueKind.Array)
                {
                    Console.WriteLine("JSON file is not an array");
                    yield break;
                }

                foreach (JsonElement element in json.EnumerateArray()) // Goes through each element in the array and adds it to the list
                {
                    if (element.TryGetProperty("User", out JsonElement name))
                    {
                        MessageData message = new MessageData();
                        message.User = name.GetString() ?? string.Empty;
                        if (element.TryGetProperty("Message", out JsonElement text))
                            message.Message = text.GetString() ?? string.Empty;
                        if (element.TryGetProperty("Timestamp", out JsonElement dateTime))
                            message.Timestamp = dateTime.GetDateTime();
                        if (element.TryGetProperty("Priority", out JsonElement priority))
                            message.Priority = MessagePriority.Normal;
                        Console.WriteLine("user:" + message.User + "\nmessage:" + message.Message + "\npriority" + message.Priority);
                        yield return message;
                    }
                }
            }
        }



        public static void AddJSONMessage(MessageData messageData, string path, List<MessageData> messages) // Adds a message to the JSON file
        {
            var jsonData = System.IO.File.ReadAllText(path);
            messages.Clear();
            messages =
                JsonConvert.DeserializeObject<List<MessageData>>(jsonData)
                ?? new List<MessageData>();

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
