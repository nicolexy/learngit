<%@ Page language="c#" Codebehind="QueryDKInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryDKInfoPage" %>
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
					
			function select_deselectAll (chkVal, idVal) 
			{
				var frm = document.forms[0];
				for (i=0; i<frm.length; i++) 
				{
					if (idVal.indexOf ('CheckAll') != -1)
					{
						if(frm.elements[i].id.indexOf('CheckBox') != -1)
						{
							if(chkVal == true) 
							{
								frm.elements[i].checked = true;
							} 
							else 
							{
								frm.elements[i].checked = false;
							}
						}
					} 
					else if (idVal.indexOf('DeleteThis') != -1) 
					{
						if(frm.elements[i].checked == false) 
						{
							frm.elements[1].checked = false;
						}
					}
				}
			}
		</script>
	</HEAD>
	<body onload="onLoadFun()" MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="1300" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;������Ϣ��ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <SPAN>���п��ţ�</SPAN>
						<asp:textbox id="tbx_bankID" Runat="server"></asp:textbox><SPAN>�û�����</SPAN>
						<asp:textbox id="tbx_userName" Runat="server"></asp:textbox><SPAN>��ѯʱ��Σ�</SPAN>
						<asp:textbox id="tbx_beginDate" Runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton><SPAN>����</SPAN>
						<asp:textbox id="tbx_endDate" Runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <SPAN>�̻��ţ�</SPAN>
						<asp:textbox id="tbx_spid" Runat="server"></asp:textbox><SPAN>�̼Ҷ����ţ�</SPAN>
						<asp:textbox id="tbx_spListID" Runat="server"></asp:textbox><SPAN>�̻����κţ�</SPAN>
						<asp:textbox id="tbx_spBatchID" Runat="server"></asp:textbox>
                        <SPAN>���۽��׵��ţ�</SPAN>
						<asp:textbox id="tbx_cep_id" Runat="server"></asp:textbox>
                        <SPAN>����״̬��</SPAN>
						<asp:dropdownlist id="ddl_state" Runat="server">
							<asp:ListItem Selected="True" Value="0">ȫ��</asp:ListItem>
							<asp:ListItem Value="1">�ɹ�</asp:ListItem>
							<asp:ListItem Value="2">ʧ��</asp:ListItem>
							<asp:ListItem Value="3">������</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <SPAN>�Ƹ�ͨ�����ţ�</SPAN>
						<asp:textbox id="txb_transaction_id" Runat="server"></asp:textbox><SPAN>����������</SPAN>
                        <asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList>
						<%--<asp:textbox id="txb_bank_type" Runat="server"></asp:textbox>--%>
                        <SPAN>�̻�ҵ����룺</SPAN>
						<asp:dropdownlist id="ddl_service_code" Runat="server"></asp:dropdownlist><SPAN>��Ӧ�룺</SPAN>
						<asp:textbox id="txb_explain" Runat="server"></asp:textbox></TD>
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
								<asp:BoundColumn Visible="False" DataField="Fcep_id" HeaderText="���׵���">
									<HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="ѡ��">
									<HeaderTemplate>
										<INPUT id="CheckAll" onclick="return select_deselectAll(this.checked,this.id)" type="checkbox"><LABEL>ѡ��</LABEL>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="CheckBox1" runat="server" Text="ѡ��"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn Text="��ѯ����" HeaderText="����" CommandName="detail"></asp:ButtonColumn>
								<asp:BoundColumn DataField="Fcoding" HeaderText="�̼Ҷ�����">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ftransaction_id" HeaderText="�Ƹ�ͨ������">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="�ۿ�ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="FpaynumName" HeaderText="�������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fservice_codeName" HeaderText="ҵ������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="����״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftrade_stateName" HeaderText="���׵�״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_typeName" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbankacc_no" HeaderText="���п���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Funame" HeaderText="�û���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FailedReason" HeaderText="ʧ��ԭ��"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Fmobile" HeaderText="�����ֻ���"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<DIV>
				<P><asp:button id="btn_batchadjust" Width="80px" Runat="server" Text="������������״̬" onclick="btn_batchadjust_Click"></asp:button></P>
				<P><LABEL>���׳ɹ�������</LABEL>
					<asp:label id="lb_successNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>���׳ɹ��ܶ</LABEL>
					<asp:label id="lb_successAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>����ʧ�ܱ�����</LABEL>
					<asp:label id="lb_failNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp; <LABEL>
						����ʧ���ܶ</LABEL>
					<asp:label id="lb_failAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>�����б�����</LABEL>
					<asp:label id="lb_handlingNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp; 
					�������ܶ</LABEL>
					<asp:label id="lb_handlingAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Button ID="btn_outExcel" Width="80px" runat="server" Text="�������" OnClick="btn_outExcel_Click" Visible="false"></asp:Button>
				</P>
			</DIV>
			<TABLE cellSpacing="1" cellPadding="0" width="1300" bgColor="black" border="0">
				<TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;��ϸ��Ϣ�б�</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" width="229" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="����">�̼Ҷ����ţ�</FONT></FONT></TD>
					<TD style="HEIGHT: 19px" width="225" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�̻���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="����">&nbsp;�Ƹ�ͨ������:</FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="����">&nbsp;
							<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;�����ж�����:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;����״̬:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���п���:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;���׵�״̬:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������Ԫ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;
							<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;��������ʱ��:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;
							<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ʵ�ʽ���ʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ʽ����ҵ��/�ӿڣ�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�������ͣ�����/���ʣ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�������κ�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ҵ�����ͣ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���ױ�ע:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
                    <TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ƾ֤��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c37" runat="server"></asp:label></FONT></TD>
				</TR>
				<tr>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ʧ��ԭ����ϸ��</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
                    <TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����ֻ��ţ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c38" runat="server"></asp:label></FONT></TD>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�˻�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c22" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���۽��׵���:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���������ӿ����κţ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�̻�ҵ�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c25" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�������ͣ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c26" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����˻�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c27" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ҵ�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c28" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c29" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֤����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c30" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c31" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��������:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c32" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֧����ʽ:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c33" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ʵ��ȥ����֧�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c34" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;ͬ�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c35" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���һ��ȥ����ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;
							<asp:label id="lb_c36" runat="server"></asp:label></FONT></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
