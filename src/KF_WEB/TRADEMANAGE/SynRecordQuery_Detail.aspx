<%@ Page language="c#" Codebehind="SynRecordQuery_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SynRecordQuery_Detail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SynRecordQuery_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server" encType="multipart/form-data">
			&nbsp;
			<TABLE id="Table2" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"
				cellPadding="1" width="90%" border="1">
				<TR bgColor="#eeeeee" height="24">
					<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">同步记录详细信息</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:Label id="Label2" runat="server">操作员代码：</asp:Label>
									<asp:Label id="labUid" runat="server" Width="64px"></asp:Label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="2" align="center">
						<TABLE style="WIDTH: 100%" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server"
							ID="Table1">
							<TBODY>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4">
									</td>
								</tr>
								<TR>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label10" runat="server">交易单ID：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFtransaction_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" width="20%">
										<P align="right"><asp:label id="Label1" runat="server">支付状态：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" width="30%">
										<P align="left"><asp:label id="labFpay_statusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label14" runat="server">同步次数：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="labFsyn_count" runat="server" ForeColor="Blue"></asp:label>
											<asp:Button id="Button1" runat="server" Text="同步重置" onclick="Button1_Click"></asp:Button>
											<asp:TextBox id="tbNewCount" runat="server" Width="48px">0</asp:TextBox></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="right"><asp:label id="Label12" runat="server">同步状态：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 17px">
										<P align="left"><asp:label id="labFsyn_statusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label5" runat="server">记录状态：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="labFrec_statusName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="right"><asp:label id="Label15" runat="server">同步类型：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 18px">
										<P align="left"><asp:label id="labFsyn_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"><FONT face="宋体"></FONT></td>
								</tr>
								<TR>
									<TD align="center">
										<P align="right"><asp:label id="Label7" runat="server">支付类型：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFpay_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center">
										<P align="right"><FONT face="宋体"></FONT><asp:label id="Label16" runat="server">支付渠道：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center">
										<P align="left"><asp:label id="labFpay_channelName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label23" runat="server">商户版本号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFverName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label25" runat="server">商户编号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_id" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label3" runat="server">商户订单号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_billno" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label6" runat="server">商户请求时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label9" runat="server">银行类型：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbank_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label13" runat="server">给银行订单：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbank_billno" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label18" runat="server">银行返回订单：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbank_ret_billno" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label20" runat="server">买家帐号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpurchaser_uin" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label22" runat="server">买家姓名：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpurchaser_true_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label26" runat="server">卖家帐号：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbargainor_uin" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label28" runat="server">卖家姓名：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFbargainor_true_name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label30" runat="server">商品标识：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFgoods_tag" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label32" runat="server">交易描述：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFdesc" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label34" runat="server">支付总金额：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFtotal_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label36" runat="server">货品费用：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpriceName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label38" runat="server">运输费用：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFtransport_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label40" runat="server">交易手续费：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFprocedure_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label42" runat="server">现金币种：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee_typeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label44" runat="server">现金支付金额：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee1Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label46" runat="server">购物券支付金额：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee2Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label48" runat="server">其它费用：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFfee3Name" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label50" runat="server">优惠金额：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFvfeeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label52" runat="server">退买家费用：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFrp_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label54" runat="server">退卖家费用：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFrb_feeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label56" runat="server">用户IP：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFclient_ip" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label58" runat="server">商户IP：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_ip" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label60" runat="server">创建时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFcreat_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label62" runat="server">支付成功时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFpay_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label64" runat="server">同步时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFlast_syn_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label66" runat="server">修改时间：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFlast_modify_timeName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label68" runat="server">同步结果：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="Fsyn_resultName" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label4" runat="server">商户MAC：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px">
										<P align="left"><asp:label id="labFsp_mac" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label8" runat="server">回跳URL：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px" colspan="3">
										<P align="left"><asp:label id="labFReturn_Url" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR>
									<TD align="center" style="HEIGHT: 16px">
										<P align="right"><asp:label id="Label11" runat="server">备注：</asp:label><FONT face="宋体">:</FONT></P>
									</TD>
									<TD align="center" style="HEIGHT: 16px" colspan="3">
										<P align="left"><asp:label id="labFMemo" runat="server" ForeColor="Blue"></asp:label></P>
									</TD>
								</TR>
								<tr borderColor="#999999" bgColor="#999999" style="HEIGHT: 4px">
									<td colSpan="4"></td>
								</tr>
								<TR style="HEIGHT: 46px">
									<TD vAlign="middle" align="center" colSpan="4"><FONT face="宋体"></FONT></TD>
								</TR>
							</TBODY>
						</TABLE>
						<INPUT style="WIDTH: 64px; HEIGHT: 22px" type="button" value="返回" onclick="history.go(-1)">
					</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
