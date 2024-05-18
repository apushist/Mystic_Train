using System.Collections;
using UnityEngine;

public class CursorHider : MonoBehaviour
{
	public float updateTime = 1f;
	public bool InMenu = false;

	private Coroutine co_HideCursor;

	private void Start()
	{
		Cursor.visible = true;
	}

	private void FixedUpdate()
	{
		if (!InMenu)
		{
			if (Input.GetAxis("Mouse X") == 0 && (Input.GetAxis("Mouse Y") == 0))
			{
				if (co_HideCursor == null)
				{
					co_HideCursor = StartCoroutine(HideCursor());
				}
			}
			else
			{
				if (co_HideCursor != null)
				{
					StopCoroutine(co_HideCursor);
					co_HideCursor = null;
					Cursor.visible = true;
				}
			}
		}
	}

	private IEnumerator HideCursor()
	{
		yield return new WaitForSeconds(updateTime);
		Cursor.visible = false;
	}
}
