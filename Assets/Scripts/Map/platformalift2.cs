using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformalift2 : MonoBehaviour
{

	[SerializeField] private float speed;
	[SerializeField] private float startingPosition;
	[SerializeField] private float endPosition;

	private bool upMove = true;

	private Vector3 pos;

	void Update()
	{
		transform.position += transform.right * speed * Time.deltaTime;
		if (pos.x > endPosition && upMove)
		{
			upMove = false;
			speed *= -1f;
		}
		if (pos.x < startingPosition && !upMove)
		{
			upMove = true;
			speed *= -1f;
		}
		pos = transform.position;
	}

	void OnCollisionEnter(Collision col)
	{
		col.gameObject.transform.parent = transform;
	}

	void OnCollisionExit(Collision col)
	{
		col.gameObject.transform.parent = null;
	}

}
