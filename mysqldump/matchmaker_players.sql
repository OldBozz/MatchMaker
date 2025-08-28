-- MySQL dump 10.13  Distrib 8.0.17, for Win64 (x86_64)
--
-- Host: mysql-auga-northeurope.mysql.database.azure.com    Database: matchmaker
-- ------------------------------------------------------
-- Server version	8.0.32

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
-- Table structure for table `players`
--

DROP TABLE IF EXISTS `players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `players` (
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
-- Dumping data for table `players`
--

LOCK TABLES `players` WRITE;
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` VALUES (1,'Bosse',NULL,20,NULL,'',0),(2,'Flemming',NULL,25,NULL,'',0),(3,'Nana',NULL,85,NULL,'',0),(4,'Mads',NULL,75,NULL,'',0),(5,'Kyller',NULL,55,NULL,'',0),(6,'Peter',NULL,30,NULL,'',0),(7,'Henrik',NULL,70,NULL,'',0),(8,'Tim',NULL,45,NULL,'',0),(9,'Kim',NULL,35,NULL,'',0),(10,'John',NULL,30,NULL,'',0),(11,'Curtis',NULL,55,NULL,'',0),(12,'Stefano',NULL,55,NULL,'',0),(13,'Bruno',NULL,35,NULL,'',0),(14,'Niels',NULL,55,NULL,'',0),(15,'Caroline',NULL,50,NULL,'',0),(16,'Antonio',NULL,75,NULL,'',0),(17,'Asser',NULL,35,NULL,'',0),(18,'Jakob',NULL,55,NULL,'',0),(19,'Simon',NULL,71,NULL,'Simon',0),(20,'Rødsgård',NULL,55,NULL,'Henrik Rødsgård',0);
/*!40000 ALTER TABLE `players` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-07-27 14:15:09
