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