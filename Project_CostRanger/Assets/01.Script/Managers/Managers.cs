using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    // 각각의 매니저들을 private 변수로 선언
    private ResourceManager resource;
    private PoolManager pool;
    private ObjectManager obj;
    private SoundManager sound;
    private DialogManager dialog;
    private EventManager event_;
    private GameManager game;
    private CoroutineManager routine;
    private UIManager ui;
    private DataManager data;
    private ScreenManager screen;
    private SceneManager scene;
    private BattleManager battle;
    private StageManager stage;

    // 각각의 매니저들에 대한 public 프로퍼티를 추가
    public static ResourceManager Resource { get { return Instance?.resource; } }
    public static PoolManager Pool { get { return Instance?.pool; } }
    public static ObjectManager Object { get { return Instance?.obj; } }
    public static SoundManager Sound { get { return Instance?.sound; } }
    public static DialogManager Dialog { get { return Instance?.dialog; } }
    public static EventManager Event { get { return Instance?.event_; } }
    public static UIManager UI { get { return Instance?.ui; } }
    public static DataManager Data { get { return Instance?.data; } }
    public static ScreenManager Screen { get { return Instance?.screen; } }
    public static SceneManager Scene { get { return Instance?.scene; } }
    public static BattleManager Battle { get { return Instance?.battle; } }

    public static StageManager Stage { get { return Instance?.stage; } }

    public static GameManager Game { get { return Instance?.game; } }
    public static CoroutineManager Routine { get { return Instance.routine; } }
    

    //어플이 실행 되면 초기화
    [RuntimeInitializeOnLoadMethod]
    public static void CreateManager()
    {
        Instance.Init();
    }

    //Scene의 오브젝트에 의해 매니저가 호출되어 생성되었을 시 초기화가 되게 하기 위해서
    public void Awake()
    {
        Init();
    }

    public void Update()
    {
        Event.Update();
    }

    //초기화
    public void Init()
    {
        if (isInit) return;
        isInit = true;
        Instance.CreateManagers();
    }

    //각 매니저들 생성
    private void CreateManagers()
    {
        resource = new ResourceManager(); 
        pool = new PoolManager(); 
        obj = new ObjectManager();
        sound = new SoundManager();
        dialog = new DialogManager();
        event_ = new EventManager();
        ui = new UIManager();
        data = new DataManager();
        screen = new ScreenManager();
        scene = new SceneManager();
        battle = new BattleManager();
        stage = new StageManager();

        routine = CoroutineManager.Instance;
        game = GameManager.Instance;
    }
}
