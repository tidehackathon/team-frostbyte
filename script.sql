USE [master]
GO
/****** Object:  Database [tide]    Script Date: 2/23/2023 5:25:59 PM ******/
CREATE DATABASE [tide]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tide', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\tide.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'tide_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\tide_log.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [tide] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tide].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tide] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [tide] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [tide] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [tide] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [tide] SET ARITHABORT OFF 
GO
ALTER DATABASE [tide] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [tide] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [tide] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [tide] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [tide] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [tide] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [tide] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [tide] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [tide] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [tide] SET  ENABLE_BROKER 
GO
ALTER DATABASE [tide] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [tide] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [tide] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [tide] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [tide] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [tide] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [tide] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [tide] SET RECOVERY FULL 
GO
ALTER DATABASE [tide] SET  MULTI_USER 
GO
ALTER DATABASE [tide] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [tide] SET DB_CHAINING OFF 
GO
ALTER DATABASE [tide] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [tide] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [tide] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'tide', N'ON'
GO
ALTER DATABASE [tide] SET QUERY_STORE = OFF
GO
USE [tide]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [tide]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Capabilities]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Capabilities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NationId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Capabilities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CapabilityCicles]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CapabilityCicles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[Maturity] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[SuccessRate] [int] NOT NULL,
	[FailureRate] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[CurrentInteroperability] [decimal](18, 2) NOT NULL,
	[BaseInteroperability] [decimal](18, 2) NOT NULL,
	[CapabilityId] [int] NOT NULL,
	[Power] [int] NOT NULL,
 CONSTRAINT [PK_CapabilityCicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CapabilityDescription]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CapabilityDescription](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CapabilityId] [int] NOT NULL,
 CONSTRAINT [PK_CapabilityDescription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CapabilityFaMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CapabilityFaMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FaId] [int] NOT NULL,
	[CapabilityId] [int] NOT NULL,
 CONSTRAINT [PK_CapabilityFaMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Duties]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Duties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Duties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DutyCapabilityMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DutyCapabilityMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DutyId] [int] NOT NULL,
	[CapabilityId] [int] NOT NULL,
 CONSTRAINT [PK_DutyCapabilityMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FocusAreaCycles]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FocusAreaCycles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[FocusAreaId] [int] NOT NULL,
	[Interoperability] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_FocusAreaCycles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FocusAreas]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FocusAreas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FocusAreas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Issues]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Issues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Issues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IssueTestCaseMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueTestCaseMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssueId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
 CONSTRAINT [PK_IssueTestCaseMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nations]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Logo] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Nations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ndpps]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ndpps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Domain] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Ndpps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveCapabilityMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveCapabilityMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CapabilityId] [int] NOT NULL,
	[ObjectiveId] [int] NOT NULL,
	[InteroperabilityScore] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ObjectiveCapabilityMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveCycles]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveCycles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[ObjectiveId] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[InteroperabilityScore] [decimal](18, 2) NOT NULL,
	[Scope] [int] NOT NULL,
	[TcCount] [int] NOT NULL,
	[TcFail] [int] NOT NULL,
	[TcSuccess] [int] NOT NULL,
 CONSTRAINT [PK_ObjectiveCycles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveDescription]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveDescription](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ObjectiveId] [int] NOT NULL,
 CONSTRAINT [PK_ObjectiveDescription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveFaMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveFaMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FaId] [int] NOT NULL,
	[ObjectiveId] [int] NOT NULL,
 CONSTRAINT [PK_ObjectiveFaMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Objectives]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Objectives](
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Objectives] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveTcMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveTcMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestId] [int] NOT NULL,
	[ObjectiveId] [int] NOT NULL,
 CONSTRAINT [PK_ObjectiveTcMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveTtMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveTtMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TemplateId] [int] NOT NULL,
	[ObjectiveId] [int] NOT NULL,
 CONSTRAINT [PK_ObjectiveTtMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationalDomainCapabilityMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationalDomainCapabilityMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DomainId] [int] NOT NULL,
	[CapabilityId] [int] NOT NULL,
 CONSTRAINT [PK_OperationalDomainCapabilityMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationalDomains]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationalDomains](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OperationalDomains] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participants]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Result] [int] NOT NULL,
	[Remarks] [nvarchar](max) NOT NULL,
	[TestId] [int] NOT NULL,
	[CapabilityId] [int] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_Participants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StandardCapabilityMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardCapabilityMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StandardId] [int] NOT NULL,
	[CapabilityId] [int] NOT NULL,
	[InteroperabilityScore] [decimal](18, 2) NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_StandardCapabilityMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StandardObjectiveMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardObjectiveMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StandardId] [int] NOT NULL,
	[ObjectiveId] [int] NOT NULL,
 CONSTRAINT [PK_StandardObjectiveMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Standards]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Standards](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Standards] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StandardTtMap]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StandardTtMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StandardId] [int] NOT NULL,
	[TestTemplateId] [int] NOT NULL,
 CONSTRAINT [PK_StandardTtMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateCycles]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateCycles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](max) NOT NULL,
	[Year] [int] NOT NULL,
	[TestTemplateId] [int] NOT NULL,
	[DiffusionId] [int] NULL,
	[Similarity] [decimal](18, 2) NOT NULL,
	[DiffusionSimilarity] [decimal](18, 2) NOT NULL,
	[TestsCount] [int] NOT NULL,
	[Anomaly] [int] NOT NULL,
 CONSTRAINT [PK_TemplateCycles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateDescriptions]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateDescriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Purpose] [nvarchar](max) NOT NULL,
	[Preconditions] [nvarchar](max) NOT NULL,
	[TemplateId] [int] NOT NULL,
 CONSTRAINT [PK_TemplateDescriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateResults]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateResults](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Success] [nvarchar](max) NOT NULL,
	[Limited] [nvarchar](max) NOT NULL,
	[Interoperability] [nvarchar](max) NOT NULL,
	[TemplateId] [int] NOT NULL,
 CONSTRAINT [PK_TemplateResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Templates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Maturity] [int] NOT NULL,
 CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tests]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TemplateId] [int] NULL,
	[Number] [nvarchar](max) NOT NULL,
	[Year] [int] NOT NULL,
	[Result] [int] NOT NULL,
	[Shortfall] [bit] NOT NULL,
	[ParticipantsCount] [int] NOT NULL,
 CONSTRAINT [PK_Tests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TtYearAnomalies]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TtYearAnomalies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TemplateId] [int] NOT NULL,
	[TemplateName] [nvarchar](max) NOT NULL,
	[ObjectiveId] [int] NOT NULL,
	[ObjectiveName] [nvarchar](max) NOT NULL,
	[FaId] [int] NOT NULL,
	[FaName] [nvarchar](max) NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_TtYearAnomalies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Capabilities_NationId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Capabilities_NationId] ON [dbo].[Capabilities]
