using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;
using UnityEngine.UI;

public class MarketUIForm : BaseUIForm {

    private void Awake()
    {
        base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForms_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.ImPenetrable;

        RegisterButtonEvent("backBtn", x => CloseUIForm("MarketUIForm"));
    }
}
