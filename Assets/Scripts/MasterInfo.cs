using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterInfo : MonoBehaviour
{
    public static int coinCount = 0;
    [SerializeField] GameObject coinDisplay;
    public static int gemCount = 0;
    [SerializeField] GameObject gemDisplay;
    public static int distanceRun = 0;
    [SerializeField] GameObject runDisplay;
    public static int internalDistance;


    private void Start()
    {
        coinCount = 0;
        gemCount = 0;
        distanceRun = 0;
    }
    void Update()
    {
        internalDistance = distanceRun;
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "" + coinCount;
        gemDisplay.GetComponent<TMPro.TMP_Text>().text = "" + gemCount;
        runDisplay.GetComponent<TMPro.TMP_Text>().text = "" + distanceRun;

    }
}
