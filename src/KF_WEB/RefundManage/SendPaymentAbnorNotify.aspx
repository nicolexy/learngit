<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="SendPaymentAbnorNotify.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.SendPaymentAbnorNotify" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>�����쳣֪ͨ</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		    .auto-style1 {
                height: 15px;
            }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <TABLE  style="Z-INDEX: 101; LEFT: 5.02%;POSITION: relative; TOP: 20px; " cellSpacing="1" cellPadding="1" width="700px"
				border="0">
              <TR>
                  <td colspan="2" align="left" ><font size="4" style="font-weight: bold">����֪ͨ:</font></td>
              </TR>
              <TR>
					<TD align="right" width="100"><asp:label id="Label2" runat="server">����:</asp:label></TD>
					<TD><asp:label id="tbDate" runat="server"></asp:label></TD>
              </TR>
              <TR>
					<TD align="right" width="100"><asp:label id="Label5" runat="server">����:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbBatchID" runat="server"></asp:label></TD>
             </TR>
              <TR>
                   <TD align="right" width="100"><asp:label id="Label3" runat="server">��ID:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbPackageID" runat="server"></asp:label></TD>
              </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label1" runat="server">ҵ�񵥺�:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tblistid" runat="server"></asp:label></TD>
              </TR>
              <TR>
                   <TD align="right" width="100"><asp:label id="Label6" runat="server">ҵ������:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbtype" runat="server"></asp:label></TD>
              </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label8" runat="server">��ҵ������:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbSubTypePay" runat="server"></asp:label></TD>
              </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label10" runat="server">֪ͨ״̬:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbNotityStatus" runat="server"></asp:label></TD>
              </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label12" runat="server">֪ͨ���:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbNotityResult" runat="server"></asp:label></TD>
              </TR>
             <TR>
                   <TD align="right" width="100"><asp:label id="Label14" runat="server">��������:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbBankType" runat="server"></asp:label></TD>
              </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label16" runat="server">��������:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbErrorType" runat="server"></asp:label></TD>
              </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label18" runat="server">�˻�����:</asp:label></TD>
					<TD class="auto-style2"><asp:label id="tbAccType" runat="server"></asp:label></TD>
              </TR>
            </TABLE>
            <TABLE  style="Z-INDEX: 101; LEFT: 5.02%;POSITION: relative; TOP: 20px; " cellSpacing="1" cellPadding="1" width="700px"
				border="1">
              <TR>
                  <th colspan="3" align="left"><asp:label id="tbNotifyType" runat="server"></asp:label>֪ͨ������</th>
              </TR>
              <TR>
					<TD align="right"><asp:label id="Label4" runat="server">�ӳ����ɣ�</asp:label></TD>
					<TD><asp:textbox id="tbDelayReason" runat="server"></asp:textbox></TD>
                   <TD align="left"><asp:label id="Label7" runat="server">ʾ�������������쳣������<font color="red">ע�⣺���ó���32���ַ���</font></asp:label></TD>
              </TR>
                <TR>
					<TD align="right"><asp:label id="Label9" runat="server">����ʱ�䣺</asp:label></TD>
					<TD><asp:textbox id="tbToAccTime" runat="server"></asp:textbox></TD>
                   <TD align="left"><asp:label id="Label11" runat="server">ʾ����2015-06-01  <font color="red">ע�⣺���밴�ոø�ʽ�����ڣ������ܷ��ͳɹ���</font></asp:label></TD>
              </TR>
                 <TR>
                      <TD colspan="3" align="center"><asp:button id="btnSendNotify" runat="server" Width="100px" Height="30px" Text="����֪ͨ" onclick="btnSendNotify_Click"></asp:button>
                      <%--<asp:Button id="btnCancel" Text=" ȡ  �� " Runat="server" onclick="btnCancel_Click"></asp:Button>&nbsp;&nbsp;--%>
                      </TD>
                </TR>
               <TR>
					<TD colspan="3"><asp:label id="Label13" runat="server"><FONT face="����" color="red">֪ͨʾ��չʾ��</FONT></asp:label>
					</TD>
              </TR>
                <tr>
                <td colspan="3" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"> <asp:Image id="imgExample"   runat="server" Width="400px" Height="500px" />
                    </td>
                 
               </tr>
			</TABLE>
		</form>
	</body>
</HTML>
