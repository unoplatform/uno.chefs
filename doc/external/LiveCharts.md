---
uid: Uno.Recipes.LiveCharts
---

# How to integrate LiveCharts controls

## Problem

Mobile and desktop applications often need to display complex data in an easy-to-understand format. Charts are an excellent tool for this, but integrating charting functionality into applications, especially cross-platform ones, can be challenging due to the variety of data sources and formats, as well as the need for responsive and intuitive user interaction.

## Solution

**LiveCharts** is a flexible and customizable charting library that can be integrated into any .NET application, including Uno Platform apps. It provides various chart types, from basic line and bar charts to more complex heat maps and financial charts.

### App startup configuration

```csharp
public class App : Application
{
    // Code omitted for brevity

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // Code omitted for brevity

        LiveCharts.Configure(config =>
            config
            .HasMap<NutritionChartItem>((nutritionChartItem, point) =>
            {
                // here we use the index as X, and the nutrition value as Y
                return new(point, nutritionChartItem.Value);
            })
        );

        // Code omitted for brevity
    }
}
```

### Custom chart control

```xml
<UserControl xmlns:lvc="using:LiveChartsCore.SkiaSharpView.WinUI">

  <!-- Code omitted for brevity -->

  <lvc:PieChart x:Name="pieChart" />

  <lvc:CartesianChart x:Name="cartesianChart"
                      TooltipPosition="Hidden" />
</UserControl>
```

### Chart control code-behind

[!code-csharp[](../../Chefs/Views/Controls/ChartControl.xaml.cs#L28-L54)]

### Using ChartControl

[!code-xml[](../../Chefs/Views/RecipeDetailsPage.xaml#L434)]

Doughtnut and horizontal bars chart on the Recipe details page:
<table>
  <tr>
    <th>LiveCharts</th>
  </tr>
  <tr>
    <td><img src="../assets/livecharts.png" width="400px" alt="LiveCharts"/></td>
  </tr>
</table>

## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L129)
- [Custom Chart Control](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Controls/ChartControl.xaml#L2)
- [Chart Control Code-Behind](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Controls/ChartControl.xaml.cs#L28)
- [Chart Item Model](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Business/Models/NutritionChartItem.cs#L5)
- [Chart Control Instance](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RecipeDetailsPage.xaml#L434-L435)

## Documentation

LiveCharts

- [Uno Platform Installation](https://livecharts.dev/docs/UnoWinUi/2.0.0-rc1/Overview.Installation)
- [Doughnut Chart](https://livecharts.dev/docs/UnoWinUi/2.0.0-beta.920/samples.pies.doughnut)
- [Bars Chart](https://livecharts.dev/docs/UnoWinUi/2.0.0-beta.920/samples.bars.withBackground)
