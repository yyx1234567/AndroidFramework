using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ETModel
{
     public class ColorConfig : ScriptObjectBaseConfig
    {
        public string TypeName;

        public string RGBA;

        [OnValueChanged("SetRGBAValue")]
        public Color ColorValue;

        private void SetRGBAValue()
        {
            RGBA = $"{ Convert.ToInt32(ColorValue.r*255)},{ Convert.ToInt32(ColorValue.g * 255)},{ Convert.ToInt32(ColorValue.b * 255)},  { Convert.ToInt32(ColorValue.a * 255)}";
        }
      }
}