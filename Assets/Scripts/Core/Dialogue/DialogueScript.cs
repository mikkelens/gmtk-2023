using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Sirenix.OdinInspector;
using Tools.Types;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Dialogue
{
	public class DialogueScript : Singleton<DialogueScript>
	{
		[SerializeField] private List<DialogueCheck> checkList;
		[ShowInInspector, ReadOnly] private List<DialogueCheck> _remainingChecks;
		private DialogueCheck _currentCheck;

		[Header("Guard")]
		[SerializeField, Required] private Animator passportAnim;
		[SerializeField, Required] private TextAnimateScript guardText;
		[SerializeField, Required] private AnimatedGuardScript guard;
		[SerializeField] private int passportMistakeSusIncrease = 8;
		[SerializeField] private int guardSusAmount = 20;
		[SerializeField] private int guardMegaSusAmount = 50;
		[SerializeField] private string detectionString = "!";
		[SerializeField] private string undetectedString = "Alright, carry on.";

		[Header("Option references")]
		[SerializeField, Required] private RectTransform dialogueButtonParent;
		[SerializeField, Required, AssetsOnly] private Button dialogueOptionButtonPrefab;
		private readonly List<Button> _spawnedButtons = new List<Button>();
		private readonly List<TextAnimateScript> _spawnedButtonTexts = new List<TextAnimateScript>();

		[SerializeField] private float pauseAfterMessage = 0.3f;
		[SerializeField] private float susDetectedDelay = 1f;
		[SerializeField] private float endDelay = 2.5f;
		private static readonly int giveTrigger = Animator.StringToHash("GiveTrigger");

		private void Start()
		{
			_remainingChecks = new List<DialogueCheck>(checkList);
			SetButtonsActive(false);
			StartCoroutine(WaitThenStart());
		}

		private IEnumerator WaitThenStart()
		{
			yield return new WaitWhile(AnyTextAnimating);
			GoNextCheck();
		}

		private bool AnyTextAnimating()
		{
			return guardText.Animating | _spawnedButtonTexts.Any(x => x.Animating);
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
			if (_currentCheck is Decision decisionCheck)
			{
				StartCoroutine(DisplayDecisionOptionsThenEnable(decisionCheck));
			}
			else if (_currentCheck is Prompt passportCheck)
			{
				StartCoroutine(PassportPromptCheck(passportCheck));
			}
		}

		#region Decision check
		private IEnumerator DisplayDecisionOptionsThenEnable(Decision decision)
		{
			DestroyButtons();
			yield return guardText.AnimateTextReplace(decision.Prompt);
			yield return new WaitForSeconds(pauseAfterMessage);

			// spawn buttons
			for (int i = 0; i < decision.Options.Count; i++)
			{
				Button newOptionButton = Instantiate(dialogueOptionButtonPrefab, dialogueButtonParent);
				DialogueButtonScript buttonScript = newOptionButton.GetComponent<DialogueButtonScript>();
				buttonScript.MyIndex = i;
				_spawnedButtons.Add(newOptionButton);
				_spawnedButtonTexts.Add(newOptionButton.GetComponentInChildren<TextAnimateScript>());
			}
			SetButtonsActive(true);
			SetButtonsInteractable(false);

			// show text on buttons
			for (int i = 0; i < decision.Options.Count; i++)
			{
				TextAnimateScript optionText = _spawnedButtonTexts[i];
				Decision.Option option = decision.Options[i];
				StartCoroutine(optionText.AnimateTextReplace(option.Reply));
			}
			yield return new WaitWhile(AnyTextAnimating);
			yield return new WaitForSeconds(pauseAfterMessage);
			SetButtonsInteractable(true); // let player interact with buttons, sparking next part
		}
		public void ChooseOption(int optionIndex) // accessed by buttons
		{
			if (_currentCheck is Decision decisionCheck)
			{
				SelectDialogueOption(decisionCheck, optionIndex);
			}
			else if (_currentCheck is Prompt)
			{
				HasAnsweredPrompt = true;
			}
			else
			{
				Debug.LogError("Not a decision check, please hide this button!");
			}
		}
		private void SelectDialogueOption(Decision decision, int optionIndex)
		{
			StartCoroutine(RespondToDecision(decision, optionIndex));
		}
		private IEnumerator RespondToDecision(Decision decision, int optionIndex)
		{
			SetButtonsInteractable(false);
			SetButtonTextEmpty(optionIndex); // all except the one we chose
			Decision.Option option = decision.Options[optionIndex];
			yield return guardText.AnimateTextReplace(option.Response);
			yield return new WaitWhile(AnyTextAnimating);
			yield return new WaitForSeconds(pauseAfterMessage);
			yield return AddSusAndContinue(option.SusAmount);
		}
		#endregion

		private void DestroyButtons()
		{
			foreach (Button spawnedButton in _spawnedButtons)
			{
				Destroy(spawnedButton.gameObject);
			}
			_spawnedButtons.Clear();
			_spawnedButtonTexts.Clear();
		}
		private void SetButtonsActive(bool setActive)
		{
			foreach (Button button in _spawnedButtons)
			{
				button.gameObject.SetActive(setActive);
			}
		}
		private void SetButtonsInteractable(bool setInteractable)
		{
			foreach (Button button in _spawnedButtons)
			{
				button.interactable = setInteractable;
			}
		}
		private void SetButtonTextEmpty(int? indexToIgnore = null)
		{
			for (int i = 0; i < _spawnedButtons.Count; i++)
			{
				if (i == indexToIgnore) continue;
				_spawnedButtonTexts[i].ClearText();
			}
		}

		#region Passport check
		private bool HasAnsweredPrompt { get; set; } // set externally
		private IEnumerator PassportPromptCheck(Prompt prompt)
		{
			DestroyButtons();
			Button promptButton = Instantiate(dialogueOptionButtonPrefab, dialogueButtonParent);
			TextAnimateScript textScript = promptButton.GetComponentInChildren<TextAnimateScript>();
			_spawnedButtons.Add(promptButton);
			_spawnedButtonTexts.Add(textScript);

			HasAnsweredPrompt = false;
			SetButtonsActive(true);
			SetButtonsInteractable(false);
			yield return guardText.AnimateTextReplace(prompt.GuardPrompt); // "can I see your passport"
			yield return new WaitForSeconds(pauseAfterMessage);
			yield return textScript.AnimateTextReplace(prompt.AnswerButtonText);
			yield return new WaitForSeconds(pauseAfterMessage);
			SetButtonsInteractable(true);
			yield return new WaitUntil(() => HasAnsweredPrompt);
			HasAnsweredPrompt = false;
			SetButtonsInteractable(false);

			passportAnim.SetTrigger(giveTrigger);
			float length = 1f;
			yield return new WaitForSeconds(length);
			yield return new WaitForSeconds(pauseAfterMessage);

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
			SetButtonsActive(false);
			passportAnim.gameObject.SetActive(false);
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
			yield return new WaitForSeconds(pauseAfterMessage * 1.5f);
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
			SetButtonTextEmpty();
			SetButtonsInteractable(false);

			yield return StartCoroutine(guardText.AnimateTextReplace(detectionString));
			yield return new WaitForSeconds(susDetectedDelay);
			PersistentGameManager.Instance.SegmentFailure();
		}

		private IEnumerator UndetectedEnd()
		{
			Debug.Log("Not sus.");
			// remove options
			SetButtonTextEmpty();
			SetButtonsInteractable(false);

			// wait then continue/return
			AddSus(-100);
			yield return StartCoroutine(guardText.AnimateTextReplace(undetectedString));
			yield return new WaitForSeconds(endDelay);
			PersistentGameManager.Instance.NextSegment();
		}

	}
}