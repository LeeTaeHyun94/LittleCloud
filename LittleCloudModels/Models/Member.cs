using Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class Member : Model
    {
        private string _userID;
        /// <summary>
        /// 유저의 아이디를 가져오거나 설정합니다.
        /// </summary>
        public string UserID
        {
            get { return this._userID; }
            set
            {
                if (this._userID != value)
                {
                    this._userID = value;
                    this.RaisePropertyChanged(() => this.UserID);
                }
            }
        }


        private string _passwd;
        /// <summary>
        /// 유저의 비밀번호를 가져오거나 설정합니다.
        /// </summary>
        public string Passwd
        {
            get { return this._passwd; }
            set
            {
                if (this._passwd != value)
                {
                    this._passwd = value;
                    this.RaisePropertyChanged(() => this.Passwd);
                }
            }
        }


        private bool _isLogin = false;
        /// <summary>
        /// 현재 로그인되어 있는 상태인지를 나타냅니다.
        /// </summary>
        public bool IsLogin
        {
            get { return this._isLogin; }
            set
            {
                if (this._isLogin != value)
                {
                    this._isLogin = value;
                    this.RaisePropertyChanged(() => this.IsLogin);
                }
            }
        }

    }
}
