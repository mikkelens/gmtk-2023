using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class ProceedScript : MonoBehaviour
{
    public float susToAdd;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = new Vector2(Random.Range(-850, 851), Random.Range(-450, 451));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        susToAdd = susToAdd + 0.02f;
    }

    public void Spawn()
    {
        
    }

    public void Pressed()
    {
        PersistentGameManager.Instance.SusMeter += ((int)susToAdd);
        Destroy(gameObject);
    }
}
