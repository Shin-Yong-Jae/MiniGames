using UnityEngine;

public abstract class SingletonNoCreate<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // ReSharper disable once StaticMemberInGenericType
    private static bool _hasInstance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();

            return _instance;
        }
    }

    protected virtual bool DontDestroyOnload => false;

    protected void Awake()
    {
        if (CheckAnotherInstance())
        {
            return;
        }

        // 접근이 없는(_instance == null인) singleton은 여기서 할당
        T _ = Instance;
        if (DontDestroyOnload)
        {
            DontDestroyOnLoad(gameObject);
        }

        OnAwake();
    }

    protected abstract void OnAwake();

    private bool CheckAnotherInstance()
    {
        T[] objects = FindObjectsOfType<T>();
        int count = objects.Length;
        if (count >= 2)
        {
            foreach (T a in objects)
            {
                if (a != _instance)
                {
                    Debug.LogError($"Duplicated singleton object [{a.gameObject.name}] destoryed");
                    Destroy(a.gameObject);
                }
            }

            return true;
        }

        return false;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            _hasInstance = false;
        }
    }
}