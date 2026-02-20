# Feature: Map Page

**Page:** `MapPage`
**ViewModel:** `MapModel`
**Route:** `/Main/Map`

## Description

The Map page displays an interactive map (powered by Mapsui) showing nearby points of interest related to recipes or food experiences. It is accessible from the Home page via a "Near me" action.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Map control | MapControl (Mapsui) | Interactive map with pan, zoom, and tile layers |
| Back button | Button | Returns to the previous page |
| Navigation bar | NavigationBar | Top bar with "Near me" title and back navigation |

## Acceptance Criteria

### AC-1: Map loads
- **Given** the user navigates to the Map page
- **When** the page loads
- **Then** an interactive map is displayed with tile layers rendered
- **And** the map is centered on a default location

### AC-2: Map interactions
- **Given** the map is displayed
- **When** the user pinches to zoom or drags to pan
- **Then** the map responds with zoom and pan gestures

### AC-3: Back navigation
- **Given** the Map page is displayed
- **When** the user taps the back button
- **Then** the app returns to the Home page or previous page
