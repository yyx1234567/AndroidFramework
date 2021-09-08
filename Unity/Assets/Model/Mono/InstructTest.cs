//=====================================================
/* 文 件  名:           InstructTest           */
/* 创 建  者:           岳                       */
/* 创建时间:	        2020/08/29/ 16:02:12         */
/* Email:	            854426372@qq.com             */
/* 描  述: 	            当前脚本的功能               */
/* 修改者列表:	        修改者名字以及修改功能       */
/* (C) 版权 2019 -      危机管理系统                 */
/*  版权所有：          上海哲寻科技                 */
//======================================================


//using BestHTTP.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InstructTest
{
    #region 时间指令相关
    /// <summary>
    /// 判断时间指令
    /// </summary>
    public string ChangeTime(string TimeTestStr)
    {
        //if (GameRoot.Instance.mVisualizationSystem.mGameTime == -1)
        //{
        //    return "请在游戏开始后再使用此指令！";
        //}
        //Regex r = new Regex(@"^\d+$");
        //if (r.Match(TimeTestStr).Success&& TimeTestStr.ToInt32() >= 0 && TimeTestStr.ToInt32() <= 1800)
        //{
        //    GameRoot.Instance.mVisualizationSystem.mGameTime =  int.Parse(TimeTestStr);
        //    return "时间修改成功！";
        //}
        return "请输入1 - 1800 秒内的数字！";
    }
    #endregion
}
