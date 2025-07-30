using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelLoader : MonoBehaviour
{
    [Header("Clone to calculate level rotation")]
    [SerializeField] private Transform cloneToCalculateRotation;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private LevelDataContainer levelDataContainer;

    [Header("Textured Material Level")]
    [SerializeField] private Material texturedMaterial;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private List<Tween> _autoCenterLevelTweens;
    private int _maxLevel;
    private Transform _currentLevelContainer;
    private Transform _pivotAdjust;
    private bool _isNotAutoCenterLevel;
    private bool _isSetLevelGradientBound;
    private bool _isNewLoopLevel;
    private int _nextLevel;
    private bool _isLoopLevel;
    #endregion

    #region EVENT
    public static event Action startLevelEvent;
    public static event Action setLevelNumberEvent;
    public static event Action<int> setLevelScrewNumberEvent;
    public static event Action<int> setMultiPhaseLevelScrewNumberEvent;
    public static event Action showSceneTransitionEvent;
    public static event Action setIsLevelLoadedEvent;
    public static event Action<bool> enableSwipingEvent;
    public static event Action<float> setTargetOrthographicSizeEvent;
    public static event Action<bool> showLoadLevelErrorEvent;
    #endregion

    private void Awake()
    {
        _tweens = new List<Tween>();
        _autoCenterLevelTweens = new List<Tween>();

        currentLevel.Load();

        _maxLevel = levelDataContainer.MaxLevel;

        GoLevel(currentLevel.Value);

        GamePersistentVariable.hasPlayerReachedGameplay = true;
    }

    void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
        CommonUtil.StopAllTweens(_autoCenterLevelTweens);
    }

    #region LEVEL
    private async void LoadLevel()
    {
        // // NO INTERNET
        // if (Application.internetReachability == NetworkReachability.NotReachable)
        // {
        //     showLoadLevelErrorEvent?.Invoke(true);

        //     return;
        // }
        // else
        // {
        //     showLoadLevelErrorEvent?.Invoke(false);
        // }
        // //

        int nextLevel = 1;

        bool isLoopLevel = false;

        // if (!UserData.IsLoopLevel)
        // {
        //     if (currentLevel.Value <= _maxLevel)
        //     {
        //         nextLevel = currentLevel.Value;
        //     }
        //     else
        //     {
        //         nextLevel = GetLoopLevel();

        //         await UserData.SetIsLoopLevel(true);
        //         await UserData.SetIsLastLevelBeforeLoop(_maxLevel);

        //         isLoopLevel = true;
        //     }
        // }
        // else
        // {
        //     if (currentLevel.Value - UserData.NumLevelLoop >= _maxLevel)
        //     {
        //         nextLevel = GetLoopLevel();
        //     }
        //     else
        //     {
        //         nextLevel = currentLevel.Value - UserData.NumLevelLoop;
        //     }

        //     isLoopLevel = true;
        // }

        // nextLevel = Mathf.Clamp(nextLevel, 1, _maxLevel);

        // GamePersistentVariable.levelPrefab = nextLevel;

        // setLevelNumberEvent?.Invoke();

        // nextLevel = RemoteConfigLevelManager.GetReplacedLevelIndex(nextLevel);

        string levelPrefab = $"Level {nextLevel}";

        // // REMOTE LEVELS
        // if (nextLevel > 10)
        // {
        //     if (!GamePersistentVariable.isRemoteLevelsDownloaded)
        //     {
        //         showLoadLevelErrorEvent?.Invoke(true);

        //         return;
        //     }
        //     else
        //     {
        //         showLoadLevelErrorEvent?.Invoke(false);
        //     }
        // }

        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(levelPrefab);

        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject level = Instantiate(op.Result, transform);

                _currentLevelContainer = level.transform;

                AutoAssignScrewFactionMultiPhaseLevel(level);

                texturedMaterial.SetVector("_MainTextureOffset", new Vector2(0, 0));

                if (isLoopLevel)
                {
                    RandomizeLevelModelColor(level);
                }
                else
                {
                    SaferioTracking.TrackLevelStart(currentLevel.Value);
                }
            }
        };

        showSceneTransitionEvent?.Invoke();
    }

    private async void GoLevel(int level)
    {
        await Task.Delay(500);

        if (gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }

        currentLevel.Value = level;

        currentLevel.Save();

        startLevelEvent?.Invoke();

        LoadLevel();

        GamePersistentVariable.isLevelDirty = false;
    }

    private void NextLevel()
    {
        GoLevel(currentLevel.Value + 1);
    }

    private void PrevLevel()
    {
        GoLevel(currentLevel.Value - 1);
    }

    private void Replay()
    {
        GamePersistentVariable.isPreventLoadLevelData = true;

        GoLevel(currentLevel.Value);
    }
    #endregion

    #region UTIL
    private int GetLoopLevel()
    {
        int currentLoopLevel = DataUtility.Load(GameConstants.CURRENT_LEVEL_LOOP, -1);

        _isNewLoopLevel = currentLoopLevel == -1;

        if (currentLoopLevel == -1)
        {
            int randomLoopLevel = UnityEngine.Random.Range(6, _maxLevel);

            DataUtility.Save(GameConstants.CURRENT_LEVEL_LOOP, randomLoopLevel);

            return randomLoopLevel;
        }
        else
        {
            return currentLoopLevel;
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    #endregion
}
