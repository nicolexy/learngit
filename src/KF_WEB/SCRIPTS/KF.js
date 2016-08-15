//日历控件设置默认值
function SetDate(days) {
    var now = new Date();
    now.setDate((now.getDate()) + days);
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    return year + '-' + month + '-' + day
}