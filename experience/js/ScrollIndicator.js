(function() {
	// https://juejin.im/post/59bc84936fb9a00a652af5a4
	// 页面文档高度
	var dh = $(document).height();
	// 当前可视区域高度
	var vh = $(window).height();
	var sHeight = dh - vh;
	$(window).scroll(function() {
		var perc = $(window).scrollTop() / (dh - vh);
		$('.j-scroll-indicator').css({
			width: perc * 100 + '%'
		});
	});
}());

/*
//阮一峰
(function() {
      var $w = $(window);
      var $prog2 = $('.progress-indicator-2');
      var wh = $w.height();
      var h = $('body').height();
      var sHeight = h - wh;
      $w.on('scroll', function() {
        window.requestAnimationFrame(function(){
          var perc = Math.max(0, Math.min(1, $w.scrollTop() / sHeight));
          updateProgress(perc);
        });
      });

      function updateProgress(perc) {
        $prog2.css({width: perc * 100 + '%'});
        ditto.save_progress && store.set('page-progress', perc);
      }

    }());
*/