<%@ Page language="c#" Codebehind="QueryDKcontractPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryDKcontractPage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryDKInfoPage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_beginDate.value=returnValue;
					}
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_endDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.tbx_endDate.value=returnValue;
					}
					
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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻�Э���ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 28px" colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<SPAN>�̻��ţ�</SPAN>
						<asp:textbox id="tbx_spid" Runat="server"></asp:textbox>�̻�Э���<SPAN>��</SPAN>
						<asp:textbox id="tbx_mer_cnr" Runat="server"></asp:textbox>�̻����κ�<SPAN>��</SPAN>
						<asp:textbox id="tbx_sp_batchid" Runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 28px" colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<SPAN>�ֻ��ţ�</SPAN>
						<asp:textbox id="tbx_mobile" Runat="server"></asp:textbox><SPAN>�����˺ţ�</SPAN>
						<asp:textbox id="tbx_bankacc_no" Runat="server"></asp:textbox><SPAN>������</SPAN>
						<asp:textbox id="tbx_uname" Runat="server"></asp:textbox><SPAN>֤�����룺</SPAN>
						<asp:textbox id="tbx_credit_id" Runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT><SPAN>�û���ʶ��</SPAN>
						<asp:textbox id="tbx_mer_aid" Runat="server"></asp:textbox><SPAN>Э��״̬��</SPAN>
						<asp:dropdownlist id="ddl_status" Runat="server">
							<asp:ListItem Value="0" Selected="True">ȫ��״̬</asp:ListItem>
							<asp:ListItem Value="1">�ɹ�</asp:ListItem>
							<asp:ListItem Value="2">ʧ��</asp:ListItem>
							<asp:ListItem Value="3">������</asp:ListItem>
						</asp:dropdownlist><SPAN>����ʱ�䣺</SPAN>
						<asp:textbox id="tbx_beginDate" Runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton><SPAN>����</SPAN>
						<asp:textbox id="tbx_endDate" Runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				<TR>
					<TD align="center" colSpan="5"><asp:button id="btn_serach" Width="80px" Runat="server" Text="��ѯ" onclick="btn_serach_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE cellSpacing="0" cellPadding="0" width="1300" border="0">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1300px" ItemStyle-HorizontalAlign="Center"
							HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
							GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="Fcep_cnr" HeaderText="Э����"></asp:BoundColumn>
								<asp:ButtonColumn Text="��ѯ����" HeaderText="����" CommandName="detail"></asp:ButtonColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="�̻���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmer_cnr" HeaderText="�̻�Э���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fstime" HeaderText="��ʼʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fetime" HeaderText="��ֹʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstatusName" HeaderText="Э��״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbankacc_attrName" HeaderText="�˻�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Funame" HeaderText="����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_typeName" HeaderText="����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbankacc_typeName" HeaderText="����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbankacc_tail" HeaderText="�����˺�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmemo" HeaderText="��Ҫ˵��"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
							ShowInputBox="always" CssClass="mypager" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]" AlwaysShow="True"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<TABLE cellSpacing="1" cellPadding="0" width="1300" bgColor="black" border="0">
				<TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;��ϸ��Ϣ�б�</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 17px" width="229" bgColor="#eeeeee" height="17"><FONT face="����">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="����">�̻��ţ�</FONT></FONT></TD>
					<TD style="HEIGHT: 17px" width="225" bgColor="#ffffff" height="17"><FONT face="����">&nbsp;
							<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 17px" bgColor="#eeeeee" height="17"><FONT face="����">&nbsp;Э����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 17px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�̻���Э���:</FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;����Э���:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�̻����û���ʶ:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�˻�����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;����:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ͻ�����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�����˺�:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�������ڵ���:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�������ڳ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ͨѶ��ַ:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������ţ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̶��绰:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ֻ���</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Э��״̬:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����״̬��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c22" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Զ��ۿ���:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Զ��ۿ���֣�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;������������:</FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="Label1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;����������:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;֧����ʽ:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ύ����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;Э����֤����:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Э����֤��ʽ:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ύ��ʽ:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���ѷ�ʽ:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;���׷���:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;Ĭ��ҵ�����:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ǩԼ��ʽ:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�޸�ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label14" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Э����֤ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��ͬ��ʼʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Э���Ҫ˵��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��ͬ��ֹʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Э����ϸ˵��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Ƿ�Ƹ�ͨ�˻���</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label20" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Ƹ�ͨ�˻�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�Ƹ�ͨ�ڲ�ID��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label22" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ͻ�IP��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label24" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������Ϣ:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label25" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�޸�IP��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label26" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����Э���ڲ����κ�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:HyperLink id="Label27" runat="server"></asp:HyperLink></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;Э��¼���ļ�·����</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label28" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����Э���̻����κ�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:HyperLink id="Label29" runat="server"></asp:HyperLink></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���Ӱ棨�ļ���Э��·����</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label30" runat="server"></asp:label></FONT></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
