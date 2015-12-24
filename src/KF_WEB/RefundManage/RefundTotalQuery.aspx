<%@ Page language="c#" Codebehind="RefundTotalQuery.aspx.cs" AutoEventWireup="false" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.RefundTotalQuery" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RefundTotalQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">					
			function select_deselectAll (chkVal, idVal) 
			{
				var frm = document.forms[0];
				for (i=0; i<frm.length; i++) 
				{
					if (idVal.indexOf ('CheckAll') != -1)
					{
						if(frm.elements[i].id.indexOf('CheckBox') != -1)
						{
							if(chkVal == true) 
							{
								frm.elements[i].checked = true;
							} 
							else 
							{
								frm.elements[i].checked = false;
							}
						}
					} 
					else if (idVal.indexOf('DeleteThis') != -1) 
					{
						if(frm.elements[i].checked == false) 
						{
							frm.elements[1].checked = false;
						}
					}
				}
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:panel id="SearchPanel" Visible="true" Runat="server">
				<TABLE style="margin-top:10px;margin-left:10px" id="Table1" border="1"
					cellSpacing="1" cellPadding="1" width="85%" runat="server">
					<TR>
						<TD bgColor="#e4e5f7" colSpan="3"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;退单汇总数据查询</FONT> <FONT color="#ff0000" face="宋体">（按交易单或给银行订单号只能查询2天的数据。按退款单查询不用选中时间，支持查2年内的单）</FONT></FONT></TD>
						<TD bgColor="#e4e5f7" align="right"><FONT face="宋体">操作员代码: <SPAN class="style3">
									<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label2" runat="server">汇总起始日期</asp:label></TD>
						<TD><FONT face="宋体">
								<asp:textbox id="TextBoxBeginDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
								</FONT></TD>
						<TD align="right">
							<asp:label id="Label3" runat="server">汇总结束日期</asp:label></TD>
						<TD>
							<asp:textbox id="TextBoxEndDate" runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
							</TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 30px" align="right">
							<asp:label id="Label5" runat="server">数据来源</asp:label></TD>
						<TD style="HEIGHT: 30px">
							<asp:dropdownlist id="ddlrefund_type" runat="server" Width="152px">
								<asp:ListItem Value="99" Selected="True">所有类型</asp:ListItem>
								<asp:ListItem Value="1">商户退单</asp:ListItem>
								<asp:ListItem Value="2">对帐结果退单</asp:ListItem>
								<asp:ListItem Value="3">人工录入退单</asp:ListItem>
								<asp:ListItem Value="4">对帐异常退单</asp:ListItem>
								<asp:ListItem Value="5">赔付退单</asp:ListItem>
								<asp:ListItem Value="9">充值单退款</asp:ListItem>
								<asp:ListItem Value="11">拍拍退单</asp:ListItem>
								<asp:ListItem Value="12">提现退款异常单</asp:ListItem>
							</asp:dropdownlist></TD>
						<TD style="HEIGHT: 30px" align="right">
							<asp:label id="Label6" runat="server">退款银行</asp:label></TD>
						<TD style="HEIGHT: 30px">
							<asp:dropdownlist id="ddlrefund_bank" runat="server" Width="152px"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">退款途径</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlrefund_path" runat="server" Width="152px">
								<asp:ListItem Value="99">所有类型</asp:ListItem>
								<asp:ListItem Value="1">网银退单</asp:ListItem>
								<asp:ListItem Value="2">接口退单</asp:ListItem>
								<asp:ListItem Value="3">人工授权</asp:ListItem>
								<asp:ListItem Value="4">退余额</asp:ListItem>
								<asp:ListItem Value="5">转入代发</asp:ListItem>
								<asp:ListItem Value="6">付款退款</asp:ListItem>
							</asp:DropDownList></TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">退款状态</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlrefund_state" runat="server" Width="152px">
								<asp:ListItem Value="99">所有状态</asp:ListItem>
								<asp:ListItem Value="0">初始状态</asp:ListItem>
								<asp:ListItem Value="1">退单流程中</asp:ListItem>
								<asp:ListItem Value="2">退单成功</asp:ListItem>
								<asp:ListItem Value="3">退单失败</asp:ListItem>
								<asp:ListItem Value="5">手工退款中</asp:ListItem>
								<asp:ListItem Value="6">申请手工退款</asp:ListItem>
								<asp:ListItem Value="7">申请转入代发</asp:ListItem>
								<asp:ListItem Value="4">退单状态未定</asp:ListItem>
								<asp:ListItem Value="8">挂异常处理中</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label4" runat="server">商户号</asp:label></TD>
						<TD>
							<asp:textbox id="tbSPID" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label7" runat="server">回导状态</asp:label></TD>
						<TD>
							<asp:DropDownList id="ddlreturn_state" runat="server" Width="152px">
								<asp:ListItem Value="99">所有状态</asp:ListItem>
								<asp:ListItem Value="1">回导前</asp:ListItem>
								<asp:ListItem Value="2">回导后</asp:ListItem>
							</asp:DropDownList></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label10" runat="server">交易单ID</asp:label></TD>
						<TD>
							<asp:textbox id="tbRefundID" runat="server" Width="200px"></asp:textbox></TD>
						<TD align="right">
							<asp:label id="Label11" runat="server">银行订单号</asp:label></TD>
						<TD>
							<asp:textbox id="tbBank_list" runat="server" Width="150px"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:Label id="Label12" runat="server">退款单：</asp:Label></TD>
						<TD>
							<asp:textbox style="Z-INDEX: 0" id="txFoldid" runat="server" Width="200px"></asp:textbox></TD>
						<TD></TD>
						<TD>
							<asp:Button id="btnQuery" runat="server" Text="查询记录"></asp:Button>
					    </TD>
					</TR>
				</TABLE>
			</asp:panel>
			<asp:panel id="Panel1" Visible="true" Runat="server">
				<TABLE style=" WIDTH: 85%;  margin-left:10px" id="Table2"
					border="1" cellSpacing="1" cellPadding="1" runat="server">
					<TR>
						<TD vAlign="top">
							<asp:datagrid id="DataGrid1" runat="server" Width="100%" PageSize="50" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="FRefundID" HeaderText="自增ID"></asp:BoundColumn>
									<asp:BoundColumn DataField="FPaylistid" HeaderText="退款单"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeName" HeaderText="退款银行"></asp:BoundColumn>
									<asp:BoundColumn DataField="FPay_time" HeaderText="支付日期"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_listid" HeaderText="银行订单号"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnamtName" HeaderText="退单金额" DataFormatString="{0:N}"></asp:BoundColumn>
									<asp:BoundColumn DataField="FamtName" HeaderText="订单金额" DataFormatString="{0:N}"></asp:BoundColumn>
									<asp:BoundColumn DataField="FstateName" HeaderText="退单状态"></asp:BoundColumn>
									<asp:BoundColumn DataField="FreturnStateName" HeaderText="回导状态"></asp:BoundColumn>
									<asp:BoundColumn DataField="FrefundPathName" HeaderText="退单途径"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="详细">
										<ItemTemplate>
											<a href='<%# String.Format("RefundTotalQuery_Detail.aspx?refundId={0}&batchid={1}", DataBinder.Eval(Container, "DataItem.FRefundId").ToString(), DataBinder.Eval(Container, "DataItem.Fbatchid").ToString()
										) %>' >详细 </a>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="fbatchid" HeaderText="fbatchid"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR height="25">
						<TD>
							<webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
								PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
								SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
					</TR>
					<TR>
						<TD align="center"><FONT face="宋体"></FONT><INPUT style="WIDTH: 64px; HEIGHT: 22px" onclick="history.go(-1)" value="返回" type="button">
						</TD>
					</TR>
				</TABLE>
			</asp:panel>
		</form>
	</body>
</HTML>
