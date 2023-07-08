using System;
using System.Collections.Generic;
using System.Linq;
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
		private List<DialogueCheck> _remainingDialogue;
		private DialogueCheck _currentCheck;

		private void Start()
		{
			_remainingDialogue = new List<DialogueCheck>(dialogueList);
			GetNextCheck();
		}

		private DialogueCheck GetNextCheck()
		{
			if (_remainingDialogue.FirstOrDefault() is not { } next)
			{
				PersistentGameManager.Instance.NextSegment();
				return null;
			}

			return next;
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

			_currentCheck = GetNextCheck();
		}
		private void DialogueConsequence(float sus)
		{

		}
	}
}