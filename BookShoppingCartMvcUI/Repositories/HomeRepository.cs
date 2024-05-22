

using Microsoft.EntityFrameworkCore;

namespace DrugShoppingCartMvcUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Drug>> GetDrugss(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Drug> drugs = await (from drug in _db.Drugs
                         join genre in _db.Genres
                         on drug.GenreId equals genre.Id
                         join stock in _db.Stocks
                         on drug.Id equals stock.DrugId
                         into drug_stocks
                         from drugWithStock in drug_stocks.DefaultIfEmpty()
                         where string.IsNullOrWhiteSpace(sTerm) || (drug != null && drug.DrugName.ToLower().StartsWith(sTerm))
                         select new Drug
                         {
                             Id = drug.Id,
                             Image = drug.Image,
                             Producer = drug.Producer,
                             DrugName = drug.DrugName,
                             GenreId = drug.GenreId,
                             Price = drug.Price,
                             GenreName = genre.CategoryName,
                             Quantity=drugWithStock==null? 0:drugWithStock.Quantity
                         }
                         ).ToListAsync();
            if (genreId > 0)
            {

                drugs = drugs.Where(a => a.GenreId == genreId).ToList();
            }
            return drugs;

        }
    }
}
