using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    public void Restart()
    {
        PersistentGameManager.Instance.GoToMainMenu();
    }
}