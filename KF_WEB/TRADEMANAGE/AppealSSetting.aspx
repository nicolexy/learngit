<%@ Page language="c#" Codebehind="AppealSSetting.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.AppealSSetting" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppealSSetting</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		 p.font1 {
                font-weight:bold;
                font-size:14px;
                color:red;
		    }
    </style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="900"
				align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD colSpan="5"><FONT color="#ff0000"><IMG src="../IMAGES/Page/post.gif" width="15"><asp:label id="lbTitle" runat="server">��������ѯ</asp:label></FONT>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="Label2" runat="server" ForeColor="ControlText">����Ա���룺</asp:label><FONT color="#ff0000"><asp:label id="Label_uid" runat="server">Label</asp:label></FONT></TD>
				</TR>
				<TR>
					<TD><FONT face="����">�̻��ţ� </FONT>
						<asp:textbox id="tb_user" runat="server"></asp:textbox></TD>
					<TD><FONT face="����">�ʺ����ͣ� </FONT>
						<asp:dropdownlist id="ddl_usertype" runat="server" Width="80px">
							<asp:ListItem Value="-1">����</asp:ListItem>
							<asp:ListItem Value="2">�̻�</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD><FONT face="����">״̬�� </FONT>
						<asp:dropdownlist id="ddl_state" runat="server" Width="96px">
							<asp:ListItem Value="-1">����</asp:ListItem>
							<asp:ListItem Value="1">�̻�����</asp:ListItem>
							<asp:ListItem Value="2">�̻�����</asp:ListItem>
							<asp:ListItem Value="3">�Ƹ�ͨ����ͨ��</asp:ListItem>
							<asp:ListItem Value="4">�ܾ�</asp:ListItem>
							<asp:ListItem Value="5">����</asp:ListItem>
							<asp:ListItem Value="6">����</asp:ListItem>
						</asp:dropdownlist></TD>
					<TD><asp:button id="btn_query" runat="server" Width="58px" BackColor="InactiveCaptionText" Text="��ѯ" onclick="btn_query_Click"></asp:button></TD>
				</TR>
				<TR>
					<TD colSpan="5"><FONT face="����"><asp:label id="lb_msg" runat="server" ForeColor="Red" Width="720px"></asp:label>&nbsp;&nbsp;</FONT></TD>
				</TR>
				<tr>
					<td colSpan="5"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" BorderWidth="1px" BorderColor="#E7E7FF"
							BorderStyle="None" CellPadding="1" AutoGenerateColumns="False" GridLines="Horizontal"
                            OnItemDataBound="DataGrid1_ItemDataBound">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fno" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUser" HeaderText="�ⲿ�ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserId" HeaderText="�ڲ��ʺ�"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserTypeS" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FUserName" HeaderText="�ʺ�����"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycTypeS" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycS" HeaderText="���ڵ�λ"></asp:BoundColumn>
								<asp:BoundColumn DataField="FCycNumberS" HeaderText="���ڿ��(T+)"></asp:BoundColumn>
								<asp:BoundColumn DataField="FStateS" HeaderText="��ǰ״̬"></asp:BoundColumn>
                              <%--   <asp:TemplateColumn HeaderText="����">
									    <ItemTemplate>
										    <asp:LinkButton id="queryButton" Visible="false" runat="server" CommandName="query" Text="�༭/�鿴"></asp:LinkButton>
									    </ItemTemplate>
								   </asp:TemplateColumn>--%>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#F7F7DE" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td colSpan="5"><webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��"
							OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" PageSize="15"></webdiyer:aspnetpager></td>
				</tr>
			</TABLE>
              <div id="RemaindDiv" runat="server" visible="true">
                <p class="font1">���ѣ�<br />�����ڡ���������ѯ���к�ʵ�Ƿ���T+0������T+0�������ڡ���������ѯ�����ȹر�T+0���Ҵ����ٲ�������ͣ���㡱����T+0�̻���ֱ�Ӳ�������ͣ���㡱��
                </p>
            </div>
		</form>
	</body>
</HTML>
