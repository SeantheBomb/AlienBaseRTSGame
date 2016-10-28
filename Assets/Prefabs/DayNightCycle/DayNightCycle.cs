using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    private Light thisLight;

    public static int day=0;

    public float transitionSpeed = 1f;
    public static TimeOfDay timeOfDay;

    private bool isIncreasing=true;
    private bool firstIt = true;
    private float slopeCheck;

	// Use this for initialization
	void Start () {
        thisLight = GetComponent<Light>();
        checkTime(transform.rotation.x);
	}
	
	// Update is called once per frame
	void Update () {

        checkTime(transform.rotation.x);

        transform.Rotate(Vector3.right, transitionSpeed * Time.deltaTime);

        //Check slope
        if (firstIt)
            slopeCheck = transform.rotation.x;
        else
            isIncreasing = slopeCheck < transform.rotation.x;
        firstIt = !firstIt;
	}


    void checkTime(float rot)
    {
        TimeOfDay updateCheck=TimeOfDay.Null;
        if (isIncreasing && rot >= 0)
        {
            if (rot >= 0f)
                updateCheck = TimeOfDay.Morning;
            if (rot >= 0.2f)
                updateCheck = TimeOfDay.Noon;
            if (rot >= 0.5f)
                updateCheck = TimeOfDay.Evening;
        }
        else if (!isIncreasing && rot <= 0)
        {
            if (rot <= 0f)
                updateCheck = TimeOfDay.Morning;
            if (rot <= -0.2f)
                updateCheck = TimeOfDay.Noon;
            if (rot <= -0.5f)
                updateCheck = TimeOfDay.Evening;
        }
        else updateCheck = TimeOfDay.Night;

        if (updateCheck == timeOfDay)
            return;
        timeOfDay = updateCheck;
        if (timeOfDay == TimeOfDay.Morning)
            day++;

    }



}
