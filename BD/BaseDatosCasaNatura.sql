CREATE DATABASE CASA_NATURA;
GO

USE CASA_NATURA;
GO

-- TABLAS PRINCIPALES

CREATE TABLE ESTADOS_TB (
    ID_ESTADO INT IDENTITY(1,1) PRIMARY KEY,
    DESCRIPCION VARCHAR(30) NOT NULL
);

CREATE TABLE PROVINCIAS_TB (
    ID_PROVINCIA INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50)
);

CREATE TABLE CANTONES_TB (
    ID_CANTON INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50),
    PROVINCIA INT NOT NULL,
    FOREIGN KEY (PROVINCIA) REFERENCES PROVINCIAS_TB(ID_PROVINCIA)
);

CREATE TABLE DISTRITOS_TB (
    ID_DISTRITO INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50),
    CANTON INT NOT NULL,
    FOREIGN KEY (CANTON) REFERENCES CANTONES_TB(ID_CANTON)
);

CREATE TABLE ROLES_TB (
    ID_ROL INT IDENTITY(1,1) PRIMARY KEY,
    ROL VARCHAR(15) NOT NULL,
    ID_ESTADO INT NOT NULL,
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO)
);

CREATE TABLE USUARIOS_TB (
    ID_USUARIO INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50) NOT NULL,
    APELLIDO1 VARCHAR(50) NOT NULL,
    APELLIDO2 VARCHAR(50) NOT NULL,
    CORREO VARCHAR(50) NOT NULL UNIQUE,
    ID_ESTADO INT NOT NULL,
    PASSWORD VARCHAR(255),
    ID_ROL INT NOT NULL,
    Identificacion VARCHAR(20) NOT NULL,
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_ROL) REFERENCES ROLES_TB(ID_ROL)
);

CREATE TABLE DIRECCIONES_TB (
    ID_DIRECCION INT IDENTITY(1,1) PRIMARY KEY,
    ID_DISTRITO INT NOT NULL,
    DIRECCION_EXACTA VARCHAR(255),
    ID_USUARIO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    FOREIGN KEY (ID_DISTRITO) REFERENCES DISTRITOS_TB(ID_DISTRITO),
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO)
);

CREATE TABLE CONSULTAS_TB (
    ID_CONSULTA INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50) NOT NULL,
    APELLIDO VARCHAR(50) NOT NULL,
    EMAIL VARCHAR(50),
    MENSAJE VARCHAR(500),
    ID_ESTADO INT NOT NULL,
    ID_USUARIO INT NOT NULL,
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO)
);

CREATE TABLE METODO_PAGO_TB (
    ID_METODO INT IDENTITY(1,1) PRIMARY KEY,
    METODO VARCHAR(30),
    ID_ESTADO INT NOT NULL,
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO)
);

CREATE TABLE DONACIONES_TB (
    ID_DONACION INT IDENTITY(1,1) PRIMARY KEY,
    MONTO DECIMAL(10, 2),
    FECHA DATETIME,
    ID_USUARIO INT NOT NULL,
    ID_METODO INT NOT NULL,
	REFERENCIA VARCHAR(100),
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO),
    FOREIGN KEY (ID_METODO) REFERENCES METODO_PAGO_TB(ID_METODO)
);

CREATE TABLE ESPECIES_TB (
    ID_ESPECIE INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50) NOT NULL,
    ID_ESTADO INT NOT NULL,
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO)
);

CREATE TABLE RAZAS_TB (
    ID_RAZA INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(50),
    ID_ESPECIE INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    FOREIGN KEY (ID_ESPECIE) REFERENCES ESPECIES_TB(ID_ESPECIE),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO)
);

CREATE TABLE ESTADOS_SALUD_TB (
    ID_SALUD INT IDENTITY(1,1) PRIMARY KEY,
    DESCRIPCION VARCHAR(30) NOT NULL
);

