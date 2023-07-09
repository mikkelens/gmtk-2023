using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceedScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        gameObject.transform.position = new Vector2(Random.Range(-850,851), Random.Range(-450, 451));
    }
}
