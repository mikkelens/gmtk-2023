using UnityEngine;
using Core;

public class ProceedScript : MonoBehaviour
{
    public float susToAdd;
    StateScript stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localPosition = new Vector2(Random.Range(-350, 351), Random.Range(-200, 201));
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
        stateMachine = GameObject.FindWithTag("StateMachine").GetComponent<StateScript>();
        stateMachine.ProceedState = false;
        stateMachine.Waits += 1;
        Destroy(gameObject);
    }
}