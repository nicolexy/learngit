<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<%@ Page language="c#" Codebehind="FreezeList.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FreezeList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>

  <HEAD>

		<title>FreezeList</title>

		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">

		<meta content="C#" name="CODE_LANGUAGE">

		<meta content="JavaScript" name="vs_defaultClientScript">

		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }

	.style3 { COLOR: #ff0000 }

	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }

	</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</HEAD>

	<body MS_POSITIONING="GridLayout">

		<form id="Form1" method="post" runat="server">

			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1"

				cellPadding="1" width="85%" border="1">

				<TR>

					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�����¼��ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>

				</TR>

				<TR>

					<TD style="WIDTH: 83px" align="right"><asp:label id="Label2" runat="server">��ʼ����</asp:label></TD>

					<TD>
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>

					<TD align="right"><asp:label id="Label3" runat="server">��������</asp:label></TD>

					<TD>
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>

				</TR>

				<TR>

					<TD style="WIDTH: 83px" align="right"><asp:label id="Label5" runat="server">����״̬</asp:label></TD>

					<TD><asp:dropdownlist id="ddlStateType" runat="server" Width="152px">

							<asp:ListItem Value="0" Selected="True">����״̬</asp:ListItem>

							<asp:ListItem Value="1">������</asp:ListItem>

							<asp:ListItem Value="2">�������</asp:ListItem>

						</asp:dropdownlist></TD>

					<TD align="right"><asp:label id="Label6" runat="server">������Ա</asp:label></TD>

					<TD><asp:textbox id="tbFreezeUser" runat="server"></asp:textbox></TD>

				</TR>

				<TR>

					<TD style="WIDTH: 83px" align="right"><asp:label id="Label7" runat="server">��������</asp:label></TD>

					<TD><asp:dropdownlist id="ddlHandleType" runat="server" Width="152px">

							<asp:ListItem Value="0" Selected="True">��������</asp:ListItem>

							<asp:ListItem Value="1">�����ʻ�</asp:ListItem>

							<asp:ListItem Value="2">�������׵�</asp:ListItem>

						</asp:dropdownlist></TD>

					<TD align="right"><asp:label id="Label4" runat="server">�û�����</asp:label></TD>

					<TD><asp:textbox id="tbUserName" runat="server"></asp:textbox></TD>

				</TR>

				<TR>

					<TD style="WIDTH: 81px" align="right" colSpan="1" rowSpan="1">

						<asp:Label id="Label8" runat="server" Width="104px">�����ʺ�/���׵�</asp:Label></TD>

					<TD>

						<asp:TextBox id="tbQQ" runat="server"></asp:TextBox></TD>

					<TD align="center" colSpan="2">

						<asp:button id="Button2" runat="server" Width="80px" Text="�� ѯ" onclick="Button2_Click"></asp:button></TD>

				</TR>

			</TABLE>

			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 60%"

				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">

				<TR>

					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"

							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">

							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>

							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>

							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>

							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>

							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>

							<Columns>

								<asp:BoundColumn DataField="FUserName" HeaderText="�û�����"></asp:BoundColumn>

								<asp:BoundColumn DataField="FFreezeTypeName" HeaderText="��������"></asp:BoundColumn>

								<asp:BoundColumn DataField="FHandleFinishName" HeaderText="����״̬"></asp:BoundColumn>

								<asp:BoundColumn DataField="FFreezeUserID" HeaderText="������Ա"></asp:BoundColumn>

								<asp:BoundColumn DataField="FFreezeTime" HeaderText="����ʱ��" DataFormatString="{0:D}"></asp:BoundColumn>

								<asp:HyperLinkColumn Text="��ϸ����" DataNavigateUrlField="FID" DataNavigateUrlFormatString="FreezeDetail.aspx?fid={0}" 
 HeaderText="��ϸ����"></asp:HyperLinkColumn>

							</Columns>

							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>

						</asp:datagrid></TD>

				</TR>

				<TR height="25">

					<TD><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"

							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"

							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager></TD>

				</TR>

			</TABLE>

		</form>

	</body>

</HTML>

