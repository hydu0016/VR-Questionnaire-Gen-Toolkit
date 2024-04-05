using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    private static UIManager _instance;
    private Transform _uiRoot;
    //界面配置关系
    private Dictionary<string, string> pathDict;
    //预制件缓存字典
    private Dictionary<string, GameObject> prefabDict;
    //已打开界面的缓存
    public  Dictionary<string, BasePanel> panelDict;
    private UIManager()
    {
        InitDicts();

    }

    public Transform UIRoot
    {
        get
        {
            if (_uiRoot==null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>()
        {
            { UIConst.PanasPanel, "Prefabs/UI/Panel/Panel"},
            {UIConst.UserPanel,"Menu/UserPanel" },
            {UIConst.NewUserPanel,"Menu/NewUserPanel" },
        };
    }
    public static UIManager Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }

    }



    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;

        //检查界面是否已经打开
        if (panelDict.TryGetValue(name,out panel))
        {
            Debug.LogError("界面已经打开" + name);
            return null;
        }

        //检查路径是否有配置
        string path = "";
        if (!pathDict.TryGetValue(name,out path))
        {
            Debug.LogError("界面名称错误，或者未配置路径:" + name);
            return null;
        }

        //使用缓存的预制件
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name,out panelPrefab))
        {
            string realPath = path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        return panel;     
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name,out panel))
        {
            Debug.LogError("界面未打开；" + name);
            return false;
        }

        panel.ClosePanel();
        return true;
    }



}

public class UIConst
{

    public const string PanasPanel = "PanasPanel";
    public const string UserPanel = "UserPanel";
    public const string NewUserPanel = "NewUserPanel";

}
