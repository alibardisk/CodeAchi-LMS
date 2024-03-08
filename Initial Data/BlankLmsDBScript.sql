USE [LMS]
GO
/****** Object:  Table [dbo].[accnSetting]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[accnSetting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[isAutoGenerate] [bit] NULL,
	[isManualPrefix] [bit] NULL,
	[prefixText] [nchar](100) NULL,
	[joiningChar] [nchar](50) NULL,
	[lastNumber] [nchar](10) NULL,
 CONSTRAINT [PK_accnSetting] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[brrIdSetting]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[brrIdSetting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[isAutoGenerate] [bit] NULL,
	[isManualPrefix] [bit] NULL,
	[prefixText] [nchar](100) NULL,
	[joiningChar] [nchar](50) NULL,
	[lastNumber] [nchar](10) NULL,
 CONSTRAINT [PK_Activation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[borrowerDetails]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[borrowerDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brrId] [nchar](150) NULL,
	[brrName] [nvarchar](250) NULL,
	[brrCategory] [nvarchar](150) NULL,
	[brrAddress] [nvarchar](500) NULL,
	[brrGender] [nchar](50) NULL,
	[brrMailId] [nchar](150) NULL,
	[brrContact] [nchar](150) NULL,
	[mbershipDuration] [nchar](50) NULL,
	[addInfo1] [nchar](150) NULL,
	[addInfo2] [nchar](150) NULL,
	[addInfo3] [nchar](150) NULL,
	[addInfo4] [nchar](150) NULL,
	[addInfo5] [nchar](150) NULL,
	[entryDate] [nchar](150) NULL,
	[brrImage] [ntext] NULL,
	[opkPermission] [bit] NULL,
	[renewDate] [nchar](150) NULL,
 CONSTRAINT [PK_borrowerDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[borrowerSettings]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[borrowerSettings](
	[categoryId] [int] IDENTITY(1,1) NOT NULL,
	[catName] [nchar](150) NULL,
	[catDesc] [nvarchar](250) NULL,
	[catlogAccess] [nvarchar] (500) NULL,
	[capInfo] [nvarchar](500) NULL,
	[maxCheckin] [nchar](50) NULL,
	[maxDue] [nchar](50) NULL,
	[membershipFees] [nchar](50) NULL,
	[avoidFine] [bit] NULL,
 CONSTRAINT [PK_borrowerSettings] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[countryDetails]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[countryDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[countryName] [nchar](150) NULL,
	[currencyName] [nchar](150) NULL,
	[cuurShort] [nchar] (50) NULL,
	[currSymbol] [nchar](50) NULL,
	[dialCode] [nchar](50) NULL,
 CONSTRAINT [PK_countryDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[generalSettings]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[generalSettings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[instName] [nchar](150) NULL,
	[instShortName] [nchar](150) NULL,
	[instAddress] [nvarchar](250) NULL,							
	[cityCode] [nchar](50) NULL,
	[instLogo] [ntext] NULL,
	[instContact] [nchar](50) NULL,
	[instWebsite] [nchar](150) NULL,
	[instMail] [nchar](150) NULL,
	[libraryName] [nchar](200) NULL,
	[countryName] [nchar](100) NULL,
	[dialCode] [nchar](50) NULL,
	[currencyName] [nchar](150) NULL,
	[currShortName] [nchar](50) NULL,
	[currSymbol] [nchar](150) NULL,
	[databasePath] [nvarchar](350) NULL,
	[backupPath] [nvarchar](350) NULL,
	[backupHour] [nchar](50) NULL,
	[printerName] [nvarchar](250) NULL,
	[productExpired] [bit] NULL,
	[licenseType] [nchar](150) NULL,
	[licenseKey] [nchar](150) NULL,
	[stepComplete] [nchar](50) NULL,
	[productBlocked] [bit] NULL,
	[reserveSystemLimit] [int] NULL,
	[opacAvailable] [bit] NULL,
	[machineId] [nvarchar](1000) NULL,
	[notificationData] [nvarchar](2000) NULL,
	[settingsData] [nvarchar](2000) NULL,
 CONSTRAINT [PK_generalSettings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[issuedItem]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[issuedItem](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brrId] [nchar](150) NULL,
	[itemAccession] [nchar](150) NULL,
	[issueDate] [nchar](50) NULL,
	[issueComment] [nvarchar](250) NULL,							
	[reissuedDate] [nchar](50) NULL,
	[reissuedComment] [nvarchar](250) NULL,			
	[expectedReturnDate] [nchar](50) NULL,
	[returnDate] [nchar](50) NULL,
	[returnedComment] [nvarchar](250) NULL,	
	[itemReturned] [bit] NULL,
	[issuedBy] [nchar](150) NULL,
	[reissuedBy] [nchar](150) NULL,
	[returnedBy] [nchar](150) NULL,
 CONSTRAINT [PK_issuedItem] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[itemDetails]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[itemDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[itemAccession] [nchar](150) NULL,
	[itemIsbn] [nchar](150) NULL,
	[itemTitle] [nvarchar](250) NULL,
	[itemCat] [nvarchar] (250) NULL,
	[itemSubCat] [nvarchar](250) NULL,
	[itemAuthor] [nvarchar](250) NULL,
	[itemClassification] [nchar](150) NULL,
	[itemSubject] [nvarchar](250) NULL,
	[rackNo] [nvarchar](250) NULL,
	[totalPages] [nchar](50) NULL,
	[itemPrice] [nchar](50) NULL,
	[addInfo1] [nchar](150) NULL,
	[addInfo2] [nchar](150) NULL,
	[addInfo3] [nchar](150) NULL,
	[addInfo4] [nchar](150) NULL,
	[addInfo5] [nchar](150) NULL,
	[addInfo6] [nchar](150) NULL,
	[addInfo7] [nchar](150) NULL,
	[addInfo8] [nchar](150) NULL,
	[entryDate] [nchar](50) NULL,
	[itemAvailable] [bit] NULL,
	[isLost] [bit] NULL,
	[isDamage] [bit] NULL,
	[itemImage] [ntext] NULL,
	[digitalReference] [nvarchar](500) NULL,
 CONSTRAINT [PK_itemDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[itemSettings]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[itemSettings](
	[catId] [int] IDENTITY(1,1) NOT NULL,
	[catName] [nchar](150) NULL,
	[catDesc] [nvarchar](250) NULL,
	[capInfo] [nvarchar](500) NULL,
	[isConsume] [bit] NULL,
 CONSTRAINT [PK_itemSettings] PRIMARY KEY CLUSTERED 
(
	[catId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[itemSubCategory]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[itemSubCategory](
	[subCatId] [int] IDENTITY(1,1) NOT NULL,
	[catName] [nchar](150) NULL,
	[subCatName] [nchar](150) NULL,
	[shortName] [nchar](50) NULL,
	[subCatDesc] [nvarchar](250) NULL,
	[notReference] [bit] NOT NULL,
	[issueDays] [nchar](50) NULL,
	[subCatFine] [nchar](50) NULL,
 CONSTRAINT [PK_itemSubCategory] PRIMARY KEY CLUSTERED 
(
	[subCatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lostDamage]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lostDamage](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[itemAccn] [nchar](150) NULL,
	[itemStatus] [nchar](50) NULL,
	[statusComment] [nvarchar](500) NULL,
	[entryDate] [nchar](50) NULL,
	[brrId] [nchar](150) NULL,
	[entryBy] [nchar](150) NULL,
 CONSTRAINT [PK_lostDamage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[noticeTemplate]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[noticeTemplate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tempName] [nchar](150) NULL,
	[senderId] [nchar](150) NULL,
	[htmlBody] [bit] NULL,
	[bodyText] [nvarchar](2000) NULL,
	[noticeType] [nchar](150) NULL,
 CONSTRAINT [PK_noticeTemplate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[paymentDetails]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[paymentDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[feesDate] [nchar](50) NULL,
	[memberId] [nchar](150) NULL,
	[invId] [nchar](50) NULL,
	[itemAccn] [nchar](50) NULL,
	[feesDesc] [nvarchar](250) NULL,
	[dueAmount] [nchar](50) NULL,
	[isPaid] [bit] NOT NULL,
	[isRemited] [bit] NOT NULL,
	[discountAmnt] [nchar](50) NULL,
	[payDate] [nchar](50) NULL,
	[invidCount] [nchar](50) NULL,
	[paymentMode] [nchar](150) NULL,
	[paymentRef] [nchar](150) NULL,
	[collctedBy] [nchar](150) NULL,
 CONSTRAINT [PK_paymentDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reportSetting]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reportSetting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[isFullname] [bit] NULL,
	[isShortName] [bit] NULL,
	[isAddress] [bit] NULL,
	[isContact] [bit] NULL,
	[isMail] [bit] NULL,
	[isWebsite] [bit] NULL,
 CONSTRAINT [PK_reportSetting] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[reservationList]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reservationList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[brrId] [nchar](50) NULL,
	[itemAccn] [nchar](50) NULL,
	[itemTitle] [nvarchar](250) NULL,
	[itemAuthor] [nvarchar](250) NULL,
	[reserveLocation] [nchar](150) NULL,
	[reserveDate] [nchar](50) NULL,
	[availableDate] [nchar](50) NULL,
 CONSTRAINT [PK_reservationList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[userActivity]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userActivity](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [nchar](150) NULL,
	[brrId] [nchar](50) NULL,
	[itemAccn] [nvarchar](max) NULL,
	[taskDesc] [nvarchar](250) NULL,
	[entryDate] [nchar](50) NULL,
 CONSTRAINT [PK_userActivity] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[userDetails]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nchar](150) NULL,
	[userDesig] [nvarchar](250) NULL,
	[userMail] [nvarchar] (150) NULL,
	[userContact] [nchar](50) NULL,
	[userPassword] [nchar](50) NULL,
	[userAddress] [nvarchar](250) NULL,
	[userPriviledge] [nvarchar](500) NULL,
	[isActive] [bit] NULL,
	[isAdmin] [bit] NULL,
	[userImage] [ntext] NULL,
 CONSTRAINT [PK_userDetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wishList]    Script Date: 04-06-2018 11:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wishList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[itemType] [nchar](150) NULL,
	[itemTitle] [nvarchar](250) NULL,
	[itemAuthor] [nvarchar](250) NULL,
	[itemPublication] [nvarchar](250) NULL,
	[queryDate] [nchar](50) NULL,
	[queryCount] [nchar](50) NULL,
 CONSTRAINT [PK_wishList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [LMS] SET READ_WRITE
GO