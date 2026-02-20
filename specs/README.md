# Chefs App — Feature Specifications

This directory contains comprehensive feature specifications for the Uno Chefs application. Each spec defines a feature area with acceptance criteria suitable for automated UI regression testing.

## App Overview

Uno Chefs is a recipe discovery, cooking, and community app built with [Uno Platform](https://platform.uno). Users can browse recipes, search with filters, save favorites, create cookbooks, follow step-by-step live cooking instructions, view nutritional information, and manage their profile/settings.

## Spec Structure

| Spec File | Feature Area |
|---|---|
| [01-welcome.md](01-welcome.md) | Welcome / Onboarding Flow |
| [02-login.md](02-login.md) | Authentication — Login |
| [03-registration.md](03-registration.md) | Authentication — Registration |
| [04-home.md](04-home.md) | Home Page |
| [05-search.md](05-search.md) | Search & Filters |
| [06-recipe-details.md](06-recipe-details.md) | Recipe Details (Ingredients, Steps, Reviews, Nutrition) |
| [07-live-cooking.md](07-live-cooking.md) | Live Cooking Mode |
| [08-favorites.md](08-favorites.md) | Favorite Recipes |
| [09-cookbooks.md](09-cookbooks.md) | Cookbooks (Create, Update, View) |
| [10-profile.md](10-profile.md) | User Profile |
| [11-notifications.md](11-notifications.md) | Notifications |
| [12-settings.md](12-settings.md) | Settings |
| [13-map.md](13-map.md) | Map — Near Me |
| [14-navigation.md](14-navigation.md) | App Navigation & Shell |
| [15-sharing.md](15-sharing.md) | Recipe Sharing |
| [16-responsive-layout.md](16-responsive-layout.md) | Responsive Layout (Normal / Wide) |

## Screenshots

Reference screenshots are stored in the [screenshots/](screenshots/) directory, captured from the desktop (Skia) target.

## Testing Approach

These specs are written to support automated UI testing via:
- **Uno Platform Dev Server** (MCP tools for visual tree inspection, pointer clicks, screenshots)
- **Uno UITest framework** (existing `Chefs.UITests` project)
- **Agent-driven regression testing** — an AI agent periodically exercises each acceptance criterion, captures screenshots, and compares against baselines

## Conventions

- Each spec uses **Given / When / Then** acceptance criteria format
- `AutomationProperties.AutomationId` values are noted where they exist in XAML for element targeting
- Navigation routes are documented to aid programmatic navigation
- Visual assertions reference screenshot baselines in the `screenshots/` folder
