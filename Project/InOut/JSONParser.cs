using System.Text.Json;
using Newtonsoft.Json;
using static Project.Models.InOutModel;

namespace Project.InOut
{
    public class JSONParser
    {
        // public struct MessageData
        // {
        //     public string Name { get; set; }
        //     public string Text { get; set; }
        // }

        // public MessageData Message { get; set; } // Initialize Message with an empty array

        // public static List<MessageData> Messages = new List<MessageData>();

        public JSONParser(string path, List<MessageData> Messages)
        {
            using (FileStream file = File.OpenRead(path))
            {
                JsonElement json = JsonDocument.Parse(file).RootElement;
                if (json.ValueKind != JsonValueKind.Array || Messages.Count() != 0)
                    return;

                foreach (JsonElement element in json.EnumerateArray())
                {
                    if (element.TryGetProperty("User", out JsonElement name))
                    {
                        MessageData message = new MessageData();
                        message.User = name.GetString() ?? string.Empty;
                        if (element.TryGetProperty("Message", out JsonElement text))
                        {
                            message.Message = text.GetString() ?? string.Empty;
                            if (element.TryGetDateTime(out DateTime d))
                                message.Timestamp = d;
                        }
                        Messages.Add(message);
                    }
                }
            }

            // Message = Messages.ToArray();
        }

        public static void AddJSONMessage(
            string user,
            string message,
            DateTime date,
            string path,
            List<MessageData> Messages
        )
        {
            var jsonData = System.IO.File.ReadAllText(path);
            Messages.Clear();
            Messages =
                JsonConvert.DeserializeObject<List<MessageData>>(jsonData)
                ?? new List<MessageData>();

            Messages.Add(
                new MessageData()
                {
                    User = user,
                    Message = message,
                    Timestamp = date
                }
            );

            jsonData = JsonConvert.SerializeObject(Messages);
            System.IO.File.WriteAllText(path, jsonData);
        }
    }
}
