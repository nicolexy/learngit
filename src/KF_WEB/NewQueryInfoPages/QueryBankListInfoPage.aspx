<%@ Page language="c#" Codebehind="QueryBankListInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryBankListInfoPage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryDKInfoPage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">
					function enterPress(e)
					{
						if(window.event)
						{
							if(e.keyCode == 13)
								document.getElementById('btn_serach').click();
						}
					}
					
					function onLoadFun()
					{
						document.getElementById('btn_serach').focus();
					}
		</script>
	</HEAD>
	<body onload="onLoadFun()" MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="1300" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�������ν��ײ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <SPAN>�Ƹ�ͨ���Σ�</SPAN>
						<asp:textbox id="tbx_batchid" Runat="server"></asp:textbox>��<SPAN>�������Σ�</SPAN>
						<asp:textbox id="tbx_batchid_forbank" Runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT><SPAN>����������</SPAN>
						<asp:textbox id="tbx_bank_type" Runat="server"></asp:textbox><SPAN>����״̬��</SPAN>
						<asp:dropdownlist id="ddl_status" Runat="server">
							<asp:ListItem Value="0" Selected="True">ȫ��״̬</asp:ListItem>
							<asp:ListItem Value="1">��ʼ״̬</asp:ListItem>
							<asp:ListItem Value="2">�������</asp:ListItem>
							<asp:ListItem Value="3">Ԥ֧�����</asp:ListItem>
							<asp:ListItem Value="4">���ַ�����</asp:ListItem>
							<asp:ListItem Value="5">�������</asp:ListItem>
							<asp:ListItem Value="6">�����ȡ��</asp:ListItem>
							<asp:ListItem Value="7">ִ�����</asp:ListItem>
							<asp:ListItem Value="8">ȫ���ɹ�</asp:ListItem>
							<asp:ListItem Value="9">ȫ��ʧ��</asp:ListItem>
						</asp:dropdownlist><SPAN>��ѯʱ��Σ�</SPAN>

                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
                        ��
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				<TR>
					<TD align="center" colSpan="5"><asp:button id="btn_serach" Width="80px" Runat="server" Text="��ѯ" onclick="btn_serach_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE cellSpacing="0" cellPadding="0" width="1300" border="0">
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
								<asp:BoundColumn Visible="False" DataField="Fbank_batch_id" HeaderText="���������ڲ����κ�"></asp:BoundColumn>
								<asp:ButtonColumn Text="��ѯ����" HeaderText="����" CommandName="detail"></asp:ButtonColumn>
								<asp:BoundColumn DataField="Fbatchid" HeaderText="�̻����κ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbatchid_forbank" HeaderText="���������κ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstatusName" HeaderText="����״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftotal_feeName" HeaderText="�ܽ��"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="totalBatchUrl" DataTextField="Ftotal_count"
									HeaderText="�ܱ���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fsucpay_feeName" HeaderText="�ɹ����"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="successBatchUrl" DataTextField="Fsucpay_count"
									HeaderText="�ɹ�����"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="" HeaderText="ʧ�ܽ��"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="failedBatchUrl" DataTextField=""
									HeaderText="ʧ�ܱ���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="" HeaderText="�����н��"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="handlingBatchUrl" DataTextField=""
									HeaderText="�����б���"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fret_code" HeaderText="���д�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fresult_info" HeaderText="����ԭ��"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<DIV>
				<P><LABEL>���׳ɹ�������</LABEL>
					<asp:label id="lb_successNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>���׳ɹ��ܶ</LABEL>
					<asp:label id="lb_successAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>����ʧ�ܱ�����</LABEL>
					<asp:label id="lb_failNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp; <LABEL>
						<LABEL>����ʧ���ܶ</LABEL>
						<asp:label id="lb_failAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
						<LABEL>�����б�����</LABEL>
						<asp:label id="lb_handlingNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp; 
						�������ܶ</LABEL>
					<asp:label id="lb_handlingAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
				</P>
			</DIV>
			<TABLE cellSpacing="1" cellPadding="0" width="1300" bgColor="black" border="0">
				<TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;��ϸ��Ϣ�б�</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" width="229" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="����">�������ͣ�</FONT></FONT></TD>
					<TD style="HEIGHT: 19px" width="225" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;���������κ�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�ӿ�����:</FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;���������ڲ����κ�:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Ƹ�ͨ�ڲ����κ�:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�ܽ��:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ѷ������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ܱ���:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ѷ�������</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�ɹ����:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ʵ�ʷ��������ܽ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�ɹ�����:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ʵ�ʷ��������ܱ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ҵ��״̬:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Ƹ�ͨ��̨������ע</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���һ������ʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����޸�ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Ԥ�ڽ��ʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���д�����Ϣ��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c22" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ͻ�IP:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����޸�IP��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
