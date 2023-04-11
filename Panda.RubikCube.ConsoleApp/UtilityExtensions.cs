using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Panda.RubikCube.ConsoleApp
{
	public static class UtilityExtensions
	{
		public static string? GetDisplayName<T>(this T enumValue)
			where T : struct, Enum
			=> enumValue.GetType()?
						.GetMember(enumValue.ToString())?
						.First()?
						.GetCustomAttribute<DisplayAttribute>()?
						.GetName();
	}
}