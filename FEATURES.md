# Uno.Chefs Features

Uno.Chefs is a sample application for showcasing the capabilities of the Uno Platform for building cross-platform apps. Here is a breakdown of features used on individual pages:

## LoginPage

| **Component/Helper**     | **Category**  | **Description**                                                                                                                                                               |
| ------------------------ | ------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ScrollViewer**         | Layout        | Used to provide vertical scrolling for content.                                                                                                                               |
| **Image**                | Visual        | Used to display the SVG logo with the use of **Uno.Resizetizer**.                                                                                                             |
| **TextBox**              | Input Control | User input for the username. Includes placeholder text, style customizations, and icon integration.                                                                           |
| **PasswordBox**          | Input Control | Secure input for the password. Custom styling and resources are used to align with the app's design.                                                                          |
| **utu:StatusBar**        | Uno.Toolkit   | Customization of the status bar using **Uno.Toolkit**. Enables dynamic theme adjustments (background, foreground).                                                            |
| **utu:AutoLayout**       | Uno.Toolkit   | Flexible layout component from **Uno.Toolkit*** to handle spacing, alignment, and responsiveness. Used to arrange controls in a clean, adaptable layout between device types. |
| **utu:Divider**          | Uno.Toolkit   | A simple horizontal divider used to separate sections of the UI.                                                                                                              |
| **ut:ControlExtensions** | Uno.Themes    | Allows controls like `TextBox` and `PasswordBox` to include icons. A reusable pattern that applies `Icon` properties to input fields for improved UX consistency.             |
| **LightweightStyling**   | Theming       | A theme customization mechanism applied to controls, ensuring consistent styling across the app (e.g., `PasswordBox` and buttons).                                            |
| **CheckBox**             | Input Control | Provides a "Remember me" option for persisting login information.                                                                                                             |
| **Button**               | Interaction   | Various button controls for user actions like login and third-party authentication. Includes icon support via ControlExtensions.                                              |
| **TextBlock**            | Text Display  | Displays a message for non-members. Simple text, styled to fit the overall page design.                                                                                       |
| **Navigation Button**    | Navigation    | Button for page navigation, bound to the app's navigation system with **Uno.Extensions**.                                                                                     |

## MainPage

| **Component/Helper**           | **Category**   | **Description**                                                                                                                                           |
| ------------------------------ | -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Grid**                       | Layout         | The main layout container for organizing content in rows and columns. Implements a responsive structure that changes based on screen size and layout.     |
| **uen:Region.Attached**        | Uno.Extensions | A navigation feature from **Uno.Extensions.Navigation** that allows dynamic region-based navigation, enabling smooth transitions between different pages. |
| **utu:AutoLayout**             | Uno.Toolkit    | Helper layout component from **Uno.Toolkit** to arrange elements and manage responsive behavior (stretching and collapsing based on screen size).         |
| **utu:TabBar**                 | Uno.Toolkit    | Bottom and vertical navigation tabs. Uses `TabBarItem` for different sections (Home, Search, Favorites) and icons to guide users visually.                |
| **utu:TabBarItem**             | Uno.Toolkit    | Represents each individual tab in the **TabBar** for different app sections. Integrates icons like `Icon_Home`, `Icon_Search`, and `Icon_Heart`.          |
| **PathIcon**                   | Visual         | Icons used within the `TabBarItem` to visually represent sections (Home, Search, Favorites). These icons are sourced from theme resources.                |
| **utu:Responsive**             | Uno.Toolkit    | The page adapts to screen size, showing the bottom tab bar on narrow screens and the vertical tab bar on wide screens.                                    |
| **SurfaceBrush ThemeResource** | Styling        | Theme resources like `BackgroundBrush` and `SurfaceBrush` are used to provide a consistent background and surface color for the application.              |

## HomePage

| **Component/Helper**       | **Category**     | **Description**                                                                                                                                                      |
| -------------------------- | ---------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **utu:AutoLayout**         | Uno.Toolkit      | Primary layout helper used extensively to arrange components responsively throughout the page.                                                                       |
| **utu:StatusBar**          | Uno.Toolkit      | Sets the background and foreground of the status bar, adjusting to the theme resources for contrast and visibility.                                                  |
| **utu:NavigationBar**      | Uno.Toolkit      | Displays a navigation bar with primary commands (e.g., Profile, Notifications) and branding content (e.g., Chefs app logo).                                          |
| **uer:FeedView**           | Uno.Extensions   | Reactive feed component from **Uno.Extensions.Reactive.UI** used for dynamic data sources like Trending Now, Categories, Recently Added, and Popular Contributors.   |
| **ScrollViewer**           | Layout           | Enables horizontal and vertical scrolling for different sections like trending, categories, and popular contributors.                                                |
| **muxc:ItemsRepeater**     | Data Layout      | A performant control for displaying repeated items (e.g., trending items, categories, contributors) with custom layouts defined by `StackLayout`.                    |
| **utu:CardContentControl** | Uno.Toolkit      | Used to display data such as recipes and contributors, styled with the `FilledCardContentControlStyle` for visual consistency and layout structure.                  |
| **ToggleButton**           | Interaction      | Allows users to favorite a recipe. The state (checked/unchecked) binds to whether a recipe is a favorite. Icons toggle between `Icon_Heart` and `Icon_Heart_Filled`. |
| **PathIcon**               | Visual           | Icons used for various components, including user profile, notifications, favorite status, and recipe categories.                                                    |
| **utu:Responsive**         | Uno.Toolkit      | Utilizes responsive layout features to adapt the orientation of controls (horizontal/vertical) and text alignment based on screen size.                              |
| **Button**                 | Interaction      | Buttons for actions such as "View all" or navigating to the map. Uses `TextButtonStyle` for visual consistency and style across the app.                             |
| **DataTemplate**           | Data Template    | Templates for repeating data in feeds, such as Trending Now and Categories, defining how each item in a list is rendered (image, text, and actions).                 |
| **Image**                  | Visual           | Displays images for items like recipes, categories, and contributors. Images are styled for uniform alignment and aspect ratio control.                              |
| **TextBlock**              | Text Display     | Displays textual data like recipe names, categories, and contributor information. Text style and wrapping are customized based on themes and layout.                 |
| **Border**                 | Visual Structure | Used to encapsulate images and other elements with padding and corner radius settings, contributing to the card-based UI appearance.                                 |
