using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public MyButton Color1;
    public MyButton Color2;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Color1.buttonPressed || Color2.buttonPressed)
        {


        }
    }
   
}
