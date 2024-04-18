using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BaseQuestionPanel : MonoBehaviour
{

    public aaaa xx = new();
    private void Start()
    {
        xx.a();
    }

}

public class Panell : BaseQuestionPanel
{
    public int a1 = 3;
}

public class aaaa : Panell
{
    public void a()
    {
        print(a1);
    }
    
}
