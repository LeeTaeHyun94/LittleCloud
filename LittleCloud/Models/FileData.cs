namespace LittleCloud.Models
{
    public class FileData : Client
    {
        private string _fileName;
        /// <summary>
        /// 파일 이름을 설정하거나 가져오는 프로퍼티
        /// </summary>
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

        //private string _filePath;
        ///// <summary>
        ///// 파일경로를 설정하거나 가져오는 프로퍼티
        ///// </summary>
        //public string FilePath
        //{
        //    get { return this._filePath; }
        //    set
        //    {
        //        if (this._filePath != value)
        //        {
        //            this._filePath = value;
        //            this.RaisePropertyChanged(() => this.FilePath);
        //        }
        //    }
        //}

        private byte[] _fileBytes;
        /// <summary>
        /// 파일의 바이트 정보를 설정하거나 가져오는 프로퍼티
        /// </summary>
        public byte[] FileBytes
        {
            get { return this._fileBytes; }
            set
            {
                if (this._fileBytes != value)
                {
                    this._fileBytes = value;
                    this.RaisePropertyChanged(() => this.FileBytes);
                }
            }
        }

        ///// <summary>
        ///// 리스트 검색 기준을 위한 Override된 메소드
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public override bool Equals(object obj)
        //{
        //    if (obj == null) return false;
        //    FileData objAsPart = obj as FileData;
        //    if (objAsPart == null) return false;
        //    else return Equals(objAsPart);
        //}

        ////public override int GetHashCode()
        ////{
        ////    return ID;
        ////}

        ///// <summary>
        ///// 리스트 검색 기준을 위한 Override된 메소드
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //public bool Equals(FileData other)
        //{
        //    if (other == null) return false;
        //    return (this.FileBytes.Equals(other.FileBytes));
        //}
    }
}