CREATE TABLE ANIMAL_TB (
    ID_ANIMAL INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE VARCHAR(30),
    ID_RAZA INT NOT NULL,
    FECHA_INGRESO DATETIME NOT NULL,
    FECHA_BAJA DATETIME,
    FECHA_NACIMIENTO DATETIME NOT NULL,
    ID_ESTADO INT NOT NULL,
    ID_SALUD INT NOT NULL,
    IMAGEN VARCHAR(255),
    HISTORIA VARCHAR(255),
    NECESIDAD VARCHAR(255),
    FOREIGN KEY (ID_RAZA) REFERENCES RAZAS_TB(ID_RAZA),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_SALUD) REFERENCES ESTADOS_SALUD_TB(ID_SALUD)
);

CREATE TABLE APADRINAMIENTOS_TB (
    ID_APADRINAMIENTO INT IDENTITY(1,1) PRIMARY KEY,
    MONTO_MENSUAL DECIMAL(10, 2) NOT NULL,
    FECHA DATETIME NOT NULL,
    FECHA_BAJA DATETIME NULL,
    ID_USUARIO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    ID_ANIMAL INT NOT NULL,
	ID_METODO INT NOT NULL,
	REFERENCIA VARCHAR(100),
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_ANIMAL) REFERENCES ANIMAL_TB(ID_ANIMAL),
    FOREIGN KEY (ID_METODO) REFERENCES METODO_PAGO_TB(ID_METODO)
);

ALTER TABLE ACTIVIDADES_TB
ADD NOMBRE VARCHAR(255);

CREATE TABLE ACTIVIDADES_TB (
    ID_ACTIVIDAD INT IDENTITY(1,1) PRIMARY KEY,
    DESCRIPCION VARCHAR(4000) NOT NULL,
    FECHA DATETIME NOT NULL,
    PRECIO_BOLETO DECIMAL(10,2) NOT NULL,
    TICKETS_DISPONIBLES INT,
    TICKETS_VENDIDOS INT,
    ID_ESTADO INT NOT NULL,
    IMAGEN VARCHAR(255),
    TIPO VARCHAR(255),
    NOMBRE VARCHAR(255),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO)
);
drop table USUARIO_ACTIVIDAD_TB;
CREATE TABLE USUARIO_ACTIVIDAD_TB (
    ID_USUARIO_ACTIVIDAD INT IDENTITY(1,1) PRIMARY KEY,
    TICKETS_ADQUIRIDOS INT,
    FECHA DATETIME,
    TOTAL DECIMAL(10,2),
    REFERENCIA VARCHAR(100),
    ID_USUARIO INT NOT NULL,
    ID_ACTIVIDAD INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    ID_METODO_PAGO INT NOT NULL,
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO),
    FOREIGN KEY (ID_ACTIVIDAD) REFERENCES ACTIVIDADES_TB(ID_ACTIVIDAD),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_METODO_PAGO) REFERENCES METODO_PAGO_TB(ID_METODO)
);

CREATE TABLE dbo.CONSULTAS_TB (
    ID_CONSULTA     INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE          VARCHAR(100) NOT NULL,
    APELLIDO        VARCHAR(100) NOT NULL,
    CORREO          VARCHAR(100) NOT NULL,
    MENSAJE         VARCHAR(MAX) NOT NULL,
    FECHA           DATETIME NOT NULL 
        CONSTRAINT DF_CONSULTAS_TB_FECHA DEFAULT (GETDATE()),
    FECHA_RESUELTA  DATETIME NULL,

    -- Asociar la consulta al usuario logueado
    ID_USUARIO      INT NULL,
    CONSTRAINT FK_CONSULTAS_TB_USUARIO
        FOREIGN KEY (ID_USUARIO) REFERENCES dbo.USUARIOS_TB(ID_USUARIO),

    -- Estado como FK a ESTADOS_TB (1=Activo, 2=Inactivo, 3=Apadrinado, 4=Resuelto, 5=Pendiente).
    ID_ESTADO       INT NOT NULL 
        CONSTRAINT DF_CONSULTAS_TB_ID_ESTADO DEFAULT (5),
    CONSTRAINT FK_CONSULTAS_TB_ESTADO
        FOREIGN KEY (ID_ESTADO) REFERENCES dbo.ESTADOS_TB(ID_ESTADO)
);


