﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    #region 系统枚举类型

    /// <summary>
    /// UI窗体（位置）类型
    /// </summary>
    public enum UIFormType
    {
        //普通窗体
        Normal,   
        //固定窗体                              
        Fixed,
        //弹出窗体
        PopUp
    }

    /// <summary>
    /// UI窗体的显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        //普通
        Normal,
        //反向切换
        ReverseChange,
        //隐藏其他
        HideOther
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucenyType
    {
        //完全透明，不能穿透
        Lucency,
        //半透明，不能穿透
        Translucence,
        //低透明度，不能穿透
        ImPenetrable,
        //可以穿透
        Pentrate    
    }

    #endregion

    public class SysDefine : MonoBehaviour {
        /* 路径常量 */
        public const string SYS_PATH_CANVAS = "Canvas";
        /* 标签常量 */
        public const string SYS_TAG_CANVAS = "_TagCanvas";

        public const string SYS_TAG_CAMERA = "_TagCamera";

        /* 全局性的方法 */
        //Todo...

        /* 委托的定义 */
        //Todo....

    }
}