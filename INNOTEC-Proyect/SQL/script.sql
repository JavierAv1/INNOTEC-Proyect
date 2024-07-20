USE [master]
GO
/****** Object:  Database [INNOTEC]    Script Date: 19/07/2024 11:43:54 p. m. ******/
CREATE DATABASE [INNOTEC]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'INNOTEC', FILENAME = N'C:\inetpub\INNOTEC.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'INNOTEC_log', FILENAME = N'C:\inetpub\INNOTEC_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [INNOTEC] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [INNOTEC].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [INNOTEC] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [INNOTEC] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [INNOTEC] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [INNOTEC] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [INNOTEC] SET ARITHABORT OFF 
GO
ALTER DATABASE [INNOTEC] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [INNOTEC] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [INNOTEC] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [INNOTEC] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [INNOTEC] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [INNOTEC] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [INNOTEC] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [INNOTEC] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [INNOTEC] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [INNOTEC] SET  DISABLE_BROKER 
GO
ALTER DATABASE [INNOTEC] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [INNOTEC] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [INNOTEC] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [INNOTEC] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [INNOTEC] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [INNOTEC] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [INNOTEC] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [INNOTEC] SET RECOVERY FULL 
GO
ALTER DATABASE [INNOTEC] SET  MULTI_USER 
GO
ALTER DATABASE [INNOTEC] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [INNOTEC] SET DB_CHAINING OFF 
GO
ALTER DATABASE [INNOTEC] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [INNOTEC] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [INNOTEC] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [INNOTEC] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [INNOTEC] SET QUERY_STORE = ON
GO
ALTER DATABASE [INNOTEC] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [INNOTEC]
GO
/****** Object:  Table [dbo].[Categoria]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categoria](
	[idCategoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NULL,
	[Descripcion] [varchar](max) NULL,
	[idDepartamento] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compra]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compra](
	[idCompra] [int] IDENTITY(1,1) NOT NULL,
	[FechaVencimiento] [date] NULL,
	[FechaDeCompra] [date] NULL,
	[idusuario] [int] NULL,
	[idproducto] [int] NULL,
	[Cantidad] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departamento]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departamento](
	[idDepartamento] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[idDepartamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Envio]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Envio](
	[idEnvio] [int] IDENTITY(1,1) NOT NULL,
	[CodigoPostal] [varchar](10) NULL,
	[Estado] [varchar](50) NULL,
	[Calle] [varchar](max) NULL,
	[Colonia] [varchar](50) NULL,
	[Municipio] [varchar](50) NULL,
	[Numero] [int] NULL,
	[idCompra] [int] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idEnvio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[idProductos] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](45) NOT NULL,
	[DescripcionDelProducto] [varchar](max) NULL,
	[Precio] [int] NOT NULL,
	[Cantidad] [int] NULL,
	[ImagenDelProducto] [image] NULL,
	[idProveedor] [int] NOT NULL,
	[idDepartamento] [int] NOT NULL,
	[idCategoria] [int] NULL,
	[idSubcategoria] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[idProductos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[idProveedor] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NULL,
	[Telefono] [nvarchar](15) NULL,
	[Direccion] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[idProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subcategoria]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subcategoria](
	[idSubcategoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](255) NULL,
	[idCategoria] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idSubcategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoUsuario]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoUsuario](
	[idTipousuario] [int] IDENTITY(1,1) NOT NULL,
	[TipoUsuario] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[idTipousuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuario]    Script Date: 19/07/2024 11:43:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuario](
	[usuario_id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](45) NULL,
	[ApellidoPaterno] [nvarchar](45) NULL,
	[ApellidoMaterno] [nvarchar](45) NULL,
	[FechaDeNacimiento] [date] NULL,
	[Sexo] [nvarchar](10) NULL,
	[UserName] [nvarchar](50) NULL,
	[Correo] [nvarchar](100) NULL,
	[Contraseña] [nvarchar](100) NULL,
	[Telefono] [nvarchar](15) NULL,
	[Celular] [nvarchar](15) NULL,
	[TipoUsuario_idTipousuario] [int] NOT NULL,
	[FotoDePerfil] [image] NULL,
PRIMARY KEY CLUSTERED 
(
	[usuario_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categoria]  WITH CHECK ADD FOREIGN KEY([idDepartamento])
REFERENCES [dbo].[Departamento] ([idDepartamento])
GO
ALTER TABLE [dbo].[Compra]  WITH CHECK ADD  CONSTRAINT [FK_Compra_Producto] FOREIGN KEY([idproducto])
REFERENCES [dbo].[Productos] ([idProductos])
GO
ALTER TABLE [dbo].[Compra] CHECK CONSTRAINT [FK_Compra_Producto]
GO
ALTER TABLE [dbo].[Compra]  WITH CHECK ADD  CONSTRAINT [FK_Compra_Usuario] FOREIGN KEY([idusuario])
REFERENCES [dbo].[usuario] ([usuario_id])
GO
ALTER TABLE [dbo].[Compra] CHECK CONSTRAINT [FK_Compra_Usuario]
GO
ALTER TABLE [dbo].[Compra]  WITH CHECK ADD  CONSTRAINT [FK_Envio_Compra] FOREIGN KEY([idCompra])
REFERENCES [dbo].[Compra] ([idCompra])
GO
ALTER TABLE [dbo].[Compra] CHECK CONSTRAINT [FK_Envio_Compra]
GO
ALTER TABLE [dbo].[Envio]  WITH CHECK ADD FOREIGN KEY([idCompra])
REFERENCES [dbo].[Compra] ([idCompra])
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD FOREIGN KEY([idDepartamento])
REFERENCES [dbo].[Departamento] ([idDepartamento])
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD FOREIGN KEY([idProveedor])
REFERENCES [dbo].[Proveedor] ([idProveedor])
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Categoria] FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Categoria] ([idCategoria])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Productos_Categoria]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Departamento] FOREIGN KEY([idDepartamento])
REFERENCES [dbo].[Departamento] ([idDepartamento])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Productos_Departamento]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Subcategoria] FOREIGN KEY([idSubcategoria])
REFERENCES [dbo].[Subcategoria] ([idSubcategoria])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Productos_Subcategoria]
GO
ALTER TABLE [dbo].[Subcategoria]  WITH CHECK ADD FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Categoria] ([idCategoria])
GO
ALTER TABLE [dbo].[usuario]  WITH CHECK ADD FOREIGN KEY([TipoUsuario_idTipousuario])
REFERENCES [dbo].[TipoUsuario] ([idTipousuario])
GO
USE [master]
GO
ALTER DATABASE [INNOTEC] SET  READ_WRITE 
GO
