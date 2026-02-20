# Feature: Cookbooks (Create, Update, Detail)

**Pages:** `CreateUpdateCookbookPage`, `CookbookDetailPage`
**ViewModels:** `CreateUpdateCookbookModel`, `CookbookDetailModel`
**Routes:** `/Main/CreateCookbook`, `/Main/UpdateCookbook`, `/Main/CookbookDetails`

## Description

Users can create custom cookbooks by giving them a name and selecting recipes from a paginated list. Existing cookbooks can be updated (rename, add/remove recipes). The Cookbook Detail page shows a cookbook's recipes in a grid with an edit option.

## UI Elements — Create/Update Cookbook

| Element | Type | Description |
|---|---|---|
| Page title | TextBlock | "New Cookbook" (create) or "Update Cookbook" (edit) |
| Cookbook name input | TextBox | Text field for the cookbook's name |
| Recipe list | GridView | Paginated, multi-selectable grid of all available recipes |
| Recipe card | Card | Selectable card with image, title, author; shows selection state |
| Load more | Button/Trigger | Loads next page of recipes |
| Submit button | Button | "Add" (create) or "Save" (update); validates and saves |
| Back / Cancel | Button | Returns to previous page without saving |

## UI Elements — Cookbook Detail

| Element | Type | Description |
|---|---|---|
| Cookbook title | TextBlock | Name of the cookbook |
| Recipe count | TextBlock | Number of recipes in the cookbook |
| Recipe grid | GridView | Displays all recipes in the cookbook |
| Edit FAB | Button | Opens Update Cookbook page for this cookbook |
| Back button | Button | Returns to Favorites page |

## Acceptance Criteria

### AC-1: Create cookbook — initial state
- **Given** the user navigates to "New Cookbook"
- **When** the page loads
- **Then** the cookbook name field is empty
- **And** a paginated list of recipes is displayed for selection
- **And** no recipes are pre-selected

### AC-2: Select recipes for cookbook
- **Given** the Create Cookbook page is displayed
- **When** the user taps recipe cards
- **Then** tapped cards show a selected visual state
- **And** multiple recipes can be selected simultaneously

### AC-3: Submit validation — no name
- **Given** the user has selected recipes but the name field is empty
- **When** the user taps "Add"
- **Then** validation fails and the cookbook is not created

### AC-4: Submit validation — no recipes
- **Given** the user has entered a name but selected no recipes
- **When** the user taps "Add"
- **Then** validation fails and the cookbook is not created

### AC-5: Successful creation
- **Given** the user has entered a name and selected at least one recipe
- **When** the user taps "Add"
- **Then** the cookbook is created
- **And** the app navigates back (to Favorites or previous page)

### AC-6: Paginate recipes
- **Given** the recipe list has more items than the initial page
- **When** the user scrolls to the end of the list
- **Then** the next page of recipes loads and appends to the grid

### AC-7: Update cookbook — pre-populated state
- **Given** the user navigates to "Update Cookbook" for an existing cookbook
- **When** the page loads
- **Then** the cookbook name field is pre-filled with the existing name
- **And** previously selected recipes are shown in the selected state

### AC-8: Update cookbook — save changes
- **Given** the user modifies the name or recipe selection
- **When** the user taps "Save"
- **Then** the cookbook is updated with the new data
- **And** the app navigates back

### AC-9: Cookbook Detail — display
- **Given** the user navigates to a Cookbook Detail page
- **When** the page loads
- **Then** the cookbook name and recipe count are displayed
- **And** all recipes in the cookbook are shown in a grid

### AC-10: Cookbook Detail — navigate to recipe
- **Given** the Cookbook Detail page shows recipes
- **When** the user taps a recipe card
- **Then** the app navigates to the Recipe Details page for that recipe

### AC-11: Cookbook Detail — edit
- **Given** the Cookbook Detail page is displayed
- **When** the user taps the edit FAB
- **Then** the app navigates to Update Cookbook page with this cookbook's data
