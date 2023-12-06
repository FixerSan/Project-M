using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class UIManager 
{
    private int order = 10;                                     // 그려지는 순서 여유 선언
    private int toastOrder = 500;                               // 인스턴트 메세지 그려지는 여유 선언

    public Dictionary<Define.UIType, UIPopup> activePopups = new Dictionary<Define.UIType, UIPopup>();
    public Dictionary<BaseController, UIHPBar> hpBars = new Dictionary<BaseController, UIHPBar>();
    private Stack<UIPopup> popupStack = new Stack<UIPopup>();   // 팝업 스택
    private Queue<UIToast> toastQueue = new Queue<UIToast>();   // 인스턴트 메세지 스택
    private Queue<UIPopup_WorldText> worldTextQueue = new Queue<UIPopup_WorldText>();   // 인스턴트 메세지 스택
    private EventSystem eventSystem = null;                     // 이벤트 시스템 선언
    private UIScene sceneUI = null;                             // SceneUI 선언
    public UIScene SceneUI { get { return sceneUI; } }          // SceneUI 프로퍼티 선언

    private CanvasGroup blackPanel;
    public CanvasGroup BlackPanel
    {
        get
        {
            if (blackPanel == null)
            {
                GameObject go = GameObject.Find("@BlackPanel");
                if(go == null)
                {
                    go = Managers.Resource.Instantiate("BlackPanel");
                    go.name = "@BlackPanel";
                    UnityEngine.Object.DontDestroyOnLoad(go);
                    blackPanel = go.GetOrAddComponent<CanvasGroup>();
                }
            }
            return blackPanel;
        }
    }
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }                                   // UI 위치 선언

    private UISkillScreen_Ranger rangerSkillScreen;
    public UISkillScreen_Ranger RangerSkillScreen
    {
        get
        {
            if(rangerSkillScreen == null)
                rangerSkillScreen = Managers.Resource.Instantiate(typeof(UISkillScreen_Ranger).Name).GetOrAddComponent<UISkillScreen_Ranger>();
            return rangerSkillScreen;
        }
    }

    private UISkillScreen_Enemy enemySkillScreen;
    public UISkillScreen_Enemy EnemySkillScreen
    {
        get
        {
            if (enemySkillScreen = null)
                enemySkillScreen = Managers.Resource.Instantiate(typeof(UISkillScreen_Enemy).Name).GetOrAddComponent<UISkillScreen_Enemy>();
            return enemySkillScreen;
        }
    }



    // 이벤트 시스템 설정
    public void SetEventSystem()
    {
        GameObject es = Managers.Resource.Instantiate("EventSystem");
        eventSystem = es.GetOrAddComponent<EventSystem>();
    }

    public void SetCanvas(GameObject _go, bool _sort = true, int _sortOrder = 0, bool _isToast = false)
    {
        GameObject go = GameObject.Find("EventSystem");
        if (go == null) SetEventSystem();
        Canvas canvas = _go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Managers.Screen.CameraController.Camera;
        canvas.overrideSorting = true;

        CanvasScaler cs = _go.GetOrAddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);

        _go.GetOrAddComponent<GraphicRaycaster>();

        if (_sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = _sortOrder;
        }
        if (_isToast)
        {
            toastOrder++;
            canvas.sortingOrder = toastOrder;
        }
    }


    public UIPopup_WorldText MakeWorldText(string _description, Vector2 _position, Define.TextType _type)
    {
        if (string.IsNullOrEmpty(_description))
            return null;

        GameObject go = Managers.Resource.Instantiate("UIPopup_WorldText", Root.transform);
        UIPopup_WorldText text = go.GetOrAddComponent<UIPopup_WorldText>();
        worldTextQueue.Enqueue(text);
        text.Init(_description, _position, _type);
        return text;
    }

    public void CloseAllWorldText()
    {
        while (worldTextQueue.Count > 0)
        {
            Managers.Resource.Destroy(worldTextQueue.Dequeue().gameObject);
        }
    }

    // SceneUI 생성
    public T ShowSceneUI<T>(string _name = null) where T : UIScene
    {
        if(string.IsNullOrEmpty(_name))
        {
            _name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"{_name}");
        
        T _sceneUI = go.GetOrAddComponent<T>();
        sceneUI = _sceneUI;

        go.transform.SetParent(Root.transform);

        return _sceneUI;
    }

    public void ClearScene(UIScene _scene)
    {
        sceneUI = null;
    }

    // 팝업 생성
    public T ShowPopupUI<T>(string _name = null, bool _pooling = false) where T : UIPopup
    {
        if (string.IsNullOrEmpty(_name))
        {
            _name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"{_name}",_pooling:_pooling);
        if (_pooling) Managers.Pool.CreatePool(go);
        T popup = go.GetOrAddComponent<T>();
        popupStack.Push(popup);
        activePopups.Add(Util.ParseEnum<Define.UIType>(_name), popup);
        go.transform.SetParent(Root.transform);
        return popup;
    }


    // 팝업 삭제체크
    public void ClosePopupUI(UIPopup _popup)
    {
        if (popupStack.Count == 0)
            return;

        if(popupStack.Peek() != _popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI(); 
    }

    // 팝업 삭제 기능
    private void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UIPopup popup = popupStack.Pop();
        if(activePopups.ContainsKey(Util.ParseEnum<Define.UIType>(popup.GetType().Name)))
            activePopups.Remove(Util.ParseEnum<Define.UIType>(popup.GetType().Name));
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        order--;
    }

    // 팝업 전부 삭제
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    // 인스턴트 메세지 생성
    public UIToast ShowToast(string _description)
    {
        string name = nameof(UIToast);
        GameObject go = Managers.Resource.Instantiate($"{name}", _pooling: true);
        UIToast popup = go.GetOrAddComponent<UIToast>();
        popup.SetInfo(_description);
        toastQueue.Enqueue(popup);
        go.transform.SetParent(Root.transform);
        return popup;
    }

    // 인스턴트 메세지 삭제 기능
    public void CloseToastUI()
    {
        if(toastQueue.Count == 0)
        {
            return;
        }

        UIToast toast = toastQueue.Dequeue();
        toast.Refresh();
        Managers.Resource.Destroy(toast.gameObject);
        toast = null;
        toastOrder--;
    }

    // 팝업 전부 삭제
    public void CloseAllToastUI()
    {
        while (toastQueue.Count > 0)
        {
            Managers.Resource.Destroy(toastQueue.Dequeue().gameObject);
        }
    }

    public GameObject ShowPrepareRanger(string _key)
    {
        GameObject go = Managers.Resource.Instantiate(_key);
        go.GetOrAddComponent<SortingGroup>().sortingOrder = order;
        order++;
        return go;
    }

    public GameObject ShowPrepareEnemy(string _key)
    {
        GameObject go = Managers.Resource.Instantiate(_key);
        go.GetOrAddComponent<SortingGroup>().sortingOrder = order;
        order++;
        return go;
    }


    public void SetRangerSkillScreen(int _rangerUID, Action _callback = null)
    {
        RangerSkillScreen.gameObject.SetActive(true);
        RangerSkillScreen.ApplyRangerSkill(Managers.Data.GetRangerInfoData(_rangerUID), _callback);
    }

    public void SetEnemySkillScreen(int _enemyUID, Action _callback = null)
    {
        EnemySkillScreen.gameObject.SetActive(true);
        EnemySkillScreen.ApplyEnemySkill(Managers.Data.GetEnemyInfoData(_enemyUID), _callback);
    }

    public void CloseRangerSkillScreen()
    {
        RangerSkillScreen.gameObject.SetActive(false);
    }

    public void CloseEnemySkillScreen()
    {
        EnemySkillScreen.gameObject.SetActive(false);
    }

    // 팝업 카운트 반환
    public int GetPopupCount()
    {
        return popupStack.Count;
    }

    //HP바 생성
    public void SetHPbar(BaseController _controller)
    {
        UIHPBar hpBar = Managers.Resource.Instantiate("UIHPBar").GetOrAddComponent<UIHPBar>();
        hpBar.transform.SetParent(Root.transform);
        hpBar.Init(_controller);

        hpBars.Add(_controller, hpBar);
    }

    //HP바 삭제
    public void ReleseHPBar(BaseController _controller)
    {
        if(hpBars.TryGetValue(_controller, out UIHPBar _hpBar))
        {
            Managers.Resource.Destroy(_hpBar.gameObject);
            hpBars.Remove(_controller);
        }
    }

    public void ReleseAllHPBar()
    {
        foreach (var item in hpBars)
            Managers.Resource.Destroy(item.Value.gameObject);

        hpBars.Clear();
    }

    public UISlot_StageRanger CreateStageRangerSlot(Transform _parent = null)
    {
        UISlot_StageRanger slot = Managers.Resource.Instantiate("Slot_StageRanger").GetOrAddComponent<UISlot_StageRanger>();
        slot.transform.SetParent(_parent);
        slot.transform.localScale = Vector3.one;
        return slot;
    }


    // 초기화
    public void Clear()
    {
        CloseAllPopupUI();
        
        Time.timeScale = 1;
        sceneUI = null;
    }
}
