<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryDKServicePage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryDKServicePage" %>
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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻����Բ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <SPAN>�̻��ţ�</SPAN>
						<asp:textbox id="tbx_spid" Runat="server"></asp:textbox>
                        <SPAN>ҵ�����ͣ�</SPAN>
						<asp:dropdownlist id="ddl_service_code" Runat="server"></asp:dropdownlist>
                        <SPAN>��ȿ��ƣ�</SPAN>
                        <asp:DropDownList id="ddlBank" runat="server">
										<asp:ListItem Value="1" Selected="True">��ǿ�</asp:ListItem>
										<asp:ListItem Value="2">���ÿ�</asp:ListItem>
					    </asp:DropDownList>
                        <%-- <span>��ѯʱ��Σ�</span>
                         <asp:TextBox ID="tbx_beginDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonBeginDate"
                            runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:ImageButton>
                         <span>����</span>
                         <asp:TextBox ID="tbx_endDate" runat="server"></asp:TextBox><asp:ImageButton ID="ButtonEndDate"
                            runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:ImageButton>--%>
                     </TD>
				</TR>
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
								<asp:BoundColumn Visible="False" DataField="Fservice_code" HeaderText="ҵ�����">
									<HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:ButtonColumn Text="��ѯ����" HeaderText="����" CommandName="detail"></asp:ButtonColumn>
								<asp:BoundColumn DataField="Fspid" HeaderText="�̻���">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FspName" HeaderText="�̻�����">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcodeid" HeaderText="ҵ�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcodename" HeaderText="ҵ������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FlstateName" HeaderText="��ͬ״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="FOnce_data" HeaderText="�����޶�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmonth_sum_data" HeaderText="ҵ�����޶�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmonth_use_data" HeaderText="ҵ�����ۼ�ʹ�ö��"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmonth_use_count" HeaderText="ҵ�����ۼƿۿ����"></asp:BoundColumn>
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
					<TD style="WIDTH: 229px; HEIGHT: 19px" width="229" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="����">�̻��ţ�</FONT></FONT></TD>
					<TD style="HEIGHT: 19px" width="225" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�̻�����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�̻�����:</FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;�̻������޶�:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�̻��������޶�:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƶ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�̻������ܴ�������:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƴ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�̻��������޶�:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƶ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�̻������ܴ�������:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƴ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�̻��������޶�:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƶ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�̻������ܴ�������:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƴ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�̻��������޶�:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƶ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�̻������ܴ�������:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƴ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;�̻��������޶�:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="Label13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƶ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label14" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;�̻������ܴ�������:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="Label15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻������ۼƴ���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ҵ����룺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻�ҵ����룺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;ҵ������:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֧�ֿ���</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֧��֧����ʽ</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���׷���:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��ͬ��Ч��־��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��ͬ��ʼʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�ͻ������̨֪ͨ�̻�URL:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c25" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��ͬ����ʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c26" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���۳ɹ���̨֪ͨ�̻�URL:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c27" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����޸�ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c28" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��¼��ͬ�޸���ˮ:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c29" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������ֹʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c30" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����޶�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c31" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp; </FONT>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����޶�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c33" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���մ�������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c34" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����޶�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���ܴ�������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����޶�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���´�������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label20" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����޶�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label22" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����޶�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="Label24" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#eeeeee" height="18" colspan="4"><FONT face="����">&nbsp;ҵ������������:</FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֧�ֵ�ҵ������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ҵ����տ���:</FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֧��ҵ�����̶���:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ҵ����������:</FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_0" runat="server" Text="�Ƿ�֧�ִ����˿�"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_0" runat="server" Text="�������ǩԼ�����֤��������"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_0" runat="server" Text="�����б�����֤Э���"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_0" runat="server" Text="�Ƿ������̻���վ����"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_1" runat="server" Text="�Ƿ�ͨ�˿����"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_1" runat="server" Text="�������ǩԼ�û����ж���������"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_1" runat="server" Text="�����б����û�����ȷ��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_1" runat="server" Text="�Ƿ������̻�ֱ���ӿڷ���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_2" runat="server" Text="�Ƿ񱣴�ͻ���չ����"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_2" runat="server" Text="�������ǩԼ�̻��ϱ�������"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_2" runat="server" Text="�������̻�ȷ�϶�����֤��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_2" runat="server" Text="�Ƿ�����ͻ����ŷ���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_3" runat="server" Text="�Ƿ񱣴�ͻ�����Э������"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_3" runat="server" Text="Э���ǩԼ������֤"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_3" runat="server" Text="˫���������۽���"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_3" runat="server" Text="�Ƿ������̻����ŷ���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_4" runat="server" Text="ҵ���Ƿ���Ҫ�ͻ��Ƹ�ͨ��¼"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_4" runat="server" Text="�����̻����û���ʶ���۹���"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_4" runat="server" Text="֧��֧���ɹ�֪ͨ"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_4" runat="server" Text="�Ƿ�����ͻ�IVR����"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_5" runat="server" Text="�Ƿ񷵻��̻��Ƹ�ͨ�˻�"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_5" runat="server" Text="����ǩԼ�̻��ϱ�������֤��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_5" runat="server" Text="֧��һ�����֧��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_5" runat="server" Text="�Ƿ������̻�IVR����"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_6" runat="server" Text="�Ƿ��˵�֧��ҵ��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Friskcrtl_mask_6" runat="server" Text="����ǩԼ�û����ж�����֤��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_6" runat="server" Text="֧�ֵ��ղ���"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_6" runat="server" Text="�Ƿ�����ͻ��ֻ�WAP/�ͻ��˷���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fsevice_mask_7" runat="server" Text="�Ƿ�֧�ֻ������"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_7" runat="server" Text="����֤֤��[�����ÿ�]"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_7" runat="server" Text="�Ƿ������̻��ֻ�WAP/�ͻ��˷���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_8" runat="server" Text="����֤֤��[���ÿ�]"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_8" runat="server" Text="�Ƿ�����ͻ���վ/СǮ������"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_9" runat="server" Text="��ֹ����"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_9" runat="server" Text="�Ƿ�����ͻ�ר���ն˷���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_10" runat="server" Text="�������ɹ�ʱ�Ƿ����ý���ʧ��"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_10" runat="server" Text="�Ƿ������̻�ר���ն˷���"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_11" runat="server" Text="�Ƿ���ýӿڲ�ѯ�Ƿ񲹵�"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_11" runat="server" Text="�Ƿ�������ҵר��ƽ̨����"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_12" runat="server" Text="����֤����Ԥ���ֻ�[�����ÿ�]"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fservice_channel_12" runat="server" Text="�Ƿ�����ˢ��֧����������"></asp:CheckBox></FONT></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:CheckBox id="cb_Fpayflow_mask_13" runat="server" Text="����֤����Ԥ���ֻ�[���ÿ�]"></asp:CheckBox></FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;</FONT></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
