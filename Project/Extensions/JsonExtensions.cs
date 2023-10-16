using Newtonsoft.Json;

namespace Extensions
{
    public static class JsonExtensions
    {
        public static List<T> AddJsonToList<T>(this List<T> input, string path)
        {
            List<T> newList = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path)) ?? new List<T>();
            input.AddRange(newList);
            return input;
        }
    }
}