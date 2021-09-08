using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel
{
    public enum MaterialResourcesType
    {
        EmergencyFoodSupplies,
        EmergencySafetyMaterials,
        EmergencyHealthSupplies
    }
     public class MaterialResourcesConfig : ScriptObjectBaseConfig
    {

        [LabelText("图片")]
        [HideReferenceObjectPicker]
        [PreviewField(80, ObjectFieldAlignment.Left)]
        public Texture2D Texture;

        [LabelText("模型")]
        [HideReferenceObjectPicker]
        [PreviewField(80,ObjectFieldAlignment.Left)]
        public GameObject Prefab;


        private Sprite _spriteTarget;
        [HideInInspector]
        public Sprite SpriteTarget 
        {
            get {
                return _spriteTarget=_spriteTarget ?? Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5F, 0.5F)); }
        }

        [HideInInspector]
        public MaterialResourcesType MaterialType;

        [LabelText("物资名称")]
        public string Name;
        [LabelText("物资类型")]
        [ValueDropdown("TypeArray")]
        [OnValueChanged("SetResourcesType")]
         public string ResourcesType;

        public string[] TypeArray => new string[] { "应急食品物资", "应急安全物资", "应急卫生物资" };

        public void SetResourcesType()
        {
            switch (ResourcesType)
            {
                case "应急食品物资":
                    MaterialType = MaterialResourcesType.EmergencyFoodSupplies;
                    break;
                case "应急安全物资":
                    MaterialType = MaterialResourcesType.EmergencySafetyMaterials;
                    break;
                case "应急卫生物资":
                    MaterialType = MaterialResourcesType.EmergencyHealthSupplies;
                     break;
            }
        }
        public static string GetResourcesTypeCN(MaterialResourcesType Type)
        {
            switch (Type)
            {
                case MaterialResourcesType.EmergencyFoodSupplies:
                    return "应急食品物资";
                case MaterialResourcesType.EmergencySafetyMaterials:
                    return "应急安全物资";
                case MaterialResourcesType.EmergencyHealthSupplies:
                    return "应急卫生物资";
             }
            return "未分类";
        }


        [LabelText("初始数量")]
         public int Count;

        private void DrawPreview()
        {
            if (this.Texture == null) return;
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(this.Texture);
            GUILayout.EndVertical();
        }

 

    }
}
 
