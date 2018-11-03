using LittleCloudClient.Libs;
using LittleCloudModels.Models;
using Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LittleCloudClient.ViewModels
{
    public class CloudViewModel : ViewModel
    {
        private ObservableCollection<CloudFileObject> _fileList = new ObservableCollection<CloudFileObject>();

        public ObservableCollection<CloudFileObject> FileList
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

        public ICommand DeleteFileCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) => {
                        CloudSystemMessage csm = new CloudSystemMessage()
                        {
                            DataType = DataType.CloudSystem,
                            FileName = parameter as string,
                            Message = "DeleteCloudFile",
                            Sender = Client.Instance.Member
                        };
                        try
                        {
                            Client.Instance.sendData(csm);
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

        public ICommand SaveFileCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        CloudSystemMessage csm = new CloudSystemMessage()
                        {
                            DataType = DataType.CloudSystem,
                            FileName = parameter as string,
                            Message = "CloudDownload",
                            Sender = Client.Instance.Member
                        };
                        try
                        {
                            Client.Instance.sendData(csm);
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

        public CloudViewModel()
        {
            var message = new SystemMessage()
            {
                Message = "getCloudFileList",
                Sender = Client.Instance.Member
            };
            Client.Instance.OnReceiveData += Instance_OnReceiveData;
            Client.Instance.sendData(message);
        }

        private void SendFile(string filePath)
        {
            var data = new FileCommunicationObject()
            {
                File = System.IO.File.ReadAllBytes(filePath),
                FileName = filePath.Split('\\').Last(),
                Sender = Client.Instance.Member.UserID.ToString(),
                ChatNum = -1
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

        private void Instance_OnReceiveData(SocketManager.SocketClient sender, byte[] e)
        {
            if (e[0] != 1)
            {
                var data = CloudFilesCollection.Parse(Encoding.UTF8.GetString(e)) as CloudFilesCollection;
                if (data != null)
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.FileList = data.Files;
                    }));
                } 
            }
            else
            {
                int splitIdx = 0;
                int idx = 0;
                for (int i = 1; i < e.Length; i++)
                {
                    if (e[i] == Encoding.UTF8.GetBytes("|")[0])
                    {
                        splitIdx = i;
                        break;
                    }
                }
                byte[] fileName = new byte[splitIdx];
                byte[] file = new byte[e.Length - splitIdx];
                idx = 0;
                for (int i = 1; i < splitIdx; i++)
                {
                    fileName[idx++] = e[i];
                }
                idx = 0;
                for (int i = splitIdx + 1; i < e.Length; i++)
                {
                    file[idx++] = e[i];
                }
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                + "\\Little Cloud Download";

                bool exists = System.IO.Directory.Exists(path);

                if (!exists) System.IO.Directory.CreateDirectory(path);
                System.IO.File.WriteAllBytes(path + "\\" + Encoding.UTF8.GetString(fileName, 0, fileName.Length - 1), file);
            }
        }
    }
}
