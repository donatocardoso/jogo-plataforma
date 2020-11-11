using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour 
{
	public float mov = 2F;

	void Start() {        
	}

	void Update()
	{         
		float x = transform.position.x + (Time.deltaTime * mov);
		float y = transform.position.y;
		float z = transform.position.z;

		transform.position = new Vector3(x, y, z);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy(gameObject);
	}
}
