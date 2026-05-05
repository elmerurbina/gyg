-- =======================
-- 📌 OWNER (usuarios)
-- =======================

-- Insertar
CREATE OR REPLACE FUNCTION sp_insert_owner(
    p_username VARCHAR, p_password VARCHAR, p_nombre_completo VARCHAR
)
RETURNS VOID AS $$
BEGIN
INSERT INTO owner (username, password, nombre_completo)
VALUES (p_username, p_password, p_nombre_completo);
END;
$$ LANGUAGE plpgsql;

-- Leer
CREATE OR REPLACE FUNCTION sp_get_owners()
RETURNS TABLE(id INT, username VARCHAR, nombre_completo VARCHAR) AS $$
BEGIN
RETURN QUERY SELECT id, username, nombre_completo FROM owner;
END;
$$ LANGUAGE plpgsql;

-- Actualizar
CREATE OR REPLACE FUNCTION sp_update_owner(
    p_id INT, p_username VARCHAR, p_password VARCHAR, p_nombre_completo VARCHAR
)
RETURNS VOID AS $$
BEGIN
UPDATE owner SET username = p_username, password = p_password, nombre_completo = p_nombre_completo
WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- Eliminar
CREATE OR REPLACE FUNCTION sp_delete_owner(p_id INT)
RETURNS VOID AS $$
BEGIN
DELETE FROM owner WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- =======================
-- 📌 PRODUCTO (inventario)
-- =======================

ALTER TABLE producto ADD COLUMN precio_final NUMERIC;

SELECT * FROM producto;

ALTER TABLE producto ALTER COLUMN fecha_vencimiento DROP DEFAULT;



-- Insertar
-- Insertar producto con manejo explícito de fecha nula
CREATE OR REPLACE FUNCTION sp_insert_producto(
    p_nombre VARCHAR, p_descripcion TEXT, p_categoria VARCHAR,
    p_precio_inventario NUMERIC, p_precio_venta NUMERIC,
    p_stock INT, p_codigo VARCHAR, p_fecha_vencimiento DATE,
    p_iva NUMERIC, p_descuento NUMERIC, p_precio_final NUMERIC
)
RETURNS VOID AS $$
BEGIN
    -- Insertar usando NULL si p_fecha_vencimiento es NULL
INSERT INTO producto (nombre, descripcion, categoria, precio_inventario, precio_venta, stock, codigo, fecha_vencimiento, iva, descuento, precio_final)
VALUES (
           NULLIF(TRIM(p_nombre), ''),
           NULLIF(TRIM(p_descripcion), ''),
           NULLIF(TRIM(p_categoria), ''),
           p_precio_inventario,
           p_precio_venta,
           p_stock,
           NULLIF(TRIM(p_codigo), ''),
           p_fecha_vencimiento,  -- aquí se acepta NULL tal cual
           p_iva,
           p_descuento,
           p_precio_final
       );
END;
$$ LANGUAGE plpgsql;


-- Leer productos sin cambios
CREATE OR REPLACE FUNCTION sp_get_productos()
RETURNS TABLE(id INT, nombre VARCHAR, descripcion TEXT, categoria VARCHAR, precio_venta NUMERIC, stock INT) AS $$
BEGIN
RETURN QUERY
SELECT id, nombre, descripcion, categoria, precio_venta, stock FROM producto;
END;
$$ LANGUAGE plpgsql;


-- Actualizar producto con manejo explícito de fecha nula
CREATE OR REPLACE FUNCTION sp_update_producto(
    p_id INT, p_nombre VARCHAR, p_descripcion TEXT, p_categoria VARCHAR,
    p_precio_inventario NUMERIC, p_precio_venta NUMERIC,
    p_stock INT, p_codigo VARCHAR, p_fecha_vencimiento DATE,
    p_iva NUMERIC, p_descuento NUMERIC, p_precio_final NUMERIC
)
RETURNS VOID AS $$
BEGIN
UPDATE producto
SET
    nombre = NULLIF(TRIM(p_nombre), ''),
    descripcion = NULLIF(TRIM(p_descripcion), ''),
    categoria = NULLIF(TRIM(p_categoria), ''),
    precio_inventario = p_precio_inventario,
    precio_venta = p_precio_venta,
    stock = p_stock,
    codigo = NULLIF(TRIM(p_codigo), ''),
    fecha_vencimiento = p_fecha_vencimiento,  -- acepta NULL tal cual
    iva = p_iva,
    descuento = p_descuento,
    precio_final = p_precio_final
WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;


-- Eliminar
CREATE OR REPLACE FUNCTION sp_delete_producto(p_id INT)
RETURNS VOID AS $$
BEGIN
DELETE FROM producto WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- =======================
-- 📌 CLIENTE
-- =======================

ALTER TABLE cliente
    ADD COLUMN IF NOT EXISTS numero_compras INTEGER DEFAULT 1;


-- Insertar
CREATE OR REPLACE FUNCTION sp_insert_cliente(
    p_nombre VARCHAR, 
    p_telefono VARCHAR, 
    p_ubicacion TEXT
)
RETURNS VOID AS $$
BEGIN
    -- Verificar si el cliente ya existe por nombre y teléfono
    IF EXISTS (
        SELECT 1 
        FROM cliente 
        WHERE nombre = p_nombre AND telefono = p_telefono
    ) THEN
        -- Cliente existe, actualizar número de compras
UPDATE cliente
SET numero_compras = numero_compras + 1
WHERE nombre = p_nombre AND telefono = p_telefono;

ELSE
        -- Cliente no existe, insertarlo con 1 compra
        INSERT INTO cliente(nombre, telefono, ubicacion, numero_compras)
        VALUES (p_nombre, p_telefono, p_ubicacion, 1);
END IF;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM cliente;

-- Leer
CREATE OR REPLACE FUNCTION sp_get_clientes()
RETURNS TABLE(id INT, nombre VARCHAR, telefono VARCHAR, ubicacion TEXT) AS $$
BEGIN
RETURN QUERY
SELECT
    c.id,
    c.nombre,
    c.telefono,
    c.ubicacion
FROM cliente c;
END;
$$ LANGUAGE plpgsql;






-- =======================
-- 📌 VENTA y lógica de stock
-- =======================

-- Insertar venta con detalle
CREATE OR REPLACE FUNCTION sp_insert_factura(
    p_id_cliente INT,
    p_estado_pago VARCHAR,
    p_total NUMERIC,
    p_descuento NUMERIC,
    p_detalles JSON
)
RETURNS VOID AS $$
DECLARE
nueva_factura_id INT;
    item JSON;
    prod_id INT;
    cantidad INT;
    precio_unit NUMERIC;
    subtotal NUMERIC;
BEGIN
INSERT INTO factura (id_cliente, estado_pago, total, descuento)
VALUES (p_id_cliente, p_estado_pago, p_total, p_descuento)
    RETURNING id INTO nueva_factura_id;

-- Insertar detalle y actualizar stock
FOR item IN SELECT * FROM json_array_elements(p_detalles)
                              LOOP
    prod_id := (item ->> 'id_producto')::INT;
cantidad := (item ->> 'cantidad')::INT;
        precio_unit := (item ->> 'precio_unitario')::NUMERIC;
        subtotal := precio_unit * cantidad;

INSERT INTO factura_detalle (id_factura, id_producto, cantidad, precio_unitario, subtotal)
VALUES (nueva_factura_id, prod_id, cantidad, precio_unit, subtotal);

-- Restar del stock
UPDATE producto SET stock = stock - cantidad WHERE id = prod_id;
END LOOP;
END;
$$ LANGUAGE plpgsql;


DROP FUNCTION sp_insert_proforma(integer,numeric,numeric,json);


