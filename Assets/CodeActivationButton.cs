using System.Collections;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class CodeActivationButton : MonoBehaviour
{
	[SerializeField, Required] private CodeScript codeScript;

    public void Pressed()
    {
	    if (PersistentGameManager.Instance.BombData.PIN == codeScript.code.ToCharArray())
	    {
		    PersistentGameManager.Instance.NextSegment();
	    }
	    else
	    {
		    PersistentGameManager.Instance.SegmentFailure();
	    }
    }
}