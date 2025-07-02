-- Conéctate a gyg_db antes de ejecutar

-- Tabla de propietarios/usuarios del sistema
CREATE TABLE owner (
                       id SERIAL PRIMARY KEY,
                       username VARCHAR(50) NOT NULL UNIQUE,
                       password VARCHAR(100) NOT NULL,  -- puedes usar SHA256 o texto plano para pruebas
                       nombre_completo VARCHAR(100),
                       tipo_usuario VARCHAR(20) DEFAULT 'admin' -- o 'empleado' futuro
);

-- Insertar usuario admin
INSERT INTO owner (username, password, nombre_completo)
VALUES ('admin', 'admin200', 'Administrador del sistema');

-- Tabla de productos en inventario
CREATE TABLE producto (
                          id SERIAL PRIMARY KEY,
                          nombre VARCHAR(100) NOT NULL,
                          descripcion TEXT,
                          categoria VARCHAR(50),
                          precio_inventario NUMERIC(10, 2),
                          precio_venta NUMERIC(10, 2),
                          stock INT,
                          codigo VARCHAR(50) UNIQUE,
                          fecha_vencimiento DATE,
                          iva NUMERIC(5, 2),
                          descuento NUMERIC(5, 2)
);

-- Tabla de clientes
CREATE TABLE cliente (
                         id SERIAL PRIMARY KEY,
                         nombre VARCHAR(100) NOT NULL,
                         telefono VARCHAR(20),
                         ubicacion TEXT
);

-- Tabla de ventas y detalle
CREATE TABLE factura (
                         id SERIAL PRIMARY KEY,
                         id_cliente INT REFERENCES cliente(id),
                         fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                         estado_pago VARCHAR(20), -- efectivo / credito
                         total NUMERIC(10, 2),
                         descuento NUMERIC(10, 2)
);

CREATE TABLE factura_detalle (
                                 id SERIAL PRIMARY KEY,
                                 id_factura INT REFERENCES factura(id),
                                 id_producto INT REFERENCES producto(id),
                                 cantidad INT,
                                 precio_unitario NUMERIC(10, 2),
                                 subtotal NUMERIC(10, 2)
);

-- Tabla de proformas
CREATE TABLE proforma (
                          id SERIAL PRIMARY KEY,
                          id_cliente INT REFERENCES cliente(id),
                          fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                          total NUMERIC(10, 2),
                          descuento NUMERIC(10, 2)
);

CREATE TABLE proforma_detalle (
                                  id SERIAL PRIMARY KEY,
                                  id_proforma INT REFERENCES proforma(id),
                                  id_producto INT REFERENCES producto(id),
                                  cantidad INT,
                                  precio_unitario NUMERIC(10, 2),
                                  subtotal NUMERIC(10, 2),
                                  iva NUMERIC(5, 2)
);

-- Tabla de proveedores
CREATE TABLE proveedor (
                           id SERIAL PRIMARY KEY,
                           nombre VARCHAR(100),
                           telefono VARCHAR(20),
                           direccion TEXT,
                           ubicacion TEXT
);

-- Tabla de pedidos
CREATE TABLE pedido (
                        id SERIAL PRIMARY KEY,
                        id_proveedor INT REFERENCES proveedor(id),
                        fecha_solicitud TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        fecha_recibido TIMESTAMP,
                        estado VARCHAR(20) -- solicitado, recibido
);

CREATE TABLE pedido_detalle (
                                id SERIAL PRIMARY KEY,
                                id_pedido INT REFERENCES pedido(id),
                                id_producto INT REFERENCES producto(id),
                                cantidad INT,
                                precio_compra NUMERIC(10, 2),
                                precio_venta NUMERIC(10, 2)
);

-- Tabla para almacenar archivos (facturas, proformas, pedidos)
CREATE TABLE archivo_pdf (
                             id SERIAL PRIMARY KEY,
                             nombre_archivo VARCHAR(255),
                             tipo VARCHAR(20), -- factura, proforma, pedido
                             contenido BYTEA,
                             fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
