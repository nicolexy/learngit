using CFT.CSOMS.DAL.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TENCENT.OSS.C2C.Finance.Common.CommLib;

namespace CFT.CSOMS.BLL.RareName
{
    #region c2c_db_xx.t_tx_card_info_x 表 用于姓名生僻字

    public class T_RareName_INFO : T_CLASS_BASIC
    {
        public string sp_id;            //商务号
        public string op_type;          //操作类型(1,新增，2，删除，3修改)
        public string card_no;          //银行卡号
        public string card_tail;        //银行卡尾标，通常保存最后4位
        public string user_type;        //用户类型，默1  0–公司1–个人
        public string account_name;     //用户姓名(密文)
        public string card_state;       //卡状态1–付款2–成功付款3–无效
        public string modify_username;  //修改人姓名
        public string modify_type;      //修改类型 a:改名b:生僻字c:中小银行信息补填，字段值不够覆盖最前面的修改历史
    }

    #endregion

    /// <summary>
    /// 姓名生僻字
    /// </summary>
    public class RareNameService
    {
       /// <summary>
        /// 姓名生僻字查询
       /// </summary>
       /// <param name="CardNo"></param>
       /// <returns></returns>
        public DataSet RareNameQuery(string CardNo)
        {
            string msg = "";
            try
            {
                string service_name = "common_query_service";//接口名
                string req = "";
                string card_tail = CardNo.Substring(CardNo.Length - 4, 4);//卡尾号
                CardNo = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(CardNo, "md5").ToLower();
                req = "flag=1&reqid=2136&fields=card_id:" + CardNo + "|card_tail:" + card_tail;
                DataSet ds = null;
                ds = CommQuery.GetXmlToDataSetFromICE(req, "", "common_query_service", out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        dt.Columns.Add("Fuser_type_str", typeof(String));//用户类型
                        dt.Columns.Add("record_type_str", typeof(String));//登记类型
                        dt.Columns.Add("updateuser", typeof(String));//修改人
                        dt.Columns.Add("card_state_str", typeof(String));//卡状态
                        dt.Columns.Add("modify_type", typeof(String));//登记类型编号

                        foreach (DataRow dr in dt.Rows)
                        {
                            //对bankid解密 等pauluszhou数据倒完后，切成该解密方式
                            string bankID_Encode = BankIDEncode_ForRareName(dr["Fcard_no"].ToString());

                            //老的解密方式
                            //string bankID_Encode = PublicRes.BankIDEncode_ForBankCardUnbind(dr["Fcard_no"].ToString());

                            bankID_Encode = bankID_Encode.Replace("\0", "");
                            dr["Fcard_no"] = bankID_Encode;
                            string name = NameEncode_ForRareName(dr["Faccount_name"].ToString());
                            dr["Faccount_name"] = name.ToString().Replace("\0", "");

                            switch (dr["Fuser_type"].ToString())
                            {
                                case "0":
                                    dr["Fuser_type_str"] = "公司"; break;
                                case "1":
                                    dr["Fuser_type_str"] = "个人"; break;
                                default:
                                    dr["Fuser_type_str"] = "未知"; break;
                            }
                            switch (dr["Fcard_state"].ToString())
                            {
                                case "1":
                                    dr["card_state_str"] = "付款"; break;
                                case "2":
                                    dr["card_state_str"] = "成功付款"; break;
                                case "3":
                                    dr["card_state_str"] = "无效"; break;
                                default:
                                    dr["card_state_str"] = "未知"; break;
                            }

                            string modify_log = dr["Fmodify_log"].ToString();//1100000000,a,2014-05-04 17:07:39|1100000000,b,2014-05-04 17:08:02
                            string[] log = modify_log.Split('|');

                            List<string[]> m = new List<string[]>();
                            List<string[]> result = new List<string[]>();
                            Hashtable logHash = new Hashtable();
                            for (int i = 0; i < log.Length; i++)
                            {
                                m.Add(log[i].Split(','));
                                if (log[i].Split(',').Length == 3)
                                    logHash.Add(log[i].Split(',')[2], i);//取到时间
                            }
                            //排序找到最新的那条记录
                            ArrayList akeys = new ArrayList(logHash.Keys);
                            akeys.Sort();

                            foreach (string k in akeys)
                            {
                                result.Add(log[int.Parse(logHash[k].ToString())].Split(','));
                            }
                            string[] modify = result[result.Count - 1];//取最新的日志
                            if (modify.Length != 3)
                                throw new Exception("Fmodify_log字段有误：" + dr["Fmodify_log"].ToString());

                            dr["updateuser"] = modify[0];
                            switch (modify[1])
                            {
                                case "a":
                                    dr["record_type_str"] = "改名"; break;
                                case "b":
                                    dr["record_type_str"] = "生僻字"; break;
                                case "c":
                                    dr["record_type_str"] = "中小银行信息补填"; break;
                                default:
                                    dr["record_type_str"] = "未知"; break;
                            }
                            dr["modify_type"] = modify[1];
                        }
                    }
                }
                DataTable dtt = ds.Tables[0];
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Service处理失败！" + msg);
            }
        }

