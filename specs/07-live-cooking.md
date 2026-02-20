# Feature: Live Cooking Mode

**Page:** `LiveCookingPage`
**ViewModel:** `LiveCookingModel`
**Route:** `/Main/LiveCooking`

## Description

Live Cooking mode guides the user through a recipe step-by-step using a FlipView-style interface. Each step shows its description and optional video content. When the user completes all steps, a completion screen appears where they can rate and favorite the recipe.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Step indicator | TextBlock | Shows current step position (e.g., "Step 2/5") |
| Step description | TextBlock | Text description of the current cooking step |
| Video player | MediaPlayerElement | Displays a video for the current step (if available) |
| FlipView | FlipView | Swipeable view for navigating between steps |
| Back button | Button | Goes to the previous step |
| Next/Done button | Button | Moves to the next step, or marks cooking as complete on the last step |
| Close button | Button | Exits live cooking and returns to Recipe Details |
| Completion screen | Panel | Shown after all steps are done; includes rating and favorite options |
| Rating prompt | TextBlock | "Did you enjoy it?" with a feedback mechanism |
| Favorite button | ToggleButton | Heart icon on the completion screen |
| Back to last step | Button | Returns from completion screen to the final step |

## Acceptance Criteria

### AC-1: Initial state
- **Given** the user starts Live Cooking for a recipe
- **When** the page loads
- **Then** the first step is displayed with its description
- **And** the step indicator shows "Step 1/N" (where N is total steps)
- **And** a video player is visible (if the step has video content)

### AC-2: Navigate to next step
- **Given** the live cooking page shows step K
- **When** the user taps the "Next" button or swipes forward
- **Then** step K+1 is displayed
- **And** the step indicator updates to "Step K+1/N"

### AC-3: Navigate to previous step
- **Given** the live cooking page shows step K (K > 1)
- **When** the user taps the "Back" button or swipes backward
- **Then** step K-1 is displayed
- **And** the step indicator updates accordingly

### AC-4: First step — no back
- **Given** the user is on step 1
- **When** the back button is evaluated
- **Then** there is no previous step available (back is disabled or hidden)

### AC-5: Complete cooking
- **Given** the user is on the last step
- **When** the user taps "Done"
- **Then** the completion screen is displayed
- **And** a "Did you enjoy it?" prompt appears
- **And** a favorite heart icon is available

### AC-6: Favorite on completion
- **Given** the completion screen is displayed
- **When** the user taps the favorite heart icon
- **Then** the recipe's favorite status toggles

### AC-7: Back to last step from completion
- **Given** the completion screen is displayed
- **When** the user taps "Back to last step"
- **Then** the view returns to the final cooking step

### AC-8: Close live cooking
- **Given** the user is in Live Cooking mode (any step or completion)
- **When** the user taps the close/exit button
- **Then** the app navigates back to the Recipe Details page

### AC-9: Step video playback
- **Given** a step has associated video content
- **When** that step is displayed
- **Then** the video is loaded in the media player
- **And** the user can play/pause the video
