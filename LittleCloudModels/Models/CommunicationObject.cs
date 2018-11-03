using Libraries;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class CommunicationObject : Model
    {
        private DataType _dataType;
        /// <summary>
        /// 송수신할 데이터의 타입을 가져오거나 설정합니다.
        /// </summary>
        public DataType DataType
        {
            get { return this._dataType; }
            set
            {
                if (this._dataType != value)
                {
                    this._dataType = value;
                    this.RaisePropertyChanged(() => this.DataType);
                }
            }
        }


        private Member _sender = null;
        /// <summary>
        /// 발신자 정보를 가져오거나 설정합니다.
        /// </summary>
        public Member Sender
        {
            get { return this._sender; }
            set
            {
                if (this._sender != value)
                {
                    this._sender = value;
                    this.RaisePropertyChanged(() => this.Sender);
                }
            }
        }

        public override string ToString()
        {
            return XmlSerializer.Serialize<CommunicationObject>(this);
        }

        public static CommunicationObject Parse(string xmlString)
        {
            CommunicationObject result = null;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            var stringReader = new System.IO.StringReader(xmlString);
            doc.Load(stringReader);

            if (doc.ChildNodes[1].SelectSingleNode("DataType").InnerText == "System")
                result = XmlSerializer.Deserial<SystemMessage>(xmlString);
            else if(doc.ChildNodes[1].SelectSingleNode("DataType").InnerText == "FriendsCollection")
                result = XmlSerializer.Deserial<FriendsCollection>(xmlString);
            else if (doc.ChildNodes[1].SelectSingleNode("DataType").InnerText == "ChatInfo")
                result = XmlSerializer.Deserial<ChatInfo>(xmlString);
            else if (doc.ChildNodes[1].SelectSingleNode("DataType").InnerText == "ChatMessage")
                result = XmlSerializer.Deserial<ChatMessage>(xmlString);
            else if (doc.ChildNodes[1].SelectSingleNode("DataType").InnerText == "FilesCollection")
                result = XmlSerializer.Deserial<CloudFilesCollection>(xmlString);
            else if (doc.ChildNodes[1].SelectSingleNode("DataType").InnerText == "CloudSystem")
                result = XmlSerializer.Deserial<CloudSystemMessage>(xmlString);

            return result;
        }
    }
}
