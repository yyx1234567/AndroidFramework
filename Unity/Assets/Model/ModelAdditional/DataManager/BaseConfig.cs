

using Sirenix.OdinInspector;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ETModel
{
    public class ScriptObjectBaseConfig:IConfig,System.ICloneable
    {
         [Sirenix.OdinInspector.TableColumnWidth(40,false)]
         [Sirenix.OdinInspector.ShowInInspector]
         public virtual long Id
        {
            get;
            set;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
