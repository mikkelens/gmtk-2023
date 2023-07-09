using UnityEngine;

namespace Core.Dialogue
{
	[CreateAssetMenu(fileName = "New Prompt", menuName = "DialogueChecks/Prompt")]
	public class Prompt : DialogueCheck
	{
		[field: SerializeField] public string GuardPrompt { get; private set; } = "Let me see your [thing].";
		[field: SerializeField] public string AnswerButtonText { get; private set; } = "Here";
	}
}