using UnityEngine;
 using System.Runtime.InteropServices;

namespace ETHotfix {
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
        public static Vector3 NewPosition { get; set; }
        public static Quaternion NewQuaternion { get; set; }

        public static void SetParameter(ViewConfig config)
        {
            RotateSpeed = 5;
            ZoomSpeed = 5;
            SummationX =float.Parse( config.X);
            SummationY = float.Parse(config.Y);
            SummationZ = float.Parse(config.Distance);
            MaxDistance = 10;
            MinDistance =-10;
            ClampMaxX =360;
            ClampMinX = -360;
            ClampMaxY =90;
            ClampMinY =-90;
         }
    }
}