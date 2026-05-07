using Xunit;
using GyG.Presentacion;
using System.Windows.Forms;

namespace GyG.Tests
{
    public class ContabilidadFormTests
    {
        [Fact]
        public void ContabilidadForm_Constructor_NoLanzaExcepcion()
        {
            // Act & Assert
            var exception = Record.Exception(() => new ContabilidadForm());
            Assert.Null(exception);
        }

        [Fact]
        public void ContabilidadForm_FormularioSeCreaCorrectamente()
        {
            // Act
            var form = new ContabilidadForm();

            // Assert
            Assert.NotNull(form);
            Assert.Equal("Reportes Contables - Sacuanjoche", form.Text);
            Assert.Equal(1300, form.ClientSize.Width);
            Assert.Equal(850, form.ClientSize.Height);
        }

        [Fact]
        public void ContabilidadForm_ControlesExisten()
        {
            // Arrange
            var form = new ContabilidadForm();

            // Act & Assert - Verificar que los controles existen usando reflexión
            var campos = new[]
            {
                "dgvEstadoResultados",
                "dgvBalanceGeneral",
                "dgvLibroDiario",
                "dgvLibroMayor",
                "dgvFlujoCaja",
                "lblEstadoResultados",
                "lblBalanceGeneral",
                "lblLibroDiario",
                "lblLibroMayor",
                "lblFlujoCaja",
                "pnlMainContainer"
            };

            foreach (var campo in campos)
            {
                var field = typeof(ContabilidadForm).GetField(campo, 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Instance);
                
                Assert.NotNull(field);
                var valor = field.GetValue(form);
                Assert.NotNull(valor);
            }
        }
    }
}