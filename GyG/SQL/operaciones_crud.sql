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

-- Insertar
CREATE OR REPLACE FUNCTION sp_insert_producto(
    p_nombre VARCHAR, p_descripcion TEXT, p_categoria VARCHAR,
    p_precio_inventario NUMERIC, p_precio_venta NUMERIC,
    p_stock INT, p_codigo VARCHAR, p_fecha_vencimiento DATE,
    p_iva NUMERIC, p_descuento NUMERIC
)
RETURNS VOID AS $$
BEGIN
INSERT INTO producto (nombre, descripcion, categoria, precio_inventario, precio_venta, stock, codigo, fecha_vencimiento, iva, descuento)
VALUES (p_nombre, p_descripcion, p_categoria, p_precio_inventario, p_precio_venta, p_stock, p_codigo, p_fecha_vencimiento, p_iva, p_descuento);
END;
$$ LANGUAGE plpgsql;

-- Leer
CREATE OR REPLACE FUNCTION sp_get_productos()
RETURNS TABLE(id INT, nombre VARCHAR, descripcion TEXT, categoria VARCHAR, precio_venta NUMERIC, stock INT) AS $$
BEGIN
RETURN QUERY SELECT id, nombre, descripcion, categoria, precio_venta, stock FROM producto;
END;
$$ LANGUAGE plpgsql;

-- Actualizar
CREATE OR REPLACE FUNCTION sp_update_producto(
    p_id INT, p_nombre VARCHAR, p_descripcion TEXT, p_categoria VARCHAR,
    p_precio_inventario NUMERIC, p_precio_venta NUMERIC,
    p_stock INT, p_codigo VARCHAR, p_fecha_vencimiento DATE,
    p_iva NUMERIC, p_descuento NUMERIC
)
RETURNS VOID AS $$
BEGIN
UPDATE producto
SET nombre = p_nombre, descripcion = p_descripcion, categoria = p_categoria,
    precio_inventario = p_precio_inventario, precio_venta = p_precio_venta,
    stock = p_stock, codigo = p_codigo, fecha_vencimiento = p_fecha_vencimiento,
    iva = p_iva, descuento = p_descuento
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

-- Insertar
CREATE OR REPLACE FUNCTION sp_insert_cliente(
    p_nombre VARCHAR, p_telefono VARCHAR, p_ubicacion TEXT
)
RETURNS VOID AS $$
BEGIN
INSERT INTO cliente(nombre, telefono, ubicacion)
VALUES (p_nombre, p_telefono, p_ubicacion);
END;
$$ LANGUAGE plpgsql;

-- Leer
CREATE OR REPLACE FUNCTION sp_get_clientes()
RETURNS TABLE(id INT, nombre VARCHAR, telefono VARCHAR, ubicacion TEXT) AS $$
BEGIN
RETURN QUERY SELECT id, nombre, telefono, ubicacion FROM cliente;
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
