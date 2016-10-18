<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="PayBusinessQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.PayBusinessQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PayBusinessQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script>
		    function CheckEmail()
		    {
		        var txtEmail = document.getElementById("txtEmail");
		        
		        if(txtEmail.value.replace( /^\s*/, "").replace( /\s*$/, "").length == 0)
		        {
		            txtEmail.focus();
		            txtEmail.select();
		            alert("���䲻����Ϊ��!");
		            return false;
		        }
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="820" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;ֱ���̻���ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">��˾����</asp:label></TD>
					<TD><asp:textbox id="txtFspidName" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label3" runat="server">�̻���</asp:label></TD>
					<TD><asp:textbox id="txtFspid" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">��ַ</asp:label></TD>
					<TD><asp:textbox id="txtFspidAddress" runat="server"></asp:textbox></TD>
                    <TD align="right"><asp:label id="Label21" runat="server">��վ����</asp:label></TD>
					<TD><asp:textbox id="txtWebName" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
					<TD align="right"><asp:label id="Label24" runat="server">APPID</asp:label></TD>
					<TD><asp:textbox id="txtAPPID" runat="server"></asp:textbox></TD>
                    <TD align="center" colspan="2"><FONT face="����"><asp:button id="btnSearch" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH:820px; POSITION: absolute; TOP: 170px; HEIGHT: 300px">
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF"  Width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<%--<asp:BoundColumn Visible="False" DataField="KeyID"></asp:BoundColumn>--%>
                                    <asp:BoundColumn Visible="False" DataField="ApplyCpInfoID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="TableFlag"></asp:BoundColumn>
                                     <asp:BoundColumn Visible="False" DataField="SPID"></asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" HeaderText="��˾����">
										<HeaderStyle Width="200px"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="WebName" HeaderText="��վ����">
									    <HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ContactUser" HeaderText="��ϵ��">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="UserName" HeaderText="������">
										<HeaderStyle Width="50px"></HeaderStyle>
									</asp:BoundColumn>
									<%--<asp:BoundColumn DataField="BargainCode" HeaderText="��ͬ��">
										<HeaderStyle Width="170px"></HeaderStyle>
									</asp:BoundColumn>--%>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="�ύʱ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FlagName" HeaderText="��ǰ״̬">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="�鿴" CommandName="Select">
										<HeaderStyle Width="30px"></HeaderStyle>
									</asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="5"
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				   </TR>
				</table>
			</div>
			<div id="divInfo" style="LEFT: 5%; WIDTH: 820px; POSITION: absolute; TOP: 400px; HEIGHT: 600px"
				runat="server">
				<table cellSpacing="1" cellPadding="1" width="820" align="center" border="1">
					<tr>
						<TD align="left" width="150"><asp:label id="Label16" runat="server">�̻���</asp:label></TD>
						<td align="left" width="250"><asp:label id="lblFspid" runat="server"></asp:label></td>
						<TD align="left" width="150"><asp:label id="Label5" runat="server">��վ����</asp:label></TD>
						<td align="left" width="250"><asp:label id="lblURL" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label6" runat="server">��ҵ���</asp:label></TD>
						<td align="left"><asp:label id="lblType" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label7" runat="server">���������֤</asp:label></TD>
						<td align="left"><asp:label id="lblFsidID" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label17" runat="server">�ʻ����</asp:label></TD>
						<td align="left"><asp:label id="lblFBalance" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label20" runat="server">�̻���������</asp:label></TD>
						<td align="left"><asp:label id="lblPickType" runat="server"></asp:label></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label9" runat="server">��������</asp:label></TD>
						<td align="left"><asp:label id="lblArea" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label18" runat="server">����BD</asp:label></TD>
						<td align="left"><asp:label id="lblBDName" runat="server"></asp:label></td>
					</tr>

                    <tr>
						<TD align="left"><asp:label id="Label22" runat="server">������ʽ</asp:label></TD>
						<td align="left"><asp:label id="txtBDSpidType" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label25" runat="server">�Ƿ����΢�ű�֤��</asp:label></TD>
						<td align="left"><asp:label id="txtIsWXBeil" runat="server"></asp:label></td>
					</tr>
                     <tr>
						<TD align="left"><asp:label id="Label23" runat="server">��֤����</asp:label></TD>
						<td align="left"><asp:label id="txtWXBeil" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label27" runat="server">������ʽ</asp:label></TD>
						<td align="left"><asp:label id="txtInType" runat="server"></asp:label></td>
					</tr>
                     <tr>
						<TD align="left"><asp:label id="Label26" runat="server">�ͷ���ϵ������</asp:label></TD>
						<td align="left"><asp:label id="txtServiceUser" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label29" runat="server">�ͷ���ϵQQ��</asp:label></TD>
						<td align="left"><asp:label id="txtServiceQQ" runat="server"></asp:label></td>
					</tr>
                      <tr>
						<TD align="left"><asp:label id="Label28" runat="server">�ͷ���ϵ�绰</asp:label></TD>
						<td align="left"><asp:label id="txtServiceTel" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label31" runat="server">�ͷ���ϵEMAIL</asp:label></TD>
						<td align="left"><asp:label id="txtServiceEmail" runat="server"></asp:label></td>
					</tr>

					<tr>
						<TD align="left"><asp:label id="Label8" runat="server">��ϵ��</asp:label></TD>
						<td align="left"><asp:textbox id="txtConnetionName" runat="server" Enabled="False"></asp:textbox></td>
						<TD align="left"><asp:label id="Label11" runat="server">��ϵ�绰</asp:label></TD>
						<td align="left"><asp:textbox id="txtPhone" runat="server" Enabled="False"></asp:textbox></td>
					</tr>
					<tr>
						<TD align="left"><asp:label id="Label10" runat="server">��ϵ�ֻ�</asp:label></TD>
						<td align="left"><asp:textbox id="txtMobile" runat="server" Enabled="False"></asp:textbox></td>
						<TD align="left"><asp:label id="Label13" runat="server">Email</asp:label><FONT color="red">*</FONT></TD>
						<td align="left"><asp:label id="txtEmail" runat="server"></asp:label></td>
					</tr>
                   <%-- <tr>
						<TD align="left"><asp:label id="Label22" runat="server">�̻���ַ</asp:label></TD>
						<td align="left"><asp:label id="txtComAddr" runat="server"></asp:label></td>
						<TD align="left"><asp:label id="Label23" runat="server">��������</asp:label></TD>
						<td align="left"><asp:label id="txtPosCode" runat="server"></asp:label></td>
					</tr>--%>
					<tr>
                      <TD align="left"><asp:label id="Label15" runat="server">QQ</asp:label></TD>
						<td align="left"><asp:textbox id="txtQQNo" runat="server" Enabled="False"></asp:textbox></td>
						<TD align="left"><asp:label id="Label19" runat="server">�̻��ʻ�����</asp:label></TD>
						<td align="left"><asp:label id="lblAttType" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td align="left" rowSpan="4"><asp:label id="Label14" runat="server">���ϱ�ע</asp:label></td>
						<td align="left" colSpan="3" rowSpan="4"><asp:textbox id="txtRemark" Width="650" Height="70px" Runat="server" Enabled="False" TextMode="MultiLine"></asp:textbox></td>
					</tr>
					<tr>
					</tr>
					<tr>
					</tr>
					<tr>
					</tr>
					<tr>
						<td colspan="4" align="center"><asp:Button ID="btnEdit" Runat="server" Text="�޸�" onclick="btnEdit_Click"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnSave" Runat="server" Text="����" onclick="btnSave_Click"></asp:Button>&nbsp;&nbsp;<asp:Button id="btnCancel" Runat="server" Text="ȡ��" onclick="btnCancel_Click"></asp:Button></td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
