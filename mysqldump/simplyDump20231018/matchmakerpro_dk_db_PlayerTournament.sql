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
-- Table structure for table `PlayerTournament`
--

DROP TABLE IF EXISTS `PlayerTournament`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PlayerTournament` (
  `PlayersId` int NOT NULL,
  `TournamentsId` int NOT NULL,
  PRIMARY KEY (`PlayersId`,`TournamentsId`),
  KEY `IX_PlayerTournament_TournamentsId` (`TournamentsId`),
  CONSTRAINT `FK_PlayerTournament_Players_PlayersId` FOREIGN KEY (`PlayersId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PlayerTournament_Tournaments_TournamentsId` FOREIGN KEY (`TournamentsId`) REFERENCES `Tournaments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PlayerTournament`
--

LOCK TABLES `PlayerTournament` WRITE;
/*!40000 ALTER TABLE `PlayerTournament` DISABLE KEYS */;
INSERT INTO `PlayerTournament` VALUES (1,3),(3,3),(5,3),(7,3),(8,3),(9,3),(10,3),(13,3),(14,3),(15,3),(1,13),(2,13),(4,13),(6,13),(7,13),(8,13),(10,13),(12,13),(13,13),(16,13),(17,13),(1,15),(5,15),(6,15),(7,15),(8,15),(10,15),(11,15),(12,15),(13,15),(16,15),(17,15),(1,21),(2,21),(3,21),(4,21),(5,21),(7,21),(8,21),(9,21),(10,21),(12,21),(14,21),(15,21),(18,21),(1,30),(4,30),(5,30),(7,30),(8,30),(9,30),(10,30),(11,30),(13,30),(17,30),(18,30),(20,30),(2,31),(4,31),(5,31),(7,31),(8,31),(9,31),(10,31),(12,31),(13,31),(15,31),(17,31),(18,31),(4,32),(5,32),(7,32),(8,32),(9,32),(10,32),(11,32),(12,32),(13,32),(14,32),(15,32),(17,32),(18,32),(20,32),(1,33),(2,33),(3,33),(4,33),(5,33),(6,33),(7,33),(8,33),(9,33),(10,33),(11,33),(12,33),(13,33),(14,33),(15,33),(16,33),(17,33),(18,33),(20,33),(3,34),(5,34),(6,34),(7,34),(9,34),(10,34),(13,34),(14,34),(15,34),(17,34),(18,34),(20,34),(7,35),(9,35),(10,35),(11,35),(13,35),(14,35),(15,35),(18,35),(20,35),(1,37),(2,37),(5,37),(7,37),(9,37),(15,37),(18,37),(20,37),(1,39),(2,39),(4,39),(6,39),(7,39),(8,39),(9,39),(10,39),(11,39),(20,39),(1,40),(4,40),(5,40),(7,40),(9,40),(10,40),(15,40),(20,40),(1,43),(2,43),(3,43),(4,43),(5,43),(7,43),(8,43),(9,43),(10,43),(11,43),(12,43),(13,43),(20,43),(1,45),(5,45),(8,45),(9,45),(10,45),(17,45),(18,45),(20,45),(1,47),(2,47),(3,47),(4,47),(5,47),(8,47),(9,47),(10,47),(11,47),(12,47),(17,47),(18,47),(20,47),(1,48),(4,48),(5,48),(9,48),(10,48),(12,48),(13,48),(18,48),(4,49),(5,49),(9,49),(10,49),(18,49),(1,51),(3,51),(10,51),(14,51),(18,51),(1,52),(3,52),(10,52),(11,52),(14,52),(18,52),(20,52),(1,54),(10,54),(18,54),(20,54),(1,55),(10,55),(11,55),(14,55),(18,55),(20,55),(1,56),(3,56),(10,56),(14,56),(18,56),(20,56);
/*!40000 ALTER TABLE `PlayerTournament` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-10-18 17:34:40
