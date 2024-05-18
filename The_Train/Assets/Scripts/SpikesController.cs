using UnityEngine;

public class SpikesController : MonoBehaviour
{
    public bool IsActivated;
	public bool isYellow = false;

    private Animator[] animators;

	private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
		playerController = FindObjectOfType<PlayerController>();
        animators = GetComponentsInChildren<Animator>();
		if (IsActivated)
		{
			Activate();
		}
    }

    public void Activate()
    {
        foreach(var anim in animators)
        {
            anim.SetBool("IsActivated", true);
			tag = isYellow? "YellowSpikes" : "BlueSpikes";
        }
		IsActivated = true;
	}

	public void Disactivate()
	{
		foreach (var anim in animators)
		{
			anim.SetBool("IsActivated", false);
			tag = "Untagged";
		}
		IsActivated = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (IsActivated && collision.CompareTag("Player"))
        {
			foreach (var anim in animators)
			{
                anim.SetTrigger("PlayerEnter");
			}
		}
	}
}
