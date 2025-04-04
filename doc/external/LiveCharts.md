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

  private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
  {
    _recipe = args.NewValue as Recipe;

    if (_recipe != null)
    {
      BuildColumnChart();
      BuildDoughnutChart();
    }
  }

  private void BuildColumnChart()
  {
    // Code omitted for brevity
  }

  private void BuildDoughnutChart()
  {
    // Code omitted for brevity
  }
}
```


### Using ChartControl

```xml
<ctrl:ChartControl DataContext="{Binding Recipe}" />
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

- [App startup](https://github.com/unoplatform/uno.chefs/blob/f3b5a256aa7afd621389089ddea75d309e28c373/src/Chefs/App.cs#L61)
- [Custom chart control](https://github.com/unoplatform/uno.chefs/blob/f3b5a256aa7afd621389089ddea75d309e28c373/src/Chefs/Views/Controls/ChartControl.xaml#L2)
- [Chart control code-behind](https://github.com/unoplatform/uno.chefs/blob/f3b5a256aa7afd621389089ddea75d309e28c373/src/Chefs/Views/Controls/ChartControl.xaml.cs#L28)
- [Chart item model](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Business/Models/NutritionChartItem.cs#L5)
- [Chart control instance](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/Views/RecipeDetailsPage.xaml#L427)

## Documentation

LiveCharts

- [Uno Platform installation](https://livecharts.dev/docs/UnoWinUi/2.0.0-rc1/Overview.Installation)
- [Doughnut chart](https://livecharts.dev/docs/UnoWinUi/2.0.0-beta.920/samples.pies.doughnut)
- [Bars chart](https://livecharts.dev/docs/UnoWinUi/2.0.0-beta.920/samples.bars.withBackground)