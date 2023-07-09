using System.Diagnostics.CodeAnalysis;

namespace Core.Data
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class BombData
	{
		public BombData(char[] pin)
		{
			PIN = pin; // please make this a 4 digit number <3
		}
		// generated at game startup
		public readonly char[] PIN;

		// generated at bomb cooking
		public int mixingOffness;
		public int incorrectWireNumber;
	}
}