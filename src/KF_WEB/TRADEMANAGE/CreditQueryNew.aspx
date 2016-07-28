<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="CreditQueryNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.CreditQueryNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CreditQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
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
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="850" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="2"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;信用卡还款</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right" width="100"><asp:radiobutton id="rbtFspid" runat="server" Text="财付通账号" GroupName="rbtCheck"></asp:radiobutton></TD>
					<TD>
						<div id="divFspid" runat="server">
							<asp:textbox id="txtFspid" runat="server"></asp:textbox>
							<asp:label id="Label3" runat="server">日期</asp:label>
							<asp:dropdownlist id="ddlDate" runat="server">
								<asp:ListItem Value="1">最近三个月</asp:ListItem>
								<asp:ListItem Value="0">指定日期</asp:ListItem>
							</asp:dropdownlist><asp:label id="Label2" runat="server">开始日期</asp:label>
                            <asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox><asp:label id="Label5" runat="server">结束日期</asp:label><asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</div>
					</TD>
				</TR>
				<TR>
					<TD align="right" width="100"><asp:radiobutton id="rbtFlistid" runat="server" Text="还款交易号" GroupName="rbtCheck"></asp:radiobutton></TD>
					<TD>
						<div id="divFlistid" runat="server">
							<asp:textbox id="txtFlistid" runat="server"></asp:textbox>
						</div>
					</TD>
					<td></td>
					<td></td>
				</TR>
				<TR>
					<td align="center" colSpan="4"><asp:button id="btnSearch" Text="查询" Runat="server" Width="80px" onclick="btnSearch_Click"></asp:button></td>
				</TR>
				<tr>
					<td></td>
				</tr>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 850px; POSITION: absolute; TOP: 150px; HEIGHT: 400px">
				<table cellSpacing="0" cellPadding="0" width="850" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" PageSize="20" HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fpay_front_time" HeaderText="还款日期">
										<HeaderStyle Width="130px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Flistid" HeaderText="还款交易号">
										<HeaderStyle Width="180px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Fbank_type">
										<HeaderStyle Width="180px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_name" HeaderText="发卡银行">
										<HeaderStyle Width="180px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="creditcard_id" HeaderText="卡号后四位">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fnum" HeaderText="还款金额（元）">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fsign" HeaderText="状态">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
					<TR height="25">
						<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="20" HorizontalAlign="right" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="转到" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
