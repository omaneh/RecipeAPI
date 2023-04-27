namespace RecipeAPI.Models;

public interface IRecipeRepository
{

    Task<IEnumerable<Recipe>> GetRecipes();

    Task<Recipe> GetRecipe(int Id);

    Task<Recipe> AddRecipe(Recipe recipe);

    Task<Recipe> UpdateRecipe(Recipe recipe);

    Task DeleteRecipe(int Id);

}
