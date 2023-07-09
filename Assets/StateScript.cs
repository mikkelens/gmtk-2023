using UnityEngine;
using System.Collections;
using Core;
using Sirenix.OdinInspector;
using Tools.Types;

public class StateScript : MonoBehaviour
{
    [SerializeField] private Range<int> waitTime = new Range<int>(3, 8);
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject canvas;

    [field: ShowInInspector] public int Waits { get; set; }

    public bool ProceedState { get; set; }
    private bool _spawned;
    private bool _mouseCoolDown;

    // Update is called once per frame
    void Update()
    {
        if (Waits < 4)
        {
            if (ProceedState)
            {
                _spawned = false;
            }
            else
            {
                if (!_spawned) { _spawned = true; StartCoroutine(Waiter()); }
                if (Input.anyKeyDown) { PersistentGameManager.Instance.SusMeter += 2; }
                if (Input.GetAxis("Mouse X") != 0 && _mouseCoolDown == false)
                {
                    PersistentGameManager.Instance.SusMeter += 2;
                    StartCoroutine(CoolDown());

                }
            }
        }
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(Random.Range(waitTime.Min,waitTime.Max + 1));
        ProceedState = true;
        Instantiate(prefab, canvas.gameObject.transform, true);
    }
    private IEnumerator CoolDown()
    {
        _mouseCoolDown = true;
        yield return new WaitForSeconds(1);
        _mouseCoolDown = false;
    }
}