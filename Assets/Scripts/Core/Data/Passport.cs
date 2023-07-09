using System.Collections.Generic;
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

		public List<(string content, bool check)> InfoAsList()
		{
			return new List<(string content, bool check)>
			{
				NAME,
				DOB,
				Sex,
				ISS,
				EXP,
				ID
			};
		}
	}
}