using Libraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LittleCloudModels.Models
{
    public class CloudFilesCollection : CommunicationObject
    {
        public ObservableCollection<CloudFileObject> Files { get; set; }

        public CloudFilesCollection()
        {
            this.DataType = DataType.FilesCollection;
        }

        public override string ToString()
        {
            return XmlSerializer.Serialize<CloudFilesCollection>(this);
        }
    }
}
