using UnityEngine;
using System.Collections;

public class Type<T> where T : MonoBehaviour
{

    public static bool isType(Transform obj, out T t)
    {
        if (obj.GetComponent<T>())
        {
            t = obj.GetComponent<T>();
            return true;
        }
        t = default(T);
        return false;
    }

    public static bool isType(GameObject obj, out T t)
    {
        if (obj.GetComponent<T>())
        {
            t = obj.GetComponent<T>();
            return true;
        }
        t = default(T);
        return false;
    }

    public static bool isType(Entity obj, out T t)
    {
        if (obj.GetComponent<T>())
        {
            t = obj.GetComponent<T>();
            return true;
        }
        t = default(T);
        return false;
    }

    public static bool isType(Transform obj)
    {
        return obj.GetComponent<T>();
    }

    public static bool isType(GameObject obj)
    {
        return obj.GetComponent<T>();
    }

    public static bool isType(Entity obj)
    {
        return obj.GetComponent<T>();
    }

}
