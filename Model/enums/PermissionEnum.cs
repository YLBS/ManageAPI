
namespace Model.enums;

/// <summary>
/// 权限模块
/// </summary>
public enum SectionEnum
{
    /// <summary>
    /// 人才信息查看
    /// </summary>
    PersonView = 0,

    /// <summary>
    /// 人才相关管理
    /// </summary>
    Person = 20,

    /// <summary>
    /// 企业信息查看
    /// </summary>
    CompanyView = 40,

    /// <summary>
    /// 企业相关管理
    /// </summary>
    Company = 60,

    /// <summary>
    /// 服务申请订单处理
    /// </summary>
    ServiceOrder = 80,

    /// <summary>
    /// 网上支付订单处理
    /// </summary>
    PaymentOrder = 100,

    /// <summary>
    /// 在线广告管理
    /// </summary>
    AD = 120,

    /// <summary>
    /// 广告类别管理
    /// </summary>
    ADType = 140,

    /// <summary>
    /// 企业专页广告管理
    /// </summary>
    ADPage = 160,

    /// <summary>
    /// 地区站点管理
    /// </summary>
    AreaSite = 180,

    /// <summary>
    /// 推广站点管理
    /// </summary>
    DevelopSite = 200,

    /// <summary>
    /// 合作伙伴站点管理
    /// </summary>
    PartnerSite = 220,

    /// <summary>
    /// 合作伙伴结算管理
    /// </summary>
    PartnerPay = 240,

    /// <summary>
    /// 合作伙伴用户管理
    /// </summary>
    PartnerUser = 260,

    /// <summary>
    /// 用户问题处理
    /// </summary>
    FAQ = 280,

    /// <summary>
    /// 资讯类别管理
    /// </summary>
    NewsType = 300,

    /// <summary>
    /// 文章图片管理
    /// </summary>
    News = 320,

    /// <summary>
    /// 模板管理
    /// </summary>
    Template = 340,

    /// <summary>
    /// 标签管理
    /// </summary>
    Label = 360,

    /// <summary>
    /// 静态页面生成
    /// </summary>
    HtmlPage = 380,

    /// <summary>
    /// 数据统计查看
    /// </summary>
    Stat = 400,
}


/// <summary>
/// 人才信息查看
/// </summary>
[Flags]
public enum PersonViewEnum
{
    /// <summary>
    /// 快速/高级搜索简历
    /// </summary>
    SearchResume = 1,

    /// <summary>
    /// 查看简历
    /// </summary>
    ViewResume = 2,

    /// <summary>
    /// 查看人才账号密码
    /// </summary>
    ViewPersonUsername = 4,

    /// <summary>
    /// 查看简历附件
    /// </summary>
    ViewResumeAddin = 8,

    /// <summary>
    /// 人才信息查看所有权限
    /// </summary>
    All = 15
}


/// <summary>
/// 人才相关管理
/// </summary>
[Flags]
public enum PersonEnum
{
    /// <summary>
    /// 审核/取消审核/复审简历
    /// </summary>
    CheckResume = 1,

    /// <summary>
    /// 打印简历
    /// </summary>
    PrintResume = 2,

    /// <summary>
    /// 删除简历
    /// </summary>
    DeleteResume = 4,

    /// <summary>
    /// 邮件激活简历
    /// </summary>
    ActiveResume = 8,

    /// <summary>
    /// 推荐简历
    /// </summary>
    CommendResume = 16,

    /// <summary>
    /// 人才相关管理所有权限
    /// </summary>
    All = 31,

    /// <summary>
    /// 2012-12-25
    /// </summary>
    TestResume = 64

}


/// <summary>
/// 企业信息查看
/// </summary>
[Flags]
public enum CompanyViewEnum
{
    /// <summary>
    /// 搜索企业
    /// </summary>
    SearchCompany = 1,

    /// <summary>
    /// 查看企业基本资料
    /// </summary>
    ViewCompanyBaseInfo = 2,

    /// <summary>
    /// 查看企业服务资料
    /// </summary>
    ViewCompanyServiceInfo = 4,

    /// <summary>
    /// 查看企业账号密码
    /// </summary>
    ViewCompanyUsername = 8,

    /// <summary>
    /// 查看企业开通记录
    /// </summary>
    ViewCompanyOpenHistory = 16,

    /// <summary>
    /// 查看注册不完整企业
    /// </summary>
    ViewUncompletedCompany = 32,

    /// <summary>
    /// 查看推荐企业列表
    /// </summary>
    ViewCommendCompany = 64,

    /// <summary>
    /// 查看收费企业列表
    /// </summary>
    ViewChargedCompany = 128,

    /// <summary>
    /// 企业信息查看所有权限
    /// </summary>
    All = 255
}


/// <summary>
/// 企业相关管理
/// </summary>
[Flags]
public enum CompanyEnum
{
    /// <summary>
    /// 审核/取消审核企业
    /// </summary>
    CheckCompany = 1,

    /// <summary>
    /// 开通企业会员
    /// </summary>
    UpgradeCompany = 2,

    /// <summary>
    /// 显示/隐藏开通记录
    /// </summary>
    UpdateOpenHistory = 4,

    /// <summary>
    /// 修改企业名称
    /// </summary>
    UpdateCompanyName = 8,

    /// <summary>
    /// 禁用企业
    /// </summary>
    DisableCompany = 16,

    /// <summary>
    /// 推荐/取消推荐企业
    /// </summary>
    CommendCompany = 32,

    /// <summary>
    /// 登录企业招聘系统
    /// </summary>
    LoginCompanySystem = 64,

