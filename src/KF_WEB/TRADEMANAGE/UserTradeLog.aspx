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
		<FONT face="����">
			<asp:datagrid id=DG_TradeLog AutoGenerateColumns="False" runat="server" DataSource="<%# DS_Utradelog %>">
				<Columns>
					<asp:BoundColumn DataField="Fmodify_time" HeaderText="ʱ��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>					
					<asp:BoundColumn Visible="False" DataField="Flistid" HeaderText="���׵�ID">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Fspid" HeaderText="�����������ƣ������ߣ�">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcoding" HeaderText="�̻���������">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fqqid" HeaderText="�ʻ�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					
					<asp:TemplateColumn HeaderText="��������" Visible=False>
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertTradeType(DataBinder.Eval(Container, "DataItem.Ftype").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="���/��Ŀ">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertSubject(DataBinder.Eval(Container, "DataItem.Fsubject").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="���">
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fpaynum").ToString()) %>' ID="Label2" NAME="Label2">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Ftrue_name" HeaderText="�û�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="�ʻ����" Visible=False>
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fbalance").ToString()) %>' ID="Label3">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Ffromid" HeaderText="�Է����ʺ�">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Ffrom_name" HeaderText="�Է�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:TemplateColumn HeaderText="�Է��ʻ������" Visible=False>
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Ffrom_balance").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="����ԭʼ״̬">
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertTradeListState(DataBinder.Eval(Container, "DataItem.Fold_state").ToString()) %>' ID="Label4">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="�����µ�״̬">
					<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertTradeListState(DataBinder.Eval(Container, "DataItem.Fnew_state").ToString()) %>' ID="Label5">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="��������">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertMoney_type(DataBinder.Eval(Container, "DataItem.Fcurtype").ToString()) %>' ID="Label1" NAME="Label1">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				
					<asp:TemplateColumn HeaderText="�û����е�����">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fbank_type").ToString()) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Fprove" HeaderText="����ƾ֤">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ�䣨���أ�">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fapplyid" HeaderText="Ӧ��ϵͳ��ID��">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fip" HeaderText="�ͻ���IP��ַ">
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Fmemo" HeaderText="��ע˵��" Visible=False >
						<HeaderStyle Wrap="False"></HeaderStyle>
						<ItemStyle Wrap="False"></ItemStyle>
					</asp:BoundColumn>
					
				</Columns>
			</asp:datagrid></FONT>
	</body>
</HTML>
