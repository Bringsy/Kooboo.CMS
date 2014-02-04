#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using System.IO;
using System.Runtime.Serialization;
using Kooboo.Runtime.Serialization;

namespace Kooboo.CMS.Content.Persistence.Elasticsearch
{
    [DataContract]
    public class ElasticsearchSettings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static ElasticsearchSettings instance = null;

        static ElasticsearchSettings()
        {
            var settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<ElasticsearchSettings>(settingFile);
            }
            else
            {
                instance = new ElasticsearchSettings()
                {
                    DEFAULT_CHARSET = "utf8",
                    ConnectionString = "Server=127.0.0.1;Database=Kooboo_CMS;Uid=root;Pwd=;"
                };
                Save(instance);
            }
        }

        public static void Save(ElasticsearchSettings instanceToSave)
        {
            var settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instanceToSave, settingFile);
        }

        private static string GetSettingFile()
        {
            var filePath = Path.Combine(Kooboo.Settings.BaseDirectory, "Elasticsearch.config");

            if (!File.Exists(filePath))
                filePath = Path.Combine(Kooboo.Settings.BinDirectory, "Elasticsearch.config");

            return filePath;
        }

        public static ElasticsearchSettings Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                Save(instance);
            }
        }

        [DataMember]
        public string DEFAULT_CHARSET { get; set; }
        
        [DataMember]
        public string ConnectionString { get; set; }
        
        [DataMember]
        public bool EnableLogging { get; set; }
    }
}
