
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class Debugger : MonoBehaviour
    {
        public GUISkin gUI;
        /// <summary>
        /// 是否允许调试
        /// </summary>
        public bool AllowDebugging = true;

        private DebugType _debugType = DebugType.Console;
        private List<LogData> _logInformations = new List<LogData>();
        private int _currentLogIndex = -1;
        private int _infoLogCount = 0;
        private int _warningLogCount = 0;
        private int _errorLogCount = 0;
        private int _fatalLogCount = 0;
        private bool _showInfoLog = true;
        private bool _showWarningLog = true;
        private bool _showErrorLog = true;
        private bool _showFatalLog = true;
        private Vector2 _scrollLogView = Vector2.zero;
        private Vector2 _scrollCurrentLogView = Vector2.zero;
        private Vector2 _scrollSystemView = Vector2.zero;
        private bool _expansion = false;
        private Rect _windowRect = new Rect(0, 0, 100, 60);

        private int _fps = 0;
        private Color _fpsColor = Color.white;
        private int _frameNumber = 0;
        private float _lastShowFPSTime = 0f;
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (AllowDebugging)
            {
                minstructTest = new InstructTest();
                Application.logMessageReceived += LogHandler;
            }

        }
        #region Test
        private int Seleced;                              //功能选项选中的选项
        private int SelecedTime;                          //选中时间指令

        private string Test_TimeStr;                      //时间文本显示器

        private string[] SelectedOption;
        private string[] testOptions = new string[] { "时间", "待开放1", "待开放2", "待开放3", "待开放4" };//选择的按钮
        private string[] testTimeOptions = new string[] { "改变时间", "待开放1", "待开放2" };//时间选择的按钮
        private InstructTest minstructTest;            //测试指令

        private string StringTest = "输入测试编码……";//输入框的输入文字
        #endregion

        private void Update()
        {
            if (AllowDebugging)
            {
                _frameNumber += 1;
                float time = Time.realtimeSinceStartup - _lastShowFPSTime;
                if (time >= 1)
                {
                    _fps = (int)(_frameNumber / time);
                    _frameNumber = 0;
                    _lastShowFPSTime = Time.realtimeSinceStartup;
                }
            }
        }
        private void OnDestory()
        {
            if (AllowDebugging)
            {
                Application.logMessageReceived -= LogHandler;
            }
        }
        private void LogHandler(string condition, string stackTrace,UnityEngine.LogType type)
        {
            LogData log = new LogData();
            log.time = DateTime.Now.ToString("HH:mm:ss");
            log.message = condition;
            log.stackTrace = stackTrace;

            if (type == UnityEngine.LogType.Assert)
            {
                log.type = "Fatal";
                _fatalLogCount += 1;
            }
            else if (type == UnityEngine.LogType.Exception || type == UnityEngine.LogType.Error)
            {
                log.type = "Error";
                _errorLogCount += 1;
            }
            else if (type == UnityEngine.LogType.Warning)
            {
                log.type = "Warning";
                _warningLogCount += 1;
            }
            else if (type == UnityEngine.LogType.Log)
            {
                log.type = "Info";
                _infoLogCount += 1;
            }

            _logInformations.Add(log);

            if (_warningLogCount > 0)
            {
                _fpsColor = Color.yellow;
            }
            if (_errorLogCount > 0)
            {
                _fpsColor = Color.red;
            }
        }

        private void OnGUI()
        {
            if (AllowDebugging)
            {
                if (gUI != null)
                {
                    GUI.skin = gUI;
                }
                if (_expansion)
                {
                    _windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "调试器");
                }
                else
                {
                    _windowRect = GUI.Window(0, _windowRect, ShrinkGUIWindow, "调试器");
                }
            }
        }
        private void ExpansionGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            #region title
            GUILayout.BeginHorizontal();
            GUI.contentColor = _fpsColor;
            if (GUILayout.Button("FPS:" + _fps, GUILayout.Height(30)))
            {
                _expansion = false;
                _windowRect.width = 100;
                _windowRect.height = 60;
            }
            GUI.contentColor = (_debugType == DebugType.Console ? Color.white : Color.gray);
            if (GUILayout.Button("Log输出", GUILayout.Height(30)))
            {
                _debugType = DebugType.Console;
            }
            GUI.contentColor = (_debugType == DebugType.Memory ? Color.white : Color.gray);
            if (GUILayout.Button("更多", GUILayout.Height(30)))
            {
                _debugType = DebugType.Memory;
            }
            GUI.contentColor = (_debugType == DebugType.System ? Color.white : Color.gray);
            if (GUILayout.Button("系统", GUILayout.Height(30)))
            {
                _debugType = DebugType.System;
            }
            GUI.contentColor = (_debugType == DebugType.Screen ? Color.white : Color.gray);
            if (GUILayout.Button("场景", GUILayout.Height(30)))
            {
                _debugType = DebugType.Screen;
            }
            GUI.contentColor = (_debugType == DebugType.Quality ? Color.white : Color.gray);
            if (GUILayout.Button("质量", GUILayout.Height(30)))
            {
                _debugType = DebugType.Quality;
            }
            GUI.contentColor = (_debugType == DebugType.Environment ? Color.white : Color.gray);
            if (GUILayout.Button("版本", GUILayout.Height(30)))
            {
                _debugType = DebugType.Environment;
            }
            if (GUILayout.Button("测试", GUILayout.Height(30)))
            {
                _debugType = DebugType.Test;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            #region console
            if (_debugType == DebugType.Console)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear"))
                {
                    _logInformations.Clear();
                    _fatalLogCount = 0;
                    _warningLogCount = 0;
                    _errorLogCount = 0;
                    _infoLogCount = 0;
                    _currentLogIndex = -1;
                    _fpsColor = Color.white;
                }
                GUI.contentColor = (_showInfoLog ? Color.white : Color.gray);
                _showInfoLog = GUILayout.Toggle(_showInfoLog, "Info [" + _infoLogCount + "]");
                GUI.contentColor = (_showWarningLog ? Color.white : Color.gray);
                _showWarningLog = GUILayout.Toggle(_showWarningLog, "Warning [" + _warningLogCount + "]");
                GUI.contentColor = (_showErrorLog ? Color.white : Color.gray);
                _showErrorLog = GUILayout.Toggle(_showErrorLog, "Error [" + _errorLogCount + "]");
                GUI.contentColor = (_showFatalLog ? Color.white : Color.gray);
                _showFatalLog = GUILayout.Toggle(_showFatalLog, "Fatal [" + _fatalLogCount + "]");
                GUI.contentColor = Color.white;
                GUILayout.EndHorizontal();

                _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(165));
                for (int i = 0; i < _logInformations.Count; i++)
                {
                    bool show = false;
                    Color color = Color.white;
                    switch (_logInformations[i].type)
                    {
                        case "Fatal":
                            show = _showFatalLog;
                            color = Color.red;
                            break;
                        case "Error":
                            show = _showErrorLog;
                            color = Color.red;
                            break;
                        case "Info":
                            show = _showInfoLog;
                            color = Color.white;
                            break;
                        case "Warning":
                            show = _showWarningLog;
                            color = Color.yellow;
                            break;
                        default:
                            break;
                    }

                    if (show)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Toggle(_currentLogIndex == i, ""))
                        {
                            _currentLogIndex = i;
                        }
                        GUI.contentColor = color;
                        GUILayout.Label("[" + _logInformations[i].type + "] ");
                        GUILayout.Label("[" + _logInformations[i].time + "] ");
                        GUILayout.Label(_logInformations[i].message);
                        GUILayout.FlexibleSpace();
                        GUI.contentColor = Color.white;
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();

                _scrollCurrentLogView = GUILayout.BeginScrollView(_scrollCurrentLogView, "Box", GUILayout.Height(100));
                if (_currentLogIndex != -1)
                {
                    GUILayout.Label(_logInformations[_currentLogIndex].message + "\r\n\r\n" + _logInformations[_currentLogIndex].stackTrace);
                }
                GUILayout.EndScrollView();
            }
            #endregion

            #region memory
            else if (_debugType == DebugType.Memory)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Memory Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
