USE [master]
GO
/****** Object:  Database [CASA_NATURA]    Script Date: 7/6/2025 6:15:35 PM ******/
CREATE DATABASE [CASA_NATURA]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CASA_NATURA', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CASA_NATURA.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CASA_NATURA_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CASA_NATURA_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CASA_NATURA] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CASA_NATURA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CASA_NATURA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CASA_NATURA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CASA_NATURA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CASA_NATURA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CASA_NATURA] SET ARITHABORT OFF 
GO
ALTER DATABASE [CASA_NATURA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CASA_NATURA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CASA_NATURA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CASA_NATURA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CASA_NATURA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CASA_NATURA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CASA_NATURA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CASA_NATURA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CASA_NATURA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CASA_NATURA] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CASA_NATURA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CASA_NATURA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CASA_NATURA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CASA_NATURA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CASA_NATURA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CASA_NATURA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CASA_NATURA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CASA_NATURA] SET RECOVERY FULL 
GO
ALTER DATABASE [CASA_NATURA] SET  MULTI_USER 
GO
ALTER DATABASE [CASA_NATURA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CASA_NATURA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CASA_NATURA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CASA_NATURA] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CASA_NATURA] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CASA_NATURA] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CASA_NATURA', N'ON'
GO
ALTER DATABASE [CASA_NATURA] SET QUERY_STORE = ON
GO
ALTER DATABASE [CASA_NATURA] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CASA_NATURA]
GO
/****** Object:  Table [dbo].[ACTIVIDADES_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACTIVIDADES_TB](
	[ID_ACTIVIDAD] [int] IDENTITY(1,1) NOT NULL,
	[DESCRIPCION] [varchar](4000) NOT NULL,
	[FECHA] [datetime] NOT NULL,
	[PRECIO_BOLETO] [decimal](10, 2) NOT NULL,
	[TICKETS_DISPONIBLES] [int] NULL,
	[TICKETS_VENDIDOS] [int] NULL,
	[ID_ESTADO] [int] NOT NULL,
	[IMAGEN] [varchar](255) NULL,
	[TIPO] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_ACTIVIDAD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANIMAL_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANIMAL_TB](
	[ID_ANIMAL] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](30) NULL,
	[ID_RAZA] [int] NOT NULL,
	[FECHA_INGRESO] [datetime] NOT NULL,
	[FECHA_BAJA] [datetime] NULL,
	[FECHA_NACIMIENTO] [datetime] NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
	[ID_SALUD] [int] NOT NULL,
	[IMAGEN] [varchar](255) NULL,
	[HISTORIA] [varchar](255) NULL,
	[NECESIDAD] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_ANIMAL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[APADRINAMIENTOS_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[APADRINAMIENTOS_TB](
	[ID_APADRINAMIENTO] [int] IDENTITY(1,1) NOT NULL,
	[MONTO_MENSUAL] [decimal](10, 2) NOT NULL,
	[FECHA] [datetime] NOT NULL,
	[FECHA_BAJA] [datetime] NOT NULL,
	[ID_USUARIO] [int] NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
	[ID_ANIMAL] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_APADRINAMIENTO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CANTONES_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CANTONES_TB](
	[ID_CANTON] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NULL,
	[PROVINCIA] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_CANTON] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CONSULTAS]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONSULTAS](
	[ID_CONSULTA] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NOT NULL,
	[APELLIDO] [varchar](50) NOT NULL,
	[EMAIL] [varchar](50) NULL,
	[MENSAJE] [varchar](500) NULL,
	[ID_ESTADO] [int] NOT NULL,
	[ID_USUARIO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_CONSULTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DIRECCIONES_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DIRECCIONES_TB](
	[ID_DIRECCION] [int] IDENTITY(1,1) NOT NULL,
	[ID_DISTRITO] [int] NOT NULL,
	[DIRECCION_EXACTA] [varchar](255) NULL,
	[ID_USUARIO] [int] NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_DIRECCION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DISTRITOS_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DISTRITOS_TB](
	[ID_DISTRITO] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NULL,
	[CANTON] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_DISTRITO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DONACIONES_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DONACIONES_TB](
	[ID_DONACION] [int] IDENTITY(1,1) NOT NULL,
	[MONTO] [decimal](10, 2) NULL,
	[FECHA] [datetime] NULL,
	[ID_USUARIO] [int] NOT NULL,
	[ID_METODO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_DONACION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ESPECIES_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ESPECIES_TB](
	[ID_ESPECIE] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_ESPECIE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ESTADOS_SALUD_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ESTADOS_SALUD_TB](
	[ID_SALUD] [int] IDENTITY(1,1) NOT NULL,
	[DESCRIPCION] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_SALUD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ESTADOS_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ESTADOS_TB](
	[ID_ESTADO] [int] IDENTITY(1,1) NOT NULL,
	[DESCRIPCION] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_ESTADO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[METODO_PAGO_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[METODO_PAGO_TB](
	[ID_METODO] [int] IDENTITY(1,1) NOT NULL,
	[METODO] [varchar](30) NULL,
	[ID_ESTADO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_METODO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PROVINCIAS_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PROVINCIAS_TB](
	[ID_PROVINCIA] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_PROVINCIA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RAZAS_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RAZAS_TB](
	[ID_RAZA] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NULL,
	[ID_ESPECIE] [int] NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_RAZA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ROLES_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES_TB](
	[ID_ROL] [int] IDENTITY(1,1) NOT NULL,
	[ROL] [varchar](15) NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_ROL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USUARIO_ACTIVIDAD_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO_ACTIVIDAD_TB](
	[ID_USUARIO_ACTIVIDAD] [int] IDENTITY(1,1) NOT NULL,
	[TICKETS_ADQUIRIDOS] [int] NULL,
	[FECHA] [datetime] NULL,
	[TOTAL] [decimal](10, 2) NULL,
	[ID_USUARIO] [int] NOT NULL,
	[ID_ACTIVIDAD] [int] NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
	[ID_METODO_PAGO] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_USUARIO_ACTIVIDAD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USUARIOS_TB]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOS_TB](
	[ID_USUARIO] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](50) NOT NULL,
	[APELLIDO1] [varchar](50) NOT NULL,
	[APELLIDO2] [varchar](50) NOT NULL,
	[CORREO] [varchar](50) NOT NULL,
	[ID_ESTADO] [int] NOT NULL,
	[PASSWORD] [varchar](255) NULL,
	[ID_ROL] [int] NOT NULL,
	[Identificacion] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_USUARIO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CORREO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ACTIVIDADES_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[ANIMAL_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[ANIMAL_TB]  WITH CHECK ADD FOREIGN KEY([ID_RAZA])
REFERENCES [dbo].[RAZAS_TB] ([ID_RAZA])
GO
ALTER TABLE [dbo].[ANIMAL_TB]  WITH CHECK ADD FOREIGN KEY([ID_SALUD])
REFERENCES [dbo].[ESTADOS_SALUD_TB] ([ID_SALUD])
GO
ALTER TABLE [dbo].[APADRINAMIENTOS_TB]  WITH CHECK ADD FOREIGN KEY([ID_ANIMAL])
REFERENCES [dbo].[ANIMAL_TB] ([ID_ANIMAL])
GO
ALTER TABLE [dbo].[APADRINAMIENTOS_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[APADRINAMIENTOS_TB]  WITH CHECK ADD FOREIGN KEY([ID_USUARIO])
REFERENCES [dbo].[USUARIOS_TB] ([ID_USUARIO])
GO
ALTER TABLE [dbo].[CANTONES_TB]  WITH CHECK ADD FOREIGN KEY([PROVINCIA])
REFERENCES [dbo].[PROVINCIAS_TB] ([ID_PROVINCIA])
GO
ALTER TABLE [dbo].[CONSULTAS]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[CONSULTAS]  WITH CHECK ADD FOREIGN KEY([ID_USUARIO])
REFERENCES [dbo].[USUARIOS_TB] ([ID_USUARIO])
GO
ALTER TABLE [dbo].[DIRECCIONES_TB]  WITH CHECK ADD FOREIGN KEY([ID_DISTRITO])
REFERENCES [dbo].[DISTRITOS_TB] ([ID_DISTRITO])
GO
ALTER TABLE [dbo].[DIRECCIONES_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[DIRECCIONES_TB]  WITH CHECK ADD FOREIGN KEY([ID_USUARIO])
REFERENCES [dbo].[USUARIOS_TB] ([ID_USUARIO])
GO
ALTER TABLE [dbo].[DISTRITOS_TB]  WITH CHECK ADD FOREIGN KEY([CANTON])
REFERENCES [dbo].[CANTONES_TB] ([ID_CANTON])
GO
ALTER TABLE [dbo].[DONACIONES_TB]  WITH CHECK ADD FOREIGN KEY([ID_METODO])
REFERENCES [dbo].[METODO_PAGO_TB] ([ID_METODO])
GO
ALTER TABLE [dbo].[DONACIONES_TB]  WITH CHECK ADD FOREIGN KEY([ID_USUARIO])
REFERENCES [dbo].[USUARIOS_TB] ([ID_USUARIO])
GO
ALTER TABLE [dbo].[ESPECIES_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[METODO_PAGO_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[RAZAS_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESPECIE])
REFERENCES [dbo].[ESPECIES_TB] ([ID_ESPECIE])
GO
ALTER TABLE [dbo].[RAZAS_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[ROLES_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[USUARIO_ACTIVIDAD_TB]  WITH CHECK ADD FOREIGN KEY([ID_ACTIVIDAD])
REFERENCES [dbo].[ACTIVIDADES_TB] ([ID_ACTIVIDAD])
GO
ALTER TABLE [dbo].[USUARIO_ACTIVIDAD_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[USUARIO_ACTIVIDAD_TB]  WITH CHECK ADD FOREIGN KEY([ID_METODO_PAGO])
REFERENCES [dbo].[METODO_PAGO_TB] ([ID_METODO])
GO
ALTER TABLE [dbo].[USUARIO_ACTIVIDAD_TB]  WITH CHECK ADD FOREIGN KEY([ID_USUARIO])
REFERENCES [dbo].[USUARIOS_TB] ([ID_USUARIO])
GO
ALTER TABLE [dbo].[USUARIOS_TB]  WITH CHECK ADD FOREIGN KEY([ID_ESTADO])
REFERENCES [dbo].[ESTADOS_TB] ([ID_ESTADO])
GO
ALTER TABLE [dbo].[USUARIOS_TB]  WITH CHECK ADD FOREIGN KEY([ID_ROL])
REFERENCES [dbo].[ROLES_TB] ([ID_ROL])
GO
/****** Object:  StoredProcedure [dbo].[LOGIN_SP]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LOGIN_SP]
    @CORREO VARCHAR(100),
    @CONTRASENNA VARCHAR(100)
AS
BEGIN

    SELECT 
        ID_USUARIO,
        NOMBRE,
        APELLIDO2,
        APELLIDO1,
        IDENTIFICACION,
        CORREO,
        ID_ROL, 
        ID_ESTADO
    FROM dbo.USUARIOS_TB
    WHERE CORREO = @CORREO
    AND PASSWORD = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @CONTRASENNA), 2);
END;
GO
/****** Object:  StoredProcedure [dbo].[REGISTRO_SP]    Script Date: 7/6/2025 6:15:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[REGISTRO_SP] 
    @NOMBRE VARCHAR(100), 
    @APELLIDO1 VARCHAR(100), 
    @APELLIDO2 VARCHAR(100), 
    @CORREO VARCHAR(100), 
    @CONTRASENNA VARCHAR(100), 
    @IDENTIFICACION VARCHAR(20)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM USUARIOS_TB WHERE CORREO = @CORREO)
    BEGIN
        RAISERROR('El correo ya está registrado.', 16, 1);
        RETURN;
    END

    -- Insertar el usuario con contraseña encriptada (hash)
    INSERT INTO dbo.USUARIOS_TB (NOMBRE, APELLIDO1, APELLIDO2, CORREO, PASSWORD, IDENTIFICACION, ID_ESTADO, ID_ROL)
    VALUES (
        @NOMBRE, 
        @APELLIDO1, 
        @APELLIDO2, 
        @CORREO, 
        CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @CONTRASENNA), 2), --AQUI ESTAMOS HASHEANDO LA CONTRASENNA
        @IDENTIFICACION, 1,1
    );
END;
GO
USE [master]
GO
ALTER DATABASE [CASA_NATURA] SET  READ_WRITE 
GO
