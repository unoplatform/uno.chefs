# Uno.Chefs Control Variants

Chefs contains many combinations and variants of controls that we would like to offer as standalone elements in the Hot Design Toolbox. The goal is to provide small, individual control variants that would be common for developers to use. Below is a comprehensive list of potential variants:

- [Button with Icon](#button-with-icon)
- [Navigation Button with Icon](#navigation-button-with-icon)
- [Navigation Button with Data](#navigation-button-with-data)
- [TextBox/PasswordBox with Icon](#textboxpasswordbox-with-icon)
- [ToggleButton with Alternate Content](#togglebutton-with-alternate-content)

## Button with Icon

```xml
<Button Content="Button">
    <ut:ControlExtensions.Icon>
        <FontIcon Glyph="&#xf787;" />
    </ut:ControlExtensions.Icon>
</Button>
```

## Navigation Button with Icon

```xml
<Button Content="Button"
        uen:Navigation.Data="{Binding Data}"
        uen:Navigation.Request="MainPage">
    <ut:ControlExtensions.Icon>
        <FontIcon Glyph="&#xf787;" />
    </ut:ControlExtensions.Icon>
</Button>
```

## Navigation Button with Data

```xml
<Button Content="Button"
        uen:Navigation.Data="{Binding Data}"
        uen:Navigation.Request="MainPage">
</Button>
```

## TextBox/PasswordBox with Icon

```xml
<TextBox PlaceholderText="Text"
         Text="{Binding Text, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    <ut:ControlExtensions.Icon>
        <FontIcon Glyph="&#xf787;" />
    </ut:ControlExtensions.Icon>
</TextBox>
```

```xml
<PasswordBox PlaceholderText="Text"
             Password="{Binding Password, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    <ut:ControlExtensions.Icon>
        <FontIcon Glyph="&#xf787;" />
    </ut:ControlExtensions.Icon>
</PasswordBox>
```

## ToggleButton with Alternate Content

```xml
<ToggleButton Command="{Binding ToggleCommand}">
    <ToggleButton.Content>
        <FontIcon Glyph="&#xf787;" />
    </ToggleButton.Content>
    <ut:ControlExtensions.AlternateContent>
        <FontIcon Glyph="&#xf8ff;" />
    </ut:ControlExtensions.AlternateContent>
</ToggleButton>
```
