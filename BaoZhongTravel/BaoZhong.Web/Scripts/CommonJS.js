$(function () {
    $.extend(Array.prototype, {
        indexOf: function (o) {
            for (var i = 0, len = this.length; i < len; i++) {
                if (this[i] == o) {
                    return i;
                }
            }
            return -1;
        }, remove: function (o) {
            var index = this.indexOf(o);
            if (index != -1) {
                this.splice(index, 1);
            }
            return this;
        }, removeById: function (filed, id) {
            for (var i = 0, len = this.length; i < len; i++) {
                if (this[i][filed] == id) {
                    this.splice(i, 1);
                    return this;
                }
            }
            return this;
        }
    });
});


function ajaxRequest(option) {
    $.ajax({
        type: option.type,
        url: option.url,
        cache: false,
        data: option.param,
        dataType: option.dataType,
        success: option.success,
        error: option.error
    });
}

//检查上传的图片格式
function checkImgType(filename) {
    var pos = filename.lastIndexOf(".");
    var str = filename.substring(pos, filename.length)
    var str1 = str.toLowerCase();
    if (!/\.(gif|jpg|jpeg|png|bmp)$/.test(str1)) {
        return false;
    }
    return true;
}

//通用loading变量
var loadingobj;

function showLoading(msg,delay) {
    /// <param name="msg" type="String">待显示的文本,非必填</param>
    /// <param name="delay" type="Int">延时显示的毫秒数，默认100毫秒显示,非必填</param>
    if (!delay)
        delay = 100;
    var loading = $('<div class="ajax-loading" style="display:none"><table height="100%" width="100%"><tr><td align="center"><p>' + (msg ? msg : '') + '</p></td></tr></table></div>');
    loading.appendTo('body');
    var s=setTimeout(function () {
		if($(".ajax-loading").length > 0){
			loading.show();
			$('.container,.login-box').addClass('blur');
		}
    }, delay);
    return {
        close: function () {
            clearTimeout(s);
            loading.remove();
			$('.container,.login-box').removeClass('blur');
        }
    }

}


function QueryString(name) {
    /// 获取QueryString

    var AllVars = window.location.search.substring(1);
    var Vars = AllVars.split("&");
    for (i = 0; i < Vars.length; i++) {
        var Var = Vars[i].split("=");
        if (Var[0] == name) return Var[1];
    }
    return "";
};


function AddFavorite(sURL, sTitle) {
    try {
        window.external.addFavorite(sURL, sTitle);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(sTitle, sURL, "");
        }
        catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
}

//表示全局唯一标识符 (GUID)。
function Guid(g) {
    var arr = new Array(); //存放32位数值的数组
    if (typeof (g) == "string") { //如果构造函数的参数为字符串
        InitByString(arr, g);
    }
    else {
        InitByOther(arr);
    }

    //返回一个值，该值指示 Guid 的两个实例是否表示同一个值。
    this.Equals = function (o) {
        if (o && o.IsGuid) {
            return this.ToString() == o.ToString();
        }
        else {
            return false;
        }
    }
    //Guid对象的标记
    this.IsGuid = function () { }
    //返回 Guid 类的此实例值的 String 表示形式。
    this.ToString = function (format) {
        if (typeof (format) == "string") {
            if (format == "N" || format == "D" || format == "B" || format == "P") {
                return ToStringWithFormat(arr, format);
            }
            else {
                return ToStringWithFormat(arr, "D");
            }
        }
        else {
            return ToStringWithFormat(arr, "D");
        }
    }
    //由字符串加载
    function InitByString(arr, g) {
        g = g.replace(/\{|\(|\)|\}|-/g, "");
        g = g.toLowerCase();
        if (g.length != 32 || g.search(/[^0-9,a-f]/i) != -1) {
            InitByOther(arr);
        }
        else {
            for (var i = 0; i < g.length; i++) {
                arr.push(g[i]);
            }
        }
    }
    //由其他类型加载
    function InitByOther(arr) {
        var i = 32;
        while (i--) {
            arr.push("0");
        }
    }
    /*
    根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
    N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
    P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
    */
    function ToStringWithFormat(arr, format) {
        switch (format) {
            case "N":
                return arr.toString().replace(/,/g, "");
            case "D":
                var str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                str = str.replace(/,/g, "");
                return str;
            case "B":
                var str = ToStringWithFormat(arr, "D");
                str = "{" + str + "}";
                return str;
            case "P":
                var str = ToStringWithFormat(arr, "D");
                str = "(" + str + ")";
                return str;
            default:
                return new Guid();
        }
    }
}
//Guid 类的默认实例，其值保证均为零。
Guid.Empty = new Guid();
//初始化 Guid 类的一个新实例。
Guid.NewGuid = function () {
    var g = "";
    var i = 32;
    while (i--) {
        g += Math.floor(Math.random() * 16.0).toString(16);
    }
    return new Guid(g);
}

//获取区域路径
//eg: /admin/home/index 页面调用此方法后返回 /admin/
function getAreaPath() {
    var path = location.pathname + '/';
    path = path.substring(1, path.length);
    path = path.substring(0, path.indexOf('/'));
    return '/' + path + '/';
}
//转换json传输date
function date_string(str) {
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.join('-');
}

//时间转换前位加零
function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }

//转换json传输date
function time_string(str, df) {
    df = df || "yyyy-MM-dd HH:mm:ss";
    var result = "";
    if (str == null || str.length<1)
    {
        return result;
    }
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    result = formatdata(d, df);
    return result;
}

function formatdata(data,fmt) {         
    var o = {         
        "M+": data.getMonth()+1, //月份         
        "d+": data.getDate(), //日         
        "h+": data.getHours() % 12 == 0 ? 12 : data.getHours() % 12, //小时         
        "H+": data.getHours(), //小时         
        "m+": data.getMinutes(), //分         
        "s+": data.getSeconds(), //秒         
        "q+": Math.floor((data.getMonth() + 3) / 3), //季度         
        "S": data.getMilliseconds() //毫秒         
    };         
    var week = {         
        "0" : "/u65e5",         
        "1" : "/u4e00",         
        "2" : "/u4e8c",         
        "3" : "/u4e09",         
        "4" : "/u56db",         
        "5" : "/u4e94",         
        "6" : "/u516d"        
    };         
    if(/(y+)/.test(fmt)){         
        fmt = fmt.replace(RegExp.$1, (data.getFullYear() + "").substr(4 - RegExp.$1.length));
    }         
    if(/(E+)/.test(fmt)){         
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[data.getDay() + ""]);
    }         
    for(var k in o){         
        if(new RegExp("("+ k +")").test(fmt)){         
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));         
        }         
    }         
    return fmt;         
}       

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

