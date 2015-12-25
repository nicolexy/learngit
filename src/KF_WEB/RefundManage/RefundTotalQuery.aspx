<%@ Page language="c#" Codebehind="RefundTotalQuery.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundTotalQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundTotalQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">					
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
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:panel id="SearchPanel" Visible="true" Runat="server">
				<TABLE style="margin-top:10px;margin-left:10px" id="Table1" border="1"
					cellSpacing="1" cellPadding="1" width="85%" runat="server">
					<TR>
						<TD bgColor="#e4e5f7" colSpan="3"><FONT color="red" face="����"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;�˵��������ݲ�ѯ</FONT> <FONT color="#ff0000" face="����">�������׵�������ж�����ֻ�ܲ�ѯ2������ݡ����˿��ѯ����ѡ��ʱ�䣬֧�ֲ�2���ڵĵ���</FONT></FONT></TD>
						<TD bgColor="#e4e5f7" align="right"><FONT face="����">����Ա����: <SPAN class="style3">
									<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label2" runat="server">������ʼ����</asp:label></TD>
						<TD><FONT face="����">
								<asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
								</FONT></TD>
						<TD align="right">
							<asp:label id="Label3" runat="server">���ܽ�������</asp:label></TD>
						<TD>
							<asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
							</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 30px" align="right">
							<asp:label id="Label5" runat="server">������Դ</asp:label></TD>
						<TD style="HEIGHT: 30px">
							<asp:dropdownlist id="ddlrefund_type" runat="server" Width="152px">
								<asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
								<asp:ListItem Value="1">�̻��˵�</asp:ListItem>
								<asp:ListItem Value="2">���ʽ���˵�</asp:ListItem>
								<asp:ListItem Value="3">�˹�¼���˵�</asp:ListItem>
								<asp:ListItem Value="4">�����쳣�˵�</asp:ListItem>
								<asp:ListItem Value="5">�⸶�˵�</asp:ListItem>
								<asp:ListItem Value="9">��ֵ���˿�</asp:ListItem>
								<asp:ListItem Value="11">�����˵�</asp:ListItem>
								<asp:ListItem Value="12">�����˿��쳣��</asp:ListItem>
							</asp:dropdownlist></TD>
						<TD style="HEIGHT: 30px" align="right">
							<asp:label id="Label6" runat="server">�˿�����</asp:label></TD>
						<TD style="HEIGHT: 30px">
							<asp:dropdownlist id="ddlrefund_bank" runat="server" Width="152px"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">�˿�;��</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlrefund_path" runat="server" Width="152px">
								<asp:ListItem Value="99">��������</asp:ListItem>
								<asp:ListItem Value="1">�����˵�</asp:ListItem>
								<asp:ListItem Value="2">�ӿ��˵�</asp:ListItem>
								<asp:ListItem Value="3">�˹���Ȩ</asp:ListItem>
								<asp:ListItem Value="4">�����</asp:ListItem>
								<asp:ListItem Value="5">ת�����</asp:ListItem>
								<asp:ListItem Value="6">�����˿�</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">�˿�״̬</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlrefund_state" runat="server" Width="152px">
								<asp:ListItem Value="99">����״̬</asp:ListItem>
								<asp:ListItem Value="0">��ʼ״̬</asp:ListItem>
								<asp:ListItem Value="1">�˵�������</asp:ListItem>
								<asp:ListItem Value="2">�˵��ɹ�</asp:ListItem>
								<asp:ListItem Value="3">�˵�ʧ��</asp:ListItem>
								<asp:ListItem Value="5">�ֹ��˿���</asp:ListItem>
								<asp:ListItem Value="6">�����ֹ��˿�</asp:ListItem>
								<asp:ListItem Value="7">����ת�����</asp:ListItem>
								<asp:ListItem Value="4">�˵�״̬δ��</asp:ListItem>
								<asp:ListItem Value="8">���쳣������</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label4" runat="server">�̻���</asp:label></TD>
						<TD>
							<asp:textbox id="tbSPID" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label7" runat="server">�ص�״̬</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlreturn_state" runat="server" Width="152px">
								<asp:ListItem Value="99">����״̬</asp:ListItem>
								<asp:ListItem Value="1">�ص�ǰ</asp:ListItem>
								<asp:ListItem Value="2">�ص���</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label10" runat="server">���׵�ID</asp:label></TD>
						<TD>
							<asp:textbox id="tbRefundID" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label11" runat="server">���ж�����</asp:label></TD>
						<TD>
							<asp:textbox id="tbBank_list" runat="server" Width="150px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label12" runat="server">�˿��</asp:Label></TD>
						<TD>
							<asp:textbox style="Z-INDEX: 0" id="txFoldid" runat="server" Width="200px"></asp:textbox></TD>
						<TD></TD>
						<TD>
							<asp:Button id="btnQuery" runat="server" Text="��ѯ��¼"></asp:Button>
					    </TD>
					</TR>
				</TABLE>
			</asp:panel>
			<asp:panel id="Panel1" Visible="true" Runat="server">
				<TABLE style=" WIDTH: 85%;  margin-left:10px" id="Table2"
					border="1" cellSpacing="1" cellPadding="1" runat="server">
					<TR>
						<TD vAlign="top">
							<asp:datagrid id="DataGrid1" runat="server" Width="100%" PageSize="50" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="FRefundID" HeaderText="����ID"></asp:BoundColumn>
									<asp:BoundColumn DataField="FPaylistid" HeaderText="�˿"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeName" HeaderText="�˿�����"></asp:BoundColumn>
									<asp:BoundColumn DataField="FPay_time" HeaderText="֧������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_listid" HeaderText="���ж�����"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnamtName" HeaderText="�˵����" DataFormatString="{0:N}"></asp:BoundColumn>
									<asp:BoundColumn DataField="FamtName" HeaderText="�������" DataFormatString="{0:N}"></asp:BoundColumn>
									<asp:BoundColumn DataField="FstateName" HeaderText="�˵�״̬"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnStateName" HeaderText="�ص�״̬"></asp:BoundColumn>
									<asp:BoundColumn DataField="FrefundPathName" HeaderText="�˵�;��"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="��ϸ">
										<ItemTemplate>
											<a href='<%# String.Format("RefundTotalQuery_Detail.aspx?refundId={0}&batchid={1}", DataBinder.Eval(Container, "DataItem.FRefundId").ToString(), DataBinder.Eval(Container, "DataItem.Fbatchid").ToString()
										) %>' >��ϸ </a>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="fbatchid" HeaderText="fbatchid"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR height="25">
						<TD>
							<webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
								PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
								SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD align="center"><FONT face="����"></FONT><INPUT style="WIDTH: 64px; HEIGHT: 22px" onclick="history.go(-1)" value="����" type="button">
						</TD>
					</TR>
				</TABLE>
			</asp:panel>
		</form>
	</body>
</HTML>
