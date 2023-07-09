using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class Progress2Script : MonoBehaviour
{
    
    [SerializeField] WireButtonLeft button1;
    [SerializeField] WireButtonLeft button2;
    [SerializeField] WireButtonLeft button3;
    [SerializeField] WireButtonLeft button4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (button1.placed == true && button2.placed == true && button3.placed == true && button4.placed == true && gameObject.GetComponent<Button>().interactable == false)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void Pressed()
    {
        PersistentGameManager.Instance.ReturnToHub();
    }


}
