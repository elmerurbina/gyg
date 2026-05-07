
using Xunit;
using GyG.Presentacion;

namespace GyG.Tests
{
    public class VentasFormTests
    {
        [Fact]
        public void Test_SubtotalProductoCarrito_SinIVA_SinDescuento()
        {
            var producto = new ProductoCarrito
            {
                Cantidad = 3,
                PrecioUnitario = 100,
                IVA = 0,
                Descuento = 0
            };

            var subtotal = producto.Subtotal;

            Assert.Equal(300, subtotal);
        }

        [Fact]
        public void Test_SubtotalProductoCarrito_ConIVA_SinDescuento()
        {
            var producto = new ProductoCarrito
            {
                Cantidad = 2,
                PrecioUnitario = 100,
                IVA = 15,
                Descuento = 0
            };

            var subtotal = producto.Subtotal;

            Assert.Equal(230, subtotal);
        }
    }
}
