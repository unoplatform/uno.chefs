# Uno.Chefs Widgets

Chefs contains many UI/UX patterns that we would like to offer as standalone widgets in the Hot Design Toolbox. Here is a comprehensive list of areas within Chefs we could turn into widgets :

- [Welcome carousel](#large-carousel)
- [Login form](#login-form)
- [HomePage TopBar](#general-navbar-with-notifications-and-more)
- [Settings](#settings)
- [Region switching TabBar](#app-region-switching-tabbar)
- [HomePage Item Carousels](#item-carousel)
- [Searchbar](#searchbar)
- [Search filters](#search-filters)
- [Responsive Grid Layout](#responsive-grid-layout)
- [Page region switching TabBar](#page-region-switching-tabbar)
- [Cookbook template](#cookbook-template)
- [Share function](#share-function)
- [Reviews](#reviews)

[Used styles](#styles-used)

## Welcome/Login

### Large Carousel

A large carousel widget, like the one on the WelcomePage with `Next` and `Previous` buttons. The image only `FlipView` is hidden when screen size is small.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Views/WelcomePage.xaml#L22-L181) is pretty much the whole WelcomePage. It's made of two `FlipView`s and then the buttons that control them. One `FlipView` is for the images and the other one is for the text.

```xml
<utu:AutoLayout Orientation="{utu:Responsive Narrow=Vertical, Wide=Horizontal}">
    <FlipView IsEnabled="False"
                Visibility="{utu:Responsive Narrow=Collapsed, Wide=Visible}"
                utu:AutoLayout.PrimaryAlignment="Stretch"
                SelectedIndex="{Binding Pages.CurrentIndex, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
        <FlipView.Items>
            <Image Source="ms-appx:///Assets/Welcome/first_splash_wide_screen.png"
                    Stretch="UniformToFill" />
            <Image Source="ms-appx:///Assets/Welcome/second_splash_wide_screen.png"
                    Stretch="UniformToFill" />
            <Image Source="ms-appx:///Assets/Welcome/third_splash_wide_screen.png"
                    Stretch="UniformToFill" />
        </FlipView.Items>
    </FlipView>

    <utu:AutoLayout utu:AutoLayout.PrimaryAlignment="Stretch">
        <FlipView x:Name="flipView"
                    utu:AutoLayout.PrimaryAlignment="Stretch"
                    Background="Transparent"
                    SelectedIndex="{Binding Pages.CurrentIndex, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
            <FlipView.Items>
                <!-- First onboarding page -->
                <utu:AutoLayout CounterAxisAlignment="Center"
                                Spacing="24">
                    <Image Visibility="{utu:Responsive Narrow=Visible,
                                                        Wide=Collapsed}"
                            MaxHeight="270"
                            Source="ms-appx:///Assets/Welcome/first_splash_screen.png"
                            Stretch="UniformToFill" />
                    <utu:AutoLayout PrimaryAxisAlignment="Center"
                                    CounterAxisAlignment="Center"
                                    utu:AutoLayout.PrimaryAlignment="Stretch"
                                    Spacing="24">
                        <Image Width="160"
                                Height="90"
                                Source="{ThemeResource ChefsLogoWithIcon}" />
                        <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleLarge}"
                                    Text="Welcome to Your App!"
                                    Padding="32,0"
                                    TextWrapping="Wrap" />
                        <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleMedium}"
                                    Text="Embark on a delightful coding journey as you discover, create, and share awesome script tailored to your app and project preferences."
                                    Padding="32,0"
                                    TextWrapping="Wrap" />
                    </utu:AutoLayout>
                </utu:AutoLayout>
                <!-- Second onboarding page -->
                <utu:AutoLayout CounterAxisAlignment="Center"
                                Spacing="24">
                    <Image Visibility="{utu:Responsive Narrow=Visible,
                                                        Wide=Collapsed}"
                            utu:AutoLayout.PrimaryAlignment="Stretch"
                            MaxHeight="270"
                            Source="ms-appx:///Assets/Welcome/second_splash_screen.png"
                            Stretch="UniformToFill" />
                    <utu:AutoLayout PrimaryAxisAlignment="Center"
                                    CounterAxisAlignment="Center"
                                    utu:AutoLayout.PrimaryAlignment="Stretch"
                                    Spacing="24">
                        <Image Width="160"
                                Height="90"
                                utu:AutoLayout.CounterAlignment="Center"
                                Source="{ThemeResource ChefsLogoWithIcon}" />
                        <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleLarge}"
                                    Text="Explore Thousands of Recipes"
                                    Padding="32,0"
                                    TextWrapping="Wrap" />
                        <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleMedium}"
                                    Text="Find your next culinary adventure or last minute lunch from our vast collection of diverse and mouth-watering recipes."
                                    Padding="32,0"
                                    TextWrapping="Wrap" />
                    </utu:AutoLayout>
                </utu:AutoLayout>
                <!-- Third onboarding page -->
                <utu:AutoLayout CounterAxisAlignment="Center"
                                Spacing="24">
                    <Image Visibility="{utu:Responsive Narrow=Visible,
                                                        Wide=Collapsed}"
                            utu:AutoLayout.PrimaryAlignment="Stretch"
                            MaxHeight="270"
                            Source="ms-appx:///Assets/Welcome/third_splash_screen.png"
                            Stretch="UniformToFill" />
                    <utu:AutoLayout PrimaryAxisAlignment="Center"
                                    CounterAxisAlignment="Center"
                                    utu:AutoLayout.PrimaryAlignment="Stretch"
                                    Spacing="24">
                        <Image Width="160"
                                Height="90"
                                Source="{ThemeResource ChefsLogoWithIcon}" />
                        <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleLarge}"
                                    Text="Personalize Your Recipe Journey"
                                    Padding="32,0"
                                    TextWrapping="Wrap" />
                        <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleMedium}"
                                    Text="Create your own recipe collections, cookbooks, follow other foodies, and share your creations with the Chefs community."
                                    Padding="32,0"
                                    TextWrapping="Wrap" />
                    </utu:AutoLayout>
                </utu:AutoLayout>
            </FlipView.Items>
        </FlipView>
        <utu:AutoLayout Padding="32,0,32,15"
                        PrimaryAxisAlignment="End"
                        Spacing="16">
            <!-- Pips -->
            <muxc:PipsPager x:Name="PipsPager"
                            Margin="0,0,0,10"
                            utu:AutoLayout.CounterAlignment="Center"
                            NumberOfPages="3"
                            MaxVisiblePips="3"
                            Orientation="Horizontal"
                            SelectedPageIndex="{Binding Pages.CurrentIndex, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                            Style="{StaticResource PipsPagerStyle}" />

            <!-- Buttons -->
            <utu:AutoLayout Spacing="16"
                            PrimaryAxisAlignment="Center">
                <utu:AutoLayout Spacing="16"
                                PrimaryAxisAlignment="Center"
                                CounterAxisAlignment="Center"
                                Orientation="Horizontal">
                    <Button HorizontalContentAlignment="Center"
                            VerticalContentAlignment="Center"
                            Content="Previous"
                            utu:AutoLayout.PrimaryAlignment="Stretch"
                            utu:FlipViewExtensions.Previous="{Binding ElementName=flipView}"
                            Style="{StaticResource ChefsOutlinedButtonStyle}"
                            IsEnabled="{Binding Pages.Value.CanMovePrevious}" />
                    <Button HorizontalContentAlignment="Center"
                            VerticalContentAlignment="Center"
                            Content="Next"
                            utu:FlipViewExtensions.Next="{Binding ElementName=flipView}"
                            utu:AutoLayout.PrimaryAlignment="Stretch"
                            Style="{StaticResource ChefsPrimaryButtonStyle}"
                            IsEnabled="{Binding Pages.Value.CanMoveNext}" />
                </utu:AutoLayout>
                <Button HorizontalContentAlignment="Center"
                        VerticalContentAlignment="Center"
                        Content="Skip"
                        Padding="12,20"
                        uen:Navigation.Request="-/Login"
                        Foreground="{ThemeResource PrimaryBrush}"
                        CornerRadius="4"
                        Style="{StaticResource TextButtonStyle}" />
            </utu:AutoLayout>
        </utu:AutoLayout>
    </utu:AutoLayout>
</utu:AutoLayout>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Presentation/WelcomeModel.cs#L5) only has the iterator state.

```csharp
public IState<IntIterator> Pages => State<IntIterator>.Value(this, () => new IntIterator(Enumerable.Range(0, 3).ToImmutableList()));
```

![WelcomePage FlipViews](/doc/assets/welcome-page.png)

### Login form

A login/register widget, would allow the user to enter a username/e-mail and password.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Views/LoginPage.xaml#L18-L114) is a standard login page with username and password fields.

```xml
<utu:AutoLayout Spacing="32"
                MaxWidth="500"
                PrimaryAxisAlignment="Center"
                Padding="32">
    <Image utu:AutoLayout.CounterAlignment="Center"
            Width="160"
            Height="90"
            Source="{ThemeResource ChefsLogoWithIcon}"
            Stretch="Uniform" />
    <utu:AutoLayout Spacing="16"
                    PrimaryAxisAlignment="Center">
        <TextBox PlaceholderText="Username"
                    Style="{StaticResource ChefsPrimaryTextBoxStyle}"
                    utu:InputExtensions.ReturnType="Next"
                    utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=LoginPassword}"
                    IsSpellCheckEnabled="False"
                    Text="{Binding UserCredentials.Username, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
            <ut:ControlExtensions.Icon>
                <PathIcon Data="{StaticResource Icon_Person_Outline}" />
            </ut:ControlExtensions.Icon>
        </TextBox>
        <PasswordBox x:Name="LoginPassword"
                        utu:InputExtensions.ReturnType="Done"
                        utu:CommandExtensions.Command="{Binding Login}"
                        Password="{Binding UserCredentials.Password, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                        PlaceholderText="Password"
                        Style="{StaticResource OutlinedPasswordBoxStyle}"
                        BorderBrush="{ThemeResource OutlineVariantBrush}">
            <PasswordBox.Resources>
                <ResourceDictionary>
                    <ResourceDictionary.ThemeDictionaries>
                        <ResourceDictionary x:Key="Light">
                            <StaticResource x:Key="OutlinedPasswordBoxPlaceholderForeground" ResourceKey="OnSurfaceMediumBrush" />
                            <x:String x:Key="WorkAroundDummy">WorkAroundDummy</x:String>
                        </ResourceDictionary>
                        <ResourceDictionary x:Key="Default">
                            <StaticResource x:Key="OutlinedPasswordBoxPlaceholderForeground" ResourceKey="OnSurfaceMediumBrush" />
                            <x:String x:Key="WorkAroundDummy">WorkAroundDummy</x:String>
                        </ResourceDictionary>
                    </ResourceDictionary.ThemeDictionaries>
                </ResourceDictionary>
            </PasswordBox.Resources>
            <ut:ControlExtensions.Icon>
                <PathIcon Data="{StaticResource Icon_Lock}" />
            </ut:ControlExtensions.Icon>
        </PasswordBox>
        <utu:AutoLayout Spacing="24"
                        Orientation="Horizontal"
                        CounterAxisAlignment="Center"
                        Justify="SpaceBetween"
                        PrimaryAxisAlignment="Stretch">
            <CheckBox Content="Remember me"
                        utu:AutoLayout.PrimaryAlignment="Auto"
                        IsChecked="{Binding UserCredentials.SaveCredentials, Mode=TwoWay}" />
            <Button Content="Forgot password?"
                    Style="{StaticResource TextButtonStyle}" />
        </utu:AutoLayout>
        <Button Content="Login"
                Style="{StaticResource ChefsPrimaryButtonStyle}"
                Command="{Binding Login}" />
    </utu:AutoLayout>

    <utu:Divider Style="{StaticResource DividerStyle}" />

    <utu:AutoLayout Spacing="8"
                    PrimaryAxisAlignment="Center">
        <Button Content="Sign in with Apple"
                Style="{StaticResource ChefsTonalButtonStyle}">
            <ut:ControlExtensions.Icon>
                <FontIcon Style="{StaticResource FontAwesomeBrandsFontIconStyle}"
                            Glyph="{StaticResource Icon_Apple_Brand}"
                            FontSize="18"
                            Foreground="{ThemeResource OnSurfaceBrush}" />
            </ut:ControlExtensions.Icon>
        </Button>
        <Button Content="Sign in with Google"
                Style="{StaticResource ChefsTonalButtonStyle}">
            <ut:ControlExtensions.Icon>
                <FontIcon Style="{StaticResource FontAwesomeBrandsFontIconStyle}"
                            Glyph="{StaticResource Icon_Google_Brand}"
                            FontSize="18"
                            Foreground="{ThemeResource OnSurfaceBrush}" />
            </ut:ControlExtensions.Icon>
        </Button>
    </utu:AutoLayout>
    <utu:AutoLayout PrimaryAxisAlignment="Center"
                    CounterAxisAlignment="Center"
                    Orientation="Horizontal"
                    Spacing="4">
        <TextBlock Text="Not a member?"
                    Foreground="{ThemeResource OnSurfaceBrush}"
                    Style="{StaticResource LabelLarge}" />
        <Button Content="Register Now"
                uen:Navigation.Request="-/Register"
                Style="{StaticResource TextButtonStyle}" />
    </utu:AutoLayout>
</utu:AutoLayout>
```

- The [RegistrationPage](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Views/RegistrationPage.xaml#L18-L91) is separate in Chefs.

```xml
<utu:AutoLayout Padding="32"
                MaxWidth="500"
                PrimaryAxisAlignment="Center"
                Spacing="32">
    <Image Source="{ThemeResource ChefsLogoWithIcon}"
            Stretch="Uniform"
            utu:AutoLayout.CounterAlignment="Center"
            Width="160"
            Height="90" />
    <utu:AutoLayout Spacing="16">
        <TextBox PlaceholderText="Username"
                    Style="{StaticResource ChefsPrimaryTextBoxStyle}"
                    utu:InputExtensions.ReturnType="Next"
                    utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=RegistrationEmail}"
                    InputScope="Text"
                    IsSpellCheckEnabled="False"
                    Text="{Binding Credentials.Username, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
            <ut:ControlExtensions.Icon>
                <PathIcon Data="{StaticResource Icon_Person_Outline}" />
            </ut:ControlExtensions.Icon>
        </TextBox>
        <TextBox PlaceholderText="Email"
                    Style="{StaticResource ChefsPrimaryTextBoxStyle}"
                    x:Name="RegistrationEmail"
                    utu:InputExtensions.ReturnType="Next"
                    utu:InputExtensions.AutoFocusNextElement="{Binding ElementName=RegistrationPassword}"
                    IsSpellCheckEnabled="False">
            <ut:ControlExtensions.Icon>
                <PathIcon Data="{StaticResource Icon_Mail_Outline}" />
            </ut:ControlExtensions.Icon>
        </TextBox>
        <PasswordBox x:Name="RegistrationPassword"
                        utu:InputExtensions.ReturnType="Done"
                        utu:CommandExtensions.Command="{Binding Register}"
                        Password="{Binding Credentials.Password, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                        PlaceholderText="Password"
                        Style="{StaticResource OutlinedPasswordBoxStyle}"
                        BorderBrush="{ThemeResource OutlineVariantBrush}">
            <PasswordBox.Resources>
                <ResourceDictionary>
                    <ResourceDictionary.ThemeDictionaries>
                        <ResourceDictionary x:Key="Light">
                            <StaticResource x:Key="OutlinedPasswordBoxPlaceholderForeground" ResourceKey="OnSurfaceMediumBrush" />
                            <x:String x:Key="WorkAroundDummy">WorkAroundDummy</x:String>
                        </ResourceDictionary>
                        <ResourceDictionary x:Key="Default">
                            <StaticResource x:Key="OutlinedPasswordBoxPlaceholderForeground" ResourceKey="OnSurfaceMediumBrush" />
                            <x:String x:Key="WorkAroundDummy">WorkAroundDummy</x:String>
                        </ResourceDictionary>
                    </ResourceDictionary.ThemeDictionaries>
                </ResourceDictionary>
            </PasswordBox.Resources>
            <ut:ControlExtensions.Icon>
                <PathIcon Data="{StaticResource Icon_Lock}" />
            </ut:ControlExtensions.Icon>
        </PasswordBox>
        <Button HorizontalContentAlignment="Center"
                VerticalContentAlignment="Center"
                Content="Sign Up"
                Command="{Binding Register}"
                Style="{StaticResource ChefsPrimaryButtonStyle}" />
    </utu:AutoLayout>
    <utu:AutoLayout PrimaryAxisAlignment="Center"
                    Orientation="Horizontal"
                    Spacing="4"
                    CounterAxisAlignment="Center">
        <TextBlock Text="Already a member?"
                    Foreground="{ThemeResource OnSurfaceBrush}"
                    Style="{StaticResource LabelLarge}" />
        <Button Content="Login Now"
                uen:Navigation.Request="-/Login"
                Style="{StaticResource TextButtonStyle}" />
    </utu:AutoLayout>
</utu:AutoLayout>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Presentation/LoginModel.cs#L7-L22) for Login is just a CommandBuilder mixed with Authentication. The [code-behind for Registration](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Presentation/RegistrationModel.cs#L10C2-L27C3) doesn't use CommandBuilder.

```csharp
public IState<Credentials> UserCredentials => State<Credentials>.Value(this, () => new Credentials());

public ICommand Login => Command.Create(b => b.Given(UserCredentials).When(CanLogin).Then(DoLogin));

private bool CanLogin(Credentials userCredentials)
{
    return userCredentials is not null &&
            !string.IsNullOrWhiteSpace(userCredentials.Username) &&
            !string.IsNullOrWhiteSpace(userCredentials.Password);
}

private async ValueTask DoLogin(Credentials userCredentials, CancellationToken ct)
{
    await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string> { { "Username", userCredentials.Username! }, { "Password", userCredentials.Password! } });
    await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack, cancellation: ct);
}
```

```csharp
public IState<Credentials> UserCredentials => State<Credentials>.Value(this, () => new Credentials());

public async ValueTask Register(Credentials userCredentials)
{
    var username = userCredentials?.Username ?? string.Empty;
    var password = userCredentials?.Password ?? string.Empty;
    
    var success = await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string>
    {
        { "Username", username },
        { "Password", password }
    });
    
    if (success)
    {
        await Navigator.NavigateViewModelAsync<MainModel>(this);
    }
}
```

![Login view](/doc/assets/login-view.png)

![Register view](/doc/assets/register-view.png)

## MainPage/Home

### General NavBar with Notifications (and more)

A TabBar that sits at the top of the page like we have in Chefs. There's the app's icon on the left and we can access the Profile and Notification flyouts from here.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/HomePage.xaml#L66-L88) on the HomePage is a NavigationBar with commands.

```xml
<utu:NavigationBar x:Name="NavBar"
                   Style="{StaticResource ChefsNavigationBarStyle}">
    <utu:NavigationBar.Content>
        <Grid>
            <Image Source="{ThemeResource ChefsAppSignature}"
                    HorizontalAlignment="Left"
                    Width="128"
                    Height="40" />
        </Grid>
    </utu:NavigationBar.Content>
    <utu:NavigationBar.PrimaryCommands>
        <AppBarButton uen:Navigation.Request="!Profile">
            <AppBarButton.Icon>
                <PathIcon Data="{StaticResource Icon_Person_Outline}" />
            </AppBarButton.Icon>
        </AppBarButton>
        <AppBarButton uen:Navigation.Request="!Notifications">
            <AppBarButton.Icon>
                <PathIcon Data="{StaticResource Icon_Notification_Bell}" />
            </AppBarButton.Icon>
        </AppBarButton>
    </utu:NavigationBar.PrimaryCommands>
</utu:NavigationBar>
```

- The [NotificationsPage in Chefs](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/NotificationsPage.xaml#L91-L307) is a bit heavy.

```xml
<Page.Resources>
    <DataTemplate x:Key="NotificationTemplate">
        <utu:AutoLayout Background="{Binding Read, Converter={StaticResource BoolToNotificationColor}}"
                        CornerRadius="4"
                        PrimaryAxisAlignment="Center"
                        Orientation="Horizontal"
                        Padding="16"
                        Spacing="16">
            <PathIcon Data="{StaticResource Icon_Notifications_Active}"
                        Foreground="{ThemeResource PrimaryBrush}" />
            <utu:AutoLayout utu:AutoLayout.PrimaryAlignment="Stretch"
                            Spacing="8"
                            PrimaryAxisAlignment="Center">
                <TextBlock TextWrapping="Wrap"
                            Text="{Binding Title}"
                            Foreground="{ThemeResource OnSurfaceBrush}"
                            Style="{StaticResource TitleSmall}" />

                <TextBlock TextWrapping="Wrap"
                            Text="{Binding Description}"
                            Foreground="{ThemeResource OnSurfaceMediumBrush}" />
            </utu:AutoLayout>
        </utu:AutoLayout>
    </DataTemplate>
    <DataTemplate x:Key="EmptyTemplate">
        <utu:AutoLayout Spacing="24"
                        PrimaryAxisAlignment="Center"
                        CounterAxisAlignment="Center"
                        Padding="32,0">
            <BitmapIcon UriSource="{ThemeResource Empty_Notification}"
                        Width="72"
                        Height="70" />
            <TextBlock TextAlignment="Center"
                        TextWrapping="Wrap"
                        Text="No Notifications Yet"
                        Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleLarge}" />
            <TextBlock TextAlignment="Center"
                        TextWrapping="Wrap"
                        Text="Notifications about your activity, updates, and community interactions will appear here. Stay tuned for recipe inspiration, comments, likes, and more as you engage with the community."
                        Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleMedium}" />
            <Button HorizontalContentAlignment="Center"
                    VerticalContentAlignment="Center"
                    Content="Close"
                    Padding="16,10,24,10"
                    Foreground="{ThemeResource OnSecondaryContainerBrush}"
                    CornerRadius="4"
                    Style="{StaticResource FilledTonalButtonStyle}"
                    uen:Navigation.Request="-">
                <ut:ControlExtensions.Icon>
                    <PathIcon Data="{StaticResource Icon_Close}"
                                Foreground="{ThemeResource OnSecondaryContainerBrush}" />
                </ut:ControlExtensions.Icon>
            </Button>
        </utu:AutoLayout>
    </DataTemplate>
</Page.Resources>

<utu:AutoLayout Background="{ThemeResource BackgroundBrush}"
                uen:Region.Attached="True"
                utu:AutoLayout.PrimaryAlignment="Stretch"
                Padding="16">
    <utu:TabBar Background="{ThemeResource BackgroundBrush}"
                uen:Region.Attached="True"
                Style="{StaticResource TopTabBarStyle}">
        <utu:TabBarItem Content="All"
                        uen:Region.Name="AllTab"
                        IsSelected="True" />
        <utu:TabBarItem Content="Unread"
                        uen:Region.Name="UnreadTab"
                        Foreground="{ThemeResource OnSurfaceMediumBrush}" />
        <utu:TabBarItem Content="Read"
                        uen:Region.Name="ReadTab"
                        Foreground="{ThemeResource OnSurfaceMediumBrush}" />
    </utu:TabBar>
    <ScrollViewer HorizontalScrollMode="Disabled"
                    utu:AutoLayout.PrimaryAlignment="Stretch"
                    VerticalContentAlignment="Center">
        <Grid uen:Region.Attached="True"
                uen:Region.Navigator="Visibility">

            <!-- All Notifications -->
            <Grid Padding="0,16"
                    uen:Region.Name="AllTab"
                    Visibility="Visible">
                <uer:FeedView x:Name="AllFeed"
                                Source="{Binding Notifications}"
                                NoneTemplate="{StaticResource EmptyTemplate}">
                    <DataTemplate>
                        <utu:AutoLayout>
                            <!-- Today -->
                            <utu:AutoLayout Visibility="{Binding Data.HasTodayNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Today"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Today}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>

                            <!-- Yesterday -->
                            <utu:AutoLayout Visibility="{Binding Data.HasYesterdayNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Yesterday"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Yesterday}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>

                            <!-- Older -->
                            <utu:AutoLayout Visibility="{Binding Data.HasOlderNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Older"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Older}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>
                        </utu:AutoLayout>
                    </DataTemplate>
                </uer:FeedView>
            </Grid>

            <!-- Unread -->
            <Grid Padding="0,16"
                    uen:Region.Name="UnreadTab"
                    Visibility="Collapsed">
                <uer:FeedView x:Name="UnreadFeed"
                                Source="{Binding Unread}"
                                NoneTemplate="{StaticResource EmptyTemplate}">
                    <DataTemplate>
                        <utu:AutoLayout>
                            <!-- Today -->
                            <utu:AutoLayout Visibility="{Binding Data.HasTodayNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Today"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Today}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>

                            <!-- Yesterday -->
                            <utu:AutoLayout Visibility="{Binding Data.HasYesterdayNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Yesterday"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Yesterday}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>

                            <!-- Older -->
                            <utu:AutoLayout Visibility="{Binding Data.HasOlderNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Older"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Older}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>
                        </utu:AutoLayout>
                    </DataTemplate>
                </uer:FeedView>
            </Grid>

            <!-- Read -->
            <Grid Padding="0,16"
                    uen:Region.Name="ReadTab"
                    Visibility="Collapsed">
                <uer:FeedView x:Name="ReadFeed"
                                Source="{Binding Read}"
                                NoneTemplate="{StaticResource EmptyTemplate}">
                    <DataTemplate>
                        <utu:AutoLayout>
                            <!-- Today -->
                            <utu:AutoLayout Visibility="{Binding Data.HasTodayNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Today"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Today}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>

                            <!-- Yesterday -->
                            <utu:AutoLayout Visibility="{Binding Data.HasYesterdayNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Yesterday"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Yesterday}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>

                            <!-- Older -->
                            <utu:AutoLayout Visibility="{Binding Data.HasOlderNotifications}"
                                            Padding="0,16"
                                            Spacing="16">
                                <TextBlock TextWrapping="Wrap"
                                            Text="Older"
                                            Foreground="{ThemeResource OnSurfaceBrush}"
                                            Style="{StaticResource LabelLarge}" />
                                <muxc:ItemsRepeater ItemsSource="{Binding Data.Older}"
                                                    ItemTemplate="{StaticResource NotificationTemplate}">
                                    <muxc:ItemsRepeater.Layout>
                                        <muxc:StackLayout Orientation="Vertical"
                                                            Spacing="2" />
                                    </muxc:ItemsRepeater.Layout>
                                </muxc:ItemsRepeater>
                            </utu:AutoLayout>
                        </utu:AutoLayout>
                    </DataTemplate>
                </uer:FeedView>
            </Grid>
        </Grid>
    </ScrollViewer>
</utu:AutoLayout>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/NotificationsModel.cs#L12-L15) for Notifications are separate feeds.

```csharp
public IFeed<GroupedNotification> Notifications => Feed<GroupedNotification>.Async(async ct
        => await _notificationService.GetAll(ct) is { Count: > 0 } result
            ? new GroupedNotification(result) : Option.None<GroupedNotification>());

public IFeed<GroupedNotification> Unread => Notifications.Select(group =>
    new GroupedNotification(group.GetAll().Where(n => !n.Read).ToImmutableList()));

public IFeed<GroupedNotification> Read => Notifications.Select(group =>
    new GroupedNotification(group.GetAll().Where(n => n.Read).ToImmutableList()));
```

![NavBar with Notifications](/doc/assets/main-tabbar.png)

### Settings

A widget with relevant app settings, like theme switching and language switching. In Chefs we have a settings flyout with theme switching and notification toggling.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/SettingsPage.xaml#L79-L123) would just need to be a couple toggle switches or other similar control.

```xml
<utu:AutoLayout Padding="16"
                Background="{ThemeResource SurfaceBrush}"
                CornerRadius="8"
                Spacing="16">
    <utu:AutoLayout PrimaryAxisAlignment="Stretch"
                    Justify="SpaceBetween"
                    Orientation="Horizontal"
                    CounterAxisAlignment="Center">
        <utu:AutoLayout Spacing="16"
                        Margin="16"
                        Orientation="Horizontal"
                        CounterAxisAlignment="Center"
                        utu:AutoLayout.CounterAlignment="Center">
            <PathIcon Data="{StaticResource Icon_Notifications_None}"
                        Foreground="{ThemeResource PrimaryBrush}" />
            <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleMedium}"
                        Text="Notifications"
                        TextWrapping="Wrap" />
        </utu:AutoLayout>
        <ToggleSwitch IsOn="{Binding Settings.Notification, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                        Style="{StaticResource ToggleSwitchStyle}" />
    </utu:AutoLayout>
    <utu:Divider Foreground="{ThemeResource OutlineVariantBrush}"
                    Style="{StaticResource DividerStyle}" />
    <utu:AutoLayout PrimaryAxisAlignment="Stretch"
                    Justify="SpaceBetween"
                    Orientation="Horizontal"
                    CounterAxisAlignment="Center">
        <utu:AutoLayout Spacing="16"
                        Margin="16"
                        Orientation="Horizontal"
                        CounterAxisAlignment="Center"
                        utu:AutoLayout.CounterAlignment="Center">
            <PathIcon Data="{StaticResource Icon_Night_Mode}"
                        Foreground="{ThemeResource PrimaryBrush}" />
            <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleMedium}"
                        Text="Night Mode"
                        TextWrapping="Wrap" />
        </utu:AutoLayout>
        <ToggleSwitch IsOn="{Binding Settings.IsDark, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
                        Style="{StaticResource ToggleSwitchStyle}" />
    </utu:AutoLayout>
</utu:AutoLayout>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/SettingsModel.cs#L22-L34) is a state of AppConfig where we track the user's choice of settings so it persists.

```csharp
public IState<AppConfig> Settings => State
    .Async(this, _userService.GetSettings)
    .ForEach(async (settings, ct) =>
    {
        if (settings is { })
        {
            var isDark = (settings.IsDark ?? false);
            await _themeService.SetThemeAsync(isDark ? Uno.Extensions.Toolkit.AppTheme.Dark : Uno.Extensions.Toolkit.AppTheme.Light);
            await _userService.SetSettings(settings, ct);

            WeakReferenceMessenger.Default.Send(new ThemeChangedMessage(isDark));
        }
    });
```

![Settings flyout](/doc/assets/settings-view.png)

### App region switching TabBar

A TabBar with Region switching already all set up, like the left-hand TabBar Chefs uses on wide screens for navigating between the Home, Search and Favorites views. The TabBar snaps to the bottom of the app when screen size is small.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/be397784a5a6a7183b617531c1b6d921c15332e6/src/Chefs/Views/MainPage.xaml#L17-L94) is a responsive layout. There's two TabBars and depending on what size the screen is one becomes visible and the other is hidden.

```xml
<Grid uen:Region.Attached="True"
      utu:AutoLayout.PrimaryAlignment="Stretch">

    <Grid.RowDefinitions>
        <RowDefinition />
        <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <Grid Grid.Row="0"
            Grid.Column="1"
            uen:Region.Attached="True"
            uen:Region.Navigator="Visibility" />

    <utu:TabBar Grid.Row="1"
                Grid.Column="1"
                Visibility="{utu:Responsive Narrow=Visible,
                                            Wide=Collapsed}"
                uen:Region.Attached="True"
                Style="{StaticResource BottomTabBarStyle}">
        <utu:TabBarItem uen:Region.Name="Home"
                        Content="Home">
            <utu:TabBarItem.Icon>
                <PathIcon Data="{StaticResource Icon_Home}" />
            </utu:TabBarItem.Icon>
        </utu:TabBarItem>

        <utu:TabBarItem uen:Region.Name="Search"
                        Content="Search">
            <utu:TabBarItem.Icon>
                <PathIcon Data="{StaticResource Icon_Search}" />
            </utu:TabBarItem.Icon>
        </utu:TabBarItem>

        <utu:TabBarItem uen:Region.Name="FavoriteRecipes"
                        Content="Favorites">
            <utu:TabBarItem.Icon>
                <PathIcon Data="{StaticResource Icon_Heart}" />
            </utu:TabBarItem.Icon>
        </utu:TabBarItem>
    </utu:TabBar>

    <utu:AutoLayout Grid.RowSpan="2"
                    Background="{ThemeResource SurfaceBrush}"
                    Visibility="{utu:Responsive Narrow=Collapsed,
                                                Wide=Visible}"
                    Width="120">
        <utu:TabBar uen:Region.Attached="True"
                    Style="{StaticResource VerticalTabBarStyle}"
                    utu:AutoLayout.PrimaryAlignment="Stretch">

            <utu:TabBarItem uen:Region.Name="Home"
                            Content="Home">
                <utu:TabBarItem.Icon>
                    <PathIcon Data="{StaticResource Icon_Home}" />
                </utu:TabBarItem.Icon>
            </utu:TabBarItem>

            <utu:TabBarItem uen:Region.Name="Search"
                            Content="Search">
                <utu:TabBarItem.Icon>
                    <PathIcon Data="{StaticResource Icon_Search}" />
                </utu:TabBarItem.Icon>
            </utu:TabBarItem>

            <utu:TabBarItem uen:Region.Name="FavoriteRecipes"
                            Content="Favorites">
                <utu:TabBarItem.Icon>
                    <PathIcon Data="{StaticResource Icon_Heart}" />
                </utu:TabBarItem.Icon>
            </utu:TabBarItem>
        </utu:TabBar>
    </utu:AutoLayout>
</Grid>
```

![HomePage TabBar](/doc/assets/homepage-tabbar.png)

### Item Carousel

An item carousel using `FeedView` like the ones on Chefs' HomePage that display recipes.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/HomePage.xaml#L105-L125) is just a `FeedView` with an `ItemsRepeater`. See the whole page for more different examples of layouts and templates.

```xml
<Page.Resources>
    <DataTemplate x:Key="HomeLargeItemTemplate">
        <utu:CardContentControl Margin="0"
                                Width="328"
                                CornerRadius="4"
                                Style="{StaticResource FilledCardContentControlStyle}">
            <utu:AutoLayout Background="{ThemeResource SurfaceBrush}"
                            CornerRadius="4"
                            PrimaryAxisAlignment="Center"
                            HorizontalAlignment="Stretch">
                <Border Height="144">
                    <Image HorizontalAlignment="Center"
                            VerticalAlignment="Center"
                            Source="{Binding ImageUrl}"
                            Stretch="UniformToFill" />
                </Border>
                <utu:AutoLayout Spacing="16"
                                Padding="16"
                                Justify="SpaceBetween"
                                Orientation="Horizontal">
                    <utu:AutoLayout Spacing="4">
                        <TextBlock TextWrapping="Wrap"
                                    Text="{Binding Name}"
                                    Foreground="{ThemeResource OnSurfaceBrush}"
                                    Style="{StaticResource TitleSmall}" />
                        <TextBlock TextWrapping="Wrap"
                                    Text="{Binding TimeCal}"
                                    Foreground="{ThemeResource OnSurfaceMediumBrush}"
                                    Style="{StaticResource CaptionMedium}" />
                    </utu:AutoLayout>
                    <ToggleButton Style="{StaticResource IconToggleButtonStyle}"
                                    IsChecked="{Binding IsFavorite}"
                                    Command="{utu:AncestorBinding AncestorType=uer:FeedView,
                                                                Path=DataContext.FavoriteRecipe}"
                                    CommandParameter="{Binding}">
                        <ToggleButton.Content>
                            <PathIcon Data="{StaticResource Icon_Heart}"
                                        Foreground="{ThemeResource OnSurfaceBrush}" />
                        </ToggleButton.Content>
                        <ut:ControlExtensions.AlternateContent>
                            <PathIcon Data="{StaticResource Icon_Heart_Filled}"
                                        Foreground="{ThemeResource PrimaryBrush}" />
                        </ut:ControlExtensions.AlternateContent>
                    </ToggleButton>
                </utu:AutoLayout>
            </utu:AutoLayout>
        </utu:CardContentControl>
    </DataTemplate>
</Page.Resources>

<uer:FeedView x:Name="TrendingNowFeed"
              Source="{Binding TrendingNow}">
    <DataTemplate>
        <ScrollViewer Padding="16,0"
                        HorizontalScrollMode="Auto"
                        HorizontalScrollBarVisibility="Auto"
                        VerticalScrollMode="Disabled"
                        VerticalScrollBarVisibility="Disabled"
                        utu:AutoLayout.PrimaryAlignment="Stretch">
            <muxc:ItemsRepeater ItemsSource="{Binding Data}"
                                uen:Navigation.Request="RecipeDetails"
                                uen:Navigation.Data="{Binding Data}"
                                ItemTemplate="{StaticResource HomeLargeItemTemplate}">
                <muxc:ItemsRepeater.Layout>
                    <muxc:StackLayout Orientation="Horizontal"
                                        Spacing="8" />
                </muxc:ItemsRepeater.Layout>
            </muxc:ItemsRepeater>
        </ScrollViewer>
    </DataTemplate>
</uer:FeedView>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/HomeModel.cs#L18-L20) is just a `ListFeed`/`ListState`.

```csharp
public IListState<Recipe> TrendingNow => ListState
    .Async(this, _recipeService.GetTrending)
    .Observe(_messenger, r => r.Id);
```

![HomePage carousels](/doc/assets/different-types-of-feedviews.png)

## Search

### Searchbar

A searchbar widget. Chefs has a simple one where the search results are separate from the searchbar in their own `FeedView`.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/SearchPage.xaml#L114-L123)

```xml
<TextBox utu:CommandExtensions.Command="{Binding Search}"
         Style="{StaticResource ChefsPrimaryTextBoxStyle}"
         CornerRadius="28"
         PlaceholderText="Search"
         Text="{Binding Term, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    <ut:ControlExtensions.Icon>
        <PathIcon Data="{StaticResource Icon_Search}"
                    Foreground="{ThemeResource OnSurfaceMediumBrush}" />
    </ut:ControlExtensions.Icon>
</TextBox>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/SearchModel.cs#L19-L20) is a state to track the current searched "term" and a [search task](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/SearchModel.cs#L41-L45) that updates the `FeedView`s [Results](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/SearchModel.cs#L23-L27).

```csharp
public IState<string> Term => State<string>.Value(this, () => string.Empty)
    .Observe(_messenger, t => t);

public IListState<Recipe> Results => ListState.FromFeed(this, Feed
    .Combine(Term, Filter)
    .SelectAsync(Search)
    .AsListFeed())
    .Observe(_messenger, r => r.Id);

private async ValueTask<IImmutableList<Recipe>> Search((string term, SearchFilter filter) inputs, CancellationToken ct)
{
    var searchedRecipes = await _recipeService.Search(inputs.term, inputs.filter, ct);
    return searchedRecipes.Where(inputs.filter.Match).ToImmutableList();
}
```

![Searchbar](/doc/assets/searchbar.png)

### Search filters

To add to the searchbar widget we could have search filters too. In Chefs they are in a separate flyout.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/FiltersPage.xaml#L27-L154) is a bit heavy because there are many types of filters. This is only a layout of the filters. The filter values are defined in the code-behind.

```xml
<Page.Resources>
    <DataTemplate x:Key="FilterChipTemplate">
        <utu:Chip Background="{ThemeResource SurfaceBrush}"
                    Content="{Binding}"
                    HorizontalAlignment="Stretch"
                    Foreground="{ThemeResource OnSurfaceVariantBrush}"
                    BorderThickness="1"
                    Style="{StaticResource MaterialChipStyle}" />
    </DataTemplate>
</Page.Resources>

<utu:AutoLayout>
    <utu:AutoLayout uen:Region.Attached="True"
                    utu:AutoLayout.PrimaryAlignment="Stretch"
                    PrimaryAxisAlignment="Stretch">
        <utu:NavigationBar Content="Filters">
            <utu:NavigationBar.MainCommand>
                <AppBarButton uen:Navigation.Request="-">
                    <AppBarButton.Icon>
                        <not_mobile:PathIcon Data="{StaticResource Icon_Close}" />
                        <mobile:BitmapIcon UriSource="ms-appx:///Assets/Icons/close.png" />
                    </AppBarButton.Icon>
                </AppBarButton>
            </utu:NavigationBar.MainCommand>
        </utu:NavigationBar>
        <ScrollViewer HorizontalScrollMode="Disabled"
                        utu:AutoLayout.PrimaryAlignment="Stretch">
            <utu:AutoLayout Padding="16"
                            Background="{ThemeResource SurfaceBrush}"
                            Spacing="32">
                <utu:AutoLayout Spacing="16">
                    <TextBlock Style="{StaticResource TitleSmall}"
                                Text="Recipe Categories"
                                TextWrapping="Wrap" />
                    <!-- It's important to put the ItemsSource above the SelectedItems, because, it will otherwise fail on WASM, see this issue: https://github.com/unoplatform/uno.toolkit.ui/issues/918 -->
                    <muxc:ItemsRepeater ItemsSource="{Binding FilterGroups}"
                                        utu:ItemsRepeaterExtensions.SelectedItem="{Binding Filter.FilterGroup, Mode=TwoWay}"
                                        utu:ItemsRepeaterExtensions.SelectionMode="SingleOrNone"
                                        ItemTemplate="{StaticResource FilterChipTemplate}">
                        <muxc:ItemsRepeater.Layout>
                            <muxc:UniformGridLayout ItemsJustification="Start"
                                                    MinColumnSpacing="8"
                                                    MinRowSpacing="8"
                                                    MinItemWidth="120"
                                                    ItemsStretch="Fill"
                                                    MaximumRowsOrColumns="4"
                                                    Orientation="Horizontal" />
                        </muxc:ItemsRepeater.Layout>
                    </muxc:ItemsRepeater>
                </utu:AutoLayout>
                <utu:AutoLayout Spacing="16">
                    <TextBlock Style="{StaticResource TitleSmall}"
                                Text="Cooking Time"
                                TextWrapping="Wrap" />
                    <muxc:ItemsRepeater ItemsSource="{Binding Times}"
                                        utu:ItemsRepeaterExtensions.SelectedItem="{Binding Filter.Time, Mode=TwoWay}"
                                        utu:ItemsRepeaterExtensions.SelectionMode="SingleOrNone">
                        <muxc:ItemsRepeater.Layout>
                            <muxc:UniformGridLayout ItemsJustification="Start"
                                                    MinColumnSpacing="8"
                                                    MinRowSpacing="8"
                                                    MinItemWidth="90"
                                                    ItemsStretch="Fill"
                                                    MaximumRowsOrColumns="4"
                                                    Orientation="Horizontal" />
                        </muxc:ItemsRepeater.Layout>
                        <muxc:ItemsRepeater.ItemTemplate>
                            <DataTemplate>
                                <utu:Chip Background="{ThemeResource SurfaceBrush}"
                                            Content="{Binding Converter={StaticResource CookingTimeFormatter}}"
                                            Foreground="{ThemeResource OnSurfaceVariantBrush}"
                                            HorizontalAlignment="Stretch"
                                            BorderThickness="1"
                                            Style="{StaticResource MaterialChipStyle}" />
                            </DataTemplate>
                        </muxc:ItemsRepeater.ItemTemplate>
                    </muxc:ItemsRepeater>
                </utu:AutoLayout>
                <utu:AutoLayout Spacing="16">
                    <TextBlock Style="{StaticResource TitleSmall}"
                                Text="Skill Level"
                                TextWrapping="Wrap" />
                    <muxc:ItemsRepeater ItemsSource="{Binding Difficulties}"
                                        utu:ItemsRepeaterExtensions.SelectedItem="{Binding Filter.Difficulty, Mode=TwoWay}"
                                        utu:ItemsRepeaterExtensions.SelectionMode="SingleOrNone"
                                        ItemTemplate="{StaticResource FilterChipTemplate}">
                        <muxc:ItemsRepeater.Layout>
                            <muxc:UniformGridLayout ItemsJustification="Start"
                                                    MinColumnSpacing="8"
                                                    MinRowSpacing="8"
                                                    MinItemWidth="140"
                                                    ItemsStretch="Fill"
                                                    MaximumRowsOrColumns="4"
                                                    Orientation="Horizontal" />
                        </muxc:ItemsRepeater.Layout>
                    </muxc:ItemsRepeater>
                </utu:AutoLayout>
                <utu:AutoLayout Spacing="16">
                    <TextBlock Style="{StaticResource TitleSmall}"
                                Text="Serves"
                                TextWrapping="Wrap" />
                    <muxc:ItemsRepeater ItemsSource="{Binding Serves}"
                                        utu:ItemsRepeaterExtensions.SelectedItem="{Binding Filter.Serves, Mode=TwoWay}"
                                        utu:ItemsRepeaterExtensions.SelectionMode="SingleOrNone"
                                        ItemTemplate="{StaticResource FilterChipTemplate}">
                        <muxc:ItemsRepeater.Layout>
                            <muxc:UniformGridLayout ItemsJustification="Start"
                                                    MinColumnSpacing="8"
                                                    MinRowSpacing="8"
                                                    MinItemWidth="50"
                                                    ItemsStretch="Fill"
                                                    MaximumRowsOrColumns="4"
                                                    Orientation="Horizontal" />
                        </muxc:ItemsRepeater.Layout>
                    </muxc:ItemsRepeater>
                </utu:AutoLayout>
            </utu:AutoLayout>
        </ScrollViewer>
    </utu:AutoLayout>
    <utu:AutoLayout Height="100"
                    Padding="16,24"
                    Background="{ThemeResource SurfaceBrush}"
                    Orientation="Horizontal"
                    PrimaryAxisAlignment="Center"
                    CounterAxisAlignment="Start"
                    Spacing="16">
        <Button Content="Reset"
                Height="60"
                Command="{Binding Reset}"
                CornerRadius="4"
                Style="{StaticResource TextButtonStyle}"
                utu:AutoLayout.PrimaryAlignment="Stretch" />
        <Button Content="Apply filter"
                Height="60"
                Command="{Binding ApplySearchFilter}"
                CornerRadius="4"
                utu:AutoLayout.PrimaryAlignment="Stretch" />
    </utu:AutoLayout>
</utu:AutoLayout>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/FilterModel.cs#L14-L25) contains the filters enumerators and two tasks, one is to Apply the filters to the search (navigates back, out of the flyout while changing the filters) and the other is to Reset the filters to their default value.

```csharp
public IState<SearchFilter> Filter { get; }
public IEnumerable<FilterGroup> FilterGroups => Enum.GetValues(typeof(FilterGroup)).Cast<FilterGroup>();
public IEnumerable<Time> Times => Enum.GetValues(typeof(Time)).Cast<Time>();
public IEnumerable<Difficulty> Difficulties => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
public IEnumerable<int> Serves => new int[] { 1, 2, 3, 4, 5 };
public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

public async ValueTask ApplySearchFilter(SearchFilter filter) =>
    await _navigator.NavigateBackWithResultAsync(this, data: filter);

public async ValueTask Reset(CancellationToken ct) =>
    await Filter.UpdateAsync(current => new SearchFilter(), ct);
```

![Search filters](/doc/assets/filter-view.png)

### Responsive Grid Layout

Chefs has many pages with the same or very similar responsive grid layout (SearchPage, FavoritesPage, CookbookDetails/EditingPage).

```xml
<muxc:UniformGridLayout x:Key="ResponsiveGridLayout"
                        ItemsStretch="Fill"
                        MaximumRowsOrColumns="{utu:Responsive Narrow=2, Wide=8}"
                        MinColumnSpacing="{utu:Responsive Narrow=8, Wide=16}"
                        MinItemWidth="{utu:Responsive Narrow=155, Wide=240}"
                        MinRowSpacing="{utu:Responsive Narrow=8, Wide=16}" />
```

## FavoriteRecipes/Cookbooks

### Page region switching TabBar

Like the "App region switching TabBar" from above but on a single page. In Chefs we have a TabBar on the FavoriteRecipes page that allows the user to switch between their FavoriteRecipes and Cookbooks. We also have a more embedded one on the RecipeDetails page to view different information about the recipe.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/FavoriteRecipesPage.xaml#L209-L220) for the FavoriteRecipes page is just a TabBar.

```xml
<utu:TabBar uen:Region.Attached="True"
            Background="{ThemeResource BackgroundBrush}"
            Style="{StaticResource TopTabBarStyle}">
    <utu:TabBarItem x:Name="TabBarRecipeItem"
                    uen:Region.Name="MyRecipes"
                    Content="All Recipes"
                    Foreground="{ThemeResource OnSurfaceMediumBrush}"
                    IsSelected="True" />
    <utu:TabBarItem uen:Region.Name="Cookbooks"
                    Content="My Cookbooks"
                    Foreground="{ThemeResource OnSurfaceMediumBrush}" />
</utu:TabBar>
```

- For the RecipeDetails page its like [this](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/RecipeDetailsPage.xaml#L180-L199) with the different Regions defined below.

```xml
<utu:TabBar uen:Region.Attached="True"
            utu:AutoLayout.CounterAlignment="{utu:Responsive Narrow=Start, Wide=Center}"
            Margin="{utu:Responsive Narrow=0, Wide='40,0'}"
            Background="{ThemeResource BackgroundBrush}"
            Style="{StaticResource TopTabBarStyle}">
    <utu:TabBarItem uen:Region.Name="IngredientsTab"
                    Content="Ingredients"
                    IsSelected="True" />
    <utu:TabBarItem uen:Region.Name="StepsTab"
                    Content="Steps"
                    Foreground="{ThemeResource OnSurfaceMediumBrush}" />
    <utu:TabBarItem uen:Region.Name="ReviewsTab"
                    Content="Reviews"
                    Foreground="{ThemeResource OnSurfaceMediumBrush}" />
    <utu:TabBarItem uen:Region.Name="NutritionTab"
                    Content="Nutrition"
                    Foreground="{ThemeResource OnSurfaceMediumBrush}" />
</utu:TabBar>
```

![Favorites TabBar](/doc/assets/favorites-tabbar.png)

### Cookbook template

The Cookbook feed has a regular layout with a different item template.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/FavoriteRecipesPage.xaml#L257C6-L272C21) is a `FeedView` that displays the user's cookbooks.

```xml
<Page.Resources>
    <DataTemplate x:Key="CookbookEmptyTemplate">
        <utu:AutoLayout Padding="32,0"
                        PrimaryAxisAlignment="Center"
                        CounterAxisAlignment="Center"
                        Spacing="24">
            <BitmapIcon Width="56"
                        Height="72"
                        UriSource="{ThemeResource Empty_Box}" />
            <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleLarge}"
                        Text="No Cookbooks Created"
                        TextAlignment="Center"
                        TextWrapping="Wrap" />
            <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleMedium}"
                        Text="It looks like you haven't created any cookbooks yet. Cookbooks are a great way to organize your recipes into collections. "
                        TextAlignment="Center"
                        TextWrapping="Wrap" />
            <Button HorizontalContentAlignment="Center"
                    VerticalContentAlignment="Center"
                    uen:Navigation.Request="!CreateCookbook"
                    Content="Create cookbook"
                    Foreground="{ThemeResource PrimaryBrush}"
                    Padding="12,10,16,10"
                    CornerRadius="20"
                    Style="{StaticResource TextButtonStyle}">
                <ut:ControlExtensions.Icon>
                    <PathIcon Data="{StaticResource Icon_Add}"
                                Style="{StaticResource FontAwesomeSolidFontIconStyle}"
                                Foreground="{ThemeResource PrimaryBrush}" />
                </ut:ControlExtensions.Icon>
            </Button>
        </utu:AutoLayout>
    </DataTemplate>

    <DataTemplate x:Key="CookbookTemplate">
        <utu:CardContentControl x:Name="CookbookTemplateCard"
                                HorizontalAlignment="Stretch"
                                VerticalAlignment="Stretch"
                                Background="{ThemeResource SurfaceBrush}"
                                CornerRadius="12"
                                Style="{StaticResource FilledCardContentControlStyle}">
            <utu:CardContentControl.ContentTemplate>
                <DataTemplate>
                    <utu:AutoLayout Margin="4">
                        <utu:AutoLayout Height="144"
                                        utu:AutoLayout.CounterAlignment="Stretch"
                                        Orientation="Horizontal"
                                        Spacing="4">
                            <Border utu:AutoLayout.PrimaryAlignment="Stretch"
                                    CornerRadius="12">
                                <Image HorizontalAlignment="Center"
                                        VerticalAlignment="Center"
                                        Source="{Binding CookbookImages.FirstImage}"
                                        Stretch="UniformToFill" />
                            </Border>

                            <Grid RowSpacing="4"
                                    utu:AutoLayout.PrimaryAlignment="Stretch">
                                <Grid.RowDefinitions>
                                    <RowDefinition Height="*" />
                                    <RowDefinition Height="*" />
                                </Grid.RowDefinitions>
                                <Border utu:AutoLayout.PrimaryAlignment="Stretch"
                                        Background="{ThemeResource BackgroundBrush}"
                                        CornerRadius="12">
                                    <Image HorizontalAlignment="Center"
                                            VerticalAlignment="Center"
                                            Source="{Binding CookbookImages.SecondImage}"
                                            Stretch="UniformToFill"
                                            Visibility="{Binding CookbookImages.SecondImage, Converter={StaticResource NullToCollapsed}}" />
                                </Border>

                                <Grid Grid.Row="1"
                                        utu:AutoLayout.PrimaryAlignment="Stretch"
                                        CornerRadius="12">
                                    <Image HorizontalAlignment="Center"
                                            VerticalAlignment="Center"
                                            Source="{Binding CookbookImages.ThirdImage}"
                                            Stretch="UniformToFill"
                                            Visibility="{Binding CookbookImages.ThirdImage, Converter={StaticResource NullToCollapsed}}" />

                                    <Button MinHeight="57"
                                            HorizontalAlignment="Stretch"
                                            VerticalAlignment="Stretch"
                                            uen:Navigation.Data="{Binding}"
                                            uen:Navigation.Request="CreateCookbook"
                                            Background="{ThemeResource BackgroundBrush}"
                                            CornerRadius="20"
                                            Style="{StaticResource TextButtonStyle}"
                                            Visibility="{Binding CookbookImages.ThirdImage, Converter={StaticResource NullToVisible}}">
                                        <Button.Content>
                                            <PathIcon Data="{StaticResource Icon_Add}"
                                                        Style="{StaticResource FontAwesomeSolidFontIconStyle}"
                                                        Foreground="{ThemeResource OnSurfaceBrush}" />
                                        </Button.Content>
                                    </Button>
                                </Grid>
                            </Grid>
                        </utu:AutoLayout>
                        <utu:AutoLayout Padding="12"
                                        PrimaryAxisAlignment="Center"
                                        Spacing="2">
                            <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                                        Style="{StaticResource TitleSmall}"
                                        Text="{Binding Name}"
                                        TextWrapping="NoWrap" />
                            <TextBlock Foreground="{ThemeResource OnSurfaceMediumBrush}"
                                        Style="{StaticResource CaptionMedium}"
                                        Text="{Binding PinsNumber, Converter={StaticResource StringFormatter}, ConverterParameter='{}{0} recipes'}"
                                        TextWrapping="NoWrap" />
                        </utu:AutoLayout>
                    </utu:AutoLayout>
                </DataTemplate>
            </utu:CardContentControl.ContentTemplate>
        </utu:CardContentControl>
    </DataTemplate>
</Page.Resources>

<uer:FeedView utu:AutoLayout.CounterAlignment="Stretch"
              utu:AutoLayout.PrimaryAlignment="Stretch"
              NoneTemplate="{StaticResource CookbookEmptyTemplate}"
              Source="{Binding SavedCookbooks}">
    <DataTemplate>
        <ScrollViewer x:Name="RecipesScrollViewer"
                        HorizontalScrollMode="Disabled"
                        VerticalScrollBarVisibility="Hidden">
            <muxc:ItemsRepeater Margin="0,0,0,16"
                                uen:Navigation.Request="CookbookDetails"
                                ItemTemplate="{StaticResource CookbookTemplate}"
                                ItemsSource="{Binding Data}"
                                Layout="{StaticResource ResponsiveGridLayout}" />
        </ScrollViewer>
    </DataTemplate>
</uer:FeedView>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/FavoriteRecipesModel.cs#L23C1-L25C37) is in the same model as FavoriteRecipes, it contains the cookbook feed.

```csharp
public IListState<Cookbook> SavedCookbooks => ListState
    .Async(this, _cookbookService.GetSaved)
    .Observe(_messenger, cb => cb.Id);
```

![Cookbooks](/doc/assets/cookbooks.png)

### Share function

A share widget. The user clicks on the share button and a native sharing popup opens. In Chefs, sharing is working on all platforms except for Wasm and Skia/Desktop.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/RecipeDetailsPage.xaml#L56-L60) is just a button with an icon.

```xml
<AppBarButton Command="{Binding Share}">
    <AppBarButton.Icon>
        <PathIcon Data="{StaticResource Icon_Share}" />
    </AppBarButton.Icon>
</AppBarButton>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/RecipeDetailsModel.cs#L71-L119) is a bit heavy but needed for cross-platform sharing.

```csharp
public partial record RecipeDetailsModel
{
    private static readonly Guid _dtm_iid = new Guid("a5caee9b-8708-49d1-8d36-67d25a8da00c");

#if WINDOWS
    static IDataTransferManagerInterop DataTransferManagerInterop => DataTransferManager.As<IDataTransferManagerInterop>();
#endif
    
    public async ValueTask Share(CancellationToken ct)
    {
#if WINDOWS
        IntPtr result;
        var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
        result = DataTransferManagerInterop.GetForWindow(hwnd, _dtm_iid);
        DataTransferManager dataTransferManager = MarshalInterface<DataTransferManager>.FromAbi(result);
        dataTransferManager.DataRequested += DataRequested;
        DataTransferManagerInterop.ShowShareUIForWindow(hwnd, null);
#else
        var dataTransferManager = DataTransferManager.GetForCurrentView();
        dataTransferManager.DataRequested += DataRequested;
        DataTransferManager.ShowShareUI();
#endif
    }

    private async void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
    {
        args.Request.Data.Properties.Title = $"Sharing {Recipe.Name}";
        args.Request.Data.Properties.Description = Recipe.Details ?? "Chefs Recipe";
        args.Request.Data.SetText(await CreateShareText());
    }

    private async ValueTask<string> CreateShareText()
    {
        var shareTextBuilder = new StringBuilder();
        var steps = await Steps;

        foreach (var step in steps)
        {
            shareTextBuilder.AppendLine($"Step {step.Number}: {step.Name}")
                            .AppendLine($"Ingredients: {string.Join(", ", step.Ingredients ?? ImmutableList<string>.Empty)}")
                            .AppendLine($"Description: {step.Description}")
                            .AppendLine();
        }

        return shareTextBuilder.ToString();
    }

#if WINDOWS
    [ComImport]
    [Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDataTransferManagerInterop
    {
        IntPtr GetForWindow([In] IntPtr appWindow, [In] ref Guid riid);
        void ShowShareUIForWindow(IntPtr appWindow, ShareUIOptions options);
    }
#endif
}
```

![Share button](/doc/assets/share-button.png)

### Reviews

A reviews `FeedView` widget. In Chefs we have reviews that the user can like or dislike.

- The [XAML](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Views/RecipeDetailsPage.xaml#L317-L420) is a `FeedView` with the review layout.

```xml
<Page.Resources>
    <DataTemplate x:Key="EmptyTemplate">
        <utu:AutoLayout Padding="{utu:Responsive Narrow='16,24',
                                                    Wide='40'}"
                        PrimaryAxisAlignment="Center"
                        Spacing="24">
            <BitmapIcon utu:AutoLayout.CounterAlignment="Center"
                        Width="72"
                        Height="72"
                        UriSource="{ThemeResource Empty_Recipe}" />
            <TextBlock Foreground="{ThemeResource OnSurfaceBrush}"
                        Style="{StaticResource TitleLarge}"
                        Text="No Reviews Yet"
                        TextAlignment="Center"
                        TextWrapping="Wrap" />
        </utu:AutoLayout>
    </DataTemplate>
</Page.Resources>

<uer:FeedView x:Name="ReviewsFeed"
              VerticalContentAlignment="Center"
              NoneTemplate="{StaticResource EmptyTemplate}"
              Source="{Binding Reviews}">
    <DataTemplate>
        <muxc:ItemsRepeater ItemsSource="{Binding Data}">
            <muxc:ItemsRepeater.Layout>
                <muxc:StackLayout Orientation="Vertical"
                                    Spacing="2" />
            </muxc:ItemsRepeater.Layout>
            <muxc:ItemsRepeater.ItemTemplate>
                <DataTemplate>
                    <utu:AutoLayout Background="{ThemeResource SurfaceBrush}"
                                    Spacing="16"
                                    CornerRadius="4"
                                    Orientation="Horizontal"
                                    Padding="16">
                        <PersonPicture Width="60"
                                        Height="60"
                                        ProfilePicture="{Binding UrlAuthorImage}" />
                        <utu:AutoLayout Spacing="8"
                                        utu:AutoLayout.PrimaryAlignment="Stretch"
                                        PrimaryAxisAlignment="Center">
                            <TextBlock TextWrapping="Wrap"
                                        Text="{Binding Description}"
                                        Foreground="{ThemeResource OnSurfaceBrush}" />
                            <TextBlock TextWrapping="Wrap"
                                        Text="{Binding PublisherName}"
                                        Foreground="{ThemeResource OnSurfaceMediumBrush}" />
                            <utu:AutoLayout Spacing="{utu:Responsive Narrow=8,
                                                                        Wide=24}"
                                            Orientation="Horizontal"
                                            CounterAxisAlignment="Center"
                                            Padding="{utu:Responsive Narrow='0,16,0,16',
                                                                        Wide='0,16,0,0'}">
                                <ToggleButton HorizontalContentAlignment="Center"
                                                VerticalContentAlignment="Center"
                                                Command="{utu:AncestorBinding AncestorType=uer:FeedView,
                                                                            Path=DataContext.Like}"
                                                CommandParameter="{Binding}"
                                                IsChecked="{Binding UserLike, Converter={StaticResource UserLikeCheckedConverter}, Mode=TwoWay}">
                                    <ToggleButton.Content>
                                        <utu:AutoLayout Orientation="Horizontal"
                                                        Spacing="{utu:Responsive Narrow=5,
                                                                                    Wide=8}"
                                                        CounterAxisAlignment="Center">
                                            <PathIcon Data="{StaticResource Icon_Thumb_Up_Off}"
                                                        Foreground="{ThemeResource PrimaryBrush}" />
                                            <TextBlock Text="{Binding Likes.Count}"
                                                        Foreground="{ThemeResource PrimaryBrush}"
                                                        Style="{StaticResource LabelLarge}" />
                                        </utu:AutoLayout>
                                    </ToggleButton.Content>
                                    <ut:ControlExtensions.AlternateContent>
                                        <utu:AutoLayout Orientation="Horizontal"
                                                        Spacing="{utu:Responsive Narrow=5,
                                                                                    Wide=8}"
                                                        CounterAxisAlignment="Center">
                                            <PathIcon Data="{StaticResource Icon_Thumb_Up}"
                                                        Foreground="{ThemeResource PrimaryBrush}" />
                                            <TextBlock Text="{Binding Likes.Count}"
                                                        Foreground="{ThemeResource PrimaryBrush}"
                                                        Style="{StaticResource LabelLarge}" />
                                        </utu:AutoLayout>
                                    </ut:ControlExtensions.AlternateContent>
                                </ToggleButton>
                                <ToggleButton HorizontalContentAlignment="Center"
                                                VerticalContentAlignment="Center"
                                                Command="{utu:AncestorBinding AncestorType=uer:FeedView,
                                                                            Path=DataContext.Dislike}"
                                                CommandParameter="{Binding}"
                                                IsChecked="{Binding UserLike, Converter={StaticResource UserDislikeCheckedConverter}, Mode=TwoWay}">
                                    <ToggleButton.Content>
                                        <utu:AutoLayout Orientation="Horizontal"
                                                        Spacing="{utu:Responsive Narrow=5,
                                                                                    Wide=8}"
                                                        CounterAxisAlignment="Center">
                                            <PathIcon Data="{StaticResource Icon_Thumb_Down_Off}"
                                                        Foreground="{ThemeResource PrimaryBrush}" />
                                            <TextBlock Text="{Binding Dislikes.Count}"
                                                        Foreground="{ThemeResource PrimaryBrush}"
                                                        Style="{StaticResource LabelLarge}" />
                                        </utu:AutoLayout>
                                    </ToggleButton.Content>
                                    <ut:ControlExtensions.AlternateContent>
                                        <utu:AutoLayout Orientation="Horizontal"
                                                        Spacing="8"
                                                        CounterAxisAlignment="Center">
                                            <PathIcon Data="{StaticResource Icon_Thumb_Down}"
                                                        Foreground="{ThemeResource PrimaryBrush}" />
                                            <TextBlock Text="{Binding Dislikes.Count}"
                                                        Foreground="{ThemeResource PrimaryBrush}"
                                                        Style="{StaticResource LabelLarge}" />
                                        </utu:AutoLayout>
                                    </ut:ControlExtensions.AlternateContent>
                                </ToggleButton>
                            </utu:AutoLayout>
                        </utu:AutoLayout>
                    </utu:AutoLayout>
                </DataTemplate>
            </muxc:ItemsRepeater.ItemTemplate>
        </muxc:ItemsRepeater>
    </DataTemplate>
</uer:FeedView>
```

- The [code-behind](https://github.com/unoplatform/uno.chefs/blob/d9e419affa5b34db2a22707e48d1e0d029899bad/src/Chefs/Presentation/RecipeDetailsModel.cs#L41-L49) is just the list state of the Reviews and Like/Dislike tasks that are hooked up to the service/API.

```csharp
public IListState<Review> Reviews => ListState
    .Async(this, async ct => await _recipeService.GetReviews(Recipe.Id, ct))
    .Observe(_messenger, r => r.Id);

public async ValueTask Like(Review review, CancellationToken ct) =>
    await _recipeService.LikeReview(review, ct);

public async ValueTask Dislike(Review review, CancellationToken ct) =>
    await _recipeService.DislikeReview(review, ct);
```

![Reviews feed](/doc/assets/reviews-feed.png)

## Styles used

Here's a list of the Chefs defined styles used in various areas above:

- ChefsOutlinedButtonStyle

```xml
<Style x:Key="ChefsOutlinedButtonStyle"
       TargetType="Button"
       BasedOn="{StaticResource OutlinedButtonStyle}">
    <Setter Property="Padding" Value="24,20" />
    <Setter Property="CornerRadius" Value="4" />
    <Setter Property="Height" Value="59" />
    <Setter Property="Foreground" Value="{ThemeResource PrimaryBrush}" />
    <Setter Property="BorderBrush" Value="{ThemeResource OutlineBrush}" />
    <Setter Property="BorderThickness" Value="1" />
</Style>
```

- ChefsPrimaryButtonStyle

```xml
<Style x:Key="ChefsPrimaryButtonStyle"
       TargetType="Button"
       BasedOn="{StaticResource FilledButtonStyle}">
    <Setter Property="Padding" Value="24,20" />
    <Setter Property="CornerRadius" Value="4" />
    <Setter Property="Height" Value="59" />
    <Setter Property="Background" Value="{ThemeResource PrimaryBrush}" />
    <Setter Property="Foreground" Value="{ThemeResource OnPrimaryBrush}" />
    <Setter Property="BorderBrush" Value="{ThemeResource PrimaryBrush}" />
</Style>
```

- ChefsPrimaryTextBoxStyle

```xml
<Style x:Key="ChefsPrimaryTextBoxStyle"
       TargetType="TextBox"
       BasedOn="{StaticResource OutlinedTextBoxStyle}">
    <Setter Property="PlaceholderForeground" Value="{ThemeResource OnSurfaceMediumBrush}" />
    <Setter Property="BorderBrush" Value="{ThemeResource OutlineVariantBrush}" />
    <Setter Property="BorderThickness" Value="1" />
    <Setter Property="CornerRadius" Value="4" />
    <Setter Property="Foreground" Value="{ThemeResource OnSurfaceBrush}" />
</Style>
```

- ChefsTonalButtonStyle

```xml
<Style x:Key="ChefsTonalButtonStyle"
       TargetType="Button"
       BasedOn="{StaticResource FilledTonalButtonStyle}">
    <Setter Property="Padding" Value="16,20,24,20" />
    <Setter Property="CornerRadius" Value="4" />
</Style>
```

- ChefsNavigationButtonStyle

```xml
<Style x:Key="ChefsNavigationBarStyle"
       BasedOn="{StaticResource NavigationBarStyle}"
       TargetType="utu:NavigationBar">
    <Setter Property="MainCommandStyle" Value="{StaticResource ChefsMainCommandStyle}" />
    <Setter Property="Background" Value="{ThemeResource SurfaceInverseBrush}" />
    <Setter Property="Foreground" Value="{ThemeResource OnSurfaceInverseBrush}" />
    <Setter Property="FontSize" Value="24" />
</Style>
```

## Improvements for Hot Design QoL

1. Standard Page Shell with NavBar & Content Region

- Every page uses a grid (or similar container) with two rows.
- Row 0: A section hosting the NavigationBar (with back/close buttons).
- Row 1: A flexible content area (often wrapped in a ScrollViewer) containing the page's main UI.

2. FeedView with ItemsRepeater for List/Grid Displays

- Multiple pages (e.g., HomePage, NotificationsPage, RecipeDetails, SearchPage, FavoriteRecipesPage) display collections of items using a combination of `<uer:FeedView>` and `<muxc:ItemsRepeater>`.

3. FlipView Carousel

- Both the WelcomePage and LiveCookingPage implement a carousel using FlipView(s) (with navigation buttons and a pips pager) to display sequential content.