-- =============================================
-- SP: InsertarDonacionSP
-- =============================================
CREATE OR ALTER PROCEDURE InsertarDonacionSP
    @Monto DECIMAL(10, 2),
    @IdUsuario INT,
    @IdMetodo INT,
    @Referencia VARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO dbo.DONACIONES_TB (MONTO, FECHA, ID_USUARIO, ID_METODO, REFERENCIA)
    VALUES (@Monto, GETDATE(), @IdUsuario, @IdMetodo, @Referencia);
END;
GO

-- =============================================
-- SP: RegistrarUsuarioSP
-- =============================================
CREATE OR ALTER PROCEDURE RegistrarUsuarioSP 
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

    INSERT INTO USUARIOS_TB (
        NOMBRE, APELLIDO1, APELLIDO2, CORREO, PASSWORD, IDENTIFICACION, ID_ESTADO, ID_ROL
    )
    VALUES (
        @NOMBRE, @APELLIDO1, @APELLIDO2, @CORREO, 
        CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @CONTRASENNA), 2), 
        @IDENTIFICACION, 1, 1
    );
END;
GO

-- =============================================
-- SP: LoginUsuarioSP
-- =============================================
CREATE OR ALTER PROCEDURE LoginSP
    @CORREO VARCHAR(100),
    @CONTRASENNA VARCHAR(100)
AS
BEGIN
    SELECT 
        ID_USUARIO,
        NOMBRE,
        APELLIDO1,
        APELLIDO2,
        IDENTIFICACION,
        CORREO,
        ID_ROL, 
        ID_ESTADO
    FROM dbo.USUARIOS_TB
    WHERE CORREO = @CORREO
      AND PASSWORD = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @CONTRASENNA), 2);
END;
GO

-- =============================================
-- SP: VisualizarActividadesSP
-- =============================================
CREATE OR ALTER PROCEDURE VisualizarActividadesSP
AS
BEGIN
    SELECT ID_ACTIVIDAD,
           DESCRIPCION,
           FECHA,
           PRECIO_BOLETO,
           TICKETS_DISPONIBLES,
           TICKETS_VENDIDOS,
           ID_ESTADO,
           IMAGEN,
           TIPO,
           NOMBRE
    FROM ACTIVIDADES_TB 
    WHERE ID_ESTADO = 1;
END;
GO

-- =============================================
-- SP: AgregarActividadSP
-- =============================================
CREATE OR ALTER PROCEDURE AgregarActividadSP (
    @Descripcion VARCHAR(4000),
    @Fecha DATETIME,
    @PrecioBoleto DECIMAL(10,2),
    @TicketsDisponibles INT,
    @Imagen VARCHAR(255),
    @Tipo VARCHAR(255),
    @Nombre VARCHAR(255)
) 
AS
BEGIN
    INSERT INTO ACTIVIDADES_TB (
        DESCRIPCION, FECHA, PRECIO_BOLETO, TICKETS_DISPONIBLES, 
        TICKETS_VENDIDOS, ID_ESTADO, IMAGEN, TIPO, NOMBRE
    )
    VALUES (
        @Descripcion, @Fecha, @PrecioBoleto, @TicketsDisponibles, 
        0, 1, @Imagen, @Tipo, @Nombre
    );
END;
GO

-- =============================================
-- SP: ObtenerAnimalesActivosSP
-- =============================================
CREATE OR ALTER PROCEDURE ObtenerAnimalesActivosSP
AS
BEGIN
    SELECT 
        ID_ANIMAL,
        NOMBRE,
        ID_RAZA,
        FECHA_INGRESO,
        FECHA_BAJA,
        FECHA_NACIMIENTO,
        ID_ESTADO,
        ID_SALUD,
        IMAGEN,
        HISTORIA,
        NECESIDAD
    FROM dbo.ANIMAL_TB
    WHERE ID_ESTADO = 1 
      AND FECHA_BAJA IS NULL
    ORDER BY FECHA_INGRESO DESC;
