using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class IceCreamManager : MonoBehaviour
{

    public static IceCreamManager Instance;
    [Header("Splines & Spins")]
    public SplineInfo[] splineData;
    public int splineSize;
    public int currentSplineDataIndex;
    public SplineComputer creamHolderSpline;
    public SplineComputer[] splinesForBottom;
    public SplineFollower creamHolderSplineFollower;
    public Transform stallingPos;
    public bool moveCreamer;
    public bool creamerReady;
    public GameObject testCube;
    public GameObject FallingObjectPrefab;
    public float timer;
    public Color iceCreamColor;
    [Header("Color Generation")]

    public Color[] color1Arr;
    public Color[] color2Arr;
    public Color[] color3Arr;
    [Header("Levels")]
    public LevelDataSc[] levels;
    public GameObject cylinderHolderPrefab;
    public GameObject cylinderHolder;
    public int levelIndex;  
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        creamerReady = true;
    }
    private void Start()
    {
        //  ChooseColor();
        //  cylinderHolder = Instantiate(cylinderHolderPrefab);
        StartCoroutine( loadlevel(0));

    }

    // Update is called once per frame
    void Update()
    {
        // InputChecker();
        if (moveCreamer && creamerReady)
        {


            CreamerMethod(splineData[currentSplineDataIndex].nozzleSpline, splineData[currentSplineDataIndex].bottomCircleSpline);
        }
        else
        {
            timer = 0;
            creamHolderSplineFollower.followDuration = 0;

        }
    }

    private void CreamerMethod(SplineComputer Top, SplineComputer Bottom)
    {
        timer += Time.deltaTime;
        if (timer > .25f)
        {
            creamHolderSplineFollower.followDuration = 2.5f;

        }

        creamHolderSplineFollower.spline = Top;
        var SplineBottomFollower = testCube.GetComponent<SplineFollower>();
        SplineBottomFollower.spline = Bottom;
        var currentPercentageInCurrentSpline = creamHolderSplineFollower.result.percent;
        SplineBottomFollower.SetPercent(currentPercentageInCurrentSpline);


        //Debug.Log(splinepositionerOfCreamLine.result.position);
        var obj = Instantiate(FallingObjectPrefab, creamHolderSplineFollower.gameObject.transform.position, Quaternion.Euler(0, 0, 90), cylinderHolder.transform);
        // var obj2 = Instantiate(FallingObjectPrefab, creamHolderSplineFollower.gameObject.transform.position, Quaternion.Euler(0, 0, 90));
        // var obj3 = Instantiate(FallingObjectPrefab, creamHolderSplineFollower.gameObject.transform.position, Quaternion.Euler(0, 0, 90));
        obj.GetComponent<Renderer>().material.color = iceCreamColor;
        //  obj2.GetComponent<Renderer>().material.color =iceCreamColor;
        //   obj3.GetComponent<Renderer>().material.color =iceCreamColor;
        Vector3[] points = new Vector3[2];
        points[0] = creamHolderSplineFollower.gameObject.transform.position;
        points[1] = Bottom.EvaluatePosition(currentPercentageInCurrentSpline);

        StartCoroutine(MoveFallingObjects(obj, points));
        //   StartCoroutine( MoveFallingObjects(obj2, points));
        //   StartCoroutine( MoveFallingObjects(obj3, points));
        if (currentPercentageInCurrentSpline >= .95f && currentSplineDataIndex < splineData.Length && currentSplineDataIndex < splineSize)
        {
            if (currentSplineDataIndex != splineSize - 1)
            {
                currentSplineDataIndex++;
                creamHolderSplineFollower.SetPercent(0);
            }
            else
            {

                creamerReady = false;
                levelIndex++;
                if (levelIndex > levels.Count())
                {
                    levelIndex = 0;
                }
               StartCoroutine (loadlevel(levelIndex));
            }
        }


        //// v.Evaluate(currentPercentageInCurrentSpline);
        //Debug.Log("percentage " + currentPercentageInCurrentSpline);
    }

    IEnumerator MoveFallingObjects(GameObject obj, Vector3[] points)
    {
        //  obj.transform.DOMove(stallingPos.position, .1f);

        obj.transform.DOMove(points[1], .5f);
        //   obj.transform.DOLocalRotate(new Vector3(0,0,90),1f);
        yield return new WaitForSeconds(.5f);
        //   obj.transform.DOLocalRotate(new Vector3(0, 0, 90), .01f);

        obj.transform.DOShakeScale(.1f, .2f);


    }

    private void InputChecker()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("pressed");
            moveCreamer = true;
            // Instantiate(CylinderPrefab,IceCreeamFallPos.position, Quaternion.Euler(0,0,90));
        }
        else
        {
            moveCreamer = false;
        }
    }


    [System.Serializable]
    public class SplineInfo
    {
        public SplineComputer nozzleSpline;
        public SplineComputer bottomCircleSpline;
        public float followDuration;


    }

    #region ColorChoose

    void ChooseColor(LevelDataSc lvl)
    {
      
        color3Arr = new Color[2];


        color3Arr[1] = lvl.colorData1;
            color3Arr[0] = lvl.colorData2;


        UiManager.Instance.Color1.SetMyColor(color3Arr[0]);
        UiManager.Instance.Color2.SetMyColor(color3Arr[1]);


    }


    #endregion
    #region level
    IEnumerator loadlevel(int index)
    {
        yield return new WaitForSeconds(1f);

        Destroy(cylinderHolder);
        cylinderHolder= Instantiate(cylinderHolderPrefab);

        ChooseColor(levels[index]);
        currentSplineDataIndex = 0;

        yield return new WaitForSeconds(1f);
        creamHolderSplineFollower.SetPercent(0);
        creamerReady = true;
        splineSize = levels[index].SplineAmount;
    }
    #endregion
}
