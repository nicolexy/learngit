<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="refund.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.refund" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>refund</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css"> @import url( ../STYLES/ossstyle.css );  </style>
		<script language="javascript">
		    window.onload = function () {
		        var div = window.parent.document.getElementById('iframe0');
		        div.style.height = document.body.scrollHeight + 20;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<asp:DataGrid id="DataGrid1" runat="server" DataSource="<%# dataSet1 %>" AutoGenerateColumns =False >
					<HeaderStyle Wrap="False" BackColor="#EEEEEE"></HeaderStyle>
					<Columns>
						<asp:BoundColumn Visible="True" DataField="Frlistid" HeaderText="退款单号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Flistid" HeaderText="交易单号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="退款方式">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.cPay_type(DataBinder.Eval(Container, "DataItem.Fpay_type").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="Fspid" HeaderText="机构代码名称">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fbuy_uid" HeaderText="买家内部帐号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fbuyid" HeaderText="买家帐号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fbuy_name" HeaderText="买家名称">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="买家银行的类型">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fbuy_bank_type").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="Fbuy_bankid" HeaderText="买家银行帐号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fsale_uid" HeaderText="卖家内部帐号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fsaleid" HeaderText="卖家帐号">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fsale_name" HeaderText="卖家的姓名">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="卖家银行的类型">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fsale_bank_type").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="卖家的银行类型">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fsale_bankid").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="退款的状态">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.cRefundState(DataBinder.Eval(Container, "DataItem.Fstate").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="退款单的状态">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.cRlistState(DataBinder.Eval(Container, "DataItem.Flstate").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="退还给买家金额">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaybuy").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="退还给卖家金额">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaysale").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="交易手续费">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fprocedure").ToString()) %>'>
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="Fbargain_time" HeaderText="c2c请求退款日期">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fcreate_time" HeaderText="生成退款单的日期">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fok_time" HeaderText="生成日期-退款日期">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fmemo" HeaderText="退款说明">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fip" HeaderText="最后修改退款单的IP">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fexplain" HeaderText="操作人员备注">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fok_time_acc" HeaderText="帐务时间">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
				<webdiyer:AspNetPager id="pager" runat="server" NumericButtonCount="3" PagingButtonSpacing="0" ShowInputBox="always"
					CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"
					AlwaysShow="True" />
			</FONT>
		</form>
	</body>
</HTML>
