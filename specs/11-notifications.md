# Feature: Notifications Page

**Page:** `NotificationsPage`
**ViewModel:** `NotificationsModel`
**Route:** `/Notifications` (modal)

## Description

The Notifications page displays the user's notifications grouped by time period (Today, Yesterday, Older). Notifications can be filtered by All, Unread, or Read via a top tab bar. Each notification shows the sender's avatar, message text, time, and read/unread visual state.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Close button | Button | Dismisses the notifications modal |
| Top tab bar | TopTabBar | Tabs: "All", "Unread", "Read" |
| Group header | TextBlock | Time group label: "Today", "Yesterday", "Older" |
| Notification item | ListItem | Avatar, sender name, message, time, read/unread indicator |
| Unread indicator | Visual | Bold text or dot indicating an unread notification |
| Empty state | Panel | Shown when there are no notifications in the selected filter |

## Acceptance Criteria

### AC-1: Initial state — All tab
- **Given** the user opens the Notifications page
- **When** the page loads
- **Then** the "All" tab is selected by default
- **And** all notifications are shown, grouped by Today/Yesterday/Older

### AC-2: Group headers
- **Given** the All tab is active
- **When** notifications span multiple time periods
- **Then** group headers "Today", "Yesterday", and/or "Older" separate the items

### AC-3: Notification item display
- **Given** notifications are loaded
- **When** a notification item is visible
- **Then** it shows the sender's avatar, name, message text, and timestamp
- **And** unread notifications are visually distinct from read ones (e.g., bold text or dot indicator)

### AC-4: Unread tab
- **Given** the Notifications page is displayed
- **When** the user selects the "Unread" tab
- **Then** only unread notifications are shown

### AC-5: Read tab
- **Given** the Notifications page is displayed
- **When** the user selects the "Read" tab
- **Then** only read notifications are shown

### AC-6: Empty state
- **Given** there are no notifications matching the current filter
- **When** the selected tab's content area is visible
- **Then** an empty state illustration and message are displayed

### AC-7: Close notifications
- **Given** the Notifications page is displayed as a modal
- **When** the user taps the close (X) button
- **Then** the modal closes and the app returns to the previous page

### AC-8: Notification count consistency
- **Given** the user has N unread notifications
- **When** the Unread tab is selected
- **Then** exactly N notification items are displayed
