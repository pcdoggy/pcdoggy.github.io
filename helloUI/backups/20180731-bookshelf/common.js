var comFn = {
	setDefault: function() {
		var width = document.documentElement.clientWidth; //获取屏幕的宽度
		var height = document.documentElement.clientHeight; //获取屏幕的高度
		console.log("width:" + width + ",,height:" + height);
		var bili1 = width / 375; //屏幕宽度与设计稿宽度的比例（375是设计稿的宽度）
		var bili2 = height / 812; //屏幕高度与设计稿高度的比例(812是设计稿的高度)
		var bili = bili1 <= bili2 ? bili1 : bili2; //宽度的比例和高度的比例进行比较，取值最小的
		if(width > height) bili = 1; //		else bili = bili1 >= bili2 ? bili1 : bili2;
		console.log("bili1:" + bili1 + ",,bili2:" + bili2);
		var html = document.querySelector('html'); //选择html节点
		var rem = 12; //手动设置rem与px的比例；
		html.style.fontSize = rem + "px"; //设置html的默认fontsize为16px。(注意，浏览器中最小值为12px，)

		var vtopbottomheight = 60,
			vpadtop = 4,
			vsearchheight = 20,
			vcontentheight = 64;

		//将比例和rem进行联系。
		var __topbottomheight = bili * vtopbottomheight / rem, // 3.75rem;
			__padtop = bili * vpadtop / rem, // 0.25rem;
			__searchheight = bili * vsearchheight / rem, // 1.25rem;
			__contentheight = bili * vcontentheight / rem; // 4rem;

		document.documentElement.style.setProperty('--top-bottom-height', __topbottomheight + "rem"); //设置css中的变量为--top-bottom-height，值为__topbottomheight
		document.documentElement.style.setProperty('--pad-top', __padtop + "rem");
		document.documentElement.style.setProperty('--search-height', __searchheight + "rem");
		document.documentElement.style.setProperty('--content-height', __contentheight + "rem");
		console.log("__topbottomheight:" + __topbottomheight + ",,__padtop:" + __padtop + ",,__searchheight:" + __searchheight + ",,__contentheight:" + __contentheight);
	}
}