<%@ Page language="c#" Codebehind="FastReplyManagePage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FastReplyManagePage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FastReplyManagePage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div>
				<table width="1100">
					<tr>
						<td width="50%"><label>�Ƹ�ͨ�ͷ�ϵͳ��ݻظ��������</label></td>
						<td width="50%"><label>������Ա��</label><asp:label id="lb_operatorID" Runat="server"></asp:label></td>
					</tr>
				</table>
				<div>
					<p><label>��ݻظ���壺</label>
						<asp:dropdownlist id="ddl_fastReplyBlock" Runat="server" AutoPostBack="True">
							<asp:ListItem Value="0" Selected="True">��ض������</asp:ListItem>
						</asp:dropdownlist></p>
				</div>
				<div>
					<p>
						<label>��ݻظ������б�</label>
						<asp:DropDownList Runat="server" ID="ddl_fastReplyContent">
						</asp:DropDownList>
					</p>
				</div>
				<div>
					<p>
						<label>��ݻظ����ݱ༭��</label>
						<asp:TextBox Runat="server" ID="tbx_fastReplyContent" TextMode="MultiLine"></asp:TextBox>
					</p>
				</div>
				<div>
					<p>
						<asp:Button Runat="server" ID="btn_addFastReply" Text="��ӿ�ݻظ�" onclick="btn_addFastReply_Click"></asp:Button>
						<asp:Button Runat="server" ID="btn_modifyFastReply" Text="�޸Ŀ�ݻظ�" onclick="btn_modifyFastReply_Click"></asp:Button>
						<asp:Button Runat="server" ID="btn_deleteFastReply" Text="ɾ����ݻظ�" onclick="btn_deleteFastReply_Click"></asp:Button>
						<asp:Button Runat="server" ID="btn_updateFastReply" Text="���¿�ݻظ��б�" onclick="btn_updateFastReply_Click"></asp:Button>
					</p>
				</div>
			</div>
		</form>
	</body>
</HTML>
