using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeScript : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public string code;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void one ()
    {
        if(code.Length > 3) { code = "1"; }
        else
        {
            code = code + "1";
        }
        text.text = code;
    }
    public void two () 
    {
        if (code.Length > 3) { code = "2"; }
        else
        {
            code = code + "2";
        }
        text.text = code;
    }
    public void three () 
    {
        if (code.Length > 3) { code = "3"; }
        else
        {
            code = code + "3";
        }
        text.text = code;
    }
    public void four () 
    {
        if (code.Length > 3) { code = "4"; }
        else
        {
            code = code + "4";
        }
        text.text = code;
    }
    public void five () 
    {
        if (code.Length > 3) { code = "5"; }
        else
        {
            code = code + "5";
        }
        text.text = code;
    }
    public void Six () 
    {
        if (code.Length > 3) { code = "6"; }
        else
        {
            code = code + "6";
        }
        text.text = code;
    }
    public void seven () 
    {
        if (code.Length > 3) { code = "7"; }
        else
        {
            code = code + "7";
        }
        text.text = code;
    }
    public void eight () 
    {
        if (code.Length > 3) { code = "8"; }
        else
        {
            code = code + "8";
        }
        text.text = code;
    }
    public void nine () 
    {
        if (code.Length > 3) { code = "9"; }
        else
        {
            code = code + "9";
        }
        text.text = code;
    }
}
