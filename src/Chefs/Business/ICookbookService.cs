using System.Collections.Immutable;

namespace Chefs.Business;

public interface ICookbookService
{
    /// <summary>
    /// Add cookbook created by the user
    /// </summary>
    /// <param name="cookbook">Cookbook to add</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask AddUserCookbook(Cookbook cookbook, CancellationToken ct);

    /// <summary>
    /// Add cookbook that the user wants to save
    /// </summary>
    /// <param name="cookbook">Cookbook to add</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    ValueTask SaveCookbook(Cookbook cookbook, CancellationToken ct);

    /// <summary>
    /// Cookbooks saved from api
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Get each cookbook from api that was saved
    /// </returns>
    ValueTask<IImmutableList<Cookbook>> GetSavedCookbooks(CancellationToken ct);

    /// <summary>
    /// Cookbooks created by user from api
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Get each cookbook from api that was created by the user
    /// </returns>
    ValueTask<IImmutableList<Cookbook>> GetUserCookbooks(CancellationToken ct);
}
