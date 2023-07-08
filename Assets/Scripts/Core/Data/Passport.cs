using System.Diagnostics.CodeAnalysis;

namespace Core.Data
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public struct Passport
	{
		public (string content, bool check) NAME;
		public (string content, bool check) DOB;
		public (string content, bool check) Sex;
		public (string content, bool check) ISS;
		public (string content, bool check) EXP;
		public (string content, bool check) ID;
	}
}