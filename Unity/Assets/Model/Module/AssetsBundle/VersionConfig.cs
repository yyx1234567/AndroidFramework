using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    public class FileVersionInfo
    {
        public string File;
        public string MD5;
        public long Size;
    }

    public class UpdateInfo
    {
        public string Description;

        public string Details;
    }
    //public class UpdateDetailsInfo
    //{
    //    public string Details;
    //}
    public class VersionInfo
    {
        public string buildVersionName;
        public int buildVersionIndex;
        public int majorVersion;
        public int minorVersion;
        public int patchVersion;
    }

    public class VersionConfig : Object
    {
        /// <summary>
        /// 修改了属性类型 记录版本号
        /// </summary>
        public string Version;

        public long TotalSize;

        [BsonIgnore]
        public Dictionary<string, FileVersionInfo> FileInfoDict = new Dictionary<string, FileVersionInfo>();
        [BsonIgnore]
        public Dictionary<string, UpdateInfo> UpdateInfoDic = new Dictionary<string, UpdateInfo>();
        public override void EndInit()
        {
            base.EndInit();

            foreach (FileVersionInfo fileVersionInfo in this.FileInfoDict.Values)
            {
                this.TotalSize += fileVersionInfo.Size;
            }
        }
    }
 }