<%@ Page language="c#" Codebehind="AdjustQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SpSettle.AdjustQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SettleReqQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
     <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT>
			<br>
			<br>
			<table cellSpacing="0" cellPadding="0" width="900" border="0">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5">
						<P><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;调帐查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></P>
					</TD>
				</TR>
                <TR>
					<TD>
						<TABLE style="WIDTH: 1080px; HEIGHT: 38px" cellSpacing="1" cellPadding="1" width="1080"
							border="1">
							<TR>
								<TD style="WIDTH: 389px">财付通订单号:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtListid" runat="server" Width="260px"></asp:textbox></TD>
                                <TD style="WIDTH: 389px" colspan="2">商户订单号:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtOrderid" runat="server" Width="260px"></asp:textbox></TD>
							</TR>
                            <TR>
								<TD style="WIDTH: 289px">商户号:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtSpid" runat="server" Width="160px"></asp:textbox></TD>
                                <TD style="WIDTH: 289px">调帐时间:&nbsp;
								<input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                                <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                                </TD>
                                <TD><FONT face="宋体">&nbsp;<asp:button id="Button_qry" runat="server" Text="查询" onclick="Button_qry_Click"></asp:button></FONT></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="900" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
							PageSize="20" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px"
							BorderStyle="None" BorderColor="#E7E7FF">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="Ftransaction_id" HeaderText="财付通订单号">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fsp_bill_no" HeaderText="商户订单号">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="商户号">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fnum_str" HeaderText="调账金额">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ftype_str" HeaderText="调账方向">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fstatus_str" HeaderText="调账状态">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmodify_time" HeaderText="修改时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                
			</table>
            
		</form>
		
	</body>
</HTML>
