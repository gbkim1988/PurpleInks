using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlueInks.Data
{
    [Serializable]
    [XmlRoot(ElementName="SortableLastAction")]
    public class SortableLastAction
    {
        [XmlArrayItem("Test")]
        public List<LastAction> ActionCollection;

        public SortableLastAction()
        {
            this.ActionCollection = new List<LastAction>();
        }
    }
    [Serializable()]
    public class LastAction
    {
        /// XML Serializable/DeSerializable Data Class
        /// lastactivityview.exe 파일에 의해 생성된 데이터를 관리
        [System.Xml.Serialization.XmlElement("ActionTime")]
        public DateTime ActionTime;
        [System.Xml.Serialization.XmlElement("Description")]
        public String Description;
        [System.Xml.Serialization.XmlElement("FileName")]
        public String FileName;
        [System.Xml.Serialization.XmlElement("FullPath")]
        public String FullPath;
        [System.Xml.Serialization.XmlElement("FileExtension")]
        public String FileExtension;
        [System.Xml.Serialization.XmlElement("SuspiciousCount")]
        public int SuspiciousCount;

        public LastAction()
        {

        }

        public LastAction(String action_time, String desc, String filename, String fullpath, String extension)
        {
            /// 데이터 입력 시 null 이 가능 하도록 하는 것이 중요
            /// 
            DateTime tmp_time = new DateTime();
            DateTime.TryParse(action_time, null, System.Globalization.DateTimeStyles.AssumeLocal, out tmp_time);
            this.ActionTime = tmp_time;
            this.Description = desc;
            this.FileName = filename;
            this.FullPath = fullpath;
            this.FileExtension = extension;
            this.SuspiciousCount = 0;
        }
        
    }
}
