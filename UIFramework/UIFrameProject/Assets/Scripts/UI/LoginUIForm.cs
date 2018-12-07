
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace SUIFW
{
    public class LoginUIForm : BaseUIForm
    {
        public GameObject accountInput;
        public GameObject pwdInput;

        private void Awake()
        {
            Debug.LogError("LoginUIForm Init");
            this.CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;
            this.CurrentUIType.UIForms_Type = UIFormType.Normal;
            this.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.ImPenetrable;

            //EventTriggerListener.Get(accountInput).OnPointerClick()
        }

        public override void Display()
        {
            this.gameObject.SetActive(true);
        }

        public void AccountInputClick()
        {

        }
    }
}