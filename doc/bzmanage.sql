/*
SQLyog Ultimate v8.32 
MySQL - 5.7.13-log : Database - bzmanage
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`bzmanage` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `bzmanage`;

/*Table structure for table `bz_managers` */

DROP TABLE IF EXISTS `bz_managers`;

CREATE TABLE `bz_managers` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ShopId` bigint(20) NOT NULL,
  `RoleId` bigint(20) NOT NULL COMMENT '角色ID',
  `GroupId` bigint(20) NOT NULL COMMENT '分组ID',
  `ParentId` bigint(20) NOT NULL COMMENT '父ID',
  `ParentName` varchar(100) DEFAULT NULL COMMENT '父名称',
  `UserName` varchar(100) NOT NULL COMMENT '用户名称',
  `Password` varchar(100) NOT NULL COMMENT '密码',
  `PasswordSalt` varchar(100) NOT NULL COMMENT '密码加盐',
  `CreateDate` datetime NOT NULL COMMENT '创建日期',
  `Remark` varchar(1000) DEFAULT NULL,
  `RealName` varchar(1000) DEFAULT NULL COMMENT '真实名称',
  `IsLogin` int(4) DEFAULT '0' COMMENT '是否登陆(0:未登陆1:已登录)',
  `Address` varchar(100) DEFAULT NULL COMMENT '地址',
  `Phone` varchar(100) DEFAULT NULL COMMENT '电话',
  `BankAccountName` varchar(100) DEFAULT NULL COMMENT '银行开户行',
  `BankAccountNumber` varchar(100) DEFAULT NULL COMMENT '个人银行账号',
  `RegionId` int(20) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2252 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

insert  into `bz_managers`(`Id`,`ShopId`,`RoleId`,`GroupId`,`ParentId`,`ParentName`,`UserName`,`Password`,`PasswordSalt`,`CreateDate`,`Remark`,`RealName`,`IsLogin`,`Address`,`Phone`,`BankAccountName`,`BankAccountNumber`,`RegionId`) values (309,0,0,0,0,'','admin','e35eb25004f0807b0c56399ec0d6eafe','0039faa2-ff8e-40c1-b1e3-a21373f9d89a','2016-12-01 11:09:55',NULL,NULL,1,NULL,NULL,NULL,NULL,0);


/*Table structure for table `bz_roles` */

DROP TABLE IF EXISTS `bz_roles`;

CREATE TABLE `bz_roles` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ShopId` bigint(20) NOT NULL COMMENT '店铺ID',
  `RoleName` varchar(100) NOT NULL COMMENT '角色名称',
  `Description` varchar(1000) NOT NULL COMMENT '说明',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

/*Data for the table `bz_roles` */

insert  into `bz_roles`(`Id`,`ShopId`,`RoleName`,`Description`) values (9,0,'总管理员','总管理员'),(23,0,'财务','财务'),(28,0,'公司员工','公司员工');

/*Table structure for table `bz_roleprivileges` */

DROP TABLE IF EXISTS `bz_roleprivileges`;

CREATE TABLE `bz_roleprivileges` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Privilege` int(11) NOT NULL COMMENT '权限ID',
  `RoleId` bigint(20) NOT NULL COMMENT '角色ID',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1761 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

/*Data for the table `bz_roleprivileges` */


/*Table structure for table `bz_members` */

DROP TABLE IF EXISTS `bz_members`;

CREATE TABLE `bz_members` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(100) NOT NULL COMMENT '会员卡号',
  `UserLevel` int(11) NOT NULL COMMENT  '会员级别',
  `UserName` varchar(100) NOT NULL COMMENT '名称',
  `Sex` int(11) DEFAULT NULL COMMENT '性别',
  `CardId` varchar(100) NOT NULL COMMENT '身份证号',
  `BornDate` datetime NOT NULL COMMENT '出生日期',
  `ImageUrl` varchar(300) DEFAULT NULL COMMENT'照片路径',
  `PassportId` varchar(100) NOT NULL COMMENT '护照号',
  `PassportDate` datetime NOT NULL COMMENT '护照有效期',
  `PassportAddr` varchar(100) NOT NULL COMMENT '护照签发地',
  `CellPhone` varchar(100) NOT NULL COMMENT '手机',
  `TelePhone` varchar(100) NOT NULL COMMENT '固定电话',
  `RecommendUserId` varchar(100) NOT NULL COMMENT '推荐人卡号',
  `RecommendUserName` varchar(100) NOT NULL COMMENT '推荐人姓名',
  `TopRegionId` int(11) NOT NULL COMMENT '省份ID',
  `RegionId` int(11) NOT NULL COMMENT '省市区ID',
  `Address` varchar(100) DEFAULT NULL COMMENT '街道地址',
  `RegisterName` varchar(100) NOT NULL COMMENT '登记人姓名',
  `Points` int(11) NOT NULL COMMENT '积分',
  `Health` varchar(100) DEFAULT NULL COMMENT '身体情况',
  `Interest` varchar(100) DEFAULT NULL COMMENT '爱好',
  `Profession` varchar(100) DEFAULT NULL COMMENT '职业',
  `InterestLine` varchar(100) DEFAULT NULL COMMENT '兴趣',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20170001 DEFAULT CHARSET=utf8;

/*Data for the table `bz_members` */


/*Table structure for table `bz_levelrule` */

DROP TABLE IF EXISTS `bz_levelrule`;

CREATE TABLE `bz_levelrule` (
  `Id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `UserLevel` INT(11) NOT NULL COMMENT  '会员级别',
  `BeginPoint` INT(11) NOT NULL COMMENT '开始积分',
  `EndPoint` INT(11) NOT NULL COMMENT '结束积分',
  PRIMARY KEY (`Id`)
) ENGINE=INNODB AUTO_INCREMENT=1761 DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

/*Data for the table `bz_levelrule` */

/*Table structure for table `bz_supplier` */

DROP TABLE IF EXISTS `bz_supplier`;

CREATE TABLE `bz_supplier` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `SupplierId` varchar(100) NOT NULL COMMENT '供应商Id',
  `SupplierName` varchar(100) NOT NULL COMMENT '供应商名称',
  `TravelName` varchar(100) NOT NULL COMMENT '旅行社名称',
  `LeaderName` varchar(100) NOT NULL COMMENT '负责人',
  `LeaderPhone` varchar(100) NOT NULL COMMENT '负责人电话',
  `SellerName` varchar(100) NOT NULL COMMENT '销售联系人',
  `ContactWay` int(11) NOT NULL COMMENT '销售联系方式：0-电话，1-微信，2-qq',
  `SellerContact` varchar(100) NOT NULL COMMENT '销售联系方式',
  `Remark` varchar(300) DEFAULT NULL COMMENT'备注',
  `TravelAddress` varchar(300) DEFAULT NULL COMMENT '旅行社地址',
  `LineId` varchar(100) NOT NULL COMMENT '线路Id',
  `LineName` varchar(100) NOT NULL COMMENT '线路名称',
  `RegionId` varchar(100) NOT NULL COMMENT '区域Id',
  `RegionName` varchar(100) NOT NULL COMMENT '区域名称',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20170001 DEFAULT CHARSET=utf8;

/*Data for the table `bz_supplier` */