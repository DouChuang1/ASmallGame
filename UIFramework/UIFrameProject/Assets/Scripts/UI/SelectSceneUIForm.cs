using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;

public class SelectSceneUIForm : BaseUIForm {

    private GameObject enterGame_btn;
    private GameObject backToLogin_btn;
    private void Awake()
    {
        base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;
        base.CurrentUIType.UIForms_Type = UIFormType.Normal;
        base.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.ImPenetrable;
        RegisterButtonEvent("enterBtn", EnterGame);
        RegisterButtonEvent("backBtn", BackToLogin);
    }

    private void EnterGame(GameObject go)
    {
        UIManager.GetInstance().ShowUIForms("MainUIForm");
        UIManager.GetInstance().ShowUIForms("PlayerInfoUIForm");
    }

    private void BackToLogin(GameObject go)
    {
        Debug.LogError("BackToLogin");
        UIManager.GetInstance().CloseUIForms("SelectSceneUIForm");
    }
}
