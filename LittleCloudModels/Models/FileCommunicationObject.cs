using Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class FileCommunicationObject : Model
    {

        private byte[] _file;

        public byte[] File
        {
            get { return this._file; }
            set
            {
                if (this._file != value)
                {
                    this._file = value;
                    this.RaisePropertyChanged(() => this.File);
                }
            }
        }

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

        private int _chatNum;

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

        public byte[] FileToByteArray()
        {
            int idx = 0;
            byte signal = 1;
            byte[] chatNum = BitConverter.GetBytes(this.ChatNum);
            byte[] senderName = Encoding.UTF8.GetBytes(this.Sender);
            byte[] fileName = Encoding.UTF8.GetBytes(this.FileName);
            byte[] result = new byte[chatNum.Length + senderName.Length + fileName.Length + this.File.Length + 3];

            result[idx++] = signal;
            for (int i = 0; i < chatNum.Length; i++)
            {
                result[idx++] = chatNum[i];
            }
            for (int i = 0; i < senderName.Length; i++)
            {
                result[idx++] = senderName[i];
            }
            result[idx++] = Encoding.UTF8.GetBytes("|")[0];
            for (int i = 0; i < fileName.Length; i++)
            {
                result[idx++] = fileName[i];
            }
            result[idx++] = Encoding.UTF8.GetBytes("|")[0];
            for (int i = 0; i < this.File.Length; i++)
            {
                result[idx++] = this.File[i];
            }

            return result;
        }

        public static FileCommunicationObject ByteArrayToFile(byte[] data)
        {
            int[] splitIdx =  new int[2];
            int idx = 0;
            
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == Encoding.UTF8.GetBytes("|")[0]) splitIdx[idx++] = i;
                if (idx == 2) break;
            }
            byte[] chatNum = new byte[4];
            byte[] senderName = new byte[splitIdx[0] - 5];
            byte[] fileName = new byte[splitIdx[1] - splitIdx[0] - 1];
            byte[] file = new byte[data.Length - splitIdx[1] - 1];
            idx = 0;
            for(int i = 0; i < 4; i++)
            {
                chatNum[idx++] = data[i + 1];
            }
            idx = 0;
            for (int i = 5; i < splitIdx[0]; i++)
            {
                senderName[idx++] = data[i];
            }
            idx = 0;
            for (int i = splitIdx[0] + 1; i < splitIdx[1]; i++)
            {
                fileName[idx++] = data[i];
            }
            idx = 0;
            for (int i = splitIdx[1] + 1; i < data.Length; i++)
            {
                file[idx++] = data[i];
            }
            FileCommunicationObject result = new FileCommunicationObject()
            {
                ChatNum = BitConverter.ToInt32(chatNum, 0),
                Sender = Encoding.UTF8.GetString(senderName),
                FileName = Encoding.UTF8.GetString(fileName),
                File = file
            };
            return result;
        }

        public bool Equals(FileCommunicationObject other)
        {
            if (other == null) return false;
            return (this.File.Equals(other.File));
        }
    }
}
