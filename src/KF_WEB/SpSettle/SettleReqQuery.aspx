<%@ Page language="c#" Codebehind="SettleReqQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SpSettle.SettleReqQuery" %>
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
						<P><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;分账请求查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                <TD style="WIDTH: 389px">分账请求流水号:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtReqid" runat="server" Width="260px"></asp:textbox></TD>
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
                                <asp:BoundColumn Visible="false" DataField="Flistid" HeaderText="财付通订单号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsettle_request_id" HeaderText="分账请求流水号">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcoding" HeaderText="商户订单">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ftotal_fee_str" HeaderText="支付金额">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fsettle_fee_str" HeaderText="分账金额">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fstate_str" HeaderText="分账状态">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="修改时间">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="查看">
                                    <ItemTemplate>
                                        <asp:LinkButton id="lbReqDetail" runat="server" CommandName="REQDETAIL">请求明细</asp:LinkButton>&nbsp;
                                        <asp:LinkButton id="lbSetDetail" runat="server" CommandName="SETDETAIL">分账明细</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <tr>
                    <td>
                        <TABLE id="rq_tb2" Width="900" visible="false" cellSpacing="1" cellPadding="0" bgColor="black" border="0" runat="server">
                <TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;请求明细：</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;分账请求流水号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;分账总单:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;订单编码:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;财付通订单号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;PNR码:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;联系人:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;机票平台ID:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;航程:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;联系电话:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;机票张数:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;币种:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;总分账金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;订单支付金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;业务规则:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;分账参数:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;分账原始描述:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;商户回调URL:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;分账状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;订单物理状态:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;创建时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;分账时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;修改时间:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c22" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 125px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;代理商信息:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 185px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
            </TABLE>
                    </td>
                </tr>
			</table>
            
		</form>
		
	</body>
</HTML>
