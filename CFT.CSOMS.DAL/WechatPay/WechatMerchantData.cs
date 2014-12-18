using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using CFT.Apollo.Logging;

namespace CFT.CSOMS.DAL.WechatPay
{
    public class WechatMerchantData
    {
        //mchid to spid
        public string WechatMchIdToSpId(string mchId) 
        {
            try
            {
                if (string.IsNullOrEmpty(mchId))
                {
                    LogHelper.LogError("WechatMchIdToSpId必填参数mchId为空");
                }
                string msg = "";
                string cgi = "http://api.hera.cm.com/index.php/mch/mchidswitch/mchid_to_spid";
                try
                {
                    cgi = System.Configuration.ConfigurationManager.AppSettings["WechatMchIdToSpIdCgi"].ToString();
                }
                catch
                {
                    
                }
                cgi += "?mchId=" + mchId;
                CFT.Apollo.Logging.LogHelper.LogInfo("WechatMchIdToSpId:" + cgi);
                string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "UTF-8", out msg);
                LogHelper.LogInfo("WechatMchIdToSpId return:" + res);

                if (msg != "")
                {
                    return "";
                }
                //解析xml
                var spid = ParseMchIdToSpidXml(res);

                return spid;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("WechatMchIdToSpId:" + ex.Message);
                return "";
            }
        }

        //spid to mchid
        public DataTable WechatSpIdToMchId(string spId)
        {
            try
            {
                if (string.IsNullOrEmpty(spId))
                {
                    LogHelper.LogError("WechatSpIdToMchId必填参数spid为空");
                }

                string msg = "";
                string cgi = "http://api.hera.cm.com/index.php/mch/mchidswitch/spid_to_mchid";
                try
                {
                    cgi = System.Configuration.ConfigurationManager.AppSettings["WechatSpIdToMchIdCgi"].ToString();
                }
                catch
                {

                }
                cgi += "?spId=" + spId;
                LogHelper.LogInfo("WechatSpIdToMchId:" + cgi);
                string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGI(cgi, "UTF-8", out msg);
                LogHelper.LogInfo("WechatSpIdToMchId return:" + res);

                if (msg != "")
                {
                    return null;
                }
                //解析xml
                var dt = ParseSpidToMchIdXml(res);

                return dt;
            }
            catch(Exception ex) 
            {
                LogHelper.LogError("WechatSpIdToMchId:" + ex.Message);
                return null;
            }
        }

        //查询微信支付商户列表
        public DataTable QueryWechatSpList() 
        {
            try
            {
                string msg = "";
                string cgi = "http://mmbiz.oa.com/mmpayop/querybizlist?f=xml";
                LogHelper.LogInfo("QueryWechatSpList:" + cgi);
                string req = "<root><mchid>10010464</mchid></root>";
                string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGIPost(cgi, "UTF-8", req, out msg);
                LogHelper.LogInfo("QueryWechatSpList return:" + res);

                if (msg != "")
                {
                    LogHelper.LogError("QueryWechatSpList:" + msg);
                    return null;
                }
                //解析xml
                var dt = ParseSpList(res);

                return dt;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("QueryWechatSpList:" + ex.Message);
                return null;
            }
        }

        //查询微信支付商户详情
        public DataTable QueryWechatSpDetail() 
        {
            try
            {
                string msg = "";
                string cgi = "http://mmbiz.oa.com/mmpayop/querybiz?f=xml";
                LogHelper.LogInfo("QueryWechatSpDetail:" + cgi);
                string req = "<root><bizindex>10059400</bizindex></root>";
                string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGIPost(cgi, "UTF-8", req, out msg);
                LogHelper.LogInfo("QueryWechatSpDetail return:" + res);

                if (msg != "")
                {
                    LogHelper.LogError("QueryWechatSpDetail:" + msg);
                    return null;
                }
                //解析xml
                //var dt = ParseSpidToMchIdXml(res);

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("QueryWechatSpDetail:" + ex.Message);
                return null;
            }
        }

        //查询微信支付商户审核信息
        public DataTable QueryWechatSpCheck()
        {
            try
            {
                string msg = "";
                string cgi = "http://mmbiz.oa.com/mmpayop/querychecklist?f=xml";
                CFT.Apollo.Logging.LogHelper.LogInfo("QueryWechatSpCheck:" + cgi);
                string req = "<root><bizindex>10059400</bizindex></root>";
                string res = TENCENT.OSS.C2C.Finance.Common.CommLib.commRes.GetFromCGIPost(cgi, "UTF-8", req, out msg);
                CFT.Apollo.Logging.LogHelper.LogInfo("QueryWechatSpCheck return:" + res);

                if (msg != "")
                {
                    LogHelper.LogError("QueryWechatSpCheck:" + msg);
                    return null;
                }
                //解析xml
                //var dt = ParseSpidToMchIdXml(res);

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("QueryWechatSpCheck:" + ex.Message);
                return null;
            }
        }

        private string ParseMchIdToSpidXml(string xml) 
        {
            var resultXml = new XmlDocument();
            resultXml.LoadXml(xml);
            var spid = resultXml.SelectSingleNode("data").SelectSingleNode("spid").InnerText;

            return spid;
        }

