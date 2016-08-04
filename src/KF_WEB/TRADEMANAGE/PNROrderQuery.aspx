<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PNROrderQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.PNROrderQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PNROrderQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;PNR订单查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">PNR：</asp:label></TD>
					<TD><asp:textbox id="txtPNR" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">财付通订单号：</asp:label></TD>
					<TD><asp:textbox id="txtPayflowcode" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="center" colspan="4"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div id="divInfo" style="LEFT: 5%; WIDTH: 820px; POSITION: absolute; TOP: 150px; HEIGHT: 600px"
				runat="server">
				<table cellSpacing="1" cellPadding="1" width="820" align="center" border="1">
					<tr> 
                        <TD align="left" width="150"><asp:label id="Label23" runat="server">财付通订单号：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpayflowcode" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label10" runat="server">航司订单号：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairflowcode" runat="server"></asp:label></td>
                   </tr>
                    <tr> 
                        <TD align="left" width="150"><asp:label id="Label16" runat="server">航空公司：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFagent" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label5" runat="server">PNR：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpnr" runat="server"></asp:label></td>
                  </tr>
                  <tr> 
                        <TD align="left" width="150"><asp:label id="Label39" runat="server">票号：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFtickets" runat="server"></asp:label></td>
                       
                        <TD align="left" width="150"><asp:label id="Label8" runat="server">出票状态：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbillStatus" runat="server"></asp:label></td>
                 </tr>
                 <tr> 
                        <TD align="left" width="150"><asp:label id="Label43" runat="server">平台：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFplatformid" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label45" runat="server">支付账号：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbuyid" runat="server"></asp:label></td>
                </tr>
                 <tr>   
                         <TD align="left" width="150"><asp:label id="Label27" runat="server">费率：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFagentRate" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label17" runat="server">票面总价：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbForiFee" runat="server"></asp:label></td>
              </tr>
              <tr> 
                        <TD align="left" width="150"><asp:label id="Label19" runat="server">总金额：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFtotalFee" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label21" runat="server">代理费：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFagentFee" runat="server"></asp:label></td>
              </tr>
              <tr> 
                       <TD align="left" width="150"><asp:label id="Label11" runat="server">燃油费：</asp:label></TD>
                       <td align="left" width="250"><asp:label id="lbFoilFee" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label29" runat="server">机建费：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairportFee" runat="server"></asp:label></td>
			</tr>
            <tr> 	
                        <TD align="left" width="150"><asp:label id="Label25" runat="server">支付时间：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpayTime" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label6" runat="server">订单时间：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbillTime" runat="server"></asp:label></td>
          </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label12" runat="server">起飞时间：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairInTime" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label14" runat="server">降落时间：</asp:label></TD>
                        <td align="left" width="250"><asp:label id="lbFairOutTime" runat="server"></asp:label></td>
          </tr>
          <tr>
                        <TD align="left" width="150"><asp:label id="Label31" runat="server">总票数：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFticketCnt" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label33" runat="server">航段数：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFsegmCnt" runat="server"></asp:label></td>
         </tr>
         <tr>
                        <TD align="left" width="150"><asp:label id="Label35" runat="server">乘客数：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpassCnt" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label41" runat="server">成人价格：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFadultPrice" runat="server"></asp:label></td>
           </tr>
            <tr>            
                     
                         
                         <TD align="left" width="150"><asp:label id="Label15" runat="server">成人数：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFadultNum" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label20" runat="server">儿童数：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFchildNum" runat="server"></asp:label></td>
         </tr>
          <tr>
                         <TD align="left" width="150"><asp:label id="Label24" runat="server">婴儿数：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFinfantNum" runat="server"></asp:label></td>
                          <TD align="left" width="150"><asp:label id="Label4" runat="server">B2B账号：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairUserID" runat="server"></asp:label></td>
        </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label7" runat="server">billmark：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbillmark" runat="server"></asp:label></td>
                           <TD align="left" width="150"><asp:label id="Label47" runat="server">操作员：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFoperator" runat="server"></asp:label></td>
            </tr>
            <tr>            
                          <TD align="left" width="150"><asp:label id="Label28" runat="server">附加信息1：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby1" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label32" runat="server">附加信息2：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby2" runat="server"></asp:label></td>
            </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label36" runat="server">附加信息3：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby3" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label40" runat="server">附加信息4：</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby4" runat="server"></asp:label></td>
            </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label44" runat="server">附加信息5：</asp:label></TD>
						<td align="left" width="250"  colspan="3"><asp:label id="lbFstandby5" runat="server"></asp:label></td>
            </tr>
            <tr>
					   <TD align="left" width="150"><asp:label id="Label37" runat="server">支付URL：</asp:label></TD>
						<td align="left" width="250"  colspan="3"><asp:label id="lbFpayurl" runat="server"></asp:label></td>
           </tr>
					
				</table>
			</div>
		</form>
	</body>
</HTML>
