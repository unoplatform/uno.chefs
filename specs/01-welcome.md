# Feature: Welcome / Onboarding

**Page:** `WelcomePage`
**ViewModel:** `WelcomeModel`
**Route:** `/Welcome`
**Screenshot:** [welcome-page.png](screenshots/welcome-page.png)

## Description

The Welcome page is the first screen new users see when launching the app. It consists of a 3-page onboarding carousel (FlipView) that introduces the app's key features. Users can navigate through the pages or skip directly to the Login page.

## UI Elements

| Element | Type | Description |
|---|---|---|
| FlipView (narrow) | FlipView | Carousel of 3 welcome screens with image + text |
| FlipView (wide) | FlipView | Full-bleed splash images on the left half (wide layout only) |
| PipsPager | PipsPager | Page indicator dots below the carousel |
| Previous button | Button | Navigates to the previous onboarding page |
| Next button | Button | Navigates to the next onboarding page |
| Skip button | Button | Skips onboarding and navigates to Login (`AutomationId: SkipButton`) |

## Onboarding Pages

1. **Page 1:** "Welcome to your App!" — Introduction to coding journey with recipe metaphors
2. **Page 2:** "Explore Thousands of Recipes" — Recipe discovery and diversity
3. **Page 3:** "Personalize Your Recipe Journey" — Collections, cookbooks, and community

## Acceptance Criteria

### AC-1: Initial state
- **Given** the app launches for the first time
- **When** the Welcome page is displayed
- **Then** the first onboarding page (index 0) is shown
- **And** the PipsPager shows 3 dots with the first dot active
- **And** the "Previous" button is disabled
- **And** the "Next" button is enabled
- **And** a "Skip" button is visible

### AC-2: Navigate forward through pages
- **Given** the user is on Welcome page 1
- **When** the user taps "Next"
- **Then** the FlipView advances to page 2
- **And** the PipsPager updates to highlight the second dot
- **And** the "Previous" button becomes enabled

### AC-3: Navigate to last page
- **Given** the user is on Welcome page 2
- **When** the user taps "Next"
- **Then** the FlipView advances to page 3
- **And** the "Next" button is disabled (cannot move past last page)
- **And** the "Previous" button is enabled

### AC-4: Navigate backward
- **Given** the user is on Welcome page 2 or 3
- **When** the user taps "Previous"
- **Then** the FlipView returns to the previous page
- **And** the PipsPager updates accordingly

### AC-5: Skip to Login
- **Given** the user is on any Welcome page
- **When** the user taps the "Skip" button
- **Then** the app navigates to the Login page (`-/Login`)

### AC-6: Wide layout shows side-by-side images
- **Given** the app is in Wide responsive mode
- **When** the Welcome page is displayed
- **Then** a left panel shows full-bleed splash images synced with the FlipView index
- **And** the right panel shows the onboarding content with controls

### AC-7: Swipe gesture support
- **Given** the user is on any Welcome page
- **When** the user swipes left on the FlipView
- **Then** the FlipView advances to the next page (if available)
