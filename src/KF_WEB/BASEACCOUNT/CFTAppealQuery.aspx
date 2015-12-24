<%@ Page language="c#" Codebehind="CFTAppealQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.CFTAppealQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FundQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
          <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">
					function Show()
					{
					    var TextBoxBeginDate = document.getElementById("TextBoxBeginDate");
					    var ButtonBeginDate = document.getElementById("ButtonBeginDate");
					    var TextBoxEndDate = document.getElementById("TextBoxEndDate");
					    var ButtonEndDate = document.getElementById("ButtonEndDate");
					    var tbFuin = document.getElementById("tbFuin");
					    var ddlState = document.getElementById("ddlState");
					    var txtQQ = document.getElementById("txtQQ");
					    var rbtnFuin = document.getElementById("rbtnFuin");
					    var rbtnQQ = document.getElementById("rbtnQQ");
					    
					    if(rbtnFuin.checked)
					    {
					        txtQQ.disabled = true;
					        
					        TextBoxBeginDate.disabled = false;
					        ButtonBeginDate.disabled = false;
					        TextBoxEndDate.disabled = false;
					        ButtonEndDate.disabled = false;
					        tbFuin.disabled = false;
					        ddlState.disabled = false;
					    }
					    else
					    {
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
		<div id="movediv" style="Z-INDEX: 102; POSITION: absolute; VISIBILITY: hidden; TOP: 300px; LEFT: 100px">
			<img id="moveimgid" width="400" height="300"></div>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1" cellPadding="1"
				width="85%" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="宋体" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;自助申诉处理查询</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="宋体">操作员代码: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">开始日期</asp:label></TD>
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
					<TD style="WIDTH: 83px" align="right"><asp:RadioButton ID="rbtnFuin" Runat="server" Text="申诉处理人" GroupName="rbtnGroup" onclick="Show();"></asp:RadioButton></TD>
					<TD><asp:textbox id="tbFuin" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 83px" align="right"><asp:label id="Label6" runat="server">处理状态</asp:label></TD>
					<TD><asp:dropdownlist id="ddlState" runat="server">
							<asp:ListItem Value="9">全部</asp:ListItem>
							<asp:ListItem Value="1" Selected="True">申诉成功</asp:ListItem>
							<asp:ListItem Value="2">申诉失败</asp:ListItem>
							<asp:ListItem Value="4">直接转后台</asp:ListItem>
							<asp:ListItem Value="3">其它转后台</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<tr>
					<td style="WIDTH: 83px" align="right">
						<asp:RadioButton ID="rbtnQQ" Runat="server" Text="用户帐号" GroupName="rbtnGroup" onclick="Show();"></asp:RadioButton>
					<td>
						<asp:TextBox ID="txtQQ" Runat="server"></asp:TextBox>
					</td>
					<TD align="center" colSpan="2"><FONT face="宋体"><asp:button id="Button2" runat="server" Width="80px" Text="查 询" onclick="Button2_Click"></asp:button></FONT></TD>
				</tr>
			</TABLE>
			<TABLE id="Table2" style="POSITION: absolute; WIDTH: 85%; HEIGHT: 70%; TOP: 145px; LEFT: 5%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Femail" HeaderText="Email"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTypeName" HeaderText="申诉类型"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateName" HeaderText="申诉状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCheckUser" HeaderText="处理人"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCheckTime" HeaderText="处理时间" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="扫描件">
									<ItemTemplate>
										<img src='<%#rooturl + DataBinder.Eval(Container, "DataItem.cre_image") %>' height="25" width="25" onmouseout="HiddenImg(movediv)" 

										onmousemove="ShowDetailImg(movediv,moveimgid,this)" id=IMG1>
									</ItemTemplate>
								</asp:TemplateColumn>
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
		<DIV></DIV>
	</body>
</HTML>