CREATE OR REPLACE FUNCTION sp_insert_proforma(
    p_id_cliente INT,
    p_total NUMERIC,
    p_descuento NUMERIC,
    p_detalles JSON
)
RETURNS INT AS $$
DECLARE
nueva_proforma_id INT;
    item JSON;
    prod_id INT;
    cantidad INT;
    precio_unit NUMERIC;
    subtotal NUMERIC;
BEGIN
    -- Insertar la proforma
INSERT INTO proforma (id_cliente, total, descuento)
VALUES (p_id_cliente, p_total, p_descuento)
    RETURNING id INTO nueva_proforma_id;

-- Recorrer cada detalle dentro del JSON
FOR item IN SELECT * FROM json_array_elements(p_detalles)
                              LOOP
    prod_id := (item ->> 'Id')::INT;
cantidad := (item ->> 'Cantidad')::INT;
        precio_unit := (item ->> 'PrecioUnitario')::NUMERIC;
        subtotal := precio_unit * cantidad;

INSERT INTO proforma_detalle (id_proforma, id_producto, cantidad, precio_unitario, subtotal)
VALUES (nueva_proforma_id, prod_id, cantidad, precio_unit, subtotal);
END LOOP;

RETURN nueva_proforma_id;
END;
$$ LANGUAGE plpgsql;


SELECT * FROM archivo_pdf;



SELECT * FROM cliente;

-- =======================
-- 📌 PEDIDOS: sumar productos recibidos
-- =======================

-- Marcar pedido como recibido y actualizar stock
CREATE OR REPLACE FUNCTION sp_recibir_pedido(p_id_pedido INT)
RETURNS VOID AS $$
DECLARE
item RECORD;
BEGIN
    -- Marcar como recibido
UPDATE pedido SET estado = 'recibido', fecha_recibido = CURRENT_TIMESTAMP
WHERE id = p_id_pedido;

-- Sumar al stock
FOR item IN
SELECT id_producto, cantidad FROM pedido_detalle WHERE id_pedido = p_id_pedido
    LOOP
UPDATE producto SET stock = stock + item.cantidad WHERE id = item.id_producto;
END LOOP;
END;
$$ LANGUAGE plpgsql;


CREATE TABLE categoria (
                           id SERIAL PRIMARY KEY,
                           nombre VARCHAR(50) NOT NULL UNIQUE
);


DROP FUNCTION sp_get_productos_en_ventas();

SELECT * FROM producto;

CREATE OR REPLACE FUNCTION sp_get_productos_en_ventas()
RETURNS TABLE (
    id_producto INT,
    nombre TEXT,
    descripcion TEXT,
    categoria TEXT,
    precio_venta NUMERIC,
    stock INT,
    iva NUMERIC,
    descuento NUMERIC
) AS $$
BEGIN
RETURN QUERY
SELECT
    p.id::INT AS id_producto,
        p.nombre::TEXT,
        p.descripcion::TEXT,
        p.categoria::TEXT,
        p.precio_venta::NUMERIC,
        p.stock::INT,
        p.iva::NUMERIC,
        p.descuento::NUMERIC
FROM producto p;
END;
$$ LANGUAGE plpgsql;

SELECT sp_resumen_clientes();

ALTER TABLE factura ADD COLUMN pagado BOOLEAN DEFAULT FALSE;

SELECT * FROM cliente;

DROP FUNCTION sp_resumen_clientes();



CREATE OR REPLACE FUNCTION sp_resumen_clientes()
RETURNS TABLE (
    nombre TEXT,
    telefono TEXT,
    ubicacion TEXT,
    numero_compras BIGINT,       -- Cambiado a bigint
    total_compras NUMERIC,
    contado BIGINT,              -- Cambiado a bigint
    credito BIGINT,              -- Cambiado a bigint
    creditos_activos BIGINT,     -- Cambiado a bigint
    fecha_credito_mas_antiguo TIMESTAMP
)
AS $$
BEGIN
RETURN QUERY
SELECT
    c.nombre::TEXT,
        c.telefono::TEXT,
        c.ubicacion::TEXT,
        COUNT(f.id) AS numero_compras,
    COALESCE(SUM(f.total), 0) AS total_compras,
    COUNT(*) FILTER (WHERE f.estado_pago = 'contado') AS contado,
        COUNT(*) FILTER (WHERE f.estado_pago = 'credito') AS credito,
        COUNT(*) FILTER (WHERE f.estado_pago = 'credito' AND f.pagado = false) AS creditos_activos,
        MIN(f.fecha) FILTER (WHERE f.estado_pago = 'credito' AND f.pagado = false) AS fecha_credito_mas_antiguo
