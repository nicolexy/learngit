<%@ Page language="c#" Codebehind="RefundDetails.aspx.cs" AutoEventWireup="True"  EnableViewStateMac = "True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>RefundDetails</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); BODY { BACKGROUND-IMAGE: none }
	        .style2 { FONT-FAMILY: "����"; FONT-SIZE: 20px; FONT-WEIGHT: bold }
	        .style5 { FONT-FAMILY: "����"; FONT-SIZE: 14px; FONT-WEIGHT: bold }
		    .style8
            {
                height: 24px;
                width: 279px;
            }
            .style24
            {
                width: 103px;
            }
            .style25
            {
                height: 24px;
                width: 103px;
            }
            .style28
            {
                width: 279px;
            }
            .style29
            {
                width: 90px;
            }
            .style30
            {
                height: 29px;
                width: 90px;
            }
            .style31
            {
                height: 31px;
                width: 90px;
            }
            .style32
            {
                width: 91px;
            }
            .style33
            {
                height: 24px;
                width: 91px;
            }
            .style34
            {
                height: 29px;
                width: 91px;
            }
		    .style36
            {
                height: 29px;
                width: 279px;
            }
		    .style37
            {
                width: 72px;
            }
            .style38
            {
                height: 29px;
                width: 72px;
            }
            .sybiv
            {
                width:100%;
             }
            .sycurbg
            {
                float:left; 
                width:40%; 
                background:#c0c0c0;
                margin-left:30px;
                }
            .syhistory
            {
                float:right; 
                width:35%; 
                background:#e6e6e6;
                margin-right:30px;

             }
		    .style39
            {
                width: 536px;
            }
		    .style40
            {
                width: 160px;
            }
		    .style41
            {
                width: 155px;
            }
		    #Button1 {
		    margin:0;
            }
		    </style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="1" width="75%" align="center" border="0">
				<tr>
					<td colSpan="4" height="41" style="HEIGHT: 41px">
						<div align="center">
							<p class="style2">�����˿������</p>
						</div>
					</td>
				</tr>
				<tr>
					<td height="341">&nbsp;
					</td>
					<td vAlign="top" colSpan="2">
						<DIV align="center">
							<table height="384" cellSpacing="0" cellPadding="0" width="800" align="center" border="0">
								<tr>
									<td bgColor="#000000">
										<table  cellSpacing="1" cellPadding="0" width="700" align="center" bgColor="#000000"border="0">
											<tr bgColor="#ffffff" style="HEIGHT: 24px" height="24">
												<td nowrap class="style29"> 
                                                <div align="center" ><strong><FONT size="2">&nbsp;�Ƹ�ͨ����</FONT></strong></div>
													
												</td>
												<td class="style28"  align = "center">
													<strong><asp:label id="lbUinID" runat="server"></asp:label></strong>
												</td>
												<td nowrap class="style32">
                                                    <div align="center"><STRONG><FONT size="2">����</FONT></STRONG></div>
													
												</td>
												<td align = "center" nowrap class="style37">
													<strong><asp:label id="lbUser" runat="server" Width = "230px" Height="16px"></asp:label></strong>
												</td>
                                                <td  align="center" nowrap class="style24" >
                                                    <div ><strong><FONT size="2">�����ַ</FONT></strong></div>
												</td>
												<td align = "center">
													<strong><asp:label id="lbMail" runat="server" Width = "240" style="margin-left: 0px"></asp:label></strong>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td class="style29" >
													<div align="center"><strong><FONT size="2">�����п���</FONT></strong></div>
												</td>
												<td class="style8">
													<div align="center"><strong><asp:label id="lbNewBank" runat="server" ></asp:label></strong></div>
												</td>
												<td class="style33">
													<P align="center"><strong><FONT size="2">���֤��</FONT></strong></P>
												</td>
												<td align="center" nowrap class="style37"><strong>
														<DIV align="center"><STRONG><asp:label id="lbIdentity" runat="server" Width = "230px" Height="16px"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                                <td class="style25">
													<P align="center"><STRONG><FONT size="2">��������</FONT></STRONG></P>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbNewBankType" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
                                            <tr bgColor="#ffffff">
												<td class="style29" >
													<div align="center"><strong><FONT size="2">�տ��ʺ�����</FONT></strong></div>
												</td>
												<td class="style8">
													<div align="center"><strong><asp:label id="lbUserFlag" runat="server" ></asp:label></strong></div>
												</td>
												<td class="style33">
													<P align="center"><strong><FONT size="2">�տ��˿�����</FONT></strong></P>
												</td>
												<td align="center" nowrap><strong>
														<DIV align="center"><STRONG><asp:label id="lbCardType" runat="server" Width = "230px" Height="16px"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                                <td class="style25">
													<P align="center"><STRONG><FONT size="2">������֧������</FONT></STRONG></P>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbBankName" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td height="29" class="style30">
													<div align="center"><strong><FONT size="2">����ʱ��</FONT></strong></div>
												</td>
												<td class="style8"><strong>
														<DIV align="center"><STRONG><asp:label id="lbCreateTime" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                                <td height="29" class="style34" >
													<div align="center"><strong><FONT size="2">���ж�����</FONT></strong></div>
												</td>
												<td align="center" class="style37"><strong>
														<DIV align="center"><STRONG><asp:label id="lbBankListID" runat="server" Width = "230px" Height="16px"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                                <td height="29" class="style25">
													<div align="center"><strong><FONT size="2">ԭ��������</FONT></strong></div>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbInitBankName" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td height="31" class="style31">
													<div align="center"><strong>ԭ���п���</strong></div>
												</td>
                                                <td class="style8"><strong>
														<DIV align="center"><STRONG><asp:label id="lbInitBank" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                     
                                                 <td  align="center" class="style24">
													<div align="center"><strong><FONT size="2">�޸�ԭ��</FONT></strong></div>
												</td>
												<td style="HEIGHT: 24px" colspan = "3" >
                                                    <asp:TextBox ID="txt_Reason" runat="server" Enabled="false" TextMode="MultiLine" width="100%" Height="40"></asp:TextBox>

                                                    <%--<strong><DIV align="center"><STRONG><asp:label id="lbReason" runat="server" ></asp:label></STRONG></DIV></strong>--%>													
												    <asp:Button ID="Button1" runat="server" Visible="false" Text="�����޸�" OnClick="Button1_Click"/>
												</td>
											</tr>
                                            <tr bgColor="#cccccc">
												<td colSpan="6" height="25">
                                                    <div style="padding:4px;">
												        <asp:Repeater runat="server" ID="RelatedOrder">
                                                            <ItemTemplate>
                                                                <div>
                                                                    <a href="#">999999999999999999999999</a>
                                                                </div>
                                                            </ItemTemplate>
												        </asp:Repeater>
                                                    </div>
												</td>
											</tr>
                                            <tr bgColor="#cccccc">
                                                <td  height="25">
                                                    <div class="style5" align="left">��̨����</div>
                                                </td>
                                                 <td colspan="5" height="25">
                                                    <strong >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ԭ�����ֻ�:  <asp:label id = "lPhone" runat = "server"  Width = "190px" Height="16px"></asp:label>
                                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ����:  <asp:Label ID="ltruename" runat="server" Width = "150px" Height="16px"></asp:Label>&nbsp;&nbsp; ���֤����: <asp:Label ID="lIdentity" runat="server" Width = "237px" Height="16px" style="margin-top: 1px"></asp:Label>
                                                     </strong>
                                                  </td>
                                            </tr>
											<tr bgColor="#cccccc">
												<td colSpan="3" height="25">
													<div class="style5" align="left">��ŵ��</div>
												</td>
                                                <td colSpan="3" height="25">
													<div class="style5" align="left">����֤��</div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td colSpan="3">
													<div align="center">
                                                        <asp:image id="igCommitment" runat="server" Width="450px" 
                                                            Height="220px"></asp:image></div>
												</td>
                                                	<td colSpan="3">
													<div align="center">
                                                        <asp:image id="igAccount" runat="server" Width="500px" 
                                                            Height="220px"></asp:image></div>
												</td>
											</tr>

											<tr bgColor="#ffffff">
												<td bgColor="#cccccc" colSpan="3" height="30"  align="left" ><span class="style5">���֤ͼƬ</span></td>
                                                <td bgColor="#cccccc" colSpan="3" height="30" align="left" ><span class="style5">���к�ʵ���</span></td>
											</tr>

											<tr bgColor="#ffffff">
												<td colSpan="3">
													<div align="center"><asp:image id="igIdentity" runat="server" Width="450px" 
                                                            Height="220px"></asp:image></div>
												</td>
                                                 <td colSpan="3">
													<div align="center"><asp:image id="igBankWater" runat="server" Width="500px" Height="220px"></asp:image></div>
												</td>
                                            </tr>
                                            <tr bgColor="#cccccc">
                                                <td class="style30"  >
                                                <div align="center"><strong><FONT size="2">�ܾ�ԭ��</FONT></strong></div>
													
												</td>
												<td class="style36">
													<asp:DropDownList ID="dropReasonList" runat="server" Width = "150"  OnSelectedIndexChanged="OnDropDownList_SelectedIndexChanged" AutoPostBack ="true">
                                                    <asp:ListItem Value="-1" Selected="True">��ѡ��ܾ�ԭ��</asp:ListItem>
                                                    <asp:ListItem Value="0" >�����˺�״̬�쳣</asp:ListItem>
                                                    <asp:ListItem Value="1">������Ϣ����ȷ</asp:ListItem>
						                            <asp:ListItem Value="2">ȱ�������ۿ��ͼ</asp:ListItem>
						                            <asp:ListItem Value="3">ǩ������ӡ������Ҫ��</asp:ListItem>
						                            <asp:ListItem Value="4">��ŵ����д������Ҫ��</asp:ListItem>
						                            <asp:ListItem Value="5">��ŵ����Ϳ��</asp:ListItem>
						                            <asp:ListItem Value="6">���֤ɨ���������</asp:ListItem>
						                            <asp:ListItem Value="7">��ŵ��ɨ���������</asp:ListItem> 
                                                    </asp:DropDownList>
                                                
												</td>                                                                                          
                                                <td class="style38" >
                                                <div ><strong><FONT size="2">�˹���д��</FONT></strong></div>
												
												</td>
												<td   nowrap colspan = "3"  >					
                                                    <strong><asp:textbox id="lbWriteReason"   runat="server"  Width = "569px" Height="28px"></asp:textbox></strong>
												</td>
                                            </tr>
										</table>
									</td>
								</tr>
							</table>
						</DIV>
					</td>
					<td>&nbsp;</td>
				</tr>
			</table>
			<div align="center"><FONT face="����">

				<br />
					<asp:button id="btnCommit" runat="server" Width="90px" Height="30px" Text="����ͨ��" BorderStyle="Groove" onclick="btnPass_Click"></asp:button>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:button id="btnRefuse" runat="server" Width="90px" Height="30px" Text="�����ܾ�" BorderStyle="Groove" onclick="btnRefuse_Click"></asp:button>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:button id="btnCW" runat="server" Width="90px" Height="30px" Text="ת�������" BorderStyle="Groove" onclick="btnTransferCW_Click"></asp:button>
          
                <br />
                <br />
           </FONT></div>
           <div clsaa="sybiv">
           <table border = "0" class ="sycurbg" >
           <tr>
            <td style="width:150px;"><div ><strong><FONT size="3">��ǰ��˼�¼��</FONT></strong></div></td>
            <td colspan="3"><div ><strong><asp:label ID ="kfOperator" runat ="server" ></asp:label></strong></div></td>
           </tr>
           <asp:Repeater runat="server" ID="rep_checkLogList">
               <ItemTemplate>
                    <tr style="height:25px;">
                       <td><strong><%#Eval("CheckTypeName") %>��</strong></td>
                       <td class="style40" ><strong><%#Eval("FCheckTime") %></strong></td>
                       <td class="style41" ><strong><%#Eval("FCheckuser") %></strong></td>
                       <td ><strong><%#Eval("FCheckMemo") %></strong></td>
                    </tr>
               </ItemTemplate>
           </asp:Repeater>
       
 <%--          <tr height = "25px">
           <td ><strong>BG��ˣ�</strong></td>
           <td class="style40" ><strong><asp:label ID ="lbBgCheckTime" runat ="server" Width="150px"></asp:label></strong></td>
           <td class="style41" ><strong><asp:label ID ="lbBgCheckName" runat ="server" 
                   Width="152px"></asp:label></strong></td>
           <td height = "25px"><strong><asp:label ID ="lbBgCheckReason" runat ="server" Width="410px" ></asp:label></strong></td>
           </tr>

           <tr height = "25px">
           <td nowrap><strong>�����ˣ�</strong></td>
           <td class="style40" ><strong><asp:label ID ="lbFengCheckTime" runat ="server" Width="150px"></asp:label></strong></td>
           <td class="style41" ><strong><asp:label ID ="lbFengCheckName" runat ="server" 
                   Width="154px"></asp:label></strong></td>
           <td ><strong><asp:label ID ="lbFengCheckReason" runat ="server" Width="406px" ></asp:label></strong></td>
           </tr>--%>
           </table>
            <table class ="syhistory">
           <tr>
            <td colspan = "3" class="style39"><div ><strong><FONT size="3">��ʷ��˼�¼��</FONT></strong></div></td>
           </tr>
            <tr height = "65px">
                <td class="style39" ><strong><asp:label ID ="lbHistory1" runat ="server" 
                        Width="597px" Height = "75px" ></asp:label></strong></td>
           </tr>
            </table>
            </div>
		</form>
	</body>
</HTML>
