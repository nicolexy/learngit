using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Reflection;
using System.Configuration;
using System.Web.Services.Protocols;
using TENCENT.OSS.CFT.KF.KF_Service;
using System.Collections;

namespace TENCENT.OSS.IMC.KF.KF_Service
{
    /// <summary>
    /// Summary description for IMCInvoke
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class IMCInvoke : System.Web.Services.WebService 
    {

        //用static提高性能，越后期越被大量使用              
        private static Hashtable        m_HashTable;
        public  Finance_Header          m_Header;       

        /*
        private enum eResult
        {
            Error  = 0, //不正常的返回结果
            Normal,     //正常的返回结果值
            ReNull,     //返回值为空，有可能以参数形式传值
            NuKnow,     //暂不知情况返回结果
        }*/

        //基本数据定义
        private enum paramType
        {
            eBool = 0,
            eByte,
            eInt,
            eFloat,
            eDouble,
            eString,
            eDateTime,
            eUnknow,

        };

        //基本数据类型转义
        private object[] GetInputParamInfor(string strInput, out string strMsg)
        {
            try
            {
                strMsg = "参数为空";
                if (string.IsNullOrEmpty(strInput))
                {
                    return null;
                }
                string[] strSplit = strInput.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (strSplit == null)
                {
                    return null;
                }
                ArrayList paramList = new ArrayList();
                for (int i = 0; i < strSplit.Length; ++i)
                {
                    string[] strType = strSplit[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strType.Length != 2)
                    {
                        strMsg = strSplit[i] + "格式不对。";
                        return null;
                    }
                    int type = Convert.ToInt32(strType[0]);
                    string strAim = strType[1].Trim();
                    switch (type)
                    {
                        case (int)paramType.eBool:
                            {
                                paramList.Add(Convert.ToBoolean(strAim));
                            }
                            break;
                        case (int)paramType.eByte:
                            {
                                paramList.Add(Convert.ToByte(strAim));
                            }
                            break;
                        case (int)paramType.eInt:
                            {
                                paramList.Add(Convert.ToInt32(strAim));
                            }
                            break;
                        case (int)paramType.eFloat:
                            {
                                paramList.Add(float.Parse(strAim));
                            }
                            break;
                        case (int)paramType.eDouble:
                            {
                                paramList.Add(Convert.ToDouble(strAim));
                            }
                            break;
                        case (int)paramType.eString:
                            {
                                if (strType[1].Trim() == "null")
                                {
                                    paramList.Add("");
                                }
                                else
                                {
                                    paramList.Add(Convert.ToString(strAim));
                                }
                            }
                            break;

                        case (int)paramType.eDateTime:
                            {
                                paramList.Add(Convert.ToDateTime(strAim));
                            }
                            break;
                        default:
                            {
                                strMsg = strType[0] + "暂时不支持此类型";
                                return null;
                            }
                            break;

                    }
                }
                return paramList.ToArray();
            }
            catch (Exception ex)
            {
                strMsg = ex.ToString();

            }
            return null;

        }
        
        //soapheader赋值
        private void SetSoapHeaderValue(string strSoap)
        {
            if (m_Header != null)
            {
                m_Header.UserName = m_Header.UserPassword = m_Header.UserIP = null;
            }

            if (!string.IsNullOrEmpty(strSoap))
            {
                if (m_Header == null)
                {
                    m_Header = new Finance_Header();
                }
                string[] sHeader = strSoap.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (m_Header != null && sHeader != null && sHeader.Length >= 3)
                {
                    m_Header.UserName = (string)sHeader[0];
                    m_Header.UserPassword = (string)sHeader[1];
                    m_Header.UserIP = (string)sHeader[2];
                }
            }

        }
        
        //通用功能
        protected int CommonInvokeKF(string strID, object[] inParam, out DataSet dsRetData, out string strRetValue,out string strOutParam ,out string strOutMsg)
        {           
            dsRetData           = null;
            object resultobj    = null;
            strRetValue         = "null";
            strOutParam         = "null";    
            strOutMsg           = "KF调用";
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            try
            {
                if (m_HashTable == null)
                {
                    m_HashTable = new Hashtable();

                }
                //完整类名|函数名|[Dll名]
                string conString    = ConfigurationManager.AppSettings[strID];
                if (conString == null)
                {
                    strOutMsg = strID+"找不到与之匹配的类名或DLL";
                    return 0;
                }
                string[] strSplit = conString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                int len = strSplit.Length;
                if (len == 2)
                {
                    //只加载一次
                    if (!m_HashTable.Contains(strSplit[0]))
                    {
                        Type kfServer = Type.GetType(strSplit[0]);
                        if (kfServer == null)
                        {
                            strOutMsg = strSplit[0]+"通过类名，取对像为空。";
                            return 0;
                        }
                       m_HashTable.Add(strSplit[0],System.Activator.CreateInstance(kfServer));
                    }
                    if (m_Header != null && m_Header.UserName != null && m_Header.UserPassword != null && m_Header.UserIP != null)
                    {
                        m_HashTable[strSplit[0]].GetType().InvokeMember("myHeader", bf | BindingFlags.SetField, null, m_HashTable[strSplit[0]], new object[] { m_Header });
                    }
                    else
                    {
                        m_HashTable[strSplit[0]].GetType().InvokeMember("myHeader", bf | BindingFlags.SetField, null, m_HashTable[strSplit[0]], new object[] { null });
                    }
                }
                else if (len >= 3)
                {
                    if (!m_HashTable.Contains(strSplit[0]))
                    {
                        string strBasePaths = System.AppDomain.CurrentDomain.BaseDirectory;
                        string strDllPaths = string.Format("{0}bin\\{1}", strBasePaths, strSplit[2]);
                        Assembly loadDll = Assembly.LoadFrom(strDllPaths);
                        Type dllServer = loadDll.GetType(strSplit[0]);
                        if (dllServer == null)
                        {
                            strOutMsg = strSplit[0]+"通过类名，取对像为空。";
                            return 0;
                        }
                        m_HashTable.Add(strSplit[0], System.Activator.CreateInstance(dllServer));                        
                    } 
                }
                else
                {
                    strOutMsg = conString + "编号对应类名|方法|[DLL名配制格式不对]";
                    return 0;
                }
                //执行指定的函数
                resultobj = m_HashTable[strSplit[0]].GetType().InvokeMember(strSplit[1].Trim(), bf | BindingFlags.InvokeMethod, null, m_HashTable[strSplit[0]], inParam);
                //若执行函数参数有out，ref，则将整个参数输出，外部再简单解析下
                if (inParam != null)
                {
                    strOutParam = null; 
                    foreach (object obj in inParam)
                    {
                        strOutParam += obj.ToString() + "|";
                    }
                }
                if (resultobj == null)
                {
                    strOutMsg = "接口无返回值";
                    return 2;
                }
                //输出数据相关的转化
                if (resultobj is DataTable)
                {
                    dsRetData = new DataSet();
                    dsRetData.Tables.Add(((DataTable)resultobj).Copy());
                }
                else if (resultobj is DataSet)
                {
                    dsRetData = new DataSet();
                    dsRetData = (DataSet)resultobj;
                }
                else
                {
                    strRetValue = resultobj.ToString();
                }
            }
            catch (TargetInvocationException ex)
            {
                strOutMsg = ex.Message;
                return 0;
            }
            return 1;
        }

        [WebMethod]
        /*通用接口参数说明
         @param strID	    函数ID
         @param strParam	输入参数字符 
         @param strHeader   SoapHeader组装数据 
         @param dsRetData   输出接口以 DataSet或DataTable形式 返回数据
         @param strRetValue 输出接口以 基本数据类型形式       返回数据
         @param strOutParam 输出接口以 参数ref或out形式       返回数据
         @param strErrMsg   输出调用过程中出错的信息
         @return   
                0：不正常的返回结果
                1：正常的返回结果值
                2：返回值为空，有可能以参数形式传值
                3：暂不知情况返回结果
         
         note:详情请参考http://up.cf.com/index.php/IMC%E9%80%9A%E7%94%A8%E6%8E%A5%E5%8F%A3%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E
         */
        public int CommonInvoke4IMC(string strID, string strParam, string strHeader, out DataSet dsRetData, out string strRetValue, out string strOutParam, out string strErrMsg)
        {
            strRetValue = "null";
            strOutParam = "null";
            strErrMsg = "KF调用";
            try
            {
                //参数解析
                object[] inParam = GetInputParamInfor(strParam, out strErrMsg);
                //soapheader传值
                SetSoapHeaderValue(strHeader);
                //KF通用接口
                return CommonInvokeKF(strID, inParam, out dsRetData, out strRetValue, out strOutParam, out strErrMsg);
            }
            catch (Exception ex)
            {
                dsRetData = null;
                strErrMsg = ex.ToString();
                return 0;
            }
        }
    }
}
