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
						<asp:label id="Label1" runat="server" CssClass="title_info">����ʵ����ϸ��Ϣ</asp:label></TD>
				</TR>
			</TABLE>
			<TABLE width="100%" border="0" align="center" cellPadding="2" cellSpacing="5" id="Table1">
				<TR>
					<TD class="detailitem" align="right" width="100">ʵ����</TD>
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
					<TD class="detailitem" align="right">��ͬ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeContract" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�̻�����</TD>
					<TD>&nbsp;
						<asp:Label id="lblSpid" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�̻��˺�</TD>
					<TD>&nbsp;
						<asp:Label id="LabelUid" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">������</TD>
					<TD>&nbsp;
						<asp:Label id="LabelChannelNo" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">��Ʒ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelProductType" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">ʵ�ձ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelDiscount" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">ƽ̨��</TD>
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
					<TD class="detailitem" align="right">�շ���Ŀ</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeItem" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�����Ŀ</TD>
					<TD>&nbsp;
						<asp:Label id="LabelItem" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�����ʺ�</TD>
					<TD>&nbsp;
						<asp:Label id="LabelInUid" runat="server"></asp:Label></TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">�շѱ�׼</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeStandard" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�Ʒ��ܼ���С���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMinAmount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�Ʒ��ܼ������</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMaxAmount" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">�ֵ���׼</TD>
					<TD>&nbsp;
						<asp:Label id="LabelTag" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">���ֵ�ֵ����</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMinTag" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">���ֵ�ֵ����</TD>
					<TD>&nbsp;
						<asp:Label id="LabelMaxTag" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">���۷�ʽ</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPriceFormat" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">���ʻ�׼</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCalUnit" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�ƷѶ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFix" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">�Ʒѻ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCurType" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�Ʒѱ��ʷ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPerMolecule" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�Ʒѱ��ʷ�ĸ</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPerDenominator" runat="server"></asp:Label></TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">�Ʒѱ���</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�Ʒѽ��</TD>
					<TD>&nbsp;
						<asp:Label id="LabelAmount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">������</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCalAmount" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">Ӧ�շ��ý��</TD>
					<TD>&nbsp;
						<asp:Label id="LabelDueAmount" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">ʹ�ñ�־</TD>
					<TD>&nbsp;
						<asp:Label id="LabelUseTag" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�շ�ʵ����</TD>
					<TD>&nbsp;
						<asp:Label id="LabelFeeNo" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="detailitem" align="right">ʵ������ʱ��</TD>
					<TD>&nbsp;
						<asp:Label id="LabelCreateTime" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�ϴν�������</TD>
					<TD>&nbsp;
						<asp:Label id="LabelPreDate" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">�´ν�������</TD>
					<TD>&nbsp;
						<asp:Label id="LabelNextDate" runat="server"></asp:Label></TD>
				</TR>
				<tr bgcolor="#cccccc" height="1">
					<td colspan="6"></td>
				</tr>
				<TR>
					<TD class="detailitem" align="right">��¼��������</TD>
					<TD>&nbsp;
						<asp:Label id="LabelModify_time" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">ά������Ա��</TD>
					<TD>&nbsp;
						<asp:Label id="LabelUserId" runat="server"></asp:Label></TD>
					<TD class="detailitem" align="right">��¼״̬</TD>
					<TD>&nbsp;
						<asp:Label id="LabelRecordStatus" runat="server"></asp:Label></TD>
				</TR>
				<TR height="50">
					<TD align="center" colSpan="6"><a href="#" onclick="javascript:window.parent.close();">�رմ���</a>&nbsp;&nbsp;|&nbsp;&nbsp;
						<asp:HyperLink id="LinkRefresh" runat="server">ˢ��</asp:HyperLink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
