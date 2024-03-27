using UnityEngine;

public enum AttachedItemType { markedWall, spike, empty };

public class LeverController : MonoBehaviour
{
	[SerializeField] GameObject _supportTextView;
	[SerializeField] GameObject leverAnimationObject;
	public AttachedItemType type;
	public GameObject attachedItem;
	

	public bool IsRight;


	internal bool nearInteractionObject = false;


	private Animator anim;
	private Animator attachedAnim = null;
	private SpikesController spikesController;
	private AudioSource audioSource;

	// Start is called before the first frame update
	void Start()
    {
		_supportTextView.SetActive(false);
        anim = leverAnimationObject.GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		switch (type)
		{
			case AttachedItemType.markedWall:
				{
					attachedAnim = attachedItem.GetComponent<Animator>();
					attachedAnim.SetBool("IsActivated", true);

					break; 
				}
			case AttachedItemType.spike:
				{
					spikesController = attachedItem.GetComponent<SpikesController>();
					break;
				}
            default:
                break;
        }
	}

    // Update is called once per frame
    void Update()
    {
		if (nearInteractionObject && Input.GetKeyDown(KeyCode.F))
		{
			if(IsRight)
			{
				anim.SetTrigger("TurnLeft");
				switch (type)
				{
					case AttachedItemType.markedWall:
						attachedAnim.SetBool("IsActivated", false);
						audioSource.Play();
						break;
					case AttachedItemType.spike:
						if(spikesController.IsActivated)
							spikesController.Disactivate();
						else
                            spikesController.Activate();
                        break;
                    default:
                        break;
                }
			}
            else
            {
				anim.SetTrigger("TurnRight");
				switch (type)
				{
					case AttachedItemType.markedWall:
						audioSource.Play();
						attachedAnim.SetBool("IsActivated", true); break;
					case AttachedItemType.spike:
                        if (spikesController.IsActivated)
                            spikesController.Disactivate();
                        else
                            spikesController.Activate();                
                        break;
                    default:
                        break;
                }
			}
			IsRight = !IsRight;

		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			nearInteractionObject = true;
			_supportTextView.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.name == "Player")
		{
			nearInteractionObject = false;
			_supportTextView.SetActive(false);
		}
	}


}
