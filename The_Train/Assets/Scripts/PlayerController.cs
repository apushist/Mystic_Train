using Cinemachine;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public float stepSoundRate = 0.5F;
    public AudioSource stepSoundSource;
    public GameObject playerTexture;
    public bool canMove = true; //чтобы запретить игроку двигаться, когда он в инвентаре
	public AudioClip[] stepSounds;
    public CinemachineVirtualCamera virtualCamera;
    public int loreItemsFound;


    private int currentSoundNumber;
	private Animator animator;
	private float nextStep = 0.0F;
	public static event Action Epressed;
	public static event Action Fpressed;
    [HideInInspector] public bool isLightOn = false;
    private bool dead;

    private void Start()
	{
        dead = false;
		animator = playerTexture.GetComponent<Animator>();
		SetAnimationOnLight(isLightOn);
		if (stepSoundSource.clip == null && stepSounds.Length > 0)
        {
            SetSound(0);
		}
	}

	// Update is called once per frame
	void Update()
    {
        // Processing Inputs
        ProcessInputs();

	}

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Physics Calculations
        Move();
	}

    void ProcessInputs()
    {
        if (canMove)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            PlayAnimation(moveX, moveY);
            moveDirection = new Vector2(moveX, moveY).normalized;

            if (Time.time > nextStep && (moveX != 0 || moveY != 0))
            {
                nextStep = Time.time + stepSoundRate;
                stepSoundSource.pitch = UnityEngine.Random.Range(0.4f, 1.0f);
                stepSoundSource.PlayOneShot(stepSoundSource.clip,0.5f);
            }
        }
        else
        {
			moveDirection = new Vector2(0, 0);
			if (!dead)
				StopAnimation();
		}
		if (Input.GetKeyDown(KeyCode.E))
        {
            Epressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fpressed?.Invoke();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void PlayAnimation(float moveX,float moveY)
    {
        if(moveX == 0 && moveY == 0)
        {
			if (!dead)
				StopAnimation();
		}
        else
        {
			if (moveY > 0 && moveX == 0)
			{
				animator.SetBool("IsBack", true);
				animator.SetBool("IsStraight", false);
				animator.SetBool("IsLeft", false);
				animator.SetBool("IsRight", false);
			}
			else
			{
				if (moveY < 0 && moveX == 0)
				{
					animator.SetBool("IsBack", false);
					animator.SetBool("IsStraight", true);
					animator.SetBool("IsLeft", false);
					animator.SetBool("IsRight", false);
				}
                else
                {
                    if(moveX > 0)
                    {
						animator.SetBool("IsBack", false);
						animator.SetBool("IsStraight", false);
						animator.SetBool("IsLeft", false);
						animator.SetBool("IsRight", true);
					}
                    else
                    {
						animator.SetBool("IsBack", false);
						animator.SetBool("IsStraight", false);
						animator.SetBool("IsLeft", true);
						animator.SetBool("IsRight", false);
					}
                }
			}
		}
		
	}

    void StopAnimation()
    {
		animator.SetBool("IsBack", false);
		animator.SetBool("IsStraight", false);
		animator.SetBool("IsLeft", false);
		animator.SetBool("IsRight", false);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Debug.Log("death");
            Death.instance.OnDeathTrigger();
        }
		else if (collision.CompareTag("BlueSpikes"))
		{
            dead = true;
			Debug.Log("deathBlue");
			Death.instance.OnDeathTrigger(DeathType.BlueSpikes);
		}
		else if (collision.CompareTag("YellowSpikes"))
		{
            dead = true;
			Debug.Log("deathYellow");
			Death.instance.OnDeathTrigger(DeathType.YellowSpikes);
		}
	}

    public void SetSound(int soundNumber)
    {
        if(soundNumber >= 0 && soundNumber < stepSounds.Length)
        {
			stepSoundSource.clip = stepSounds[soundNumber];
            currentSoundNumber = soundNumber;
		}
    }

    public int GetSoundNumber() { return currentSoundNumber;}

    public void SetAnimationOnLight(bool on = true)
	{
		isLightOn = on;
		StopAnimation();
        animator.SetBool("IsLightOn", on);
    }

    public void DeathBlue()
    {
        animator.SetTrigger("DeathBlue");
    }

	public void DeathYellow()
	{
		animator.SetTrigger("DeathYellow");
	}

    public void DeathMonster()
    {
        StopAnimation();
		animator.SetTrigger("DeathMonster");
	}
}
