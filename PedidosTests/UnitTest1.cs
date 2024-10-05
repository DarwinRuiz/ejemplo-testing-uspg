using pedidos.Entidades;
using NUnit.Framework;

namespace PedidosTests
{
    public class Tests
    {

        private Producto _producto1;
        private Producto _producto2;
        private Pedido _pedido;

        [SetUp]
        public void Setup()
        {
            _producto1 = new Producto("Laptop", 1000m, 20);
            _producto2 = new Producto("Mouse", 50m, 50);
            _pedido = new Pedido();
        }

        [Test]
        public void AgregarProducto_StockSuficiente_AgregaProductoCorrectamente()
        {
            // Act
            _pedido.AgregarProducto(_producto1, 2);

            // Assert
            Assert.That(_pedido.Items.Count, Is.EqualTo(1));
            Assert.That(_pedido.Items[0].Producto, Is.EqualTo(_producto1));
            Assert.That(_pedido.Items[0].Cantidad, Is.EqualTo(2));
        }

        [Test]
        public void AgregarProducto_StockInsuficiente_LanzaExcepcion()
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                _pedido.AgregarProducto(_producto1, 25));
            Assert.That(ex.Message, Is.EqualTo("No hay suficiente stock disponible."));
        }

        [Test]
        public void CalcularTotal_SinDescuento_CalculaCorrectamente()
        {
            // Arrange
            _pedido.AgregarProducto(_producto1, 5); // 5 * 1000 = 5000
            _pedido.AgregarProducto(_producto2, 3); // 3 * 50 = 150

            // Act
            decimal total = _pedido.CalcularTotal();

            // Assert
            Assert.That(total, Is.EqualTo(5150)); // 5000 + 150 + (5150 * 0.12)
        }

        [Test]
        public void CalcularTotal_ConDescuento_CalculaCorrectamente()
        {
            // Arrange
            _pedido.AgregarProducto(_producto1, 15); // 15 * 1000 = 15000 (10% de descuento)
            _pedido.AgregarProducto(_producto2, 5); // 5 * 50 = 250

            // Act
            decimal total = _pedido.CalcularTotal();

            // Assert
            Assert.That(total, Is.EqualTo(15900)); // (15000 * 0.9) + 250 + ((15900) * 0.12)
        }

        [Test]
        public void ProcesarPedido_ReduceStockCorrectamente()
        {
            // Arrange
            _pedido.AgregarProducto(_producto1, 2);
            _pedido.AgregarProducto(_producto2, 3);

            // Act
            _pedido.ProcesarPedido();

            // Assert
            Assert.That(_producto1.Stock, Is.EqualTo(18)); // Stock inicial 20 - 2
            Assert.That(_producto2.Stock, Is.EqualTo(47)); // Stock inicial 50 - 3
        }

    }
}