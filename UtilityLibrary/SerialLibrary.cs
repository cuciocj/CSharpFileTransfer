using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLibrary {

    [Serializable]
    public class Message {
        public byte[] Data { get; set; }

        public Message() {

        }
    }

    public class SerialLibrary {
        public static Message Serialize(object obj) {
            using (var memoryStream = new MemoryStream()) {
                (new BinaryFormatter()).Serialize(memoryStream, obj);
                return new Message { Data = memoryStream.ToArray() };
            }
        }

        public static object Deserialize(Message message) {
            using (var memoryStream = new MemoryStream(message.Data))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
}
