using System.Text.Json;
using Newtonsoft.Json;
using static Project.Models.InOutModel;

namespace Project.InOut
{
    public class JSONParser
    {
        // private string filePath;
        // private List<MessageData> messages = new List<MessageData>();
        // public JSONParser(string path) // Once a JSON Parser is created, you specifiy the path of the file with which is class will work with
        // {
        //     filePath = path;
        // }

        public static List<MessageData> ReadMessagesFromJSON(string filePath)
        {
            List<MessageData> messages = new List<MessageData>();
            using (FileStream file = File.OpenRead(filePath))
            {
                JsonElement json = JsonDocument.Parse(file).RootElement;
                if (json.ValueKind != JsonValueKind.Array || messages.Count() != 0)
                {
                    Console.WriteLine("JSON file is not an array or messages is not empty");
                    return messages;
                }
                
                foreach (JsonElement element in json.EnumerateArray())
                {
                    if (element.TryGetProperty("User", out JsonElement name))
                    {
                        MessageData message = new MessageData();
                        message.User = name.GetString() ?? string.Empty;
                        if (element.TryGetProperty("Message", out JsonElement text))
                        {
                            message.Message = text.GetString() ?? string.Empty;
                        }
                        if (element.TryGetProperty("Timestamp", out JsonElement dateTime))
                            message.Timestamp = dateTime.GetDateTime();
                        Console.WriteLine("user:" + message.User + "\nmessage:" + message.Message);
                        messages.Add(message);
                    }
                }
            }
            return messages;
        }

        public static void AddJSONMessage(
            string user,
            string message,
            DateTime date,
            string path,
            List<MessageData> messages
        )
        {
            var jsonData = System.IO.File.ReadAllText(path);
            messages.Clear();
            messages =
                JsonConvert.DeserializeObject<List<MessageData>>(jsonData)
                ?? new List<MessageData>();

            messages.Add(
                new MessageData()
                {
                    User = user,
                    Message = message,
                    Timestamp = date
                }
            );

            jsonData = JsonConvert.SerializeObject(messages);
            System.IO.File.WriteAllText(path, jsonData);
        }
    }
}
