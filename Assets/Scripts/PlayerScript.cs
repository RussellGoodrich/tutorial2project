using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
	public Text win;
	public Text lives;

    private int scoreValue = 0;
	private int livesValue = 3;
	private bool facingRight = true;
	
	public AudioClip victory;
	public AudioSource victorySource;
	
	private bool isOnGround;
	public Transform groundcheck;
	public float checkRadius;
	public LayerMask allGround;
	
	Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
		lives.text = "Lives: " + livesValue.ToString();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
		
		if (facingRight == false && hozMovement > 0)
		{
			Flip();
		}
		else if (facingRight == true && hozMovement < 0)
		{
			Flip();
		}
		
		if (isOnGround == false)
        {
          anim.SetInteger("State", 2);
        }
		
		if (isOnGround == true && hozMovement == 0)
		{
			anim.SetInteger("State", 0);
		}
		
		isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector2 Scaler = transform.localScale;
		Scaler.x = Scaler.x * -1;
		transform.localScale = Scaler;
	}
	
	void Update()
    {

		if (Input.GetKeyDown(KeyCode.D))
        {
          anim.SetInteger("State", 1);
        }

		if (Input.GetKeyUp(KeyCode.D))
        {
          anim.SetInteger("State", 0);
        }

		if (Input.GetKeyDown(KeyCode.A))
        {
          anim.SetInteger("State", 1);
        }

		if (Input.GetKeyUp(KeyCode.A))
        {
			anim.SetInteger("State", 0);
        }
		
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
			
			if (scoreValue == 4)
			{
				livesValue = 3;
				lives.text = "Lives: " + livesValue.ToString();
				transform.position = new Vector3(60.0f, 0.0f, 0.0f); 
			}
			
			if (scoreValue >= 8)
			{
				win.text = "You win! Game developed by Russell Goodrich.";
				victorySource.clip = victory;
				victorySource.Play();
			}
        }
		
		if (collision.collider.tag == "Enemy")
		{
			livesValue -= 1;
			lives.text = "Lives: " + livesValue.ToString();
			Destroy(collision.collider.gameObject);
			
			if (livesValue <= 0)
			{
				win.text = "You lose! Game developed by Russell Goodrich.";
				Destroy(gameObject);
			}
		}

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
}