using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETModel;

namespace ETModel
{
    public class AudioConfig : ScriptObjectBaseConfig
    {
        [Sirenix.OdinInspector.TableColumnWidth(60)]
        public string Name;
        public AudioClip Sorce;
     }
}