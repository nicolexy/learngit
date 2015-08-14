<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="ButtonInfo.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ButtonInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ButtonInfo</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); 
		</style>
		<script language="javascript">
		    window.onload = function () {
		        var div = window.parent.document.getElementById('iframe0');
		        div.style.height = document.body.scrollHeight + 20;
		    }
		</script>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT>
			<asp:datagrid id="DataGrid2" runat="server" AutoGenerateColumns="False">
				<HeaderStyle HorizontalAlign="Center" BackColor="#EEEEEE"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Fbutton_id" HeaderText="��ť���">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmerchan_name" HeaderText="��Ʒ����">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Fmerchan_url" DataNavigateUrlFormatString="{0}"
						DataTextField="Fmerchan_url" HeaderText="��Ʒչʾ��ַ" DataTextFormatString="{0}">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:HyperLinkColumn>
					<asp:BoundColumn DataField="Fmerchan_price" HeaderText="��Ʒ����">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmail_fee_bear" HeaderText="�˷ѳе���">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcomm_mail_fee" HeaderText="ƽ��">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fexpress_mail_fee" HeaderText="���">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flast_modify_time" HeaderText="����޸�ʱ��">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flast_modify_ip" HeaderText="����޸�IP">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmerchan_desc" HeaderText="��Ʒ����">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fstatus" HeaderText="״̬">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fdelete_time" HeaderText="ɾ��ʱ��">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
			</asp:datagrid>
			<webdiyer:AspNetPager id="pager" runat="server" NumericButtonCount="3" ShowCustomInfoSection="left" PagingButtonSpacing="0"
				ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="ת��"
				NumericButtonTextFormatString="[{0}]" PageSize="8" />
		</form>
	</body>
</HTML>
