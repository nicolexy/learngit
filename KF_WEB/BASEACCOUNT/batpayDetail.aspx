<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="batPayDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.batpayDetail"  %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>batPayDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
					    var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				   	    if(returnValue != null)
				   	        Form1.TextBoxBeginDate.value=returnValue;
					}
					
					function  CheckData()
					{
					    var tbUserName = document.getElementById("tbUserName");
					    var tbBankAcc = document.getElementById("tbBankAcc");
					    
					    if(tbUserName.value.replace( /^\s*/, "").replace( /\s*$/, "").length==0 && tbBankAcc.value.replace( /^\s*/, "").replace( /\s*$/, "").length==0)
		                {
		                    tbUserName.focus();
		                    tbUserName.select();
		                    alert("�������û������������ʺ�!");
		                    return false;
		                }
					}
		</script>
	</HEAD>
	<body background="../IMAGES/Page/bg01.gif" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table3" style="Z-INDEX: 101; LEFT: 5%; WIDTH: 94%; POSITION: absolute; TOP: 5%"
				borderColor="#666666" height="127" cellSpacing="1" cellPadding="1" width="383" align="center"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;������ϸ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD colspan="2">
						<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="1">
							<TR>
								<TD>
									<asp:Label id="Label2" runat="server">��ǰ״̬</asp:Label></TD>
								<TD>
									<asp:DropDownList id="ddlState" runat="server">
										<asp:ListItem Value="9" Selected="True">����״̬</asp:ListItem>
										<asp:ListItem Value="0">������</asp:ListItem>
										<asp:ListItem Value="1">���ύ����</asp:ListItem>
										<asp:ListItem Value="2">����ɹ�</asp:ListItem>
										<asp:ListItem Value="3">����ʧ��</asp:ListItem>
										<asp:ListItem Value="5">�ɹ�����Ʊ</asp:ListItem>
										<asp:ListItem Value="6">��Ʊ��</asp:ListItem>
										<asp:ListItem Value="7">����Ʊ</asp:ListItem>
									</asp:DropDownList></TD>
								<TD>
									<asp:Label id="Label3" runat="server">�û�����</asp:Label></TD>
								<TD>
									<asp:TextBox id="tbUserName" runat="server" BorderColor="Gray" BorderWidth="1px"></asp:TextBox></TD>
							</TR>
							<TR>
								<TD>
									<asp:Label id="Label4" runat="server">�����ʺ�</asp:Label></TD>
								<TD>
									<asp:TextBox id="tbBankAcc" runat="server"></asp:TextBox></TD>
								&lt;
								<TD>
									<asp:Label id="Label6" runat="server">��������</asp:Label></TD>
								<TD>
									<asp:DropDownList id="ddlBankType" runat="server"></asp:DropDownList></TD>
							</TR>
							<TR>
								<TD colspan="4" align="center">
									<asp:Button id="Button2" runat="server" Text="��ѯ��¼" onclick="Button2_Click"></asp:Button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top" colspan="2">
						<asp:DataGrid id="DataGrid1" runat="server" Width="100%" Height="122px" AutoGenerateColumns="False"
							BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3"
							GridLines="Horizontal">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="FSequence" HeaderText="����˳���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTruename" HeaderText="��ʵ����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FBankAccNo" HeaderText="�����ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Famt1" HeaderText="������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStatusName" HeaderText="��ǰ״̬"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="FSequence1" HeaderText="��Ӧ��ϵ˳���">
									<ItemStyle BackColor="#66FFCC"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FRTFlagName" HeaderText="��Ʊ"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPayBankTypeName" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTde_ID" HeaderText="�����ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="FQQID" HeaderText="�û��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fnum1" HeaderText="���">
									<ItemStyle BackColor="#66FFCC"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FStatusName1" HeaderText="״̬">
									<ItemStyle BackColor="#66FFCC"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:DataGrid></TD>
				</TR>
				<TR>
					<TD colspan="2">
						<webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="15"></webdiyer:aspnetpager></TD>
				</TR>
				<TR>
					<TD width="60%" align="center">
						<DIV style="DISPLAY: inline; WIDTH: 33px; COLOR: #eeeeee; HEIGHT: 15px" ms_positioning="FlowLayout">--</DIV>
					</TD>
					<TD align="left" width="40%">
						<asp:hyperlink id="hlBack" runat="server" ForeColor="Blue" NavigateUrl="javascript:history.go(-1)">����</asp:hyperlink></TD>
				</TR>
			</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
