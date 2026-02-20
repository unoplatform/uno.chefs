# Feature: Recipe Sharing

**Service:** `IShareService` / `ShareService`
**Triggered from:** Recipe Details page (Share button)

## Description

Users can share a recipe's details via the system share sheet. The shared content includes the recipe title, full list of ingredients (with quantities), and step-by-step instructions formatted as plain text.

## Share Text Format

```
Recipe: {Title}

Ingredients:
- {Ingredient1.Name}: {Ingredient1.Quantity}
- {Ingredient2.Name}: {Ingredient2.Quantity}
...

Steps:
1. {Step1.Text}
2. {Step2.Text}
...
```

## Acceptance Criteria

### AC-1: Share button visible
- **Given** the user is on the Recipe Details page
- **When** the page is displayed
- **Then** a Share button/icon is visible in the header area

### AC-2: Share invokes system sheet
- **Given** the user is on the Recipe Details page
- **When** the user taps the Share button
- **Then** the system share sheet (or clipboard action on supported platforms) is invoked
- **And** the share content contains the recipe title

### AC-3: Share content includes ingredients
- **Given** the share action is triggered
- **When** the share text is generated
- **Then** all ingredients are listed with their name and quantity

### AC-4: Share content includes steps
- **Given** the share action is triggered
- **When** the share text is generated
- **Then** all cooking steps are listed in order with their descriptions

### AC-5: Share on non-Windows platforms
- **Given** the app is running on a platform other than Windows
- **When** the user taps share
- **Then** the recipe text is copied to the clipboard as a fallback
