using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;
using Microsoft.AspNetCore.Http;


namespace RecipeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : Controller
{
    private readonly IRecipeRepository recipeRepository;

    public RecipeController(IRecipeRepository recipeRepository)
    {
        this.recipeRepository = recipeRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetRecipes()
    {
        try
        {
            return Ok(await recipeRepository.GetRecipes());

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
        }
    }

    [HttpGet("(PublicId:int)")]
    
    public async Task<ActionResult<Recipe>> GetRecipe(int id)
    {
        try
        {
            var result = await recipeRepository.GetRecipe(id);

            if(result == null)
            {
                return NotFound();
            }

            return result;
        }
        catch(Exception) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
        }
    }

    [HttpPost]

    public async Task<ActionResult<Recipe>> AddRecipe(Recipe recipe)
    {
        try
        {
            if(recipe == null)
            {
                return BadRequest();
            }
            var doesRecipeExist = await recipeRepository.GetRecipe(recipe.PublicId);

            if(doesRecipeExist != null)
            {
                ModelState.AddModelError("Id", "Recipe id already in use");
                return BadRequest(ModelState);
            }

            var addedRecipe = await recipeRepository.AddRecipe(recipe);
            return CreatedAtAction(nameof(GetRecipe), addedRecipe);
        }
        catch(Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error adding new recipe");
        }
    }

    [HttpPut("(PublicId: int)")]

    public async Task<ActionResult<Recipe>> UpdateRecipe(int id, Recipe recipe)
    {
        try
        {
            if(id != recipe.PublicId)
            {
                return BadRequest("Recipe id is not correct");
            }

            var recipeToUpdate = await recipeRepository.GetRecipe(id);
            if(recipeToUpdate == null) 
            {
                return NotFound($"Recipe id: {id} not found");
            }

            var result = await recipeRepository.UpdateRecipe(recipe);
            return result;
        }
        catch(Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating recipe.");

        }
    }

    [HttpDelete("(PublicId: int)")]

    public async Task<ActionResult> DeleteRecipe(int id)
    {
        try
        {
            var recipeToDelete = await recipeRepository.GetRecipe(id);

            if(recipeToDelete == null)
            {
                return NotFound();
            }

            await recipeRepository.DeleteRecipe(id);
            return Ok($"Recipe id: {id} was deleted");

        }
        catch(Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Item could not be deleted at this time");
        }
    }

}
