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
-- Table structure for table `CourtTournament`
--

DROP TABLE IF EXISTS `CourtTournament`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CourtTournament` (
  `CourtsId` int NOT NULL,
  `TournamentsId` int NOT NULL,
  PRIMARY KEY (`CourtsId`,`TournamentsId`),
  KEY `IX_CourtTournament_TournamentsId` (`TournamentsId`),
  CONSTRAINT `FK_CourtTournament_Courts_CourtsId` FOREIGN KEY (`CourtsId`) REFERENCES `Courts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CourtTournament_Tournaments_TournamentsId` FOREIGN KEY (`TournamentsId`) REFERENCES `Tournaments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CourtTournament`
--

LOCK TABLES `CourtTournament` WRITE;
/*!40000 ALTER TABLE `CourtTournament` DISABLE KEYS */;
INSERT INTO `CourtTournament` VALUES (1,3),(2,3),(1,13),(2,13),(3,13),(1,15),(2,15),(1,21),(2,21),(3,21),(1,30),(2,30),(3,30),(1,31),(2,31),(3,31),(1,32),(2,32),(3,32),(1,33),(1,34),(2,34),(3,34),(1,35),(2,35),(2,37),(3,37),(2,39),(3,39),(1,40),(2,40),(1,43),(2,43),(3,43),(1,45),(2,45),(1,47),(2,47),(3,47),(1,48),(2,48),(2,49),(1,51),(1,52),(1,54),(1,55),(1,56);
/*!40000 ALTER TABLE `CourtTournament` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-10-18 17:34:38
