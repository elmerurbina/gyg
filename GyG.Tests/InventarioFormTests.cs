
using Xunit;
using GyG.Presentacion;
using System.Windows.Forms;

namespace GyG.Tests
{
    public class InventarioFormTests
    {
        [Fact]
        public void InventarioForm_Constructor_NoLanzaExcepcion()
        {
            var exception = Record.Exception(() => new InventarioForm());
            Assert.Null(exception);
        }

        [Fact]
        public void InventarioForm_FormularioSeCreaCorrectamente()
        {
            var form = new InventarioForm();
            Assert.NotNull(form);
            // CORREGIDO: Ahora espera "Sacuanjoche" en lugar de "Negocio+"
            Assert.Equal("Gestión de Inventario - Sacuanjoche", form.Text);
        }

        [Fact]
        public void InventarioForm_DataGridViewExiste()
        {
            var form = new InventarioForm();
            var dgvProductos = GetDataGridView(form, "dgvProductos");
            Assert.NotNull(dgvProductos);
        }

        [Fact]
        public void InventarioForm_BotonesExisten()
        {
            var form = new InventarioForm();
            Assert.NotNull(GetButton(form, "btnGuardar"));
            Assert.NotNull(GetButton(form, "btnEditar"));
            Assert.NotNull(GetButton(form, "btnEliminar"));
        }

        [Fact]
        public void InventarioForm_TextBoxesExisten()
        {
            var form = new InventarioForm();
            Assert.NotNull(GetTextBox(form, "txtNombre"));
            Assert.NotNull(GetTextBox(form, "txtDescripcion"));
            Assert.NotNull(GetTextBox(form, "txtPrecioInv"));
            Assert.NotNull(GetTextBox(form, "txtPrecioVenta"));
            Assert.NotNull(GetTextBox(form, "txtStock"));
            Assert.NotNull(GetTextBox(form, "txtCodigoBarra"));
            Assert.NotNull(GetTextBox(form, "txtIVA"));
            Assert.NotNull(GetTextBox(form, "txtDescuento"));
            Assert.NotNull(GetTextBox(form, "txtBuscar"));
        }

        [Fact]
        public void InventarioForm_DateTimePickerExiste()
        {
            var form = new InventarioForm();
            var dtpFechaExpiracion = GetDateTimePicker(form, "dtpFechaExpiracion");
            Assert.NotNull(dtpFechaExpiracion);
        }

        [Fact]
        public void InventarioForm_ComboBoxExiste()
        {
            var form = new InventarioForm();
            var cmbCategoria = GetComboBox(form, "cmbCategoria");
            Assert.NotNull(cmbCategoria);
        }

        [Fact]
        public void InventarioForm_ValidarCampos_Existe()
        {
            var form = new InventarioForm();
            var method = typeof(InventarioForm).GetMethod("ValidarCampos",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(method);
        }

        [Fact]
        public void InventarioForm_HabilitarBotones_Existe()
        {
            var form = new InventarioForm();
            var method = typeof(InventarioForm).GetMethod("HabilitarBotones",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(method);
        }

        [Fact]
        public void InventarioForm_CargarProductos_Existe()
        {
            var form = new InventarioForm();
            var method = typeof(InventarioForm).GetMethod("CargarProductos",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(method);
        }

        [Fact]
        public void InventarioForm_LimpiarCampos_Existe()
        {
            var form = new InventarioForm();
            var method = typeof(InventarioForm).GetMethod("LimpiarCampos",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(method);
        }

        // Métodos auxiliares para acceder a controles privados
        private DataGridView GetDataGridView(InventarioForm form, string fieldName)
        {
            var field = typeof(InventarioForm).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            return field?.GetValue(form) as DataGridView;
        }

        private Button GetButton(InventarioForm form, string fieldName)
        {
            var field = typeof(InventarioForm).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            return field?.GetValue(form) as Button;
        }

        private TextBox GetTextBox(InventarioForm form, string fieldName)
        {
            var field = typeof(InventarioForm).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            return field?.GetValue(form) as TextBox;
        }

        private DateTimePicker GetDateTimePicker(InventarioForm form, string fieldName)
        {
            var field = typeof(InventarioForm).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            return field?.GetValue(form) as DateTimePicker;
        }

        private ComboBox GetComboBox(InventarioForm form, string fieldName)
        {
            var field = typeof(InventarioForm).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            return field?.GetValue(form) as ComboBox;
        }
    }
}
