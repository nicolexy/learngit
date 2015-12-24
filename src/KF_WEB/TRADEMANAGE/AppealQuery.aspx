<%@ Page language="c#" Codebehind="AppealQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppealQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
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
			<asp:panel id="PanelList" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="1000">
					<TR>
						<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;Ͷ���б�</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>����Ա����: </FONT><SPAN class="style3">
								<asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label2" runat="server">Ͷ�߱��</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealID" Runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label6" runat="server">���׶�����</asp:label></TD>
						<TD>
							<asp:textbox id="txtOrderNo" Runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label7" runat="server">Ͷ������</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlAppealType" Runat="server" Width="400px">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">�ɽ�����</asp:ListItem>
								<asp:ListItem Value="2">�ջ������ȷ�ϣ�</asp:ListItem>
								<asp:ListItem Value="3">�˿���ף�����Ͷ����ң�</asp:ListItem>
								<asp:ListItem Value="4">��Ҷ�������</asp:ListItem>
								<asp:ListItem Value="5">�ɽ�����</asp:ListItem>
								<asp:ListItem Value="6">���Ҿܾ�ʹ�òƸ�ͨ����</asp:ListItem>
								<asp:ListItem Value="7">�տ����</asp:ListItem>
								<asp:ListItem Value="8">��Ʒ����������</asp:ListItem>
								<asp:ListItem Value="9">���Ҷ�������</asp:ListItem>
								<asp:ListItem Value="10">�˿���ף����Ͷ�����ң�</asp:ListItem>
								<asp:ListItem Value="11">����Ҫ�������ȷ���ջ��������ٷ���</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:label id="Label3" runat="server">��Ͷ���û�</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealedQQ" runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label4" runat="server">Ͷ���û�</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealQQ" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label5" runat="server">�Ƿ����</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlFcheck_state" Runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">δ���</asp:ListItem>
								<asp:ListItem Value="2">���ύ���</asp:ListItem>
								<asp:ListItem Value="3">��˲�ͨ��</asp:ListItem>
								<asp:ListItem Value="4">���ͨ��</asp:ListItem>
								<asp:ListItem Value="5">���˿�</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">Ͷ�߷�������</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate" runat="server" Width="200px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
							</TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">������</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate1" runat="server" Width="200px" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label10" runat="server">Ͷ�߸�������</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestUpdDate" runat="server" Width="200px" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
						<TD align="right">
							<asp:label id="Label11" runat="server">������</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestUpdDate1" runat="server" Width="200px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label12" runat="server">Ͷ��״̬</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlAppealState" Runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">δ����</asp:ListItem>
								<asp:ListItem Value="2">������</asp:ListItem>
								<asp:ListItem Value="3">�ѽ���</asp:ListItem>
								<asp:ListItem Value="4">�ѳ���</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:label id="Label13" runat="server">����Ͷ����Ӧ</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlResponseFlag" Runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">��</asp:ListItem>
								<asp:ListItem Value="2">��</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label14" runat="server">��������</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlRefundFlag" Runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">��</asp:ListItem>
								<asp:ListItem Value="2">��</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:Button id="btnQuery" Runat="server" Text="�� ѯ" onclick="btnQuery_Click"></asp:Button></TD>
						<TD></TD>
					</TR>
				</TABLE>
				<TABLE border="0" cellSpacing="0" cellPadding="0" width="1000">
					<TR>
						<TD vAlign="top">
							<asp:datagrid id="DataGrid1" runat="server" Width="1000px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								PageSize="15" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fappealid" HeaderText="Ͷ�߱��">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fappeal_time" HeaderText="Ͷ�߷�������">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmodify_time" HeaderText="Ͷ�߸�������">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fvs_qqid" HeaderText="�����û�">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fqqid" HeaderText="Ͷ���û�">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FstateStr" HeaderText="Ͷ��״̬">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fappeal_type" HeaderText="Ͷ������">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcheck_stateStr" HeaderText="��˽��">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CLResult" HeaderText="������">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="KFResult" HeaderText="�ͷ�����">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="��ϸ">
										<ItemTemplate>
											<a href='./AppealQuery.aspx?type=detail&Fappealid=<%# DataBinder.Eval(Container, "DataItem.Fappealid")%>'>
												��ϸ</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="ת��" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								NumericButtonCount="5"></webdiyer:aspnetpager></TD>
					</TR>
				</TABLE>
			</asp:panel><asp:panel id="PanelMod" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="900">
					<TR>
						<TD colSpan="6">������ϸ���
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label15" Runat="server">Ͷ�߷�����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFqqid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label16" Runat="server">Ͷ�ߵ����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappealid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label17" Runat="server">�û�����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFresponse_flag" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label18" Runat="server">��Ͷ�ߺ���</asp:Label></TD>
						<TD>
							<asp:Label id="lblFvs_qqid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label19" Runat="server">���׵����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFlistid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label20" Runat="server">����ѡ��</asp:Label></TD>
						<TD>��</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label21" Runat="server">�������</asp:Label></TD>
						<TD>
							<asp:Label id="lblFtotal_fee" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label22" Runat="server">Ͷ�߷��˿��ң�</asp:Label></TD>
						<TD>
							<asp:Label id="lblFpaybuy" Runat="server"></asp:Label>����ȷ���֣�</TD>
						<TD align="right">
							<asp:Label id="Label23" Runat="server">����״̬</asp:Label></TD>
						<TD>
							<asp:Label id="lblFlist_state" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label24" Runat="server">������ȯ</asp:Label></TD>
						<TD>
							<asp:Label id="lblFtoken_fee" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label25" Runat="server">���߷��˿���ң�</asp:Label></TD>
						<TD>
							<asp:Label id="lblFpaysale" Runat="server"></asp:Label>����ȷ���֣�
						</TD>
						<TD align="right"></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD colSpan="6">Ͷ������
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label27" Runat="server">Ͷ������</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappeal_type" Runat="server" Visible=False></asp:Label>
							<asp:Label id="lblFappeal_typeStr" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label28" Runat="server">��ʼʱ��</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappeal_time" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label29" Runat="server">����ʱ��</asp:Label></TD>
						<TD>
							<asp:Label id="lblFend_time" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="Label30" Runat="server">Ͷ������</asp:Label></TD>
						<TD colSpan="5">
							<asp:Label id="lblFappeal_con" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="Label31" Runat="server">�������</asp:Label></TD>
						<TD colSpan="5">
							<asp:Label id="lblFmemo" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD colSpan="6">
							<asp:datagrid id="Datagrid2" runat="server" Width="1000px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								PageSize="100" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Fownerid" HeaderText="�ʺ�">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FtypeStr" HeaderText="����">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmsg" HeaderText="Ͷ������">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmodify_time" HeaderText="����ʱ��">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fattach1" HeaderText="ͼƬ1">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fmemo" HeaderText="��ע">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager1" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="ת��" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								NumericButtonCount="5"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD colSpan="6">���������֧��400�֣�
						</TD>
					</TR>
					<TR>
						<TD colSpan="6">
							<asp:TextBox id="txtMess" Runat="server" Width="500px" Height="200px"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="Button1" Runat="server" Text="Ҫ��˫������" onclick="Button1_Click"></asp:Button></TD>
						<TD>
							<asp:Button id="Button2" Runat="server" Text="Ҫ���߷�����" onclick="Button2_Click"></asp:Button></TD>
						<TD>
							<asp:Button id="Button3" Runat="server" Text="Ҫ��Ͷ�߷�����" onclick="Button3_Click"></asp:Button></TD>
						<TD colSpan="3">
							<asp:Label id="Label26" Runat="server">�Ƿ��漰�˿��</asp:Label>��
							<asp:RadioButton id="IsFfund_flag" Runat="server" Text="�漰" GroupName="Ffund_flag"></asp:RadioButton>
							<asp:RadioButton id="NoFfund_flag" Runat="server" Text="���漰" GroupName="Ffund_flag"></asp:RadioButton><BR>
							<asp:Button id="Button4" Runat="server" Text="�ύ���" Visible=False onclick="Button4_Click"></asp:Button></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="Button5" Runat="server" Text="���ͨ��" onclick="Button5_Click"></asp:Button></TD>
						<TD>
							<asp:Button id="Button6" Runat="server" Text="���δͨ��" onclick="Button6_Click"></asp:Button></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
