-- Eliminar base de datos si existe (CUIDADO: esto borra todo)
DROP DATABASE IF EXISTS iespro_taller;

-- Crear base de datos
CREATE DATABASE iespro_taller CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Usar la base de datos
USE iespro_taller;


SHOW TABLES;


-- Sin Endpoints

CREATE TABLE Modelos(    
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL
);

CREATE TABLE TiposCombustible (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Status BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL
);

CREATE TABLE Empresas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(250) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Status BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL
);

CREATE TABLE Puestos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(250) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    IdEmpresa INT NULL,
    Status BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id),
    INDEX idx_puestos_empresa (IdEmpresa),
    UNIQUE KEY uk_puesto_empresa (Nombre, IdEmpresa)
);

CREATE TABLE Roles (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    IdEmpresa INT NULL,
    Status BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id),
    INDEX idx_roles_empresa (IdEmpresa),
    UNIQUE KEY uk_rol_empresa (Nombre, IdEmpresa)
);





-- Insertar en Modelos
INSERT INTO Modelos (Nombre, Descripcion, IdUser) VALUES
('Corolla', 'Sedán compacto de Toyota', NULL),
('Civic', 'Sedán compacto de Honda', NULL),
('Mustang', 'Deportivo de Ford', NULL);

-- Insertar en TiposCombustible
INSERT INTO TiposCombustible (Nombre, Descripcion, IdUser) VALUES
('Gasolina', 'Combustible común para autos de motor a explosión', NULL),
('Diésel', 'Combustible usado comúnmente en camiones y buses', NULL),
('Eléctrico', 'Fuente de energía para autos eléctricos', NULL),
('Híbrido', 'Combina gasolina y electricidad', NULL);

-- Insertar en Empresas
INSERT INTO Empresas (Nombre, Descripcion, IdUser) VALUES
('AutoPlus S.A.', 'Empresa de venta de autos nuevos y usados', NULL),
('Motores del Norte', 'Concesionaria de autos pesados', NULL);

-- Insertar en Puestos
INSERT INTO Puestos (Nombre, Descripcion, IdEmpresa, IdUser) VALUES
('Gerente General', 'Responsable de la administración general', 1, NULL),
('Asesor de Ventas', 'Encargado de la atención al cliente y ventas', 1, NULL),
('Mecánico', 'Técnico de reparación y mantenimiento', 2, NULL),
('Recepcionista', 'Atención inicial a clientes y llamadas', 2, NULL);

-- Insertar en Roles
INSERT INTO Roles (Nombre, Descripcion, IdEmpresa, IdUser) VALUES
('Administrador', 'Acceso completo al sistema', 1, NULL),
('Vendedor', 'Acceso a módulos de ventas y clientes', 1, NULL),
('Supervisor', 'Acceso a reportes y estadísticas', 2, NULL),
('Técnico', 'Acceso a módulos de servicio y mantenimiento', 2, NULL);






-- Con Endpoints

CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(250) NOT NULL,
    Apellido VARCHAR(250),
    Telefono VARCHAR(13),
    Correo VARCHAR(250) NOT NULL,
    Contrasena VARCHAR(250) NOT NULL,
    IdPuesto INT NULL,
    IdRol INT NULL,
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6),
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,

    -- Foreign keys
    FOREIGN KEY (IdPuesto) REFERENCES Puestos(Id),
    FOREIGN KEY (IdRol) REFERENCES Roles(Id),
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id),
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id),

    -- Índices adicionales
    INDEX idx_usuarios_empresa (IdEmpresa),
    INDEX idx_usuarios_rol (IdRol),
    INDEX idx_usuarios_correo (Correo),

    -- Unique por empresa
    UNIQUE KEY uk_correo_empresa (Correo, IdEmpresa)
);





-- En Espera 

CREATE TABLE CategoriasPermiso (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdCategoriaPadre INT NULL,
    Clave VARCHAR(50) NOT NULL UNIQUE,
    Etiqueta VARCHAR(100) NOT NULL,
    Orden INT DEFAULT 0,
    Icono VARCHAR(50),
    Url VARCHAR(255),
    NombrePantalla VARCHAR(100),
    Status BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6),
    FOREIGN KEY (IdCategoriaPadre) REFERENCES CategoriasPermiso(Id),
    INDEX idx_categoria_padre (IdCategoriaPadre),
    INDEX idx_orden (Orden)
);

CREATE TABLE Usuario_Permiso (
    IdUsuario INT NOT NULL,
    IdCategoriaPermiso INT NOT NULL,
    TieneAcceso BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME DEFAULT CURRENT_TIMESTAMP,
    DateUpdate DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    IdUserUpdate INT NULL,
    PRIMARY KEY (IdUsuario, IdCategoriaPermiso),
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id) ON DELETE CASCADE,
    FOREIGN KEY (IdCategoriaPermiso) REFERENCES CategoriasPermiso(Id) ON DELETE CASCADE,
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);

