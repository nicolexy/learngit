<%@ Page language="c#" Codebehind="BankRollLog.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BankRollLog" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BankRollLog</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css"> @import url( ../STYLES/ossstyle.css );  </style>
		<script language="javascript">
		    window.onload = function () {
		        var div = window.parent.document.getElementById('iframe3');
		        div.style.height = document.body.scrollHeight + 20;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid id="DG_Bankrolllog" runat="server" Height="0px" AutoGenerateColumns="False" DataSource="<%# DS_Bankroll %>">
				<ItemStyle Wrap="False"></ItemStyle>
				<HeaderStyle Wrap="False" BackColor="#EEEEEE"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Fbkid" HeaderText="流水的ID号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flistid" HeaderText="交易单的ID号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fspid" HeaderText="机构代码名称（发起者）">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fuid" HeaderText="帐户号码">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcurtype" HeaderText="币种的类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ftype" HeaderText="交易的类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fsubject" HeaderText="类别/科目">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ftrue_name" HeaderText="用户名称">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbalance" HeaderText="帐户余额">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ffromid" HeaderText="对方的帐号">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ffrom_name" HeaderText="对方名称">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fpaynum" HeaderText="金额">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_type" HeaderText="用户银行的类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fprove" HeaderText="记帐凭证">
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
					<asp:BoundColumn DataField="Fmemo" HeaderText="备注/说明">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Faction_type" HeaderText="动作类型">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmodify_time" HeaderText="最后修改时间">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
		</form>
	</body>
</HTML>
