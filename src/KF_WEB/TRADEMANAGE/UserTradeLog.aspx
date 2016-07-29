<%@ Page language="c#" Codebehind="UserTradeLog.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.UserTradeLog" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ShowTradeLog</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style1 { FONT-WEIGHT: bold; FONT-SIZE: 14px }
		</style>
		<script language="javascript">
		    window.onload = function () {
		        var div = window.parent.document.getElementById('iframe4');
		        div.style.height = document.body.scrollHeight + 20;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<FONT face="宋体">
			<asp:datagrid id=DG_TradeLog AutoGenerateColumns="False" runat="server" DataSource="<%# DS_Utradelog %>">
				<Columns>
					<asp:BoundColumn DataField="Fmodify_time" HeaderText="时间">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>					
					<asp:BoundColumn Visible="False" DataField="Flistid" HeaderText="交易单ID">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Fspid" HeaderText="机构代码名称（发起者）">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcoding" HeaderText="商户订单编码">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fqqid" HeaderText="帐户号码">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					
					<asp:TemplateColumn HeaderText="交易类型" Visible=False>
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertTradeType(DataBinder.Eval(Container, "DataItem.Ftype").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="类别/科目">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertSubject(DataBinder.Eval(Container, "DataItem.Fsubject").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="金额">
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaynum").ToString()) %>' ID="Label2" NAME="Label2">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Ftrue_name" HeaderText="用户名称">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="帐户余额" Visible=False>
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fbalance").ToString()) %>' ID="Label3">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Ffromid" HeaderText="对方的帐号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ffrom_name" HeaderText="对方名称">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="对方帐户的余额" Visible=False>
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Ffrom_balance").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="交易原始状态">
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertTradeListState(DataBinder.Eval(Container, "DataItem.Fold_state").ToString()) %>' ID="Label4">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="交易新的状态">
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertTradeListState(DataBinder.Eval(Container, "DataItem.Fnew_state").ToString()) %>' ID="Label5">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="币种类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertMoney_type(DataBinder.Eval(Container, "DataItem.Fcurtype").ToString()) %>' ID="Label1" NAME="Label1">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				
					<asp:TemplateColumn HeaderText="用户银行的类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fbank_type").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Fprove" HeaderText="记帐凭证">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间（本地）">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fapplyid" HeaderText="应用系统的ID号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fip" HeaderText="客户的IP地址">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmemo" HeaderText="备注说明" Visible=False >
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					
				</Columns>
			</asp:datagrid></FONT>
	</body>
</HTML>
