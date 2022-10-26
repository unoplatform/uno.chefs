using Chefs.Data;

namespace Chefs.Business;

public record SearchFilter(
    OrganizeCategories? OrganizeCategories, 
    Times? Times, 
    Difficulties? Difficulty, 
    Category? Category,
    string? TextFilter
);
