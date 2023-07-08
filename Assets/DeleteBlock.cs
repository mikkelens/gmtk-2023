using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    string backup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueChanged()
    {
        if (string.IsNullOrEmpty(backup))
        {
            backup = gameObject.GetComponent<TMP_InputField>().text;
        }
        if (gameObject.GetComponent<TMP_InputField>().text.Length < backup.Length)
        {
            gameObject.GetComponent<TMP_InputField>().text = backup;
        }
        backup = gameObject.GetComponent<TMP_InputField>().text;
    }
}
