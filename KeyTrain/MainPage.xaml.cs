using KeyTrain.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace KeyTrain
{
	public sealed partial class MainPage : Page
	{
		private const int COUNT = 50;

		private readonly int _maxIndex = COUNT;

		private readonly List<TextBlock> _tops;
		private readonly List<TextBlock> _bots;

		private int _index = -1;

		private bool _delay;
		private bool _hasErrors;

		public MainPage()
		{
			this.InitializeComponent();

			(_tops, _bots) = Helper.InitGridContainer<TextBlock>(COUNT, container, topStyle, botStyle);

			Reset();

			Window.Current.Content.KeyUp += OnKeyUp;
		}

		private int Index
		{
			get => _index;
			set
			{
				_index = value;

				if (value >= _maxIndex)
					Reset();
				else
					Grid.SetColumn(caret, value);
			}
		}

		private void Reset()
		{
			if (_delay)
				return;

			var source = Generator.Generate(COUNT);

			for (int i = 0; i < COUNT; i++)
			{
				var current = i < source.Length ? source[i].ToString() : string.Empty;

				_tops[i].Text = current;
				_bots[i].Text = string.Empty;
			}

			caret.Background = caretGreenBrush;
			caret.Visibility = Visibility.Visible;
			Index = 0;

			_hasErrors = false;
		}

		private void OnKeyUp(object sender, KeyRoutedEventArgs e)
		{
			switch (e.Key)
			{
				case VirtualKey.Escape:
					Application.Current.Exit();
					break;
				case VirtualKey.Enter:
					Reset();
					e.Handled = true;
					break;
				default:
					ResetFocus();
					break;
			}
		}

		private async void Content_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (_delay)
				return;

			if (inputBox.Text == string.Empty)
				return;

			var text = _tops[Index].Text;
			var input = inputBox.Text[0].ToString();

			if (text == input)
			{
				_bots[Index].Text = text;
				caret.Background = caretGreenBrush;

				await MoveCaretAsync();
			}
			else
			{
				_hasErrors = true;
				_bots[Index].Text = input;

				caret.Background = caretRedBrush;
			}

			inputBox.Text = string.Empty;
		}

		private Task MoveCaretAsync()
		{
			var nextIndex = Index + 1;
			if (nextIndex >= COUNT || _tops[nextIndex].Text == string.Empty)
			{
				return EndLineWithDelayAsync();
			}
			else
			{
				Index++;
				return Task.CompletedTask;
			}
		}

		private async Task EndLineWithDelayAsync()
		{
			_delay = true;

			if (!_hasErrors)
				_bots.ForEach(x => x.Foreground = perfectBrush);

			caret.Visibility = Visibility.Collapsed;

			await Task.Delay(500);

			_delay = false;

			Reset();

			if (!_hasErrors)
				_bots.ForEach(x => x.Foreground = correctBrush);

			inputBox.Text = string.Empty;
		}

		private void InputBox_Paste(object sender, TextControlPasteEventArgs e)
		{
			e.Handled = true;
		}

		private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RequestedTheme = (ElementTheme)Enum.Parse(typeof(ElementTheme), ((ComboBoxItem)themeComboBox.SelectedItem).Content.ToString());
			ResetFocus();
		}

		private void ResetFocus()
		{
			inputBox?.Focus(FocusState.Keyboard);
		}
	}
}