        private DataTable ParseSpidToMchIdXml(string xml)
        {
            DataTable dt = new DataTable();
            var resultXml = new XmlDocument();
            resultXml.LoadXml(xml);
            XmlElement root = resultXml.DocumentElement;
            int total = 0;
            total = Convert.ToInt32(root.SelectSingleNode("total").InnerText);
            if (total > 0) 
            {
                XmlNode idt = root.SelectSingleNode("idtable");
                XmlNodeList record_el = idt.SelectNodes("idrow");
                
                for (int i = 0; i < record_el.Count; i++)
                {
                    DataRow drfield = dt.NewRow();
                    drfield.BeginEdit();

                    XmlNodeList ch_list = record_el[i].ChildNodes;
                    foreach (XmlElement ele in ch_list)
                    {
                        string name = ele.Name.Trim();
                        if (i == 0)
                        {
                            dt.Columns.Add(name, typeof(String));
                        }
                        drfield[name] = ele.InnerText;
                    }

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }

            return dt;
        }

        private DataTable ParseSpList(string xml) 
        {
            DataTable dt = new DataTable();
            var resultXml = new XmlDocument();
            resultXml.LoadXml(xml);
            XmlElement root = resultXml.DocumentElement;
            int total = 0;
            total = Convert.ToInt32(root.SelectSingleNode("result").SelectSingleNode("biz_num").InnerText);
            if (total > 0)
            {
                XmlNode idt = root.SelectSingleNode("result").SelectSingleNode("biz_list");
                XmlNodeList record_el = idt.SelectNodes("item");

                for (int i = 0; i < record_el.Count; i++)
                {
                    DataRow drfield = dt.NewRow();
                    drfield.BeginEdit();

                    XmlNodeList ch_list = record_el[i].ChildNodes;
                    foreach (XmlElement ele in ch_list)
                    {
                        string name = ele.Name.Trim();
                        if (i == 0)
                        {
                            dt.Columns.Add(name, typeof(String));
                        }
                        drfield[name] = ele.InnerText;
                    }

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }

            return dt;
        }

        private DataTable ParseSpDetail(string xml) 
        {
            DataTable dt = new DataTable();
            var resultXml = new XmlDocument();
            resultXml.LoadXml(xml);
            XmlElement root = resultXml.DocumentElement;

            string s = root.SelectSingleNode("result").SelectSingleNode("biz_num").InnerText;
            XmlNode res = root.SelectSingleNode("result");  //基本信息

            XmlNode wx_node = res.SelectSingleNode("wx_data"); //微信
            XmlNode fn_node = res.SelectSingleNode("fn_data"); //账务
            XmlNode risk_node = res.SelectSingleNode("risk_data");//风控

            XmlNode node = res.SelectSingleNode("bizindex"); //商户编号

            DataRow dr = dt.NewRow();
            dr[node.Name] = node.InnerText;

            //基本信息
            node = res.SelectSingleNode("aliases"); //商户名称
            dr[node.Name] = node.InnerText;
            node = res.SelectSingleNode("appid"); //appid
            dr[node.Name] = node.InnerText;
            node = res.SelectSingleNode("mchid"); //mchid
            dr[node.Name] = node.InnerText;
            node = res.SelectSingleNode("spid"); //财付通spid
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("company_addr"); //商户地址
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("web_address"); //网站域名
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("postal_code"); //邮编
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("trade_name"); //行业类别 /**推荐人、所属区域、所属BD、QQ、合作形式字段不支持**/
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("contact_user"); //联系人
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("contact_phone"); //联系电话
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("contact_email"); //email
            dr[node.Name] = node.InnerText;
            node = risk_node.SelectSingleNode("contact_phone"); //联系手机
            dr[node.Name] = node.InnerText;
            node = wx_node.SelectSingleNode("earnest_money"); //是否需要缴纳保证金
            dr[node.Name] = node.InnerText;
            //结算信息
            node = fn_node.SelectSingleNode("bank_owner_type"); //开户类型
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("user_cert_id"); //开户人身份证
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("bank_user_name"); //开户名称
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("deposit_bank_province_name"); //开户省份
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("deposit_bank_detail_name"); //开户银行
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("deposit_bank_city_name"); //开户城市
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("deposit_bank_detail_name"); //开户行
            dr[node.Name] = node.InnerText;
            node = fn_node.SelectSingleNode("bank_account"); //银行账号
            dr[node.Name] = node.InnerText;
            //node = fn_node.SelectSingleNode("bank_owner_type"); //结算费率
            //dr[node.Name] = node.InnerText;
            //node = fn_node.SelectSingleNode("bank_owner_type"); //开户类型
            //dr[node.Name] = node.InnerText;

            dt.Rows.Add(dr);

            return dt;
        }

        private DataTable ParseSpCheck(string xml)
        {
            DataTable dt = new DataTable();
            var resultXml = new XmlDocument();
            resultXml.LoadXml(xml);
            XmlElement root = resultXml.DocumentElement;
            int total = 0;
            total = Convert.ToInt32(root.SelectSingleNode("result").SelectSingleNode("biz_num").InnerText);
            if (total > 0)
            {
                XmlNode idt = root.SelectSingleNode("result").SelectSingleNode("biz_list");
                XmlNodeList record_el = idt.SelectNodes("item");

                for (int i = 0; i < record_el.Count; i++)
                {
                    DataRow drfield = dt.NewRow();
                    drfield.BeginEdit();

                    XmlNodeList ch_list = record_el[i].ChildNodes;
                    foreach (XmlElement ele in ch_list)
                    {
                        string name = ele.Name.Trim();
                        if (i == 0)
                        {
                            dt.Columns.Add(name, typeof(String));
                        }
                        drfield[name] = ele.InnerText;
                    }

                    drfield.EndEdit();
                    dt.Rows.Add(drfield);
                }
            }

            return dt;
        }
    }
}
