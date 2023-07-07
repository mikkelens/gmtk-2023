using System;
using System.Collections;
using System.Collections.Generic;
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
    #endif

    #region Inspector
    [SerializeField] private float smoothLoadTime = 0.25f;
    [SerializeField, Required] private PersistentUIManager uiPrefab;

    [Header("Levels and Scenes")]
    [SerializeField, Required] private SceneReference gameHub;

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

    #endregion
    private SceneState _sceneState;
    private enum SceneState
    {
        Loading,
        InHub,
        InLevel
    }

    #if UNITY_EDITOR // Scene debugging stuff
    private void OnEnable()
    {
        hubHotkey.performed += _ => ReturnToHub();
        nextLevelHotkey.performed += _ => NextLevel();
        hubHotkey.Enable();
        nextLevelHotkey.Enable();
    }
    private void OnDisable()
    {
        hubHotkey.Disable();
        nextLevelHotkey.Disable();
    }
    #endif

    public void ReturnToHub()
    {
        if (_sceneState != SceneState.InLevel) return;
        StartCoroutine(SmoothLoad(gameHub, true));
    }

    public void NextLevel()
    {

    }

    public void GameOver()
    {
        throw new NotImplementedException();
    }

    private IEnumerator SmoothLoad(SceneReference scene, bool isHub)
    {
        _sceneState = SceneState.Loading;
        float t;
        // fade to black
        float transitionStartTime = Time.time;
        do
        {
            t = transitionStartTime.TimeSince() / smoothLoadTime;
            GetUIInfallible().SetTransitionImageWithT(t);
            yield return new WaitForSeconds(Time.deltaTime);
        } while (t < 1f);

        SceneManager.LoadScene(scene.ScenePath);

        // fade from black
        transitionStartTime = Time.time;
        do
        {
            t = transitionStartTime.TimeSince() / smoothLoadTime;
            GetUIInfallible().SetTransitionImageWithT(t);
            yield return new WaitForSeconds(Time.deltaTime);
        } while (t < 1f);

        _sceneState = isHub ? SceneState.InHub : SceneState.InLevel;
    }

    private PersistentUIManager GetUIInfallible()
    {
        return PersistentUIManager.Exists ? PersistentUIManager.Instance : Instantiate(uiPrefab);
    }
}