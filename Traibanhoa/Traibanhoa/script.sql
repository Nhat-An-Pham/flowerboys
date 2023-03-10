USE [master]
GO
/****** Object:  Database [Traibanhoa]    Script Date: 3/7/2023 5:00:34 PM ******/
CREATE DATABASE [Traibanhoa]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Traibanhoa', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Traibanhoa.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Traibanhoa_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Traibanhoa_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Traibanhoa] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Traibanhoa].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Traibanhoa] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Traibanhoa] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Traibanhoa] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Traibanhoa] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Traibanhoa] SET ARITHABORT OFF 
GO
ALTER DATABASE [Traibanhoa] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Traibanhoa] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Traibanhoa] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Traibanhoa] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Traibanhoa] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Traibanhoa] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Traibanhoa] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Traibanhoa] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Traibanhoa] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Traibanhoa] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Traibanhoa] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Traibanhoa] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Traibanhoa] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Traibanhoa] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Traibanhoa] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Traibanhoa] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Traibanhoa] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Traibanhoa] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Traibanhoa] SET  MULTI_USER 
GO
ALTER DATABASE [Traibanhoa] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Traibanhoa] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Traibanhoa] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Traibanhoa] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Traibanhoa] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Traibanhoa] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Traibanhoa] SET QUERY_STORE = OFF
GO
USE [Traibanhoa]
GO
/****** Object:  Table [dbo].[Basket]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Basket](
	[basketId] [uniqueidentifier] NOT NULL,
	[title] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[imageURL] [nvarchar](100) NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
	[view] [int] NULL,
	[basketPrice] [money] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Basket] PRIMARY KEY CLUSTERED 
(
	[basketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BasketDetail]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BasketDetail](
	[basketId] [uniqueidentifier] NOT NULL,
	[productId] [uniqueidentifier] NOT NULL,
	[quantity] [int] NULL,
 CONSTRAINT [PK_BasketDetail] PRIMARY KEY CLUSTERED 
(
	[basketId] ASC,
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BasketSubCate]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BasketSubCate](
	[basketId] [uniqueidentifier] NOT NULL,
	[subCateId] [uniqueidentifier] NOT NULL,
	[createdDate] [datetime] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_BasketSubCate] PRIMARY KEY CLUSTERED 
(
	[basketId] ASC,
	[subCateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[cartId] [uniqueidentifier] NOT NULL,
	[customerId] [uniqueidentifier] NULL,
	[quantityOfItem] [int] NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[cartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartDetail]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartDetail](
	[cartId] [uniqueidentifier] NOT NULL,
	[itemId] [uniqueidentifier] NOT NULL,
	[quantity] [int] NULL,
	[unitPrice] [money] NULL,
 CONSTRAINT [PK_CartDetail] PRIMARY KEY CLUSTERED 
(
	[cartId] ASC,
	[itemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[categoryId] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](150) NULL,
	[description] [nvarchar](max) NULL,
	[status] [bit] NULL,
	[createdDate] [datetime] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[customerId] [uniqueidentifier] NOT NULL,
	[username] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[email] [nvarchar](100) NULL,
	[password] [nvarchar](50) NULL,
	[phonenumber] [nvarchar](50) NULL,
	[gender] [bit] NULL,
	[avatar] [nvarchar](100) NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
	[isGoogle] [bit] NULL,
	[isBlocked] [bit] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[customerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[orderId] [uniqueidentifier] NOT NULL,
	[orderDate] [datetime] NULL,
	[shippedDate] [datetime] NULL,
	[shippedAddress] [nvarchar](max) NULL,
	[phonenumber] [nvarchar](50) NULL,
	[email] [nvarchar](100) NULL,
	[totalPrice] [money] NULL,
	[orderStatus] [int] NULL,
	[orderBy] [uniqueidentifier] NULL,
	[confirmBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderBasketDetail]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderBasketDetail](
	[orderId] [uniqueidentifier] NOT NULL,
	[basketId] [uniqueidentifier] NOT NULL,
	[quantity] [int] NULL,
	[price] [money] NULL,
	[isRequest] [bit] NULL,
 CONSTRAINT [PK_OrderBasketDetail] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC,
	[basketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderProductDetail]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderProductDetail](
	[orderId] [uniqueidentifier] NOT NULL,
	[productId] [uniqueidentifier] NOT NULL,
	[quantity] [int] NULL,
	[price] [money] NULL,
 CONSTRAINT [PK_OrderProductDetail] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC,
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[productId] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[picture] [nvarchar](100) NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
	[status] [bit] NULL,
	[price] [money] NULL,
	[typeId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestBasket]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestBasket](
	[requestBasketId] [uniqueidentifier] NOT NULL,
	[title] [nvarchar](50) NULL,
	[imageURL] [nvarchar](100) NULL,
	[createdDate] [datetime] NULL,
	[requestStatus] [int] NULL,
	[createBy] [uniqueidentifier] NULL,
	[confirmBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RequestBasket] PRIMARY KEY CLUSTERED 
(
	[requestBasketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestBasketDetail]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestBasketDetail](
	[requestBasketId] [uniqueidentifier] NOT NULL,
	[productId] [uniqueidentifier] NOT NULL,
	[quantity] [int] NULL,
 CONSTRAINT [PK_RequestBasketDetail] PRIMARY KEY CLUSTERED 
(
	[requestBasketId] ASC,
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategory](
	[subCategoryId] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](150) NULL,
	[description] [nvarchar](max) NULL,
	[createdDate] [datetime] NULL,
	[status] [bit] NULL,
	[categoryId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SubCategory] PRIMARY KEY CLUSTERED 
(
	[subCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[typeId] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[typeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/7/2023 5:00:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userId] [uniqueidentifier] NOT NULL,
	[username] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[email] [nvarchar](100) NULL,
	[password] [nvarchar](50) NULL,
	[phonenumber] [nvarchar](50) NULL,
	[gender] [bit] NULL,
	[avatar] [nvarchar](100) NULL,
	[role] [int] NULL,
	[createdDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
	[isGoogle] [bit] NULL,
	[isBlocked] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_BasketDetail_Basket] FOREIGN KEY([basketId])
REFERENCES [dbo].[Basket] ([basketId])
GO
ALTER TABLE [dbo].[BasketDetail] CHECK CONSTRAINT [FK_BasketDetail_Basket]
GO
ALTER TABLE [dbo].[BasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_BasketDetail_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
GO
ALTER TABLE [dbo].[BasketDetail] CHECK CONSTRAINT [FK_BasketDetail_Product]
GO
ALTER TABLE [dbo].[BasketSubCate]  WITH CHECK ADD  CONSTRAINT [FK_BasketSubCate_Basket] FOREIGN KEY([basketId])
REFERENCES [dbo].[Basket] ([basketId])
GO
ALTER TABLE [dbo].[BasketSubCate] CHECK CONSTRAINT [FK_BasketSubCate_Basket]
GO
ALTER TABLE [dbo].[BasketSubCate]  WITH CHECK ADD  CONSTRAINT [FK_BasketSubCate_SubCategory] FOREIGN KEY([subCateId])
REFERENCES [dbo].[SubCategory] ([subCategoryId])
GO
ALTER TABLE [dbo].[BasketSubCate] CHECK CONSTRAINT [FK_BasketSubCate_SubCategory]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([orderBy])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([confirmBy])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO
ALTER TABLE [dbo].[OrderBasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderBasketDetail_Basket] FOREIGN KEY([basketId])
REFERENCES [dbo].[Basket] ([basketId])
GO
ALTER TABLE [dbo].[OrderBasketDetail] CHECK CONSTRAINT [FK_OrderBasketDetail_Basket]
GO
ALTER TABLE [dbo].[OrderBasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderBasketDetail_Order] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
GO
ALTER TABLE [dbo].[OrderBasketDetail] CHECK CONSTRAINT [FK_OrderBasketDetail_Order]
GO
ALTER TABLE [dbo].[OrderBasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderBasketDetail_RequestBasket] FOREIGN KEY([basketId])
REFERENCES [dbo].[RequestBasket] ([requestBasketId])
GO
ALTER TABLE [dbo].[OrderBasketDetail] CHECK CONSTRAINT [FK_OrderBasketDetail_RequestBasket]
GO
ALTER TABLE [dbo].[OrderProductDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderProductDetail_Order] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
GO
ALTER TABLE [dbo].[OrderProductDetail] CHECK CONSTRAINT [FK_OrderProductDetail_Order]
GO
ALTER TABLE [dbo].[OrderProductDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderProductDetail_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
GO
ALTER TABLE [dbo].[OrderProductDetail] CHECK CONSTRAINT [FK_OrderProductDetail_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Type] FOREIGN KEY([typeId])
REFERENCES [dbo].[Type] ([typeId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Type]
GO
ALTER TABLE [dbo].[RequestBasket]  WITH CHECK ADD  CONSTRAINT [FK_RequestBasket_Customer] FOREIGN KEY([createBy])
REFERENCES [dbo].[Customer] ([customerId])
GO
ALTER TABLE [dbo].[RequestBasket] CHECK CONSTRAINT [FK_RequestBasket_Customer]
GO
ALTER TABLE [dbo].[RequestBasket]  WITH CHECK ADD  CONSTRAINT [FK_RequestBasket_User] FOREIGN KEY([confirmBy])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[RequestBasket] CHECK CONSTRAINT [FK_RequestBasket_User]
GO
ALTER TABLE [dbo].[RequestBasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_RequestBasketDetail_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
GO
ALTER TABLE [dbo].[RequestBasketDetail] CHECK CONSTRAINT [FK_RequestBasketDetail_Product]
GO
ALTER TABLE [dbo].[RequestBasketDetail]  WITH CHECK ADD  CONSTRAINT [FK_RequestBasketDetail_RequestBasket] FOREIGN KEY([requestBasketId])
REFERENCES [dbo].[RequestBasket] ([requestBasketId])
GO
ALTER TABLE [dbo].[RequestBasketDetail] CHECK CONSTRAINT [FK_RequestBasketDetail_RequestBasket]
GO
ALTER TABLE [dbo].[SubCategory]  WITH CHECK ADD  CONSTRAINT [FK_SubCategory_Category] FOREIGN KEY([categoryId])
REFERENCES [dbo].[Category] ([categoryId])
GO
ALTER TABLE [dbo].[SubCategory] CHECK CONSTRAINT [FK_SubCategory_Category]
GO
USE [master]
GO
ALTER DATABASE [Traibanhoa] SET  READ_WRITE 
GO
