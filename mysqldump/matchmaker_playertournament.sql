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
-- Table structure for table `playertournament`
--

DROP TABLE IF EXISTS `playertournament`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playertournament` (
  `PlayersId` int NOT NULL,
  `TournamentsId` int NOT NULL,
  PRIMARY KEY (`PlayersId`,`TournamentsId`),
  KEY `IX_PlayerTournament_TournamentsId` (`TournamentsId`),
  CONSTRAINT `FK_PlayerTournament_Players_PlayersId` FOREIGN KEY (`PlayersId`) REFERENCES `players` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PlayerTournament_Tournaments_TournamentsId` FOREIGN KEY (`TournamentsId`) REFERENCES `tournaments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `playertournament`
--

LOCK TABLES `playertournament` WRITE;
/*!40000 ALTER TABLE `playertournament` DISABLE KEYS */;
INSERT INTO `playertournament` VALUES (1,3),(3,3),(5,3),(7,3),(8,3),(9,3),(10,3),(13,3),(14,3),(15,3),(1,13),(2,13),(4,13),(6,13),(7,13),(8,13),(10,13),(12,13),(13,13),(16,13),(17,13),(19,13),(1,15),(5,15),(6,15),(7,15),(8,15),(10,15),(11,15),(12,15),(13,15),(16,15),(17,15),(1,21),(2,21),(3,21),(4,21),(5,21),(7,21),(8,21),(9,21),(10,21),(12,21),(14,21),(15,21),(18,21),(1,30),(4,30),(5,30),(7,30),(8,30),(9,30),(10,30),(11,30),(13,30),(17,30),(18,30),(20,30),(2,31),(4,31),(5,31),(7,31),(8,31),(9,31),(10,31),(12,31),(13,31),(15,31),(17,31),(18,31),(4,32),(5,32),(7,32),(8,32),(9,32),(10,32),(11,32),(12,32),(13,32),(14,32),(15,32),(17,32),(18,32),(20,32),(1,33),(2,33),(3,33),(4,33),(5,33),(6,33),(7,33),(8,33),(9,33),(10,33),(11,33),(12,33),(13,33),(14,33),(15,33),(16,33),(17,33),(18,33),(19,33),(20,33),(3,34),(5,34),(6,34),(7,34),(9,34),(10,34),(13,34),(14,34),(15,34),(17,34),(18,34),(20,34),(7,35),(9,35),(10,35),(11,35),(13,35),(14,35),(15,35),(18,35),(20,35),(1,37),(2,37),(5,37),(7,37),(9,37),(15,37),(18,37),(20,37);
/*!40000 ALTER TABLE `playertournament` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-07-27 14:15:01
