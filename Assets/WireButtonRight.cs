using UnityEngine;
using UnityEngine.UI;

public class WireButtonRight : MonoBehaviour
{
    [SerializeField] WireButtonLeft contraire;
    [SerializeField] WireButtonLeft falsebutton1;
    [SerializeField] WireButtonLeft falsebutton2;
    [SerializeField] WireButtonLeft falsebutton3;

    public bool ConnectedCorrectly { get; private set; } = false;

    public void Pressed()
    {
        if (contraire.pressed == true)
        {
            ConnectedCorrectly = true;
            contraire.placeTarget = gameObject.transform;
            contraire.pressed = false;
            contraire.placed = true;
            
            gameObject.GetComponent<Button>().interactable = false;
        }
        if (falsebutton1.pressed == true)
        {
            falsebutton1.placeTarget = gameObject.transform;
            falsebutton1.pressed = false;
            falsebutton1.placed = true;

            gameObject.GetComponent<Button>().interactable = false;
        }
        if (falsebutton2.pressed == true)
        {

            falsebutton2.placeTarget = gameObject.transform;
            falsebutton2.pressed = false;
            falsebutton2.placed = true;
            gameObject.GetComponent<Button>().interactable = false;
        }
        if (falsebutton3.pressed == true)
        {

            falsebutton3.placeTarget = gameObject.transform;
            falsebutton3.pressed = false;
            falsebutton3.placed = true;
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}