END;
GO

-- =============================================
-- SP: EditarAnimalSP
-- =============================================
CREATE OR ALTER PROCEDURE EditarAnimalSP
    @IdAnimal INT,
    @Nombre VARCHAR(100),
    @IdRaza INT,
    @FechaNacimiento DATETIME,
    @FechaIngreso DATETIME,
    @Historia VARCHAR(MAX),
    @Necesidad VARCHAR(MAX),
    @Imagen VARCHAR(MAX) = NULL
AS
BEGIN
    UPDATE dbo.ANIMAL_TB
    SET
        NOMBRE = @Nombre,
        ID_RAZA = @IdRaza,
        FECHA_NACIMIENTO = @FechaNacimiento,
        FECHA_INGRESO = @FechaIngreso,
        HISTORIA = @Historia,
        NECESIDAD = @Necesidad,
        IMAGEN = CASE 
            WHEN @Imagen IS NOT NULL AND @Imagen != '' THEN @Imagen 
            ELSE IMAGEN 
        END
    WHERE ID_ANIMAL = @IdAnimal;
END;
GO

-- =============================================
-- SP: VisualizarAnimalesSP
-- =============================================

CREATE OR ALTER PROCEDURE VisualizarAnimalesSP
AS
BEGIN
    SELECT 
        a.ID_ANIMAL,
        a.NOMBRE AS NOMBRE_ANIMAL,
        a.ID_RAZA,
        r.NOMBRE AS NOMBRE_RAZA,
        a.FECHA_INGRESO,
        a.FECHA_NACIMIENTO,
        a.ID_ESTADO,
		e.DESCRIPCION AS ESTADO,
        a.ID_SALUD,
        s.DESCRIPCION,
        a.IMAGEN,
        a.HISTORIA,
        a.NECESIDAD
    FROM ANIMAL_TB a
    INNER JOIN RAZAS_TB r ON a.ID_RAZA = r.ID_RAZA
	INNER JOIN ESTADOS_TB e ON a.ID_ESTADO = e.ID_ESTADO
    INNER JOIN ESTADOS_SALUD_TB s ON a.ID_SALUD = s.ID_SALUD
END;
GO

-- =============================================
-- SP: VisualizarApadrinamientosSP
-- =============================================
CREATE OR ALTER PROCEDURE VisualizarApadrinamientosSP
AS
BEGIN
    SELECT 
        A.ID_APADRINAMIENTO,
        A.MONTO_MENSUAL,
        A.FECHA,
        A.FECHA_BAJA,
        A.ID_USUARIO,
        U.NOMBRE AS NOMBRE_USUARIO,
        U.APELLIDO1 AS APELLIDO1_USUARIO,
        U.APELLIDO2 AS APELLIDO2_USUARIO,
        A.ID_ESTADO,
        E.DESCRIPCION AS ESTADO,
        A.ID_ANIMAL,
        AN.NOMBRE AS NOMBRE_ANIMAL,
        A.ID_METODO,
        M.METODO AS METODO_PAGO,
        A.REFERENCIA
    FROM APADRINAMIENTOS_TB A
    INNER JOIN USUARIOS_TB U ON A.ID_USUARIO = U.ID_USUARIO
    INNER JOIN ESTADOS_TB E ON A.ID_ESTADO = E.ID_ESTADO
    INNER JOIN ANIMAL_TB AN ON A.ID_ANIMAL = AN.ID_ANIMAL
    INNER JOIN METODO_PAGO_TB M ON A.ID_METODO = M.ID_METODO
END;
GO

-- =============================================
-- SP: CambiarEstadoAnimalSP
-- =============================================
CREATE OR ALTER PROCEDURE CambiarEstadoAnimalSP
    @IdAnimal INT,
    @IdEstado INT
