using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMoraleGauge : MonoBehaviour {

    public Slider gauge = null;

	// Use this for initialization
	void Start () {
        gauge = gameObject.GetComponent<Slider>();
        gauge.value = 1f;

    }

    int tracker = 0;
	// Update is called once per frame
	void Update () {
        //gauge.value;
        tracker++;
        if (tracker <= 10)
        {
            ReduceGauge();
        }
        else if (tracker <= 20)
        {
            IncreaseGauge();
        }
        else if (tracker <= 30)
        {
            tracker = 0;
        }
	}

    public void ReduceGauge()
    {
        gauge.value -= 0.1f;
        gauge.value = Mathf.Clamp01(gauge.value);

        if (gauge.value <= 0)
        {
            // failed the level, all cats fainted
        }
    }

    public void IncreaseGauge()
    {
        gauge.value += 0.1f;
        
        if (gauge.value > 1)
        {
            // HIGH TIMES, GET EM GOING (activate the crazyness)
        }

        gauge.value = Mathf.Clamp01(gauge.value);
    }
}
