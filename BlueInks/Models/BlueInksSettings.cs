using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlueInks.Models
{
    public class BlueInksSettings
    {
        public enum ProcessMode
        {
            InstantMode=1,
            GuidMode=2
        }
        public enum TransMode
        {
            HTTP=1,
            HTTPS=2,
            SMB=3,
            FTP=4,
            SSH=5
        }
        public ProcessMode ExecutionMode;
        public String ServerIP;
        public TransMode TransferMode;
        public String PortNumber;
        [XmlIgnore]
        public const String FileName = "BlueInks.xml";
        public BlueInksSettings() {
            // Empty
        }
        [XmlIgnore]
        public static BlueInksSettings Instance { get; private set; }
        public static void Default()
        {
            BlueInksSettings.Instance = new BlueInksSettings();
            BlueInksSettings.Instance.SetDefaultValues();
        }
        private void SetDefaultValues()
        {
            this.ExecutionMode = ProcessMode.GuidMode;
            this.ServerIP = "192.168.3.169";
            this.TransferMode = TransMode.HTTP;
            this.PortNumber = "8080";
        }

        public static void Load()
        {
            var Serializer = new XmlSerializer(typeof(BlueInksSettings));
            using (var fStream = new FileStream(BlueInksSettings.FileName, FileMode.Open))
                BlueInksSettings.Instance = ((BlueInksSettings)Serializer.Deserialize(fStream));
        }

        public void Save()
        {
            var Serializer = new XmlSerializer(typeof(BlueInksSettings));
            using (var fStream = new FileStream(BlueInksSettings.FileName, FileMode.Create))
                Serializer.Serialize(fStream, this);
        }
    }
}
