<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryAuthenStateInfoPageDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryAuthenStateInfoPageDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>QueryAuthenStateInfoPageDetail</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	</style>
		<script src="../SCRIPTS/Local.js"></script>
</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: ����" id="Form1" method="post" runat="server">
            <TABLE id="tableCreate" runat="server" style="LEFT: 5%; POSITION:relative;top:30px;" cellSpacing="1" cellPadding="1" width="800"
				border="1">
                 <tr>
                    <td style="width: 100%" bgcolor="#e4e5f7" colspan="4" align="center">
                        <font size="4">
                           ʵ����֤�쳣�����</font>
                    </td>
                </tr>
                 <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">ԭ������</asp:label></TD>
                     <TD><asp:label id="tb_nameOld" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label5" runat="server">ԭ֤���ţ�</asp:label></TD>
                     <TD><asp:label id="tb_certifyNoOld" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label7" runat="server">��������</asp:label></TD>
                     <TD><asp:label id="tb_name" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label9" runat="server">��֤���ţ�</asp:label></TD>
                     <TD><asp:label id="tb_certifyNo" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label8" runat="server">���֤��Ч������</asp:label></TD>
                     <TD><asp:label id="tbx_cerDate" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label11" runat="server">������ϵ��ַ��</asp:label></TD>
                     <TD><asp:label id="tb_address" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label13" runat="server">֤�����ͣ�</asp:label></TD>
                     <TD><asp:label id="tb_cre_type" runat="server">���֤</asp:label></TD>
                     <TD align="right"><asp:label id="Label15" runat="server">���֤�汾��</asp:label></TD>
                     <TD><asp:label id="tb_cre_version" runat="server"></asp:label></TD>
				</TR>
               <TR>
                    <TD align="left" colspan="4"><asp:label id="Label23" runat="server"><font color="red">���֤���棺</font></asp:label></TD>
				</TR>
               <TR> 
                     <td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="ImageF" runat="server"></asp:image>
                    </td>
                </TR>
                <TR>
                     <TD align="left" colspan="4"><asp:label id="Label1" runat="server"><font color="red">���֤���棺</font></asp:label></TD>
						</TR>
               <TR> 
                     <td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="ImageR" runat="server"></asp:image>
                    </td>
			 </TR>
             <TR>
                    <TD align="left" colspan="4"><asp:label id="Label2" runat="server"><font color="red">����ƾ֤��</font></asp:label></TD>
				 </TR>
                <TR>	 
                 <td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="ImageO" runat="server"></asp:image>
                    </td>
			</TR>
                  <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">�ܾ�ԭ��</asp:label></TD>
                    <TD colspan="3"><asp:dropdownlist id="ddlRefuseReason" runat="server" Width="150px">
							<asp:ListItem Value="0">��ѡ��ܾ�ԭ��</asp:ListItem>
							<asp:ListItem Value="1">֤��ɨ���������</asp:ListItem>
							<asp:ListItem Value="2">����ƾ֤������</asp:ListItem>
						</asp:dropdownlist>
                    </TD> 
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label6" runat="server">�˹���д��</asp:label></TD>
                   <TD colspan="3"><asp:TextBox ID="tb_comment" Runat="server" TextMode="MultiLine" Width="500px"></asp:TextBox></td>
				</TR>

             <TR>
                   <TD align="center" colspan="4">
                       <asp:button id="ButtonOK" Visible="false" runat="server" Width="200px" Text="ͨ��" onclick="btnOK_Click"></asp:button>
                       <asp:button id="ButtonNO" Visible="false" runat="server" Width="200px" Text="�ܾ�" onclick="btnNO_Click"></asp:button>
                   </TD>
			</TR>
			</TABLE>
		</form>
	</body>
</HTML>
