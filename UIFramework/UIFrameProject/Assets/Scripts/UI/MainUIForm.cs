using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;

public class MainUIForm : BaseUIForm {

    private void Awake()
    {
        RegisterButtonEvent("equipmentBtn", x => OpenUIForm("MarketUIForm"));
     
    }

}
