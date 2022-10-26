﻿using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public record Recipe
{
    internal Recipe(RecipeData recipeData)
    {
        Id = recipeData.Id;
        UserId = recipeData.UserId;
        Steps = recipeData.Steps?
            .Select(s => new Step(s))
            .ToImmutableList();
        ImageUrl = recipeData.ImageUrl;
        Name = recipeData.Name;
        Serves = recipeData.Serves;
        CookTime = recipeData.CookTime;
        Difficulty = recipeData.Difficulty;
        Ingredients = recipeData.Ingredients?
            .Select(i => new Ingredient(i))
            .ToImmutableList();
        Calories = recipeData.Calories;
        Reviews = recipeData.Reviews?
            .Select(r => new Review(r))
            .ToImmutableList();
        Details = recipeData.Details;
        Category = new Category(recipeData.Category);
        Date = recipeData.Date;
    }
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public IImmutableList<Step>? Steps { get; init; }
    public string? ImageUrl { get; init; }
    public string? Name { get; init; }
    public int Serves { get; init; }
    public TimeSpan CookTime { get; init; }
    public Difficulties Difficulty { get; init; }
    public IImmutableList<Ingredient>? Ingredients { get; init; }
    public string? Calories { get; init; }
    public IImmutableList<Review>? Reviews { get; init; }
    public string? Details { get; init; }
    public Category Category { get; init; }
    public DateTime Date { get; init; }
    public bool Save { get; init; }

    internal RecipeData ToData() => new()
    {
        Id = Id,
        UserId = UserId,
        Steps = Steps?
            .Select(s => s.ToData())
            .ToImmutableList(),
        ImageUrl = ImageUrl,
        Name = Name,
        Serves = Serves,
        CookTime = CookTime,
        Difficulty = Difficulty,
        Ingredients = Ingredients?
            .Select(i => i.ToData())
            .ToImmutableList(),
        Calories = Calories,
        Reviews = Reviews?
            .Select(r => r.ToData())
            .ToImmutableList(),
        Details = Details,
        Category = Category.ToData(),
        Date = Date
    };
}
