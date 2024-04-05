using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    private static UIManager _instance;
    private Transform _uiRoot;
    //�������ù�ϵ
    private Dictionary<string, string> pathDict;
    //Ԥ�Ƽ������ֵ�
    private Dictionary<string, GameObject> prefabDict;
    //�Ѵ򿪽���Ļ���
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

        //�������Ƿ��Ѿ���
        if (panelDict.TryGetValue(name,out panel))
        {
            Debug.LogError("�����Ѿ���" + name);
            return null;
        }

        //���·���Ƿ�������
        string path = "";
        if (!pathDict.TryGetValue(name,out path))
        {
            Debug.LogError("�������ƴ��󣬻���δ����·��:" + name);
            return null;
        }

        //ʹ�û����Ԥ�Ƽ�
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
            Debug.LogError("����δ�򿪣�" + name);
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
