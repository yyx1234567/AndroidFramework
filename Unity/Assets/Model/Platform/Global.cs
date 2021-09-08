using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
namespace ETModel
{
    public class Global
    {
        public static TMPro.TMP_FontAsset TMP_MobileShader;

        /// <summary>
        /// 记录所有加载过的场景 防止重复加载
        /// </summary>
        public static List<string> LoadSceneAssetsbundle = new List<string>();
  
        /// <summary>
        /// 记录打开项目的HotFix中的事件 避免调用多次
        /// </summary>
        public static List<string> HotFixEvent = new List<string>();

        public static UserModel LoginUser { get; set; }

        /// <summary>
        /// 当前打开的项目名称
        /// </summary>
        public static string LoadProjectName { get; set; }
 
        /// <summary>
        /// 第一次打开项目
        /// </summary>
        public static bool FirstOpenProject = true;

        /// <summary>
        /// 已经进入项目
        /// </summary>
        public static bool HasLogined = true;

        /// <summary>
        /// 当前选择的考试内容
        /// </summary>
        public static StudentExamInfo ExamInfo;

        public static Dictionary<string, ProjectBtn> NeedUpdateProjectDic = new Dictionary<string, ProjectBtn>();
        
        public static Dictionary<string, List<ProjectBtn>> SameProjectDic = new Dictionary<string, List<ProjectBtn>>();
        
 
        public static AidLogInfo aidLogInfo;

        public static ServerConnect ServerState;

        public static LoginRole CurrentRole;

        public static DateTime OpenAidTime;

        private static DateTime CurrentTime;
        private static float lastGetTimeTime;
        public static DateTime GetCurrentTime()
        {
            WebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=1");
            DateTime dateTime = default(DateTime);
            try
            {
                WebResponse response = myHttpWebRequest.GetResponse();
                string TimeString = response.Headers["date"];
                return DateTime.Parse(DateTime.Parse(TimeString).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.StackTrace);
                return DateTime.Now;
            }
        }
    }

    public enum ServerConnect
    {
        UnConnect,
        Success,
        Error,
    }

    public enum LoginRole
    {
        student,
        teacher
    }

    public class StudentExamInfo
    {
        //public TaskExamModel taskExamModel;
        //public TaskItemModel taskItemModel;
        public string DbRecordId;
        public string VirExecId;
    }




    public class AidLogInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string tenantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string virtualAidName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string virtualAidCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string virtualAidId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int timeLong { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userTrueName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tenantName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientIpAddress { get; set; }

    }
}