FROM cliente c
         LEFT JOIN factura f ON f.id_cliente = c.id
GROUP BY c.id;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM producto;



-- Primero, eliminar la función existente
DROP FUNCTION IF EXISTS sp_insert_factura(INT, VARCHAR, NUMERIC, NUMERIC, JSON);

-- Crear la función CORREGIDA que SÍ retorna el ID de la factura
CREATE OR REPLACE FUNCTION sp_insert_factura(
    p_id_cliente INT,
    p_estado_pago VARCHAR,
    p_total NUMERIC,
    p_descuento NUMERIC,
    p_detalles JSON
)
RETURNS INT AS $$
DECLARE
    nueva_factura_id INT;
    item JSON;
    prod_id INT;
    cantidad INT;
    precio_unit NUMERIC;
    subtotal NUMERIC;
BEGIN
    -- Insertar la factura y guardar el ID retornado
    INSERT INTO factura (id_cliente, estado_pago, total, descuento)
    VALUES (p_id_cliente, p_estado_pago, p_total, p_descuento)
    RETURNING id INTO nueva_factura_id;

    -- Insertar detalle y actualizar stock
    FOR item IN SELECT * FROM json_array_elements(p_detalles)
    LOOP
        prod_id := (item ->> 'id')::INT;
        cantidad := (item ->> 'cantidad')::INT;
        precio_unit := (item ->> 'precioUnitario')::NUMERIC;
        subtotal := precio_unit * cantidad;

        INSERT INTO factura_detalle (id_factura, id_producto, cantidad, precio_unitario, subtotal)
        VALUES (nueva_factura_id, prod_id, cantidad, precio_unit, subtotal);

        -- Restar del stock
        UPDATE producto SET stock = stock - cantidad WHERE id = prod_id;
    END LOOP;

    -- Retornar el ID de la factura generada
    RETURN nueva_factura_id;
END;
$$ LANGUAGE plpgsql;






-- Primero, eliminar la función existente
DROP FUNCTION IF EXISTS sp_insert_proforma(INT, NUMERIC, NUMERIC, JSON);

-- Crear la función CORREGIDA
CREATE OR REPLACE FUNCTION sp_insert_proforma(
    p_id_cliente INT,
    p_total NUMERIC,
    p_descuento NUMERIC,
    p_detalles JSON
)
RETURNS INT AS $$
DECLARE
    nueva_proforma_id INT;
    item JSON;
    prod_id INT;
    cantidad INT;
    precio_unit NUMERIC;
    subtotal NUMERIC;
BEGIN
    -- Insertar la proforma y guardar el ID retornado
    INSERT INTO proforma (id_cliente, total, descuento)
    VALUES (p_id_cliente, p_total, p_descuento)
    RETURNING id INTO nueva_proforma_id;

    -- Recorrer cada detalle dentro del JSON
    FOR item IN SELECT * FROM json_array_elements(p_detalles)
    LOOP
        prod_id := (item ->> 'Id')::INT;
        cantidad := (item ->> 'Cantidad')::INT;
        precio_unit := (item ->> 'PrecioUnitario')::NUMERIC;
        subtotal := precio_unit * cantidad;

        INSERT INTO proforma_detalle (id_proforma, id_producto, cantidad, precio_unitario, subtotal)
        VALUES (nueva_proforma_id, prod_id, cantidad, precio_unit, subtotal);
    END LOOP;

    -- Retornar el ID de la proforma generada
    RETURN nueva_proforma_id;
END;
$$ LANGUAGE plpgsql;
