using System;

namespace PekoBot.Core.Extensions
{
	public static class GenericExtensions
	{
		public static T ToEnum<T>(this string value)
		{
			return (T) Enum.Parse(typeof(T), value, true);
		}
	}
}