USE CASA_NATURA;
GO
--PROVINCIAS--

INSERT INTO PROVINCIAS_TB (NOMBRE) VALUES
('San José'),
('Alajuela'),
('Cartago'),
('Heredia'),
('Guanacaste'),
('Puntarenas'),
('Limón');

--CANTONES--

-- San José
INSERT INTO CANTONES_TB (NOMBRE, PROVINCIA) VALUES
('San José', 1),
('Escazú', 1),
('Desamparados', 1),

-- Alajuela
('Alajuela', 2),
('San Ramón', 2),
('Grecia', 2),

-- Cartago
('Cartago', 3),
('Paraíso', 3),
('La Unión', 3),

-- Heredia
('Heredia', 4),
('Barva', 4),
('Santo Domingo', 4),

-- Guanacaste
('Liberia', 5),
('Nicoya', 5),
('Santa Cruz', 5),

-- Puntarenas
('Puntarenas', 6),
('Esparza', 6),
('Buenos Aires', 6),

-- Limón
('Limón', 7),
('Pococí', 7),
('Siquirres', 7);


--DISTRITOS--

-- San José
INSERT INTO DISTRITOS_TB (NOMBRE, CANTON) VALUES
('Carmen', 1),
('Merced', 1),
('Hospital', 1),

-- Escazú
('Escazú Centro', 2),
('San Rafael', 2),
('San Antonio', 2),

-- Desamparados
('Desamparados Centro', 3),
('San Miguel', 3),
('San Juan de Dios', 3),

-- Alajuela
('Alajuela Centro', 4),
('San José', 4),
('Carrizal', 4),

-- San Ramón
('San Ramón Centro', 5),
('Santiago', 5),
('San Juan', 5),

-- Grecia
('Grecia Centro', 6),
('San Isidro', 6),
('San Roque', 6),

-- Cartago
('Oriental', 7),
('Occidental', 7),
('Carmen', 7),

-- Paraíso
('Paraíso Centro', 8),
('Santiago', 8),
('Orosi', 8),

-- La Unión
('Tres Ríos', 9),
('San Diego', 9),
('San Juan', 9),

-- Heredia
('Heredia Centro', 10),
('Mercedes', 10),
('San Francisco', 10),

-- Barva
('Barva Centro', 11),
('San Pedro', 11),
('San Pablo', 11),

-- Santo Domingo
('Santo Domingo Centro', 12),
('San Vicente', 12),
('San Miguel', 12),

-- Liberia
('Liberia Centro', 13),
('Cañas Dulces', 13),
('Mayorga', 13),

-- Nicoya
('Nicoya Centro', 14),
('Mansión', 14),
('San Antonio', 14),

-- Santa Cruz
('Santa Cruz Centro', 15),
('Bolsón', 15),
('Veintisiete de Abril', 15),

-- Puntarenas
('Puntarenas Centro', 16),
('Pitahaya', 16),
('Chomes', 16),

-- Esparza
('Espíritu Santo', 17),
('San Juan Grande', 17),
('Macacona', 17),

-- Buenos Aires
('Buenos Aires Centro', 18),
('Volcán', 18),
('Potrero Grande', 18),

-- Limón
('Limón Centro', 19),
('Valle La Estrella', 19),
('Río Blanco', 19),

-- Pococí
('Guápiles', 20),
('Jiménez', 20),
('Rita', 20),

-- Siquirres
('Siquirres Centro', 21),
('Pacuarito', 21),
('Florida', 21);
