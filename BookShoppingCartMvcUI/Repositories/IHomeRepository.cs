namespace DrugShoppingCartMvcUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Drug>> GetDrugss(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}