<%@ Page language="c#" Codebehind="UserAccountQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserAccountQuery1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserAccountQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
	.style3 { COLOR: #000000 }
	.style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<SCRIPT language="javascript">
		function IsNumber(string,sign) 
		{ 
			var number; 
			if (string==null) 
				return false; 
 
			number = new Number(string); 
			if (isNaN(number)) 
			{ 
				return false; 
			} 
			else
			{ 
				return true; 
			} 
		}
			function checkvlid()
			{
				with(Form1)
				{
					if(TextBox1_QQID.value=="")
					{
						alert("�û��ʻ�����Ϊ��!!");
						TextBox1_QQID.focus();
						return false;
					}	
				}
				return true;
			}
		</SCRIPT>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="90%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="97%" border="0">
							<tr>
								<td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										�û���ѯ</font>
									<div align="right"></div>
								</td>
								<td width="20%">����Ա����: <span class="style4">
										<asp:Label id="Label_uid" runat="server">0755688</asp:Label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<div align="center"></div>
						<div align="left">
							<table height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<tr>
									<td width="19%">&nbsp;</td>
									<td width="78%">���룺&nbsp;
										<asp:textbox id="TextBox1_QQID" runat="server" BorderWidth="1px" BorderStyle="Solid"></asp:textbox>
										&nbsp;&nbsp;
										<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1_QQID" Width="117px"
											ErrorMessage="RequiredFieldValidator">������QQ����</asp:requiredfieldvalidator></td>
									<TD width="3%">&nbsp;</TD>
								</tr>
							</table>
						</div>
						<div align="left"></div>
						<div align="center"></div>
					</td>
					<td width="25%">
						<div align="center">&nbsp;
							<asp:button id="Button1" runat="server" Text="�� ѯ" onclick="Button1_Click"></asp:button></div>
					</td>
				</tr>
			</table>
			<br>
			<TABLE cellSpacing="0" cellPadding="0" width="95%" align="center" border="0">
				<TR>
					<TD bgColor="#666666">
						<TABLE cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
							<TR bgColor="#e4e5f7">
								<TD background="../IMAGES/Page/bg_bl.gif" height="20"><font color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;<FONT face="����">��ϸ</FONT>
									</font>
								</TD>
							</TR>
							<TR>
								<TD bgColor="#ffffff" height="12"><asp:panel id="PanelList" runat="server">
										<P>
											<asp:DataGrid id="DGData" runat="server" BorderStyle="None" BorderWidth="1px" Width="100%" GridLines="Horizontal"
												CellPadding="3" BackColor="White" BorderColor="#E7E7FF" AutoGenerateColumns="False" OnItemDataBound="DGData_ItemDataBound">
												<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
												<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
												<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
												<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
												<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
												<Columns>
													<asp:BoundColumn DataField="Fbank_name" HeaderText="��������">
														<HeaderStyle Width="100px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Fcard_tail" HeaderText="��β��">
														<HeaderStyle Width="150px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FstateName" HeaderText="�ʻ�״̬">
														<HeaderStyle Width="200px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FcurtypeName" HeaderText="����">
														<HeaderStyle Width="150px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Fprimary_flagName" HeaderText="�Ƿ�����"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Farea" HeaderText="��������"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fcity" HeaderText="��������"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fbankid_str" HeaderText="�����ʺ�"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Flogin_ip" HeaderText="IP��ַ"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fbank_type" HeaderText="��������"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fmemo" HeaderText="��ע"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fcreate_time" HeaderText="��������"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fmodify_time" HeaderText="����ʱ��"></asp:BoundColumn>
													
                                                    <asp:TemplateColumn HeaderText="����">
                                                      <ItemTemplate>
                                                        <asp:LinkButton id="lbDetail" runat="server" CommandName="DETAIL">��ϸ</asp:LinkButton>
                                                        <asp:LinkButton id="lbChange" Visible="false" runat="server" CommandName="CHANGE">�ⶳ</asp:LinkButton>
                                                      </ItemTemplate>
                                                    </asp:TemplateColumn>
													<asp:BoundColumn Visible="False" DataField="fqqid" HeaderText="QQ��"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Ftruename" HeaderText="������"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fcompany_name" HeaderText="��˾����"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Fcurtype" HeaderText="��"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
											</asp:DataGrid></P>
									</asp:panel><asp:panel id="PanelDetail" runat="server" Height="128px" Visible="False">
										<TABLE cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
											<TR>
												<TD><IFRAME id=fsonhist2 style="WIDTH: 100%; HEIGHT: 160px" 
                  name=fsonhist2 marginWidth=0 marginHeight=0 
                  src="<%=iFramePath%>" frameBorder=0 width="100%" scrolling=no 
                  height="<%=iFrameBank%>"></IFRAME>
												</TD>
											</TR>
										</TABLE>
										<DIV align="center"><FONT face="����">
												<asp:Button id="btBack" runat="server" BorderStyle="Groove" Width="71px" Text="  �� ��  " onclick="btBack_Click"></asp:Button></FONT></DIV>
									</asp:panel><INPUT id="Harea" type="hidden" name="Harea" runat="server"><INPUT id="Hcity" type="hidden" name="Hcity" runat="server"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
