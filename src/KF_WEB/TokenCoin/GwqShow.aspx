<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="GwqShow.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TokenCoin.GwqShow" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GwqShow</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	TD { FONT-SIZE: 9pt }
	.style4 { COLOR: #ff0000 }
		</style>
	</HEAD>
	<BODY id="bodyId" runat="server">
		<form id="Form1" method="post" encType="multipart/form-data" runat="server">
			&nbsp;
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="90%" align="center" border="1">
				<TBODY>
					<TR bgColor="#eeeeee" height="24">
						<TD colSpan="2"><FONT color="#ff0000"><SPAN class="style1"><IMG height="16" src="../IMAGES/Page/post.gif" width="15"><STRONG>&nbsp;<asp:label id="lbTitle" runat="server">财付券查看</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="Label2" runat="server">操作员代码：</asp:label><asp:label id="labUid" runat="server" Width="64px"></asp:label><SPAN class="style3"></SPAN></STRONG></SPAN></FONT></TD>
					</TR>
					<TR>
						<TD align="right" colSpan="2">
							<TABLE id="Table1" style="WIDTH: 100%" cellSpacing="0" cellPadding="1" width="100%" border="0"
								runat="server">
								<TBODY>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"></td>
									</tr>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label7" runat="server">使用者帐号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUserId" runat="server"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label33" runat="server">财付券ID号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxId" runat="server" Width="160px"></asp:textbox><asp:button id="Button1" runat="server" Text="查询"></asp:button></TD>
									</TR>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"></td>
									</tr>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label10" runat="server">发行批次号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxTdeId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label19" runat="server">子批次号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxSonId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label24" runat="server">财付券名称：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxAttName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label25" runat="server">商品代码：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxMerId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label21" runat="server">发行者帐号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label14" runat="server">发行者内部帐号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubUId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label12" runat="server">发行者名称：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubName" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label39" runat="server">发行IP：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubIp" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label28" runat="server">类型：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxType" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label29" runat="server">发行类型：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubType" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT face="宋体"></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label22" runat="server">发行日期：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label23" runat="server">发行操作员：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxPubUser" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label11" runat="server">面值（元/比例）：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxFee" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label36" runat="server">实际使用金额：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxFactFee" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label15" runat="server">最高使用比例：</asp:label></TD>
										<TD>&nbsp; 万分之
											<asp:textbox id="TextBoxUsePro" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox>
											<asp:label id="Label32" runat="server">(10000表示不限制)</asp:label></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label16" runat="server">最低使用限额：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxMinFee" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox>元
											<asp:label id="Label35" runat="server">(0表示不限制)</asp:label></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label17" runat="server">每次限制使用数量：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxMaxNum" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox>张
											<asp:label id="Label34" runat="server">(0表示不限制)</asp:label></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label13" runat="server">是否允许赠送：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxDonateType" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label8" runat="server">生效日期：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxSTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label9" runat="server">结束日期：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxETime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"></td>
									</tr>
									<TR>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label44" runat="server">被领用者帐号：</asp:label></TD>
										<TD style="HEIGHT: 17px"><FONT face="宋体">&nbsp;<asp:textbox id="TextBoxAcUin" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></FONT></TD>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label45" runat="server">被领用者内部帐号：</asp:label></TD>
										<TD style="HEIGHT: 17px"><FONT face="宋体">&nbsp;<asp:textbox id="TextBoxAcUid" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 17px" align="right"><asp:label id="Label46" runat="server">是否可以随即领用：</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcFlag" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label47" runat="server">同一用户领用次数：</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcNum" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 17px" align="right"><asp:label id="Label48" runat="server">开始领用日期：</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcSTime" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
										<TD style="HEIGHT: 17px" align="right"><asp:label id="Label49" runat="server">结束领用日期：</asp:label></TD>
										<TD style="HEIGHT: 17px">&nbsp;
											<asp:textbox id="TextBoxAcETime" runat="server" ReadOnly="True" BorderStyle="None"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT face="宋体"></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label20" runat="server">使用者内部帐号：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUserUid" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="WIDTH: 133px" align="right" width="133"><asp:label id="Label26" runat="server">使用日期：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUseTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 10px" align="right"><asp:label id="Label1" runat="server">赠送者帐号：</asp:label></TD>
										<TD style="HEIGHT: 10px">&nbsp;
											<asp:textbox id="TextBoxDonateId" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="HEIGHT: 10px" align="right"><asp:label id="Label5" runat="server">赠送者内部帐号：</asp:label></TD>
										<TD style="HEIGHT: 10px">&nbsp;
											<asp:textbox id="TextBoxDonateUid" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 22px" align="right"><asp:label id="Label3" runat="server">赠送次数：</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxDonateNum" runat="server" Width="75px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="HEIGHT: 22px" align="right"><asp:label id="Label27" runat="server">赠送日期：</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxDonateTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px; HEIGHT: 22px" align="right"><asp:label id="Label30" runat="server">业务状态：</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxState" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD style="HEIGHT: 22px" align="right"><asp:label id="Label31" runat="server">最后修改时间：</asp:label></TD>
										<TD style="HEIGHT: 22px">&nbsp;
											<asp:textbox id="TextBoxModifyTime" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<tr borderColor="#999999" bgColor="#999999">
										<td colSpan="4"><FONT face="宋体"></FONT></td>
									</tr>
									<TR>
										<TD style="WIDTH: 133px" align="right"><asp:label id="Label40" runat="server">用户IP：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUserIp" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD align="right"><asp:label id="Label18" runat="server">机构代码：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxSpid" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right"><asp:label id="Label37" runat="server">发行交易单：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxListid" runat="server" BorderStyle="None" ReadOnly="True" Width="220px"></asp:textbox></TD>
										<TD align="right"><asp:label id="Label38" runat="server">使用交易单：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxUseListid" runat="server" BorderStyle="None" ReadOnly="True" Width="220px"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" align="right"><asp:label id="Label41" runat="server">物理状态：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxListState" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
										<TD align="right"><asp:label id="Label42" runat="server">调账标记：</asp:label></TD>
										<TD>&nbsp;
											<asp:textbox id="TextBoxAdjustFlag" runat="server" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT style="BACKGROUND-COLOR: #ffffff" face="宋体"></FONT><FONT face="宋体"></FONT></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" vAlign="top" align="right"><asp:label id="Label6" runat="server">使用场所URL：</asp:label><BR>
										</TD>
										<TD colSpan="3">&nbsp;
											<asp:textbox id="TextBoxUrl" runat="server" Width="550px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR>
										<TD style="WIDTH: 133px" vAlign="top" align="right"><asp:label id="Label4" runat="server">备注：</asp:label><BR>
										</TD>
										<TD colSpan="3">&nbsp;
											<asp:textbox id="TextBoxMemo" runat="server" Width="550px" BorderStyle="None" ReadOnly="True"></asp:textbox></TD>
									</TR>
									<TR borderColor="#999999" bgColor="#999999">
										<TD colSpan="4"><FONT face="宋体"></FONT></TD>
									</TR>
								</TBODY>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD align="left" colSpan="2"><asp:Label id="Label43" runat="server" Font-Bold="True">当前使用者操作流水：</asp:Label>
							<asp:datagrid id="DataGrid1" runat="server" Width="100%" BorderStyle="None" ForeColor="Black"
								PageSize="20" AutoGenerateColumns="False" HorizontalAlign="Center" CellPadding="1" BackColor="White"
								BorderColor="#DEDFDE" BorderWidth="1px">
								<FooterStyle BackColor="#CCCC99"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" Wrap="False" ForeColor="White" BackColor="#CE5D5A"></SelectedItemStyle>
								<EditItemStyle Wrap="False"></EditItemStyle>
								<AlternatingItemStyle Wrap="False" BackColor="White"></AlternatingItemStyle>
								<ItemStyle Wrap="False" BackColor="#F7F7DE"></ItemStyle>
								<HeaderStyle Font-Bold="True" Wrap="False" HorizontalAlign="Center" ForeColor="White" BackColor="#6B696B"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FCreate_time" HeaderText="生成时间">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Fuser_id" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
										DataTextField="Fuser_id" HeaderText="使用者帐号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Facce_id" DataNavigateUrlFormatString="../BaseAccount/InfoCenter.aspx?id={0}"
										DataTextField="Facce_id" HeaderText="接受者帐号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:BoundColumn DataField="Frequest_type" HeaderText="请求类型">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Flistid" DataNavigateUrlFormatString="../TradeManage/TradeLogQuery.aspx?id={0}"
										DataTextField="Flistid" HeaderText="交易单号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:BoundColumn DataField="Ffact_fee" HeaderText="使用&lt;br&gt;金额">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Flist_state" HeaderText="流水&lt;br&gt;标记">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fold_state" HeaderText="原状态">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fnew_state" HeaderText="新状态">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fuser_ip" HeaderText="用户IP"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fmemo" HeaderText="备注"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Left" ForeColor="Black" BackColor="White" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
				</TBODY>
			</TABLE>
			</TR></TBODY></TABLE></TR></TBODY></TABLE></form>
	</BODY>
</HTML>
