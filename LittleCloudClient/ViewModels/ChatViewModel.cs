using LittleCloudClient.Libs;
using LittleCloudModels.Models;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LittleCloudClient.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        /// <summary>
        /// 메시지 목록을 가져오거나 설정합니다.
        /// </summary>
        public ObservableCollection<string> Messages
        {
            get { return this._messages; }
            set
            {
                if (this._messages != value)
                {
                    this._messages = value;
                    this.RaisePropertyChanged(() => this.Messages);
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


        private ObservableCollection<FileCommunicationObject> _fileList = new ObservableCollection<FileCommunicationObject>();

        public ObservableCollection<FileCommunicationObject> FileList
        {
            get { return this._fileList; }
            set
            {
                if (this._fileList != value)
                {
                    this._fileList = value;
                    this.RaisePropertyChanged(() => this.FileList);
                }
            }
        }

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

        public ICommand InviteFriendsCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        FriendsCollection data = new FriendsCollection()
                        {
                            chatNum = this.ChatNum,
                            DataType = DataType.FriendsCollection,
                            Friends = SelectedFriends,
                            Sender = Client.Instance.Member
                        };
                        try
                        {
                            Client.Instance.sendData(data);
                        }
                        catch
                        {
                        }
                    },
                    (parameter) =>
                    {
                        return true;
                    });
            }
        }

        public ICommand ExitChatRoomCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        var data = new ChatMessage()
                        {
                            Sender = Client.Instance.Member,
                            Message = "Exitroom",
                            ChatNum = this.ChatNum
                        };
                        try
                        {
                            Client.Instance.sendData(data);
                        }
                        catch
                        {
                        }
                    },
                    (parameter) =>
                    {
                        return true;
                    });
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        var data = new ChatMessage()
                        {
                            Sender = Client.Instance.Member,
                            Message = this.Message,
                            ChatNum = this.ChatNum
                        };
                        try
                        {
                            Client.Instance.sendData(data);
                        }
                        catch
                        {

                        }
                        this.Message = "";
                    },
                    (parameter) =>
                    {
                        return this.Message != null && this.Message != "";
                    });
            }
        }

        public ICommand SaveFileCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        byte[] bytes = parameter as byte[];
                        SaveFile(bytes);
                    },
                    (parameter) => 
                    {
                        return true;
                    });
            }
        }

        public ICommand SendFileCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        string fName = parameter as string;
                        SendFile(fName);
                    },
                    (parameter) =>
                    {
                        return true;
                    });
            }
        }

        public ChatViewModel()
        {
            Client.Instance.OnReceiveData += Instance_OnReceiveData;
        }

        private void SendFile(string filePath)
        {
            var data = new FileCommunicationObject()
            {
                ChatNum = this.ChatNum,
                File = System.IO.File.ReadAllBytes(filePath),
                FileName = filePath.Split('\\').Last(),
                Sender = Client.Instance.Member.UserID.ToString()
            };
            var d = data.FileToByteArray();
            try
            {
                Client.Instance.SendData(d);
            }
            catch
            {

            }
        }

        private void SaveFile(byte[] bytes)
        {
            FileCommunicationObject file = new FileCommunicationObject()
            {
                File = bytes
            };
            
            for(int i = 0; i < FileList.Count; i++)
            {
                if (FileList[i].Equals(file))
                {
                    file.FileName = FileList[i].FileName;
                    file.Sender = FileList[i].Sender;
                    file.ChatNum = FileList[i].ChatNum;
                    break;
                }
            }
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\Little Cloud Download";

            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(path);
            System.IO.File.WriteAllBytes(path + "\\" + file.FileName, file.File);
        }

        private void Instance_OnReceiveData(SocketManager.SocketClient sender, byte[] e)
        {
            if(e[0] == 1)
            {
                var data = FileCommunicationObject.ByteArrayToFile(e);
                if(data!=null && data.ChatNum == this.ChatNum)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.FileList.Add(data);
                        this.Messages.Add(data.Sender + " 님이 파일을 전송했습니다.");
                    }));
                }
            }
            else
            {
                var data = CommunicationObject.Parse(Encoding.UTF8.GetString(e)) as ChatMessage;
                if (data != null && data.ChatNum == this.ChatNum)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Messages.Add(data.Sender.UserID + " : " + data.Message);
                    }));
                }
                var receiveData2 = FriendsCollection.Parse(Encoding.UTF8.GetString(e)) as FriendsCollection;
                if (receiveData2 != null)
                    this.Friends = receiveData2.Friends;
            }
        }
    }
}
