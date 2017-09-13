$(function() {
	const contoday = mainFn.today; // 当前日期
	let y = mainFn.year(contoday); // 当前年
	let m = mainFn.month(contoday) + 1; // 当前月
	let d = mainFn.date(contoday); // 当前日
	let dow = mainFn.firstday_of_week(contoday); // 当前月第一天對應星期几，0~6
	let fdow = mainFn.firstday_of_week(contoday); // 当前月第一天對應星期几，0~6
	let dsow = mainFn.daystr_of_week(dow); // 当前日對應星期几，具体
	let dspm = mainFn.days_per_month(contoday); // 当前年每月对应天数
	let lns = mainFn.line_nums(contoday); // 当前年当前月对应表格行数

	console.log("yyy" + y +
		",mmm" + m +
		",ddd" + d +
		",dow" + dow +
		",fdow" + fdow +
		",dsow" + dsow +
		",dspm" + dspm +
		",lns" + lns);

	$("#spanDay").text(d);
	$(".spanYearMonth").text(y + "年" + m + "月");
	$("#spanWeekday").text(dsow);

	//	${expression}
	let days = "";
	// 日期信息
	let nowday = 1;
	for(var i = 0; i < lns; i++) {
		days += '<tr">';
		for(j = 0; j < 7; j++) {
			days += '<td ';
			days += 'class="pd-bottom-20 ';
			if(j == 0 || j == 6) {
				days += 'text-danger';
			} else {
				if(nowday == d) {
					days += 'bg-info';
				}
			}
			days += '">';

			if(nowday <= dspm[m - 1]) {
				if(i == 0 && j >= fdow) {
					// 第一週
					days += nowday++;
				} else if(i > 0) {
					days += nowday++;
				}
			}
			days += '</td>';
		}
		days += '</tr>';

	}
	var vdaylist = `
					<tr style="padding-bottom: 10px;">
						<td class="pd-bottom-10">周日</td>
						<td class="pd-bottom-10">周一</td>
						<td class="pd-bottom-10">周二</td>
						<td class="pd-bottom-10">周三</td>
						<td class="pd-bottom-10">周四</td>
						<td class="pd-bottom-10">周五</td>
						<td class="pd-bottom-10">周六</td>
					</tr>
					${days}
					`;

	$("#day-list").html(vdaylist);

	var vinter = setInterval(mainFn.cur_time, 1000);

})

// 首页函数相关
var mainFn = {
	today: new Date(),
	year: function(today = new Date()) {
		// 当前日期对应年
		return today.getFullYear();
	},
	month: function(today = new Date()) {
		// 当前日期对应月份
		return today.getMonth();
	},
	date: function(today = new Date()) {
		// 当前日期对应日
		return today.getDate();
	},
	firstday: function(today = new Date()) {
		// 当前日期对应月中的第一天	

		var year = mainFn.year(today);
		var month = mainFn.month(today);
		return new Date(year, month, 1);
	},
	firstday_of_week: function(today = new Date()) {
		// 获取当前日期对应星期几
		return mainFn.firstday(today).getDay();
	},
	day_of_week: function(today = new Date()) {
		// 判断当月份第一天对应星期几,0:星期天,1:星期一...
		return mainFn.today.getDay();
	},
	daystr_of_week: function(day = 0) {
		// 獲取星期几，具體

		var week = new Array("星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
		return week[mainFn.day_of_week(day)];
	},
	isLeap: function(year) {
		// 判断当前年是否为闰年，闰年返回1否则返回0
		return year % 4 == 0 ? 1 : 0;
	},
	days_per_month: function(today = new Date()) {
		// 对应年每月对应天数

		var year = mainFn.year(today);
		return new Array(31, 28 + mainFn.isLeap(year), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
	},
	line_nums: function(today = new Date()) {
		// 对应年每月对应表格行数

		var vday_of_week = mainFn.day_of_week(today);
		var vm = mainFn.month(today);
		return Math.ceil((mainFn.day_of_week(today) + mainFn.days_per_month(today)[vm]) / 7);
	},
	cur_time: function() {
		// 当前时间  时分秒
		var today = new Date();

		var seperator2 = ":",
			vmin = today.getMinutes(),
			vsec = today.getSeconds();
		var vcurtime = today.getHours() + seperator2 + (vmin < 10 ? ("0" + vmin) : vmin) +
			seperator2 + (vsec < 10 ? ("0" + vsec) : vsec);
		var $time = $(".time:eq(0)");
		$time.html(vcurtime);
	}
}