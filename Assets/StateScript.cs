using UnityEngine;
using System.Collections;
using Core;

public class StateScript : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject canvas;
    public bool proceedState = false;
    bool spawned = false;
    public int waits;
    bool mouseCoolDown;
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
                if (Input.anyKeyDown) { PersistentGameManager.Instance.SusMeter += 2; }
                if (Input.GetAxis("Mouse X") != 0 && mouseCoolDown == false)
                {
                    PersistentGameManager.Instance.SusMeter += 2;
                    StartCoroutine(CoolDown());

                }
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
    private IEnumerator CoolDown()
    {
        mouseCoolDown = true;
        yield return new WaitForSeconds(1);
        mouseCoolDown = false;
    }


}