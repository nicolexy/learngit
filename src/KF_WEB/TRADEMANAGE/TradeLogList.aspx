<%@ Page language="c#" Codebehind="TradeLogList.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.TradeLogList" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TradeLogList</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css );
.style2 {
	COLOR: #000000
}
.style3 {
	COLOR: #ff0000
}
BODY {
	BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif)
}
TD {
	FONT-SIZE: 9pt
}
.style4 {
	COLOR: #ff0000
}
		</style>
		<script language="javascript">				
					function IsShowState()
					{
					    var ListState_0 = document.getElementById("ListState_0");
					    var DropDownList2_tradeState = document.getElementById("DropDownList2_tradeState");
					    
					    if(ListState_0.checked)
					        DropDownList2_tradeState.disabled = true;
					    else
					        DropDownList2_tradeState.disabled = false;
					}
					
					

		</script>
        <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT>
			<br>
			<table border="0" cellSpacing="1" cellPadding="0" width="95%" bgColor="#666666" align="center">
				<tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
					<td height="20" vAlign="middle" colSpan="2">
						<table border="0" cellSpacing="0" cellPadding="1" width="100%" height="90%">
							<tr>
								<td height="18" background="../IMAGES/Page/bg_bl.gif" width="80%"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
										�̻������嵥��ѯ�����ڿ��15�죩</font>
									<div align="right"><FONT color="#ff0000" face="Tahoma"></FONT></div>
								</td>
								<td background="../IMAGES/Page/bg_bl.gif" width="20%">����Ա����: <span class="style3">
										<asp:label id="Label_uid" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<table border="0" cellSpacing="0" cellPadding="1" width="100%" height="100%">
							<tr>
								<td style="HEIGHT: 37px" width="78%">
									<P align="left"><asp:radiobutton id="ListSpidSelect" runat="server" GroupName="Spid" Text="ѡ���̻�" Enabled="False"></asp:radiobutton><asp:dropdownlist id="DropDownList1" runat="server"></asp:dropdownlist><asp:radiobutton id="ListSpidInput" runat="server" GroupName="Spid" Text="�����̻���" Checked="True"></asp:radiobutton><asp:textbox id="TextBoxSpid" runat="server" Width="104px" BorderStyle="Groove"></asp:textbox><asp:label id="lblFcode" Runat="server">��������</asp:label><asp:textbox id="txtFcode" Width="104px" BorderStyle="Groove" Runat="server"></asp:textbox><BR>
										<asp:radiobuttonlist id="ListState" onclick="IsShowState();" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
											<asp:ListItem Value="���н���״̬" Selected="True">���н���״̬</asp:ListItem>
											<asp:ListItem Value="ָ������״̬">ָ������״̬</asp:ListItem>
										</asp:radiobuttonlist>��
										<asp:dropdownlist id="DropDownList2_tradeState" runat="server" Enabled="False" ForeColor="Black"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;���ڴ�
										<asp:textbox id="txDateBegin" runat="server" Width="144px" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"  CssClass="Wdate"></asp:textbox>&nbsp;��
										<asp:textbox id="txDateEnd" runat="server" Width="144px" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"  CssClass="Wdate"></asp:textbox>
                                        <label style="color:red">ʱ����ֻ֧�ְ���Ȼ�²�ѯ����֧�ֿ��²�ѯ</label>
									</P>
                                    
								</td>
								<TD style="HEIGHT: 40px" width="3%">&nbsp;</TD>
							</tr>
						</table>
						<asp:label id="LabelError" runat="server" ForeColor="Red"></asp:label></td>
					<td width="12%">
						<div align="center"><asp:button id="btQuery" runat="server" Text="��ѯ" Width="66px" BorderStyle="Groove" Height="23px" onclick="btQuery_Click"></asp:button></div>
					</td>
				</tr>
			</table>
			<div align="center">
				<asp:datagrid id="DataGrid1" runat="server" Width="95%" BorderStyle="None" ForeColor="Black" PageSize="20"
					AutoGenerateColumns="False" HorizontalAlign="Center" CellPadding="2" BackColor="White" BorderColor="#DEDFDE"
					AllowPaging="True" BorderWidth="1px">
					<FooterStyle BackColor="#CCCC99"></FooterStyle>
					<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
					<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
					<ItemStyle BackColor="#F7F7DE"></ItemStyle>
					<HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Center" ForeColor="White" BackColor="#6B696B"></HeaderStyle>
					<Columns>
						<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="flistid" DataNavigateUrlFormatString="TradeLogQuery.aspx?id={0}"
							DataTextField="flistid" HeaderText="������">
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
					<PagerStyle HorizontalAlign="Left" ForeColor="Black" BackColor="White" Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
				<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
					SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD></div>
		</form>
	</body>
</HTML>
