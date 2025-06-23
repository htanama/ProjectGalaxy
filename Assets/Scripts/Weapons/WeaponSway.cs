using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
	[SerializeField] private float smooth;
	[SerializeField] private float swayMultiplier;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw ("Mouse X") * swayMultiplier;
		float mouseY = Input.GetAxisRaw ("Mouse Y") * swayMultiplier;

		// calculate target rotation
		Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right); // Vector3 can be left or right
		Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

		Quaternion targetRotation = rotationX * rotationY;

		// Rotate the weapon holder
		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth*Time.deltaTime);
	
    }

	
}
