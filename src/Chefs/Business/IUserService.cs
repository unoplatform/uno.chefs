﻿using Chefs.Settings;

namespace Chefs.Business;

/// <summary>
/// Implements user related methods
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Authentication method
    /// </summary>
    /// <param name="email"> The user email </param>
    /// <param name="ct"></param>
    /// <returns>
    /// User logged in
    /// </returns>
    ValueTask<User?> BasicAuthenticate(string email, string password, CancellationToken ct);

    /// <summary>
    /// Current user data
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// User logged in
    /// </returns>
    ValueTask<User> GetUser(CancellationToken ct);

    ///<summary>
    /// Gets chef settings
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// App settings from the phone
    /// </returns>
    ValueTask<ChefApp> GetChefSettings(CancellationToken ct);

    ///<summary>
    /// Update app settings
    /// </summary>
    /// <param name="chefSettings">new settings</param>
    /// <param name="ct"></param>
    /// <returns>
    /// </returns>
    Task SetCheffSettings(ChefApp chefSettings, CancellationToken ct);
}
