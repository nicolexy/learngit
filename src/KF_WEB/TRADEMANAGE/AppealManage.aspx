<%@ Page language="c#" Codebehind="AppealManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealManage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppealManage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
			function openModeBegin()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtRequestDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.txtRequestDate.value=returnValue;
			}
			
			function openModeEnd()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.txtRequestDate1.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.txtRequestDate1.value=returnValue;
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:panel id="PanelList" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="1000">
					<TR>
						<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;Ͷ���б�</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</FONT>����Ա����: </FONT><SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label2" runat="server">������</asp:label></TD>
						<TD>
							<asp:textbox id="txtOrderNo" Runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label3" runat="server">�����û�</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealedQQ" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label4" runat="server">Ͷ���û�</asp:label></TD>
						<TD>
							<asp:textbox id="txtAppealQQ" runat="server" Width="200px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">Ͷ�߿�ʼ����</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate" runat="server" Width="200px"></asp:textbox>
							<asp:imagebutton id="BeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">������</asp:label></TD>
						<TD>
							<asp:textbox id="txtRequestDate1" runat="server" Width="200px"></asp:textbox>
							<asp:imagebutton id="EndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:Button id="btnQuery" Runat="server" Text="�� ѯ"></asp:Button></TD>
						<TD></TD>
					</TR>
				</TABLE>
				<TABLE border="0" cellSpacing="0" cellPadding="0" width="1000">
					<TR>
						<TD vAlign="top">
							<asp:datagrid id="DataGrid1" runat="server" Width="1000px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="15" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Flistid" HeaderText="������">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fqqid" HeaderText="Ͷ�߷�QQ">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fvs_qqid" HeaderText="��Ͷ�߷�QQ">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fappeal_time" HeaderText="Ͷ������">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="ʵ�����">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FstateStr" HeaderText="����״̬">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CLResult" HeaderText="����״̬">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="KFResult" HeaderText="���״���">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="��ϸ">
										<ItemTemplate>
											<a href='./AppealManage.aspx?type=detail&Fappealid=<%# DataBinder.Eval(Container, "DataItem.Fappealid")%>'>
												��ϸ</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" NumericButtonCount="5" PagingButtonSpacing="0"
								ShowInputBox="always" CssClass="mypager" OnPageChanged="ChangePage" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"
								AlwaysShow="True"></webdiyer:aspnetpager></TD>
					</TR>
				</TABLE>
			</asp:panel><asp:panel id="PanelMod" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="900">
					<TR>
						<TD colSpan="6">��������
						</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label15" Runat="server">������</asp:Label></TD>
						<TD>
							<asp:Label id="lblFqqid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label16" Runat="server">Ͷ�ߵ����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFappealid" Runat="server"></asp:Label></TD>
						<TD align="right">
							<asp:Label id="Label17" Runat="server">����˿���</asp:Label></TD>
						<TD>
							<asp:Label id="lblFresponse_flag" Runat="server"></asp:Label>��λ��Ԫ��</TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label18" Runat="server">���ҽ�����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFvs_qqid" Runat="server"></asp:Label>��λ��Ԫ��</TD>
						<TD align="right">
							<asp:Label id="Label19" Runat="server">ʵ�����</asp:Label></TD>
						<TD>
							<asp:Label id="lblFlistid" Runat="server"></asp:Label>��λ��Ԫ��</TD>
						<TD align="right">
							<asp:Label id="Label20" Runat="server">����״̬</asp:Label></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label21" Runat="server">������ʾ</asp:Label></TD>
						<TD colSpan="5">
							<asp:Label id="lblFtotal_fee" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label24" Runat="server">��ע�����</asp:Label></TD>
						<TD colSpan="2">
							<asp:TextBox id="txtMess" Runat="server" Width="500px" Height="200px"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label5" Runat="server">�Ƹ�ͨ�û��������</asp:Label></TD>
						<TD>
							<asp:TextBox id="Textbox1" Runat="server"></asp:TextBox></TD>
						<TD align="right">
							<asp:Label id="Label6" Runat="server">�Ƹ�ͨ���루���</asp:Label></TD>
						<TD>
							<asp:TextBox id="Textbox2" Runat="server"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="Button1" Runat="server" Text="Ͷ������"></asp:Button></TD>
						<TD>
							<asp:Button id="Button2" Runat="server" Text="�������"></asp:Button></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
