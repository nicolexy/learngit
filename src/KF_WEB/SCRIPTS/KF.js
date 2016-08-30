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

/**
 * 获取当前月的第一天
 */
function getCurrentMonthFirst() {
    var date = new Date();
    date.setDate(1);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    return year + '-' + month + '-' + day
}
/**
 * 获取当前月的最后一天
 */
function getCurrentMonthLast() {
    var date = new Date();
    var currentMonth = date.getMonth();
    var nextMonth = ++currentMonth;
    var nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
    var oneDay = 1000 * 60 * 60 * 24;
    return new Date(nextMonthFirstDay - oneDay);
}