(
	[NationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CapabilityCicles_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_CapabilityCicles_CapabilityId] ON [dbo].[CapabilityCicles]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CapabilityDescription_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_CapabilityDescription_CapabilityId] ON [dbo].[CapabilityDescription]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CapabilityFaMap_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_CapabilityFaMap_CapabilityId] ON [dbo].[CapabilityFaMap]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CapabilityFaMap_FaId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_CapabilityFaMap_FaId] ON [dbo].[CapabilityFaMap]
(
	[FaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DutyCapabilityMap_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_DutyCapabilityMap_CapabilityId] ON [dbo].[DutyCapabilityMap]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DutyCapabilityMap_DutyId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_DutyCapabilityMap_DutyId] ON [dbo].[DutyCapabilityMap]
(
	[DutyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FocusAreaCycles_FocusAreaId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_FocusAreaCycles_FocusAreaId] ON [dbo].[FocusAreaCycles]
(
	[FocusAreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IssueTestCaseMap_IssueId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_IssueTestCaseMap_IssueId] ON [dbo].[IssueTestCaseMap]
(
	[IssueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_IssueTestCaseMap_TestId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_IssueTestCaseMap_TestId] ON [dbo].[IssueTestCaseMap]
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveCapabilityMap_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveCapabilityMap_CapabilityId] ON [dbo].[ObjectiveCapabilityMap]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveCapabilityMap_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveCapabilityMap_ObjectiveId] ON [dbo].[ObjectiveCapabilityMap]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveCycles_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveCycles_ObjectiveId] ON [dbo].[ObjectiveCycles]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveDescription_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ObjectiveDescription_ObjectiveId] ON [dbo].[ObjectiveDescription]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveFaMap_FaId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveFaMap_FaId] ON [dbo].[ObjectiveFaMap]
(
	[FaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveFaMap_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveFaMap_ObjectiveId] ON [dbo].[ObjectiveFaMap]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveTcMap_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveTcMap_ObjectiveId] ON [dbo].[ObjectiveTcMap]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveTcMap_TestId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveTcMap_TestId] ON [dbo].[ObjectiveTcMap]
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveTtMap_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveTtMap_ObjectiveId] ON [dbo].[ObjectiveTtMap]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectiveTtMap_TemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectiveTtMap_TemplateId] ON [dbo].[ObjectiveTtMap]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OperationalDomainCapabilityMap_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_OperationalDomainCapabilityMap_CapabilityId] ON [dbo].[OperationalDomainCapabilityMap]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OperationalDomainCapabilityMap_DomainId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_OperationalDomainCapabilityMap_DomainId] ON [dbo].[OperationalDomainCapabilityMap]
(
	[DomainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Participants_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Participants_CapabilityId] ON [dbo].[Participants]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Participants_TestId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Participants_TestId] ON [dbo].[Participants]
