# Feature: Profile Page

**Page:** `ProfilePage`
**ViewModel:** `ProfileModel`
**Route:** `/Main/Profile` (modal)

## Description

The Profile page displays a user's avatar, name, description, stats (recipes created, followers, following), and their published recipes. When viewing the current user's own profile, a settings gear icon is available.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Close button | Button | Dismisses the profile modal and returns to previous page |
| Settings button | Button | Gear icon; navigates to Settings (only for current user) |
| Avatar | Image | User profile picture |
| User name | TextBlock | Full name of the user |
| User description | TextBlock | Bio or description text |
| Recipes count | TextBlock | Number of recipes created by the user |
| Followers count | TextBlock | Number of followers |
| Following count | TextBlock | Number of users being followed |
| My Recipes grid | GridView | Grid of recipes created by this user |
| Empty state | Panel | "No Recipes Created" illustration and message |

## Acceptance Criteria

### AC-1: Current user profile
- **Given** the user navigates to their own Profile page
- **When** the page loads
- **Then** the avatar, name, and description are displayed
- **And** the recipes/followers/following counts are shown
- **And** a settings gear icon is visible in the header

### AC-2: Other user's profile
- **Given** the user navigates to another user's Profile page (e.g., from Popular Creators or Recipe author)
- **When** the page loads
- **Then** that user's avatar, name, description, and stats are displayed
- **And** the settings gear icon is NOT visible

### AC-3: My Recipes — with recipes
- **Given** the profile user has published recipes
- **When** the profile page is displayed
- **Then** a "My Recipes" section shows a grid of their recipe cards

### AC-4: My Recipes — empty state
- **Given** the profile user has no published recipes
- **When** the profile page is displayed
- **Then** a "No Recipes Created" empty state with illustration is shown

### AC-5: Navigate to recipe
- **Given** the My Recipes grid is visible
- **When** the user taps a recipe card
- **Then** the app navigates to the Recipe Details page for that recipe

### AC-6: Navigate to settings
- **Given** the user is viewing their own profile
- **When** the user taps the settings gear icon
- **Then** the app navigates to the Settings page

### AC-7: Close profile
- **Given** the Profile page is displayed as a modal
- **When** the user taps the close (X) button
- **Then** the modal closes and the app returns to the previous page

### AC-8: Stats display
- **Given** the profile is loaded
- **When** the stats section is visible
- **Then** "Recipes", "Followers", and "Following" are each labeled and show numeric values
