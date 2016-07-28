<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="CFTUserAppeal.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.CFTUserAppeal" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">
		    function Show() {
		        var TextBoxBeginDate = document.getElementById("TextBoxBeginDate");
		        var ButtonBeginDate = document.getElementById("ButtonBeginDate");
		        var TextBoxEndDate = document.getElementById("TextBoxEndDate");
		        var ButtonEndDate = document.getElementById("ButtonEndDate");
		        var tbFuin = document.getElementById("tbFuin");
		        var ddlState = document.getElementById("ddlState");
		        var txtQQ = document.getElementById("txtQQ");
		        var rbtnFuin = document.getElementById("rbtnFuin");
		        var rbtnQQ = document.getElementById("rbtnQQ");

		        if (rbtnFuin.checked) {
		            txtQQ.disabled = true;

		            TextBoxBeginDate.disabled = false;
		            ButtonBeginDate.disabled = false;
		            TextBoxEndDate.disabled = false;
		            ButtonEndDate.disabled = false;
		            tbFuin.disabled = false;
		            ddlState.disabled = false;
		        }
		        else {
		            txtQQ.disabled = false;

		            TextBoxBeginDate.disabled = true;
		            ButtonBeginDate.disabled = true;
		            TextBoxEndDate.disabled = true;
		            ButtonEndDate.disabled = true;
		            tbFuin.disabled = true;
		            ddlState.disabled = true;
		        }
		    }

		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;自助申诉查询</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label5" runat="server">用户帐号</asp:label></TD>
					<TD><asp:textbox id="tbFuin" runat="server"></asp:textbox>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="tbFuin" ErrorMessage="请输入帐号"></asp:RequiredFieldValidator></TD>
                    <td>开始日期：</td>
                    <td>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    </td>
                    <td>结束日期：</td>
                    <td>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                    </td>
					<TD align="center" colspan="2"><FONT face="宋体"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; WIDTH: 85%; HEIGHT: 70%; TOP: 130px; LEFT: 5%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%" EnableViewState="False">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Femail" HeaderText="Email"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTypeName" HeaderText="申诉类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="申诉状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSubmitTime" HeaderText="提交时间" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCheckInfo" HeaderText="审核信息"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FCheckTime" HeaderText="审批时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FReCheckTime" HeaderText="二次审批时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FCheckUser" HeaderText="审批人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FReCheckUser" HeaderText="二次审批人"></asp:BoundColumn>

							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