AS
BEGIN
    DECLARE @ApadrinamientoActivo INT = 0;
    
    IF @IdEstado = 2
    BEGIN
        SELECT @ApadrinamientoActivo = COUNT(*)
        FROM APADRINAMIENTOS_TB 
        WHERE ID_ANIMAL = @IdAnimal AND ID_ESTADO = 1;
    END;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Actualizar estado del animal
        UPDATE ANIMAL_TB
        SET ID_ESTADO = @IdEstado
        WHERE ID_ANIMAL = @IdAnimal;
        
        IF @IdEstado = 2 AND @ApadrinamientoActivo > 0
        BEGIN
            UPDATE APADRINAMIENTOS_TB
            SET ID_ESTADO = 2 
            WHERE ID_ANIMAL = @IdAnimal AND ID_ESTADO = 1;
        END;
        
        COMMIT TRANSACTION;
              
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;


-- =============================================
-- SP: CambiarEstadoApadrinamientoSP
-- =============================================
CREATE OR ALTER PROCEDURE CambiarEstadoApadrinamientoSP
    @IdApadrinamiento INT
AS
BEGIN
    DECLARE @IdAnimal INT;
    DECLARE @EstadoActual INT;
    DECLARE @NuevoEstadoApadrinamiento INT;
    DECLARE @NuevoEstadoAnimal INT;
    
    SELECT 
        @IdAnimal = ID_ANIMAL,
        @EstadoActual = ID_ESTADO 
    FROM APADRINAMIENTOS_TB
    WHERE ID_APADRINAMIENTO = @IdApadrinamiento;
    
    IF @IdAnimal IS NULL
    BEGIN
        RAISERROR('No se encontró el apadrinamiento especificado', 16, 1);
        RETURN;
    END;
    
    IF @EstadoActual = 1
    BEGIN
        SET @NuevoEstadoApadrinamiento = 2; 
        SET @NuevoEstadoAnimal = 1;        
    END
    ELSE
    BEGIN
        SET @NuevoEstadoApadrinamiento = 1; 
        SET @NuevoEstadoAnimal = 3;        
    END;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        UPDATE APADRINAMIENTOS_TB
        SET ID_ESTADO = @NuevoEstadoApadrinamiento,
		FECHA_BAJA = GETDATE()
        WHERE ID_APADRINAMIENTO = @IdApadrinamiento;
        
        UPDATE ANIMAL_TB
        SET ID_ESTADO = @NuevoEstadoAnimal
        WHERE ID_ANIMAL = @IdAnimal;
        
        COMMIT TRANSACTION;
              
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;

-- =============================================
-- SP: ObtenerAnimalPorIdSP
-- =============================================
CREATE OR ALTER PROCEDURE ObtenerAnimalPorIdSP
    @ID_ANIMAL INT
AS
BEGIN
    SELECT 
        a.ID_ANIMAL,
        a.NOMBRE AS NOMBRE_ANIMAL,
        a.ID_RAZA,
        r.NOMBRE AS NOMBRE_RAZA,
        a.FECHA_INGRESO,
        a.FECHA_BAJA,
        a.FECHA_NACIMIENTO,
        a.ID_ESTADO,
        a.ID_SALUD,
        s.DESCRIPCION,
        a.IMAGEN,
        a.HISTORIA,
        a.NECESIDAD
    FROM ANIMAL_TB a
    INNER JOIN RAZAS_TB r ON a.ID_RAZA = r.ID_RAZA
    INNER JOIN ESTADOS_SALUD_TB s ON a.ID_SALUD = s.ID_SALUD
    WHERE a.ID_ANIMAL = @ID_ANIMAL;
END
GO

