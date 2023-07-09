using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Dialogue
{
	public class DialogueScript : MonoBehaviour
	{
		[SerializeField] private List<DialogueCheck> checkList;
		[ShowInInspector, ReadOnly] private List<DialogueCheck> _remainingChecks;
		private DialogueCheck _currentCheck;

		[Header("Guard")]
		[SerializeField, Required] private TextAnimateScript guardText;
		[SerializeField, Required] private AnimatedGuardScript guard;
		[SerializeField] private int passportMistakeSusIncrease = 8;
		[SerializeField] private int guardSusAmount = 20;
		[SerializeField] private int guardMegaSusAmount = 50;
		[SerializeField] private string detectionString = "!";
		[SerializeField] private string undetectedString = "Alright, carry on.";

		[Header("Decision references")]
		[SerializeField, Required] private TextAnimateScript option1Text;
		[SerializeField, Required] private TextAnimateScript option2Text;
		[SerializeField, Required] private TextAnimateScript option3Text;
		[SerializeField, Required] private Button option1Button;
		[SerializeField, Required] private Button option2Button;
		[SerializeField, Required] private Button option3Button;

		[Header("Generic prompt stuff")]
		[SerializeField, Required] private TextAnimateScript promptAnswerText;
		[SerializeField, Required] private Button promptAnswerButton;


		[SerializeField] private float pauseAfterMessage = 0.3f;
		[SerializeField] private float susDetectedDelay = 1f;
		[SerializeField] private float endDelay = 2.5f;

		private void Start()
		{
			_remainingChecks = new List<DialogueCheck>(checkList);
			// SetDecisionButtonTextEmpty();
			SetDecisionButtonsVisible(false);
			SetPromptButtonVisible(false);
			StartCoroutine(WaitThenStart());
		}
		private void SetPromptButtonVisible(bool setVisible)
		{
			promptAnswerButton.gameObject.SetActive(setVisible);
		}
		private void SetPromptButtonInteractable(bool setInteractable)
		{
			promptAnswerButton.interactable = setInteractable;
		}

		private IEnumerator WaitThenStart()
		{
			yield return new WaitWhile(AnyTextAnimating);
			GoNextCheck();
		}

		private bool AnyTextAnimating()
		{
			return guardText.Animating | option1Text.Animating | option2Text.Animating | option3Text.Animating;
		}

		private void GoNextCheck()
		{
			if (_remainingChecks.FirstOrDefault() is not { } next)
			{
				Debug.Log("No more dialogue checks...");
				StartCoroutine(UndetectedEnd());
				return;
			}
			_currentCheck = next;
			_remainingChecks.Remove(next);
			if (_currentCheck is DialogueDecisionCheck decisionCheck)
			{
				StartCoroutine(DisplayDecisionOptionsThenEnable(decisionCheck));
			}
			else if (_currentCheck is DialoguePassportCheck passportCheck)
			{
				StartCoroutine(PassportCheck(passportCheck));
			}
		}
		#region Decision check
		private IEnumerator DisplayDecisionOptionsThenEnable(DialogueDecisionCheck decisionCheck)
		{
			SetDecisionButtonsInteractable(false);
			StartCoroutine(option1Text.AnimateTextReplace(decisionCheck.Option1.Content));
			StartCoroutine(option2Text.AnimateTextReplace(decisionCheck.Option2.Content));
			StartCoroutine(option3Text.AnimateTextReplace(decisionCheck.Option3.Content));
			yield return new WaitWhile(AnyTextAnimating);
			yield return new WaitForSeconds(pauseAfterMessage);
			SetDecisionButtonsInteractable(true);
		}
		public void ChooseOption1()
		{
			if (_currentCheck is not DialogueDecisionCheck decisionCheck)
			{
				Debug.LogError("Not a decision check, please hide this button!");
				return;
			}
			SelectDialogueOption(decisionCheck.Option1);
		}
		public void ChooseOption2()
		{
			if (_currentCheck is not DialogueDecisionCheck decisionCheck)
			{
				Debug.LogError("Not a decision check, please hide this button!");
				return;
			}
			SelectDialogueOption(decisionCheck.Option2);
		}
		public void ChooseOption3()
		{
			if (_currentCheck is not DialogueDecisionCheck decisionCheck)
			{
				Debug.LogError("Not a decision check, please hide this button!");
				return;
			}
			SelectDialogueOption(decisionCheck.Option3);
		}
		private void SelectDialogueOption(DialogueDecisionCheck.Decision chosenDecision)
		{
			StartCoroutine(guardText.AnimateTextReplace(chosenDecision.Response.Reply));
			StartCoroutine(WaitThenDecide(chosenDecision));
		}
		private IEnumerator WaitThenDecide(DialogueDecisionCheck.Decision chosenDecision)
		{
			SetDecisionButtonTextEmpty();
			SetDecisionButtonsInteractable(false);
			yield return new WaitWhile(AnyTextAnimating);
			yield return new WaitForSeconds(pauseAfterMessage);
			yield return AddSusAndContinue(chosenDecision.Response.SusAmount);
		}
		private void SetDecisionButtonsVisible(bool setVisible)
		{
			option1Button.gameObject.SetActive(setVisible);
			option2Button.gameObject.SetActive(setVisible);
			option3Button.gameObject.SetActive(setVisible);
		}
		private void SetDecisionButtonsInteractable(bool setInteractable)
		{
			option1Button.interactable = setInteractable;
			option2Button.interactable = setInteractable;
			option3Button.interactable = setInteractable;
		}
		private void SetDecisionButtonTextEmpty()
		{
			option1Text.ClearText();
			option2Text.ClearText();
			option3Text.ClearText();
		}
		#endregion

		#region Passport check
		private bool HasAnsweredPrompt { get; set; } // set externally
		private IEnumerator PassportCheck(DialoguePassportCheck dialoguePassportCheck)
		{
			HasAnsweredPrompt = false;
			SetPromptButtonVisible(true);
			SetPromptButtonInteractable(false);
			yield return guardText.AnimateTextReplace(dialoguePassportCheck.Prompt); // "can I see your passport"
			yield return new WaitForSeconds(pauseAfterMessage);
			yield return promptAnswerText.AnimateTextReplace(dialoguePassportCheck.AnswerButtonText);
			yield return new WaitForSeconds(pauseAfterMessage);
			SetPromptButtonInteractable(true);
			yield return new WaitUntil(() => HasAnsweredPrompt);
			HasAnsweredPrompt = false;
			SetPromptButtonInteractable(false);

			// calculate sus
			int susIncrease = 0;
			Passport passport = PersistentGameManager.Instance.Passport;
			foreach ((string content, bool check) passportEntry in passport.InfoAsList())
			{
				yield return guardText.AnimateTextReplace(passportEntry.content);
				yield return new WaitForSeconds(pauseAfterMessage);
				if (passportEntry.check) continue;

				susIncrease += passportMistakeSusIncrease;
				yield return guardText.AnimateTextAdd("...");
				yield return new WaitForSeconds(susDetectedDelay);
			}
			SetPromptButtonVisible(false);
			yield return AddSusAndContinue(susIncrease);
		}
		#endregion

		private IEnumerator AddSusAndContinue(int amount)
		{
			AddSus(amount);
			yield return new WaitForSeconds(pauseAfterMessage);
			if (PersistentGameManager.Instance.SusMeter >= guardMegaSusAmount)
			{
				int coinflip = Random.Range(0, 100); // 0-99
				if (coinflip > PersistentGameManager.Instance.SusMeter)
				{
					StartCoroutine(SusDetectedEnd());
					yield break;
				}
			}
			GoNextCheck();
		}

		private void AddSus(int amount)
		{
			PersistentGameManager.Instance.SusMeter += amount;
			guard.GuardSpriteRenderer.sprite = GetGuardSprite(PersistentGameManager.Instance.SusMeter);
		}

		private Sprite GetGuardSprite(int susMeter)
		{
			if (susMeter < guardSusAmount) return guard.NormalSprite;
			if (susMeter < guardMegaSusAmount) return guard.SusSprite;
			return guard.MegaSusSprite;
		}

		private IEnumerator SusDetectedEnd()
		{
			Debug.Log("SUS!!!!");
			// remove options
			SetDecisionButtonTextEmpty();
			SetDecisionButtonsInteractable(false);

			yield return StartCoroutine(guardText.AnimateTextReplace(detectionString));
			yield return new WaitForSeconds(susDetectedDelay);
			PersistentGameManager.Instance.SegmentFailure();
		}

		private IEnumerator UndetectedEnd()
		{
			Debug.Log("Not sus.");
			// remove options
			SetDecisionButtonTextEmpty();
			SetDecisionButtonsInteractable(false);

			// wait then continue/return
			AddSus(-100);
			yield return StartCoroutine(guardText.AnimateTextReplace(undetectedString));
			yield return new WaitForSeconds(endDelay);
			PersistentGameManager.Instance.NextSegment();
		}

	}
}