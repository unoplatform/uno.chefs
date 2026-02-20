# Feature: Favorites Page

**Page:** `FavoriteRecipesPage`
**ViewModel:** `FavoriteRecipesModel`
**Route:** `/Main/FavoriteRecipes`

## Description

The Favorites page shows the user's saved recipes and cookbooks in two tabs: "All Recipes" and "My Cookbooks". Users can browse their favorite recipes, view their custom cookbooks, and create new cookbooks.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Top tab bar | TopTabBar | Tabs: "All Recipes" and "My Cookbooks" |
| Recipe count | TextBlock | Number of saved recipes (e.g., "21 Results") |
| Recipe grid | GridView | Grid of favorited recipe cards |
| Recipe card | Card | Shows image, title, author, cook time, favorite heart |
| Cookbook grid | GridView | Grid of cookbook cards |
| Cookbook card | Card | Shows 4-image collage, cookbook name, recipe count |
| Create cookbook FAB | Button | "+" floating action button; opens Create Cookbook page |
| Empty state (recipes) | Panel | Illustration and message when no favorites exist |
| Empty state (cookbooks) | Panel | Illustration and message when no cookbooks exist |

## Acceptance Criteria

### AC-1: Default tab — All Recipes
- **Given** the user navigates to the Favorites page
- **When** the page loads
- **Then** the "All Recipes" tab is selected by default
- **And** the user's favorited recipes are displayed in a grid
- **And** a results count is shown

### AC-2: Recipe card display
- **Given** the All Recipes tab is active
- **When** recipes are loaded
- **Then** each card shows the recipe image, title, author name, and cook time

### AC-3: Navigate to recipe from favorites
- **Given** the All Recipes tab is showing recipes
- **When** the user taps a recipe card
- **Then** the app navigates to the Recipe Details page for that recipe

### AC-4: My Cookbooks tab
- **Given** the Favorites page is displayed
- **When** the user selects the "My Cookbooks" tab
- **Then** the user's saved cookbooks are displayed in a grid

### AC-5: Cookbook card display
- **Given** the My Cookbooks tab is active
- **When** cookbooks are loaded
- **Then** each cookbook card shows a 4-image collage from its recipes, the cookbook name, and recipe count

### AC-6: Navigate to cookbook detail
- **Given** the My Cookbooks tab shows cookbooks
- **When** the user taps a cookbook card
- **Then** the app navigates to the Cookbook Detail page for that cookbook

### AC-7: Create new cookbook
- **Given** the My Cookbooks tab is active
- **When** the user taps the "+" (create) FAB
- **Then** the app navigates to the Create Cookbook page

### AC-8: Empty state — no recipes
- **Given** the user has no favorited recipes
- **When** the All Recipes tab is displayed
- **Then** an empty state with an illustration and message is shown

### AC-9: Empty state — no cookbooks
- **Given** the user has no saved cookbooks
- **When** the My Cookbooks tab is displayed
- **Then** an empty state with an illustration and message is shown

### AC-10: Unfavorite from card
- **Given** a recipe card is displayed with a filled heart icon
- **When** the user taps the heart icon
- **Then** the recipe is removed from favorites
- **And** the card is removed from the grid