    /// <summary>
    /// 添加合作伙伴结算企业
    /// </summary>
    AddPartnerPayCompany = 128,

    /// <summary>
    /// 企业相关管理所有权限
    /// </summary>
    All = 255
}


/// <summary>
/// 服务申请订单处理
/// </summary>
[Flags]
public enum ServiceOrderEnum
{
    /// <summary>
    /// 查看/搜索
    /// </summary>
    View = 1,

    /// <summary>
    /// 处理标记
    /// </summary>
    DoFlag = 2,

    /// <summary>
    /// 服务申请订单处理所有权限
    /// </summary>
    All = 3
}


/// <summary>
/// 网上支付订单处理
/// </summary>
[Flags]
public enum PaymentOrderEnum
{
    /// <summary>
    /// 查看/搜索
    /// </summary>
    View = 1,

    /// <summary>
    /// 处理标记
    /// </summary>
    DoFlag = 2,

    /// <summary>
    /// 网上支付订单处理所有权限
    /// </summary>
    All = 3
}


/// <summary>
/// 在线广告管理
/// </summary>
[Flags]
public enum ADEnum
{
    /// <summary>
    /// 查看/搜索
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 生成广告标签
    /// </summary>
    Build = 16,

    /// <summary>
    /// 在线广告管理所有权限
    /// </summary>
    All = 31
}


/// <summary>
/// 广告类别管理
/// </summary>
[Flags]
public enum ADTypeEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 广告类别管理所有权限
    /// </summary>
    All = 15
}


/// <summary>
/// 企业专页广告管理
/// </summary>
[Flags]
public enum ADPageEnum
{
    /// <summary>
    /// 查看/搜索
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 企业专页广告管理所有权限
    /// </summary>
    All = 15
}


/// <summary>
/// 地区站点管理
/// </summary>
[Flags]
public enum AreaSiteEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 禁用/启用
    /// </summary>
    Disabled = 2,

    /// <summary>
    /// 地区站点管理所有权限
    /// </summary>
    All = 3
}


/// <summary>
/// 推广站点管理
/// </summary>
[Flags]
public enum DevelopSiteEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 站点内容编辑
    /// </summary>
    EditContent = 16,

    /// <summary>
    /// 推广站点管理所有权限
    /// </summary>
    All = 31
}


/// <summary>
/// 合作伙伴站点管理
/// </summary>
[Flags]
public enum PartnerSiteEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 站点内容编辑
    /// </summary>
    EditContent = 16,

    /// <summary>
    /// 合作伙伴站点管理所有权限
    /// </summary>
    All = 31
}


/// <summary>
/// 合作伙伴结算管理
/// </summary>
[Flags]
public enum PartnerPayEnum
{
    /// <summary>
    /// 查看/搜索
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加结算企业
    /// </summary>
    Create = 2,

    /// <summary>
    /// 结算
    /// </summary>
    Payed = 4,

    /// <summary>
    /// 合作伙伴结算管理所有权限
    /// </summary>
    All = 7
}


/// <summary>
/// 合作伙伴用户管理
/// </summary>
[Flags]
public enum PartnerUserEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 合作伙伴用户管理所有权限
    /// </summary>
    All = 15
}


/// <summary>
/// 用户问题处理
/// </summary>
[Flags]
public enum FAQEnum
{
    /// <summary>
    /// 查看/搜索
    /// </summary>
    View = 1,

    /// <summary>
    /// 处理/回复/解决问题
    /// </summary>
    Deal = 2,

    /// <summary>
    /// 问题统计
    /// </summary>
    Stat = 4,

    /// <summary>
    /// 用户问题处理所有权限
    /// </summary>
    All = 7
}



/// <summary>
/// 资讯类别管理
/// </summary>
[Flags]
public enum NewsTypeEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 合并类别
    /// </summary>
    Move = 16,

    /// <summary>
    /// 资讯类别管理所有权限
    /// </summary>
    All = 31
}


/// <summary>
/// 文章图片管理
/// </summary>
[Flags]
public enum NewsEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 生成资讯文章/类别
    /// </summary>
    Build = 16,

    /// <summary>
    /// 文章图片管理所有权限
    /// </summary>
    All = 31
}


/// <summary>
/// 模板管理
/// </summary>
[Flags]
public enum TemplateEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加/复制
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 模板管理所有权限
    /// </summary>
    All = 15
}


/// <summary>
/// 标签管理
/// </summary>
[Flags]
public enum LabelEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 添加/复制
    /// </summary>
    Create = 2,

    /// <summary>
    /// 修改
    /// </summary>
    Update = 4,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 8,

    /// <summary>
    /// 标签管理所有权限
    /// </summary>
    All = 15
}


/// <summary>
/// 静态页面生成
/// </summary>
[Flags]
public enum HtmlPageEnum
{
    /// <summary>
    /// 查看
    /// </summary>
    View = 1,

    /// <summary>
    /// 生成
    /// </summary>
    Build = 2,

    /// <summary>
    /// 静态页面生成所有权限
    /// </summary>
    All = 3
}


/// <summary>
/// 数据统计查看
/// </summary>
[Flags]
public enum StatEnum
{
    /// <summary>
    /// 人才统计
    /// </summary>
    PersonStat = 1,

    /// <summary>
    /// 简历统计
    /// </summary>
    ResumeStat = 2,

    /// <summary>
    /// 企业统计
    /// </summary>
    CompanyStat = 4,

    /// <summary>
    /// 职位统计
    /// </summary>
    PositionStat = 8,

    /// <summary>
    /// 互动统计
    /// </summary>
    ActiveStat = 16,

    /// <summary>
    /// 数据统计查看所有权限
    /// </summary>
    All = 31
}