using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETHotfix
{
    public class AidData
    {
        public string id { get; set; }
        public string tenantId { get; set; }
        public string virtualAidId { get; set; }
        public string virtualAidName { get; set; }
        public string virtualAidCode { get; set; }
        public string virtualAidImage { get; set; }
        public int isPerpetualLicense { get; set; }
        public string dueTime { get; set; }
    }
}