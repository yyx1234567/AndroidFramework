using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
namespace ETModel
{
    public class ScreenExtension
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool DestoryWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern bool CloseWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        const uint SWP_SHOWWINDOW = 0x0040;
        const int GWL_STYLE = -16;
        const int WS_BORDER = 1;
        const int WS_POPUP = 0x800000;

        public static void SetScreenMin()
        {
            ShowWindow(GetForegroundWindow(), 2);
        }

        public static void SetScreenResolution(int x, int y, int width, int height)
        {
            SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, WS_POPUP);
            SetWindowPos(FindWindow(null, Application.productName), 0, x, y, width, height, SWP_SHOWWINDOW);
        }

        public static async ETTask SetScreenResolution(Vector2 windowSize, FullScreenMode screenMode = FullScreenMode.Windowed)
        {
            Screen.SetResolution((int)windowSize.x, (int)windowSize.y, screenMode);
            await Task.Delay(30);
            Vector2 screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            int x = (int)(screenSize.x - windowSize.x) / 2;
            int y = (int)(screenSize.y - windowSize.y) / 2;
            SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, WS_POPUP);
            SetWindowPos(FindWindow(null, Application.productName), 0, x, y, (int)windowSize.x, (int)windowSize.y, SWP_SHOWWINDOW);
        }

        public static void SetScreenResolutionWindow(Vector2 windowSize, FullScreenMode screenMode = FullScreenMode.Windowed)
        {
            Vector2 screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            int x = (int)(screenSize.x - windowSize.x) / 2;
            int y = (int)(screenSize.y - windowSize.y) / 2;
            SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, WS_POPUP);
            SetWindowPos(FindWindow(null, Application.productName), 0, x, y, (int)windowSize.x, (int)windowSize.y, SWP_SHOWWINDOW);
        }

        public static async ETTask SetScreenResolutionEnd(Vector2 windowSize, FullScreenMode screenMode = FullScreenMode.Windowed)
        {
            Screen.SetResolution((int)windowSize.x, (int)windowSize.y, screenMode);
            int count = 0;
            while (count < 3)
            {
                ScreenExtension.SetWindowMin();
                await Task.Delay(10);
                count++;
            }
            Vector2 screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            int x = (int)(screenSize.x - windowSize.x) / 2;
            int y = (int)(screenSize.y - windowSize.y) / 2;
            SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, WS_POPUP);
            SetWindowPos(FindWindow(null, Application.productName), 0, x, y, (int)windowSize.x, (int)windowSize.y, SWP_SHOWWINDOW);
        }

        public static void SetWindowStyle()
        {
            SetWindowLong(FindWindow(null, Application.productName), GWL_STYLE, WS_POPUP);
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        public static void SetWindowMin()
        {
            CloseWindow(FindWindow(null, Application.productName));
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static void CloseWindow()
        {
            DestoryWindow(FindWindow(null, Application.productName));
        }

        /// <summary>
        /// 拖拽窗口
        /// </summary>
        public static void DragWindow()
        {
            ReleaseCapture();
            SendMessage(FindWindow(null, Application.productName), 0x0112, 0xF012, 0);
        }

        public static void ResetCapture()
        {
            ReleaseCapture();
        }
    }
}