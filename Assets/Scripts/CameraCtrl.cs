using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
	public Transform target;
	public float followSpeed, distanceLimit;

	void Update()
	{
		if (target != null)
		{
			FollowTarget();

			while (Vector3.Distance(transform.position, target.position) > distanceLimit)
			{
				FollowTarget();
			}

			//make sure the camera isn't moving in the z axis
			Vector3 pos = transform.position;
			pos.z = -10f;
			transform.position = pos;
		}
	}

	private void FollowTarget()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
	}
}
