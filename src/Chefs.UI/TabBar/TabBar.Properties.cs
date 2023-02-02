﻿using System;
using System.Collections.Generic;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Chefs.UI
{
	public partial class TabBar
	{
		#region SelectedItem
		public object? SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static DependencyProperty SelectedItemProperty { get; } =
			DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(TabBar), new PropertyMetadata(null, OnPropertyChanged));
		#endregion

		#region SelectedIndex
		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public static DependencyProperty SelectedIndexProperty { get; } =
			DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(TabBar), new PropertyMetadata(-1, OnPropertyChanged));
		#endregion

		#region TemplateSettings
		public TabBarTemplateSettings TemplateSettings
		{
			get => (TabBarTemplateSettings)GetValue(TemplateSettingsProperty);
			private set => SetValue(TemplateSettingsProperty, value);
		}
		public static DependencyProperty TemplateSettingsProperty { get; } =
			DependencyProperty.Register(nameof(TemplateSettings), typeof(TabBarTemplateSettings), typeof(TabBar), new PropertyMetadata(null));
		#endregion

		#region Orientation
		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public static DependencyProperty OrientationProperty { get; } =
			DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(TabBar), new PropertyMetadata(Orientation.Horizontal, OnPropertyChanged));
		#endregion

		private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			var owner = (TabBar)sender;
			owner.OnPropertyChanged(args);
		}
	}
}
