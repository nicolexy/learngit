<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="MediCertExpireQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MediCertExpireQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MediCertExpireQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="3"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻�֤�鵽�ڲ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
                <TR>
					<TD align="right"><asp:label id="Label5" runat="server">֤�鵽��ʱ�䣺</asp:label></TD>
					<TD>
                        <asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
					</TD>
					<TD align="left"><asp:label id="Label6" runat="server">����</asp:label>
                    <asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label2" runat="server">�̻��ţ�</asp:label></TD>
					<TD><asp:textbox id="txtFspid" runat="server"></asp:textbox></TD>
                    <TD align="left">
                   <FONT face="����"><asp:button id="Button1" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH: 90%; POSITION: absolute; TOP: 150px;">
				<table cellSpacing="0" cellPadding="0" width=100% border="0" >
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" Width=100%>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
                                    <%--<asp:TemplateColumn HeaderText="����">
                                     <ItemTemplate>
                                            <input type="hidden" id="SelectID" runat="server" 
                                            value='<%#DataBinder.Eval(Container.DataItem,"Fspid")%>'/>
                                            <asp:CheckBox ID="chkExport" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>--%>
									<asp:BoundColumn DataField="Fspid" HeaderText="�̻���">
										<HeaderStyle Width="150px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CompanyName" HeaderText="��˾����">
									    <HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ContactPhone" HeaderText="��ϵ�绰">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ContactQQ" HeaderText="QQ">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ContactEmail" HeaderText="Email">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UserName" HeaderText="����DB">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fcrt_etime" HeaderText="֤�����ʱ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn DataField="Fmemo" HeaderText="��ע">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:TemplateColumn HeaderText="�༭">
									<ItemTemplate>
										<asp:LinkButton id="lbEdit" runat="server" CommandName="EDIT">�༭</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
                                    <asp:BoundColumn DataField="FmodifyTime" HeaderText="�޸�ʱ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="FupdateUser" HeaderText="������">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                   <%-- <tr>
                        <td>
                        <asp:Button id="cmdSelectAll" runat="server" Text="ȫ��ѡ��"/>
                        </td>
                    </tr>--%>
                     <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				   </TR>
                   <%-- <tr>
                    <TD><FONT face="����"><asp:button id="btnSendEmail" runat="server" Width="80px" Text="�ʼ�����֪ͨ" onclick="btnSendEmailNotice_Click"></asp:button></FONT></TD>
                    </tr>--%>
				</table>
			</div>
		</form>
	</body>
</HTML>
