using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Tools.Types;
using Tools.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PersistentGameManager : PersistentSingleton<PersistentGameManager>
{
    #if UNITY_EDITOR // Scene debugging stuff
    [SerializeField] private InputAction hubHotkey = new InputAction(type: InputActionType.Button);
    [SerializeField] private InputAction nextLevelHotkey = new InputAction(type: InputActionType.Button);
    [SerializeField] private InputAction gameOverHotkey = new InputAction(type: InputActionType.Button);
    #endif

    #region Inspector
    [SerializeField] private float smoothLoadTime = 0.25f;
    [SerializeField, Required, AssetsOnly] private PersistentUIManager uiPrefab;

    [Header("Levels and Scenes")]
    [SerializeField, Required] private SceneReference gameHub;
    [SerializeField, Required] private SceneReference gameOver;

    [Serializable]
    public class Level
    {
        [field: SerializeField] public SceneReference LevelScene { get; private set; }

        [field: SerializeField] public SceneReference FailScene { get; private set; } // one for each of the ways to fail

        [field: SerializeField] public SceneReference WinScene { get; private set; }
    }

    private bool ValidateLevelList => levels.Count > 0;
    [ValidateInput(nameof(ValidateLevelList), "No levels assigned!", InfoMessageType.Warning)]
    [SerializeField] private List<Level> levels;

    [SerializeField] private SceneState sceneState;
    private enum SceneState
    {
        Loading,
        InHub,
        InLevel
    }
    #endregion

    #if UNITY_EDITOR // Scene debugging stuff
    private void OnEnable()
    {
        hubHotkey.performed += _ => ReturnToHub();
        nextLevelHotkey.performed += _ => NextLevel();
        gameOverHotkey.performed += _ => GameOver();
        hubHotkey.Enable();
        nextLevelHotkey.Enable();
        gameOverHotkey.Enable();
    }
    private void OnDisable()
    {
        hubHotkey.Disable();
        nextLevelHotkey.Disable();
        gameOverHotkey.Disable();
    }
    #endif

    public void ReturnToHub()
    {
        if (sceneState != SceneState.InLevel) return;
        StartCoroutine(SmoothLoad(gameHub, true));
    }

    public void NextLevel()
    {
        if (sceneState != SceneState.InHub) return;
        if (levels.FirstOrDefault() is not { } nextLevel)
        {
            Debug.LogError("No more levels to go to!");
            return;
        }
        levels.Remove(nextLevel);
        StartCoroutine(SmoothLoad(nextLevel.LevelScene, false));
    }

    public void GameOver()
    {
        StartCoroutine(SmoothLoad(gameOver, false));
    }

    private IEnumerator SmoothLoad(SceneReference scene, bool targetIsHub)
    {
        sceneState = SceneState.Loading;
        float t;
        // fade to black
        float transitionStartTime = Time.time;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            t = transitionStartTime.TimeSince() / smoothLoadTime;
            GetUIInfallible().SetTransitionImageWithT(t);
        } while (t <= 1f);

        SceneManager.LoadScene(scene.ScenePath);

        // fade from black
        transitionStartTime = Time.time;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            t = transitionStartTime.TimeSince() / smoothLoadTime;
            GetUIInfallible().SetTransitionImageWithT(1f - t);
        } while (t <= 1f);

        sceneState = targetIsHub ? SceneState.InHub : SceneState.InLevel;
    }

    private PersistentUIManager GetUIInfallible()
    {
        return PersistentUIManager.Exists ? PersistentUIManager.Instance : Instantiate(uiPrefab);
    }
}