#if UNITY_5
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemory() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemory() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSize() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSize() / 1000000 + "MB");
#endif
#if UNITY_7
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSizeLong() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSizeLong() / 1000000 + "MB");
#endif
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("卸载未使用的资源"))
                {
                    Resources.UnloadUnusedAssets();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("使用GC垃圾回收"))
                {
                    GC.Collect();
                }
                GUILayout.EndHorizontal();
            }
            #endregion

            #region system
            else if (_debugType == DebugType.System)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("操作系统信息");
                GUILayout.EndHorizontal();

                _scrollSystemView = GUILayout.BeginScrollView(_scrollSystemView, "Box");
                GUILayout.Label("操作系统：" + SystemInfo.operatingSystem);
                GUILayout.Label("系统内存：" + SystemInfo.systemMemorySize + "MB");
                GUILayout.Label("处理器：" + SystemInfo.processorType);
                GUILayout.Label("处理器数量：" + SystemInfo.processorCount);
                GUILayout.Label("显卡：" + SystemInfo.graphicsDeviceName);
                GUILayout.Label("显卡类型：" + SystemInfo.graphicsDeviceType);
                GUILayout.Label("显存：" + SystemInfo.graphicsMemorySize + "MB");
                GUILayout.Label("显卡标识：" + SystemInfo.graphicsDeviceID);
                GUILayout.Label("显卡供应商：" + SystemInfo.graphicsDeviceVendor);
                GUILayout.Label("显卡供应商标识码：" + SystemInfo.graphicsDeviceVendorID);
                GUILayout.Label("设备模式：" + SystemInfo.deviceModel);
                GUILayout.Label("设备名称：" + SystemInfo.deviceName);
                GUILayout.Label("设备类型：" + SystemInfo.deviceType);
                GUILayout.Label("设备标识：" + SystemInfo.deviceUniqueIdentifier);
                GUILayout.EndScrollView();
            }
            #endregion

            #region screen
            else if (_debugType == DebugType.Screen)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Screen Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                GUILayout.Label("DPI：" + Screen.dpi);
                GUILayout.Label("分辨率：" + Screen.currentResolution.ToString());
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("全屏"))
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
                }
                GUILayout.EndHorizontal();
            }
            #endregion

            #region Quality
            else if (_debugType == DebugType.Quality)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Quality Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                string value = "";
                if (QualitySettings.GetQualityLevel() == 0)
                {
                    value = " [最低]";
                }
                else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1)
                {
                    value = " [最高]";
                }

                GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value);
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("降低一级图形质量"))
                {
                    QualitySettings.DecreaseLevel();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("提升一级图形质量"))
                {
                    QualitySettings.IncreaseLevel();
                }
                GUILayout.EndHorizontal();
            }
            #endregion

            #region Environment
            else if (_debugType == DebugType.Environment)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Environment Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                GUILayout.Label("项目名称：" + Application.productName);
