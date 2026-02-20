# Feature: Home Page

**Page:** `HomePage`
**ViewModel:** `HomeModel`
**Route:** `/Main/Home` (default tab)

## Description

The Home page is the main landing screen after login. It displays trending recipes, categories, recently added recipes, and popular creators in a vertically scrollable layout. A navigation bar provides access to the user profile and notifications.

## UI Elements

| Element | Type | AutomationId | Description |
|---|---|---|---|
| Navigation Bar | NavigationBar | — | App bar with profile avatar and notification bell |
| Profile button | Button | — | User avatar; navigates to current user's Profile |
| Notification bell | Button | — | Navigates to Notifications page |
| Hero / Trending Now | Section | `TrendingNowFeed` | Horizontal scrollable list of large recipe cards |
| Trending "View all" | HyperlinkButton | — | Navigates to Search page filtered to trending |
| Categories | Section | — | Horizontal scrollable chips/cards showing categories with recipe counts |
| Category card | Button (per category) | — | Shows category image, name, and recipe count; navigates to Search filtered by category |
| Recently Added | Section | — | Horizontal scrollable list of recent recipe cards |
| Recently Added "View all" | HyperlinkButton | — | Navigates to Search page with recently-added filter |
| Popular Creators | Section | — | Horizontal list of creator avatars with name and recipe count |
| Popular Creator card | Button | — | Navigates to that creator's Profile page |
| Favorite button | ToggleButton | — | Heart icon on each recipe card; toggles favorite state |

## Acceptance Criteria

### AC-1: Initial load
- **Given** the user has logged in
- **When** the Home page is displayed
- **Then** the Trending Now section loads with a horizontal list of recipe cards
- **And** the Categories section loads with category chips/cards
- **And** the Recently Added section loads with recipe cards
- **And** the Popular Creators section loads with creator profiles

### AC-2: Trending Now section
- **Given** the Home page is displayed
- **When** the Trending Now section is visible
- **Then** each card shows a recipe image, title, author name, cook time, and a favorite heart icon
- **And** the list scrolls horizontally

### AC-3: View all trending
- **Given** the Home page is displayed
- **When** the user taps "View all" next to Trending Now
- **Then** the app navigates to the Search page with results showing all trending recipes

### AC-4: Categories section
- **Given** the Home page is displayed
- **When** the Categories section is visible
- **Then** each category shows an image, name, and recipe count
- **And** the list scrolls horizontally

### AC-5: Category navigation
- **Given** the Home page is displayed
- **When** the user taps a category card
- **Then** the app navigates to the Search page filtered by that category

### AC-6: Recently Added section
- **Given** the Home page is displayed
- **When** the Recently Added section is visible
- **Then** each card shows a recipe image, title, author name, cook time, and a favorite heart
- **And** the list scrolls horizontally

### AC-7: View all recently added
- **Given** the Home page is displayed
- **When** the user taps "View all" next to Recently Added
- **Then** the app navigates to the Search page showing recently added recipes

### AC-8: Popular Creators section
- **Given** the Home page is displayed
- **When** the Popular Creators section is visible
- **Then** each card shows a creator avatar, name, and recipe count
- **And** tapping a creator navigates to their Profile page

### AC-9: Favorite toggle on recipe card
- **Given** a recipe card is displayed with a heart icon
- **When** the user taps the heart icon
- **Then** the recipe is added to (or removed from) favorites
- **And** the heart icon fill state updates to reflect the new status

### AC-10: Navigation bar — profile
- **Given** the Home page is displayed
- **When** the user taps the profile avatar in the navigation bar
- **Then** the app navigates to the current user's Profile page

### AC-11: Navigation bar — notifications
- **Given** the Home page is displayed
- **When** the user taps the notification bell icon
- **Then** the app navigates to the Notifications page

### AC-12: Vertical scrolling
- **Given** the Home page is displayed
- **When** the user scrolls down
- **Then** all sections (Trending, Categories, Recently Added, Popular Creators) are accessible via vertical scroll
