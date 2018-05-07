;
(function($) {
    $.fn.extend({
        checkbox: function() {
            return this.each(function() {
                var $this = $(this);
                if ($this.hasClass("on")) {
                    $this.siblings("input").prop("checked", "checked");
                } else {
                    $this.siblings("input").removeAttr("checked");
                }
                $this.on("click", function() {
                    if ($this.hasClass("on")) {
                        $this.siblings("input").removeAttr("checked");
                        $this.removeClass("on");
                    } else {
                        $this.siblings("input").prop("checked", "checked");
                        $this.addClass("on");
                    }
                });
            });
        }
    });
})(jQuery);

//<div class="check_box">
//	<div class="checkbox_item box_inline">
//		<input type="checkbox" />
//		<label class="check_label on">
//            <i class="checkbox_icon "></i>
//            <em class="checkbox_text">项目一</em>
//        </label>
//	</div>
//	<div class="checkbox_item box_inline">
//		<input type="checkbox" />
//        <label class="check_label">
//            <i class="checkbox_icon"></i>
//            <em class="checkbox_text">项目二</em>
//        </label>
//	</div>
//    <div class="checkbox_item box_inline">
//    	<input type="checkbox" />
//        <label class="check_label">
//            <i class="checkbox_icon"></i>
//            <em class="checkbox_text">项目三</em>
//        </label>
//    </div>
//</div>

// $(".check_label").checkbox();