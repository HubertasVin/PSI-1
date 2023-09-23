using System.Text.Json;
using Project.InOut;

namespace Project.Models
{
    public class InOutModel
    {
        public List<InOut.JSONParser.MessageData> Messages { get; set; }
        public string InputMessage { get; set; }

        public InOutModel(FileStream file)
        {
            var chatHistory = new InOut.JSONParser(file);
            this.Messages = JSONParser.Messages;
            this.InputMessage = string.Empty; // Initialize InputMessage in the constructor
            file.Close(); // Won't work on windows without this 
        }

        public void AddMessage(string name, string message, string path)
        {
            Project.InOut.JSONParser.AddJSONMessage(name, message, path);
        }
    }
}
