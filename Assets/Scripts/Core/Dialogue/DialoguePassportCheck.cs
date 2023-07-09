using UnityEngine;

namespace Core.Dialogue
{
	[CreateAssetMenu(fileName = "New Passport", menuName = "DialogueChecks/Passport")]
	public class DialoguePassportCheck : DialogueCheck
	{
		[field: SerializeField] public string Prompt { get; private set; } = "Let me see your passport.";
		[field: SerializeField] public string AnswerButtonText { get; private set; } = "Here";
	}
}