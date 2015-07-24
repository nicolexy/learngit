<%@ Page language="c#" Codebehind="SelfQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.SelfQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SelfQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
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
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="850" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="6"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;����BD�̻���ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ƣ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
						<asp:TextBox id="BoxCpName" runat="server"></asp:TextBox></TD>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left">
						<asp:TextBox id="BoxCpNumber" runat="server"></asp:TextBox></TD>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">�̻�����״̬��</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px" align="left">
						<asp:DropDownList id="BoxCpStatus" runat="server" Width="152px"></asp:DropDownList></TD>
				</TR>
                <tr>
                    <TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��ַ��</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px">
						<asp:TextBox id="boxWWWAddress" runat="server"></asp:TextBox></TD>
                    <TD style="WIDTH: 100px; HEIGHT: 27px" align="right">APPID��</TD>
					<TD style="WIDTH: 193px; HEIGHT: 27px" colspan="3">
						<asp:TextBox id="txtAppid" runat="server"></asp:TextBox></TD>
                </tr>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">�ύʱ���</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px">
						<asp:TextBox id="TextBoxBeginDate" runat="server"></asp:TextBox>
						<asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">
						��</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px">
						<asp:TextBox id="TextBoxEndDate" runat="server"></asp:TextBox>
						<asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>
					</TD>
                    <TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ͣ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px">
						<asp:DropDownList id="ddlMerType" runat="server" Width="150px">
							
						</asp:DropDownList></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�������ƣ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px">
						<asp:TextBox id="tbBankName" runat="server"></asp:TextBox></TD>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">�����ͷ���Ա��</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px">
						<asp:DropDownList id="ddlKFList" runat="server" Width="150px"></asp:DropDownList></TD>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">�Ƽ��ˣ�</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px"><asp:TextBox id="tbSuggestUser" runat="server"></asp:TextBox></TD>
				</TR>
				<TR>
					
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">��ѯ�����</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px">
						<asp:Label ID="lblResultCount" Runat="server"></asp:Label></TD>
					<TD colspan="4">
						<asp:Button id="btnSearch" runat="server" Text="�� ѯ" onclick="btnSearch_Click"></asp:Button></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 850px; POSITION: absolute; TOP: 250px; HEIGHT: 800px">
				<table cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="ApplyCpInfoID" HeaderText="���"></asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" HeaderText="�̻�����">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BankUserName" HeaderText="��������">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BargainCode" HeaderText="��ͬ��">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="����ʱ��">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UserName" HeaderText="������">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="KFCheckUser" HeaderText="�쵥��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckUserName" HeaderText="������">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckTime" HeaderText="����ʱ��">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FlagStr" HeaderText="��ǰ״̬">
										<HeaderStyle Width="90px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Flag" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="datafrom" Visible="False"></asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemStyle Width="30px"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink id="hylDetail" runat="server" Text='�鿴' NavigateUrl='<%#"SelfQueryDetail.aspx?mode=query&ID=" +DataBinder.Eval(Container.DataItem, "ApplyCpInfoID")+"&SPID="+DataBinder.Eval(Container.DataItem, "SPID")%>'>
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:ButtonColumn Text="�쵥" ButtonType="PushButton" CommandName="Select"></asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR height="25">
						<TD><webdiyer:aspnetpager id="pager" runat="server" PageSize="10" HorizontalAlign="right" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="ת��" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
