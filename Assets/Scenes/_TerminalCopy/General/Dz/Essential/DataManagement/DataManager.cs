using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Dz.DataManagement {
    public class DataManager<T> {
        public T Data { get; private set; }
        static string path => "user://save.txt";

        public DataManager(T data) {
            Data = data;
        }

        //GODOT
        //public void Save() {
        //    using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        //    file.StoreString(JsonConvert.SerializeObject(Data));
        //    // File.WriteAllText(path, JsonConvert.SerializeObject(Data));
        //}

        //public T Load() {
        //    if (FileAccess.FileExists(path)) {
        //        //JObject emptyDataAsJobject = JObject.Parse(JsonConvert.SerializeObject(emptyData));
        //        JObject emptyDataAsJobject = JObject.Parse(JsonConvert.SerializeObject(Data));
        //        emptyDataAsJobject.Merge(JObject.Parse(FileAccess.Open(path, FileAccess.ModeFlags.Read).GetAsText()));
        //        Data = JsonConvert.DeserializeObject<T>(emptyDataAsJobject.ToString(Newtonsoft.Json.Formatting.None));
        //    }

        //    return Data;
        //}

        public void Save() {
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
        }

        public T Load() {
            if (File.Exists(path)) {
                JObject emptyDataAsJobject = JObject.Parse(JsonConvert.SerializeObject(Data));
                emptyDataAsJobject.Merge(JObject.Parse(File.ReadAllText(path)));

                Data = JsonConvert.DeserializeObject<T>(
                    emptyDataAsJobject.ToString(Formatting.None));
            }

            return Data;
        }
    }
}
