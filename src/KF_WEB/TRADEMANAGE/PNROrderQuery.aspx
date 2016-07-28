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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;PNR������ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">PNR��</asp:label></TD>
					<TD><asp:textbox id="txtPNR" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">�Ƹ�ͨ�����ţ�</asp:label></TD>
					<TD><asp:textbox id="txtPayflowcode" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="center" colspan="4"><FONT face="����"><asp:button id="btnSearch" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div id="divInfo" style="LEFT: 5%; WIDTH: 820px; POSITION: absolute; TOP: 150px; HEIGHT: 600px"
				runat="server">
				<table cellSpacing="1" cellPadding="1" width="820" align="center" border="1">
					<tr> 
                        <TD align="left" width="150"><asp:label id="Label23" runat="server">�Ƹ�ͨ�����ţ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpayflowcode" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label10" runat="server">��˾�����ţ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairflowcode" runat="server"></asp:label></td>
                   </tr>
                    <tr> 
                        <TD align="left" width="150"><asp:label id="Label16" runat="server">���չ�˾��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFagent" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label5" runat="server">PNR��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpnr" runat="server"></asp:label></td>
                  </tr>
                  <tr> 
                        <TD align="left" width="150"><asp:label id="Label39" runat="server">Ʊ�ţ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFtickets" runat="server"></asp:label></td>
                       
                        <TD align="left" width="150"><asp:label id="Label8" runat="server">��Ʊ״̬��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbillStatus" runat="server"></asp:label></td>
                 </tr>
                 <tr> 
                        <TD align="left" width="150"><asp:label id="Label43" runat="server">ƽ̨��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFplatformid" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label45" runat="server">֧���˺ţ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbuyid" runat="server"></asp:label></td>
                </tr>
                 <tr>   
                         <TD align="left" width="150"><asp:label id="Label27" runat="server">���ʣ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFagentRate" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label17" runat="server">Ʊ���ܼۣ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbForiFee" runat="server"></asp:label></td>
              </tr>
              <tr> 
                        <TD align="left" width="150"><asp:label id="Label19" runat="server">�ܽ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFtotalFee" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label21" runat="server">����ѣ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFagentFee" runat="server"></asp:label></td>
              </tr>
              <tr> 
                       <TD align="left" width="150"><asp:label id="Label11" runat="server">ȼ�ͷѣ�</asp:label></TD>
                       <td align="left" width="250"><asp:label id="lbFoilFee" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label29" runat="server">�����ѣ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairportFee" runat="server"></asp:label></td>
			</tr>
            <tr> 	
                        <TD align="left" width="150"><asp:label id="Label25" runat="server">֧��ʱ�䣺</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpayTime" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label6" runat="server">����ʱ�䣺</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbillTime" runat="server"></asp:label></td>
          </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label12" runat="server">���ʱ�䣺</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairInTime" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label14" runat="server">����ʱ�䣺</asp:label></TD>
                        <td align="left" width="250"><asp:label id="lbFairOutTime" runat="server"></asp:label></td>
          </tr>
          <tr>
                        <TD align="left" width="150"><asp:label id="Label31" runat="server">��Ʊ����</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFticketCnt" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label33" runat="server">��������</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFsegmCnt" runat="server"></asp:label></td>
         </tr>
         <tr>
                        <TD align="left" width="150"><asp:label id="Label35" runat="server">�˿�����</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFpassCnt" runat="server"></asp:label></td>
                        <TD align="left" width="150"><asp:label id="Label41" runat="server">���˼۸�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFadultPrice" runat="server"></asp:label></td>
           </tr>
            <tr>            
                     
                         
                         <TD align="left" width="150"><asp:label id="Label15" runat="server">��������</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFadultNum" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label20" runat="server">��ͯ����</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFchildNum" runat="server"></asp:label></td>
         </tr>
          <tr>
                         <TD align="left" width="150"><asp:label id="Label24" runat="server">Ӥ������</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFinfantNum" runat="server"></asp:label></td>
                          <TD align="left" width="150"><asp:label id="Label4" runat="server">B2B�˺ţ�</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFairUserID" runat="server"></asp:label></td>
        </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label7" runat="server">billmark��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFbillmark" runat="server"></asp:label></td>
                           <TD align="left" width="150"><asp:label id="Label47" runat="server">����Ա��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFoperator" runat="server"></asp:label></td>
            </tr>
            <tr>            
                          <TD align="left" width="150"><asp:label id="Label28" runat="server">������Ϣ1��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby1" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label32" runat="server">������Ϣ2��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby2" runat="server"></asp:label></td>
            </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label36" runat="server">������Ϣ3��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby3" runat="server"></asp:label></td>
                         <TD align="left" width="150"><asp:label id="Label40" runat="server">������Ϣ4��</asp:label></TD>
						<td align="left" width="250"><asp:label id="lbFstandby4" runat="server"></asp:label></td>
            </tr>
            <tr>
                         <TD align="left" width="150"><asp:label id="Label44" runat="server">������Ϣ5��</asp:label></TD>
						<td align="left" width="250"  colspan="3"><asp:label id="lbFstandby5" runat="server"></asp:label></td>
            </tr>
            <tr>
					   <TD align="left" width="150"><asp:label id="Label37" runat="server">֧��URL��</asp:label></TD>
						<td align="left" width="250"  colspan="3"><asp:label id="lbFpayurl" runat="server"></asp:label></td>
           </tr>
					
				</table>
			</div>
		</form>
	</body>
</HTML>
