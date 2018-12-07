/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题: UI窗体的父类
 *    Description: 
 *           功能：定义所有UI窗体的父类。
 *           定义四个生命周期
 *           
 *           1：Display 显示状态。
 *           2：Hiding 隐藏状态
 *           3：ReDisplay 再显示状态。
 *           4：Freeze 冻结状态。
 *           
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace SUIFW
{
	public class BaseUIForm : MonoBehaviour {
        /*字段*/
        private UIType _CurrentUIType=new UIType();

        /* 属性*/
        //当前UI窗体类型
	    public UIType CurrentUIType
	    {
	        get { return _CurrentUIType; }
	        set { _CurrentUIType = value; }
	    }


	    /// <summary>
        /// 显示状态
        /// </summary>
	    public virtual void Display()
	    {
	        this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏状态
        /// </summary>
	    public virtual void Hiding()
	    {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 重新显示状态
        /// </summary>
	    public virtual void Redisplay()
	    {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 冻结状态
        /// </summary>
	    public virtual void Freeze()
	    {
            this.gameObject.SetActive(true);
        }

	}
}