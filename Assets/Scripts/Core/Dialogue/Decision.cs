using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Dialogue
{
	[CreateAssetMenu(fileName = "New Decision", menuName = "DialogueChecks/Decision")]
	public class Decision : DialogueCheck
	{
		[Serializable]
		public class Option
		{
			[field: SerializeField] public string Reply { get; private set; }
			[field: SerializeField] public string Response { get; private set; }
			[field: SerializeField] public int SusAmount { get; private set; } = 1;
		}

		[field: SerializeField] public string Prompt { get; private set; } = "?"; // can be question or just request
		[field: SerializeField] public List<Option> Options { get; private set; }
	}
}