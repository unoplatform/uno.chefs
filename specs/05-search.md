# Feature: Search Page

**Page:** `SearchPage`
**ViewModel:** `SearchModel`
**Route:** `/Main/Search`

## Description

The Search page lets users find recipes by text query and/or structured filters (category, time, difficulty, serves). It shows search results in a grid, maintains search history, and provides curated sections (Recommended, From the Chefs) when no search is active.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Search box | TextBox | Text input for recipe search term |
| Filters button | Button | Opens the Filters page as a modal |
| Active filter chips | ChipGroup | Displays currently active filter values with remove capability |
| Results count | TextBlock | Shows total number of results (e.g., "33 Results") |
| Results grid | GridView / ListView | Displays matching recipes as cards |
| Recipe card | Card | Shows image, title, author, cook time, favorite heart |
| Recommended section | Section | Shown when no search term; horizontal recipe list |
| From the Chefs section | Section | Shown when no search term; horizontal recipe list |
| Search history | Section | Previously searched terms, shown when search box is focused |
| Empty state | Illustration + text | Shown when search returns no results, with "View popular recipes" link |

## Acceptance Criteria

### AC-1: Initial state — no search
- **Given** the user navigates to the Search page without a pre-set filter
- **When** the page loads
- **Then** the search box is empty
- **And** the Recommended and From the Chefs sections are displayed

### AC-2: Text search
- **Given** the Search page is displayed
- **When** the user types a term in the search box
- **Then** the results grid updates to show recipes matching the term
- **And** the results count is displayed (e.g., "33 Results")

### AC-3: Search results display
- **Given** search results are loaded
- **When** the results grid is visible
- **Then** each recipe card shows: image, title, author name, cook time, and a favorite heart icon

### AC-4: Navigate to recipe details
- **Given** search results are displayed
- **When** the user taps a recipe card
- **Then** the app navigates to the Recipe Details page for that recipe

### AC-5: Open filters
- **Given** the Search page is displayed
- **When** the user taps the Filters button
- **Then** the Filters page opens as a modal / overlay

### AC-6: Apply filters
- **Given** the user has selected filters on the Filters page
- **When** the user taps "Show results" on the Filters page
- **Then** the Search page updates results based on the applied filters
- **And** active filter chips appear below the search box

### AC-7: Active filter chips
- **Given** filters are applied and chips are visible
- **When** the user taps the "×" on a filter chip
- **Then** that filter is removed
- **And** results update accordingly

### AC-8: Reset filters
- **Given** filters are applied
- **When** the user taps "Reset" (via Filters page or chip area)
- **Then** all filters are cleared
- **And** results return to the unfiltered search results

### AC-9: Has-filter indicator
- **Given** filters are currently applied
- **When** the Search page is displayed
- **Then** a visual indicator (badge or highlight) appears on the Filters button

### AC-10: Empty results
- **Given** the user has searched for a term that returns no results
- **When** the results grid is empty
- **Then** an empty state illustration and message are shown
- **And** a "View popular recipes" link is provided

### AC-11: Search history
- **Given** the user has previously submitted search queries
- **When** the search box is focused
- **Then** recent search terms are displayed for quick re-selection

### AC-12: Navigate from Home with category
- **Given** the user tapped a category on the Home page
- **When** the Search page loads
- **Then** the filter for that category is pre-applied
- **And** results are filtered to that category

### AC-13: Favorite toggle on result card
- **Given** a recipe card is displayed in search results
- **When** the user taps the heart icon
- **Then** the recipe's favorite state toggles
- **And** the heart icon updates accordingly
