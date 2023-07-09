using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    // Drag & drop the player in the inspector
    public Transform Target;

    public float RotationSpeed = 1;

    public float CircleRadius = 1;

    public float ElevationOffset = 0;

    private Vector3 positionOffset;

    private float angle;

    private void LateUpdate()
    {
        positionOffset.Set(
            Mathf.Cos(angle) * CircleRadius,
            Mathf.Sin(angle) * CircleRadius, ElevationOffset
            
        );
        transform.position = Target.position + positionOffset;
        angle += Time.deltaTime * RotationSpeed;
    }
}
