using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KeyTrain.Classes
{
	public static class Helper
	{
		public static (List<T>, List<T>) InitGridContainer<T>(int count, Grid container, Style topItemStyle, Style bottomItemStyle) where T : FrameworkElement, new()
		{
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (container == null)
				throw new ArgumentNullException(nameof(container));
			if (topItemStyle == null)
				throw new ArgumentNullException(nameof(topItemStyle));
			if (bottomItemStyle == null)
				throw new ArgumentNullException(nameof(bottomItemStyle));

			container.ColumnDefinitions.Clear();

			var tops = new List<T>(count);
			var bots = new List<T>(count);

			for (int i = 0; i < count; i++)
			{
				container.ColumnDefinitions.Add(new ColumnDefinition());
				addTopItem(i);
				addBotItem(i);
			}

			void addTopItem(int column)
			{
				var item = new T { Style = topItemStyle };
				Grid.SetColumn(item, column);
				container.Children.Add(item);
				tops.Add(item);
			}

			void addBotItem(int column)
			{
				var item = new T { Style = bottomItemStyle };
				Grid.SetColumn(item, column);
				Grid.SetRow(item, 2);
				container.Children.Add(item);
				bots.Add(item);
			}

			return (tops, bots);
		}
	}
}
