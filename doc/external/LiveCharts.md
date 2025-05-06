---
uid: Uno.Recipes.LiveCharts
---

# How to integrate LiveCharts controls

## Problem

Mobile and desktop applications often need to display complex data in an easy-to-understand format. Charts are an excellent tool for this, but integrating charting functionality into applications, especially cross-platform ones, can be challenging due to the variety of data sources and formats, as well as the need for responsive and intuitive user interaction.

## Solution

**LiveCharts** is a flexible and customizable charting library that can be integrated into any .NET application, including Uno Platform apps. It provides various chart types, from basic line and bar charts to more complex heat maps and financial charts.

### Code behind configuration

```csharp
public sealed partial class RecipeDetailsPage : Page
{
  public RecipeDetailsPage()
  {
    this.InitializeComponent();

    LiveCharts.Configure(config =>
      config
        .HasMap<NutritionChartItem>((nutritionChartItem, point) =>
        {
          // here we use the index as X, and the nutrition value as Y 
          return new(point, nutritionChartItem.Value);
        })
    );
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

```csharp
public sealed partial class ChartControl : UserControl
{
 private Recipe? _recipe;
 public ChartControl()
 {
    this.InitializeComponent();

    _recipe = DataContext as Recipe;
    if (_recipe != null)
    {
      BuildColumnChart();
      BuildDoughnutChart();
    }

    DataContextChanged += OnDataContextChanged;
 }
 ...
```

### Using ChartControl

```xml
<ctrl:ChartControl DataContext="{Binding Recipe}" Grid.Row="1" />
```

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

- [Code behind Configuration](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/Views/RecipeDetailsPage.xaml.cs#L11-L18)
- [Custom Chart Control](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Controls/ChartControl.xaml)
- [Chart Control Code-Behind](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/Controls/ChartControl.xaml.cs#)
- [Chart Item Model](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Business/Models/NutritionChartItem.cs)
- [Chart Control Instance](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/RecipeDetailsPage.xaml#L434-L435)

## Documentation

LiveCharts

- [Uno Platform Installation](https://livecharts.dev/docs/UnoWinUi/2.0.0-rc1/Overview.Installation)
- [Doughnut Chart](https://livecharts.dev/docs/UnoWinUi/2.0.0-beta.920/samples.pies.doughnut)
- [Bars Chart](https://livecharts.dev/docs/UnoWinUi/2.0.0-beta.920/samples.bars.withBackground)
