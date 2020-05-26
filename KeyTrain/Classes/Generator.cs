using System;
using System.Text;

namespace KeyTrain.Classes
{
	public static class Generator
	{
		private static readonly Random _random = new Random();

		public static string Generate(int length)
		{
			var result = new StringBuilder();

			var last = -1;
			var current = -1;

			while (result.Length < length - 1)
			{
				if (result.Length > 0)
					result.Append(Example.Separators[_random.Next(Example.Separators.Length)]);

				while (last == current)
					current = _random.Next(Example.Collection.Length);

				last = current;

				result.Append(Example.Collection[current]);
			}

			if (result.Length > length - 1)
			{
				var lastWordLength = Example.Collection[current].Length;
				result = result.Remove(result.Length - lastWordLength - 1, lastWordLength + 1);
			}

			return result.Append(";").ToString();
		}
	}
}
