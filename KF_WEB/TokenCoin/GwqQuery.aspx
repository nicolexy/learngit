<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="GwqQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TokenCoin.GwqQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GwqQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css  ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	TD { FONT-SIZE: 9pt }
	.style4 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<body runat="server" id="bodyId">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="95%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<tr>
								<td width="80%" background="../IMAGES/Page/bg_bl.gif" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										财付券查询</font>
									<div align="right"><FONT face="Tahoma" color="#ff0000"></FONT></div>
								</td>
								<td width="20%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style3">
										<asp:label id="Label_uid" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<table height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
							<tr>
								<td style="HEIGHT: 37px" width="78%"><STRONG><asp:radiobutton id="RadioSpid" runat="server" GroupName="BY" Text="使用者帐号" Checked="True"></asp:radiobutton></STRONG><asp:textbox id="TextBoxSpid" runat="server" BorderStyle="Groove" Width="100px"></asp:textbox>&nbsp;&nbsp;<strong>
										<asp:radiobutton id="RadioID" runat="server" GroupName="BY" Text="发行批次号"></asp:radiobutton></strong><asp:textbox id="TextBoxId" runat="server" BorderStyle="Groove" Width="70px"></asp:textbox>&nbsp;&nbsp; 
									按业务状态
									<asp:DropDownList id="ListState" runat="server"></asp:DropDownList></td>
								<TD style="HEIGHT: 40px" width="3%">&nbsp;</TD>
							</tr>
						</table>
					</td>
					<td width="12%">
						<div align="center"><asp:button id="btQuery" runat="server" Text="查询" BorderStyle="Groove" Width="66px" Height="23px"></asp:button></div>
					</td>
				</tr>
			</table>
			<div align="center"><FONT face="宋体"></FONT><BR>
				<asp:datagrid id="DataGrid1" runat="server" BorderStyle="None" Width="95%" BorderWidth="1px" BorderColor="#DEDFDE"
					BackColor="White" CellPadding="2" HorizontalAlign="Center" AutoGenerateColumns="False" PageSize="20"
					ForeColor="Black">
					<FooterStyle BackColor="#CCCC99"></FooterStyle>
					<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
					<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
					<ItemStyle BackColor="#F7F7DE"></ItemStyle>
					<HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Center" ForeColor="White" BackColor="#6B696B"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="查看">
							<ItemTemplate>
								<asp:HyperLink ID="HyperLink1" runat="server" Text="查看" NavigateUrl='<%# "GwqShow.aspx?id="+DataBinder.Eval(Container, "DataItem.fticket_id")+"&user="+DataBinder.Eval(Container, "DataItem.Fuser_uid") %>' Target="_blank">
								</asp:HyperLink>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="Fticket_id" HeaderText="购物券ID">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Ftde_id" HeaderText="发行批次号">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:HyperLinkColumn DataNavigateUrlField="Fpub_id" Target="_blank" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
							DataTextField="Fpub_id" HeaderText="发行者号码">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:HyperLinkColumn>
						<asp:BoundColumn DataField="Fatt_name" HeaderText="购物券名称">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Ftype" HeaderText="类型">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="FFee" HeaderText="面值">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundColumn>
						<asp:HyperLinkColumn DataNavigateUrlField="Fuser_id" Target="_blank" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
							DataTextField="Fuser_id" HeaderText="使用者">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:HyperLinkColumn>
						<asp:HyperLinkColumn DataNavigateUrlField="Fdonate_id" Target="_blank" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
							DataTextField="Fdonate_id" HeaderText="赠送者">
							<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:HyperLinkColumn>
						<asp:BoundColumn DataField="Fstate" HeaderText="业务状态">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						</asp:BoundColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Left" ForeColor="Black" BackColor="White" Mode="NumericPages"></PagerStyle>
				</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" Width="95%" HorizontalAlign="right" NumericButtonTextFormatString="[{0}]"
					SubmitButtonText="转到" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
					ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="18"></webdiyer:aspnetpager></div>
		</form>
	</body>
</HTML>
