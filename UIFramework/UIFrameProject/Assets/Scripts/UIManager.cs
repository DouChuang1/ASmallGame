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

        //定义栈集合 处理具有反向切换功能的UIForm
        private Stack<BaseUIForm> _StaCurrentUIForm;

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
            _StaCurrentUIForm = new Stack<BaseUIForm>();
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
                _DicFormsPaths.Add("SelectSceneUIForm", @"Prefab\SelectSceneUIForm");
                _DicFormsPaths.Add("MainUIForm", @"Prefab\MainUIForm");
                _DicFormsPaths.Add("PlayerInfoUIForm", @"Prefab\PlayerInfoUIForm");
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

            //一个栈结构跳转到另外一个栈结构 需要清空之前的栈
            if(baseUIForms.CurrentUIType.isClearStack)
            {
                ClearStack();
            }

            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (baseUIForms.CurrentUIType.UIForms_ShowMode)
            {                    
                case UIFormShowMode.Normal:                 //“普通显示”窗口模式
                    //Todo.....
                    LoadUIToCurrentCache(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:          //需要“反向切换”窗口模式
                    PushUIFormToStack(uiFormName);
                    break;
                case UIFormShowMode.HideOther:              //“隐藏其他”窗口模式
                    EnterUIFormHideOthers(uiFormName);
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
                goCloneUIPrefabs.SetActive(false);
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
        }
        
        private void LoadUIToCurrentCache(string uiFormName)
        {
            BaseUIForm baseUIForm;
            BaseUIForm baseUIFormFromAllCache;

            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm != null) return;
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);

            if(baseUIFormFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormFromAllCache);
                baseUIFormFromAllCache.Display();
            }
        }

        /// <summary>
        /// UI窗体入栈
        /// </summary>
        /// <param name="窗体名字"></param>
        private void PushUIFormToStack(string uiFormName)
        {
            BaseUIForm currentUIForm;
            //判断栈集合中 是否有其他窗体 如果有 冻结处理
            if(_StaCurrentUIForm.Count>0)
            {
                BaseUIForm topUIForm = _StaCurrentUIForm.Peek();
                topUIForm.Freeze();
            }
            //判断UI所有窗体 是否有指定UI 有则处理 没有报错
            _DicALLUIForms.TryGetValue(uiFormName, out currentUIForm);
            if (currentUIForm != null)
            {
                currentUIForm.Display();
                //入栈
                _StaCurrentUIForm.Push(currentUIForm);
            }
            else
            {
                Debug.LogError("UI 加载错误");
            }
        }

        private void EnterUIFormHideOthers(string uiFormName)
        {
            BaseUIForm currentUIForm;
            BaseUIForm currentUIFormFromAll;
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out currentUIForm);
            if (currentUIForm != null) return;

            foreach(var it in _DicCurrentShowUIForms.Values)
            {
                it.Hiding();
            }

            foreach (var it in _StaCurrentUIForm)
            {
                it.Hiding();
            }

            _DicALLUIForms.TryGetValue(uiFormName, out currentUIFormFromAll);
            if(currentUIFormFromAll!=null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, currentUIFormFromAll);
                currentUIFormFromAll.Display();
            }
        }

        /// <summary>
        /// 关闭 返回上一层级界面
        /// </summary>
        /// <param name="uiFormName"></param>
        public void CloseUIForms(string uiFormName)
        {
            BaseUIForm currentUIForm;
            //参数检查
            if (string.IsNullOrEmpty(uiFormName)) return;

            _DicALLUIForms.TryGetValue(uiFormName, out currentUIForm);

            if (currentUIForm == null) return;

            switch(currentUIForm.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    NormalCloseUIForm(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:
                    ReverseCloseUIForm();
                    break;
                case UIFormShowMode.HideOther:
                    HideOtherUIFormClose(uiFormName);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 正常窗体关闭处理
        /// </summary>
        /// <param name="uiFormName"></param>
        private void NormalCloseUIForm(string uiFormName)
        {
            BaseUIForm currentUIForm;
            //参数检测
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out currentUIForm);
            if (currentUIForm == null) return;

            //关闭界面
            currentUIForm.Hiding();
            //移除集合中的窗体
            _DicCurrentShowUIForms.Remove(uiFormName);
        }

        /// <summary>
        /// 反转窗体界面关闭
        /// </summary>
        private void ReverseCloseUIForm()
        {
            if(_StaCurrentUIForm.Count>=2)
            {
                BaseUIForm topUIForm = _StaCurrentUIForm.Pop();
                topUIForm.Hiding();

                BaseUIForm nextUIForm = _StaCurrentUIForm.Peek();
                nextUIForm.Redisplay();
            }
            else if(_StaCurrentUIForm.Count==1)
            {
                BaseUIForm topUIForm = _StaCurrentUIForm.Pop();
                topUIForm.Hiding();
            }
        }

        /// <summary>
        /// 隐藏其他类型的UI窗口关闭处理
        /// </summary>
        /// <param name="uiFormName"></param>
        private void HideOtherUIFormClose(string uiFormName)
        {
            BaseUIForm currentUIForm;
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out currentUIForm);
            if (currentUIForm == null) return;
            currentUIForm.Hiding();
            _DicCurrentShowUIForms.Remove(uiFormName);

            foreach (var it in _DicCurrentShowUIForms.Values)
            {
                it.Redisplay();
            }

            foreach (var it in _StaCurrentUIForm)
            {
                it.Redisplay();
            }

        }

        /// <summary>
        /// 清空栈集合
        /// </summary>
        /// <returns></returns>
        private void ClearStack()
        {
            if(_StaCurrentUIForm!=null && _StaCurrentUIForm.Count>=1)
            {
                _StaCurrentUIForm.Clear();
            }
        }
        //Mehtod_end

    }//class_end
}