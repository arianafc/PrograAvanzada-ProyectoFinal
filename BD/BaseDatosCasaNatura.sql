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

CREATE TABLE CONSULTAS (
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
    FECHA_BAJA DATETIME NOT NULL,
    ID_USUARIO INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    ID_ANIMAL INT NOT NULL,
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_ANIMAL) REFERENCES ANIMAL_TB(ID_ANIMAL)
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

CREATE TABLE USUARIO_ACTIVIDAD_TB (
    ID_USUARIO_ACTIVIDAD INT IDENTITY(1,1) PRIMARY KEY,
    TICKETS_ADQUIRIDOS INT,
    FECHA DATETIME,
    TOTAL DECIMAL(10,2),
    ID_USUARIO INT NOT NULL,
    ID_ACTIVIDAD INT NOT NULL,
    ID_ESTADO INT NOT NULL,
    ID_METODO_PAGO INT NOT NULL,
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS_TB(ID_USUARIO),
    FOREIGN KEY (ID_ACTIVIDAD) REFERENCES ACTIVIDADES_TB(ID_ACTIVIDAD),
    FOREIGN KEY (ID_ESTADO) REFERENCES ESTADOS_TB(ID_ESTADO),
    FOREIGN KEY (ID_METODO_PAGO) REFERENCES METODO_PAGO_TB(ID_METODO)
);

CREATE TABLE CONSULTAS_TB (
    ID_CONSULTA INT PRIMARY KEY IDENTITY(1,1),
    NOMBRE VARCHAR(100) NOT NULL,
    APELLIDO VARCHAR(100) NOT NULL,
    CORREO VARCHAR(100) NOT NULL,
    MENSAJE TEXT NOT NULL,
    FECHA DATETIME NOT NULL DEFAULT GETDATE(),
    ESTADO VARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    FECHA_RESUELTA DATETIME NULL
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
-- SP: RecuperarAccesoSP
-- =============================================
CREATE OR ALTER PROCEDURE RecuperarAccesoSP 
    @CONTRASENNA VARCHAR(100), 
    @CORREO VARCHAR(255)
AS
BEGIN
    UPDATE USUARIOS_TB 
    SET PASSWORD = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @CONTRASENNA), 2)
    WHERE CORREO = @CORREO;
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
CREATE OR ALTER PROCEDURE LoginUsuarioSP
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
-- SP: ObtenerAnimalPorIdSP
-- =============================================
CREATE OR ALTER PROCEDURE ObtenerAnimalPorIdSP
    @ID_ANIMAL INT
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
    WHERE ID_ANIMAL = @ID_ANIMAL;
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



-- =============================================
-- INSERTS
-- =============================================

-- =============================================
-- INSERTS PARA ESTADOS_TB
-- =============================================
INSERT INTO ESTADOS_TB (DESCRIPCION) VALUES
('Activo'),
('Inactivo');

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
('Max', 1, '20230110', NULL, '20220615', 1, 1, 'imagenes/Perro-Labrador Retriever.jpg', 'Rescatado de la calle en malas condiciones.', 'Requiere medicamentos mensuales'),
('Luna', 3, '20230512', NULL, '20211120', 1, 2, 'imagenes/Gato-Persa.jpg', 'Abandonada por su familia anterior.', 'Necesita una dieta especial'),
('Rocky', 2, '20220901', NULL, '20210101', 1, 3, 'imagenes/Perro-Pitbull.jpg', 'Encontrado en zona rural, muy delgado.', 'Tratamiento para piel'),
('Milo', 4, '20230318', NULL, '20220228', 2, 4, 'imagenes/Gato-Siames.jpg', 'Convaleciente por accidente.', 'Atención médica semanal'),
('Coco', 5, '20230705', NULL, '20230115', 1, 2, 'imagenes/Conejo-Enano Holandes.jpg', 'Nacimiento en refugio.', 'Vacunas pendientes');


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
