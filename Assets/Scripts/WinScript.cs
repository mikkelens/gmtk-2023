using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField, Required] private Animator earthAnim;
    [SerializeField, Required] private SpriteRenderer goodJobSpriteRenderer;
    [SerializeField] private float goodJobDelay = 1f;
    [SerializeField, Required] private Button restartButton;

    private static readonly int blowEarth = Animator.StringToHash("BlowEarth");

    private void Start()
    {
        StartCoroutine(ShowWinScreen());
    }

    private IEnumerator ShowWinScreen()
    {
        yield return new WaitForSeconds(startDelay);
        earthAnim.SetTrigger(blowEarth);
        yield return new WaitForSeconds(goodJobDelay);
        goodJobSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(startDelay);
        restartButton.gameObject.SetActive(true);
    }
}