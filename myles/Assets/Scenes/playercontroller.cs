using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)){
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            transform.position += forward * Time.deltaTime * 2;
        }
	}
}
