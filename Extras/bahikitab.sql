-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jun 26, 2024 at 05:42 AM
-- Server version: 8.0.30
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bahikitab`
--

-- --------------------------------------------------------

--
-- Table structure for table `category`
--

CREATE TABLE `category` (
  `id` int NOT NULL,
  `categ_name` varchar(100) DEFAULT NULL,
  `gst` double DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `category`
--

INSERT INTO `category` (`id`, `categ_name`, `gst`, `created_at`, `updated_at`) VALUES
(1, 'qwerty', 10, '2024-05-11 15:22:10', '2024-05-11 15:22:10'),
(2, 'dfgdfgdfgdfg', 6, '2024-05-11 16:17:41', '2024-05-11 16:17:41'),
(3, 'tretert', 12, '2024-05-11 16:18:31', '2024-05-11 16:18:31'),
(4, 'categ4', 18, '2024-05-11 19:43:24', '2024-05-11 19:43:24'),
(5, 'tertiary', 6, '2024-05-16 14:03:03', '2024-05-16 14:03:03'),
(6, 'qwsdfg', 12, '2024-05-16 14:03:57', '2024-05-16 14:03:57');

-- --------------------------------------------------------

--
-- Table structure for table `company_profile`
--

CREATE TABLE `company_profile` (
  `id` int NOT NULL,
  `comp_name` varchar(100) NOT NULL,
  `comp_email` varchar(100) NOT NULL,
  `comp_address` varchar(100) DEFAULT NULL,
  `comp_type` varchar(100) NOT NULL,
  `comp_contact` int NOT NULL,
  `comp_gst` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `customer_lead`
--

CREATE TABLE `customer_lead` (
  `id` int NOT NULL,
  `cust_fname` varchar(100) NOT NULL,
  `cust_lname` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `cust_email` varchar(100) NOT NULL,
  `cust_phone` text,
  `cust_address` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `cust_city` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `cust_district` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `cust_state` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `cust_pin` int DEFAULT NULL,
  `cust_country` varchar(100) DEFAULT NULL,
  `cust_doa` date DEFAULT NULL,
  `cust_dob` date DEFAULT NULL,
  `cust_label` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `cust_notes` text
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `customer_lead`
--

INSERT INTO `customer_lead` (`id`, `cust_fname`, `cust_lname`, `cust_email`, `cust_phone`, `cust_address`, `cust_city`, `cust_district`, `cust_state`, `cust_pin`, `cust_country`, `cust_doa`, `cust_dob`, `cust_label`, `cust_notes`) VALUES
(2, 'Puneet', 'Aggarwal', 'abc@gmail.com', '987654321', 'dfgdsgdsg\r\nfdgdsf\r\ngsdfg\r\nsdfgsdfgsdfg', 'Zirakpur', '', 'Punjab', 140604, 'India', '2015-10-25', '0001-01-01', NULL, 'fgdsfgsdg\r\nsdfgd\r\nfgdsfg'),
(3, 'Subodh', 'Moudgil', 'abc@gmail.com', '9876543210', 'fdsfdsdf\r\ndfdfs\r\ndfsdfs\r\ndfsdfs\r\nffdssdf', 'Baltana', 'Mohali', 'Punjab', 140604, 'India', '0001-01-01', '0001-01-01', NULL, 'fdzcvvxcv');

-- --------------------------------------------------------

--
-- Table structure for table `events`
--

CREATE TABLE `events` (
  `id` int NOT NULL,
  `event_name` varchar(100) DEFAULT NULL,
  `event_msg` varchar(1000) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `id` int NOT NULL,
  `order_id` varchar(100) NOT NULL,
  `payment_type` varchar(100) NOT NULL,
  `amount` double NOT NULL,
  `balance` double NOT NULL,
  `customer_id` int NOT NULL,
  `payment_status` varchar(50) NOT NULL,
  `order_status` varchar(50) NOT NULL,
  `takenby` varchar(100) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `nextfollowup` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `advanceamount` double DEFAULT NULL,
  `billdate` date DEFAULT NULL,
  `billnumber` varchar(100) DEFAULT NULL,
  `discount` double DEFAULT NULL,
  `amountafterdiscount` double DEFAULT NULL,
  `priority` tinyint(1) NOT NULL DEFAULT '0',
  `ordered_products` json DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `purchase`
--

CREATE TABLE `purchase` (
  `id` int NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Category` int NOT NULL,
  `Status` varchar(100) NOT NULL,
  `Packing` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BatchNo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Expiry` int DEFAULT NULL,
  `MfgDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Stock` int NOT NULL,
  `PurchasePrice` double DEFAULT NULL,
  `MRP` double DEFAULT NULL,
  `MaxSP` double DEFAULT NULL,
  `MinSP` double DEFAULT NULL,
  `MfrName` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Company` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `rate` double DEFAULT NULL,
  `data` json DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `purchase`
