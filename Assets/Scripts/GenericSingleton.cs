using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                    instance = new GameObject().AddComponent<T>();
            }
            return instance;
        }
    }

}
