# Feature: Recipe Details Page

**Page:** `RecipeDetailsPage`
**ViewModel:** `RecipeDetailsModel`
**Route:** `/Main/RecipeDetails`

## Description

The Recipe Details page presents comprehensive information about a single recipe, including its author, cook time, difficulty, calorie count, ingredients, step-by-step instructions, reviews, and nutritional breakdown. Users can favorite the recipe, share it, or start live cooking mode.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Hero image | Image | Large recipe photo at the top |
| Back button | Button | Returns to the previous page |
| Recipe title | TextBlock | Name of the recipe |
| Author section | Button | Author avatar and name; navigates to author's profile |
| Stats sidebar | Panel | Cook time, difficulty level, and calorie count |
| Tab bar | TopTabBar | Tabs: Ingredients, Steps, Reviews, Nutrition |
| Ingredients list | ListView | Ingredient name and quantity for each item |
| Steps list | ListView | Step number and description for each step |
| Reviews list | ListView | Reviewer avatar, name, review text, like/dislike buttons with counts |
| Nutrition panel | Panel | Nutritional info (calories, carbs, protein, vitamins, fat) |
| Favorite button | ToggleButton | Heart icon to toggle favorite status |
| Share button | Button | Shares recipe via system share sheet |
| Start Cooking FAB | Button | "Start Cooking!" floating action button; navigates to Live Cooking |

## Acceptance Criteria

### AC-1: Page load with recipe data
- **Given** the user navigates to Recipe Details for a specific recipe
- **When** the page loads
- **Then** the hero image, title, and author info are displayed
- **And** the stats sidebar shows cook time, difficulty, and calories
- **And** the Ingredients tab is selected by default

### AC-2: Ingredients tab
- **Given** the Ingredients tab is selected
- **When** the content area is visible
- **Then** a list of ingredients is displayed, each with name and quantity
- **And** the number of ingredients matches the recipe data

### AC-3: Steps tab
- **Given** the Recipe Details page is displayed
- **When** the user selects the "Steps" tab
- **Then** a numbered list of cooking steps is shown, each with a description

### AC-4: Reviews tab
- **Given** the Recipe Details page is displayed
- **When** the user selects the "Reviews" tab
- **Then** a list of reviews is shown, each with reviewer info, text, and like/dislike counts
- **And** the current user can like or dislike a review

### AC-5: Like a review
- **Given** the Reviews tab is displayed
- **When** the user taps the "like" button on a review
- **Then** the like count increments by 1
- **And** the button visual indicates the user has liked it

### AC-6: Dislike a review
- **Given** the Reviews tab is displayed
- **When** the user taps the "dislike" button on a review
- **Then** the dislike count increments by 1
- **And** the button visual indicates the user has disliked it

### AC-7: Nutrition tab
- **Given** the Recipe Details page is displayed
- **When** the user selects the "Nutrition" tab
- **Then** nutritional information is displayed (calories, carbs, protein, vitamins, fat)

### AC-8: Favorite toggle
- **Given** the Recipe Details page is displayed
- **When** the user taps the heart (favorite) button
- **Then** the recipe's favorite status toggles
- **And** the heart icon fill state updates

### AC-9: Share recipe
- **Given** the Recipe Details page is displayed
- **When** the user taps the Share button
- **Then** the system share sheet opens with the recipe title, ingredients, and steps

### AC-10: Start Cooking
- **Given** the Recipe Details page is displayed
- **When** the user taps "Start Cooking!"
- **Then** the app navigates to the Live Cooking page for that recipe

### AC-11: Navigate to author profile
- **Given** the Recipe Details page shows the author section
- **When** the user taps the author name or avatar
- **Then** the app navigates to that author's Profile page

### AC-12: Back navigation
- **Given** the user is on the Recipe Details page
- **When** the user taps the back button
- **Then** the app returns to the previous page (Search, Home, or Favorites)

### AC-13: Wide layout
- **Given** the app window is wide (≥ wide breakpoint)
- **When** the Recipe Details page is displayed
- **Then** the stats sidebar appears on the right side alongside the content
- **And** the hero image and content use a two-column layout
