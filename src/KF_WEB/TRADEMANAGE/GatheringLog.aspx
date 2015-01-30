<%@ Page language="c#" Codebehind="GatheringLog.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.GatheringLog" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GatheringLog</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); 
		</style>
		<script language="javascript">
			function window.onload()
			{
			var div = window.parent.document.getElementById('iframe1');
			div.style.height=document.body.scrollHeight+20;
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid id="DataGrid1" runat="server" AutoGenerateColumns="False" Height="0px" DataSource="<%# DS_Gather %>">
				<SelectedItemStyle Wrap="False"></SelectedItemStyle>
				<EditItemStyle Wrap="False"></EditItemStyle>
				<AlternatingItemStyle Wrap="False"></AlternatingItemStyle>
				<ItemStyle Wrap="False"></ItemStyle>
				<HeaderStyle Wrap="False"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Ftde_id" HeaderText="流水的ID">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flistid" HeaderText="交易单的ID">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fqqid" HeaderText="帐户号码">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_list" HeaderText="银行订单号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fuid" HeaderText="腾讯银行帐户">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fstate" HeaderText="当前状态">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ftype" HeaderText="交易类型/业务种类">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fsubject" HeaderText="类别（科目）">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fnum" HeaderText="交易的金额">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fsign" HeaderText="交易标记">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_acc" HeaderText="银行订单授权号/凭证号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_type" HeaderText="银行的类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcurtype" HeaderText="币种代码">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Faid" HeaderText="对方的帐号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Faname" HeaderText="对方的名称">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fprove" HeaderText="记帐凭证">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fip" HeaderText="IP地址">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmemo" HeaderText="备注/说明">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fpay_front_time" HeaderText="付款前时间">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
				<PagerStyle Wrap="False"></PagerStyle>
			</asp:DataGrid>
		</form>
	</body>
</HTML>
