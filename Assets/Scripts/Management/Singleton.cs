using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // Ensure the instance is not null when accessed
            if (instance == null)
            {
                Debug.LogError($"Instance of {typeof(T)} is not set or has been destroyed.");
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning($"An instance of {typeof(T)} already exists. Destroying duplicate.");
            Destroy(this.gameObject); // Destroy duplicate instance
        }
        else
        {
            instance = (T)this; // Set the singleton instance
            Debug.Log($"Singleton {typeof(T)} initialized successfully.");
        }

        // Ensure the object persists across scene loads
        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Resets the singleton instance by destroying its GameObject and setting the instance to null.
    /// </summary>
    public static void ResetInstance()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject); // Destroy the GameObject associated with the singleton
            instance = null; // Clear the instance reference
        }
    }
}
