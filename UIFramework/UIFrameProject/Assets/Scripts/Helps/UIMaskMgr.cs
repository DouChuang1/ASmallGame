using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SUIFW;

public class UIMaskMgr : MonoBehaviour {

    private static UIMaskMgr _Instance = null;

    //UI根节点
    private GameObject _GoCanvasRoot = null;

    //UI脚本节点对象
    private Transform _TraUIScriptsNode = null;
    //顶层面板
    private GameObject _GoTopPanel = null;
    //遮罩面板
    private GameObject _GoMaskPanel = null;
    //UI摄像机
    private Camera _UICamera = null;
    //ui摄像机层深
    private float _OriginalUICameraDepth;

    public static UIMaskMgr GetInstance()
    {
        if(_Instance==null)
        {
            _Instance = new GameObject("_UIMaskMgr").AddComponent<UIMaskMgr>();
        }

        return _Instance;
    }

    void Awake()
    {
        _GoCanvasRoot = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);
        _TraUIScriptsNode = _GoCanvasRoot.transform.Find("_ScriptMgr");

        UnityHelper.AddChildNodeToParentNode(_TraUIScriptsNode, this.transform);
        _GoTopPanel = _GoCanvasRoot;
        _GoMaskPanel = UnityHelper.FindChildNode(_GoCanvasRoot, "UIMaskPanel").gameObject;
        _UICamera = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CAMERA).GetComponent<Camera>();

        if(_UICamera!=null)
        {
            _OriginalUICameraDepth = _UICamera.depth;
        }

    }

    /// <summary>
    /// 设置遮罩状态
    /// </summary>
    /// <param name="uiForm">显示UI</param>
    /// <param name="lucenyType">透明度属性</param>
    public void SetMaskWindow(GameObject uiForm,UIFormLucenyType lucenyType = UIFormLucenyType.Lucency)
    {
        //顶层窗口下移
        _GoTopPanel.transform.SetAsLastSibling();
        //启用这招窗体以及设置透明度
        _GoMaskPanel.SetActive(true);
        switch (lucenyType)
        {
            //完全透明 不能穿透
            case UIFormLucenyType.Lucency:
                Color newColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0);
                _GoMaskPanel.GetComponent<Image>().color = newColor;
                break;
            //低透明 不能穿透
            case UIFormLucenyType.ImPenetrable:
                Color newColor2 = new Color(50 / 255f, 50 / 255f, 50 / 255f, 200/255f);
                _GoMaskPanel.GetComponent<Image>().color = newColor2;
                break;
            //穿透
            case UIFormLucenyType.Pentrate:
                _GoMaskPanel.SetActive(false);
                break;
            //半透明 不能穿透
            case UIFormLucenyType.Translucence:
                Color newColor3 = new Color(220 / 255f, 220 / 255f, 220 / 255f, 50/255f);
                _GoMaskPanel.GetComponent<Image>().color = newColor3;
                break;
            default:
                break;
        }
        //遮罩窗体下移
        _GoMaskPanel.transform.SetAsLastSibling();
        //显示窗口下移
        uiForm.transform.SetAsLastSibling();
        //增加UI相机层
        _UICamera.depth = _UICamera.depth + 100;
    }

    /// <summary>
    /// 取消遮罩
    /// </summary>
    public void CancelMaskWindow()
    {
        _GoTopPanel.transform.SetAsFirstSibling();

        if(_GoMaskPanel.activeInHierarchy)
        {
            _GoMaskPanel.SetActive(false);
        }
        _UICamera.depth = _OriginalUICameraDepth;
    }
}
