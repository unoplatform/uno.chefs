# Feature: Responsive Layout

**Applies to:** All pages
**Mechanism:** `{utu:Responsive Wide=..., Normal=...}` markup extension

## Description

The app adapts its layout based on window width, providing optimized experiences for wide (desktop/tablet landscape) and narrow (phone/tablet portrait) viewports. Key differences include tab bar orientation, content column layout, and element sizing.

## Breakpoints

| Name | Condition | Typical Use |
|---|---|---|
| Wide | Window width ≥ wide threshold | Desktop, tablet landscape |
| Normal | Window width < wide threshold | Phone, tablet portrait |

## Per-Page Responsive Behavior

| Page | Wide Layout | Normal Layout |
|---|---|---|
| **Welcome** | Side-by-side: FlipView on left, onboarding content on right | Stacked: FlipView above, content below |
| **Login** | Side-by-side: image/brand on left, form on right | Stacked: form only |
| **Registration** | Side-by-side: image/brand on left, form on right | Stacked: form only |
| **Main (Shell)** | Vertical tab bar on the left side | Bottom tab bar |
| **Home** | Wider cards, more items per row | Smaller cards, fewer per row |
| **Search** | Multi-column grid (3-4 columns) | 2-column grid |
| **Recipe Details** | Two-column layout: content left, stats sidebar right | Stacked: image → content → stats |
| **Favorites** | Multi-column grid | 2-column grid |
| **Profile** | Wider layout with horizontal stats | Stacked vertical layout |
| **Notifications** | Centered content with max-width | Full-width list |
| **Settings** | Centered form with max-width | Full-width form |

## Acceptance Criteria

### AC-1: Wide layout — tab bar orientation
- **Given** the app window is at or above the wide breakpoint
- **When** the Main page is displayed
- **Then** the tab bar is rendered vertically on the left side

### AC-2: Normal layout — tab bar orientation
- **Given** the app window is below the wide breakpoint
- **When** the Main page is displayed
- **Then** the tab bar is rendered horizontally at the bottom

### AC-3: Welcome page — wide layout
- **Given** the window is wide
- **When** the Welcome page is displayed
- **Then** the FlipView images appear on the left and onboarding text on the right in a side-by-side layout

### AC-4: Welcome page — normal layout
- **Given** the window is narrow
- **When** the Welcome page is displayed
- **Then** the FlipView images appear above the onboarding text in a stacked layout

### AC-5: Login page — wide layout
- **Given** the window is wide
- **When** the Login page is displayed
- **Then** a brand image/panel appears on the left and the login form on the right

### AC-6: Login page — normal layout
- **Given** the window is narrow
- **When** the Login page is displayed
- **Then** only the login form is visible (stacked layout)

### AC-7: Recipe Details — wide layout
- **Given** the window is wide
- **When** the Recipe Details page is displayed
- **Then** recipe content is on the left and the stats sidebar (time, difficulty, calories) is on the right

### AC-8: Recipe Details — normal layout
- **Given** the window is narrow
- **When** the Recipe Details page is displayed
- **Then** the hero image, content, and stats are stacked vertically

### AC-9: Search/Favorites grid density
- **Given** the window width changes from wide to narrow (or vice versa)
- **When** the Search or Favorites grid is visible
- **Then** the number of columns adjusts to fit the available width

### AC-10: Resize during use
- **Given** the user is actively using the app on desktop
- **When** the window is resized across the breakpoint
- **Then** the layout transitions smoothly between wide and normal modes
- **And** no content is lost or overlapping
