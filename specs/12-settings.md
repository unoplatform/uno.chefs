# Feature: Settings Page

**Page:** `SettingsPage`
**ViewModel:** `SettingsModel`
**Route:** `/Settings` (modal)

## Description

The Settings page allows the user to view and edit personal information (name, email, mobile number), toggle push notifications, and change the app theme (System, Light, Dark).

## UI Elements

| Element | Type | Description |
|---|---|---|
| Back/Close button | Button | Returns to the Profile page |
| Page title | TextBlock | "Settings" |
| Personal Information section | Section | Header for user details |
| Name field | TextBox | Editable full name |
| Email field | TextBox | Editable email address |
| Mobile field | TextBox | Editable phone number |
| Application section | Section | Header for app preferences |
| Notifications toggle | ToggleSwitch | Enable/disable push notifications |
| Theme dropdown | ComboBox | Options: "System default", "Light", "Dark" |

## Acceptance Criteria

### AC-1: Initial load
- **Given** the user navigates to the Settings page
- **When** the page loads
- **Then** the name, email, and mobile fields are populated with the current user's data
- **And** the notifications toggle reflects the current notification setting
- **And** the theme dropdown reflects the current theme

### AC-2: Edit name
- **Given** the Settings page is displayed
- **When** the user modifies the value in the Name field
- **Then** the user's profile is updated with the new name

### AC-3: Edit email
- **Given** the Settings page is displayed
- **When** the user modifies the value in the Email field
- **Then** the user's profile is updated with the new email

### AC-4: Edit mobile number
- **Given** the Settings page is displayed
- **When** the user modifies the value in the Mobile field
- **Then** the user's profile is updated with the new phone number

### AC-5: Toggle notifications
- **Given** the Settings page is displayed
- **When** the user toggles the Notifications switch
- **Then** the notification preference is persisted
- **And** the toggle state reflects the new value on next visit

### AC-6: Change theme to Light
- **Given** the theme is set to "System default"
- **When** the user selects "Light" from the theme dropdown
- **Then** the app immediately applies the light theme
- **And** the selection persists on next app launch

### AC-7: Change theme to Dark
- **Given** the theme is set to "System default"
- **When** the user selects "Dark" from the theme dropdown
- **Then** the app immediately applies the dark theme
- **And** the selection persists on next app launch

### AC-8: Change theme to System default
- **Given** the theme is set to "Light" or "Dark"
- **When** the user selects "System default" from the theme dropdown
- **Then** the app theme follows the OS setting
- **And** the selection persists on next app launch

### AC-9: Navigate back
- **Given** the Settings page is displayed
- **When** the user taps the back/close button
- **Then** the app returns to the Profile page
