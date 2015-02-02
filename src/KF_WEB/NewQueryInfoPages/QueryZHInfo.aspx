<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryZHInfo.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryZHInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryZHInfo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
		}
		function showRadioClick() 
        {
            var detailTB_id = document.getElementById("detailTB");
            var certTB_id = document.getElementById("certTB");
            var rd = document.getElementsByName("radioListOrder");
            
            for (var i = 1; i < rd.length; i++) {
                if (rd[i].checked && rd[i].value == "0") {
                    detailTB_id.style.display = "block";
                    certTB_id.style.display = "none";
                }
                if (rd[i].checked && rd[i].value == "1") {
                    detailTB_id.style.display = "none";
                    certTB_id.style.display = "block";
                }
             } 
        }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
            
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�����˺���Ϣ��ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<tr>
                    <td>
                      <!--  <asp:radiobuttonlist id="radioListOrder" onclick="showRadioClick()" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
					      <asp:ListItem Value="0" Selected="True">��ͨ�����˺������ѯ</asp:ListItem>
					      <asp:ListItem Value="1">ͬһ���֤��ͨ��ͨ�˺Ÿ�����ѯ</asp:ListItem>
				      </asp:radiobuttonlist>-->
                      <asp:radiobutton id="rbYtno" runat="server" Text="���й����˺������ѯ" Checked="True" GroupName="id" AutoPostBack="True" ToolTip="��ͨ�����˺�"></asp:radiobutton>
                      <asp:radiobutton id="rbCertno" runat="server" Text="ͬһ���֤��ͨ������ѯ" GroupName="id" AutoPostBack="True" ToolTip="���֤��"></asp:radiobutton>
                    </td>
                </tr>
                
			</TABLE>
			
            <TABLE id="detailTB" cellSpacing="1" cellPadding="1" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 104px; HEIGHT: 30%"  border="1" runat="server">
                <TR>
                    <TD align="left" colspan="4"><asp:label id="Label2" runat="server">�Ƹ�ͨ�˺ţ�</asp:label>
                    <asp:textbox id="cftNo" style="WIDTH: 180px;" runat="server"></asp:textbox>
					<asp:label id="Label3" runat="server">���й����˺ţ�</asp:label>
                    <asp:textbox id="ytNo" style="WIDTH: 180px;" runat="server"></asp:textbox>
                    <asp:label id="Label5" runat="server">��Ч�ڣ�</asp:label>
                    <asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    <asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
                    </TD>
				</TR>
                
				<TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�Ƹ�ͨ�˺�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�״ο�ͨʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���й����˺�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�˺�״̬:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�ֻ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;email:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;֧�����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
                   <TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���һ�θ���ʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;��������֧���޶�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;����֧���޶�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;����¼IP��ַ:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
            </TABLE>
            <TABLE id="certTB" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP:104px;" cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
                    <TD align="left" colspan="4"><asp:label id="Label4" runat="server">���֤�ţ�</asp:label>
                    <asp:textbox id="certNo" style="WIDTH: 210px;" runat="server"></asp:textbox>
					<asp:button id="btnCert" runat="server" Width="80px" Text="�� ѯ" onclick="btnCert_Click"></asp:button></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���֤��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�ѿ�ͨ��ͨ�����˺Ÿ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
				</TR>
			</TABLE>
            </form>
	</body>
</HTML>
