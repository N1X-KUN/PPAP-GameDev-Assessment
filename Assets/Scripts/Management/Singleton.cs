using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        instance = (T)this;
        Debug.Log($"Singleton {typeof(T)} initialized successfully.");

        // Allow nested GameObjects to persist across scenes
        Transform rootTransform = transform;
        while (rootTransform.parent != null)
        {
            rootTransform = rootTransform.parent;
        }
        DontDestroyOnLoad(rootTransform.gameObject);
    }

    public static void ResetAllSingletons()
    {
        foreach (var singleton in FindObjectsOfType<T>())
        {
            Destroy(singleton.gameObject); // Destroy each Singleton GameObject
        }
        instance = null; // Clear the static instance
    }

    public static void ResetInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}
