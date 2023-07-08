using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Sirenix.OdinInspector;
using Tools.Types;
using Tools.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Core
{
    public class PersistentGameManager : PersistentSingleton<PersistentGameManager>
    {
        #if UNITY_EDITOR // Scene debugging stuff
        [SerializeField] private InputAction hubHotkey = new InputAction(type: InputActionType.Button);
        [SerializeField] private InputAction nextLevelHotkey = new InputAction(type: InputActionType.Button);
        [SerializeField] private InputAction nextSegmentHotkey = new InputAction(type: InputActionType.Button);
        [SerializeField] private InputAction gameOverHotkey = new InputAction(type: InputActionType.Button);
        #endif

        #region Inspector
        [SerializeField] private float smoothLoadTime = 0.25f;
        [SerializeField, Required, AssetsOnly] private PersistentUIManager uiPrefab;

        [Header("Levels and Scenes")]
        [SerializeField] private SceneType sceneType;
        [SerializeField, Required] private SceneReference gameHub;
        [SerializeField, Required] private SceneReference gameOver;

        private bool ValidateListNonEmpty => levels.Count > 0;
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private List<SceneReference> allLevelScenes;
        private bool ListIsRepresentedInBuildSettings
        {
            get
            {
                allLevelScenes = new List<SceneReference>
                {
                    gameHub,
                    gameOver
                };
                foreach (Level level in levels)
                {
                    allLevelScenes.AddRange(level.PreparationSegments.Select(x => x.SegmentScene));
                    allLevelScenes.AddRange(level.ActionSegments.Select(x => x.SegmentScene));
                    allLevelScenes.AddRange(level.ActionSegments.Select(x => x.FailScene));
                    allLevelScenes.AddRange(level.Ending);
                }
                allLevelScenes.RemoveAll(x => x.ScenePath == string.Empty);

                if (!allLevelScenes.All(scene => EditorBuildSettings.scenes.Any(x => x.path == scene.ScenePath)))
                    return false;
                return true;
            }
        }
        private const string LevelGroupID = "LevelGroup";
        [VerticalGroup(LevelGroupID)]
        [DisableIf(nameof(ListIsRepresentedInBuildSettings))]
        [Button("Add missing scenes to build settings")]
        private void AddMissingScenesToBuildSettings()
        {
            List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();
            foreach (EditorBuildSettingsScene wantedScene in allLevelScenes.Select(x => new EditorBuildSettingsScene(x.ScenePath, true)))
            {
                if (!buildScenes.Contains(wantedScene))
                {
                    buildScenes.Add(wantedScene);
                }
            }
            EditorBuildSettings.scenes = buildScenes.ToArray();
        }
        [VerticalGroup(LevelGroupID)]
        #endif
        [ValidateInput(nameof(ListIsRepresentedInBuildSettings), "Build settings is missing some of these scenes!", ContinuousValidationCheck = true)]
        [ValidateInput(nameof(ValidateListNonEmpty), "No levels assigned!", InfoMessageType.Warning)]
        [SerializeField] private List<Level> levels;
        #endregion

        private PersistentUIManager _uiManager;

        #if UNITY_EDITOR // Scene debugging stuff
        private void OnEnable()
        {
            hubHotkey.performed += _ => ReturnToHub();
            nextLevelHotkey.performed += _ => StartNewLevel();
            nextSegmentHotkey.performed += _ => NextSegment();
            gameOverHotkey.performed += _ => GameOver();
            hubHotkey.Enable();
            nextLevelHotkey.Enable();
            nextSegmentHotkey.Enable();
            gameOverHotkey.Enable();
        }
        private void OnDisable()
        {
            hubHotkey.Disable();
            nextLevelHotkey.Disable();
            nextSegmentHotkey.Disable();
            gameOverHotkey.Disable();
        }
        #endif

        private bool _loading;
        private enum SceneType
        {
            Hub,
            LevelSegment
        }
        private List<Level> _remainingLevels;
        public List<Level> RemainingLevelsCopy => new List<Level>(_remainingLevels);
        private Level _currentLevel;
        private Level.Segment _currentSegment;

        private void Start()
        {
            _remainingLevels = new List<Level>(levels);
            _uiManager = GetUIInfallible();
            #if UNITY_EDITOR
            if (!_loading)
            {
                StartCoroutine(WaitThenFadeUp());
            }
        }
        private IEnumerator WaitThenFadeUp() // only used when starting playmode
        {
            yield return FadeToBlack(0.08f);
            yield return FadeFromBlack(smoothLoadTime);
            #endif
        }

        public void ReturnToHub()
        {
            if (_loading || sceneType == SceneType.Hub) return;
            sceneType = SceneType.Hub;
            StartCoroutine(SmoothLoad(gameHub));
        }

        public void StartNewLevel()
        {
            if (_loading || sceneType == SceneType.LevelSegment) return;

            _currentLevel = _remainingLevels.FirstOrDefault();
            if (_currentLevel == null)
            {
                Debug.LogError("No more levels to go to!");
                return;
            }

            _currentLevel.remainingSegments = new List<Level.Segment>(_currentLevel.PreparationSegments);
            _currentLevel.remainingSegments.AddRange(_currentLevel.ActionSegments); // flow directly

            _currentSegment = _currentLevel.remainingSegments.FirstOrDefault();
            if (_currentSegment == null)
            {
                Debug.LogError("No first segment in level!");
                return;
            }

            sceneType = SceneType.LevelSegment;

            _remainingLevels.Remove(_currentLevel);
            _currentLevel.remainingSegments.Remove(_currentSegment);
            StartCoroutine(SmoothLoad(_currentSegment.SegmentScene));
        }

        public void NextSegment()
        {
            if (_loading || sceneType == SceneType.Hub) return;

            if (_currentLevel == null)
            {
                Debug.LogWarning("Current level is null!");
                ReturnToHub();
                return;
            }

            _currentSegment = _currentLevel.remainingSegments.FirstOrDefault();
            if (_currentSegment == null)
            {
                Debug.Log("No next segment, returning to hub.");
                ReturnToHub();
                return;
            }

            _currentLevel.remainingSegments.Remove(_currentSegment);
            StartCoroutine(SmoothLoad(_currentSegment.SegmentScene));
        }

        public void SegmentFailure() // assume this would only be called where it makes sense
        {
            if (_loading || sceneType == SceneType.Hub) return;

            Debug.Log("SUS SPOTTED");

            if (_currentSegment is not Level.ActionSegment actionSegment)
            {
                Debug.LogError("Current segment is not an action segment!");
                return;
            }

            _currentLevel = null;
            _currentSegment = null;
            StartCoroutine(SmoothLoad(actionSegment.FailScene));
        }

        public void GameOver() // assume this would only be called when it needs to
        {
            _currentLevel = null;
            _currentSegment = null;
            StartCoroutine(SmoothLoad(gameOver));
        }

        private IEnumerator SmoothLoad(SceneReference scene)
        {
            _loading = true;

            yield return FadeToBlack(smoothLoadTime);

            SceneManager.LoadScene(scene.ScenePath);

            yield return FadeFromBlack(smoothLoadTime);

            _loading = false;
        }
        private IEnumerator FadeToBlack(float fadeTime)
        {
            float time = 0;
            float t;
            do
            {
                yield return new WaitForSeconds(Time.deltaTime);
                time += Time.deltaTime;
                t = time / fadeTime;
                _uiManager.SetTransitionImageWithT(t);
            } while (t <= 1f);
        }
        private IEnumerator FadeFromBlack(float fadeTime)
        {
            float time = 0;
            float t;
            do
            {
                yield return new WaitForSeconds(Time.deltaTime);
                time += Time.deltaTime;
                t = time / fadeTime;
                _uiManager.SetTransitionImageWithT(1f - t);
            } while (t <= 1f);
        }

        public int SusMeter { get ; set ; }

        public PassportData PassportData { get; set; }
        public BombData BombData { get; set; }

        private PersistentUIManager GetUIInfallible()
        {
            return PersistentUIManager.Exists ? PersistentUIManager.Instance : Instantiate(uiPrefab);
        }
    }
}