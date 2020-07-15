using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public bool buttonPressed;
    public Color mycolor;
    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        IceCreamManager.Instance.moveCreamer = true;
        IceCreamManager.Instance.iceCreamColor = mycolor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        IceCreamManager.Instance.moveCreamer = false;

    }
    public void SetMyColor(Color col)
    {
        mycolor = col;
        GetComponent<Image>().color = col;
    }
}
