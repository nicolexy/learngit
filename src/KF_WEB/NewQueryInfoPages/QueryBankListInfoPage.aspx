<%@ Page language="c#" Codebehind="QueryBankListInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryBankListInfoPage" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QueryDKInfoPage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">
					function enterPress(e)
					{
						if(window.event)
						{
							if(e.keyCode == 13)
								document.getElementById('btn_serach').click();
						}
					}
					
					function onLoadFun()
					{
						document.getElementById('btn_serach').focus();
					}
		</script>
	</HEAD>
	<body onload="onLoadFun()" MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="1300" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;银行批次交易查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT> <SPAN>财付通批次：</SPAN>
						<asp:textbox id="tbx_batchid" Runat="server"></asp:textbox>给<SPAN>银行批次：</SPAN>
						<asp:textbox id="tbx_batchid_forbank" Runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD colSpan="2"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT><SPAN>银行渠道：</SPAN>
						<asp:textbox id="tbx_bank_type" Runat="server"></asp:textbox><SPAN>批次状态：</SPAN>
						<asp:dropdownlist id="ddl_status" Runat="server">
							<asp:ListItem Value="0" Selected="True">全部状态</asp:ListItem>
							<asp:ListItem Value="1">初始状态</asp:ListItem>
							<asp:ListItem Value="2">分批完成</asp:ListItem>
							<asp:ListItem Value="3">预支付完成</asp:ListItem>
							<asp:ListItem Value="4">部分发送中</asp:ListItem>
							<asp:ListItem Value="5">发送完成</asp:ListItem>
							<asp:ListItem Value="6">结果获取中</asp:ListItem>
							<asp:ListItem Value="7">执行完成</asp:ListItem>
							<asp:ListItem Value="8">全部成功</asp:ListItem>
							<asp:ListItem Value="9">全部失败</asp:ListItem>
						</asp:dropdownlist><SPAN>查询时间段：</SPAN>

                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
                        到
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="选择日期" />
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				<TR>
					<TD align="center" colSpan="5"><asp:button id="btn_serach" Width="80px" Runat="server" Text="查询" onclick="btn_serach_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<TABLE cellSpacing="0" cellPadding="0" width="1300" border="0">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1300px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="Fbank_batch_id" HeaderText="银行批量内部批次号"></asp:BoundColumn>
								<asp:ButtonColumn Text="查询详情" HeaderText="操作" CommandName="detail"></asp:ButtonColumn>
								<asp:BoundColumn DataField="Fbatchid" HeaderText="商户批次号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbatchid_forbank" HeaderText="给银行批次号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="生成时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="FstatusName" HeaderText="批次状态"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftotal_feeName" HeaderText="总金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="totalBatchUrl" DataTextField="Ftotal_count"
									HeaderText="总笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fsucpay_feeName" HeaderText="成功金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="successBatchUrl" DataTextField="Fsucpay_count"
									HeaderText="成功笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="" HeaderText="失败金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="failedBatchUrl" DataTextField=""
									HeaderText="失败笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="" HeaderText="处理中金额"></asp:BoundColumn>
								<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="handlingBatchUrl" DataTextField=""
									HeaderText="处理中笔数"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="Fret_code" HeaderText="银行错误码"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fresult_info" HeaderText="错误原因"></asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
			<DIV>
				<P><LABEL>交易成功笔数：</LABEL>
					<asp:label id="lb_successNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>交易成功总额：</LABEL>
					<asp:label id="lb_successAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
					<LABEL>交易失败笔数：</LABEL>
					<asp:label id="lb_failNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp; <LABEL>
						<LABEL>交易失败总额：</LABEL>
						<asp:label id="lb_failAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
						<LABEL>处理中笔数：</LABEL>
						<asp:label id="lb_handlingNum" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp; 
						处理中总额：</LABEL>
					<asp:label id="lb_handlingAllMoney" Runat="server">0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
				</P>
			</DIV>
			<TABLE cellSpacing="1" cellPadding="0" width="1300" bgColor="black" border="0">
				<TR>
					<TD bgColor="#eeeeee" colSpan="4" height="18"><SPAN>&nbsp;详细信息列表：</SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" width="229" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">银行类型：</FONT></FONT></TD>
					<TD style="HEIGHT: 19px" width="225" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c1" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;给银行批次号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c2" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><FONT face="宋体">&nbsp;接口类型:</FONT></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c3" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;银行批量内部批次号:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="宋体">&nbsp;币种类型:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;财付通内部批次号:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;总金额:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;已分批金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;总笔数:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;已分批笔数</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="宋体">&nbsp;成功金额:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;实际发送银行总金额:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c12" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;成功笔数:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;实际发送银行总笔数:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c14" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;业务状态:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;财付通后台操作备注</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c16" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;生成时间:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c17" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;最后一包发送时间：</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c18" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;最后修改时间:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c19" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;预期结果时间：</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c20" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;银行最后错误码:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c21" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;银行错误信息：</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c22" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;客户IP:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c23" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;最后修改IP：</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
							<asp:label id="lb_c24" runat="server"></asp:label></FONT></TD>
				</TR>
			</TABLE>
		</FORM>
	</body>
</HTML>
