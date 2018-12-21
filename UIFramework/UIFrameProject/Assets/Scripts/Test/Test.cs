/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 测试脚本 
 *    Description: 
 *           功能： yyy
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
	public class Test : MonoBehaviour {
        Dictionary<string,string> _dicTest=new Dictionary<string, string>(); 

		void Start ()
		{
		    string strResult = string.Empty;  //输出内容

            _dicTest.Add("zhangsan","张三");
            _dicTest.Add("lisi", "李四");

            //查询
		    _dicTest.TryGetValue("zhangsan123", out strResult);
            print("查询结果 strResult=" + strResult);

            UIManager.GetInstance().ShowUIForms("LogonUIForm");

            Stack<int> testStack = new Stack<int>();

            testStack.Push(1);
            testStack.Push(2);
            testStack.Push(3);
            testStack.Push(4);

            testStack.Pop();
            testStack.Pop();
            testStack.Pop();
            testStack.Pop();

            testStack.Push(2);
            testStack.Push(3);
            testStack.Push(4);
            testStack.Push(5);

        }
		
	}
}