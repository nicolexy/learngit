<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ValueAddedTaxApprove.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ValueAddedTaxApprove" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ValueAddedTaxApprove</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="850">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="4"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;营改增商户审核</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">商户号：</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left"><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">审核状态：</TD>
					<TD style="WIDTH: 203px; HEIGHT: 27px"><asp:dropdownlist id="ddlFlag" Width="200px" Runat="server">
							<asp:ListItem Value="1">初次申请授权书待上传</asp:ListItem>
							<asp:ListItem Value="2">初次申请审核中</asp:ListItem>
							<asp:ListItem Value="3">全单修改审核中</asp:ListItem>
							<asp:ListItem Value="4">审核通过</asp:ListItem>
							<asp:ListItem Value="5">审核不通过</asp:ListItem>
							<asp:ListItem Value="6">收件人信息修改中</asp:ListItem>
							<asp:ListItem Value="7">(用户自助修改成功)收件人信息修改成功</asp:ListItem>
							<asp:ListItem Value="8">收件人信息修改失败</asp:ListItem>
							<asp:ListItem Value="9">待商户提交全单修改申请</asp:ListItem>
							<asp:ListItem Value="10">全单修改授权书待上传</asp:ListItem>
							<asp:ListItem Value="2|3" Selected="True">待审核状态</asp:ListItem>
							<asp:ListItem Value="">全部</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD colSpan="4" align="center"><asp:button id="btnSearch" Width="80px" Runat="server" Text="查  询" onclick="btnSearch_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<div style="POSITION: absolute; WIDTH: 850px; HEIGHT: 800px; OVERFLOW: auto; TOP: 140px; LEFT: 5%">
				<table border="0" cellSpacing="0" cellPadding="0">
					<TR>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="SPID" HeaderText="商户号">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" HeaderText="商户名称">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTypeStr" HeaderText="申请类型">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="申请时间">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FlagStr" HeaderText="状态">
										<HeaderStyle Width="300px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemStyle Width="30px"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink id="hylDetail" runat="server" Text='查看' NavigateUrl='<%#"ValueAddedTaxDetail.aspx?TaskID=" +DataBinder.Eval(Container.DataItem, "TaskId")%>'>
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR height="25">
						<TD><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" PageSize="10" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="10" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
