using Libraries;
using Mvvm;

namespace LittleCloudModels.Models
{
    public class ChatInfo : FriendsCollection
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

        public ChatInfo()
        {
            this.DataType = DataType.ChatInfo;
        }


        public override string ToString()
        {
            return XmlSerializer.Serialize<ChatInfo>(this);
        }
    }
}
