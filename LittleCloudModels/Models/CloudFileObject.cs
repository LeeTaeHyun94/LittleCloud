using Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class CloudFileObject : Model
    {
        private string _fileName;

        public string FileName
        {
            get { return this._fileName; }
            set
            {
                if (this._fileName != value)
                {
                    this._fileName = value;
                    this.RaisePropertyChanged(() => this.FileName);
                }
            }
        }

        private string _sender = null;
        /// <summary>
        /// 발신자 정보를 가져오거나 설정합니다.
        /// </summary>
        public string Sender
        {
            get { return this._sender; }
            set
            {
                if (this._sender != value)
                {
                    this._sender = value;
                    this.RaisePropertyChanged(() => this.Sender);
                }
            }
        }
    }
}
