$(function () {
	$(".start_datetime").datetimepicker({
		language: 'zh-CN',
		format: 'yyyy-mm-dd',
		autoclose: true,
		weekStart: 1,
		minView: 2
	});
	$(".end_datetime").datetimepicker({
		language: 'zh-CN',
		format: 'yyyy-mm-dd',
		autoclose: true,
		weekStart: 1,
		minView: 2
	});
	//$(".start_datetime").click(function () {
	//	$('.end_datetime').datetimepicker('show');
	//});
	//$(".end_datetime").click(function () {
	//	$('.start_datetime').datetimepicker('show');
	//});
	
	$('.start_datetime').on('changeDate', function(){
		if($(".end_datetime").val()){
			if($(".start_datetime").val()>$(".end_datetime").val()){
				$('.end_datetime').val($(".start_datetime").val());
			}
		}
		
		$('.end_datetime').datetimepicker('setStartDate', $(".start_datetime").val());
	});


});
