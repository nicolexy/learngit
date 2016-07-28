<%@ Page language="c#" Codebehind="CreateAppForm.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.CreateAppForm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>CreateAppForm</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); BODY { BACKGROUND-IMAGE: none }
	        .style2 { FONT-FAMILY: "黑体"; FONT-SIZE: 20px; FONT-WEIGHT: bold }
	        .style5 { FONT-FAMILY: "黑体"; FONT-SIZE: 14px; FONT-WEIGHT: bold }
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
		    </style>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="1" width="75%" align="center" border="0">
				<tr>
					<td colSpan="4" height="41" style="HEIGHT: 41px">
						<div align="center">
							<p class="style2"><asp:label id="lbHeadID" runat="server">特殊退款申请表</asp:label></p>
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
                                            <tr bgColor="#ffffff" style="HEIGHT: 24px" height="40">
                                                <td colspan = "6">
                                                <strong><FONT size="2">&nbsp;财付通订单:</FONT></strong>
                                                <asp:textbox id="tbUinID" runat="server" Width="900px" height="50" BorderWidth="1px"  TextMode="MultiLine"  Enabled ="false"></asp:textbox>
                                                </td>                                                                        
                                            </tr>
                                           <tr bgColor="#ffffff" style="HEIGHT: 24px" height="40">
                                                <td colspan = "6">
                                                <strong><FONT size="2">&nbsp;银行订单号:&nbsp;  </FONT></strong>
                                                <asp:textbox id="tbBankListID" runat="server" Width="900px" height="50" BorderWidth="1px"  TextMode="MultiLine"  Enabled ="false"></asp:textbox>
                                                </td>                                                                        
                                            </tr>
                                            <tr bgColor="#ffffff" style="HEIGHT: 24px" height="40">
                                                <td colspan = "6">
                                                <strong><FONT size="2">&nbsp;申请时间:</FONT></strong>
                                                <asp:textbox id="tbCreateTime" runat="server" Width="900px" height="50" BorderWidth="1px"  TextMode="MultiLine"  Enabled ="false"></asp:textbox>
                                                </td>                                                                        
                                            </tr>
											<tr bgColor="#ffffff" style="HEIGHT: 24px" height="24">
							
												<td nowrap class="style32">
                                                    <div align="center"><STRONG><FONT size="2">银行户名</FONT></STRONG></div>
													
												</td>
												<td align = "center" nowrap>
													<strong><asp:label id="lbBankUserName" runat="server" Width = "230px" Height="16px"></asp:label></strong>
												</td>
                                                <td  align="center" nowrap class="style24" >
                                                    <div ><strong><FONT size="2">邮箱地址</FONT></strong></div>
												</td>
												<td align = "center">
													<strong><asp:label id="lbMail" runat="server" Width = "240" style="margin-left: 0px"></asp:label></strong>
												</td>
                                                 <td height="29" class="style25">
													<div align="center"><strong><FONT size="2">原银行类型</FONT></strong></div>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbInitBankType" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td class="style29" >
													<div align="center"><strong><FONT size="2">新银行卡号</FONT></strong></div>
												</td>
												<td class="style8">
													<div align="center"><strong><asp:label id="lbNewBankAccNo" runat="server" ></asp:label></strong></div>
												</td>
												<td class="style33">
													<P align="center"><strong><FONT size="2">身份证号</FONT></strong></P>
												</td>
												<td align="center" nowrap><strong>
														<DIV align="center"><STRONG><asp:label id="lbIdentity" runat="server" Width = "230px" Height="16px"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                                <td class="style25">
													<P align="center"><STRONG><FONT size="2">新银行类型</FONT></STRONG></P>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbNewBankType" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
                                            <tr bgColor="#ffffff">
												<td class="style29" >
													<div align="center"><strong><FONT size="2">收款帐号类型</FONT></strong></div>
												</td>
												<td class="style8">
													<div align="center"><strong><asp:label id="lbUserFlag" runat="server" ></asp:label></strong></div>
												</td>
												<td class="style33">
													<P align="center"><strong><FONT size="2">收款人卡类型</FONT></strong></P>
												</td>
												<td align="center" nowrap><strong>
														<DIV align="center"><STRONG><asp:label id="lbCardType" runat="server" Width = "230px" Height="16px"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                                <td class="style25">
													<P align="center"><STRONG><FONT size="2">开户行支行名称</FONT></STRONG></P>
												</td>
												<td style="HEIGHT: 24px"><strong>
														<DIV align="center"><STRONG><asp:label id="lbBankName" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td height="31" class="style31">
													<div align="center"><strong>原银行卡号</strong></div>
												</td>
                                                <td class="style8"><strong>
														<DIV align="center"><STRONG><asp:label id="lbInitBankAccNo" runat="server"></asp:label></STRONG></DIV>
													</strong>
												</td>
                                     
                                                 <td  align="center" class="style24">
													<div align="center"><strong><FONT size="2">修改原因</FONT></strong></div>
												</td>
												<td style="HEIGHT: 24px" colspan = "3" >
                                                    <strong><DIV align="center"><STRONG><asp:label id="lbReason" runat="server" ></asp:label></STRONG></DIV></strong>													
												</td>
											</tr>
											<tr bgColor="#cccccc">
												<td colSpan="3" height="25">
													<div class="style5" align="left">承诺涵</div>
												</td>
                                                <td colSpan="3" height="25">
													<div class="style5" align="left">销户证明</div>
												</td>
											</tr>
											<tr bgColor="#ffffff">
												<td colSpan="3">
													<div align="center">
                                                        <asp:image id="igCommitment" runat="server" Width="450px" 
                                                            Height="250px"></asp:image></div>
												</td>
                                                	<td colSpan="3">
													<div align="center">
                                                        <asp:image id="igAccount" runat="server" Width="500px" 
                                                            Height="250px"></asp:image></div>
												</td>
											</tr>

											<tr bgColor="#ffffff">
												<td bgColor="#cccccc" colSpan="3" height="30"  align="left" ><span class="style5">身份证图片</span></td>
                                                <td bgColor="#cccccc" colSpan="3" height="30" align="left" ><span class="style5">银行流水</span></td>
											</tr>

											<tr bgColor="#ffffff">
												<td colSpan="3">
													<div align="center"><asp:image id="igIdentity" runat="server" Width="450px" 
                                                            Height="250px"></asp:image></div>
												</td>
                                                 <td colSpan="3">
													<div align="center"><asp:image id="igBankWater" runat="server" Width="500px" Height="250px"></asp:image></div>
												</td>
                                            </tr>
                                            <tr bgColor="#cccccc">
                                                <td class="style30"  >
                                                <div align="center"><strong><FONT size="2">提交人：</FONT></strong></div>
													
												</td>
												<td class="style36" >
													<strong><asp:label id="lbOperator" runat="server" Width = "70%" Height="26px">yonghua</asp:label></strong>
												</td>
                                             
                                                 <td class="style30" >
                                                <div ><strong><FONT size="2">提交日期：</FONT></strong></div>
												
												</td>
												<td  nowrap colspan = "3" >
													<strong><asp:label id="lbOperatorDate" runat="server" ></asp:label></strong>
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
			<P align="center"><FONT face="宋体">
			<asp:button id="btCommit" runat="server" Width="190px" Height="30px" Text="确认无误，提交审批！" BorderStyle="Groove" onclick="btCommit_Click"></asp:button></FONT>
             </P>         
            <p align="center"><INPUT style="WIDTH: 64px;HEIGHT: 22px" onclick="history.go(-1)" type="button"  value="返回"></p>

		</form>
	</body>
</HTML>
