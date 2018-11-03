using LittleCloud.Models;
using Mvvm;
using SocketManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LittleCloud.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private string[,] accountList = new string[5, 2]
        {
            {"Hyun", "1234"}, {"Hyun1", "1234"}, {"Hyun2", "1234"}, {"Hyun3", "1234"}, {"Hyun4", "1234"}
        };
        private SocketClient _clientSideSocket = null;

        private bool _isLogined;

        private string _ID;
        public string ID
        {
            get { return this._ID; }
            set
            {
                if (this._ID != value)
                {
                    this._ID = value;
                    this.RaisePropertyChanged(() => this.ID);
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return this._password; }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.RaisePropertyChanged(() => this.Password);
                }
            }
        }

        private void CheckInfo()
        {
            if (ID != null && ID != "" && Password != null && Password != "")
            {
                for (int i = 0; i < accountList.GetLength(0); i++)
                {
                    if (accountList[i, 0].Equals(ID) && accountList[i, 1].Equals(Password))
                    {
                        _isLogined = true;
                        break;
                    }
                } 
            }
        }

        private void Login()
        {
            CheckInfo();
            if (_isLogined)
            {
                _clientSideSocket = new SocketClient();
                _clientSideSocket.ConnectToServer("127.0.0.1", ushort.Parse("15937"));
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(
                    () =>
                    {
                        Login();
                    },
                    () =>
                    {
                        if (_isLogined) return false;
                        else return true;
                    }
                    );
            }
        }
    }
}
