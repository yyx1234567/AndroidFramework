using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;

namespace ETModel
{
    public enum GloablConfigType
    {
        DataBaseAddress,
     
    }

    public class GlobalConfig : ScriptObjectBaseConfig
    {
        public GloablConfigType Type;
        public string Value;
    }
 }