-- =============================================
-- SP: InsertarApadrinamientoSP
-- =============================================
CREATE OR ALTER PROCEDURE InsertarApadrinamientoSP
    @MontoMensual DECIMAL(10,2),
    @IdUsuario INT,
    @IdMetodo INT,
    @Referencia NVARCHAR(100),
    @IdAnimal INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION;

        INSERT INTO dbo.APADRINAMIENTOS_TB 
        (
            MONTO_MENSUAL, 
            FECHA, 
            FECHA_BAJA, 
            ID_USUARIO, 
            ID_ESTADO, 
            ID_ANIMAL,
            ID_METODO,
            REFERENCIA
        )
        VALUES 
        (
            @MontoMensual, GETDATE(), NULL, @IdUsuario, 1, @IdAnimal, @IdMetodo, ISNULL(@Referencia, 'N/A')
        );

		UPDATE ANIMAL_TB 
        SET ID_ESTADO = 3
        WHERE ID_ANIMAL = @IdAnimal;

        COMMIT TRANSACTION;
        RETURN 0; 

		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			RAISERROR(@ErrorMessage, 16, 1);
			RETURN -99;
		END CATCH;
END;
GO

-- =============================================
-- SP: CambioEstadoActividadSP
-- =============================================

CREATE PROCEDURE CambioEstadoActividadSP
    @IdEstado INT,
    @IdActividad INT
AS
BEGIN
    UPDATE ACTIVIDADES_TB
    SET ID_ESTADO = @IdEstado
    WHERE ID_ACTIVIDAD = @IdActividad;
END;

-- =============================================
-- SP: VisualizarActividadesActivasSP
-- =============================================

CREATE OR ALTER PROCEDURE VisualizarActividadesActivasSP 
AS
BEGIN
    SELECT 
        ID_ACTIVIDAD,
        DESCRIPCION,
        FECHA,
        PRECIO_BOLETO,
        TICKETS_DISPONIBLES,
        TICKETS_VENDIDOS,
        IMAGEN,
        TIPO,
		NOMBRE
    FROM dbo.ACTIVIDADES_TB 
    WHERE ID_ESTADO = 1;
END;
GO

-- =============================================
-- SP: DetalleActividadSP
-- =============================================
CREATE PROCEDURE dbo.DetalleActividadSP
@IdActividad int
AS
BEGIN
    SELECT ID_ACTIVIDAD
      ,DESCRIPCION
      ,FECHA
      ,PRECIO_BOLETO
      ,TICKETS_DISPONIBLES
      ,TICKETS_VENDIDOS
      ,ID_ESTADO
      ,IMAGEN
      ,TIPO
      ,NOMBRE
  FROM dbo.ACTIVIDADES_TB
  WHERE ID_ACTIVIDAD = @IdActividad;
 
END;
GO

-- =============================================
-- SP: EditarActividadSP
-- =============================================

CREATE OR ALTER PROCEDURE EditarActividadSP
    @IdActividad INT,
    @Descripcion VARCHAR(MAX),
    @Fecha DATETIME,
    @PrecioBoleto DECIMAL(10,2),
    @TicketsDisponibles INT,
    @Imagen VARCHAR(MAX),
    @Tipo VARCHAR(50),
    @Nombre VARCHAR(100)
AS
BEGIN
    UPDATE dbo.ACTIVIDADES_TB
    SET
        DESCRIPCION = @Descripcion,
        FECHA = @Fecha,
        PRECIO_BOLETO = @PrecioBoleto,
        TICKETS_DISPONIBLES = @TicketsDisponibles,
        IMAGEN = @Imagen,
        TIPO = @Tipo,
        NOMBRE = @Nombre
    WHERE ID_ACTIVIDAD = @IdActividad;
END;
GO

-- =============================================
-- SP: CambiarContrasennaSP
-- =============================================
CREATE OR ALTER PROCEDURE CambiarContrasennaSP
    @CORREO VARCHAR(100),
    @NUEVA_CONTRASENNA VARCHAR(100)
AS
BEGIN
    -- Verifica si el usuario existe
    IF NOT EXISTS (SELECT 1 FROM USUARIOS_TB WHERE CORREO = @CORREO)
    BEGIN
        RAISERROR('El usuario no existe.', 16, 1);
        RETURN;
    END

    -- Actualiza la contraseña encriptada
    UPDATE USUARIOS_TB
    SET PASSWORD = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @NUEVA_CONTRASENNA), 2)
    WHERE CORREO = @CORREO;