--

INSERT INTO `purchase` (`id`, `Name`, `Description`, `Category`, `Status`, `Packing`, `BatchNo`, `Expiry`, `MfgDate`, `Stock`, `PurchasePrice`, `MRP`, `MaxSP`, `MinSP`, `MfrName`, `Company`, `rate`, `data`) VALUES
(1, 'ghgdfhgf', 'gfhfghd', 2, 'Published', 'dfhfdh', 'dfhfgh', 45, '2024-05-07 00:00:00', 55, 4567, 54654, 435, 4567, 'gdfg', 'dfghfh', NULL, NULL),
(2, 'qwertyy', 'sfsdfsdfsdfsdfdsfsfsdf', 4, 'Published', 'fsdfsdfsdf', 'sdfdsfdsf', 24, '2024-05-16 00:00:00', 45, 1523, 2222, 2100, 1800, 'sdfsdfdsf', 'sdfsdfsd', 0, NULL),
(3, 'gdgfdfg', 'gfdgdfg', 2, 'Published', 'dgfdfg', 'gdfgdfgdfg', 45, '2024-06-05 00:00:00', 43, 5466556, 43434, 0, 0, 'gdfgfgfd', 'dgfdfgf', 0, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `sales`
--

CREATE TABLE `sales` (
  `id` int NOT NULL,
  `data` json DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `stages`
--

CREATE TABLE `stages` (
  `id` int NOT NULL,
  `od_stages` varchar(100) DEFAULT NULL,
  `od_color` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `sub_category`
--

CREATE TABLE `sub_category` (
  `id` int NOT NULL,
  `categ_name` varchar(100) DEFAULT NULL,
  `parent_categ_id` int NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `sub_category`
--

INSERT INTO `sub_category` (`id`, `categ_name`, `parent_categ_id`, `created_at`, `updated_at`) VALUES
(1, 'sub1', 2, '2024-05-11 19:18:59', '2024-05-11 19:18:59'),
(2, 'sub2', 1, '2024-05-11 19:41:51', '2024-05-11 19:41:51'),
(3, 'sub3', 4, '2024-05-11 19:43:35', '2024-05-11 19:43:35');

-- --------------------------------------------------------

--
-- Table structure for table `sub_sub_category`
--

CREATE TABLE `sub_sub_category` (
  `id` int NOT NULL,
  `categ_name` varchar(100) DEFAULT NULL,
  `parent_categ_id` int DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `sub_sub_category`
--

INSERT INTO `sub_sub_category` (`id`, `categ_name`, `parent_categ_id`, `created_at`, `updated_at`) VALUES
(1, 'ssub1', 1, '2024-05-11 19:42:04', '2024-05-11 19:42:04'),
(2, 'ssub2', 4, '2024-05-11 19:43:47', '2024-05-11 19:43:47');

-- --------------------------------------------------------

--
-- Table structure for table `transactions`
--

CREATE TABLE `transactions` (
  `id` int NOT NULL,
  `amount` double NOT NULL,
  `trns_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `trns_id` varchar(100) NOT NULL,
  `order_id` int NOT NULL,
  `message` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int NOT NULL,
  `username` varchar(1000) NOT NULL,
  `password` varchar(1000) NOT NULL,
  `email_id` varchar(100) NOT NULL,
  `role` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `company_profile`
--
ALTER TABLE `company_profile`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `customer_lead`
--
ALTER TABLE `customer_lead`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `events`
--
ALTER TABLE `events`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `purchase`
--
ALTER TABLE `purchase`
  ADD PRIMARY KEY (`id`),
  ADD KEY `CategoryDeleteProduct` (`Category`);

--
-- Indexes for table `sales`
--
ALTER TABLE `sales`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `stages`
--
ALTER TABLE `stages`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sub_category`
--
ALTER TABLE `sub_category`
  ADD PRIMARY KEY (`id`),
  ADD KEY `TreeDelete` (`parent_categ_id`);

--
-- Indexes for table `sub_sub_category`
--
ALTER TABLE `sub_sub_category`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `transactions`
--
ALTER TABLE `transactions`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `company_profile`
--
ALTER TABLE `company_profile`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `customer_lead`
--
ALTER TABLE `customer_lead`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `events`
--
ALTER TABLE `events`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `purchase`
--
ALTER TABLE `purchase`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `sales`
--
ALTER TABLE `sales`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `stages`
--
ALTER TABLE `stages`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sub_category`
--
ALTER TABLE `sub_category`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `sub_sub_category`
--
ALTER TABLE `sub_sub_category`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `transactions`
--
ALTER TABLE `transactions`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `purchase`
--
ALTER TABLE `purchase`
  ADD CONSTRAINT `CategoryDeleteProduct` FOREIGN KEY (`Category`) REFERENCES `category` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