(
	[TestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StandardCapabilityMap_CapabilityId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_StandardCapabilityMap_CapabilityId] ON [dbo].[StandardCapabilityMap]
(
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StandardCapabilityMap_StandardId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_StandardCapabilityMap_StandardId] ON [dbo].[StandardCapabilityMap]
(
	[StandardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StandardObjectiveMap_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_StandardObjectiveMap_ObjectiveId] ON [dbo].[StandardObjectiveMap]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StandardObjectiveMap_StandardId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_StandardObjectiveMap_StandardId] ON [dbo].[StandardObjectiveMap]
(
	[StandardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StandardTtMap_StandardId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_StandardTtMap_StandardId] ON [dbo].[StandardTtMap]
(
	[StandardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StandardTtMap_TestTemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_StandardTtMap_TestTemplateId] ON [dbo].[StandardTtMap]
(
	[TestTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TemplateCycles_DiffusionId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_TemplateCycles_DiffusionId] ON [dbo].[TemplateCycles]
(
	[DiffusionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TemplateCycles_TestTemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_TemplateCycles_TestTemplateId] ON [dbo].[TemplateCycles]
(
	[TestTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TemplateDescriptions_TemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TemplateDescriptions_TemplateId] ON [dbo].[TemplateDescriptions]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TemplateResults_TemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TemplateResults_TemplateId] ON [dbo].[TemplateResults]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tests_TemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Tests_TemplateId] ON [dbo].[Tests]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TtYearAnomalies_FaId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_TtYearAnomalies_FaId] ON [dbo].[TtYearAnomalies]
(
	[FaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TtYearAnomalies_ObjectiveId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_TtYearAnomalies_ObjectiveId] ON [dbo].[TtYearAnomalies]
(
	[ObjectiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TtYearAnomalies_TemplateId]    Script Date: 2/23/2023 5:26:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_TtYearAnomalies_TemplateId] ON [dbo].[TtYearAnomalies]
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ObjectiveCycles] ADD  DEFAULT ((0)) FOR [TcCount]
GO
ALTER TABLE [dbo].[ObjectiveCycles] ADD  DEFAULT ((0)) FOR [TcFail]
GO
ALTER TABLE [dbo].[ObjectiveCycles] ADD  DEFAULT ((0)) FOR [TcSuccess]
GO
ALTER TABLE [dbo].[Capabilities]  WITH CHECK ADD  CONSTRAINT [FK_Capabilities_Nations_NationId] FOREIGN KEY([NationId])
REFERENCES [dbo].[Nations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Capabilities] CHECK CONSTRAINT [FK_Capabilities_Nations_NationId]
GO
ALTER TABLE [dbo].[CapabilityCicles]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityCicles_Capabilities_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capabilities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CapabilityCicles] CHECK CONSTRAINT [FK_CapabilityCicles_Capabilities_CapabilityId]
GO
ALTER TABLE [dbo].[CapabilityDescription]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityDescription_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CapabilityDescription] CHECK CONSTRAINT [FK_CapabilityDescription_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[CapabilityFaMap]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityFaMap_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CapabilityFaMap] CHECK CONSTRAINT [FK_CapabilityFaMap_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[CapabilityFaMap]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityFaMap_FocusAreaCycles_FaId] FOREIGN KEY([FaId])
REFERENCES [dbo].[FocusAreaCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CapabilityFaMap] CHECK CONSTRAINT [FK_CapabilityFaMap_FocusAreaCycles_FaId]
GO
ALTER TABLE [dbo].[DutyCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_DutyCapabilityMap_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DutyCapabilityMap] CHECK CONSTRAINT [FK_DutyCapabilityMap_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[DutyCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_DutyCapabilityMap_Duties_DutyId] FOREIGN KEY([DutyId])
REFERENCES [dbo].[Duties] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DutyCapabilityMap] CHECK CONSTRAINT [FK_DutyCapabilityMap_Duties_DutyId]
GO
ALTER TABLE [dbo].[FocusAreaCycles]  WITH CHECK ADD  CONSTRAINT [FK_FocusAreaCycles_FocusAreas_FocusAreaId] FOREIGN KEY([FocusAreaId])
REFERENCES [dbo].[FocusAreas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FocusAreaCycles] CHECK CONSTRAINT [FK_FocusAreaCycles_FocusAreas_FocusAreaId]
GO
ALTER TABLE [dbo].[IssueTestCaseMap]  WITH CHECK ADD  CONSTRAINT [FK_IssueTestCaseMap_Issues_IssueId] FOREIGN KEY([IssueId])
REFERENCES [dbo].[Issues] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IssueTestCaseMap] CHECK CONSTRAINT [FK_IssueTestCaseMap_Issues_IssueId]
GO
ALTER TABLE [dbo].[IssueTestCaseMap]  WITH CHECK ADD  CONSTRAINT [FK_IssueTestCaseMap_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[Tests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IssueTestCaseMap] CHECK CONSTRAINT [FK_IssueTestCaseMap_Tests_TestId]
GO
ALTER TABLE [dbo].[ObjectiveCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveCapabilityMap_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveCapabilityMap] CHECK CONSTRAINT [FK_ObjectiveCapabilityMap_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[ObjectiveCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveCapabilityMap_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveCapabilityMap] CHECK CONSTRAINT [FK_ObjectiveCapabilityMap_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[ObjectiveCycles]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveCycles_Objectives_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[Objectives] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveCycles] CHECK CONSTRAINT [FK_ObjectiveCycles_Objectives_ObjectiveId]
GO
ALTER TABLE [dbo].[ObjectiveDescription]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveDescription_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveDescription] CHECK CONSTRAINT [FK_ObjectiveDescription_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[ObjectiveFaMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveFaMap_FocusAreaCycles_FaId] FOREIGN KEY([FaId])
REFERENCES [dbo].[FocusAreaCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveFaMap] CHECK CONSTRAINT [FK_ObjectiveFaMap_FocusAreaCycles_FaId]
GO
ALTER TABLE [dbo].[ObjectiveFaMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveFaMap_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveFaMap] CHECK CONSTRAINT [FK_ObjectiveFaMap_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[ObjectiveTcMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveTcMap_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveTcMap] CHECK CONSTRAINT [FK_ObjectiveTcMap_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[ObjectiveTcMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveTcMap_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[Tests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveTcMap] CHECK CONSTRAINT [FK_ObjectiveTcMap_Tests_TestId]
GO
ALTER TABLE [dbo].[ObjectiveTtMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveTtMap_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveTtMap] CHECK CONSTRAINT [FK_ObjectiveTtMap_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[ObjectiveTtMap]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveTtMap_TemplateCycles_TemplateId] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[TemplateCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveTtMap] CHECK CONSTRAINT [FK_ObjectiveTtMap_TemplateCycles_TemplateId]
GO
ALTER TABLE [dbo].[OperationalDomainCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_OperationalDomainCapabilityMap_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OperationalDomainCapabilityMap] CHECK CONSTRAINT [FK_OperationalDomainCapabilityMap_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[OperationalDomainCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_OperationalDomainCapabilityMap_OperationalDomains_DomainId] FOREIGN KEY([DomainId])
REFERENCES [dbo].[OperationalDomains] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OperationalDomainCapabilityMap] CHECK CONSTRAINT [FK_OperationalDomainCapabilityMap_OperationalDomains_DomainId]
GO
ALTER TABLE [dbo].[Participants]  WITH CHECK ADD  CONSTRAINT [FK_Participants_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Participants] CHECK CONSTRAINT [FK_Participants_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[Participants]  WITH CHECK ADD  CONSTRAINT [FK_Participants_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[Tests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Participants] CHECK CONSTRAINT [FK_Participants_Tests_TestId]
GO
ALTER TABLE [dbo].[StandardCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_StandardCapabilityMap_CapabilityCicles_CapabilityId] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[CapabilityCicles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StandardCapabilityMap] CHECK CONSTRAINT [FK_StandardCapabilityMap_CapabilityCicles_CapabilityId]
GO
ALTER TABLE [dbo].[StandardCapabilityMap]  WITH CHECK ADD  CONSTRAINT [FK_StandardCapabilityMap_Standards_StandardId] FOREIGN KEY([StandardId])
REFERENCES [dbo].[Standards] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StandardCapabilityMap] CHECK CONSTRAINT [FK_StandardCapabilityMap_Standards_StandardId]
GO
ALTER TABLE [dbo].[StandardObjectiveMap]  WITH CHECK ADD  CONSTRAINT [FK_StandardObjectiveMap_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StandardObjectiveMap] CHECK CONSTRAINT [FK_StandardObjectiveMap_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[StandardObjectiveMap]  WITH CHECK ADD  CONSTRAINT [FK_StandardObjectiveMap_Standards_StandardId] FOREIGN KEY([StandardId])
REFERENCES [dbo].[Standards] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StandardObjectiveMap] CHECK CONSTRAINT [FK_StandardObjectiveMap_Standards_StandardId]
GO
ALTER TABLE [dbo].[StandardTtMap]  WITH CHECK ADD  CONSTRAINT [FK_StandardTtMap_Standards_StandardId] FOREIGN KEY([StandardId])
REFERENCES [dbo].[Standards] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StandardTtMap] CHECK CONSTRAINT [FK_StandardTtMap_Standards_StandardId]
GO
ALTER TABLE [dbo].[StandardTtMap]  WITH CHECK ADD  CONSTRAINT [FK_StandardTtMap_Templates_TestTemplateId] FOREIGN KEY([TestTemplateId])
REFERENCES [dbo].[Templates] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StandardTtMap] CHECK CONSTRAINT [FK_StandardTtMap_Templates_TestTemplateId]
GO
ALTER TABLE [dbo].[TemplateCycles]  WITH CHECK ADD  CONSTRAINT [FK_TemplateCycles_TemplateCycles_DiffusionId] FOREIGN KEY([DiffusionId])
REFERENCES [dbo].[TemplateCycles] ([Id])
GO
ALTER TABLE [dbo].[TemplateCycles] CHECK CONSTRAINT [FK_TemplateCycles_TemplateCycles_DiffusionId]
GO
ALTER TABLE [dbo].[TemplateCycles]  WITH CHECK ADD  CONSTRAINT [FK_TemplateCycles_Templates_TestTemplateId] FOREIGN KEY([TestTemplateId])
REFERENCES [dbo].[Templates] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemplateCycles] CHECK CONSTRAINT [FK_TemplateCycles_Templates_TestTemplateId]
GO
ALTER TABLE [dbo].[TemplateDescriptions]  WITH CHECK ADD  CONSTRAINT [FK_TemplateDescriptions_Templates_TemplateId] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Templates] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemplateDescriptions] CHECK CONSTRAINT [FK_TemplateDescriptions_Templates_TemplateId]
GO
ALTER TABLE [dbo].[TemplateResults]  WITH CHECK ADD  CONSTRAINT [FK_TemplateResults_Templates_TemplateId] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Templates] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemplateResults] CHECK CONSTRAINT [FK_TemplateResults_Templates_TemplateId]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_TemplateCycles_TemplateId] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[TemplateCycles] ([Id])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_TemplateCycles_TemplateId]
GO
ALTER TABLE [dbo].[TtYearAnomalies]  WITH CHECK ADD  CONSTRAINT [FK_TtYearAnomalies_FocusAreas_FaId] FOREIGN KEY([FaId])
REFERENCES [dbo].[FocusAreas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TtYearAnomalies] CHECK CONSTRAINT [FK_TtYearAnomalies_FocusAreas_FaId]
GO
ALTER TABLE [dbo].[TtYearAnomalies]  WITH CHECK ADD  CONSTRAINT [FK_TtYearAnomalies_ObjectiveCycles_ObjectiveId] FOREIGN KEY([ObjectiveId])
REFERENCES [dbo].[ObjectiveCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TtYearAnomalies] CHECK CONSTRAINT [FK_TtYearAnomalies_ObjectiveCycles_ObjectiveId]
GO
ALTER TABLE [dbo].[TtYearAnomalies]  WITH CHECK ADD  CONSTRAINT [FK_TtYearAnomalies_TemplateCycles_TemplateId] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[TemplateCycles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TtYearAnomalies] CHECK CONSTRAINT [FK_TtYearAnomalies_TemplateCycles_TemplateId]
GO
/****** Object:  StoredProcedure [dbo].[chart_all_tt_year_anomalies]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[chart_all_tt_year_anomalies]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @year INT;

declare @years table (yr int)
insert into @years (yr) exec tide_years

DECLARE year_cursor CURSOR FOR
SELECT yr FROM @years;

OPEN year_cursor;

FETCH NEXT FROM year_cursor INTO @year;

WHILE @@FETCH_STATUS = 0
BEGIN
  exec chart_tt_year_anomalies @year
  FETCH NEXT FROM year_cursor INTO @year;
END;

CLOSE year_cursor;
DEALLOCATE year_cursor;
END
GO
/****** Object:  StoredProcedure [dbo].[chart_tt_year_anomalies]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[chart_tt_year_anomalies]
@year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [dbo].[TtYearAnomalies]
           ([TemplateId]
           ,[TemplateName]
           ,[ObjectiveId]
           ,[ObjectiveName]
           ,[FaId]
           ,[FaName]
           ,[Year])
select TT.Id, TT.Number,O.Id,O.Number,FA.Id,FA.Name,Year=@year
from FocusAreas FA 
inner join FocusAreaCycles FAC on FAC.FocusAreaId=FA.Id and FAC.Year=@year
inner join ObjectiveFaMap OFA on OFA.FaId=FAC.Id
inner join ObjectiveCycles O on O.Id=OFA.ObjectiveId and O.Year=@year
inner join ObjectiveTtMap OTT on OTT.ObjectiveId=O.Id
inner join TemplateCycles TT on TT.Id=OTT.TemplateId and TT.Year=@year
where TT.DiffusionId is not null


END
GO
/****** Object:  StoredProcedure [dbo].[enh_obj_tc]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[enh_obj_tc]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     declare @t table (id int, result int)

insert into @t select O.Id,P.Result from ObjectiveCycles O
inner join ObjectiveCapabilityMap OC on Oc.ObjectiveId=O.Id
inner join Participants P on P.CapabilityId=Oc.CapabilityId

declare @total table (id int, result int)

insert into @total select id,count(*) from @t group by Id

Update O
set O.TcCount=T.result
from ObjectiveCycles O inner join @total T on T.id=O.Id

declare @suc table (id int, result int)

insert into @suc select id,count(*) from @t where result=0 or result=1  group by Id

Update O
set O.TcSuccess=T.result
from ObjectiveCycles O inner join @suc T on T.id=O.Id

declare @err table (id int, result int)

insert into @err select id,count(*) from @t where result=2  group by Id

Update O
set O.TcFail=T.result
from ObjectiveCycles O inner join @err T on T.id=O.Id
END
GO
/****** Object:  StoredProcedure [dbo].[enh_tt_anomalies]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[enh_tt_anomalies]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     declare @tt table (id int,cnt int)

insert into @tt select TestTemplateId as Id, count(*) as Cnt from TemplateCycles T where DiffusionId is not null group by TestTemplateId

update T
set T.Anomaly=x.cnt
from TemplateCycles T inner join @tt X on X.id=T.Id
END
GO
/****** Object:  StoredProcedure [dbo].[enh_tt_maturity]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[enh_tt_maturity]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @tt table (id int,cnt int)

insert into @tt select TestTemplateid as id,count(*) as count from TemplateCycles where DiffusionId is null group by  TestTemplateid

update Templates set Maturity=1

update  T
Set T.Maturity=X.cnt
from Templates T inner join @tt X on T.Id=X.Id
END
GO
/****** Object:  StoredProcedure [dbo].[enh_tt_tc_count]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[enh_tt_tc_count]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   declare @tt table (id int,cnt int)

insert into @tt select TemplateId as Id,count(TemplateId) as cnt from Tests where TemplateId is not null group by TemplateId

update T
set T.TestsCount=X.cnt
from TemplateCycles T inner join @tt X on T.Id=X.id
END
GO
/****** Object:  StoredProcedure [dbo].[tide_years]    Script Date: 2/23/2023 5:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[tide_years]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Distinct Year from FocusAreaCycles order by Year desc
END
GO
USE [master]
GO
ALTER DATABASE [tide] SET  READ_WRITE 
GO
