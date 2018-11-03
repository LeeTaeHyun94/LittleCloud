using Libraries;

namespace LittleCloudModels.Models
{
    public class SystemMessage : CommunicationObject
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

        public SystemMessage()
        {
            this.DataType = DataType.System;
        }

        public override string ToString()
        {
            return XmlSerializer.Serialize<SystemMessage>(this);
        }
    }
}
