<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FastPayLimitQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FastPay.FastPayLimitQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FastPayLimitQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="900"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;��ݶ�Ȳ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">���п��ţ�</asp:label></TD>
                    <TD><asp:textbox id="txtCardNo" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label4" runat="server">�������ͣ�</asp:label></TD>
                    <TD><asp:textbox id="txtBankType" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
               </TR>
				<TR>
                    <TD align="right"><asp:label id="Label10" runat="server">��������</asp:label></TD>
					<TD><asp:dropdownlist id="ddlpay_type" runat="server" Width="90px">
							<asp:ListItem Value="1">��ֵ</asp:ListItem>
							<asp:ListItem Value="2" Selected="True">֧��</asp:ListItem>
						</asp:dropdownlist>
					</TD>
                     <TD align="right"><asp:label id="Label3" runat="server">������</asp:label></TD>
                    <TD><asp:dropdownlist id="ddlcard_type" runat="server" Width="90px">
							<asp:ListItem Value="1" Selected="True">��ǿ�</asp:ListItem>
							<asp:ListItem Value="2">���ÿ�</asp:ListItem>
						</asp:dropdownlist>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
                    </TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%;POSITION: absolute; TOP: 150px; "
				cellSpacing="1" cellPadding="1" width="900" border="1" runat="server">
                <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16">�����޶�</font>
                </td>
               </tr>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="type" HeaderText="���">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="day_0" HeaderText="����">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="day_existed_num_0" HeaderText="�����ۼƴ���">
                                      <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="day_existed_amount_0" HeaderText="�����ۼƶ��">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="month_existed_num_0" HeaderText="�����ۼƴ���">
                                      <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="month_existed_amount_0" HeaderText="�����ۼƶ��">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" 
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
            <TABLE id="Table1" style="Z-INDEX: 103; LEFT: 5.02%;POSITION: absolute; TOP: 350px; "
				cellSpacing="1" cellPadding="1" width="900" border="1" runat="server">
                  <tr>
                <td style="width: 100%" bgcolor="#e4e5f7" colspan="5">
                    <font color="red">
                        <img src="../IMAGES/Page/post.gif" width="20" height="16">��ҵ�޶�</font>
                </td>
               </tr>
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid2" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                               <asp:BoundColumn DataField="type" HeaderText="ҵ������"> 
                                   <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                               </asp:BoundColumn>
                                 <asp:BoundColumn DataField="day_0" HeaderText="����">
                                      <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                 </asp:BoundColumn>
                                <asp:BoundColumn DataField="day_existed_num_0" HeaderText="�����ۼƴ���">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="day_existed_amount_0" HeaderText="�����ۼƶ��">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="month_existed_num_0" HeaderText="�����ۼƴ���">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="month_existed_amount_0" HeaderText="�����ۼƶ��">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="limit_type_1_0" HeaderText="�����޶�">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="limit_type_2_0" HeaderText="���޶�">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="limit_type_3_0" HeaderText="�մ�������">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="limit_type_4_0" HeaderText="���޶�">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="limit_type_5_0" HeaderText="�´�������">
                                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"/>  
                                </asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="Aspnetpager1" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" 
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
