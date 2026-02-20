# Feature: Registration

**Page:** `RegistrationPage`
**ViewModel:** `RegistrationModel`
**Route:** `/Register`

## Description

The Registration page allows new users to create an account by providing a username, email, and password. On successful registration, the user is navigated to the Main page.

## UI Elements

| Element | Type | Description |
|---|---|---|
| Logo | Image | Chefs logo with icon (theme-aware) |
| Username field | TextBox | Username input with person icon |
| Email field | TextBox | Email input with mail icon (`RegistrationEmail`) |
| Password field | PasswordBox | Password input with lock icon (`RegistrationPassword`) |
| Sign Up button | Button | Submits registration |
| Login Now link | Button | Navigates to Login page (`-/Login`) |

## Acceptance Criteria

### AC-1: Initial state
- **Given** the user navigates to the Registration page
- **When** the page loads
- **Then** username, email, and password fields are empty
- **And** the Sign Up button is visible
- **And** a "Login Now" link is visible at the bottom

### AC-2: Successful registration
- **Given** the user has entered a username and password
- **When** the user taps "Sign Up"
- **Then** authentication is performed with the provided credentials
- **And** on success, the app navigates to the Main page

### AC-3: Navigate to Login
- **Given** the Registration page is displayed
- **When** the user taps "Login Now"
- **Then** the app navigates to the Login page (`-/Login`)

### AC-4: Tab order
- **Given** the Registration page is displayed
- **When** the user presses Tab/Next in the username field
- **Then** focus moves to the email field
- **When** the user presses Tab/Next in the email field
- **Then** focus moves to the password field

### AC-5: Return key in password field submits
- **Given** the user has filled in credentials
- **When** the user presses Return/Done in the password field
- **Then** the Register command is executed

### AC-6: Page is scrollable
- **Given** the Registration page is displayed on a small screen
- **When** the keyboard opens or content exceeds viewport
- **Then** the page scrolls vertically to show all fields
