using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Xml;
using Apollo = CFT.Apollo;//设置别名，不然命名空间与该文件命名空间冲突，会找不到

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    public partial class MermberDiscount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Query_Click(object sender, EventArgs e)
        {
            var qq = txtQQ.Text.Trim();
            if (string.IsNullOrEmpty(qq))
            {
                ShowMsg("请输入支付QQ号码");
                return;
            }
            BindDate(qq);
        }

        void BindDate(string qq)
        {
            var md5 = FormsAuthentication.HashPasswordForStoringInConfigFile(string.Format("QQUin={0}|Key=@kd#%&^d13mury", qq), "md5").ToLower();
            var bufferIn = Encoding.Default.GetBytes(string.Format("CmdCode=CLUBNEW&Data=QQUin={0}&CheckSum={1}\r\n", qq, md5));
            var resultXml = new XmlDocument();
            //应答串示例
            //"<?xml version='1.0' encoding='gb2312'?><client code='CLUB'><retcode>0</retcode><uin>393054141</uin><yearmonth>201304</yearmonth><amt>0</amt><vip>4</vip><selfamt>0</selfamt></client>"
            string ip = Apollo.Common.Configuration.AppSettings.Get<string>("MermDiscountIP", "172.23.60.182");
            int port = Apollo.Common.Configuration.AppSettings.Get<int>("MermDiscountPort", 20001);
            
            var returnResult = RemoveTroublesomeCharacters(GetTCPReply(bufferIn,ip, port));
            if (string.IsNullOrEmpty(returnResult))
            {
                ShowMsg("查询记录为空");
            }
            resultXml.LoadXml(returnResult);

            //返回结果示例：
            //<?xml version="1.0" encoding="utf-8"?>
            //<client code="CLUBNEW">
            //    <retcode>0</retcode>
            //    <uin>654044031</uin>
            //    <yearmonth>201504</yearmonth>
            //    <amt>0</amt>
            //    <selfamt>0</selfamt>
            //    <vip>6</vip>
            //    <cjlevel>6</cjlevel>
            //</client>
            var clientNode = resultXml.SelectSingleNode("client");
            var isSuccess = clientNode.SelectSingleNode("retcode").InnerText == "0";
            if (!isSuccess)
            {
                ShowMsg("查询失败");
            }
            var yearMonth = clientNode.SelectSingleNode("yearmonth").InnerText;
            qqLbl.InnerText = clientNode.SelectSingleNode("uin").InnerText;
            yearMonth = yearMonth.Substring(0, 4) + "-" + yearMonth.Substring(4, 2);
            var date = Convert.ToDateTime(yearMonth + "-01");
            dateLbl.InnerText = date.ToString("yyyy-MM");
            validDateLbl.InnerText = date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            var vip = clientNode.SelectSingleNode("vip").InnerText;
            levelLbl.InnerText = vip;
            //var discount = InternetBankDictionary.GetMemberDiscount(Convert.ToInt16(vip));
            //discountLbl.InnerText = discount.ToString();
            var spent = Convert.ToInt32(clientNode.SelectSingleNode("amt").InnerText); //已经给他人充值了多少Q币
            var self_spent = Convert.ToInt32(clientNode.SelectSingleNode("selfamt").InnerText);//已经给自己充值了多少Q币
            
            var self_discount = 0;
            var friend_discount = 0;
            // 0 表示非超级会员；其它，均是超级会员
            var cjlevel = Convert.ToInt32(clientNode.SelectSingleNode("cjlevel").InnerText);
            if (cjlevel != 0)
            {
                self_discount = 500;
                friend_discount = 500;//超级会员，额度为500
            }
            else 
            {
                self_discount = InternetBankDictionary.GetMemberDiscount(Convert.ToInt16(vip));
                friend_discount = InternetBankDictionary.GetMemberDiscount(Convert.ToInt16(vip));
            }

            discountLbl.InnerText = self_discount.ToString();

            var remain = self_discount - self_spent;
            remainDiscountLbl.InnerText = (remain > 0 ? remain : 0).ToString();

            remain = friend_discount - spent;
            lb_friendQB.InnerText = (remain > 0 ? remain : 0).ToString();
        }

        string GetTCPReply(byte[] bufferIn, string IP, int port)
        {
            using (var tcpClient = new TcpClient())
            {
                var ipAddress = IPAddress.Parse(IP);
                var ipPort = new IPEndPoint(ipAddress, port);
                tcpClient.Connect(ipPort);
                var stream = tcpClient.GetStream();
                stream.Write(bufferIn, 0, bufferIn.Length);
                var bufferOut = new byte[1024];
                stream.Read(bufferOut, 0, 1024);
                return Encoding.Default.GetString(bufferOut);
            }
        }

        private void ShowMsg(string msg)
        {
            Response.Write("<script language=javascript>alert('" + msg + "')</script>");
        }

        /// <summary> 
        /// Removes control characters and other non-UTF-8 characters 
        /// </summary> 
        /// <paramname="inString">The string to process</param> 
        /// <returns>A string with no control characters or entities above 0x00FD</returns> 
        string RemoveTroublesomeCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {

                ch = inString[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters 
                // except tabs and new lines 
                if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();

        } 

    }
}