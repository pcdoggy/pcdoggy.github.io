$(function() {
	var today = mainFn.today,
		y = mainFn.year(today),
		m = mainFn.month(today) + 1,
		d = mainFn.date(today),
		fd = mainFn.date(today),
		dow = mainFn.day_of_week(today),
		dsow = mainFn.daystr_of_week(today),
		dspm = mainFn.days_per_month(today),
		ln = mainFn.line_nums(today);

	console.log("yyy" + y +
		",mmm" + m +
		",ddd" + d +
		",fd" + fd +
		",dow" + dow +
		",dsow" + dsow +
		",dspm" + dspm +
		",ln" + ln);

	var vinter = setInterval(mainFn.cur_time, 1000);

})

// 首页函数相关
var mainFn = {
	today: new Date(),
	year: function(today) {
		// 当前日期对应年
		today = today || new Date();
		return today.getFullYear();
	},
	month: function(today) {
		// 当前日期对应月份
		today = today || new Date();
		return today.getMonth();
	},
	date: function(today) {
		// 当前日期对应日
		today = today || new Date();
		return today.getDate();
	},
	firstday: function(today) {
		// 当前日期对应月中的第一天	
		today = today || new Date();

		var year = mainFn.year(today);
		var month = mainFn.month(today);
		return new Date(year, month, 1);
	},
	day_of_week: function(today) {
		// 判断当月份第一天对应星期几,0:星期天,1:星期一...
		today = today || new Date();

		return mainFn.firstday(today).getDay();
	},
	daystr_of_week: function(today) {
		// 判断当月份第一天对应星期几
		today = today || new Date();

		var week = new Array("星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
		return week[mainFn.day_of_week(today)];
	},
	isLeap: function(year) {
		// 判断当前年是否为闰年，闰年返回1否则返回0
		return year % 4 == 0 ? 1 : 0;
	},
	days_per_month: function(today) {
		// 对应年每月对应天数
		today = today || new Date();

		var year = mainFn.year(today);
		return new Array(31, 28 + mainFn.isLeap(year), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
	},
	line_nums: function(today) {
		// 对应年每月对应表格行数
		today = today || new Date();

		var vday_of_week = mainFn.day_of_week(today);
		var vm = mainFn.month(today);
		return Math.ceil((mainFn.day_of_week(today) + mainFn.days_per_month(today)[vm]) / 7);
	},
	cur_time: function() {
		var today = new Date();

		var seperator2 = ":",
			vmin = today.getMinutes(),
			vsec = today.getSeconds();
		var vcurtime = today.getHours() + seperator2 + (vmin < 10 ? ("0" + vmin) : vmin) +
			seperator2 + (vsec < 10 ? ("0" + vsec) : vsec);
		var $time=$(".time:eq(0)");
		$time.html(vcurtime);
	}
}