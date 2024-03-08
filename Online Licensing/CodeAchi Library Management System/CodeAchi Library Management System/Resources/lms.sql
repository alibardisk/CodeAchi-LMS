-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: lms
-- ------------------------------------------------------
-- Server version	5.7.31-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `accn_setting`
--

DROP TABLE IF EXISTS `accn_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accn_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `isAutoGenerate` varchar(5) DEFAULT NULL,
  `isManualPrefix` varchar(5) DEFAULT NULL,
  `prefixText` varchar(45) DEFAULT NULL,
  `joiningChar` varchar(45) DEFAULT NULL,
  `lastNumber` varchar(45) DEFAULT NULL,
  `noPrefix` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accn_setting`
--

LOCK TABLES `accn_setting` WRITE;
/*!40000 ALTER TABLE `accn_setting` DISABLE KEYS */;
/*!40000 ALTER TABLE `accn_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `borrower_details`
--

DROP TABLE IF EXISTS `borrower_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `borrower_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `brrId` varchar(45) DEFAULT NULL,
  `brrName` varchar(100) DEFAULT NULL,
  `brrCategory` varchar(100) DEFAULT NULL,
  `brrAddress` varchar(255) DEFAULT NULL,
  `brrGender` varchar(10) DEFAULT NULL,
  `brrMailId` varchar(200) DEFAULT NULL,
  `brrContact` varchar(45) DEFAULT NULL,
  `mbershipDuration` varchar(45) DEFAULT NULL,
  `addInfo1` varchar(150) DEFAULT NULL,
  `addInfo2` varchar(150) DEFAULT NULL,
  `addInfo3` varchar(150) DEFAULT NULL,
  `addInfo4` varchar(150) DEFAULT NULL,
  `addInfo5` varchar(150) DEFAULT NULL,
  `entryDate` varchar(45) DEFAULT NULL,
  `brrImage` longtext,
  `opkPermission` varchar(5) DEFAULT NULL,
  `renewDate` varchar(45) DEFAULT NULL,
  `brrPass` varchar(45) DEFAULT NULL,
  `memPlan` varchar(45) DEFAULT NULL,
  `memFreq` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `borrower_details`
--

LOCK TABLES `borrower_details` WRITE;
/*!40000 ALTER TABLE `borrower_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `borrower_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `borrower_settings`
--

DROP TABLE IF EXISTS `borrower_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `borrower_settings` (
  `categoryId` int(11) NOT NULL AUTO_INCREMENT,
  `catName` varchar(150) DEFAULT NULL,
  `catDesc` varchar(250) DEFAULT NULL,
  `catlogAccess` longtext,
  `capInfo` varchar(500) DEFAULT NULL,
  `maxCheckin` varchar(45) DEFAULT NULL,
  `maxDue` varchar(45) DEFAULT NULL,
  `membershipFees` varchar(45) DEFAULT NULL,
  `avoidFine` varchar(5) DEFAULT NULL,
  `defaultPass` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`categoryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `borrower_settings`
--

LOCK TABLES `borrower_settings` WRITE;
/*!40000 ALTER TABLE `borrower_settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `borrower_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `brrid_setting`
--

DROP TABLE IF EXISTS `brrid_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `brrid_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `isAutoGenerate` varchar(5) DEFAULT NULL,
  `isManualPrefix` varchar(5) DEFAULT NULL,
  `prefixText` varchar(45) DEFAULT NULL,
  `joiningChar` varchar(45) DEFAULT NULL,
  `lastNumber` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `brrid_setting`
--

LOCK TABLES `brrid_setting` WRITE;
/*!40000 ALTER TABLE `brrid_setting` DISABLE KEYS */;
/*!40000 ALTER TABLE `brrid_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `country_details`
--

DROP TABLE IF EXISTS `country_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `country_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `countryName` varchar(45) DEFAULT NULL,
  `currencyName` varchar(45) DEFAULT NULL,
  `cuurShort` varchar(45) DEFAULT NULL,
  `currSymbol` varchar(45) DEFAULT NULL,
  `dialCode` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `country_details`
--

LOCK TABLES `country_details` WRITE;
/*!40000 ALTER TABLE `country_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `country_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `general_settings`
--

DROP TABLE IF EXISTS `general_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `general_settings` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `instName` varchar(200) DEFAULT NULL,
  `instShortName` varchar(10) DEFAULT NULL,
  `instAddress` varchar(255) DEFAULT NULL,
  `cityCode` varchar(45) DEFAULT NULL,
  `instLogo` longtext,
  `instContact` varchar(45) DEFAULT NULL,
  `instWebsite` varchar(100) DEFAULT NULL,
  `instMail` varchar(100) DEFAULT NULL,
  `libraryName` varchar(45) DEFAULT NULL,
  `countryName` varchar(45) DEFAULT NULL,
  `dialCode` varchar(45) DEFAULT NULL,
  `currencyName` varchar(45) DEFAULT NULL,
  `currShortName` varchar(45) DEFAULT NULL,
  `currSymbol` varchar(45) DEFAULT NULL,
  `databasePath` varchar(255) DEFAULT NULL,
  `backupPath` varchar(255) DEFAULT NULL,
  `backupHour` varchar(45) DEFAULT NULL,
  `printerName` varchar(100) DEFAULT NULL,
  `productExpired` varchar(5) DEFAULT NULL,
  `licenseType` varchar(45) DEFAULT NULL,
  `licenseKey` varchar(100) DEFAULT NULL,
  `stepComplete` varchar(45) DEFAULT NULL,
  `productBlocked` varchar(5) DEFAULT NULL,
  `reserveSystemLimit` varchar(45) DEFAULT NULL,
  `opacAvailable` varchar(5) DEFAULT NULL,
  `machineId` varchar(200) DEFAULT NULL,
  `notificationData` longtext,
  `settingsData` longtext,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `general_settings`
--

LOCK TABLES `general_settings` WRITE;
/*!40000 ALTER TABLE `general_settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `general_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `issued_item`
--

DROP TABLE IF EXISTS `issued_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `issued_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `brrId` varchar(45) DEFAULT NULL,
  `itemAccession` varchar(45) DEFAULT NULL,
  `issueDate` varchar(45) DEFAULT NULL,
  `reissuedDate` varchar(45) DEFAULT NULL,
  `expectedReturnDate` varchar(45) DEFAULT NULL,
  `returnDate` varchar(45) DEFAULT NULL,
  `itemReturned` varchar(5) DEFAULT NULL,
  `issuedBy` varchar(45) DEFAULT NULL,
  `issueComment` varchar(200) DEFAULT NULL,
  `reissuedBy` varchar(45) DEFAULT NULL,
  `reissuedComment` varchar(200) DEFAULT NULL,
  `returnedBy` varchar(45) DEFAULT NULL,
  `returnedComment` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `issued_item`
--

LOCK TABLES `issued_item` WRITE;
/*!40000 ALTER TABLE `issued_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `issued_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_details`
--

DROP TABLE IF EXISTS `item_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `itemTitle` varchar(255) DEFAULT NULL,
  `itemIsbn` varchar(45) DEFAULT NULL,
  `itemAccession` varchar(45) DEFAULT NULL,
  `itemCat` varchar(100) DEFAULT NULL,
  `itemSubCat` varchar(100) DEFAULT NULL,
  `itemAuthor` varchar(255) DEFAULT NULL,
  `itemClassification` varchar(100) DEFAULT NULL,
  `itemSubject` varchar(255) DEFAULT NULL,
  `rackNo` varchar(45) DEFAULT NULL,
  `totalPages` varchar(45) DEFAULT NULL,
  `itemPrice` varchar(45) DEFAULT NULL,
  `addInfo1` varchar(150) DEFAULT NULL,
  `addInfo2` varchar(150) DEFAULT NULL,
  `addInfo3` varchar(150) DEFAULT NULL,
  `addInfo4` varchar(150) DEFAULT NULL,
  `addInfo5` varchar(150) DEFAULT NULL,
  `addInfo6` varchar(150) DEFAULT NULL,
  `addInfo7` varchar(150) DEFAULT NULL,
  `addInfo8` varchar(150) DEFAULT NULL,
  `entryDate` varchar(45) DEFAULT NULL,
  `itemAvailable` varchar(5) DEFAULT NULL,
  `isLost` varchar(5) DEFAULT NULL,
  `isDamage` varchar(5) DEFAULT NULL,
  `itemImage` longtext,
  `digitalReference` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item_details`
--

LOCK TABLES `item_details` WRITE;
/*!40000 ALTER TABLE `item_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `item_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_settings`
--

DROP TABLE IF EXISTS `item_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item_settings` (
  `catId` int(11) NOT NULL AUTO_INCREMENT,
  `catName` varchar(100) DEFAULT NULL,
  `catDesc` varchar(200) DEFAULT NULL,
  `capInfo` varchar(450) DEFAULT NULL,
  `isConsume` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`catId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item_settings`
--

LOCK TABLES `item_settings` WRITE;
/*!40000 ALTER TABLE `item_settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `item_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_subcategory`
--

DROP TABLE IF EXISTS `item_subcategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item_subcategory` (
  `subCatId` int(11) NOT NULL AUTO_INCREMENT,
  `catName` varchar(100) DEFAULT NULL,
  `subCatName` varchar(100) DEFAULT NULL,
  `shortName` varchar(45) DEFAULT NULL,
  `subCatDesc` varchar(150) DEFAULT NULL,
  `notReference` varchar(5) DEFAULT NULL,
  `issueDays` varchar(45) DEFAULT NULL,
  `subCatFine` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`subCatId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item_subcategory`
--

LOCK TABLES `item_subcategory` WRITE;
/*!40000 ALTER TABLE `item_subcategory` DISABLE KEYS */;
/*!40000 ALTER TABLE `item_subcategory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lost_damage`
--

DROP TABLE IF EXISTS `lost_damage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lost_damage` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `itemAccn` varchar(45) DEFAULT NULL,
  `itemStatus` varchar(45) DEFAULT NULL,
  `statusComment` varchar(200) DEFAULT NULL,
  `brrId` varchar(45) DEFAULT NULL,
  `entryDate` varchar(45) DEFAULT NULL,
  `entryBy` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lost_damage`
--

LOCK TABLES `lost_damage` WRITE;
/*!40000 ALTER TABLE `lost_damage` DISABLE KEYS */;
/*!40000 ALTER TABLE `lost_damage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbership_setting`
--

DROP TABLE IF EXISTS `mbership_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbership_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `membrshpName` varchar(45) DEFAULT NULL,
  `planFreqncy` varchar(45) DEFAULT NULL,
  `membrDurtn` varchar(45) DEFAULT NULL,
  `membrFees` varchar(45) DEFAULT NULL,
  `issueLimit` varchar(45) DEFAULT NULL,
  `planDesc` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbership_setting`
--

LOCK TABLES `mbership_setting` WRITE;
/*!40000 ALTER TABLE `mbership_setting` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbership_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notice_template`
--

DROP TABLE IF EXISTS `notice_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notice_template` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tempName` varchar(100) DEFAULT NULL,
  `senderId` varchar(100) DEFAULT NULL,
  `htmlBody` longtext,
  `bodyText` longtext,
  `noticeType` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notice_template`
--

LOCK TABLES `notice_template` WRITE;
/*!40000 ALTER TABLE `notice_template` DISABLE KEYS */;
/*!40000 ALTER TABLE `notice_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payment_details`
--

DROP TABLE IF EXISTS `payment_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payment_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `feesDate` varchar(45) DEFAULT NULL,
  `memberId` varchar(45) DEFAULT NULL,
  `invId` varchar(45) DEFAULT NULL,
  `itemAccn` varchar(45) DEFAULT NULL,
  `feesDesc` varchar(250) DEFAULT NULL,
  `dueAmount` varchar(45) DEFAULT NULL,
  `isPaid` varchar(5) DEFAULT NULL,
  `isRemited` varchar(5) DEFAULT NULL,
  `discountAmnt` varchar(45) DEFAULT NULL,
  `payDate` varchar(45) DEFAULT NULL,
  `collctedBy` varchar(45) DEFAULT NULL,
  `invidCount` varchar(45) DEFAULT NULL,
  `paymentMode` varchar(45) DEFAULT NULL,
  `paymentRef` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_details`
--

LOCK TABLES `payment_details` WRITE;
/*!40000 ALTER TABLE `payment_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `payment_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reservation_list`
--

DROP TABLE IF EXISTS `reservation_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reservation_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `brrId` varchar(45) DEFAULT NULL,
  `itemAccn` varchar(45) DEFAULT NULL,
  `itemTitle` varchar(255) DEFAULT NULL,
  `itemAuthor` varchar(255) DEFAULT NULL,
  `reserveLocation` varchar(45) DEFAULT NULL,
  `reserveDate` varchar(45) DEFAULT NULL,
  `availableDate` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservation_list`
--

LOCK TABLES `reservation_list` WRITE;
/*!40000 ALTER TABLE `reservation_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `reservation_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_activity`
--

DROP TABLE IF EXISTS `user_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_activity` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` varchar(45) DEFAULT NULL,
  `itemAccn` longtext,
  `brrId` longtext,
  `taskDesc` varchar(200) DEFAULT NULL,
  `dateTime` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_activity`
--

LOCK TABLES `user_activity` WRITE;
/*!40000 ALTER TABLE `user_activity` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_activity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_details`
--

DROP TABLE IF EXISTS `user_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(45) DEFAULT NULL,
  `userDesig` varchar(45) DEFAULT NULL,
  `userMail` varchar(45) DEFAULT NULL,
  `userContact` varchar(45) DEFAULT NULL,
  `userPassword` varchar(45) DEFAULT NULL,
  `userAddress` varchar(200) DEFAULT NULL,
  `userPriviledge` varchar(500) DEFAULT NULL,
  `isActive` varchar(5) DEFAULT NULL,
  `isAdmin` varchar(5) DEFAULT NULL,
  `userImage` longtext,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_details`
--

LOCK TABLES `user_details` WRITE;
/*!40000 ALTER TABLE `user_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `wish_list`
--

DROP TABLE IF EXISTS `wish_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `wish_list` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `itemType` varchar(45) DEFAULT NULL,
  `itemTitle` varchar(255) DEFAULT NULL,
  `itemAuthor` varchar(255) DEFAULT NULL,
  `itemPublication` varchar(255) DEFAULT NULL,
  `queryDate` varchar(45) DEFAULT NULL,
  `queryCount` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wish_list`
--

LOCK TABLES `wish_list` WRITE;
/*!40000 ALTER TABLE `wish_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `wish_list` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-04-16 15:17:38
