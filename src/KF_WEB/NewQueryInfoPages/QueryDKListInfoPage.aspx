<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryDKListInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryDKListInfoPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryDKListInfoPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="1300">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;������Ϣ��ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<td colspan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <span>�̻��ţ�</span><asp:TextBox Runat="server" ID="tbx_spid"></asp:TextBox>
						
						<asp:Label id="Label2" runat="server">����״̬��</asp:Label>
						<asp:DropDownList id="ddl_state" runat="server">
							<asp:ListItem Value="0" Selected="True">ȫ��״̬</asp:ListItem>
							<asp:ListItem Value="1">���γ�ʼ��</asp:ListItem>
							<asp:ListItem Value="2">���μ�¼ʧ��</asp:ListItem>
							<asp:ListItem Value="3">���δ�����</asp:ListItem>
							<asp:ListItem Value="4">����ȡ��</asp:ListItem>
							<asp:ListItem Value="5">�������ʧ��</asp:ListItem>
							<asp:ListItem Value="6">������֤��</asp:ListItem>
							<asp:ListItem Value="7">���δ�����</asp:ListItem>
							<asp:ListItem Value="8">���δ�����</asp:ListItem>
							<asp:ListItem Value="9">������ȫ���ɹ�</asp:ListItem>
							<asp:ListItem Value="10">���������ֳɹ�</asp:ListItem>
							<asp:ListItem Value="11">������ȫ��ʧ��</asp:ListItem>
							<asp:ListItem Value="12">��̨Ԥ������</asp:ListItem>
							<asp:ListItem Value="13">Ԥ�������</asp:ListItem>
						</asp:DropDownList>
						<span>��ѯʱ��Σ�</span>
					    <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                        ��
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                    </td>
				</tr>
				<tr>
					<td colSpan="5" align="left">
                       &nbsp;&nbsp; <span>�̻����κţ�</span><asp:TextBox Runat="server" ID="tbx_spBatchID"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;<span>�Ƹ�ͨ���κţ�</span><asp:TextBox Runat="server" ID="tbx_batchid"></asp:TextBox>
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:button id="btn_serach" Width="80px" Text="��ѯ" Runat="server" onclick="btn_serach_Click"></asp:button></td>
				</tr>
			</TABLE>
			<table border="0" cellSpacing="0" cellPadding="0" width="1300">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1300px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fsp_batchid" HeaderText="�̻����κ�"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="spidUrl" DataTextField="Fspid" HeaderText="�̻���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fservice_codeName" HeaderText="ҵ������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftotal_paynumName" HeaderText="�����ܽ��"></asp:BoundColumn>								
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="totalBatchUrl" DataTextField="Ftotal_count"
									HeaderText="�����ܱ���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fsucpay_amountName" HeaderText="�ɹ����"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="successBatchUrl" DataTextField="Fsucpay_count"
									HeaderText="�ɹ�����"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Ffailpay_amountName" HeaderText="ʧ�ܽ��"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="failedBatchUrl" DataTextField="Ffailpay_count"
									HeaderText="ʧ�ܱ���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="FHandling_amountName" HeaderText="�����н��"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="handlingBatchUrl" DataTextField="FHandling_Count"
									HeaderText="�����б���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="����״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fresult_info" HeaderText="ʧ��ԭ��"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<div>
				<p>
					<label>���׳ɹ��ܱ�����</label><asp:Label Runat="server" ID="lb_successNum">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>���׳ɹ��ܶ</label><asp:Label Runat="server" ID="lb_successAllMoney">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>����ʧ���ܱ�����</label><asp:Label Runat="server" ID="lb_failNum">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>����ʧ���ܶ</label><asp:Label Runat="server" ID="lb_failAllMoney">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>�����е��ܱ�����</label><asp:Label Runat="server" ID="lb_handlingNum">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
					<label>�����е��ܶ</label><asp:Label Runat="server" ID="lb_handlingMoney">0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_outExcel" Width="80px" runat="server" Text="�������" OnClick="btn_outExcel_Click" Visible="false"></asp:Button>
				</p>
			</div>
			<table border="0" cellSpacing="1" cellPadding="0" width="1200" bgColor="black" style="DISPLAY:none">
				<tr>
					<td bgColor="#eeeeee" height="18" colSpan="4"><span>&nbsp;��ϸ��Ϣ�б�</span></td>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19" width="229"><font face="����">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="����">�������˻���ӦID��</FONT></font></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19" width="225"><font face="����">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="����">&nbsp;�û���CFT�ڲ�ID:</font></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="����">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="����">&nbsp;Ͷ������ʵ����:</font></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="����">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;������Ч��־:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="����">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;����״̬:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����޸�ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="Label1" runat="server">&nbsp;</asp:label></FONT></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
