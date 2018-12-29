using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;
using UnityEngine.UI;

public class ItemsInfoUIForm : BaseUIForm {

    private Text infoText;
    private void Awake()
    {
        base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForms_Type = UIFormType.PopUp;
        base.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.Translucence;

        RegisterButtonEvent("btn_back", p => CloseUIForm("ItemsInfoUIForm"));
        infoText = UnityHelper.FindChildNode(this.gameObject, "InfoText").GetComponent<Text>();

        MessageCenter.Register("ItemsInfo", p =>
         {
             infoText.text = p.Values.ToString();
         });
    }

    private void OnDestroy()
    {
        Debug.LogError("UnRegister=============");
        MessageCenter.UnRegister("ItemsInfo", p =>
        {
            infoText.text = p.Values.ToString();
        });
    }
}
