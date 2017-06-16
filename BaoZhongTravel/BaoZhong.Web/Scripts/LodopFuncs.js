var CreatedOKLodop7766 = null;

function getLodop(oOBJECT, oEMBED) {

    var strHtmInstall = "打印控件未安装!点击这里<a style='color:#e3393c' href='/Printer/install_lodop32.exe' target='_self'>执行安装</a>,安装后请刷新页面或重新进入。";
    var strHtmUpdate = "打印控件需要升级!点击这里<a style='color:#e3393c' href='/Printer/install_lodop32.exe' target='_self'>执行升级</a>,升级后请重新进入。";
    var strHtmFireFox = "<br><br><font color='#FF00FF'>（注意：如曾安装过Lodop旧版附件npActiveXPLugin,请在【工具】->【附加组件】->【扩展】中先卸它）</font>";
    var strHtmChrome = "<br><br><font color='#FF00FF'>(如果此前正常，仅因浏览器升级或重安装而出问题，需重新执行以上安装）</font>";
    var LODOP;
    try {
        //=====判断浏览器类型:===============
        var isIE = (navigator.userAgent.indexOf('MSIE') >= 0) || (navigator.userAgent.indexOf('Trident') >= 0);
        var is64IE = isIE && (navigator.userAgent.indexOf('x64') >= 0);
        //var x = navigator.userAgent.match(/x86_64|Win64|WOW64/) || navigator.cpuClass === 'x64' ? 'x64' : 'x86';
        //if (x=="x64"){
        //	strHtmInstall = "打印控件未安装!点击这里<a style='color:#e3393c' href='/Printer/install_lodop64.exe' target='_self'>执行安装</a>,安装后请刷新页面或重新进入。";
        //	strHtmUpdate = "打印控件需要升级!点击这里<a style='color:#e3393c' href='/Printer/install_lodop64.exe' target='_self'>执行升级</a>,升级后请重新进入。";
        //}
        //=====如果页面有Lodop就直接使用，没有则新建:==========
        if (oOBJECT != undefined || oEMBED != undefined) {
            if (isIE)
                LODOP = oOBJECT;
            else
                LODOP = oEMBED;
        } else {
            if (CreatedOKLodop7766 == null) {
                LODOP = document.createElement("object");
                LODOP.setAttribute("width", 0);
                LODOP.setAttribute("height", 0);
                LODOP.setAttribute("style", "position:absolute;left:0px;top:-100px;width:0px;height:0px;");
                if (isIE) LODOP.setAttribute("classid", "clsid:2105C259-1E0C-4534-8141-A753534CB4CA");
                else LODOP.setAttribute("type", "application/x-print-lodop");
                document.documentElement.appendChild(LODOP);
                CreatedOKLodop7766 = LODOP;
            } else
                LODOP = CreatedOKLodop7766;
        };
        //=====判断Lodop插件是否安装过，没有安装或版本过低就提示下载安装:==========
        if ((LODOP == null) || (typeof (LODOP.VERSION) == "undefined")) {
            if (navigator.userAgent.indexOf('Chrome') >= 0) {
                $.dialog.alert(strHtmChrome);
            }
            if (navigator.userAgent.indexOf('Firefox') >= 0) {
                $.dialog.alert(strHtmFireFox);
            }
            if (is64IE) {
                $.dialog.alert(strHtm64_Install);
            } else if (isIE) {
                $.dialog.alert(strHtmInstall);
            } else {
                $.dialog.alert(strHtmInstall);
            }
        }

        if (LODOP.VERSION < "6.2.0.5") {
            if (is64IE) {
                $.dialog.alert(strHtm64_Install);
            } else if (isIE) {
                $.dialog.alert(strHtmInstall);
            } else {
                $.dialog.alert(strHtmInstall);
            }
        }
        //=====如下空白位置适合调用统一功能(如注册码、语言选择等):====	     
        LODOP.SET_LICENSES("长沙海商网络技术有限公司", "659597279737383919278901905623", "", "");

        //============================================================	     
        return LODOP;
    } catch (err) {
        $.dialog.alert(strHtmInstall);
        return LODOP;
    };
}
