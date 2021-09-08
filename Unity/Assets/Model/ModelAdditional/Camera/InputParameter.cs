using UnityEngine;
 using System.Runtime.InteropServices;

namespace ETModel {
    public class InputParameter
    {
        public static float RotateSpeed { get; private set; }
        public static float ZoomSpeed { get; private set; }
        public static float SummationX { get; set; }
        public static float SummationY { get; set; }
        public static float SummationZ { get; set; }
        public static float MaxDistance { get; private set; }
        public static float MinDistance { get; private set; }
        public static float ClampMaxX { get; private set; }
        public static float ClampMinX { get; private set; }
        public static float ClampMaxY { get; private set; }
        public static float ClampMinY { get; private set; }

        public static void SetParameter(ViewConfig config)
        {
            RotateSpeed = config.RotateSpeed;
            ZoomSpeed = config.Distance*config.ZoomSpeed;
            SummationX = config.X;
            SummationY = config.Y;
            SummationZ = config.Distance;
            MaxDistance = config.MaxDistance;
            MinDistance = config.MinDistance;
            ClampMaxX = config.MaxX;
            ClampMinX = config.MinX;
            ClampMaxY = config.MaxY;
            ClampMinY = config.MinY;
         }
    }
}