<%@ Page language="c#" Codebehind="test.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.test" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>test</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:Button id="Button1" style="Z-INDEX: 101; LEFT: 40px; POSITION: absolute; TOP: 24px" runat="server"
				Text="Button" onclick="Button1_Click"></asp:Button>
			<asp:TextBox id="tbcallnum" style="Z-INDEX: 116; LEFT: 736px; POSITION: absolute; TOP: 336px"
				runat="server"></asp:TextBox>
			<asp:TextBox id="TextBox1" style="Z-INDEX: 102; LEFT: 32px; POSITION: absolute; TOP: 56px" runat="server"
				Width="624px">DataBase=test;Data Source=172.16.61.44;User ID=root;Password=root1234;CharSet='latin1'</asp:TextBox>
			<asp:TextBox id="TextBox2" style="Z-INDEX: 103; LEFT: 32px; POSITION: absolute; TOP: 88px" runat="server"
				Width="432px" Height="304px" TextMode="MultiLine"></asp:TextBox>
			<asp:Label id="Label1" style="Z-INDEX: 104; LEFT: 128px; POSITION: absolute; TOP: 32px" runat="server">Label</asp:Label>
			<asp:Label id="Label2" style="Z-INDEX: 105; LEFT: 384px; POSITION: absolute; TOP: 32px" runat="server">Label</asp:Label>
			<asp:Button id="Button2" style="Z-INDEX: 106; LEFT: 512px; POSITION: absolute; TOP: 24px" runat="server"
				Text="Button" onclick="Button2_Click"></asp:Button>
			<asp:Button id="Button3" style="Z-INDEX: 107; LEFT: 608px; POSITION: absolute; TOP: 160px" runat="server"
				Text="获取IVR数据" onclick="Button3_Click"></asp:Button>
			<asp:Button id="Button4" style="Z-INDEX: 108; LEFT: 760px; POSITION: absolute; TOP: 160px" runat="server"
				Text="发送呼叫结果" onclick="Button4_Click"></asp:Button>
			<asp:TextBox id="tbAppealID" style="Z-INDEX: 109; LEFT: 736px; POSITION: absolute; TOP: 200px"
				runat="server"></asp:TextBox>
			<asp:Label id="Label3" style="Z-INDEX: 110; LEFT: 648px; POSITION: absolute; TOP: 200px" runat="server">申诉ID</asp:Label>
			<asp:Label id="Label4" style="Z-INDEX: 111; LEFT: 616px; POSITION: absolute; TOP: 248px" runat="server">财付通账号</asp:Label>
			<asp:TextBox id="tbuin" style="Z-INDEX: 112; LEFT: 736px; POSITION: absolute; TOP: 240px" runat="server"></asp:TextBox>
			<asp:Label id="label11" style="Z-INDEX: 113; LEFT: 640px; POSITION: absolute; TOP: 296px" runat="server">手机号</asp:Label>
			<asp:TextBox id="tbmobile" style="Z-INDEX: 114; LEFT: 736px; POSITION: absolute; TOP: 288px"
				runat="server"></asp:TextBox>
			<asp:Label id="Label5" style="Z-INDEX: 115; LEFT: 632px; POSITION: absolute; TOP: 344px" runat="server">呼叫次数</asp:Label>
			<asp:Label id="Label6" style="Z-INDEX: 117; LEFT: 640px; POSITION: absolute; TOP: 424px" runat="server">呼叫结果</asp:Label>
			<asp:TextBox id="tbresult" style="Z-INDEX: 118; LEFT: 744px; POSITION: absolute; TOP: 416px"
				runat="server"></asp:TextBox>
			<asp:Label id="label33" style="Z-INDEX: 119; LEFT: 648px; POSITION: absolute; TOP: 464px" runat="server">呼叫备注</asp:Label>
			<asp:TextBox id="tbmemo" style="Z-INDEX: 120; LEFT: 744px; POSITION: absolute; TOP: 456px" runat="server"></asp:TextBox>
             <asp:Label id="Label7" style="Z-INDEX: 121; LEFT: 648px; POSITION: absolute; TOP: 500px" runat="server">dbName</asp:Label>
             <asp:TextBox id="dbName" style="Z-INDEX: 122; LEFT: 744px; POSITION: absolute; TOP: 500px"
				runat="server"></asp:TextBox>
			<asp:Label id="Label8" style="Z-INDEX: 123; LEFT: 648px; POSITION: absolute; TOP: 540px" runat="server">tbName</asp:Label>
            <asp:TextBox id="tbName" style="Z-INDEX: 124; LEFT: 744px; POSITION: absolute; TOP: 540px"
				runat="server"></asp:TextBox>
		</form>
	</body>
</HTML>
