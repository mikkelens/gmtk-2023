using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeScript : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public string code;

    public void AddNumber(int newNumber)
    {
        if (code.Length > 3)
        {
            code = newNumber.ToString();
        }
        else
        {
            code += newNumber.ToString();
        }
        text.text = code;
    }
}