        public void OperateRareName(string op_type, string card_no, string user_type, string account_name, string card_state, string modify_username, string modify_type)
        {
            T_RareName_INFO rare = new T_RareName_INFO();
            rare.op_type = op_type;
            rare.card_no = card_no;
            rare.user_type = user_type;
            rare.account_name = account_name;
            rare.card_state = card_state;
            rare.modify_username = modify_username;
            rare.modify_type = modify_type;
            AddOneRareName(rare);
        }
        /// <summary>
        /// 新增及编辑姓名生僻字
        /// </summary>
        /// <param name="rareName"></param>
        public void AddOneRareName(T_RareName_INFO rareName)
        {
            string msg = "";
            try
            {
                string service_name = "bind_txcard_reg_service";//提现卡信息登记接口
                string req = "&req_src=2&op_type=" + rareName.op_type;
                req += "&card_no=" + rareName.card_no;
                req += "&card_tail=" + rareName.card_no.Substring((rareName.card_no).Length - 4, 4);
                req += "&user_type=" + rareName.user_type;
                req += "&account_name=" + rareName.account_name;
                req += "&card_state=" + rareName.card_state;
                req += "&modify_username=" + rareName.modify_username;
                req += "&modify_type=" + rareName.modify_type;
                CommQuery.GetOneTableFromICE(req, "", service_name, true, out msg);
                if (msg != "")
                    throw new Exception("Service处理失败！" + msg);
            }
            catch (Exception ex)
            {
                throw new Exception("Service处理失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 姓名生僻字  银行卡解密方法
        /// 与一点通解密算法一样，秘钥不一样
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string BankIDEncode_ForRareName(string base64Bankid)
        {
            byte[] iv = { 0x3e, 0x3e, 0x3a, 0x6e, 0x4f, 0x6a, 0x3f, 0x0a };
            byte[] newkey = { 0x1a, 0x2a, (byte)(-0x58 & 0xFF), 0x59, 0x4a, (byte)(-0x63 & 0xFF), 0x5f, (byte)(-0x4d & 0xFF) };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.ASCII.GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

        /// <summary>
        /// 姓名生僻字  姓名解密方法
        /// 与一点通解密算法一样，秘钥不一样
        /// </summary>
        /// <param name="base64Bankid"></param>
        /// <returns></returns>
        public static string NameEncode_ForRareName(string base64Bankid)
        {
            byte[] iv = { 0x2e, 0x3a, 0x35, 0x67, 0x45, 0x6a, 0x7f, 0x0a };
            byte[] newkey = { 0x3a, 0x2a, (byte)(-0x28 & 0xFF), 0x59, 0x43, (byte)(-0x23 & 0xFF), 0x16, (byte)(-0x65 & 0xFF) };

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                //middle的base64转换不标准，这个地方需要替换下 
                byte[] inputByteArray = Convert.FromBase64String(base64Bankid.Replace("-", "+").Replace("_", "/").Replace("%3d", "=").Replace("%3D", "="));

                //建立加密对象的密钥和偏移量，此值重要，不能修改 

                des.Key = newkey;
                des.IV = iv;
                //Padding设置为None，这个很重要，因为middle是这个加密的
                des.Padding = System.Security.Cryptography.PaddingMode.None;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return System.Text.Encoding.GetEncoding("gb2312").GetString(ms.ToArray()).Trim();
            }
            catch (Exception ex)
            {
                return base64Bankid;
            }
        }

    }
}
