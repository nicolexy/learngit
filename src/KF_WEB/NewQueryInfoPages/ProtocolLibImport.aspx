<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ProtocolLibImport.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.ProtocolLibImport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComplainBussinessInput</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="2"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�������Э��</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">�̻��ţ�</asp:label></TD>
                    <TD><asp:textbox id="bussId" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">�ϴ�Э�������ļ���</asp:label></TD>
                    <TD ><asp:FileUpload id="File1" runat="server"/></TD>
				</TR>
				<TR>
                    <TD align="center" colspan="4"><asp:button id="btnUpload" runat="server" Width="80px" Text="�� ��" onclick="btnUpload_Click"></asp:button>
				</TR>
                <tr>
                    <td colspan="4">
                        <asp:label id="Label3" runat="server">�������кţ�</asp:label><asp:label id="Fpz_seq" runat="server"></asp:label>
                        &nbsp;&nbsp;<asp:label id="Label7" runat="server">��Э������</asp:label><asp:label id="Fproto_count" runat="server"></asp:label>
                        &nbsp;&nbsp;<asp:label id="Label9" runat="server">����˵����</asp:label><asp:label id="Fpz_memo" runat="server"></asp:label>
                        &nbsp;&nbsp;<asp:label id="Label11" runat="server">Э����֤��ʽ��</asp:label><asp:label id="Fproto_veri" runat="server"></asp:label>
                    </td>
                </tr>
			</TABLE>
			<TABLE id="Table2" visible="false" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 35%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fseq" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbuss_proto_flag" HeaderText="�̻���Э���ʶ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbuss_user_flag" HeaderText="�̻����û���ʶ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Facc_att" HeaderText="�˻�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fname" HeaderText="����/��˾����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fphone" HeaderText="�ֻ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fopenacc_cer_type" HeaderText="����֤������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fopenacc_cer_id" HeaderText="����֤������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_name" HeaderText="��������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Facc_type" HeaderText="�˺�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_acc" HeaderText="�����˺�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fprovince" HeaderText="ʡ������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbranch_name" HeaderText="֧������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcre_card_valid" HeaderText="���ÿ���Ч��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcre_card_cvv2" HeaderText="���ÿ�cvv2"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcft_no" HeaderText="�Ƹ�ͨ�˺�"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbuss_code" HeaderText="ҵ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fpay_type" HeaderText="֧����ʽ"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_start_time" HeaderText="Э����ʼ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_end_time" HeaderText="Э�������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_brief_desc" HeaderText="Э���Ҫ˵��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fproto_detail_desc" HeaderText="Э����ϸ˵��"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
                <tr>
                  <td align="center"><asp:button id="btnConfirm" runat="server" Width="80px" Text="ȷ ��" onclick="btnConfirm_Click"></asp:button></td>
                </tr>
			</TABLE>
            
		</form>
	</body>
</HTML>
