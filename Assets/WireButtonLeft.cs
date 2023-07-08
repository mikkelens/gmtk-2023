using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireButtonLeft : MonoBehaviour
{
    public bool pressed;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject wire;
    [SerializeField] private StretchWireScript wireScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    Vector2 target = Input.mousePosition;
	    Vector2 source = transform.position;
	    wireScript.StretchBetween(source, target);
    }

    private void OnDrawGizmos()
    {
	    Vector2 source = transform.position;
	    Vector2 target = Input.mousePosition;
	    Gizmos.color = Color.blue;
	    Gizmos.DrawWireSphere(source, 10f);
	    Gizmos.color = Color.white;
	    Gizmos.DrawLine(source, target);
	    Gizmos.color = Color.red;
	    Gizmos.DrawWireSphere(target, 10f);
    }
}