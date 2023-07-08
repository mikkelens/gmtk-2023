using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Core
{
	public class DialogueScript : MonoBehaviour
	{
		[Serializable]
		private class DialogueOption
		{
			[field: SerializeField] public string Content { get; private set; } = "New Dialogue Option";
			[field: SerializeField] public float SusAmount { get; private set; } = 0.1f;
		}
		[Serializable]
		private class DialogueCheck
		{
			[field: SerializeField] public DialogueOption Option1 { get; private set; }
			[field: SerializeField] public DialogueOption Option2 { get; private set; }
			[field: SerializeField] public DialogueOption Option3 { get; private set; }
		}
		[SerializeField] private List<DialogueCheck> dialogueList;
		[ShowInInspector, ReadOnly] private List<DialogueCheck> _remainingDialogue;
		private DialogueCheck _currentCheck;

		[SerializeField, Required] private TextMeshProUGUI option1Text;
		[SerializeField, Required] private TextMeshProUGUI option2Text;
		[SerializeField, Required] private TextMeshProUGUI option3Text;

		private void Start()
		{
			_remainingDialogue = new List<DialogueCheck>(dialogueList);
			SetNextCheck();
		}

		private void SetNextCheck()
		{
			if (_remainingDialogue.FirstOrDefault() is not { } next)
			{
				Debug.Log("No more dialogue checks...");
				PersistentGameManager.Instance.NextSegment();
				return;
			}
			option1Text.text = next.Option1.Content;
			option2Text.text = next.Option2.Content;
			option3Text.text = next.Option3.Content;
			_currentCheck = next;
			_remainingDialogue.Remove(next);
			Debug.Log("Updated Current Check");
		}

		public void ChooseOption1()
		{
			SelectDialogueOption(_currentCheck.Option1);
		}
		public void ChooseOption2()
		{
			SelectDialogueOption(_currentCheck.Option2);
		}
		public void ChooseOption3()
		{
			SelectDialogueOption(_currentCheck.Option3);
		}
		private void SelectDialogueOption(DialogueOption option)
		{
			Debug.Log($"Selected Dialogue {option}");
			PersistentGameManager.Instance.SusMeter += option.SusAmount;
			SetNextCheck();
		}
	}
}