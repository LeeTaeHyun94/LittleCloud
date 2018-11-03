using Libraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class FriendsCollection : CommunicationObject
    {
        public ObservableCollection<Member> Friends { get; set; }
        
        private int _chatNum = -1;

        public int chatNum
        {
            get { return this._chatNum; }
            set
            {
                if (this._chatNum != value)
                {
                    this._chatNum = value;
                    this.RaisePropertyChanged(() => this.chatNum);
                }
            }
        }
        
        public FriendsCollection()
        {
            this.DataType = DataType.FriendsCollection;
        }

        public override string ToString()
        {
            return XmlSerializer.Serialize<FriendsCollection>(this);
        }
    }
}
