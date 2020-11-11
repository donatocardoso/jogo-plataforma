using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JogadorController : MonoBehaviour
{
	public Transform fundo;
	public Transform camera;

	public GameObject sons;
	public GameObject fireball;

	public Text txtVidas;
	public Text txtPontuacao;

	public float speed;
	public float jumpForce;

	public float minimoCameraX;
	public float maximoCameraX;
	public float minimoCameraY;
	public float maximoCameraY;
		
	private Rigidbody2D rig;
	private Animator animator;
	private bool pulando = false;
	private bool abaixando = false;
	private int pontuacao = 0;
	private int vidas = 5;

	// Start is called before the first frame update
	void Start()
	{
		txtVidas.text = vidas + " vidas";
		txtPontuacao.text = pontuacao + " pts";

		rig = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		float camx = rig.transform.position.x + 3;

		if(camx < minimoCameraX)
		{
			camx = minimoCameraX;
		}

		if(camx > maximoCameraX)
		{
			camx = maximoCameraX;
		}

		float camy = rig.transform.position.y + 3;

		if(camy < minimoCameraY)
		{
			camy = minimoCameraY;
		}

		if(camy > maximoCameraY)
		{
			camy = maximoCameraY;
		}

		//posiciona a camera
		camera.position = new Vector3(camx, camy , -10);

		//efeito paralax
		float fundox = ((camx - minimoCameraX) / 1.5F) + 22;
		fundo.position  = new Vector3(fundox, 0 , 2F);

		//pega o valor da seta do teclado(1=direita -1=esquerda)
		float mov = Input.GetAxisRaw("Horizontal");

		//faz o flip do sprite do jogador de acordo com sua direcao
		if(mov == 1)
		{
			GetComponent<SpriteRenderer>().flipX = false;
		} 
		else if(mov == -1)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}

		//move o jogador para a direita ou esquerda se nao estiver pulando
		rig.velocity = new Vector2(mov * speed, rig.velocity.y);
		animator.SetFloat("Velocidade", Mathf.Abs(mov));

		//pula se nao estiver pulando
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && pulando == false)
		{
			rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

			pulando = true;
			
			animator.SetBool("Pulando", true);
			sons.GetComponents<AudioSource>()[3].Play();
		}

		if(Input.GetKeyDown(KeyCode.DownArrow) &&(abaixando == false))
		{
			abaixando = true;
			animator.SetBool("Abaixando",true);
		}

		if(Input.GetKeyUp(KeyCode.UpArrow))
		{
			abaixando = false;
			animator.SetBool("Abaixando", false);
		}

		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			sons.GetComponents<AudioSource>()[4].Play();

			float fx;
			float movFire;
			bool flipFire;
			
			if(GetComponent<SpriteRenderer>().flipX)
			{
				movFire = -3F;
				fx = rig.transform.position.x - 2;
				flipFire = false;
			}
			else
			{
				movFire = 3F;
				fx = rig.transform.position.x + 2;
				flipFire = true;
			}

			float fy = rig.transform.position.y + 0.5F;
			float fz = rig.transform.position.z;

			GameObject novo = Instantiate(fireball, new Vector3(fx, fy, fz), Quaternion.identity);
			
			novo.GetComponent<FireballController>().mov = movFire;
			novo.GetComponent<SpriteRenderer>().flipX = flipFire;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		pulando = false;		
		animator.SetBool("Pulando",false);

		if(collision.gameObject.tag == "Enemy")
		{
			if(collision.gameObject.transform.position.y+1 < gameObject.transform.position.y)
			{
				sons.GetComponents<AudioSource>()[2].Play();
				Destroy(gameObject);
			} 
			else
			{
				vidas--;

				if(vidas > 0)
				{
					txtVidas.text = vidas + " vidas";

					sons.GetComponents<AudioSource>()[1].Play();
					animator.SetBool("Pulando",false);
				}
				else
				{
					SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
				}
			}
		} 
		else if(collision.gameObject.CompareTag("Coins"))
		{
			Destroy(collision.gameObject);

			pontuacao++;
			txtPontuacao.text = pontuacao + " pts";
		}
		else if(collision.gameObject.CompareTag("Flag"))
		{
			SceneManager.LoadScene("Win", LoadSceneMode.Single);
		}
	}
}
