using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class StateScript : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject canvas;
    public bool proceedState = false;
    bool spawned = false;
    public int waits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waits < 4)
        {
            if (proceedState)
            {
                spawned = false;
            }
            else
            {
                if (!spawned) { spawned = true; StartCoroutine(Waiter()); }
                if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0) { PersistentGameManager.Instance.SusMeter += 2; }
            }
        }
        else
        {


        }
    }

    private IEnumerator Waiter()
    {
        
        yield return new WaitForSeconds(Random.Range(25,36));
        proceedState = true;
        var button = Instantiate(prefab);
        button.transform.parent = canvas.gameObject.transform;
    }

   
}
