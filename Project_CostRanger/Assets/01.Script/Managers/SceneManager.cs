using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManager 
{
    private Transform sceneTrans;
    public Transform SceneTrans
    {
        get
        {
            if (sceneTrans == null)
            {
                GameObject go = GameObject.Find("@SceneTrans");
                if (go == null)
                    go = new GameObject(name: "@SceneTrans");
                sceneTrans = go.transform;
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
            return sceneTrans;
        }
    }
    private Define.Scene currentScene;
    private bool isLoading = false;
    private Action loadCallback;

    public void LoadScene(Define.Scene _scene, Action _loadCallback = null)
    {
        if (isLoading) return;
        isLoading = true;
        loadCallback = _loadCallback;
        string sceneName = _scene.ToString();
        Managers.Pool.Clear();

        RemoveScene(currentScene, () =>
        {
            currentScene = _scene;
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            async.completed += (_) => 
            { 
                AddScene(sceneName);
                isLoading = false;
            };
        });
    }

    public void RemoveScene(Define.Scene _scene, Action _callback = null)
    {
        BaseScene bs = null;
        switch (_scene)
        {
            case Define.Scene.Main:
                bs = SceneTrans.GetComponent<MainScene>();
                break;

            case Define.Scene.Stage:
                bs = SceneTrans.GetComponent<StageScene>();
                break;

            default:
                _callback?.Invoke();
                return;
        }
        if(bs != null)
        {
            bs.Clear();
            UnityEngine.Object.Destroy(bs);
        }
        _callback?.Invoke();
    }

    // ¾À Ãß°¡
    public void AddScene(string _sceneName)
    {
        BaseScene bs = null;
        Define.Scene addScene = Util.ParseEnum<Define.Scene>(_sceneName);
        //Managers.Data.LoadSceneData(addScene);
        switch (addScene)
        {
            case Define.Scene.Main:
                bs = SceneTrans.gameObject.AddComponent<MainScene>();
                break;

            case Define.Scene.Stage:
                bs = SceneTrans.gameObject.AddComponent<StageScene>();
                break;

            default:
                loadCallback?.Invoke();
                loadCallback = null;
                return;
        }

        bs.Init(() =>
        {
            loadCallback?.Invoke();
            loadCallback = null;
        });
    }
}
