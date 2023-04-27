using Microsoft.EntityFrameworkCore;

namespace RecipeAPI.Models;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeDbContext recipieDbContext;

    public RecipeRepository(RecipeDbContext recipieDbContext)
    {
        this.recipieDbContext = recipieDbContext;
    }

    public async Task<IEnumerable<Recipe>> GetRecipes()
    {
        return await recipieDbContext.Recipes.ToListAsync();
    }

    public async Task<Recipe> AddRecipe(Recipe recipe) 
    {
        var result = await recipieDbContext.Recipes.AddAsync(recipe);
        await recipieDbContext.SaveChangesAsync();
        return result.Entity;
    }
    public async Task<Recipe> GetRecipe(int id)
    {
        return await recipieDbContext.Recipes.FirstOrDefaultAsync(p => p.PublicId == id);

    }

    public async Task<Recipe> UpdateRecipe(Recipe recipe)
    {
        var result = await recipieDbContext.Recipes.FirstOrDefaultAsync(p => p.Id == recipe.Id);

        if(result != null)
        {
            result.PublicId = recipieDbContext.Recipes.Count() + 1;
            result.Category = recipe.Category;
            result.Description = recipe.Description;
            result.Title = recipe.Title;
            await recipieDbContext.SaveChangesAsync();
            return result;
        }
        return null;

    }

    public async Task DeleteRecipe(int id)
    {
        var result = await recipieDbContext.Recipes.FirstOrDefaultAsync(p => p.PublicId == id);
        if (result != null)
        {
            recipieDbContext.Recipes.Remove(result);
            int total = 1;
            foreach(var dish in recipieDbContext.Recipes)
            {
                dish.PublicId = total;
                total++;
            }
            await recipieDbContext.SaveChangesAsync();
        }

    }

}
