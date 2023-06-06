using UnityEngine;


public class UIManager : BSingleton<UIManager>
{
    const string PATH_UI = "UI/";

    public static T EnableUI<T>() where T : BUIBaseType
    {
        T compt = GetUI<T>();
        if(!compt.gameObject.activeSelf)
            compt.GetComponent<BUIBaseType>().Enable();
        return compt;
    }

    public static T DisableUI<T>() where T : BUIBaseType
    {
        T compt = GetUI<T>();
        if (compt.gameObject.activeSelf)
            compt.GetComponent<BUIBaseType>().Disable();
        return compt;
    }

    public static void  DestoryUI<T>() where T : BUIBaseType
    {
        Destroy(GetUI<T>().gameObject);
    }

    public static T LoadUI<T>() where T : BUIBaseType
    {
        try
        {
            GameObject obj = Resources.Load(PATH_UI + typeof(T).ToString()) as GameObject;
            return obj.GetComponent<T>();
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.ToString());
            return null;
        }
    }

    public static T GetUI<T>() where T : BUIBaseType
    {
        return Instance.GetComponentInChildren<T>(true);
    }
}
