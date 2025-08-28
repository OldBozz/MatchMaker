-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: mysql57.unoeuro.com    Database: matchmakerpro_dk_db
-- ------------------------------------------------------
-- Server version	8.0.34-26

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
-- Table structure for table `Players`
--

DROP TABLE IF EXISTS `Players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Players` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Displayname` longtext NOT NULL,
  `Dob` datetime(6) DEFAULT NULL,
  `Rank` int NOT NULL,
  `Identity` longtext,
  `Name` longtext NOT NULL,
  `State` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Players`
--

LOCK TABLES `Players` WRITE;
/*!40000 ALTER TABLE `Players` DISABLE KEYS */;
INSERT INTO `Players` VALUES (1,'Bosse','2023-10-18 17:05:57.000000',14,'','',0),(2,'Flemming','2023-10-18 17:05:57.000000',0,'','',0),(3,'Nana','2023-10-18 17:05:57.000000',66,'','',0),(4,'Mads','2023-10-18 17:05:57.000000',0,'','',0),(5,'Kyller','2023-10-18 17:05:57.000000',0,'','',0),(6,'Peter','2023-10-18 17:05:57.000000',0,'','',0),(7,'Henrik','2023-10-18 17:05:57.000000',0,'','',0),(8,'Tim','2023-10-18 17:05:57.000000',0,'','',0),(9,'Kim','2023-10-18 17:05:57.000000',0,'','',0),(10,'John','2023-10-18 17:05:57.000000',26,'','',0),(11,'Curtis','2023-10-18 17:05:57.000000',60,'','',0),(12,'Stefano','2023-10-18 17:05:57.000000',0,'','',0),(13,'Bruno','2023-10-18 17:05:57.000000',0,'','',0),(14,'Niels','2023-10-18 17:05:57.000000',76,'','',0),(15,'Caroline','2023-10-18 17:05:57.000000',0,'','',0),(16,'Antonio','2023-10-18 17:05:57.000000',0,'','',0),(17,'Asser','2023-10-18 17:05:57.000000',0,'','',0),(18,'Jakob','2023-10-18 17:05:57.000000',52,'','',0),(20,'Rødsgård','2023-10-18 17:05:57.000000',81,'','Henrik Rødsgård',0);
/*!40000 ALTER TABLE `Players` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-10-18 17:34:34
