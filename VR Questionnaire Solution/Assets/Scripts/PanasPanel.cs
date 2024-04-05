using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanasPanel : BasePanel
{

    public Text userName;
    public Button b1;
    public Button b2;

    
    public void OnBtnChangeUser()
    {
        UIManager.Instance.OpenPanel(UIConst.UserPanel);
    }
}
