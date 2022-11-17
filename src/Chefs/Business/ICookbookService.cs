namespace Chefs.Business;

public interface ICookbookService
{
    /// <summary>
    /// Add cookbook created by the user
    /// </summary>
    /// <param name="cookbook">Cookbook to add</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask Create(string name, IImmutableList<Recipe> recipes, CancellationToken ct);

    /// <summary>
    /// Add cookbook created by the user
    /// </summary>
    /// <param name="cookbook">Cookbook to add</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct);

    /// <summary>
    /// Add cookbook created by the user
    /// </summary>
    /// <param name="cookbook">Cookbook to add</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask Update(Cookbook cookbook, CancellationToken ct);

    /// <summary>
    /// Add cookbook that the user wants to save
    /// </summary>
    /// <param name="cookbook">Cookbook to add</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask Save(Cookbook cookbook, CancellationToken ct);

    /// <summary>
    /// Cookbooks saved from api
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Get each cookbook from api that was saved
    /// </returns>
    ValueTask<IImmutableList<Cookbook>> GetSaved(CancellationToken ct);

    /// <summary>
    /// Cookbooks by user
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// User's cookbooks
    /// </returns>
    ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct);
}
