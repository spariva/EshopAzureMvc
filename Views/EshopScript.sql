USE [master]
GO
/****** Object:  Database [ESHOP]    Script Date: 12/03/2025 11:43:44 ******/
CREATE DATABASE [ESHOP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ESHOP', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.DESARROLLO\MSSQL\DATA\ESHOP.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ESHOP_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.DESARROLLO\MSSQL\DATA\ESHOP_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ESHOP] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ESHOP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ESHOP] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ESHOP] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ESHOP] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ESHOP] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ESHOP] SET ARITHABORT OFF 
GO
ALTER DATABASE [ESHOP] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ESHOP] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ESHOP] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ESHOP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ESHOP] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ESHOP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ESHOP] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ESHOP] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ESHOP] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ESHOP] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ESHOP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ESHOP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ESHOP] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ESHOP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ESHOP] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ESHOP] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ESHOP] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ESHOP] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ESHOP] SET  MULTI_USER 
GO
ALTER DATABASE [ESHOP] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ESHOP] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ESHOP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ESHOP] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ESHOP] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ESHOP] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ESHOP] SET QUERY_STORE = ON
GO
ALTER DATABASE [ESHOP] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ESHOP]
GO
/****** Object:  Table [dbo].[PAYMENTS]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAYMENTS](
	[PAYMENT_ID] [int] NOT NULL,
	[PURCHASE_ID] [int] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[PAYMENT_METHOD] [nvarchar](50) NOT NULL,
	[TRANSACTION_ID] [nvarchar](50) NOT NULL,
	[AMOUNT] [decimal](10, 2) NOT NULL,
	[PAYMENT_DATE] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PAYMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PROD_CAT]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PROD_CAT](
	[PRODUCT_ID] [int] NOT NULL,
	[CATEGORY_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PRODUCT_ID] ASC,
	[CATEGORY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PRODUCTS]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRODUCTS](
	[PRODUCT_ID] [int] NOT NULL,
	[STORE_ID] [int] NOT NULL,
	[PRODUCT_NAME] [nvarchar](50) NOT NULL,
	[DESCRIPTION] [nvarchar](200) NULL,
	[IMAGE] [nvarchar](255) NULL,
	[PRICE] [decimal](10, 2) NOT NULL,
	[STOCK_QUANTITY] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PRODUCT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PRODUCTS_CATEGORIES]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRODUCTS_CATEGORIES](
	[CATEGORY_ID] [int] NOT NULL,
	[CATEGORY_NAME] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CATEGORY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PURCHASE_ITEMS]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PURCHASE_ITEMS](
	[PURCHASE_ITEM_ID] [int] NOT NULL,
	[PURCHASE_ID] [int] NOT NULL,
	[PRODUCT_ID] [int] NOT NULL,
	[QUANTITY] [int] NOT NULL,
	[PRICE_AT_PURCHASE] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PURCHASE_ITEM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PURCHASE_VENDOR_MAPPING]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PURCHASE_VENDOR_MAPPING](
	[PURCHASE_VENDOR_ID] [int] NOT NULL,
	[PURCHASE_ID] [int] NOT NULL,
	[VENDOR_ID] [int] NOT NULL,
	[VENDOR_AMOUNT] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PURCHASE_VENDOR_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PURCHASES]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PURCHASES](
	[PURCHASE_ID] [int] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[TOTAL_PRICE] [decimal](10, 2) NOT NULL,
	[PAYMENT_STATUS] [nvarchar](50) NOT NULL,
	[CREATED_AT] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PURCHASE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STORE_PAYOUTS]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORE_PAYOUTS](
	[PAYOUT_ID] [int] NOT NULL,
	[STORE_ID] [int] NOT NULL,
	[PURCHASE_ID] [int] NOT NULL,
	[PAYOUT_AMOUNT] [decimal](10, 2) NOT NULL,
	[PAYOUT_STATUS] [nvarchar](50) NOT NULL,
	[PAYOUT_DATE] [datetime] NULL,
	[PAYOUT_METHOD] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PAYOUT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STORES]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORES](
	[STORE_ID] [int] NOT NULL,
	[STORE_NAME] [nvarchar](50) NOT NULL,
	[EMAIL] [nvarchar](50) NOT NULL,
	[IMAGE] [nvarchar](255) NULL,
	[CATEGORY] [nvarchar](50) NULL,
	[USER_ID] [int] NOT NULL,
	[STRIPE_ID] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[STORE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[USER_ID] [int] NOT NULL,
	[NAME] [nvarchar](50) NOT NULL,
	[EMAIL] [nvarchar](50) NOT NULL,
	[PASSWORD_HASH] [varbinary](max) NOT NULL,
	[SALT] [nvarchar](50) NOT NULL,
	[TELEPHONE] [nvarchar](15) NULL,
	[ADDRESS] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[PAYMENTS] ([PAYMENT_ID], [PURCHASE_ID], [USER_ID], [PAYMENT_METHOD], [TRANSACTION_ID], [AMOUNT], [PAYMENT_DATE]) VALUES (1, 1, 1, N'Credit Card', N'TXN12345', CAST(60.00 AS Decimal(10, 2)), CAST(N'2025-03-05T09:23:05.560' AS DateTime))
INSERT [dbo].[PAYMENTS] ([PAYMENT_ID], [PURCHASE_ID], [USER_ID], [PAYMENT_METHOD], [TRANSACTION_ID], [AMOUNT], [PAYMENT_DATE]) VALUES (2, 2, 2, N'PayPal', N'TXN67890', CAST(22.00 AS Decimal(10, 2)), CAST(N'2025-03-05T09:23:05.560' AS DateTime))
GO
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (1, 1)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (2, 1)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (3, 2)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (4, 2)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (5, 3)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (6, 3)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (7, 4)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (8, 1)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (8, 2)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (8, 5)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (8, 6)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (9, 1)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (9, 7)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (10, 4)
INSERT [dbo].[PROD_CAT] ([PRODUCT_ID], [CATEGORY_ID]) VALUES (10, 8)
GO
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (1, 1, N'Silver Hoop Earrings', N'Handmade silver hoop earrings.', N'hoop_earrings.jpg', CAST(25.00 AS Decimal(10, 2)), 10)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (2, 1, N'Beaded Earrings', N'Colorful beaded earrings.', N'Captura de pantalla 2025-03-07 091748.png', CAST(18.00 AS Decimal(10, 2)), 20)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (3, 2, N'Wool Scarf', N'Warm wool scarf in neutral tones.', N'wool_scarf.jpg', CAST(35.00 AS Decimal(10, 2)), 15)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (4, 2, N'Silk Scarf', N'Elegant silk scarf with floral patterns.', N'silk_scarf.jpg', CAST(45.00 AS Decimal(10, 2)), 10)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (5, 3, N'Indie Art Zine', N'A collection of indie art and poetry.', N'art_zine.jpg', CAST(10.00 AS Decimal(10, 2)), 30)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (6, 3, N'Music Fanzine', N'A fanzine dedicated to underground music.', N'music_zine.jpg', CAST(12.00 AS Decimal(10, 2)), 25)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (7, 1, N'Prubita', N'prueb', N'dayan-tengyun-2x2-plus-m.jpg', CAST(12.00 AS Decimal(10, 2)), 3)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (8, 1, N'PrubitaS', N'prueba', N'cubone_on_minecraft_by_miccopicco_d4kabuh-pre.jpg', CAST(12.00 AS Decimal(10, 2)), 3)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (9, 1, N'PrubitaS', N'prueba', N'4545050103-0.jpg', CAST(12.00 AS Decimal(10, 2)), 3)
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [STORE_ID], [PRODUCT_NAME], [DESCRIPTION], [IMAGE], [PRICE], [STOCK_QUANTITY]) VALUES (10, 1, N'a', N'a', N'17c496ece5b2e99c316777d06ef23eb63b433efa_original.jpeg', CAST(1.00 AS Decimal(10, 2)), 1)
GO
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (1, N'Earrings')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (2, N'Winter Clothing')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (3, N'Zines')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (4, N'Cubos')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (5, N'Photos')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (6, N'rings')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (7, N'Electronics')
INSERT [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (8, N'VUE')
GO
INSERT [dbo].[PURCHASE_ITEMS] ([PURCHASE_ITEM_ID], [PURCHASE_ID], [PRODUCT_ID], [QUANTITY], [PRICE_AT_PURCHASE]) VALUES (1, 1, 1, 1, CAST(25.00 AS Decimal(10, 2)))
INSERT [dbo].[PURCHASE_ITEMS] ([PURCHASE_ITEM_ID], [PURCHASE_ID], [PRODUCT_ID], [QUANTITY], [PRICE_AT_PURCHASE]) VALUES (2, 1, 3, 1, CAST(35.00 AS Decimal(10, 2)))
INSERT [dbo].[PURCHASE_ITEMS] ([PURCHASE_ITEM_ID], [PURCHASE_ID], [PRODUCT_ID], [QUANTITY], [PRICE_AT_PURCHASE]) VALUES (3, 2, 5, 1, CAST(10.00 AS Decimal(10, 2)))
INSERT [dbo].[PURCHASE_ITEMS] ([PURCHASE_ITEM_ID], [PURCHASE_ID], [PRODUCT_ID], [QUANTITY], [PRICE_AT_PURCHASE]) VALUES (4, 2, 6, 1, CAST(12.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[PURCHASE_VENDOR_MAPPING] ([PURCHASE_VENDOR_ID], [PURCHASE_ID], [VENDOR_ID], [VENDOR_AMOUNT]) VALUES (1, 1, 1, CAST(25.00 AS Decimal(10, 2)))
INSERT [dbo].[PURCHASE_VENDOR_MAPPING] ([PURCHASE_VENDOR_ID], [PURCHASE_ID], [VENDOR_ID], [VENDOR_AMOUNT]) VALUES (2, 1, 2, CAST(35.00 AS Decimal(10, 2)))
INSERT [dbo].[PURCHASE_VENDOR_MAPPING] ([PURCHASE_VENDOR_ID], [PURCHASE_ID], [VENDOR_ID], [VENDOR_AMOUNT]) VALUES (3, 2, 3, CAST(22.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[PURCHASES] ([PURCHASE_ID], [USER_ID], [TOTAL_PRICE], [PAYMENT_STATUS], [CREATED_AT]) VALUES (1, 1, CAST(60.00 AS Decimal(10, 2)), N'Completed', CAST(N'2025-03-05T09:23:05.560' AS DateTime))
INSERT [dbo].[PURCHASES] ([PURCHASE_ID], [USER_ID], [TOTAL_PRICE], [PAYMENT_STATUS], [CREATED_AT]) VALUES (2, 2, CAST(22.00 AS Decimal(10, 2)), N'Completed', CAST(N'2025-03-05T09:23:05.560' AS DateTime))
GO
INSERT [dbo].[STORE_PAYOUTS] ([PAYOUT_ID], [STORE_ID], [PURCHASE_ID], [PAYOUT_AMOUNT], [PAYOUT_STATUS], [PAYOUT_DATE], [PAYOUT_METHOD]) VALUES (1, 1, 1, CAST(25.00 AS Decimal(10, 2)), N'Paid', CAST(N'2025-03-05T09:23:05.560' AS DateTime), N'Bank Transfer')
INSERT [dbo].[STORE_PAYOUTS] ([PAYOUT_ID], [STORE_ID], [PURCHASE_ID], [PAYOUT_AMOUNT], [PAYOUT_STATUS], [PAYOUT_DATE], [PAYOUT_METHOD]) VALUES (2, 2, 1, CAST(35.00 AS Decimal(10, 2)), N'Paid', CAST(N'2025-03-05T09:23:05.560' AS DateTime), N'Bank Transfer')
INSERT [dbo].[STORE_PAYOUTS] ([PAYOUT_ID], [STORE_ID], [PURCHASE_ID], [PAYOUT_AMOUNT], [PAYOUT_STATUS], [PAYOUT_DATE], [PAYOUT_METHOD]) VALUES (3, 3, 2, CAST(22.00 AS Decimal(10, 2)), N'Paid', CAST(N'2025-03-05T09:23:05.560' AS DateTime), N'PayPal')
GO
INSERT [dbo].[STORES] ([STORE_ID], [STORE_NAME], [EMAIL], [IMAGE], [CATEGORY], [USER_ID], [STRIPE_ID]) VALUES (1, N'Handmade Earring', N'earrings@example.com', N'qiyi-mastermorphix.jpg', N'Jewelry', 1, N'acct_1Nv0FGQ9RKHgCVdK')
INSERT [dbo].[STORES] ([STORE_ID], [STORE_NAME], [EMAIL], [IMAGE], [CATEGORY], [USER_ID], [STRIPE_ID]) VALUES (2, N'Cozy Scarves', N'scarves@example.com', N'scarves.jpg', N'Clothing', 1, N'acct_1Nv0FGQ9RKHgCVdK')
INSERT [dbo].[STORES] ([STORE_ID], [STORE_NAME], [EMAIL], [IMAGE], [CATEGORY], [USER_ID], [STRIPE_ID]) VALUES (3, N'Artistic Fanzines', N'fanzines@example.com', N'yongjun-special-2x2x2-cube-elephant-blue-35135.jpg', N'ART', 2, N'acct_2Nv0FGQ9RKHgCVdZ')
GO
INSERT [dbo].[USERS] ([USER_ID], [NAME], [EMAIL], [PASSWORD_HASH], [SALT], [TELEPHONE], [ADDRESS]) VALUES (1, N'Alice Smith', N'alice.smith@example.com', 0x1234, N'salt123', N'123456789', N'123 Main St')
INSERT [dbo].[USERS] ([USER_ID], [NAME], [EMAIL], [PASSWORD_HASH], [SALT], [TELEPHONE], [ADDRESS]) VALUES (2, N'Bob Johnson', N'bob.johnson@example.com', 0x5678, N'salt456', N'987654321', N'456 Elm St')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__STORES__161CF724D5AB8C4B]    Script Date: 12/03/2025 11:43:44 ******/
ALTER TABLE [dbo].[STORES] ADD UNIQUE NONCLUSTERED 
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__STORES__AF865DE9C9BE1926]    Script Date: 12/03/2025 11:43:44 ******/
ALTER TABLE [dbo].[STORES] ADD UNIQUE NONCLUSTERED 
(
	[STORE_NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__USERS__161CF7240AADE5B4]    Script Date: 12/03/2025 11:43:44 ******/
ALTER TABLE [dbo].[USERS] ADD UNIQUE NONCLUSTERED 
(
	[EMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PAYMENTS] ADD  DEFAULT (getdate()) FOR [PAYMENT_DATE]
GO
ALTER TABLE [dbo].[PURCHASES] ADD  DEFAULT (getdate()) FOR [CREATED_AT]
GO
ALTER TABLE [dbo].[STORE_PAYOUTS] ADD  DEFAULT (getdate()) FOR [PAYOUT_DATE]
GO
ALTER TABLE [dbo].[PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENTS_PURCHASES] FOREIGN KEY([PURCHASE_ID])
REFERENCES [dbo].[PURCHASES] ([PURCHASE_ID])
GO
ALTER TABLE [dbo].[PAYMENTS] CHECK CONSTRAINT [FK_PAYMENTS_PURCHASES]
GO
ALTER TABLE [dbo].[PAYMENTS]  WITH CHECK ADD  CONSTRAINT [FK_PAYMENTS_USERS] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USERS] ([USER_ID])
GO
ALTER TABLE [dbo].[PAYMENTS] CHECK CONSTRAINT [FK_PAYMENTS_USERS]
GO
ALTER TABLE [dbo].[PROD_CAT]  WITH CHECK ADD  CONSTRAINT [FK_PROD_CAT_CATEGORIES] FOREIGN KEY([CATEGORY_ID])
REFERENCES [dbo].[PRODUCTS_CATEGORIES] ([CATEGORY_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PROD_CAT] CHECK CONSTRAINT [FK_PROD_CAT_CATEGORIES]
GO
ALTER TABLE [dbo].[PROD_CAT]  WITH CHECK ADD  CONSTRAINT [FK_PROD_CAT_PRODUCTS] FOREIGN KEY([PRODUCT_ID])
REFERENCES [dbo].[PRODUCTS] ([PRODUCT_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PROD_CAT] CHECK CONSTRAINT [FK_PROD_CAT_PRODUCTS]
GO
ALTER TABLE [dbo].[PURCHASE_ITEMS]  WITH CHECK ADD  CONSTRAINT [FK_PURCHASE_ITEMS_PRODUCTS] FOREIGN KEY([PRODUCT_ID])
REFERENCES [dbo].[PRODUCTS] ([PRODUCT_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PURCHASE_ITEMS] CHECK CONSTRAINT [FK_PURCHASE_ITEMS_PRODUCTS]
GO
ALTER TABLE [dbo].[PURCHASE_ITEMS]  WITH CHECK ADD  CONSTRAINT [FK_PURCHASE_ITEMS_PURCHASES] FOREIGN KEY([PURCHASE_ID])
REFERENCES [dbo].[PURCHASES] ([PURCHASE_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PURCHASE_ITEMS] CHECK CONSTRAINT [FK_PURCHASE_ITEMS_PURCHASES]
GO
ALTER TABLE [dbo].[PURCHASE_VENDOR_MAPPING]  WITH CHECK ADD  CONSTRAINT [FK_PURCHASE_VENDOR_MAPPING_PURCHASES] FOREIGN KEY([PURCHASE_ID])
REFERENCES [dbo].[PURCHASES] ([PURCHASE_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PURCHASE_VENDOR_MAPPING] CHECK CONSTRAINT [FK_PURCHASE_VENDOR_MAPPING_PURCHASES]
GO
ALTER TABLE [dbo].[PURCHASES]  WITH CHECK ADD  CONSTRAINT [FK_PURCHASES_USERS] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USERS] ([USER_ID])
GO
ALTER TABLE [dbo].[PURCHASES] CHECK CONSTRAINT [FK_PURCHASES_USERS]
GO
ALTER TABLE [dbo].[STORE_PAYOUTS]  WITH CHECK ADD  CONSTRAINT [FK_STORE_PAYOUTS_PURCHASES] FOREIGN KEY([PURCHASE_ID])
REFERENCES [dbo].[PURCHASES] ([PURCHASE_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STORE_PAYOUTS] CHECK CONSTRAINT [FK_STORE_PAYOUTS_PURCHASES]
GO
ALTER TABLE [dbo].[STORES]  WITH CHECK ADD  CONSTRAINT [FK_STORES_USERS] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[USERS] ([USER_ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STORES] CHECK CONSTRAINT [FK_STORES_USERS]
GO
ALTER TABLE [dbo].[PURCHASES]  WITH CHECK ADD CHECK  (([PAYMENT_STATUS]='Failed' OR [PAYMENT_STATUS]='Completed' OR [PAYMENT_STATUS]='Pending'))
GO
ALTER TABLE [dbo].[STORE_PAYOUTS]  WITH CHECK ADD CHECK  (([PAYOUT_STATUS]='Failed' OR [PAYOUT_STATUS]='Paid' OR [PAYOUT_STATUS]='Pending'))
GO
ALTER TABLE [dbo].[USERS]  WITH CHECK ADD CHECK  (([TELEPHONE] like '[0-9]%'))
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_USER]    Script Date: 12/03/2025 11:43:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DELETE_USER](@USER_ID INT)
AS
    -- Update PURCHASES Table
    UPDATE PURCHASES
    SET USER_ID = 0
    WHERE USER_ID = @USER_ID;

    -- Update PAYMENTS Table
    UPDATE PAYMENTS
    SET USER_ID = 0
    WHERE USER_ID = @USER_ID;

    -- Delete the User
    DELETE FROM USERS
    WHERE USER_ID = @USER_ID;
GO
USE [master]
GO
ALTER DATABASE [ESHOP] SET  READ_WRITE 
GO
