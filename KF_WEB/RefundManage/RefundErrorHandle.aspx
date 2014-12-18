<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="RefundErrorHandle.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundErrorHandle" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundErrorHandle</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
					}
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1"
				cellPadding="1" width="85%" border="1" runat="server">
				<TR>
					<TD bgColor="#e4e5f7"><FONT face="����" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;�˵��쳣���ݲ�ѯ</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="����">����Ա����: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="1" runat="server">
							<TR>
								<TD align="right"><asp:label id="Label2" runat="server">��ʧ�ܿ�ʼ����</asp:label></TD>
								<TD><FONT face="����"><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></FONT></TD>
								<TD align="right"><asp:label id="Label3" runat="server">��ʧ�ܽ�������</asp:label></TD>
								<TD><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 30px" align="right"><asp:label id="Label9" runat="server">����״̬</asp:label></TD>
								<TD style="HEIGHT: 30px"><asp:dropdownlist id="ddlhandle_type" runat="server" Width="152px" AutoPostBack="True">
										<asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
										<asp:ListItem Value="1">������</asp:ListItem>
										<asp:ListItem Value="2">������</asp:ListItem>
										<asp:ListItem Value="4">�����״̬</asp:ListItem>
										<asp:ListItem Value="3">�������</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD style="HEIGHT: 30px" align="right"><asp:label id="Label7" runat="server">�쳣ԭ��</asp:label></TD>
								<TD style="HEIGHT: 30px"><asp:dropdownlist id="ddlerror_type" runat="server" Width="152px">
										<asp:ListItem Value="99">����״̬</asp:ListItem>
										<asp:ListItem Value="1">�����˵�ʧ��</asp:ListItem>
										<asp:ListItem Value="2">�ֹ���ʧ��</asp:ListItem>
										<asp:ListItem Value="3">�˿�У����ʧ��</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="Label5" runat="server">������Դ</asp:label></TD>
								<TD><asp:dropdownlist id="ddlrefund_type" runat="server" Width="152px">
										<asp:ListItem Value="99" Selected="True">��������</asp:ListItem>
										<asp:ListItem Value="1">�̻��˵�</asp:ListItem>
										<asp:ListItem Value="2">���ʽ���˵�</asp:ListItem>
										<asp:ListItem Value="3">�˹�¼���˵�</asp:ListItem>
										<asp:ListItem Value="4">�����쳣�˵�</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD align="right"><asp:label id="Label6" runat="server">�˿�����</asp:label></TD>
								<TD><asp:dropdownlist id="ddlrefund_bank" runat="server" Width="152px"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:dropdownlist id="ddlorder_type" runat="server" Width="105px">
										<asp:ListItem Value="1">�����ж�����</asp:ListItem>
										<asp:ListItem Value="2">��ֵ����</asp:ListItem>
										<asp:ListItem Value="3">�˿��</asp:ListItem>
										<asp:ListItem Value="4">�̻���</asp:ListItem>
										<asp:ListItem Value="5">�˿����κ�</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD><asp:textbox id="tbrefund_order" runat="server"></asp:textbox></TD>
								<TD align="right"><asp:label id="Label8" runat="server">�˿�;��</asp:label></TD>
								<TD><asp:dropdownlist id="ddlrefund_path" runat="server" Width="152px">
										<asp:ListItem Value="99" Selected="True">���з�ʽ</asp:ListItem>
										<asp:ListItem Value="1">�����˵�</asp:ListItem>
										<asp:ListItem Value="2">�ӿ��˵�</asp:ListItem>
										<asp:ListItem Value="3">�˹���Ȩ</asp:ListItem>
										<asp:ListItem Value="4">ת���˵�</asp:ListItem>
										<asp:ListItem Value="5">ת�����</asp:ListItem>
										<asp:ListItem Value="6">�����˿�</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="Label10" runat="server">�˵�״̬</asp:label></TD>
								<TD><asp:dropdownlist id="ddlState" runat="server" Width="152px">
										<asp:ListItem Value="99" Selected="True">����״̬</asp:ListItem>
										<asp:ListItem Value="0">��ʼ״̬</asp:ListItem>
										<asp:ListItem Value="1">�˵�������</asp:ListItem>
										<asp:ListItem Value="2">�˵��ɹ�</asp:ListItem>
										<asp:ListItem Value="3">�˵�ʧ��</asp:ListItem>
										<asp:ListItem Value="8">���쳣������</asp:ListItem>
										<asp:ListItem Value="6">�����ֹ�����</asp:ListItem>
										<asp:ListItem Value="5">�ֹ��˵���</asp:ListItem>
										<asp:ListItem Value="7">����ת�����</asp:ListItem>
										<asp:ListItem Value="4">�˵�״̬δ��</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD align="right"><asp:label id="Label4" runat="server">ÿҳ��ʾ����</asp:label><asp:dropdownlist id="ddlChangePageSize" runat="server">
										<asp:ListItem Value="50">Ĭ��50</asp:ListItem>
										<asp:ListItem Value="100">100</asp:ListItem>
										<asp:ListItem Value="500">500</asp:ListItem>
										<asp:ListItem Value="1000">1000</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD><asp:button id="btnQuery" runat="server" Text="��ѯ��¼" onclick="btnQuery_Click"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="1" runat="server">
							<TR>
								<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
										CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" PageSize="50">
										<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
										<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
										<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
										<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
										<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="FOldID" HeaderText="�˵�ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="FPaylistid" HeaderText="��ֵ����"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fbank_listid" HeaderText="���ж�����"></asp:BoundColumn>
											<asp:BoundColumn DataField="Fbank_typeName" HeaderText="�˿�����"></asp:BoundColumn>
											<asp:BoundColumn DataField="FBatch_date" HeaderText="ԭ�˿�����"></asp:BoundColumn>
											<asp:BoundColumn DataField="FreturnamtName" HeaderText="�˵����" DataFormatString="{0:N}"></asp:BoundColumn>
											<asp:BoundColumn DataField="FamtName" HeaderText="�������" DataFormatString="{0:N}"></asp:BoundColumn>
											<asp:BoundColumn DataField="FstateName" HeaderText="�˵�״̬"></asp:BoundColumn>
											<asp:BoundColumn DataField="FreturnStateName" HeaderText="�ص�״̬"></asp:BoundColumn>
											<asp:BoundColumn DataField="FrefundPathName" HeaderText="�˵�;��"></asp:BoundColumn>
											<asp:BoundColumn DataField="FHandleTypeName" HeaderText="����״̬"></asp:BoundColumn>
											<asp:BoundColumn DataField="FHandleBatchId" HeaderText="����ID"></asp:BoundColumn>
											<asp:BoundColumn DataField="FAuthorizeFlagName" HeaderText="��Ȩ�˿���Ϣ"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="��ϸ">
												<ItemTemplate>
													<A href='<%# String.Format("RefundErrorQuery_Detail.aspx?oldid={0}", DataBinder.Eval(Container, "DataItem.FOldID").ToString()) %>'>
														��ϸ </A>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR height="25">
								<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="50" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
										OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
										ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td colSpan="2"><asp:label id="labErrMsg" runat="server" ForeColor="Red"></asp:label></td>
				</tr>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>
