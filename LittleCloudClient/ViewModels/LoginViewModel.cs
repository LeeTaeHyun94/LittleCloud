using Libraries;
using LittleCloudClient.Libs;
using LittleCloudModels.Models;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LittleCloudClient.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        #region Properties
        private string _loginResult = null;
        /// <summary>
        /// 로그인 성공여부를 나타냅니다.
        /// </summary>
        public string LoginResult
        {
            get { return this._loginResult; }
            set
            {
                this._loginResult = value;
                this.RaisePropertyChanged(() => this.LoginResult);
            }
        }


        private Member _member = new Member();
        /// <summary>
        /// 로그인 정보를 나타냅니다.
        /// </summary>
        public Member Member
        {
            get { return this._member; }
            set
            {
                if (this._member != value)
                {
                    this._member = value;
                    this.RaisePropertyChanged(() => this.Member);
                }
            }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(
                    (parameter) =>
                    {
                        SystemMessage data = new SystemMessage();
                        data.DataType = DataType.System;
                        data.Message = "Login";
                        data.Sender = new Member()
                        {
                            UserID = Member.UserID,
                            Passwd = (parameter as Converters.PasswordBoxConverter.PasswordBoxWrapper).Value
                        };
                        try
                        {
                            Libs.Client.Instance.OnReceiveData += SocketClient_OnReceiveData;
                            Libs.Client.Instance.sendData(data);
                        }
                        catch
                        {
                            this.LoginResult = "no Connect";
                        }
                    },
                    (parameter) =>
                    {
                        return this.Member.UserID != null && this.Member.UserID != "" && this.Member.Passwd != "";
                    });
            }
        }
        #endregion

        #region Helpers
        private void SocketClient_OnReceiveData(SocketManager.SocketClient sender, byte[] data)
        {
            if (data[0] != 1)
            {
                var message = CommunicationObject.Parse(Encoding.UTF8.GetString(data)) as SystemMessage;
                if (message.Message == "Success")
                {
                    Client.Instance.Member = this.Member;
                    this.LoginResult = "OK";
                    Client.Instance.OnReceiveData -= SocketClient_OnReceiveData;
                }
                else if(message.Message == "Already Logined")
                {
                    this.LoginResult = "Already Logined";
                }
                else
                {
                    this.LoginResult = new string("Fail".ToCharArray());
                } 
            }
        } 
        #endregion
    }
}
