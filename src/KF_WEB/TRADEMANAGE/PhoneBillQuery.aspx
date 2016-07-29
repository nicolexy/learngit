<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PhoneBillQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.PhoneBillQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PhoneBillQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">			
			function CheckDate()
			{
				var ddlDate = document.getElementById('ddlDate');
				var TextBoxBeginDate = document.getElementById('TextBoxBeginDate');
				var ButtonBeginDate = document.getElementById('ButtonBeginDate');
				var TextBoxEndDate = document.getElementById('TextBoxEndDate');
				var ButtonEndDate = document.getElementById('ButtonEndDate');
				
				if(ddlDate.value == 0)
				{
					TextBoxBeginDate.disabled = false;
					TextBoxEndDate.disabled = false;
					ButtonBeginDate.disabled = false;
					ButtonEndDate.disabled = false;
				}
				else
				{
					TextBoxBeginDate.disabled = true;
					TextBoxEndDate.disabled = true;
					ButtonBeginDate.disabled = true;
					ButtonEndDate.disabled = true;
				}
			}

			function CheckType()
			{
				var divFspid = document.getElementById('divFspid');
				var divFlistid = document.getElementById('divFlistid');
				var rbtFspid = document.getElementById('rbtFspid');
				var rbtFlistid = document.getElementById('rbtFlistid');
				
				if(rbtFspid.checked)
				{
					divFspid.style.display = "block";
					divFlistid.style.display = "none";
				}
				else
				{
					divFspid.style.display = "none";
					divFlistid.style.display = "block";
				}
			}
			
		</script>
	</HEAD>
	<body id="bodyid" runat="server">
		<form style="Z-INDEX: 0" id="Form1" method="post" runat="server">
			<TABLE style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="850">
				<TR>
					<TD style="WIDTH: 54.06%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;话费发货查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD width="100" align="right"><asp:radiobutton id="rbtFlistID" runat="server" Checked="True" GroupName="rbtCheck" Text="财付通订单号"></asp:radiobutton></TD>
					<TD style="WIDTH: 68px">
						<div id="divFspid" runat="server"><asp:textbox id="tbListID" runat="server" Width="200px"></asp:textbox></div>
					</TD>
					<TD width="100" align="right"><asp:radiobutton id="rbtPhoneNo" runat="server" GroupName="rbtCheck" Text="手机号码"></asp:radiobutton></TD>
					<TD>
						<div id="Div1" runat="server"><asp:textbox id="tbPhoneNo" runat="server" Width="200px" ontextchanged="Textbox1_TextChanged"></asp:textbox></div>
					</TD>
				</TR>
				<TR>
					<td style="HEIGHT: 22px" colSpan="4" align="center"><asp:button id="btnSearch" Width="80px" Text="查询" Runat="server" onclick="btnSearch_Click"></asp:button></td>
				</TR>
			</TABLE>
			<div style="POSITION: absolute; WIDTH: 850px; HEIGHT: 400px; OVERFLOW: auto; TOP: 150px; LEFT: 5%">
				<table border="0" cellSpacing="0" cellPadding="0" width="850">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="20" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FTransId" HeaderText="财付通订单号"></asp:BoundColumn>
									<asp:BoundColumn DataField="FSubmitTime" HeaderText="交易时间"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="FTotalFee_yuan" HeaderText="交易金额(元)"></asp:BoundColumn>
									<asp:BoundColumn DataField="FSpName_conv" HeaderText="商户名称"></asp:BoundColumn>
									<asp:BoundColumn DataField="FChargeTime" HeaderText="发货时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="FAmount_yuan" HeaderText="发货金额(元)"></asp:BoundColumn>
									<asp:BoundColumn DataField="FComment_conv" HeaderText="充值方式"></asp:BoundColumn>
									<asp:BoundColumn DataField="FChgMobile" HeaderText="手机号码"></asp:BoundColumn>
									<asp:BoundColumn DataField="FState_conv" HeaderText="状态"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FUserStateName" HeaderText="用户登录状态"></asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
					<TR height="25">
						<TD><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" PageSize="20" CustomInfoTextAlign="Center"
								AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left" PagingButtonSpacing="0" ShowInputBox="always"
								CssClass="mypager" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
