<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="GetFundRatePageDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.GetFundRatePageDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>�ͷ�ϵͳǿ��</title>
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
			<TABLE id="tableCloseInput" runat="server" style="LEFT: 5%; POSITION:relative;top:50px;" cellSpacing="1" cellPadding="1" width="800"
				border="0">
                <TR>
                    <TD align="right"><asp:label id="lb" runat="server">��ֹ��־��</asp:label></TD>
                    <TD><asp:textbox id="txtEndDate" runat="server"></asp:textbox><Font color="red">*</Font></TD>
                    <TD align="right"><asp:label id="Label1" runat="server">����˾ƾ֤��</asp:label></TD>
                    <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="����"> </FONT><INPUT id="FileClose" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="FileClose"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
                </TR>
                 <TR>
                     <TD align="center"><asp:button id="BtnCreateApplyClose" runat="server" Width="80px" Text="�������뵥" onclick="btnCreateApplyClose_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE id="tableCloseApply" runat="server" style="LEFT: 5%; POSITION:relative;top:80px;" cellSpacing="1" cellPadding="1" width="800"
				border="1">
                <tr><th colspan="4">����ǿ�������</th></tr>
                 <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">���ͨ�˺ţ�</asp:label></TD>
                     <TD><asp:label id="tb_Cuin" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label5" runat="server">�˻����ͣ�</asp:label></TD>
                     <TD><asp:label id="tb_Cacct_type" runat="server">΢���û�</asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label7" runat="server">����˾�̻��ţ�</asp:label></TD>
                     <TD><asp:label id="tb_Cspid" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label9" runat="server">������룺</asp:label></TD>
                     <TD><asp:label id="tb_Cfund_code" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label11" runat="server">��ؽ�</asp:label></TD>
                     <TD><asp:label id="tb_Ctotal_fee" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label13" runat="server">��صĶ��ڻ����ֹ�գ�</asp:label></TD>
                     <TD><asp:label id="tb_Cend_date" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                    <TD align="right"><asp:label id="Label15" runat="server">������</asp:label></TD>
                     <TD><asp:label id="tb_Cchannel_id" runat="server">�ͷ�����</asp:label></TD>
                     <TD align="right"><asp:label id="Label17" runat="server">�ͷ��ֹ������ֹ���ڣ�</asp:label></TD>
                     <TD><asp:label id="tb_Cend_dateHand" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                    <TD align="right"><asp:label id="Label19" runat="server">�����;��</asp:label></TD>
                     <TD><asp:label id="tb_Credem_type" runat="server">�ͷ�ǿ�����</asp:label></TD>
                     <TD align="right"><asp:label id="Label21" runat="server">�û���IP��ַ��</asp:label></TD>
                     <TD><asp:label id="tb_Cclient_ip" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="left" colspan="4"><asp:label id="Label23" runat="server"><font color="red">����˾ƾ֤��</font></asp:label></TD>
				</TR>
               <tr>
					<td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="ImageC" runat="server"></asp:image>
                    </td>
			</tr>
             <TR>
                   <TD align="center" colspan="4"><asp:button id="ButtonSubmitClose" runat="server" Width="200px" Text="ȷ��������ύ����" onclick="btnSubmitCloseApply_Click"></asp:button></TD>
			</TR>
			</TABLE>
			<%--<TABLE id="tableUNCloseInput"  runat="server" style="LEFT: 5%; POSITION:relative;top:50px;" cellSpacing="1" cellPadding="1" width="800"
				border="1">
                <TR>
                    <TD align="right"><asp:label id="Label2" runat="server">������������</asp:label></TD>
                    <TD><asp:textbox id="tbtrue_name_new" runat="server"></asp:textbox><Font color="red">*</Font></TD>
               </TR>
               <TR>
                    <TD align="right"><asp:label id="Label6" runat="server">�������˺ţ�</asp:label></TD>
                    <TD><asp:textbox id="tbbank_id_new" runat="server"></asp:textbox><Font color="red">*</Font></TD>
               </TR>
               <TR>
                    <TD align="right"><asp:label id="Label8" runat="server">���֤���룺</asp:label></TD>
                    <TD><asp:textbox id="tbcert_id" runat="server"></asp:textbox><Font color="red">*</Font></TD>
               </TR>
               <TR>
                    <TD align="right"><asp:label id="Label10" runat="server">�¿������к�֧�����ƣ�</asp:label></TD>
                    <TD><asp:textbox id="tbbank_type_new" runat="server"></asp:textbox><Font color="red">*</Font></TD>
              </TR>
               <TR>
                    <TD align="right"><asp:label id="Label12" runat="server">���ڳ��У�ʡ���У���</asp:label></TD>
                    <TD><asp:textbox id="tbaddress" runat="server"></asp:textbox><Font color="red">*</Font></TD>
               </TR>
               <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">��ŵ����</asp:label></TD>
                    <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="����"> </FONT><INPUT id="FileUNClose1" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="FileUNClose1"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font>
							<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Display="Dynamic" ControlToValidate="FileUNClose1"
								></asp:requiredfieldvalidator>&nbsp; </SPAN>
					</td>
              </TR>
              <TR>
                    <TD align="right"><asp:label id="Label14" runat="server">���֤ɨ�裺</asp:label></TD>
                    <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="����"> </FONT><INPUT id="FileUNClose2" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="FileUNClose2"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font>
							<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="FileUNClose2"
								></asp:requiredfieldvalidator>&nbsp; </SPAN>
					</td>
              </TR>
              <TR>
                    <TD align="right"><asp:label id="Label16" runat="server">���п�ɨ�����</asp:label></TD>
                    <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="����"> </FONT><INPUT id="FileUNClose3" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="FileUNClose3"
							runat="server">
					</td>
              </TR>
              <TR>
                    <TD align="right"><asp:label id="Label18" runat="server">������</asp:label></TD>
                    <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="����"> </FONT><INPUT id="FileUNClose4" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="FileUNClose4"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font>
							<asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" Display="Dynamic" ControlToValidate="FileUNClose4"
								></asp:requiredfieldvalidator>&nbsp; </SPAN>
					</td>
              </TR>
              <TR>
                     <TD align="center" colspan="2"><asp:button id="btnCreateApplyUNClose" runat="server" Width="80px" Text="�������뵥" onclick="btnCreateApplyUNClose_Click"></asp:button></TD>
			 </TR>
			</TABLE>--%>
            	<TABLE id="tableUNCloseApply" runat="server" style="LEFT: 5%; POSITION:relative;top:80px;" cellSpacing="1" cellPadding="1" width="800"
				border="1">
                <tr><th colspan="4">����ǿ�������</th></tr>
                 <TR>
                    <TD align="right"><asp:label id="Label20" runat="server">���ͨ�˺ţ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCuin" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label24" runat="server">������ͣ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCredem_type" runat="server">t+0</asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label26" runat="server">����˾�̻��ţ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCspid" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label28" runat="server">������룺</asp:label></TD>
                     <TD><asp:label id="tb_UNCfund_code" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label30" runat="server">��ؽ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCtotal_fee" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label32" runat="server">client_ip��</asp:label></TD>
                     <TD><asp:label id="tb_UNCclient_ip" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                    <TD align="right"><asp:label id="Label34" runat="server">�����кţ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCbind_serialno" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label36" runat="server">��β�ţ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCcard_tail" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                      <TD align="right"><asp:label id="Label2" runat="server">�������ͣ�</asp:label></TD>
                     <TD><asp:label id="tb_UNCbank_type" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label40" runat="server">�ֻ��ţ�</asp:label></TD>
                     <TD><asp:label id="tb_UNmobile" runat="server"></asp:label></TD>
				</TR>
             <TR>
                   <TD align="center" colspan="4"><asp:button id="btnSubmitUNClose" runat="server" Width="200px" Text="ȷ��������ύ����" onclick="btnSubmitUNCloseApply_Click"></asp:button></TD>
			</TR>
			</TABLE>
		</form>
	</body>
</HTML>
