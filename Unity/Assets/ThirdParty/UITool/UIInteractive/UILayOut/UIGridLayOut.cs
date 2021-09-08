using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace ETModel
{
    public class UIGridLayOut : MonoBehaviour
    {
        public int Lenght;
        public int Height;
        public Vector2 CellSize;
        public Vector2 Space;
        public Vector3 OffSet = new Vector3(40, 40);
        public int ChildCount
        {
            get { return transform.childCount; }
        }
        public int Column
        {
            get
            {
                if (ItemList.Count % Lenght == 0)
                {
                    return ItemList.Count / Lenght - 1;
                }
                return ItemList.Count / Lenght;
            }
        }
        public int Row
        {
            get
            {
                if (ItemList.Count % Lenght == 0)
                    return Lenght - 1;
                return ItemList.Count % Lenght - 1;
            }
        }
        public List<Transform> ItemList = new List<Transform>();

        public List<float> HeightList = new List<float>();

        public void Initialize()
        {
            //var canvas = GetComponentInChildren<UnityEngine.UI.Image>(true).canvas.GetComponent<CanvasScaler>();
            //float scale = Screen.width / canvas.referenceResolution.x;
            //Space *= scale;
            //OffSet *= scale;
            Init();
        }

        public void Init()
        {
            HeightList.Clear();
            for (int i = 0; i < Lenght; i++)
            {
                HeightList.Add(0);
            }
            GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, HeightList.Max());
        }

        public void Add(Transform drag)
        {
            ItemList.Add(drag);
            float height = HeightList[Row] + (drag.GetComponent<RectTransform>().sizeDelta.y + Space.y);
            float lenght = Row * (CellSize.x + Space.x);
            Vector3 pos = new Vector3(-lenght, HeightList[Row], 0) + OffSet;
            Tween tween = drag.transform.DOLocalMove(transform.localPosition - pos - Vector3.up * GetComponent<RectTransform>().localPosition.y, 0.3f);

            HeightList[Row] = height;
            float max = 0;
            for (int i = 0; i < HeightList.Count; i++)
            {
                if (HeightList[i] > max)
                    max = HeightList[i];
            }
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, max + OffSet.y);
        }


        public void Remove(Transform drag)
        {
            if (ItemList.Contains(drag))
            {
                HeightList.Clear();
                ItemList.Remove(drag);
                List<Transform> draglist = new List<Transform>(ItemList);
                ItemList.Clear();
                Init();
                for (int i = 0; i < draglist.Count; i++)
                {
                    Add(draglist[i]);
                }
            }
        }
    }
}