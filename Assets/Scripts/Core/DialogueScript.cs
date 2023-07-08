using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class DialogueScript : MonoBehaviour
	{
		[Serializable]
		private class DialogueReaction
		{
			[field: SerializeField] public string Response { get; private set; } = "(New Dialogue Reaction)";
			[field: SerializeField] public float SusAmount { get; private set; } = 0.1f;
		}
		[Serializable]
		private class DialogueOption
		{
			[field: SerializeField] public string Content { get; private set; } = "(New Dialogue Option)";
			[field: SerializeField] public DialogueReaction Reaction { get; private set; }
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

		[SerializeField, Required] private TextAnimateScript guardText;

		[SerializeField, Required] private TextAnimateScript option1Text;
		[SerializeField, Required] private TextAnimateScript option2Text;
		[SerializeField, Required] private TextAnimateScript option3Text;

		[SerializeField, Required] private Button option1Button;
		[SerializeField, Required] private Button option2Button;
		[SerializeField, Required] private Button option3Button;

		// [SerializeField] private float startDelay = 2f;
		[SerializeField] private float endDelay = 3f;

		private void Start()
		{
			_remainingDialogue = new List<DialogueCheck>(dialogueList);
			SetButtonTextEmpty();
			SetButtonInteractable(false);
			StartCoroutine(WaitThenStart());
		}

		private IEnumerator WaitThenStart()
		{
			Debug.Log("Waiting for text to stop animating before starting.");
			yield return new WaitWhile(AnyTextAnimating);
			Debug.Log("Text stopped animating, starting...");
			SetNextCheck();
		}

		private bool AnyTextAnimating()
		{
			return guardText.Animating | option1Text.Animating | option2Text.Animating | option3Text.Animating;
		}

		private void SetNextCheck()
		{
			if (_remainingDialogue.FirstOrDefault() is not { } next)
			{
				Debug.Log("No more dialogue checks...");
				StartCoroutine(EndConversation());
				return;
			}
			_currentCheck = next;
			_remainingDialogue.Remove(next);
			StartCoroutine(DisplayOptionsThenEnable());
		}
		private IEnumerator DisplayOptionsThenEnable()
		{
			SetButtonInteractable(false);
			StartCoroutine(option1Text.AnimateText(_currentCheck.Option1.Content));
			StartCoroutine(option2Text.AnimateText(_currentCheck.Option2.Content));
			StartCoroutine(option3Text.AnimateText(_currentCheck.Option3.Content));
			yield return new WaitWhile(AnyTextAnimating);
			SetButtonInteractable(true);
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
		private void SelectDialogueOption(DialogueOption chosenOption)
		{
			StartCoroutine(guardText.AnimateText(chosenOption.Reaction.Response));
			StartCoroutine(WaitThenContinue(chosenOption));
		}

		private IEnumerator WaitThenContinue(DialogueOption chosenOption)
		{
			SetButtonTextEmpty();
			SetButtonInteractable(false);
			yield return new WaitWhile(AnyTextAnimating);
			PersistentGameManager.Instance.SusMeter += chosenOption.Reaction.SusAmount;
			SetNextCheck();
		}

		private IEnumerator EndConversation()
		{
			// remove options
			SetButtonTextEmpty();
			SetButtonInteractable(false);

			// wait then continue/return
			yield return new WaitForSeconds(endDelay);
			PersistentGameManager.Instance.NextSegment();
		}

		private void SetButtonInteractable(bool setInteractable)
		{
			option1Button.interactable = setInteractable;
			option2Button.interactable = setInteractable;
			option3Button.interactable = setInteractable;
		}

		private void SetButtonTextEmpty()
		{
			option1Text.ClearText();
			option2Text.ClearText();
			option3Text.ClearText();
		}

	}
}