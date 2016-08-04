<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page language="c#" Codebehind="FreezeList.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FreezeList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>

  <HEAD>

		<title>FreezeList</title>

		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">

		<meta content="C#" name="CODE_LANGUAGE">

		<meta content="JavaScript" name="vs_defaultClientScript">

		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }

	.style3 { COLOR: #ff0000 }

	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }

	</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</HEAD>

	<body MS_POSITIONING="GridLayout">

		<form id="Form1" method="post" runat="server">

			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"

				cellPadding="1" width="85%" border="1">

				<TR>

					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;冻结记录查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>

				</TR>

				<TR>

					<TD style="WIDTH: 83px" align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>

					<TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>

					<TD align="right"><asp:label id="Label3" runat="server">结束日期</asp:label></TD>

					<TD>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
					</TD>

				</TR>

				<TR>

					<TD style="WIDTH: 83px" align="right"><asp:label id="Label5" runat="server">处理状态</asp:label></TD>

					<TD><asp:dropdownlist id="ddlStateType" runat="server" Width="152px">

							<asp:ListItem Value="0" Selected="True">所有状态</asp:ListItem>

							<asp:ListItem Value="1">处理中</asp:ListItem>

							<asp:ListItem Value="2">处理完成</asp:ListItem>

						</asp:dropdownlist></TD>

					<TD align="right"><asp:label id="Label6" runat="server">操作人员</asp:label></TD>

					<TD><asp:textbox id="tbFreezeUser" runat="server"></asp:textbox></TD>

				</TR>

				<TR>

					<TD style="WIDTH: 83px" align="right"><asp:label id="Label7" runat="server">操作类型</asp:label></TD>

					<TD><asp:dropdownlist id="ddlHandleType" runat="server" Width="152px">

							<asp:ListItem Value="0" Selected="True">所有类型</asp:ListItem>

							<asp:ListItem Value="1">冻结帐户</asp:ListItem>

							<asp:ListItem Value="2">锁定交易单</asp:ListItem>

						</asp:dropdownlist></TD>

					<TD align="right"><asp:label id="Label4" runat="server">用户姓名</asp:label></TD>

					<TD><asp:textbox id="tbUserName" runat="server"></asp:textbox></TD>

				</TR>

				<TR>

					<TD style="WIDTH: 81px" align="right" colSpan="1" rowSpan="1">

						<asp:Label id="Label8" runat="server" Width="104px">冻结帐号/交易单</asp:Label></TD>

					<TD>

						<asp:TextBox id="tbQQ" runat="server"></asp:TextBox></TD>

					<TD align="center" colSpan="2">

						<asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></TD>

				</TR>

			</TABLE>

			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 60%"

				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">

				<TR>

					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"

							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">

							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>

							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>

							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>

							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>

							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>

							<Columns>

								<asp:BoundColumn DataField="FUserName" HeaderText="用户姓名"></asp:BoundColumn>

								<asp:BoundColumn DataField="FFreezeTypeName" HeaderText="操作类型"></asp:BoundColumn>

								<asp:BoundColumn DataField="FHandleFinishName" HeaderText="处理状态"></asp:BoundColumn>

								<asp:BoundColumn DataField="FFreezeUserID" HeaderText="冻结人员"></asp:BoundColumn>

								<asp:BoundColumn DataField="FFreezeTime" HeaderText="冻结时间" DataFormatString="{0:D}"></asp:BoundColumn>

								<asp:HyperLinkColumn Text="详细内容" DataNavigateUrlField="FID" DataNavigateUrlFormatString="FreezeDetail.aspx?fid={0}" 
 HeaderText="详细内容"></asp:HyperLinkColumn>

							</Columns>

							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>

						</asp:datagrid></TD>

				</TR>

				<TR height="25">

					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到"

							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"

							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>

				</TR>

			</TABLE>

		</form>

	</body>

</HTML>

