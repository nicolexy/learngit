<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="gatheringlog.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.gatheringlog" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TradeLog</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); 
		</style>
		<%
			if (Request.QueryString["type"].ToString() == "ListID")
			{
		%>
		<script language="javascript">
		    window.onload = function () {
		        var div = window.parent.document.getElementById('iframe1');
		        div.style.height=document.body.scrollHeight+20;
		    }
		</script>
		<%
			}else {
		%>
		<script language="javascript">
		    window.onload = function () {
		        var div = window.parent.document.getElementById('iframe0');
		        div.style.height = document.body.scrollHeight + 20;
		    }
		</script>
		<%
			}
		%>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����">
				<asp:DataGrid id="DataGrid1" runat="server" DataSource="<%# DS_Gather %>" DataMember="Table1" AutoGenerateColumns="False" EnableViewState="False" PageSize="12">
					<HeaderStyle BackColor="#EEEEEE"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="Ftde_id" HeaderText="��ˮ��ID��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Flistid" HeaderText="�տ��ID��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="���ֵ�����">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertMoney_type(DataBinder.Eval(Container, "DataItem.Fcurtype").ToString()) %>' ID="Label1" NAME="Label1">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="��ǰ״̬">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertTCState(DataBinder.Eval(Container, "DataItem.Fstate").ToString()) %>' ID="Label2" NAME="Label2">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn Visible="False" HeaderText="��������">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertTradeType(DataBinder.Eval(Container, "DataItem.Ftype").ToString()) %>' ID="Label3" NAME="Label3">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="��Ŀ">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertSubject(DataBinder.Eval(Container, "DataItem.Fsubject").ToString()) %>' ID="Label4" NAME="Label4">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="���׵Ľ��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.FenToYuan(DataBinder.Eval(Container, "DataItem.Fnum").ToString()) %>' ID="Label5" NAME="Label5">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="���ױ��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertTradeSign(DataBinder.Eval(Container, "DataItem.Fsign").ToString()) %>' ID="Label6" NAME="Label6">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="���е�����">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
							<ItemTemplate>
								<asp:Label runat="server" Text='<%# setConfig.convertbankType(DataBinder.Eval(Container, "DataItem.Fbank_type").ToString()) %>' ID="Label7" NAME="Label7">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn DataField="Fauid" HeaderText="�Է��ڲ��˻�ID">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Faid" HeaderText="�Է���ID">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Faname" HeaderText="�Է�������">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fbank_list" HeaderText="���ж�����">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fbank_acc" HeaderText="���з��صĶ�����">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fpay_time_acc" HeaderText="����ʱ��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fpay_front_time" HeaderText="����ǰʱ��">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Fpay_time" HeaderText="����ʱ��">
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
						<asp:BoundColumn DataField="Fprove" HeaderText="����ƾ֤">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Ftc_bankid" HeaderText="��Ѷ�����˻�ID">
							<HeaderStyle Wrap="False"></HeaderStyle>
							<ItemStyle Wrap="False"></ItemStyle>
						</asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
				<%
			if (Request.QueryString["type"].ToString() == "QQID")
			{
%>
				<webdiyer:AspNetPager id="pager" runat="server" NumericButtonCount="5" PagingButtonSpacing="0" ShowInputBox="always"
					CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"
					AlwaysShow="True" />
				<%		    }   %>
			</FONT>
		</form>
	</body>
</HTML>
