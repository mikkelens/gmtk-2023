using Core;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    public void Restart()
    {
        PersistentGameManager.Instance.GoToMainMenu();
    }
}