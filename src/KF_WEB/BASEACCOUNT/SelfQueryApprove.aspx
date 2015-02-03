<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="SelfQueryApprove.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.SelfQueryApprove" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SelfQueryApprove</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
		function openModeBegin()
		{
			var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');

			if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
		}
		
		function openModeEnd()
		{
			var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');

			if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
		}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="850" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="6"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;自助商户审核</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">商户名称：</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px" align="left"><asp:textbox id="BoxCpName" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">商户号：</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left"><asp:textbox id="BoxCpNumber" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">网址：</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px"><asp:textbox id="boxWWWAddress" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">提交时间段</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px"><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">至</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px"><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">开户名称：</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px"><asp:textbox id="tbBankName" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">推荐人：</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px"><asp:textbox id="tbSuggestUser" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">查询结果：</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px"><asp:label id="lblResultCount" Runat="server"></asp:label></TD>
					<TD><asp:button id="btnSearch" runat="server" Text="查 询" onclick="btnSearch_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 850px; POSITION: absolute; TOP: 140px; HEIGHT: 800px">
				<table cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px"
								BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="ApplyCpInfoID" HeaderText="序号"></asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" HeaderText="商户名称">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BankUserName" HeaderText="开户名称">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BargainCode" HeaderText="合同号">
										<HeaderStyle Width="70px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="申请时间">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UserName" HeaderText="申请人">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="KFCheckUser" HeaderText="领单人">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemStyle Width="30px"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink id="hylDetail" runat="server" Text='查看' NavigateUrl='<%#"SelfQueryDetail.aspx?mode=approve&ID=" +DataBinder.Eval(Container.DataItem, "ApplyCpInfoID")+"&SPID="+DataBinder.Eval(Container.DataItem, "SPID")%>'>
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR height="25">
						<TD><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" CustomInfoTextAlign="Center" AlwaysShow="True"
								NumericButtonCount="5" ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always"
								CssClass="mypager" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
								PageSize="10"></webdiyer:aspnetpager></TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
