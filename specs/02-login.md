# Feature: Login

**Page:** `LoginPage`
**ViewModel:** `LoginModel`
**Route:** `/Login`
**Screenshot:** [login-page.png](screenshots/login-page.png)

## Description

The Login page allows users to authenticate using a username/password combination or via third-party providers (Apple, Google). After successful login, the user is navigated to the Main page with the back stack cleared.

## UI Elements

| Element | AutomationId | Type | Description |
|---|---|---|---|
| Logo | — | Image | Chefs logo with icon (theme-aware) |
| Username field | `LoginUsername` | TextBox | Username input with person icon |
| Password field | `LoginPassword` | PasswordBox | Password input with lock icon |
| Remember me | — | CheckBox | Binds to `UserCredentials.SaveCredentials` |
| Forgot password | — | Button | Placeholder (no action implemented) |
| Login button | `LoginButton` | Button | Submits username/password login |
| Divider | — | Divider | Visual separator between form and social login |
| Sign in with Apple | — | Button | Triggers Apple login flow |
| Sign in with Google | — | Button | Triggers Google login flow |
| Register Now | — | Button | Navigates to Registration page (`-/Register`) |

## Acceptance Criteria

### AC-1: Initial state
- **Given** the user navigates to the Login page
- **When** the page loads
- **Then** the username and password fields are empty
- **And** the Login button is disabled (command `CanLogin` returns false)
- **And** "Sign in with Apple" and "Sign in with Google" buttons are visible
- **And** a "Register Now" link is visible at the bottom

### AC-2: Login button enablement
- **Given** the Login page is displayed
- **When** the user enters a non-empty username AND a non-empty password
- **Then** the Login button becomes enabled

### AC-3: Login button remains disabled with incomplete input
- **Given** the Login page is displayed
- **When** the user enters only a username (password is empty) OR only a password (username is empty)
- **Then** the Login button remains disabled

### AC-4: Successful username/password login
- **Given** the user has entered valid credentials (username: any non-empty, password: any non-empty)
- **When** the user taps the Login button
- **Then** the `Authentication.LoginAsync` is called with the provided credentials
- **And** the app navigates to `/Main` with `ClearBackStack` qualifier
- **And** the Home page is displayed

### AC-5: Sign in with Apple
- **Given** the Login page is displayed
- **When** the user taps "Sign in with Apple"
- **Then** authentication is performed with username "AppleUser" and password "uno123"
- **And** the app navigates to `/Main` with `ClearBackStack` qualifier

### AC-6: Sign in with Google
- **Given** the Login page is displayed
- **When** the user taps "Sign in with Google"
- **Then** authentication is performed with username "GoogleUser" and password "uno123"
- **And** the app navigates to `/Main` with `ClearBackStack` qualifier

### AC-7: Navigate to Registration
- **Given** the Login page is displayed
- **When** the user taps "Register Now"
- **Then** the app navigates to the Registration page (`-/Register`)

### AC-8: Return key submits login
- **Given** the user has filled in both username and password
- **When** the user presses Return/Done in the password field
- **Then** the Login command is executed (same as tapping Login button)

### AC-9: Tab order
- **Given** the Login page is displayed
- **When** the user presses Tab/Next in the username field
- **Then** focus moves to the password field (via `AutoFocusNextElement`)

### AC-10: Back navigation is blocked after login
- **Given** the user successfully logged in
- **When** the user attempts to navigate back
- **Then** the back stack is empty (due to `ClearBackStack` qualifier)
- **And** the user remains on the Home page
