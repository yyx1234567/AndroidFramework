using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace ETModel
{
    public class UIRoundedRectangleDymic : BaseMeshEffect
    {
        [HideInInspector]
        public float LTCorner;
        [HideInInspector]
        public float RTCorner;
        [HideInInspector]
        public float RBCorner;
        [HideInInspector]
        public float LBCorner;

        [Tooltip("左上角半径")]
        [Min(0)]
        public float InitLTCorner;
        [Tooltip("右上角半径")]
        [Min(0)]
        public float InitRTCorner;
        [Tooltip("右下角半径")]
        [Min(0)]
        public float InitRBCorner;
        [Tooltip("左下角半径")]
        [Min(0)]
        public float InitLBCorner;
        [Tooltip("圆角分段")]
        [Range(1, 90)]
        public int Segements = 12;

        [Tooltip("模糊距离")]
        [Range(0.1f, 5)]
        public float BlurDistance = 0.1f;

        private float angleDelta => 90f / Segements;

        private List<UIVertex> vertices = new List<UIVertex>();
        private List<int> indices = new List<int>();

        private UnityEngine.UI.Image image;

        protected override void Awake()
        {
            base.Awake();
            this.image = GetComponent<UnityEngine.UI.Image>();
            BlurDistance = 1;
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            vh.Clear();
            vertices.Clear();
            indices.Clear();

            Vector2 center = graphic.rectTransform.rect.center;
            float width = this.graphic.rectTransform.rect.width;
            float height = this.graphic.rectTransform.rect.height;
            float left = center.x - width * 0.5f;
            float right = center.x + width * 0.5f;
            float top = center.y + height * 0.5f;
            float bottom = center.y - height * 0.5f;
            float maxRadius = Mathf.Min((right - left) * 0.5f, (top - bottom) * 0.5f);
             this.LTCorner=InitLTCorner ;
            this.RTCorner=InitRTCorner;
            this.RBCorner=InitRBCorner;
            this.LBCorner = InitLBCorner;

            this.LTCorner = Mathf.Min(this.LTCorner, maxRadius);
            this.RTCorner = Mathf.Min(this.RTCorner, maxRadius);
            this.RBCorner = Mathf.Min(this.RBCorner, maxRadius);
            this.LBCorner = Mathf.Min(this.LBCorner, maxRadius);

 
            if(image==null)
                this.image = GetComponent<UnityEngine.UI.Image>();
            Vector4 uv = this.image.overrideSprite != null ? DataUtility.GetOuterUV(this.image.overrideSprite) : Vector4.zero;

            float uvCenterX = (uv.x + uv.z) * 0.5f + this.graphic.rectTransform.pivot.x - 0.5f;
            float uvCenterY = (uv.y + uv.w) * 0.5f + this.graphic.rectTransform.pivot.y - 0.5f;
            float uvScaleX = (uv.z - uv.x) / width;
            float uvScaleY = (uv.w - uv.y) / height;

            Vector2 currentVertice = center;
            UIVertex uiVertex = new UIVertex();
            uiVertex.color = this.graphic.color;
            uiVertex.position = currentVertice;
            uiVertex.uv0 = new Vector2(currentVertice.x * uvScaleX + uvCenterX, currentVertice.y * uvScaleY + uvCenterY);
            vertices.Add(uiVertex);

            #region 左上角
            if (LTCorner > 0)
            {
                vertices.Add(GenerateUIVertex(left, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(left, top - LTCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 180, 90, new Vector2(left + LTCorner, top - LTCorner), LTCorner, this.graphic.color);

                vertices.Add(GenerateUIVertex(left + LTCorner, top, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(center.x, top, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            else
            {
                vertices.Add(GenerateUIVertex(left, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(left, top, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(center.x, top, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            #endregion
            #region 右上角
            if (RTCorner > 0)
            {
                vertices.Add(GenerateUIVertex(right - RTCorner, top, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 90, 0, new Vector2(right - RTCorner, top - RTCorner), RTCorner, this.graphic.color);

                vertices.Add(GenerateUIVertex(right, top - RTCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(right, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            else
            {
                vertices.Add(GenerateUIVertex(right, top, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(right, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            #endregion
            #region 右下角
            if (RBCorner > 0)
            {
                vertices.Add(GenerateUIVertex(right, bottom + RBCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 360, 270, new Vector2(right - RBCorner, bottom + RBCorner), RBCorner, this.graphic.color);

                vertices.Add(GenerateUIVertex(right - RBCorner, bottom, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(center.x, bottom, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            else
            {
                vertices.Add(GenerateUIVertex(right, bottom, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
                vertices.Add(GenerateUIVertex(center.x, bottom, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            #endregion
            #region 左下角
            if (LBCorner > 0)
            {
                vertices.Add(GenerateUIVertex(left + LBCorner, bottom, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 270, 180, new Vector2(left + LBCorner, bottom + LBCorner), LBCorner, this.graphic.color);

                vertices.Add(GenerateUIVertex(left, bottom + LBCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            else
            {
                vertices.Add(GenerateUIVertex(left, bottom, uvCenterX, uvCenterY, uvScaleX, uvScaleY, this.graphic.color));
            }
            #endregion

            //边缘模糊
            var edgeColor = new Color(this.graphic.color.r, this.graphic.color.g, this.graphic.color.b, 0);

            #region 左上角
            if (LTCorner > 0)
            {
                vertices.Add(GenerateUIVertex(left - this.BlurDistance, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(left - this.BlurDistance, top - LTCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 180, 90, new Vector2(left + LTCorner, top - LTCorner), LTCorner + this.BlurDistance, edgeColor);

                vertices.Add(GenerateUIVertex(left + LTCorner, top + this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(center.x, top + this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            else
            {
                vertices.Add(GenerateUIVertex(left - this.BlurDistance, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(left - this.BlurDistance, top + this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(center.x, top + this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            #endregion
            #region 右上角
            if (RTCorner > 0)
            {
                vertices.Add(GenerateUIVertex(right - RTCorner, top + this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 90, 0, new Vector2(right - RTCorner, top - RTCorner), RTCorner + this.BlurDistance, edgeColor);

                vertices.Add(GenerateUIVertex(right + this.BlurDistance, top - RTCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(right + this.BlurDistance, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            else
            {
                vertices.Add(GenerateUIVertex(right + this.BlurDistance, top + this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(right + this.BlurDistance, center.y, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            #endregion
            #region 右下角
            if (RBCorner > 0)
            {
                vertices.Add(GenerateUIVertex(right + this.BlurDistance, bottom + RBCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 360, 270, new Vector2(right - RBCorner, bottom + RBCorner), RBCorner + this.BlurDistance, edgeColor);

                vertices.Add(GenerateUIVertex(right - RBCorner, bottom - this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(center.x, bottom - this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            else
            {
                vertices.Add(GenerateUIVertex(right + this.BlurDistance, bottom - this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
                vertices.Add(GenerateUIVertex(center.x, bottom - this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            #endregion
            #region 左下角
            if (LBCorner > 0)
            {
                vertices.Add(GenerateUIVertex(left + LBCorner, bottom - this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));

                GenerateQuaterCircle(uvCenterX, uvCenterY, uvScaleX, uvScaleY, 270, 180, new Vector2(left + LBCorner, bottom + LBCorner), LBCorner + this.BlurDistance, edgeColor);

                vertices.Add(GenerateUIVertex(left - this.BlurDistance, bottom + LBCorner, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            else
            {
                vertices.Add(GenerateUIVertex(left - this.BlurDistance, bottom - this.BlurDistance, uvCenterX, uvCenterY, uvScaleX, uvScaleY, edgeColor));
            }
            #endregion

            #region 设置三角形顶点顺序
            var length = (vertices.Count - 1) / 2;
            for (int i = 1; i < length; i++)
            {
                indices.AddRange(new List<int>() { i, i + 1, 0 });
                indices.AddRange(new List<int>() { i, length + i, length + i + 1 });
                indices.AddRange(new List<int>() { i, length + i + 1, i + 1 });
            }
            indices.AddRange(new List<int>() { length, 1, 0 });
            indices.AddRange(new List<int>() { length, length * 2, length + 1 });
            indices.AddRange(new List<int>() { length, length + 1, 1 });
            #endregion
            vh.AddUIVertexStream(vertices, indices);
        }

        private UIVertex GenerateUIVertex(float x, float y, float uvCenterX, float uvCenterY, float uvScaleX, float uvScaleY, Color color)
        {
            UIVertex uiVertex = new UIVertex();
            uiVertex.color = color;
            uiVertex.position = new Vector3(x, y, 0);
            uiVertex.uv0 = new Vector2(x * uvScaleX + uvCenterX, y * uvScaleY + uvCenterY);
            return uiVertex;
        }

        /// <summary>
        /// 生成四分之一圆
        /// </summary>
        /// <param name="startAngle">开始角度</param>
        /// <param name="endAngle">结束角度</param>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        private void GenerateQuaterCircle(float uvCenterX, float uvCenterY, float uvScaleX, float uvScaleY, float startAngle, float endAngle, Vector2 center, float radius, Color color)
        {
            float currentDegree = startAngle - this.angleDelta;
            float cosA;
            float sinA;
            Vector2 currentVertice;
            for (float currentAngle = startAngle - this.angleDelta; currentAngle > endAngle; currentAngle -= this.angleDelta)
            {
                currentDegree = currentAngle * Mathf.Deg2Rad;
                cosA = Mathf.Cos(currentDegree);
                sinA = Mathf.Sin(currentDegree);
                currentVertice = new Vector2(cosA * radius + center.x, sinA * radius + center.y);

                UIVertex uiVertex = new UIVertex();
                uiVertex.color = color;
                uiVertex.position = currentVertice;
                uiVertex.uv0 = new Vector2(currentVertice.x * uvScaleX + uvCenterX, currentVertice.y * uvScaleY + uvCenterY);
                vertices.Add(uiVertex);
            }
        }
    }
}