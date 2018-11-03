using Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class CloudSystemMessage : CommunicationObject
    {
        private string _message;
        /// <summary>
        /// 메시지
        /// </summary>
        public string Message
        {
            get { return this._message; }
            set
            {
                if (this._message != value)
                {
                    this._message = value;
                    this.RaisePropertyChanged(() => this.Message);
                }
            }
        }

        private string _fileName;

        public string FileName
        {
            get { return this._fileName; }
            set
            {
                if (this._fileName != value)
                {
                    this._fileName = value;
                    this.RaisePropertyChanged(() => this.FileName);
                }
            }
        }

        public CloudSystemMessage()
        {
            this.DataType = DataType.CloudSystem;
        }

        public override string ToString()
        {
            return XmlSerializer.Serialize<CloudSystemMessage>(this);
        }
    }
}
