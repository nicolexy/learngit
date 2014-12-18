<%@ Page language="c#" Codebehind="SettShow.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.SettShow" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SettShow</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body id="bodyid" runat="server">
		<form id="Form1" method="post" runat="server">
			<TABLE height="50" cellSpacing="0" cellPadding="5" width="100%" align="center" border="0">
				<TR>
					<TD vAlign="middle" align="center">&nbsp;&nbsp;
						<asp:label id="Label1" runat="server" CssClass="title_info">结算实例详细信息</asp:label></TD>
				</TR>
			</TABLE>
			<TABLE width="100%" border="0" align="center" cellPadding="2" cellSpacing="5" id="Table1">
				<TR>
					<TD class="detailitem" align="right" width="100">实例号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelNo" runat="server"></asp:Label></TD>
					<TD align="right" width="100">&nbsp;</TD>
					<TD>&nbsp;</TD>
					<TD align="right" width="100">&nbsp;</TD>
					<TD>&nbsp;</TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">合同编号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeContract" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">商户名称</TD>
					<TD>&nbsp;
						<asp:Label id="lblSpid" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">商户账号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelUid" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">渠道号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelChannelNo" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">产品类别</TD>
					<TD>&nbsp;
						<asp:Label id="LabelProductType" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">实收比例</TD>
					<TD>&nbsp;
						<asp:Label id="LabelDiscount" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">平台商</TD>
					<TD>&nbsp;
						<asp:Label id="LabelAgentSPID" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right"></TD>
					<TD>&nbsp;</TD>
					<TD class="detailitem" align="right"></TD>
					<TD>&nbsp;</TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">收费项目</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeItem" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">结算科目</TD>
					<TD>&nbsp;
						<asp:Label id="LabelItem" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">收入帐号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelInUid" runat="server"></asp:Label></TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">收费标准</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeStandard" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计费总计最小金额</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMinAmount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计费总计最大金额</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMaxAmount" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">分档标准</TD>
					<TD>&nbsp;
						<asp:Label id="LabelTag" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">金额分档值下限</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMinTag" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">金额分档值上限</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMaxTag" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">定价方式</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPriceFormat" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">费率基准</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCalUnit" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计费定额</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFix" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">计费货币</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCurType" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计费比率分子</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPerMolecule" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计费比率分母</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPerDenominator" runat="server"></asp:Label></TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">计费笔数</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计费金额</TD>
					<TD>&nbsp;
						<asp:Label id="LabelAmount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">计算金额</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCalAmount" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">应收费用金额</TD>
					<TD>&nbsp;
						<asp:Label id="LabelDueAmount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">使用标志</TD>
					<TD>&nbsp;
						<asp:Label id="LabelUseTag" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">收费实例号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeNo" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">实例产生时间</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCreateTime" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">上次结算日期</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPreDate" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">下次结算日期</TD>
					<TD>&nbsp;
						<asp:Label id="LabelNextDate" runat="server"></asp:Label></TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">记录更新日期</TD>
					<TD>&nbsp;
						<asp:Label id="LabelModify_time" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">维护操作员号</TD>
					<TD>&nbsp;
						<asp:Label id="LabelUserId" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">记录状态</TD>
					<TD>&nbsp;
						<asp:Label id="LabelRecordStatus" runat="server"></asp:Label></TD>
				</TR>
				<TR height="50">
					<TD align="center" colSpan="6"><a href="#" onclick="javascript:window.parent.close();">关闭窗口</a>&nbsp;&nbsp;|&nbsp;&nbsp;
						<asp:HyperLink id="LinkRefresh" runat="server">刷新</asp:HyperLink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
