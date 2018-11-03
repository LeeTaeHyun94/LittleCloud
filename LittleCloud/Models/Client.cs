using Mvvm;

namespace LittleCloud.Models
{
    public class Client : Model
    {
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
                if(this._password != value)
                {
                    this._password = value;
                    this.RaisePropertyChanged(() => this.Password);
                }
            }
        }

        private bool _isLogin;
        public bool IsLogin
        {
            get { return this._isLogin; }
            set
            {
                if(this._isLogin != value)
                {
                    this._isLogin = value;
                    this.RaisePropertyChanged(() => this.IsLogin);
                }
            }
        }
    }
}
