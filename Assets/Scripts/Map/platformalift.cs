using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformalift : MonoBehaviour
{

	[SerializeField] private float speed;
	[SerializeField] private float startingPosition;
	[SerializeField] private float endPosition;

	private bool upMove = true;

	private Vector3 pos;

	void Update()
	{
		transform.position += transform.up * speed * Time.deltaTime;
		if (pos.y > endPosition && upMove)
		{
			upMove = false;
			speed *= -1f;
		}
		if (pos.y < startingPosition && !upMove)
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
