using UnityEngine;
using UnityEngine.UI;

public class WireButtonLeft : MonoBehaviour
{
    public bool pressed;
    public bool placed;
    public Transform placeTarget;
    [SerializeField] GameObject contraire;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject wire;
    [SerializeField] private StretchWireScript wireScript;
    [SerializeField] WireButtonLeft button2, button3, button4;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    Vector2 target;
	    Vector2 source = transform.position;
        if (placed == false)
        {
            target = Input.mousePosition;
        }
        else
        {
            target = placeTarget.position;
        }
        wireScript.StretchBetween(source, target);
        if (pressed == true) { cursor.SetActive(true); wire.SetActive(true); }
        else
        {
            if (placed == false)
            {
                cursor.SetActive(false); wire.SetActive(false);
            }
            
        }

        if (placed == true)
        {
            gameObject.GetComponent<Button>().interactable = false;
            cursor.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
	    Vector2 source = transform.position;
        Vector2 target;
        if (placed == false)
        {
            target = Input.mousePosition;
        }
        else
        {
            target = contraire.transform.position;
        }
	    Gizmos.color = Color.blue;
	    Gizmos.DrawWireSphere(source, 10f);
	    Gizmos.color = Color.white;
	    Gizmos.DrawLine(source, target);
	    Gizmos.color = Color.red;
	    Gizmos.DrawWireSphere(target, 10f);
    }

    public void PressButton() { 
        
        pressed = true; 
        button2.pressed = false; 
        button3.pressed = false; 
        button4.pressed = false; 
    
    
    
    }
}