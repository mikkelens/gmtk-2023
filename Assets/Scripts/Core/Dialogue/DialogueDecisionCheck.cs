using System;
using UnityEngine;

namespace Core.Dialogue
{
	[CreateAssetMenu(fileName = "New Decision", menuName = "DialogueChecks/Decision")]
	public class DialogueDecisionCheck : DialogueCheck
	{
		[Serializable]
		public class Decision
		{
			[Serializable]
			public class Reaction
			{
				[field: SerializeField] public string Reply { get; private set; } = "(New Dialogue Response)";
				[field: SerializeField] public int SusAmount { get; private set; } = 1;
			}

			[field: SerializeField] public string Content { get; private set; } = "(New Dialogue Option)";
			[field: SerializeField] public Reaction Response { get; private set; }
		}

		[field: SerializeField] public Decision Option1 { get; private set; }
		[field: SerializeField] public Decision Option2 { get; private set; }
		[field: SerializeField] public Decision Option3 { get; private set; }
	}
}