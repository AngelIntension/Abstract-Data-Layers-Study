using System;

namespace DomainLayer
{
    public class NotEnoughStockException : Exception
    {
        public NotEnoughStockException(int quantityInStock, int amountToRemove)
            : base($"You cannot remove {amountToRemove} item(s) when there is only {quantityInStock} item(s) in stock.")
        {
            QuantityInStock = quantityInStock;
            AmountToRemove = amountToRemove;
        }

        public int QuantityInStock { get; set; }
        public int AmountToRemove { get; set; }
    }
}
