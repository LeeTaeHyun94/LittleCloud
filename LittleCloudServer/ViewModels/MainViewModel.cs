using LittleCloudModels.Models;
using LittleCloudServer.Libs;
using LittleCloudServer.Models;
using Mvvm;
using SocketManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LittleCloudServer.ViewModels
{
    public class MainViewModel : ViewModel
    {
        SocketServer<Client> server = new SocketServer<Client>();

        int port = 15937;


        public MainViewModel()
        {
            server.OnConnectedClient += (client) =>
            {
                client.OnReceiveData += (senderClient, data) =>
                {
                    var sender = senderClient as Client;
                    if (data[0] == 1)
                    {
                        FileCommunicationObject receivedMessage = FileCommunicationObject.ByteArrayToFile(data);
                        if (receivedMessage.ChatNum == -1)
                        {
                            string path = @"D:\\LittleCloudDirectory\\" + receivedMessage.Sender;
                            bool existflag = System.IO.Directory.Exists(path);
                            if (!existflag) System.IO.Directory.CreateDirectory(path);
                            System.IO.File.WriteAllBytes(path + "\\" + receivedMessage.FileName, receivedMessage.File);
                            DAO.CloudFileSave(receivedMessage.Sender, path + "\\" + receivedMessage.FileName, receivedMessage.FileName);
                            CloudFilesCollection sendData = new CloudFilesCollection();
                            sendData.DataType = DataType.FilesCollection;
                            sendData.Files = DAO.getCloudFileList(receivedMessage.Sender);
                            sender.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                        }
                        else
                        {
                            var members = DAO.getMembersInChatRoom(receivedMessage.ChatNum);
                            foreach (var item in members)
                            {
                                foreach (var item2 in this.server.m_ConnectedClient)
                                {
                                    if (item2.Member.UserID == item.UserID)
                                    {
                                        var sendData = receivedMessage.FileToByteArray();
                                        item2.SendData(sendData);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        CommunicationObject receivedMessage = CommunicationObject.Parse(Encoding.UTF8.GetString(data));

                        switch (receivedMessage.DataType)
                        {
                            case DataType.CloudSystem:
                                CloudSystemMessage csm = receivedMessage as CloudSystemMessage;
                                if (csm.Message == "CloudDownload")
                                {                                    
                                    int idx = 0;
                                    byte signal = 1;
                                    byte[] fileName = Encoding.UTF8.GetBytes(csm.FileName);
                                    byte[] contents = File.ReadAllBytes(@"D:\\LittleCloudDirectory\\" + csm.Sender.UserID + "\\" + csm.FileName);
                                    byte[] result = new byte[fileName.Length + contents.Length + 2];

                                    result[idx++] = signal;
                                    for (int i = 0; i < fileName.Length; i++)
                                    {
                                        result[idx++] = fileName[i];
                                    }
                                    result[idx++] = Encoding.UTF8.GetBytes("|")[0];
                                    for (int i = 0; i < contents.Length; i++)
                                    {
                                        result[idx++] = contents[i];
                                    }
                                    sender.SendData(result);
                                }
                                else if (csm.Message == "DeleteCloudFile")
                                {
                                    File.Delete(@"D:\\LittleCloudDirectory\\" + csm.Sender.UserID + "\\" + csm.FileName);
                                    DAO.CloudFileDelete(csm.Sender.UserID, csm.FileName);
                                    CloudFilesCollection sendData = new CloudFilesCollection();
                                    sendData.DataType = DataType.FilesCollection;
                                    sendData.Files = DAO.getCloudFileList(csm.Sender.UserID);
                                    sender.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                }
                                break;
                            case DataType.System:
                                SystemMessage systemMessage = receivedMessage as SystemMessage;
                                if (systemMessage.Message == "Login")
                                {
                                    try
                                    {
                                        DAO.Login(systemMessage.Sender.UserID, systemMessage.Sender.Passwd);
                                        var sendData = new SystemMessage();
                                        sendData.DataType = DataType.System;
                                        sendData.Message = "Success";
                                        sender.Member = new Member()
                                        {
                                            UserID = systemMessage.Sender.UserID,
                                            IsLogin = true
                                        };
                                        sender.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                    }
                                    catch (Exception e)
                                    {
                                        var sendData = new SystemMessage();
                                        sendData.DataType = DataType.System;
                                        sendData.Message = e.Message;
                                        sender.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                    }
                                }
                                else if (systemMessage.Message == "getFriends")
                                {
                                    FriendsCollection sendData = new FriendsCollection();
                                    sendData.DataType = DataType.FriendsCollection;
                                    sendData.Friends = DAO.getFriends(sender.Member.UserID);
                                    sender.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                }
                                else if (systemMessage.Message == "Logout")
                                {
                                    var chatRooms = DAO.Logout(systemMessage.Sender.UserID);
                                    sender.StopClient();
                                    server.m_ConnectedClient.Remove(sender);
                                    foreach (var item in chatRooms)
                                    {
                                        var members = DAO.getMembersInChatRoom(item);
                                        foreach (var item1 in members)
                                        {
                                            foreach (var item2 in this.server.m_ConnectedClient)
                                            {
                                                if (item2.Member.UserID == item1.UserID)
                                                {
                                                    var sendData = new ChatMessage();
                                                    sendData.Sender = systemMessage.Sender;
                                                    sendData.Message = "님이 나가셨습니다.";
                                                    sendData.ChatNum = item;
                                                    item2.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if(systemMessage.Message == "getCloudFileList")
                                {
                                    CloudFilesCollection sendData = new CloudFilesCollection();
                                    sendData.DataType = DataType.FilesCollection;
                                    sendData.Files = DAO.getCloudFileList(systemMessage.Sender.UserID);
                                    sender.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                }                                
                                break;
                            case DataType.FriendsCollection:                                
                                FriendsCollection sendFriendsData = receivedMessage as FriendsCollection;
                                if (sendFriendsData.chatNum == -1)
                                {
                                    int chatNum = DAO.StartChat(sendFriendsData.Friends);

                                    foreach (var item in sendFriendsData.Friends)
                                    {
                                        foreach (var item2 in this.server.m_ConnectedClient)
                                        {
                                            if (item2.Member.UserID == item.UserID)
                                            {
                                                ChatInfo chatInfo = new ChatInfo()
                                                {
                                                    Friends = sendFriendsData.Friends,
                                                    ChatNum = chatNum
                                                };
                                                item2.SendData(Encoding.UTF8.GetBytes(chatInfo.ToString()));
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var members = DAO.getMembersNotInChatRoom(sendFriendsData.chatNum);
                                    var temp = DAO.getMembersInChatRoom(sendFriendsData.chatNum);
                                    ObservableCollection<Member> resultMembers = new ObservableCollection<Member>();
                                    ObservableCollection<Member> addMembers = new ObservableCollection<Member>();
                                    foreach (var item in temp)
                                    {
                                        resultMembers.Add(item);
                                    }                                    
                                    foreach (var item1 in members)
                                    {
                                        foreach (var item2 in sendFriendsData.Friends)
                                        {
                                            if (item1.UserID == item2.UserID)
                                            {
                                                resultMembers.Add(item1);
                                                addMembers.Add(item1);
                                                break;
                                            }
                                        }
                                    }

                                    foreach (var item in addMembers)
                                    {
                                        foreach (var item2 in this.server.m_ConnectedClient)
                                        {
                                            if (item2.Member.UserID == item.UserID)
                                            {
                                                DAO.InviteChat(item.UserID, sendFriendsData.chatNum);
                                                ChatInfo chatInfo = new ChatInfo()
                                                {
                                                    Friends = resultMembers,
                                                    ChatNum = sendFriendsData.chatNum
                                                };
                                                item2.SendData(Encoding.UTF8.GetBytes(chatInfo.ToString()));
                                                break;
                                            } 
                                        }
                                    }
                                }
                                break;
                            case DataType.ChatMessage:
                                ChatMessage sendChatData = receivedMessage as ChatMessage;
                                if (sendChatData.Message == "Exitroom")
                                {
                                    DAO.ExitRoom(sendChatData.Sender.UserID, sendChatData.ChatNum);
                                    var members = DAO.getMembersInChatRoom(sendChatData.ChatNum);
                                    foreach (var item in members)
                                    {
                                        foreach (var item2 in this.server.m_ConnectedClient)
                                        {
                                            if (item2.Member.UserID == item.UserID)
                                            {
                                                var sendData = new ChatMessage();
                                                sendData.Sender = sendChatData.Sender;
                                                sendData.Message = "님이 나가셨습니다.";
                                                sendData.ChatNum = sendChatData.ChatNum;
                                                item2.SendData(Encoding.UTF8.GetBytes(sendData.ToString()));
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var members = DAO.getMembersInChatRoom(sendChatData.ChatNum);
                                    foreach (var item in members)
                                    {
                                        foreach (var item2 in this.server.m_ConnectedClient)
                                        {
                                            if (item2.Member.UserID == item.UserID)
                                            {
                                                item2.SendData(data);
                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            default:
                                break; 
                        }
                    }
                };
            };
        }
        
        public ICommand StartCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        server.StartServer(port);
                    },
                    (parameter) =>
                    {
                        return !this.server.isStarted;
                    });
            }
        }

    }
}
