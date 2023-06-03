using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllManager : Singleton<SceneControllManager>
{
    #region Variables

    protected override bool DontDestroyOnload => true;

    public CanvasGroup canvasGroupLoading;

    public SceneType CurrentSceneType
    {
        get;
        private set;
    }

    public SceneType NextLoadingSceneType { get; private set; }
    public bool IsWait { get; private set; }

    private Action loadingAction;
    #endregion Variables

    #region Unity Methods
    protected override void OnAwake()
    {
        canvasGroupLoading.alpha = 0f;
        canvasGroupLoading.gameObject.SetActive(false);

        SceneManager.sceneLoaded += OnLoadSceneFinish;

        // 임시 Scene 이동.
        LoadScene(SceneType.Lobby, true);
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadSceneFinish;

        base.OnDestroy();
    }
    #endregion Unity Methods

    #region Main Methods
    private void OnLoadSceneFinish(Scene scene, LoadSceneMode sceneMode)
    {
        if (!Enum.TryParse<SceneType>(scene.name, out SceneType finishSceneType))
        {
            throw new Exception("Not Exist Scene");
        }

        CurrentSceneType = finishSceneType;
    }

    /// <summary>
    /// 일단 로딩씬으로 이동하고, 로딩씬에서 호출할 씬을 설정할 때 사용
    /// </summary>
    public void SetNextLoadingScene(SceneType nextScene)
    {
        ResetWaitFlag();

        NextLoadingSceneType = nextScene;
        LoadScene(SceneType.Loading, false);
    }

    /// <summary>
    /// 단순히 씬만 이동할 때 사용
    /// ex) 로딩씬으로 일단 씬을 옮겨놓고 비동기 응답이 왔을 때 LoadScene을 호출하는?
    /// </summary>    
    public void JustLoadScene(SceneType sceneType)
    {
        IsWait = true;
        LoadScene(sceneType, false);
    }

    /// <summary>
    /// 기본적인 씬로딩을 할 때 사용
    /// </summary>
    public void LoadScene(SceneType loadScene, bool isAsync)
    {
        Debug.Log($"Try Load Scene  To {loadScene.ToString()} [{(isAsync ? "Async" : "Sync")}]");
        
        if (isAsync)
        {
            StopAllCoroutines();

            StartCoroutine(AsyncLoadScene(loadScene));
        }
        else
        {
            SceneManager.LoadScene(loadScene.ToString());
        }
    }

    private IEnumerator AsyncLoadScene(SceneType sceneType)
    {
        var operate = SceneManager.LoadSceneAsync(sceneType.ToString());

        operate.allowSceneActivation = false;

        while (!operate.isDone)
        {
            if (operate.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        if (loadingAction != null)
        {
            loadingAction?.Invoke();
            yield return GameManager.Instance.Get_WaitForSeconds(1.5f);
        }

        yield return GameManager.Instance.Get_WaitForSeconds(0.5f);
        operate.allowSceneActivation = true;
    }

    /// <summary>
    /// 로딩 중 필요한 액션이 있는 경우 설정
    /// </summary>
    public void SetLoadingAction(Action action) => loadingAction = action;

    public void ResetWaitFlag()
    {
        IsWait = false;
    }
    #endregion Main Methods
}
