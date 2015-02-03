<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QuerySpOrderPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.QuerySpOrderPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QuerySpOrderPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css );
UNKNOWN {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_beginDate.value=returnValue;
					}
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_endDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_endDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="900">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;��ѯ�Ƹ�ͨ����</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label id="Label1" runat="server">�̻��ţ�&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_spid" Width="250" Runat="server"></asp:textbox></td>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label id="Label2" runat="server">�̻������ţ�&nbsp;&nbsp;&nbsp;</asp:label><asp:textbox id="tbx_spcoding" Width="250" Runat="server"></asp:textbox><asp:dropdownlist id="ddl_state" Runat="server" Visible="False"></asp:dropdownlist></td>
				</tr>
				<TR>
					<TD colSpan="4" align="center"><asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button></TD>
				</TR>
			
			<tr>
				<td colspan=4 align=left>
					<asp:datagrid id="DataGrid1" runat="server" ForeColor="Black" Width="100%" BorderStyle="None" BorderWidth="1px"
					AllowPaging="false" BorderColor="#DEDFDE" BackColor="White" CellPadding="2" HorizontalAlign="left"
					AutoGenerateColumns="False">
					<FooterStyle BackColor="#CCCC99"></FooterStyle>
					<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
					<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
					<ItemStyle BackColor="#F7F7DE"></ItemStyle>
					<HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Center" ForeColor="White" BackColor="#6B696B"></HeaderStyle>
					<Columns>
						<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="flistidUrl" DataTextField="flistid" HeaderText="������">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:HyperLinkColumn>
						<asp:BoundColumn DataField="Fcoding" HeaderText="��������">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="fmodify_time" HeaderText="��������">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fstate" HeaderText="����״̬">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="FBuyid" HeaderText="����ʺ�">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="FBuy_name" HeaderText="�������">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fpaynum" HeaderText="���׽��">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="fmemo" HeaderText="����˵��">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
					</Columns>
				</asp:datagrid>
				</td>
			</tr>
			<!-- <p><label style="FONT-SIZE: 15px">�Ƹ�ͨ�����ţ�</label><asp:label style="FONT-SIZE: 15px" id="lb_cftListID" Runat="server"></asp:label></p> -->
			<tr>
				<td colspan=4 align=right>
					<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
					SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager>
				</td>
			</tr>
			
				
		
			</TABLE>
		</form>
	</body>
</HTML>
