using Microsoft.EntityFrameworkCore;

namespace DrugShoppingCartMvcUI.Repositories
{
    public interface IDrugRepository
    {
        Task AddDrug(Drug drug);
        Task DeleteDrug(Drug drug);
        Task<Drug?> GetDrugById(int id);
        Task<IEnumerable<Drug>> GetDrugs();
        Task UpdateDrug(Drug drug);
    }

    public class DrugRepository : IDrugRepository
    {
        private readonly ApplicationDbContext _context;
        public DrugRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddDrug(Drug drug)
        {
            _context.Drugs.Add(drug);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDrug(Drug drug)
        {
            _context.Drugs.Update(drug);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDrug(Drug drug)
        {
            _context.Drugs.Remove(drug);
            await _context.SaveChangesAsync();
        }

        public async Task<Drug?> GetDrugById(int id) => await _context.Drugs.FindAsync(id);

        public async Task<IEnumerable<Drug>> GetDrugs() => await _context.Drugs.Include(a=>a.Genre).ToListAsync();
    }
}
