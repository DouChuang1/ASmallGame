
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

namespace SUIFW
{
    public class LoginUIForm : BaseUIForm
    {
        public GameObject accountInput;
        public GameObject pwdInput;
        public GameObject LoginObj;

        private string account = string.Empty;
        private string password = string.Empty;

        private void Awake()
        {
            
            Debug.LogError("LoginUIForm Init");
            base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;
            base.CurrentUIType.UIForms_Type = UIFormType.Normal;
            base.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.ImPenetrable;
           
            EventTriggerListener.Get(LoginObj).onClick = LoginClick;
        }
        
        public override void Display()
        {
            this.gameObject.SetActive(true);
        }

        public void LoginClick(GameObject go)
        {
            account = accountInput.GetComponent<InputField>().text;
            password = pwdInput.GetComponent<InputField>().text;

            //登陆账号密码检测
            if (account == "111111" && password == "111111")
            {
                Debug.LogError("LoginSuccessFully");
                UIManager.GetInstance().ShowUIForms("SelectSceneUIForm");
            }
            else
            {
                Debug.LogError("账号或者密码错误");
            }
            
        }
    }
}