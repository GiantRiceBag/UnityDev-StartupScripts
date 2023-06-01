using UnityEngine;


public class UIManager : BSingleton<UIManager>
{
    const string PATH_UI = "UI/";

    public static T EnableUI<T>() where T : UIBaseType
    {
        T compt = GetUI<T>();
        if(!compt.gameObject.activeSelf)
            compt.GetComponent<UIBaseType>().Enable();
        return compt;
    }

    public static T DisableUI<T>() where T : UIBaseType
    {
        T compt = GetUI<T>();
        if (compt.gameObject.activeSelf)
            compt.GetComponent<UIBaseType>().Disable();
        return compt;
    }

    public static void  DestoryUI<T>() where T : UIBaseType
    {
        Destroy(GetUI<T>().gameObject);
    }

    public static T LoadUI<T>() where T : UIBaseType
    {
        try
        {
            GameObject obj = Resources.Load(PATH_UI + typeof(T).ToString()) as GameObject;
            return obj.GetComponent<T>();
        }
        catch(System.NullReferenceException e)
        {
            Debug.LogError(e.ToString());
            return null;
        }
    }

    public static T GetUI<T>() where T : UIBaseType
    {
        return Instance.GetComponentInChildren<T>(true);
    }
}
