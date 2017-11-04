using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
		if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Play");
        }
	}
}