CREATE TABLE Rol_Permiso (
    IdRol INT NOT NULL,
    IdCategoriaPermiso INT NOT NULL,
    TieneAcceso BOOLEAN NOT NULL DEFAULT TRUE,
    DateCreate DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (IdRol, IdCategoriaPermiso),
    FOREIGN KEY (IdRol) REFERENCES Roles(Id) ON DELETE CASCADE,
    FOREIGN KEY (IdCategoriaPermiso) REFERENCES CategoriasPermiso(Id) ON DELETE CASCADE
);

-- Con Endpoints

CREATE TABLE Departamentos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(250) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id),
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);

CREATE TABLE Clientes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NombreComercial VARCHAR(255) NOT NULL,
    RazonSocial VARCHAR(255) NOT NULL,
    RFC VARCHAR(13) NOT NULL,
    Direccion VARCHAR(255),
    Correo VARCHAR(255),
    Telefono VARCHAR(20),
    NombreContacto VARCHAR(255),
    TelefonoContacto VARCHAR(20),
    CorreoContacto VARCHAR(255),
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id) ON DELETE RESTRICT
);

CREATE TABLE TiposUnidad (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Clave VARCHAR(250) NOT NULL,
    Nombre VARCHAR(250),
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id) ON DELETE CASCADE,
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);

CREATE TABLE Sucursales (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Clave VARCHAR(250) NOT NULL,
    Nombre VARCHAR(250),
    EsMatriz BOOLEAN NOT NULL,
    Direccion VARCHAR(255) NOT NULL,
    Telefono VARCHAR(20),
    Correo VARCHAR(255),
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id) ON DELETE CASCADE,
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);

CREATE TABLE Almacenes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Clave VARCHAR(250) NOT NULL,
    Nombre VARCHAR(250),
    Descripcion VARCHAR(255) NOT NULL,
    IdSucursal INT NOT NULL,
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdSucursal) REFERENCES Sucursales(Id),
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id),
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);

CREATE TABLE Ubicaciones (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Clave VARCHAR(250) NOT NULL,
    Zona VARCHAR(250),
    Pasillo VARCHAR(250),
    Nivel VARCHAR(250),
    Subnivel VARCHAR(250),
    Descripcion VARCHAR(250),
    IdSucursal INT NOT NULL,
    IdAlmacen INT NOT NULL,
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdSucursal) REFERENCES Sucursales(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdAlmacen) REFERENCES Almacenes(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id) ON DELETE RESTRICT
);

CREATE TABLE Marcas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Clave VARCHAR(100) NOT NULL,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion VARCHAR(255),
    IdEmpresa INT NULL,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id) ON DELETE RESTRICT,
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id) ON DELETE RESTRICT
);

CREATE TABLE Articulos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion VARCHAR(255),
    IdMarca INT,
    IdModelo INT,
    Material VARCHAR(255) NOT NULL,
    Resistencia VARCHAR(255),
    Duracion DECIMAL(5,2) NOT NULL,
    Compatibilidad LONGTEXT,
    PrecioUnitario DECIMAL(10,2),
    PrecioVentanilla DECIMAL(10,2),
    CodigoBarras VARCHAR(255),
    Serie VARCHAR(255),
    PesoPieza DECIMAL(10,2),
    Lote VARCHAR(255),
    FechaLote DATETIME(6),
    PiezasCaja INT,
    TiempoVida INT,
    Factura VARCHAR(255),
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6), 
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT,
    FOREIGN KEY (IdMarca) REFERENCES Marcas(Id),
    FOREIGN KEY (IdModelo) REFERENCES Modelos(Id),
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);

CREATE TABLE Vehiculos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NumeroEconomico VARCHAR(250) NOT NULL,
    IdMarca INT,
    IdModelo INT,
    Placa VARCHAR(250) NOT NULL,
    IdPropietario INT NOT NULL,
    Serie VARCHAR(250) NOT NULL,
    IdTipoCombustible INT,
    Observaciones VARCHAR(255),
    IdTipoUnidad INT,
    IdEmpresa INT,
    Status BOOLEAN DEFAULT TRUE,
    DateCreate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6),
    IdUser INT NULL,
    DateUpdate DATETIME(6) NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    IdUserUpdate INT NULL,
    FOREIGN KEY (IdMarca) REFERENCES Marcas(Id),
    FOREIGN KEY (IdModelo) REFERENCES Modelos(Id),
    FOREIGN KEY (IdPropietario) REFERENCES Clientes(Id),
    FOREIGN KEY (IdTipoCombustible) REFERENCES TiposCombustible(Id),
    FOREIGN KEY (IdTipoUnidad) REFERENCES TiposUnidad(Id),
    FOREIGN KEY (IdEmpresa) REFERENCES Empresas(Id),
    FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    FOREIGN KEY (IdUserUpdate) REFERENCES Usuarios(Id)
);


-- Notificaciones

