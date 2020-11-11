using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InimigoController : MonoBehaviour
{
	public GameObject sons;
	
	private Rigidbody2D rig;

	private float mov = 1F;
	
	void Start()
	{
		rig = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (mov > 0)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		} 
		else if (mov < 0)
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}

		rig.velocity = new Vector2(mov, rig.velocity.y);
	}
  

	  void OnCollisionEnter2D(Collision2D col)
	  {
		if (col.gameObject.tag == "Fire")
		{
			sons.GetComponents<AudioSource>()[2].Play();
			Destroy(gameObject);
		}
		else
		{
			mov = mov * -1;
		}
	}
}
