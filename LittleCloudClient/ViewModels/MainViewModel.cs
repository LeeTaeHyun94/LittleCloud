using LittleCloudClient.Libs;
using LittleCloudModels.Models;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LittleCloudClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Properties
        
        private ObservableCollection<Member> _chatStartMembers = new ObservableCollection<Member>();
        /// <summary>
        /// 채팅을 시작할 멤버들의 목록을 가져오거나 설정합니다.
        /// </summary>
        public ObservableCollection<Member> ChatStartMembers
        {
            get { return this._chatStartMembers; }
            set
            {
                if (this._chatStartMembers != value)
                {
                    this._chatStartMembers = value;
                    this.RaisePropertyChanged(() => this.ChatStartMembers);
                }
            }
        }


        private ObservableCollection<Member> _friends = new ObservableCollection<Member>();
        /// <summary>
        /// 친구목록을 가져오거나 설정합니다.
        /// </summary>
        public ObservableCollection<Member> Friends
        {
            get { return this._friends; }
            set
            {
                if (this._friends != value)
                {
                    this._friends = value;
                    this.RaisePropertyChanged(() => this.Friends);
                }
            }
        }


        private ObservableCollection<Member> _selectedFriends = new ObservableCollection<Member>();
        /// <summary>
        /// 현재 선택한 친구들의 목록을 가져오거나 설정합니다.
        /// </summary>
        public ObservableCollection<Member> SelectedFriends
        {
            get { return this._selectedFriends; }
            set
            {
                if (this._selectedFriends != value)
                {
                    this._selectedFriends = value;
                    this.RaisePropertyChanged(() => this.SelectedFriends);
                }
            }
        }

        public System.Collections.IList SelectedFriendsItems
        {
            get
            {
                return SelectedFriends;
            }
            set
            {
                SelectedFriends.Clear();
                foreach (Member model in value)
                {
                    SelectedFriends.Add(model);
                }
            }
        }
        #endregion

        #region Commands
        
        public ICommand RefreshFriendsListCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        var message = new SystemMessage()
                        {
                            Message = "getFriends"
                        };
                        Client.Instance.sendData(message);
                    },
                    (parameter) =>
                    {
                        return true;
                    });
            }
        }

        public ICommand LogoutCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        SystemMessage data = new SystemMessage();
                        data.DataType = DataType.System;
                        data.Message = "Logout";
                        data.Sender = new Member()
                        {
                            UserID = Client.Instance.Member.UserID
                        };
                        try
                        {
                            Libs.Client.Instance.sendData(data);
                        }
                        catch (Exception e)
                        {
                        }
                    },
                    (parameter) =>
                    {
                        return true;
                    });
            }
        }


        public ICommand ChatStartCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        FriendsCollection data = new FriendsCollection()
                        {
                            Friends = new ObservableCollection<Member>()
                        };

                        foreach (var item in this.SelectedFriends)
                            data.Friends.Add(item);
                        data.Friends.Add(Client.Instance.Member);

                        Client.Instance.sendData(data);
                    },
                    (parameter) =>
                    {
                        return this.SelectedFriends.Count > 0;
                    });
            }
        }

        #endregion

        #region Contructors
        public MainViewModel()
        {
            var message = new SystemMessage()
            {
                Message = "getFriends"
            };
            Client.Instance.OnReceiveData += Instance_OnReceiveData;
            Client.Instance.sendData(message);
        }
        #endregion

        #region Events
        public event Action<MainViewModel, ChatInfo> OnChatStart = null;
        #endregion

        #region Helpers

        private void Instance_OnReceiveData(SocketManager.SocketClient sender, byte[] data)
        {
            if (data[0] != 1)
            {
                var receiveData = ChatInfo.Parse(Encoding.UTF8.GetString(data)) as ChatInfo;
                if (receiveData != null)
                {
                    if (this.OnChatStart != null)
                        this.OnChatStart(this, receiveData);
                    return;
                }
                var receiveData2 = FriendsCollection.Parse(Encoding.UTF8.GetString(data)) as FriendsCollection;
                if (receiveData2 != null)
                    this.Friends = receiveData2.Friends;
            }
        } 
        #endregion
    }
}
