<%@ Page language="c#" Codebehind="CalendarForm3.aspx.cs" AutoEventWireup="True" Inherits="SDate.CalendarForm3" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>

	<HEAD>

		<title>日期选择窗体</title>

		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">

		<meta content="JavaScript" name="vs_defaultClientScript">

		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

	</HEAD>

	<body id="Mybody" runat="server" ms_positioning="GridLayout">

		<form id="Form1" method="post" runat="server">

			<asp:calendar id="Calendar1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server"

				Height="190px" Width="340px" BorderWidth="1px" BackColor="#FFFFCC" ForeColor="#663399" Font-Size="8pt"

				Font-Names="Verdana" BorderColor="#FFCC66" ShowGridLines="True" PrevMonthText="上个月&amp;lt;&amp;lt;"

				NextMonthText="下个月&amp;gt;&amp;gt;" onselectionchanged="Calendar1_SelectionChanged">

				<TodayDayStyle ForeColor="#00C000" BackColor="Khaki"></TodayDayStyle>

				<SelectorStyle BackColor="#FFCC66"></SelectorStyle>

				<NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC"></NextPrevStyle>

				<DayHeaderStyle Height="1px" BackColor="#FFCC66"></DayHeaderStyle>

				<SelectedDayStyle Font-Bold="True" BackColor="MediumPurple"></SelectedDayStyle>

				<TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="#FFFFCC" BackColor="#990000"></TitleStyle>

				<OtherMonthDayStyle ForeColor="#CC9966"></OtherMonthDayStyle>

			</asp:calendar>

			<asp:TextBox id="TextBox1" style="Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 200px" runat="server"

				Visible="False"></asp:TextBox>

			<asp:Button id="Button1" style="Z-INDEX: 103; LEFT: 160px; POSITION: absolute; TOP: 200px" runat="server"

				Text="确 定" BorderColor="SteelBlue" ForeColor="White" BackColor="SteelBlue" Width="64px"

				Height="24px" onclick="Button1_Click"></asp:Button></form>

	</body>

</HTML>

