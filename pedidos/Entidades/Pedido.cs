using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pedidos.Entidades
{
    public class Pedido
    {
        public List<(Producto Producto, int Cantidad)> Items { get; private set; }
        private const decimal Impuesto = 0.12m;

        public Pedido()
        {
            Items = new List<(Producto Producto, int Cantidad)>();
        }

        // Método para agregar productos al pedido
        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (producto.Stock < cantidad)
                throw new InvalidOperationException("No hay suficiente stock disponible.");
            Items.Add((producto, cantidad));
        }

        // Método para calcular el total con descuentos e impuestos
        public decimal CalcularTotal()
        {
            decimal subtotal = 0;

            foreach (var item in Items)
            {
                decimal precioItem = item.Producto.Precio * item.Cantidad;

                // Aplicar descuento si la cantidad es mayor a 10
                if (item.Cantidad > 10)
                {
                    precioItem *= 0.9m; // 10% de descuento
                }

                subtotal += precioItem;
            }

            // Calcular el total con impuestos
            decimal totalConImpuesto = subtotal + (subtotal * Impuesto);
            return totalConImpuesto;
        }

        // Método para procesar el pedido y reducir el stock
        public void ProcesarPedido()
        {
            foreach (var item in Items)
            {
                item.Producto.ReducirStock(item.Cantidad);
            }
        }
    }
}
