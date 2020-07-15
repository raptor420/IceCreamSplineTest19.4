using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamManager2 : MonoBehaviour
{

    public SplineComputer[] iceCreamSplines;
    public int CurrentSplineIndex;
    public SplineFollower nozzle;
    public GameObject cylinderPrefab;
    public GameObject Plane;
    public Vector3 PlaneOffset;
    bool InputTaking;
    public Transform cylinderHolder;
    public Transform plateHolder;

    public int MaxCylinderAmount;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        nozzle.spline = iceCreamSplines[0];
    }

    // Update is called once per frame
    void Update()
    {
        InputChecker();
        if (InputTaking)
        {
           
            if (CurrentSplineIndex < iceCreamSplines.Length)
            {
                timer += Time.deltaTime;

                nozzle.followSpeed = 2;
             //   if (timer > 0.03f)
                {
                    Instantiate(cylinderPrefab, nozzle.gameObject.transform.position, Quaternion.Euler(0, 0, 90), cylinderHolder);
                    timer = 0;

                }
                var currentPercentageInCurrentSpline = nozzle.result.percent;
                if (currentPercentageInCurrentSpline >= .95f && CurrentSplineIndex < iceCreamSplines.Length-1)
                {
                    CurrentSplineIndex++;
                   var obj = Instantiate(Plane,plateHolder);
                    obj.transform.position=new Vector3(0, CurrentSplineIndex *1,0) + PlaneOffset;
                    nozzle.spline = iceCreamSplines[CurrentSplineIndex];
                    nozzle.SetPercent(0);
                  
                }
            }
        }
        else
        {
            timer = 0;

            nozzle.followSpeed = 0;


        }
    }

    private void InputChecker()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            InputTaking = true;

        }
        else
        {
            InputTaking = false;
        }
    }
}
