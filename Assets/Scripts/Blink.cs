using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float visibleTime = 1f;
    public float invisibleTime = 0.2f;

    private bool isVisible = true;
    private float timeCount = 0;
	
	// Update is called once per frame
	void Update ()
    {
        float dt = Time.deltaTime;

        timeCount += dt;

        if (isVisible && timeCount >= visibleTime)
        {
            isVisible = false;
            timeCount = 0;
            this.transform.localScale = Vector3.zero;
        }
        else if (!isVisible && timeCount >= invisibleTime)
        {
            isVisible = true;
            timeCount = 0;
            this.transform.localScale = Vector3.one;
        }
	}
}
