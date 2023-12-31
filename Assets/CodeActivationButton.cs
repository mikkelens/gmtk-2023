using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class CodeActivationButton : MonoBehaviour
{
	[SerializeField, Required] private CodeScript codeScript;

    public void Pressed()
    {
	    if (PersistentGameManager.Instance.BombData.PIN.SequenceEqual(codeScript.code.ToCharArray()))
	    {
		    PersistentGameManager.Instance.GoToEnding();
	    }
	    else
	    {
		    PersistentGameManager.Instance.SegmentFailure();
	    }
    }
}