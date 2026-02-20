# Feature: Navigation & Shell

**Controls:** `ShellControl`, `MainPage`
**ViewModels:** `ShellModel`, `MainModel`
**Routes:** Root shell → Welcome / Login / Register / Main (with nested tabs)

## Description

The app uses region-based navigation with a shell that manages the overall navigation flow. The main navigation structure includes a bottom tab bar (narrow layout) or vertical side tab bar (wide layout) with three primary destinations: Home, Search, and Favorites. Modal pages (Profile, Notifications, Settings, Filter) overlay the main content.

## Route Tree

```
Shell
├── Welcome
├── Login
├── Register
└── Main (nested)
    ├── Home (default)
    ├── Search
    ├── FavoriteRecipes
    ├── RecipeDetails
    ├── CookbookDetails
    ├── CreateCookbook
    ├── UpdateCookbook
    ├── LiveCooking
    └── Map
Modals:
├── Notifications
├── Filter
├── Profile
├── Settings
├── Completed (Live Cooking done)
└── Dialog (Generic)
```

## UI Elements

| Element | Type | Description |
|---|---|---|
| Tab bar (narrow) | TabBar | Bottom tab bar with Home, Search, Favorites icons |
| Tab bar (wide) | TabBar | Vertical side tab bar on the left with Home, Search, Favorites icons |
| Content region | Frame | Main content area where pages are rendered |
| Modal overlay | Frame | Overlay region for Profile, Notifications, Settings, etc. |

## Acceptance Criteria

### AC-1: App launch — Welcome
- **Given** the app is launched for the first time
- **When** the shell initializes
- **Then** the app navigates to the Welcome page
- **And** no tab bar is visible

### AC-2: Post-login — Main with tabs
- **Given** the user has logged in
- **When** the Main page loads
- **Then** the tab bar is visible (bottom for narrow, side for wide)
- **And** the Home tab is selected by default
- **And** the Home page content is displayed

### AC-3: Tab navigation — Search
- **Given** the Main page is displayed with tab bar
- **When** the user taps the Search tab
- **Then** the Search page is displayed in the content area
- **And** the Search tab is visually selected

### AC-4: Tab navigation — Favorites
- **Given** the Main page is displayed with tab bar
- **When** the user taps the Favorites tab
- **Then** the Favorites page is displayed in the content area
- **And** the Favorites tab is visually selected

### AC-5: Tab navigation — Home
- **Given** the user is on the Search or Favorites tab
- **When** the user taps the Home tab
- **Then** the Home page is displayed
- **And** the Home tab is visually selected

### AC-6: Deep navigation preserves tabs
- **Given** the user navigates from Home → Search → Recipe Details
- **When** the user opens Recipe Details
- **Then** the tab bar remains visible
- **And** the Search tab is still selected

### AC-7: Modal opens over content
- **Given** the user taps Profile, Notifications, or Settings
- **When** the modal page opens
- **Then** it overlays the main content
- **And** the underlying tab bar is hidden or not interactive

### AC-8: Modal close returns to content
- **Given** a modal page (Profile/Notifications/Settings) is open
- **When** the user closes the modal
- **Then** the app returns to the previously active tab and page

### AC-9: Login clears back stack
- **Given** the user logs in from the Login page
- **When** navigation to Main completes
- **Then** the back stack is cleared
- **And** the user cannot navigate back to Welcome or Login

### AC-10: No tab bar on auth pages
- **Given** the app is on Welcome, Login, or Registration
- **When** the page is displayed
- **Then** no tab bar is visible