CREATE TABLE `NotificacionPlantillas` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Clave` varchar(100) NOT NULL,
  `Modulo` varchar(100) NOT NULL,
  `Asunto` varchar(200) NOT NULL,
  `ContenidoHTML` longtext NOT NULL,
  `CorreoEnvio` varchar(200) DEFAULT NULL,
  `NombreEnvio` varchar(200) DEFAULT NULL,
  `ConCopia` varchar(500) DEFAULT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT '1',
  `DateCreate` datetime(6) DEFAULT NULL,
  `IdUser` int DEFAULT NULL,
  `DateUpdate` datetime(6) DEFAULT NULL,
  `IdUserUpdate` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_NotificacionPlantillas_Clave_Modulo` (`Clave`,`Modulo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Insertar plantilla de bienvenida para usuarios
INSERT INTO `NotificacionPlantillas` (
    `Clave`, 
    `Modulo`, 
    `Asunto`, 
    `ContenidoHTML`, 
    `CorreoEnvio`, 
    `NombreEnvio`, 
    `Activo`, 
    `DateCreate`, 
    `IdUser`
) VALUES (
    'USUARIO_BIENVENIDA',
    'USUARIOS',
    'Bienvenido a IESPRO - Tus credenciales de acceso',
    '<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bienvenido a IESPRO</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f4f4f4;
        }
        .container {
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .header {
            text-align: center;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #007bff;
        }
        .header h1 {
            color: #007bff;
            margin: 0;
            font-size: 28px;
        }
        .welcome-message {
            font-size: 18px;
            margin-bottom: 25px;
            color: #555;
        }
        .credentials-box {
            background-color: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 5px;
            padding: 20px;
            margin: 20px 0;
        }
        .credentials-box h3 {
            color: #007bff;
            margin-top: 0;
            margin-bottom: 15px;
        }
        .credential-item {
            margin-bottom: 10px;
        }
        .credential-label {
            font-weight: bold;
            color: #495057;
        }
        .credential-value {
            color: #6c757d;
            margin-left: 10px;
        }
        .password-warning {
            background-color: #fff3cd;
            border: 1px solid #ffeaa7;
            border-radius: 5px;
            padding: 15px;
            margin: 20px 0;
            color: #856404;
        }
        .footer {
            text-align: center;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #dee2e6;
            color: #6c757d;
            font-size: 14px;
        }
        .btn {
            display: inline-block;
            background-color: #007bff;
            color: white;
            padding: 12px 24px;
            text-decoration: none;
            border-radius: 5px;
            margin: 10px 5px;
        }
        .btn:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>🎉 ¡Bienvenido a IESPRO!</h1>
        </div>
        
        <div class="welcome-message">
            <p>Hola <strong>{nombre} {apellido}</strong>,</p>
            <p>¡Nos complace darte la bienvenida al sistema IESPRO! Tu cuenta ha sido creada exitosamente.</p>
        </div>
        
        <div class="credentials-box">
            <h3>🔐 Tus credenciales de acceso:</h3>
            <div class="credential-item">
                <span class="credential-label">Correo electrónico:</span>
                <span class="credential-value">{correo}</span>
            </div>
            <div class="credential-item">
                <span class="credential-label">Contraseña:</span>
                <span class="credential-value">{contrasena}</span>
            </div>
            <div class="credential-item">
                <span class="credential-label">Puesto:</span>
                <span class="credential-value">{puesto}</span>
            </div>
            <div class="credential-item">
                <span class="credential-label">Rol:</span>
                <span class="credential-value">{rol}</span>
            </div>
            <div class="credential-item">
                <span class="credential-label">Fecha de creación:</span>
                <span class="credential-value">{fechaCreacion}</span>
            </div>
        </div>
        
        <div class="password-warning">
            <strong>⚠️ Importante:</strong> Por seguridad, te recomendamos cambiar tu contraseña después de tu primer inicio de sesión.
        </div>
        
        <div style="text-align: center; margin: 25px 0;">
            <a href="#" class="btn">🚀 Acceder al Sistema</a>
        </div>
        
        <div class="footer">
            <p>Este es un mensaje automático del sistema IESPRO.</p>
            <p>Si tienes alguna pregunta, contacta al administrador del sistema.</p>
            <p><small>© 2025 IESPRO - Sistema de Gestión</small></p>
        </div>
    </div>
</body>
</html>',
    'ricardomendez1919@gmail.com',
    'IESPRO - Sistema de Gestión',
    1,
    NOW(),
    1
); 


SELECT * FROM Modelos ;
SELECT * FROM TiposCombustible;
SELECT *FROM Empresas;
SELECT * FROM Puestos;
SELECT * FROM Roles;
SELECT * FROM Usuarios;
SELECT * FROM Departamentos;
SELECT * FROM Clientes;
SELECT * FROM TiposUnidad;
SELECT * FROM Sucursales;
SELECT * FROM Almacenes;
SELECT * FROM Ubicaciones;
SELECT * FROM Marcas;
SELECT * FROM Articulos;
SELECT * FROM Vehiculos;



-- Primer usuario -- 1234segura
INSERT INTO Usuarios (
    Nombre, Apellido, Telefono, Correo, Contrasena, IdPuesto, IdRol, IdEmpresa
) VALUES (
    'Juan', 'Pérez', '5512345678', 'juan.perez@email.com',
    '$2a$12$aUR4IFCv78tB9UjNaRkLuODP31NFsnDcCNczCgG7Hv2H4s9FVHHnu', 1, 1, 1
);

