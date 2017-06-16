
namespace BaoZhong.Model
{
    /// <summary>
    /// desc:平台权限枚举
    /// author:cgm
    /// date:2016/11/9 
    /// </summary>
    public enum AdminPrivilege
    {
        [Privilege("系统", "管理员", 1001, "Manager/management", "Manager", "")]
        AdminManage = 1001,
        [Privilege("系统", "权限组", 1002, "Privilege/management", "privilege", "")]
        PrivilegesManage,
        [Privilege("系统", "分红权", 1004, "Divide/Edit", "Divide", "")]
        DivideRights,
        [Privilege("系统", "协议管理", 1005, "Agreement/management", "Agreement", "")]
        Agreement,
        [Privilege("系统", "消费币兑换规则", 1006, "IntegralRule/management", "IntegralRule", "")]
        IntegralRule,
        [Privilege("系统", "代缴提醒规则", 1007, "PayRule/management", "PayRule", "")]
        PayRule,
        [Privilege("系统", "客服电话", 1008, "ServicePhone/management", "ServicePhone", "")]
        ServicePhoneManage,
        [Privilege("系统", "关于我们", 1009, "Aboutus/management", "Aboutus", "")]
        AboutusManage,
        [Privilege("系统", "操作日志", 1010, "OperationLog/management", "OperationLog", "")]
        OperationLog,
        [Privilege("系统", "站点设置", 1011, "SiteSetting/Edit", "SiteSetting", "")]
        SiteSetting,
        [Privilege("系统", "广告推送设置", 1012, "MoveAdvertConfig/management", "MoveAdvertConfig", "")]
        MoveAdvertConfig,
        [Privilege("系统", "通告推送设置", 1013, "MoveNoticeConfig/management", "MoveNoticeConfig", "")]
        MoveNoticeConfig,
        [Privilege("平台", "余额查询", 2001, "PlatBalance/management", "PlatBalance", "")]
        PlatBalanceQuery = 2001,
        [Privilege("平台", "提现查询", 2002, "MarketingAtm/management", "MarketingAtm", "")]
        MarketingAtm,
        [Privilege("店铺", "分类管理", 3001, "Category/management", "category", "")]
        CategoryManage = 3001,
        [Privilege("店铺", "店铺管理", 3002, "shop/management?type=Auditing", "Shop", "")]
        ShopManage,
        [Privilege("商家", "商家管理", 4001, "Business/management", "Business", "")]
        BusinessManagement = 4001,
        [Privilege("商家", "余额查询", 4002, "BusinessBalance/management", "BusinessBalance", "")]
        BusinessBalanceQuery,
        [Privilege("商家", "充值查询", 4003, "BusinessRecharge/management", "BusinessRecharge", "")]
        BusinessRechargeQuery,
        [Privilege("商家", "分红查询", 4004, "BusinessDivide/management", "BusinessDivide", "")]
        BusinessDivideQuery,
        [Privilege("商家", "提现查询", 4005, "BusinessAtm/management", "BusinessAtm", "")]
        BusinessAtmQuery,
        [Privilege("商家", "代缴查询", 4006, "BusinessAgent/management", "BusinessAgent", "")]
        BusinessAgentQuery,
        [Privilege("商家", "代缴提醒", 4007, "BusinessPayNotice/management", "BusinessPayNotice", "")]
        BusinessPayNotice,
        [Privilege("会员", "会员管理", 5001, "member/management", "member", "")]
        MemberManage = 5001,
        [Privilege("会员", "余额查询", 5002, "MemberCapital/management", "MemberCapital", "")]
        MemberCapitalQuery,
        [Privilege("会员", "消费查询", 5003, "MemberConsumer/management", "MemberConsumer", "")]
        MemberConsumerRecord,
        [Privilege("会员", "提现查询", 5004, "MemberAtm/management", "MemberAtm", "")]
        MemberAtmQuery,
        [Privilege("会员", "分红查询", 5005, "MemberBonus/management", "MemberBonus", "")]
        MemberBonusQuery,
        [Privilege("交易", "提现处理", 6001, "AtmRefund/management", "AtmRefund", "")]
        AtmRefundManage = 6001,
        [Privilege("分组", "分组管理", 7001, "Group/management", "Group", "")]
        GroupManage = 7001,
        [Privilege("分组", "分组列表", 7002, "GroupMember/management", "GroupMember", "")]
        GroupMember,
        [Privilege("营销", "营销管理", 8001, "Marketing/management", "Marketing", "")]
        MarketingManage = 8001,
        [Privilege("营销", "人员管理", 8002, "MarketingMember/management", "MarketingMember", "")]
        MarketingMember,
        [Privilege("页面", "横滚动广告", 9001, "SlideAd/SlideManagement", "SlideAd", "")]
        SlideAdManage = 9001,
        [Privilege("页面", "首页广告", 9002, "HomeAd/HomeManagement", "HomeAd", "")]
        HomeAdManage,
        [Privilege("统计", "每日分红总计", 10001, "EveryDayRecord/Management", "EveryDayRecord", "")]
        EveryDayRecord = 10001,
        [Privilege("统计", "地区统计", 10002, "RegionRecord/Management", "RegionRecord", "")]
        RegionRecord,
        [Privilege("统计", "今天分红池记录", 10003, "EveryDaySharePondRecord/Management", "EveryDaySharePondRecord", "")]
        EveryDaySharePondRecord,
        [Privilege("统计", "商家分红记录", 10004, "EveryDayShopDivideRecord/Management", "EveryDayShopDivideRecord", "")]
        EveryDayShopDivideRecord,
        [Privilege("统计", "用户分红记录", 10005, "EveryDayMemberDivideRecord/Management", "EveryDayMemberDivideRecord", "")]
        EveryDayMemberDivideRecord,
        [Privilege("统计", "商家分红权总计", 10006, "EveryDayShopRecord/Management", "EveryDayShopRecord", "")]
        EveryDayShopRecord,
        [Privilege("统计", "用户分红权总计", 10007, "EveryDayMemberRecord/Management", "EveryDayMemberRecord", "")]
        EveryDayMemberRecord,
    }
}
