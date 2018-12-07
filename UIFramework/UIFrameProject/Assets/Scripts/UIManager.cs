/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： UI管理器  
 *    Description: 
 *           功能： 是整个UI框架的核心，用户程序通过本脚本，来实现框架绝大多数的功能实现。
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 * 
 *    软件开发原则：
 *    1： “高内聚，低耦合”。
 *    2： 方法的“单一职责”
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
	public class UIManager : MonoBehaviour {
        /* 字段 */
	    private static UIManager _Instance = null;
        //UI窗体预设路径(参数1：窗体预设名称，2：表示窗体预设路径)
	    private Dictionary<string, string> _DicFormsPaths; 
        //缓存所有UI窗体
	    private Dictionary<string, BaseUIForm> _DicALLUIForms;
        //当前显示的UI窗体
	    private Dictionary<string, BaseUIForm> _DicCurrentShowUIForms;
        //UI根节点
	    private Transform _TraCanvasTransfrom = null;
        //全屏幕显示的节点
	    private Transform _TraNormal = null;
        //固定显示的节点
	    private Transform _TraFixed = null;
        //弹出节点
	    private Transform _TraPopUp = null;
        //UI管理脚本的节点
	    private Transform _TraUIScripts = null;


        /// <summary>
        /// 得到实例
        /// </summary>
        /// <returns></returns>
	    public static UIManager GetInstance()
	    {
	        if (_Instance==null)
	        {
	            _Instance = new GameObject("_UIManager").AddComponent<UIManager>();
	        }
	        return _Instance;
	    }

        //初始化核心数据，加载“UI窗体路径”到集合中。
	    public void Awake()
	    {
	        //字段初始化
            _DicALLUIForms=new Dictionary<string, BaseUIForm>();
            _DicCurrentShowUIForms=new Dictionary<string, BaseUIForm>();
            _DicFormsPaths=new Dictionary<string, string>();
            //初始化加载（根UI窗体）Canvas预设
	        InitRootCanvasLoading();
	        //得到UI根节点、全屏节点、固定节点、弹出节点
            _TraCanvasTransfrom = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;
            _TraNormal = _TraCanvasTransfrom.Find("Normal");
            _TraFixed = _TraCanvasTransfrom.Find("Fixed");
            _TraPopUp = _TraCanvasTransfrom.Find("PopUp");
            _TraUIScripts = _TraCanvasTransfrom.Find("_ScriptMgr");
            //把本脚本作为“根UI窗体”的子节点。
            this.gameObject.transform.SetParent(_TraUIScripts, false);
	        //"根UI窗体"在场景转换的时候，不允许销毁
            DontDestroyOnLoad(_TraCanvasTransfrom);
	        //初始化“UI窗体预设”路径数据
            //先写简单的，后面我们使用Json做配置文件，来完善。
	        if (_DicFormsPaths!=null)
	        {
                _DicFormsPaths.Add("LogonUIForm", @"Prefab\LogonUIForm");
            }
	    }

        /// <summary>
        /// 显示（打开）UI窗体
        /// 功能：
        /// 1: 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        /// 2: 根据不同的UI窗体的“显示模式”，分别作不同的加载处理
        /// </summary>
        /// <param name="uiFormName">UI窗体预设的名称</param>
	    public void ShowUIForms(string uiFormName)
        {
            BaseUIForm baseUIForms=null;                    //UI窗体基类

            //参数的检查
            if (string.IsNullOrEmpty(uiFormName)) return;
            //根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
            baseUIForms = LoadFormsToAllUIFormsCatch(uiFormName);
            if (baseUIForms == null) return;
            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (baseUIForms.CurrentUIType.UIForms_ShowMode)
            {                    
                case UIFormShowMode.Normal:                 //“普通显示”窗口模式
                    //Todo.....
                    break;
                case UIFormShowMode.ReverseChange:          //需要“反向切换”窗口模式
                    //更靠后课程进行讲解。
                    break;
                case UIFormShowMode.HideOther:              //“隐藏其他”窗口模式
                    //更靠后课程进行讲解。
                    break;
                default:
                    break;
            }
        }

	    //初始化加载（根UI窗体）Canvas预设
	    private void InitRootCanvasLoading()
	    {
	        ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS, false);
	    }

        /// <summary>
        /// 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        /// 功能： 检查“所有UI窗体”集合中，是否已经加载过，否则才加载。
        /// </summary>
        /// <param name="uiFormsName">UI窗体（预设）的名称</param>
        /// <returns></returns>
	    private BaseUIForm LoadFormsToAllUIFormsCatch(string uiFormsName)
	    {
	        BaseUIForm baseUIResult = null;                 //加载的返回UI窗体基类

	        _DicALLUIForms.TryGetValue(uiFormsName, out baseUIResult);
            if (baseUIResult==null)
	        {
	            //加载指定名称的“UI窗体”
                baseUIResult = LoadUIForm(uiFormsName);
	        }

	        return baseUIResult;
	    }

        /// <summary>
        /// 加载指定名称的“UI窗体”
        /// 功能：
        ///    1：根据“UI窗体名称”，加载预设克隆体。
        ///    2：根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到“根窗体”下不同的节点。
        ///    3：隐藏刚创建的UI克隆体。
        ///    4：把克隆体，加入到“所有UI窗体”（缓存）集合中。
        /// 
        /// </summary>
        /// <param name="uiFormName">UI窗体名称</param>
	    private BaseUIForm LoadUIForm(string uiFormName)
        {
            string strUIFormPaths = null;                   //UI窗体路径
            GameObject goCloneUIPrefabs = null;             //创建的UI克隆体预设
            BaseUIForm baseUiForm=null;                     //窗体基类


            //根据UI窗体名称，得到对应的加载路径
            _DicFormsPaths.TryGetValue(uiFormName, out strUIFormPaths);
            //根据“UI窗体名称”，加载“预设克隆体”
            if (!string.IsNullOrEmpty(strUIFormPaths))
            {
                goCloneUIPrefabs = ResourcesMgr.GetInstance().LoadAsset(strUIFormPaths, false);
            }
            //设置“UI克隆体”的父节点（根据克隆体中带的脚本中不同的“位置信息”）
            if (_TraCanvasTransfrom != null && goCloneUIPrefabs != null)
            {
                baseUiForm = goCloneUIPrefabs.GetComponent<BaseUIForm>();
                if (baseUiForm == null)
                {
                    Debug.Log("baseUiForm==null! ,请先确认窗体预设对象上是否加载了baseUIForm的子类脚本！ 参数 uiFormName=" + uiFormName);
                    return null;
                }
                switch (baseUiForm.CurrentUIType.UIForms_Type)
                {
                    case UIFormType.Normal: //普通窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraNormal, false);
                        break;
                    case UIFormType.Fixed: //固定窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraFixed, false);
                        break;
                    case UIFormType.PopUp: //弹出窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraPopUp, false);
                        break;
                    default:
                        break;
                }

                //设置隐藏
                //goCloneUIPrefabs.SetActive(false);
                //把克隆体，加入到“所有UI窗体”（缓存）集合中。
                _DicALLUIForms.Add(uiFormName, baseUiForm);
                return baseUiForm;
            }
            else
            {
                Debug.Log("_TraCanvasTransfrom==null Or goCloneUIPrefabs==null!! ,Plese Check!, 参数uiFormName="+uiFormName); 
            }

            Debug.Log("出现不可以预估的错误，请检查，参数 uiFormName="+uiFormName);
            return null;
        }//Mehtod_end

	}//class_end
}