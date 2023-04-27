using System.ComponentModel.DataAnnotations;


namespace RecipeAPI;

public class Recipe
{
    public Guid Id { get; set; }
    public int PublicId { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public Type Category { get; set; }

    public Recipe()
    {
        Id = new Guid();
    }
}

public enum Type
{
    Meal,
    Entree,
    Appetizer,
    Dessert,
    Beverage
}