END;
GO


------------------------------------------------------
--SP PARA COMPRAR BOLETOS

USE [CASA_NATURA]
GO

/****** Object:  StoredProcedure [dbo].[CompraActividadSP]    Script Date: 8/18/2025 7:10:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[CompraActividadSP] (
    @IdUsuario INT,
    @IdMetodoPago INT,
    @NumeroBoletos INT,
    @IdActividad INT,
    @Referencia VARCHAR(100)
)
AS
BEGIN
    DECLARE @CostoBoleto DECIMAL(10, 2);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Obtener el precio del boleto
        SELECT @CostoBoleto = PRECIO_BOLETO 
        FROM ACTIVIDADES_TB 
        WHERE ID_ACTIVIDAD = @IdActividad;

     
        INSERT INTO USUARIO_ACTIVIDAD_TB 
        (TICKETS_ADQUIRIDOS,FECHA, TOTAL, ID_USUARIO, ID_ESTADO, ID_METODO_PAGO, ID_ACTIVIDAD, REFERENCIA)
        VALUES 
        (@NumeroBoletos, GETDATE(), @NumeroBoletos * @CostoBoleto, @IdUsuario, 6, @IdMetodoPago, @IdActividad, @Referencia);

      
        UPDATE ACTIVIDADES_TB 
        SET 
            TICKETS_DISPONIBLES = TICKETS_DISPONIBLES - @NumeroBoletos,
            TICKETS_VENDIDOS = TICKETS_VENDIDOS + @NumeroBoletos
        WHERE ID_ACTIVIDAD = @IdActividad;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
-- =============================================
-- SP: ObtenerMisAnimales
-- =============================================
CREATE PROCEDURE [dbo].[ObtenerMisAnimalesSP]
    @ID_USUARIO INT
AS
BEGIN
    SELECT 
        a.ID_ANIMAL,
        a.NOMBRE,
        a.ID_RAZA,
        a.FECHA_INGRESO,
        a.FECHA_BAJA,
        a.FECHA_NACIMIENTO,
        a.ID_ESTADO,
        a.ID_SALUD,
        a.IMAGEN,
        a.HISTORIA,
        a.NECESIDAD
    FROM dbo.ANIMAL_TB a
    INNER JOIN dbo.APADRINAMIENTOS_TB ap
        ON a.ID_ANIMAL = ap.ID_ANIMAL
    WHERE ap.ID_USUARIO = @ID_USUARIO
      AND a.ID_ESTADO = 1 
      AND a.FECHA_BAJA IS NULL
    ORDER BY a.FECHA_INGRESO DESC;
END;
GO

-- =============================================
-- INSERTS
-- =============================================

-- =============================================
-- INSERTS PARA ESTADOS_TB
-- =============================================
INSERT INTO ESTADOS_TB (DESCRIPCION) VALUES
('Activo'),
('Inactivo'),
('Apadrinado');
('Resuelto');
('Pendiente');
('Confirmada');

-- =============================================
-- INSERTS PARA ESTADOS_SALUD_TB
-- =============================================
INSERT INTO ESTADOS_SALUD_TB (DESCRIPCION) VALUES
('Excelente'),
('Buena'),
('Regular'),
('Delicada'),
('Crítica');

-- =============================================
-- INSERTS PARA ESPECIES_TB
-- =============================================
INSERT INTO ESPECIES_TB (NOMBRE, ID_ESTADO) VALUES
('Perro', 1),
('Gato', 1),
('Conejo', 1),
('Ave', 1);

-- =============================================
-- INSERTS PARA RAZAS_TB
-- =============================================
INSERT INTO RAZAS_TB (NOMBRE, ID_ESPECIE, ID_ESTADO) VALUES
('Labrador Retriever', 1, 1),
('Pitbull', 1, 1),
('Persa', 2, 1),
('Siames', 2, 1),
('Enano Holandés', 3, 1),
('Canario', 4, 1); 

-- =============================================
-- INSERTS PARA ANIMAL_TB
-- =============================================
INSERT INTO ANIMAL_TB (
    NOMBRE, ID_RAZA, FECHA_INGRESO, FECHA_BAJA,
    FECHA_NACIMIENTO, ID_ESTADO, ID_SALUD,
    IMAGEN, HISTORIA, NECESIDAD
) VALUES
('Max', 1, '20230110', NULL, '20220615', 1, 1, '/Imagenes/1.jpg', 'Rescatado de la calle en malas condiciones.', 'Requiere medicamentos mensuales'),
('Luna', 3, '20230512', NULL, '20211120', 1, 2, '/Imagenes/2.jpg', 'Abandonada por su familia anterior.', 'Necesita una dieta especial'),
('Rocky', 2, '20220901', NULL, '20210101', 1, 3, '/Imagenes/3.jpg', 'Encontrado en zona rural, muy delgado.', 'Tratamiento para piel'),
('Milo', 4, '20230318', NULL, '20220228', 2, 4, '/Imagenes/4.jpg', 'Convaleciente por accidente.', 'Atención médica semanal'),
('Coco', 5, '20230705', NULL, '20230115', 1, 2, '/Imagenes/5.jpg', 'Nacimiento en refugio.', 'Vacunas pendientes');


-- =============================================
-- INSERTS PARA ROLES_TB
-- =============================================
INSERT INTO ROLES_TB (ROL, ID_ESTADO) VALUES
('Cliente', 1),
('Administrador', 1);

-- =============================================
-- INSERTS PARA METODO_PAGO_TB
-- =============================================
INSERT INTO METODO_PAGO_TB (METODO, ID_ESTADO) VALUES
('Tarjeta crédito/débito', 1),
('Sinpe móvil', 1),
('PayPal', 1);


--- =======================================
--- SP VENTAS
---=======================================
USE [CASA_NATURA]
GO

/****** Object:  StoredProcedure [dbo].[VisualizacionVentasSP]    Script Date: 8/16/2025 8:58:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[VisualizacionVentasSP]
AS
BEGIN
    SELECT 
        UA.ID_USUARIO_ACTIVIDAD AS NUMERO_FACTURA,
        UA.TICKETS_ADQUIRIDOS, 
        UA.FECHA, 
        UA.TOTAL, 
        U.NOMBRE + ' ' + U.APELLIDO1 + ' ' + U.APELLIDO2 AS NOMBRE_COMPLETO,
        A.NOMBRE AS NOMBRE_ACTIVIDAD,
        A.FECHA AS FECHA_ACTIVIDAD,
        E.DESCRIPCION AS ESTADO_COMPRA,
        M.METODO
    FROM USUARIO_ACTIVIDAD_TB UA
    INNER JOIN ACTIVIDADES_TB A ON A.ID_ACTIVIDAD = UA.ID_ACTIVIDAD
    INNER JOIN ESTADOS_TB E ON E.ID_ESTADO = UA.ID_ESTADO
    INNER JOIN METODO_PAGO_TB M ON M.ID_METODO = UA.ID_METODO_PAGO
    INNER JOIN USUARIOS_TB U ON U.ID_USUARIO = UA.ID_USUARIO;
END
GO


---===================================================

CREATE OR ALTER PROCEDURE ObtenerUsuariosSP
AS
BEGIN
	SELECT U.ID_USUARIO, U.NOMBRE, U.APELLIDO1, U.APELLIDO2, U.ID_ROL, U.CORREO,
	U.ID_ESTADO, U.IDENTIFICACION, R.ROL, E.DESCRIPCION AS ESTADO
	FROM USUARIOS_TB U
	INNER JOIN ROLES_TB R ON U.ID_ROL = R.ID_ROL
	INNER JOIN ESTADOS_TB E ON U.ID_ESTADO = E.ID_ESTADO;

END;