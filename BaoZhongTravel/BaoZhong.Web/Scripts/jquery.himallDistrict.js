/*
author: Five(673921852)
description: 市级地区选择（适用于运费计算，收货地址）
2015.6.1
*/

(function($) {
	$.fn.himallDistrict = function(options) {
		var defaults = {
			// 设置id
			id : $(this).attr("id"),
			// 指定绑定源
			renderTo : $(this).parent(),
			// 地区数据源
			items : new Array(),
			// 默认选中项,优先从this的data-select属性获取默认选中值
			select : '',
			// 关闭时回调
			closeFn : function() {
			}
		};

		var params = $.extend(defaults, options);
		params.renderTo = (typeof params.renderTo == 'string' ? $(params.renderTo) : params.renderTo);
		/**
		 * 遍历
		 */
		this.each(function() {
			var _this = $(this);
			var district=_this.siblings('.himall-district');
			
			if (params.items.length < 0) {
				return false;
			}
			if (!(params.id.length > 0)) {
				return false;
			}
			var thisId = 'himallDistrict-' + params.id;
			var arrSelect = params.select.split(',');
			if (_this.data("select")) {
				arrSelect = _this.data("select").split(',');
			}

			var provinceStr='<ul class="district-ul province-ul cl">';
			for(var i=0; i<params.items.length;i++){
				provinceStr+='<li><a data-id="'+params.items[i].id+'" data-index="'+i+'">'+params.items[i].name+'</a></li>';
			}
			provinceStr+='</ul>';
			
			var himallDistrict = '<div class="himall-district" id="'+thisId+'"><div class="district-hd"><span>请选择省</span></div>'+provinceStr+'<ul class="district-ul city-ul cl"></ul><ul class="district-ul county-ul cl"></ul></div>';
			
			
			if(params.renderTo.find("#"+thisId).length>0){
				if($("#"+thisId).is(':visible')){
					$("#"+thisId).hide();
					_this.removeClass('active');
				}else{
					$("#"+thisId).show();
					_this.addClass('active');
				}
				
			}else{
				params.renderTo.append(himallDistrict);
				_this.addClass('active');
			}
			
			
			var headSelect=params.renderTo.find('.district-hd');
			
			
			//省级点击
			var provinceId,
				cityId,
				provinceIndex;
			params.renderTo.on('click','.province-ul a',function() {
				var cityStr='';
				provinceIndex=$(this).data('index');
				var parent=params.items[provinceIndex];
				provinceId=parent.id;
				$(this).parent().addClass('cur').siblings().removeClass();
				$(this).parents('.district-ul').hide().siblings('ul.city-ul').show();

				headSelect.html('<span>'+$(this).text()+'</span><span class="cur">请选择市</span>')
				for(var i=0; i<parent.city.length;i++){
					cityStr+='<li><a data-id="'+parent.city[i].id+'" data-index="'+i+'">'+parent.city[i].name+'</a></li>';
				}
				params.renderTo.find('.city-ul').html(cityStr);
			});
			
			//市级点击
			params.renderTo.off('click','.city-ul a');
			params.renderTo.on('click','.city-ul a',function() {
				var countyStr='';
				var parent=params.items[provinceIndex].city[$(this).data('index')];
				cityId=parent.id;
				$(this).parent().addClass('cur').siblings().removeClass();
				if(parent.county!=null){
					$(this).parents('.district-ul').hide().siblings('ul.county-ul').show();
					headSelect.html('<span>'+headSelect.children().eq(0).text()+'</span><span>'+$(this).text()+'</span><span class="cur">请选择区</span>');
					for(var i=0; i<parent.county.length;i++){
						countyStr+='<li><a data-id="'+parent.county[i].id+'">'+parent.county[i].name+'</a></li>';
					}
					params.renderTo.find('.county-ul').html(countyStr);
				}else{
					headSelect.html('<span>'+headSelect.children().eq(0).text()+'</span><span class="cur">'+$(this).text()+'</span>');
					_this.removeClass('active').data('select',provinceId+','+cityId).html(headSelect.children().eq(0).text()+' '+headSelect.children().eq(1).text());
					$(this).parents('.himall-district').hide();
					params.closeFn();
				}
				
			});
			
			//区级点击
			params.renderTo.off('click','.county-ul a');
			params.renderTo.on('click','.county-ul a',function() {
				headSelect.children().eq(2).text($(this).text());
				_this.removeClass('active').data('select',provinceId+','+cityId+','+$(this).data('id')).html(headSelect.children().eq(0).text()+' '+headSelect.children().eq(1).text()+' '+headSelect.children().eq(2).text());
				$(this).parent().addClass('cur').siblings().removeClass();
				$(this).parents('.himall-district').hide();
				params.closeFn();
				
			} );
			
			//切换卡点击
			params.renderTo.on('click','.district-hd span',function() {
				$(this).addClass('cur').siblings().removeClass().parent().siblings('ul').hide().eq($(this).index()).show();
			});
			
			//初始化数据
			if(arrSelect){
				var arrSet = _this.text().split(' ');
				if(arrSet.length==3){
					params.renderTo.find('.province-ul').find('a[data-id="'+arrSelect[0]+'"]').click();
					params.renderTo.find('.city-ul').find('a[data-id="'+arrSelect[1]+'"]').click();
					params.renderTo.find('.county-ul').find('a[data-id="'+arrSelect[2]+'"]').parent().addClass('cur');
					headSelect.html('<span>'+arrSet[0]+'</span><span>'+arrSet[1]+'</span><span>'+arrSet[2]+'</span>');
				}else if(arrSet.length==2){
					params.renderTo.find('.province-ul').find('a[data-id="'+arrSelect[0]+'"]').click();
					params.renderTo.find('.city-ul').find('a[data-id="'+arrSelect[1]+'"]').parent().addClass('cur');
					headSelect.html('<span>'+arrSet[0]+'</span><span>'+arrSet[1]+'</span>');
				}
				headSelect.children().first().click();
				params.closeFn();
			}
		});
	};
})(jQuery);