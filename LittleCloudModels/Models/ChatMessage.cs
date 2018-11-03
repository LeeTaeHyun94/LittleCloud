using Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class ChatMessage : CommunicationObject
    {

        private int _chatNum;
        /// <summary>
        /// 채팅방의 일련번호를 가져오거나 설정합니다.
        /// </summary>
        public int ChatNum
        {
            get { return this._chatNum; }
            set
            {
                if (this._chatNum != value)
                {
                    this._chatNum = value;
                    this.RaisePropertyChanged(() => this.ChatNum);
                }
            }
        }


        private string _message = "";
        /// <summary>
        /// 메시지를 가져오거나 설정합니다.
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

        public ChatMessage()
        {
            this.DataType = DataType.ChatMessage;
        }


        public override string ToString()
        {
            return XmlSerializer.Serialize<ChatMessage>(this);
        }

    }
}
