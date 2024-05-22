using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrugShoppingCartMvcUI.Repositories
{
    // Observer interface
    public interface IStockObserver
    {
        Task UpdateStock(StockDTO stockToManage);
    }

    // Subject class
    public class StockSubject
    {
        private readonly List<IStockObserver> _observers = new List<IStockObserver>();

        public void Attach(IStockObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IStockObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyObservers(StockDTO stockToManage)
        {
            foreach (var observer in _observers)
            {
                await observer.UpdateStock(stockToManage);
            }
        }
    }

    public class StockRepository : IStockRepository, IStockObserver
    {
        private readonly ApplicationDbContext _context;
        private readonly StockSubject _stockSubject;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
            _stockSubject = new StockSubject();
        }

        public async Task<Stock?> GetStockByDrugId(int drugId) => await _context.Stocks.FirstOrDefaultAsync(s => s.DrugId == drugId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given drug id, then add new record
            // if there is already stock for given drug id, update stock's quantity
            var existingStock = await GetStockByDrugId(stockToManage.DrugId);
            if (existingStock is null)
            {
                var stock = new Stock { DrugId = stockToManage.DrugId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }

            await _context.SaveChangesAsync();
            // Notify observers after stock management
            await _stockSubject.NotifyObservers(stockToManage);
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from drug in _context.Drugs
                                join stock in _context.Stocks
                                on drug.Id equals stock.DrugId
                                into drug_stock
                                from drugStock in drug_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || drug.DrugName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    DrugId = drug.Id,
                                    DrugName = drug.DrugName,
                                    Quantity = drugStock == null ? 0 : drugStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

        // Implementation of IStockObserver
        public async Task UpdateStock(StockDTO stockToManage)
        {
            // Do something when stock is updated, if needed.
            // For example, log the update.
            Console.WriteLine($"Stock updated for Drug ID: {stockToManage.DrugId}, New Quantity: {stockToManage.Quantity}");
        }
    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByDrugId(int drugId);
        Task ManageStock(StockDTO stockToManage);
    }
}