#if UNITY_5
            GUILayout.Label("项目ID：" + Application.bundleIdentifier);
#endif
#if UNITY_7
            GUILayout.Label("项目ID：" + Application.identifier);
#endif
                GUILayout.Label("项目版本：" + Application.version);
                GUILayout.Label("Unity版本：" + Application.unityVersion);
                GUILayout.Label("公司名称：" + Application.companyName);
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("退出程序"))
                {
                    Application.Quit();
                }
                GUILayout.EndHorizontal();
            }
            #endregion

            #region Test 
            else if (_debugType == DebugType.Test)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("测试指令选择：");
                Seleced = GUILayout.Toolbar(Seleced, testOptions, GUILayout.Height(25));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (SelectedOption != null)
                {
                    SelecedTime = GUILayout.Toolbar(SelecedTime, SelectedOption, GUILayout.Height(25));
                }
                else
                {
                    GUILayout.Toolbar(SelecedTime, nullStr, GUILayout.Height(0));
                }
                GUILayout.EndHorizontal();
                _scrollSystemView = GUILayout.BeginScrollView(_scrollSystemView, "Box");
                TestContent();
                GUILayout.EndScrollView();

                GUILayout.BeginHorizontal();
                StringTest = GUILayout.TextField(StringTest, GUILayout.Height(30));
                if (GUILayout.Button("执行", GUILayout.Height(30), GUILayout.Width(60)))
                {
                    switch (Seleced)
                    {
                        case 0:
                            Test_TimeStr += "\r\n" + StringTest;
                            if (SelecedTime == 0)
                            {
                                Test_TimeStr += "\r\n" + minstructTest.ChangeTime(StringTest);
                            }
                            break;
                        default:
                            break;
                    }
                }

                GUILayout.EndHorizontal();
            }
            #endregion
        }
        private string[] nullStr = new string[] { "功能暂未开放！" };
        /// <summary>
        /// 测试内的内容
        /// </summary>
        private void TestContent()
        {
            switch (Seleced)
            {
                case 0:
                    SelectedOption = testTimeOptions;
                    GUILayout.Label(Test_TimeStr);
                    break;
                default:
                    SelectedOption = nullStr;
                    break;
            }
        }
        private void ShrinkGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUI.contentColor = _fpsColor;
            if (GUILayout.Button("FPS:" + _fps, GUILayout.Width(80), GUILayout.Height(30)))
            {
                _expansion = true;
                _windowRect.width = 600;
                _windowRect.height = 360;
            }
            GUI.contentColor = Color.white;
        }
    }
}
public struct LogData
{
    public string time;
    public string type;
    public string message;
    public string stackTrace;
}
public enum DebugType
{
    Console,
    Memory,
    System,
    Screen,
    Quality,
    Environment,
    Test,
}
