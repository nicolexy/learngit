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
			<FONT face="宋体"></FONT>
			<asp:datagrid id="DataGrid2" runat="server" AutoGenerateColumns="False">
				<HeaderStyle HorizontalAlign="Center" BackColor="#EEEEEE"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Fbutton_id" HeaderText="按钮编号">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmerchan_name" HeaderText="商品名称">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
					</asp:BoundColumn>
					<asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Fmerchan_url" DataNavigateUrlFormatString="{0}"
						DataTextField="Fmerchan_url" HeaderText="商品展示网址" DataTextFormatString="{0}">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:HyperLinkColumn>
					<asp:BoundColumn DataField="Fmerchan_price" HeaderText="商品单价">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmail_fee_bear" HeaderText="运费承担者">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcomm_mail_fee" HeaderText="平邮">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fexpress_mail_fee" HeaderText="快递">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcreate_time" HeaderText="创建时间">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flast_modify_time" HeaderText="最近修改时间">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Flast_modify_ip" HeaderText="最近修改IP">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmerchan_desc" HeaderText="商品描述">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fstatus" HeaderText="状态">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fdelete_time" HeaderText="删除时间">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
			</asp:datagrid>
			<webdiyer:AspNetPager id="pager" runat="server" NumericButtonCount="3" ShowCustomInfoSection="left" PagingButtonSpacing="0"
				ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="转到"
				NumericButtonTextFormatString="[{0}]" PageSize="8" />
		</form>
	</body>
</HTML>
