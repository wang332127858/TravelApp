$(function () {
    //模块类型1 富文本
    HiShop.DIY.Unit.event_type1 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom;//控制内容

        //如果之前存在编辑器则销毁
        if (data.ue) {
            data.ue.destroy();
        }

        data.ue = UE.getEditor('editor' + data.id); //创建编辑器

        data.ue.ready(function () {
            data.ue.setContent(HiShop.DIY.Unit.html_decode(data.content.fulltext)); //设置编辑器的默认值
            data.ue.focus(true); //编辑器获得焦点
            //当值改变时反应到手机视图中
            var reSetVal = function () {
                var val = data.ue.getContent();
                if (val == "") val = "<p>『富文本编辑器』</p>";
                $conitem.find(".fulltext").html(val); //更新到手机视图
                data.content.fulltext = HiShop.DIY.Unit.html_encode(val); //更新到缓存
            }
            data.ue.addListener("selectionchange", reSetVal);
            data.ue.addListener("contentChange", reSetVal);
        });
    };

    //模块类型2 标题
    HiShop.DIY.Unit.event_type2 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom; //控制内容

        data.dom_ctrl = ctrldom;
        var render = function () {

        }
        //主标题
        $ctrl.find("input[name='title']").change(function () {
            var val = $(this).val();
            $conitem.find(".j-title").text($(this).val());
            data.content.title = val;
        });
        //副标题
        $ctrl.find("input[name='subtitle']").change(function () {
            var val = $(this).val();
            $conitem.find(".j-subtitle").text($(this).val());
            data.content.subtitle = val;
        });
        //显示方式
        $ctrl.find("input[name='direction']").change(function () {
            var val = $(this).val();
            $conitem.find(".members_special").removeClass("left center right").addClass(val);
            data.content.direction = val;
        });
        // 显示类型
        $ctrl.find("input[name='titlestyle']").change(function () {
            var val = $(this).val();
            $conitem.find(".members_special").attr('class', 'members_special titlestyle' + val);

            data.content.style = val;
        });
    };

    //模块类型3 自定义模块
    HiShop.DIY.Unit.event_type3 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom; //控制内容

        data.dom_ctrl = ctrldom;

        //修改事件
        var modify = function () {
            HiShop.popbox.ModulePicker(function (module) {
                $conitem.find(".type3_custModule").text(module.title);//修改手机内容
                $ctrl.find(".type3_custModule_ctrl").text(module.title);//修改控制内容
                data.content = module;
            });
        };

        //添加事件
        var add = function () {
            HiShop.popbox.ModulePicker(function (module) {
                $conitem.find(".type3_custModule").text(module.title);//修改手机内容
                //去掉添加按钮，替换为修改按钮
                var html = _.template($("#tpl_diy_ctrl_type3_modify").html(), { content: module }),
                    $render = $(html);

                $render.filter(".j-btn-modify").click(modify);//绑定修改按钮的事件
                $ctrl.find(".form-controls").empty().append($render);//替换dom内容
                data.content = module;
            });
        };

        $ctrl.find(".j-btn-add").click(add);//初始化模块时的修改按钮时间
        $ctrl.find(".j-btn-modify").click(modify);//初始化模块时的修改按钮时间
    };

    //模块类型4 商品
    HiShop.DIY.Unit.event_type4 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type4").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type4").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function (callback) {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type4($ctrl, data);
            if (callback) callback();
        }
        var setImgHeightcallback = function () {
            $('.mingoods').each(function (index, el) {
                var me = $(this),
                    imgHeight = me.width();
                me.find('img').closest('a').height(imgHeight);
            });
        };
        //改变布局
        $ctrl.find("input[name='layout']").change(function () {
            var val = $(this).val();
            data.content.layout = val;//同步数据到缓存
            reRender(setImgHeightcallback);
        });

        //是否显示商品名称
        $ctrl.find("input[name='showName']").change(function () {
            var val = $(this).val();
            data.content.showName = val;//同步数据到缓存
            reRender();
        });

        //是否显示购物车图标
        $ctrl.find("input[name='showIco']").change(function () {
            var val = $(this).is(":checked");
            data.content.showIco = val;//同步数据到缓存
            reRender();
        });

        //是否显示商品价格
        $ctrl.find("input[name='showPrice']").change(function () {
            var val = $(this).is(":checked");
            data.content.showPrice = val;//同步数据到缓存
            reRender();
        });

        //删除商品
        $ctrl.find(".j-delgoods").click(function () {
            var index = $(this).parents("li").index();
            data.content.goodslist.splice(index, 1);
            reRender(setImgHeightcallback);
            return false;
        });

        //添加商品
        $ctrl.find(".j-addgoods").click(function () {
            HiShop.popbox.GoodsAndGroupPicker("goodsMulti", function (list) {
                _.each(list, function (goods) {
                    data.content.goodslist.push(goods);
                });
                reRender(setImgHeightcallback);
            });
            return false;
        });
    };

    //模块类型5 商品列表（分组标签）
    HiShop.DIY.Unit.event_type5 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type5").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type5").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function () {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type5($ctrl, data);
        }

        //改变布局
        $ctrl.find("input[name='layout']").change(function () {
            var val = $(this).val();
            data.content.layout = val;//同步数据到缓存
            reRender();
        });

        //是否显示商品名称
        $ctrl.find("input[name='showName']").change(function () {
            var val = $(this).is(":checked");
            data.content.showName = val;//同步数据到缓存
            reRender();
        });

        //是否显示购物车图标
        $ctrl.find("input[name='showIco']").change(function () {
            var val = $(this).is(":checked");
            data.content.showIco = val;//同步数据到缓存
            reRender();
        });

        //是否显示商品价格
        $ctrl.find("input[name='showPrice']").change(function () {
            var val = $(this).is(":checked");
            data.content.showPrice = val;//同步数据到缓存
            reRender();
        });

        //是否第一级优先
        $ctrl.find("select[name='firstPriority']").change(function () {
            var val = $(this).val();
            data.content.firstPriority = val;//同步数据到缓存
            reRender();
        });

        //是否第二级优先
        $ctrl.find("select[name='thirdPriority']").change(function () {
            var val = $(this).val();
            data.content.thirdPriority = val;//同步数据到缓存
            reRender();
        });

        //是否第二级优先
        $ctrl.find("select[name='secondPriority']").change(function () {
            var val = $(this).val();
            data.content.secondPriority = val;//同步数据到缓存
            reRender();
        });

        //选择/修改分组
        $ctrl.find(".j-btn-add,.j-btn-modify").click(function () {
            HiShop.popbox.GoodsAndGroupPicker("group", function (group) {
                data.content.group = group;
                reRender();
            });
        });
        // 选择商品显示数量
        $ctrl.find('input[name="goodsize"]').change(function (event) {
            var me = $(this),
                num = me.val();
            data.content.goodsize = num;
            reRender();
        });
    };

    //模块类型7 文本导航
    HiShop.DIY.Unit.event_type7 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type7").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type7").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function () {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type7($ctrl, data);
        }

        //改变标题
        $ctrl.find("input[name='title']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].showtitle = $(this).val();
            reRender();
        });

        //改变链接
        $ctrl.find(".droplist li").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    data.content.dataset[index].title = item.title;
                    data.content.dataset[index].showtitle = item.title;
                    data.content.dataset[index].link = item.link;
                    data.content.dataset[index].linkType = type;
                    reRender();
                }
            });
        });

        //自定义链接
        $ctrl.find("input[name='customlink']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].link = $(this).val();
        });

        //上移
        $ctrl.find(".j-moveup").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            if (index == 0) return;//第一个导航不可再向上移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index - 1, 0, tmpdata);

            reRender();//更新视图
        });

        //下移
        $ctrl.find(".j-movedown").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                len = data.content.dataset.length;

            if (index == len - 1) return;//最后一个导航不可再向下移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index + 1, 0, tmpdata);

            reRender();//更新视图
        });

        //添加
        $ctrl.find(".ctrl-item-list-add").click(function () {
            var newdata = {
                linkType: 0,
                link: "",
                title: "",
                showtitle: ""
            };
            data.content.dataset.push(newdata);
            reRender();
        });

        //删除
        $ctrl.find(".j-del").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset.splice(index, 1);
            reRender();
        });
    };

    //模块类型8 图片导航
    HiShop.DIY.Unit.event_type8 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type8").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type8").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function (callback) {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type8($ctrl, data);
            if (callback) callback();
        }

        // 控制图片高度
        var setImgHeightcallback = function () {
            $('.members_nav1 ul li').each(function (index, el) {
                var me = $(this),
                    imgHeight = me.find('img').width();
                me.find('img').height(imgHeight);
            });
        }
        //改变标题
        $ctrl.find("input[name='title']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].showtitle = $(this).val();
            reRender();
        });

        //改变链接
        $ctrl.find(".droplist li").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    data.content.dataset[index].title = item.title;
                    data.content.dataset[index].showtitle = item.title;
                    data.content.dataset[index].link = item.link;
                    data.content.dataset[index].linkType = type;
                    if (item.pic && item.pic != "") {
                        data.content.dataset[index].pic = item.pic;
                    }
                    reRender(setImgHeightcallback);
                }
            });
        });

        //选择图片
        $ctrl.find(".j-selectimg").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.ImgPicker(function (imgs) {
                data.content.dataset[index].pic = imgs[0];//获取第一张图片
                reRender(setImgHeightcallback);
            });
        });

        //自定义链接
        $ctrl.find("input[name='customlink']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].link = $(this).val();
        });

        //上移
        $ctrl.find(".j-moveup").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            if (index == 0) return;//第一个导航不可再向上移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index - 1, 0, tmpdata);

            reRender();//更新视图
        });

        //下移
        $ctrl.find(".j-movedown").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                len = data.content.dataset.length;

            if (index == len - 1) return;//最后一个导航不可再向下移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index + 1, 0, tmpdata);

            reRender();//更新视图
        });

        //添加
        $ctrl.find(".ctrl-item-list-add").click(function () {
            var newdata = {
                linkType: 0,
                link: "",
                title: "",
                showtitle: "",
                pic: ""
            };
            data.content.dataset.push(newdata);
            reRender();
        });

        //删除
        $ctrl.find(".j-del").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset.splice(index, 1);
            reRender();
        });
    };

    //模块类型9 广告图片
    HiShop.DIY.Unit.event_type9 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type9").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type9").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function () {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type9($ctrl, data);
        }

        //改变标题
        $ctrl.find("input[name='title']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].showtitle = $(this).val();
            reRender();
        });

        //改变显示方式
        $ctrl.find("input[name='showType']").change(function () {
            data.content.showType = $(this).val();
            reRender();
        });

        // 是否留白
        $ctrl.find("input[name='space']").change(function () {
            data.content.space = $(this).val();
            reRender();
        });
        // 为非滚动图片时的上下距离
        $ctrl.find("#slider").slider({
            min: 0,
            max: 20,
            step: 1,
            animate: "fast",
            value: data.content.margin,
            slide: function (event, ui) {
                $conitem.find(".members_imgad ul li").css("margin-bottom", ui.value);
                $ctrl.find(".j-ctrl-showheight").text(ui.value + "px");
            },
            stop: function (event, ui) {
                data.content.margin = parseInt(ui.value);
            }
        });
        //改变链接
        $ctrl.find(".droplist li").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    data.content.dataset[index].title = item.title;
                    data.content.dataset[index].showtitle = item.title;
                    data.content.dataset[index].link = item.link;
                    data.content.dataset[index].linkType = type;
                    if (item.pic && item.pic != "") {
                        data.content.dataset[index].pic = item.pic;
                    }
                    reRender();
                }
            });
        });

        //选择图片
        $ctrl.find(".j-selectimg").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.ImgPicker(function (imgs) {
                data.content.dataset[index].pic = imgs[0];//获取第一张图片
                reRender();
            });
        });

        //自定义链接
        $ctrl.find("input[name='customlink']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].link = $(this).val();
        });

        //上移
        $ctrl.find(".j-moveup").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            if (index == 0) return;//第一个导航不可再向上移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index - 1, 0, tmpdata);

            reRender();//更新视图
        });

        //下移
        $ctrl.find(".j-movedown").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                len = data.content.dataset.length;

            if (index == len - 1) return;//最后一个导航不可再向下移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index + 1, 0, tmpdata);

            reRender();//更新视图
        });

        //添加
        $ctrl.find(".ctrl-item-list-add").click(function () {
            var newdata = {
                linkType: 0,
                link: "",
                title: "",
                showtitle: "",
                pic: ""
            };
            data.content.dataset.push(newdata);
            reRender();
        });

        //删除
        $ctrl.find(".j-del").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset.splice(index, 1);
            reRender();
        });
    };

    //模块类型11 辅助空白
    HiShop.DIY.Unit.event_type11 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type11").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type11").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function () {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type11($ctrl, data);
        }

        $ctrl.find("#slider").slider({
            min: 10,
            max: 100,
            step: 1,
            animate: "fast",
            value: data.content.height,
            slide: function (event, ui) {
                $conitem.find(".custom-space").css("height", ui.value);
                $ctrl.find(".j-ctrl-showheight").text(ui.value + "px");
            },
            stop: function (event, ui) {
                data.content.height = parseInt(ui.value);
            }
        });
    };

    //模块类型12 顶部导航
    HiShop.DIY.Unit.event_type12 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type12").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type12").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function () {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".Header_style12_panel").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);

            HiShop.DIY.Unit.event_type12($ctrl, data);
        }

        //导航名称修改
        $ctrl.find("input[name='navtitle']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                val = $(this).val();

            data.content.dataset[index].showtitle = val;
            reRender();
        });

        //改变链接
        $ctrl.find(".droplist li").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    data.content.dataset[index].title = item.title;
                    data.content.dataset[index].showtitle = item.title;
                    data.content.dataset[index].link = item.link;
                    data.content.dataset[index].linkType = type;
                    if (item.pic && item.pic != "") {
                        data.content.dataset[index].pic = item.pic;
                    }
                    reRender();
                }
            });
        });

        //选择图片
        $ctrl.find(".j-selectimg").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.ImgPicker(function (imgs) {
                data.content.dataset[index].pic = imgs[0];//获取第一张图片
                reRender();
            });

        });

        //自定义链接
        $ctrl.find("input[name='customlink']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].link = $(this).val();
        });

        //修改导航背景颜色
        $ctrl.find("select[name='navbgColor']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                val = $(this).val();

            data.content.dataset[index].bgColor = val;
            reRender();
        });


        // 显示导航类型
        $ctrl.find('input[name="showstyle"]').change(function (event) {
            var me = $(this),
                style = me.val();
            data.content.style = style;
            reRender();
        });

        // 显示导航是否有边距
        $ctrl.find('input[name="marginstyle"]').change(function (event) {
            var me = $(this),
                marginstyle = me.val();
            data.content.marginstyle = marginstyle;
            reRender();
        });
        //导航颜色选择器
        // $ctrl.find(".colorPicker").each(function(e){
        //     var name=$(this).data("name"),
        //         color=$(this).data("color"),
        //         selector="#j_clp_col"+name;
        //         // alert($(selector).html());
        //     $(this).ColorPicker({
        //         color: color,
        //         onShow: function (colpkr) {
        //             $(colpkr).fadeIn(500);
        //             return false;
        //         },
        //         onHide: function (colpkr) {
        //             $(colpkr).fadeOut(500);
        //             reRender();
        //             return false;
        //         },
        //         onChange: function (hsb, hex, rgb) {
        //             var hex='#' + hex;
        //             $(selector).css("background-color",hex);
        //             data.content.dataset[e].bgColor=hex;
        //         }
        //     });
        // });
        $ctrl.find('.ctrl-item-list-li').each(function (index, ell) {
            var me = $(this);
            me.find('.colorPicker').each(function (indexs, el) {
                var _this = $(this);
                if (indexs == 0) {
                    var name = $(this).data("name"),
                        color = $(this).data("color"),
                        selector = "#j_clp_col" + name;
                    // alert($(selector).html());
                    _this.ColorPicker({
                        color: color,
                        onShow: function (colpkr) {
                            $(colpkr).fadeIn(100);
                            return false;
                        },
                        onHide: function (colpkr) {
                            $(colpkr).fadeOut(100);
                            reRender();
                            return false;
                        },
                        onChange: function (hsb, hex, rgb) {
                            var hex = '#' + hex;
                            _this.css("background-color", hex);
                            data.content.dataset[index].bgColor = hex;
                        }
                    });
                } else {
                    var name = $(this).data("name"),
                        color = $(this).data("color"),
                        selector = "#j_clp_col" + name;
                    // alert($(selector).html());
                    _this.ColorPicker({
                        color: color,
                        onShow: function (colpkr) {
                            $(colpkr).fadeIn(100);
                            return false;
                        },
                        onHide: function (colpkr) {
                            $(colpkr).fadeOut(100);
                            reRender();
                            return false;
                        },
                        onChange: function (hsb, hex, rgb) {
                            var hex = '#' + hex;
                            $(selector).css("background-color", hex);
                            data.content.dataset[index].fotColor = hex;
                        }
                    });
                }
            });
        });
        // 上传导航小图标
        $ctrl.find('.j-uploadIcon').click(function (event) {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            HiShop.popbox.ImgPicker(function (imgs) {
                data.content.dataset[index].pic = imgs[0];
                reRender();
            });
        });
        //修改导航小图标
        $ctrl.find(".j-navModifyIcon").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            // HiShop.popbox.ImgPicker(function(imgs){

            //     data.content.dataset[index].pic=imgs[0];
            //     reRender();
            // });
            HiShop.popbox.IconPicker(function (imgs) {
                console.log(imgs)
                data.content.dataset[index].pic = imgs[0];
                reRender();
            });
        });
        //上移
        $ctrl.find(".j-moveup").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            if (index == 0) return;//第一个导航不可再向上移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index - 1, 0, tmpdata);

            reRender();//更新视图
        });

        //下移
        $ctrl.find(".j-movedown").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                len = data.content.dataset.length;

            if (index == len - 1) return;//最后一个导航不可再向下移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index + 1, 0, tmpdata);

            reRender();//更新视图
        });

        //添加
        $ctrl.find(".ctrl-item-list-add").click(function () {
            var newdata = {
                linkType: 0,
                link: "",
                title: "",
                showtitle: "",
                pic: ""
            };
            data.content.dataset.push(newdata);
            reRender();
        });

        //删除
        $ctrl.find(".j-del").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset.splice(index, 1);
            reRender();
        });
    };

    //模块类型13 橱窗
    HiShop.DIY.Unit.event_type13 = function (ctrldom, data) {
        var $conitem = data.dom_conitem, //手机内容
            $ctrl = ctrldom, //控制内容
            tpl_con = $("#tpl_diy_con_type13").html(),//手机内容模板
            tpl_ctrl = $("#tpl_diy_ctrl_type13").html();//控制内容模板

        data.dom_ctrl = ctrldom;

        //重新渲染数据
        var reRender = function (callback) {
            var $render = $(_.template(tpl_con, data));
            $conitem.find(".members_con").remove().end().append($render);

            var $render_ctrl = $(_.template(tpl_ctrl, data));
            $ctrl.empty().append($render_ctrl);
            HiShop.DIY.Unit.event_type13($ctrl, data);
            if (callback) callback();
        }
        // 设置宽高
        var setImgHeight = function () {
            var inputval = $('input[name=layout]:checked').val();
            if (parseInt(inputval) == 1) {
                $('.board3').each(function (index, el) {
                    var me = $(this);
                    var bwidth = me.width() * 0.6;
                    //me.height(bwidth).css('overflow', 'hidden');
                    if (me.hasClass('small_board') || !me.hasClass('big_board')) {
                        me.attr('style', 'height:' + bwidth + 'px !important;overflow:hidden;');
                    }
                    if (me.hasClass('big_board')) {
                        me.attr('style', 'height:' + (bwidth * 2 + 4) + 'px !important;overflow:hidden;');
                    }
                });
            } else {
                $('.board3').each(function (index, el) {
                    var me = $(this);
                    var bwidth = me.width() * 0.6;
                    if (me.hasClass('small_board') || !me.hasClass('big_board')) {
                        me.attr('style', 'height:' + bwidth + 'px !important;overflow:hidden;');
                    }
                    if (me.hasClass('big_board')) {
                        me.attr('style', 'height:' + (bwidth * 2 + 4) + 'px !important;overflow:hidden;');
                    }
                });
            };
        };
        // 选择布局方式
        $ctrl.find('input[name="layout"]').change(function (event) {
            var me = $(this),
                layout = me.val();
            data.content.layout = layout;
            reRender(setImgHeight);
        });
        //改变标题
        $ctrl.find("input[name='title']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].showtitle = $(this).val();
            reRender();
        });

        //改变链接
        $ctrl.find(".droplist li").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    data.content.dataset[index].title = item.title;
                    data.content.dataset[index].showtitle = item.title;
                    data.content.dataset[index].link = item.link;
                    data.content.dataset[index].linkType = type;
                    if (item.pic && item.pic != "") {
                        data.content.dataset[index].pic = item.pic;
                    }
                    reRender();
                }
            });
        });

        //选择图片
        $ctrl.find(".j-selectimg").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            HiShop.popbox.ImgPicker(function (imgs) {
                data.content.dataset[index].pic = imgs[0];//获取第一张图片
                reRender(setImgHeight);
            });
        });

        //自定义链接
        $ctrl.find("input[name='customlink']").change(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset[index].link = $(this).val();
        });

        //上移
        $ctrl.find(".j-moveup").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();

            if (index == 0) return;//第一个导航不可再向上移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index - 1, 0, tmpdata);

            reRender();//更新视图
        });

        //下移
        $ctrl.find(".j-movedown").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index(),
                len = data.content.dataset.length;

            if (index == len - 1) return;//最后一个导航不可再向下移动

            //替换缓存数组中的位置
            var tmpdata = data.content.dataset.slice(index, index + 1)[0];
            data.content.dataset.splice(index, 1);
            data.content.dataset.splice(index + 1, 0, tmpdata);

            reRender();//更新视图
        });

        //添加
        $ctrl.find(".ctrl-item-list-add").click(function () {
            var newdata = {
                linkType: 0,
                link: "",
                title: "",
                showtitle: "",
                pic: ""
            };
            data.content.dataset.push(newdata);
            reRender();
        });

        //删除
        $ctrl.find(".j-del").click(function () {
            var index = $(this).parents("li.ctrl-item-list-li").index();
            data.content.dataset.splice(index, 1);
            reRender();
        });

    };
});