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
					<asp:BoundColumn DataField="Ftde_id" HeaderText="��ˮ��ID">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flistid" HeaderText="���׵���ID">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fqqid" HeaderText="�ʻ�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_list" HeaderText="���ж�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fuid" HeaderText="��Ѷ�����ʻ�">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fstate" HeaderText="��ǰ״̬">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ftype" HeaderText="��������/ҵ������">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fsubject" HeaderText="��𣨿�Ŀ��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fnum" HeaderText="���׵Ľ��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fsign" HeaderText="���ױ��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_acc" HeaderText="���ж�����Ȩ��/ƾ֤��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fbank_type" HeaderText="���е�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcurtype" HeaderText="���ִ���">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Faid" HeaderText="�Է����ʺ�">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Faname" HeaderText="�Է�������">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fprove" HeaderText="����ƾ֤">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fip" HeaderText="IP��ַ">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmemo" HeaderText="��ע/˵��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fpay_front_time" HeaderText="����ǰʱ��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
				<PagerStyle Wrap="False"></PagerStyle>
			</asp:DataGrid>
		</form>
	</body>
</HTML>
