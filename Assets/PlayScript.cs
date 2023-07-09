using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class PlayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        PersistentGameManager.Instance.ReturnToHub();
    }
}
