<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="GetFundRatePageDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.GetFundRatePageDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title id="title" runat="server">�ͷ�ϵͳǿ��</title>
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
		<form id="Form1" method="post" runat="server" >
            <div style="width:800px; margin:0 auto; " >
			<TABLE id="tableCloseInput" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false"
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
			<TABLE id="tableCloseApply" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false"
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
            <TABLE id="tableUNCloseApply" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false"
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

            <TABLE id="tbLCTinput" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false"
				border="0">
                <TR>
                     <TD align="right"><asp:label id="Label25" runat="server">������</asp:label></TD>
                    <td style="HEIGHT: 23px"  >&nbsp;<FONT face="����"> </FONT>
                        <asp:textbox id="txtLCTDescription" runat="server"></asp:textbox>&nbsp;<SPAN class="style5"><Font color="red"></Font></SPAN>
					</td>

                    <TD align="right"><asp:label id="Label18" runat="server">����˾ƾ֤��</asp:label></TD>
                    <td style="HEIGHT: 23px"  >&nbsp;<FONT face="����"> </FONT><INPUT id="FileLCT" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="FileLCT"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red"></Font></SPAN>
					</td>
                </TR>
                 <TR>
                     <TD align="center" colspan="2"><asp:button id="btnCreateLCTFund" runat="server" Width="80px" Text="�������뵥" onclick="btnCreateLCTFund_Click"></asp:button></TD>
				</TR>
			</TABLE>
            <TABLE id="tbLCT" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false"
				border="1">
                <tr><th colspan="4">���ͨ���ǿ��</th></tr>
                 <TR>
                    <TD align="right"><asp:label id="Label4" runat="server">���ͨ�˺ţ�</asp:label></TD>
                     <TD><asp:label id="lblLCT_uin" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label8" runat="server">�˻����ͣ�</asp:label></TD>
                     <TD><asp:label id="Label10" runat="server">΢���û�</asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label22" runat="server">���ֽ��(��)��</asp:label></TD>
                     <TD><asp:label id="lblLCT_total_fee" runat="server"></asp:label></TD>
                     <TD align="right"> <asp:label id="Label44" runat="server">Client IP��</asp:label></TD>
                     <TD><asp:label id="lblLCTclient_ip" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                    <TD align="right"><asp:label id="Label6" runat="server">��ȫ�������кţ�</asp:label></TD>
                     <TD><asp:label id="lblLCT_bind_serialno" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label14" runat="server">��ȫ���������ͣ�</asp:label></TD>
                     <TD><asp:label id="lblLCT_bank_type" runat="server"></asp:label></TD>
				</TR>

                  <TR>
                    <TD align="right"><asp:label id="Label12" runat="server">��β�ţ�</asp:label></TD>
                     <TD><asp:label id="lblLCT_card_tail" runat="server"></asp:label></TD>
                     <TD align="right"><asp:label id="Label16" runat="server">������</asp:label></TD>
                     <TD><asp:label id="lblLCTDescription" runat="server"></asp:label></TD>
				</TR> 
                 <TR>
                    <TD align="left" colspan="4"><asp:label id="Label43" runat="server"><font color="red">����˾ƾ֤��</font></asp:label></TD>
				</TR>
               <tr>
					<td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"> <asp:Image id="imgLCT"   runat="server" />
                    </td>
			</tr>
             <TR>
                   <TD align="center" colspan="4"><asp:button id="btnLCTFund" runat="server" Width="200px" Text="ȷ��������ύ����" onclick="btnLCTFundApply_Click"></asp:button></TD>
			</TR>
			</TABLE>
            <TABLE id="fRate_table" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false"
				border="1">
                <tr><th colspan="4">ָ�������ǿ��</th></tr>
                 <TR>
                    <TD align="right">���ͨ�˺ţ�</TD>
                     <TD><asp:label id="fRate_uin" runat="server"></asp:label></TD>
                     <TD align="right">������ͣ�</TD>
                     <TD><asp:label id="fRate_type" runat="server">t+0</asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right">����˾�̻��ţ�</TD>
                     <TD><asp:label id="fRate_spid" runat="server"></asp:label></TD>
                     <TD align="right">������룺</TD>
                     <TD><asp:label id="fRate_fund_code" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right">��ؽ�</TD>
                     <TD><asp:label id="fRate_total_fee" runat="server"></asp:label></TD>
                     <TD align="right">client_ip��</TD>
                     <TD><asp:label id="fRate_client_ip" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                    <TD align="right">�����кţ�</TD>
                     <TD><asp:label id="fRate_bind_serial_no" runat="server"></asp:label></TD>
                     <TD align="right">��β�ţ�</TD>
                     <TD><asp:label id="fRate_crad_tail" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                      <TD align="right">�������ͣ�</TD>
                     <TD><asp:label id="fRate_bank_type" runat="server"></asp:label></TD>
                     <TD align="right">�ֻ��ţ�</TD>
                     <TD><asp:label id="fRate_mobile" runat="server"></asp:label></TD>
				</TR>
             <TR>
                   <TD align="center" colspan="4"><asp:button id="Button1" runat="server" Width="200px" Text="ȷ��������ύ����" OnClick="Button1_Click" ></asp:button></TD>
			</TR>
			</TABLE>
            <table id="AlterES_table" runat="server" style=" " cellSpacing="1" cellPadding="1" width="800" visible="false" border="1">
                 <tr>
                    <th  colspan="4" align="center">�û�ָ���ĵ����깺/��ز��Ժ͵��ڲ����޸�</th>
                </tr>  
                  <tr>
                    <td  align="right">���ͨ�˺ţ�</td>
                    <td>
                        <asp:Label ID="AlterES_uin" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td  align="right">�������˻���Ӧid��</td>
                    <td>
                        <asp:Label ID="AlterES_Trade_id" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  align="right">������룺</td>
                    <td>
                        <asp:Label ID="AlterES_Fund_code" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td  align="right">�������ƣ�</td>
                    <td>
                        <asp:Label ID="AlterES_fund_name" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>    
                <tr>
                    <td  align="right">�û�ָ���ĵ����깺/��ز��ԣ�</td>
                    <td>
                        <select runat="server" id="AlterES_user_end_type">
                           <%--<option value="1">ָ����ؽ�����ȫ���깺</option>--%>
                           <option value="2">ȫ����� </option>
                           <option value="3">ȫ��˳������һ��</option>
                           <%--<option value="4">ָ���깺������ȫ����أ�ɨβ��أ�</option>--%>
                        </select>   
                    </td>
                    <td  align="right">���ڲ�����</td>
                    <td>
                        <select  runat="server" id="AlterES_end_sell_type">
                           <option value="0">0</option>
                           <option value="1">����������ֵ����п�</option>
                           <%--<option value="2">�������ת����һֻ����</option>--%>
                           <option value="3">�������ת����˻�</option>
                           <%--<option value="4">ָ���깺������ȫ����أ�ɨβ��أ�</option>--%>
                        </select>   
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="AlterEndStrategy" runat="server" Text="�����޸�" OnClick="AlterEndStrategy_Click" />
                    </td> 
                </tr>
			</table>
         </div>   	
		</form>
	</body>
</HTML>
