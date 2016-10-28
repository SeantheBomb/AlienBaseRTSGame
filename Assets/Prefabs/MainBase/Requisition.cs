using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Requisition : MonoBehaviour {

    public bool isTotal;
    public Text label;
    public Slider slider;
    public Text value;

    public int step = 1;

    int totalVal;

    // Use this for initialization
    void Start () {
        if (step < 1)
            step = 1;
        label = transform.FindChild("Label").GetComponent<Text>();
        slider = transform.FindChild("Slider").GetComponent<Slider>();
        value = transform.FindChild("Value").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!isTotal)
            setValue((int)slider.value);
        value.text = getValue().ToString();
	}

    public int getValue()
    {
        if (!isTotal)
            return (int)slider.value / step;
        else
            return totalVal / step;
    }

    public int getValueNoStep()
    {
        if (!isTotal)
            return (int)slider.value;
        else
            return totalVal;
    }

    public void setValue(int val)
    {
        if (isTotal)
            totalVal = val;
        slider.value = stepValue(val);
    }

    public int getMaxValue()
    {
        return (int)slider.maxValue;
    }

    public void setMaxValue(int val)
    {
        slider.maxValue = val;
    }

    public int stepValue(int val)
    {
        if (step <= 1)
            return val;
        int stepTo=0;
        while (stepTo <= val)
            stepTo += step;
        if (stepTo == val)
            return val;
        int stepFrom = stepTo - step;
        if (stepFrom == val)
            return val;
        if (stepTo - val < val - stepFrom)
            return stepTo;
        else
            return stepFrom;
    }
}
