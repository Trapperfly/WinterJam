using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
	public Transform player;
	public Combining combine;
	public int targetFrameRate = 60;

	private void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFrameRate;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
