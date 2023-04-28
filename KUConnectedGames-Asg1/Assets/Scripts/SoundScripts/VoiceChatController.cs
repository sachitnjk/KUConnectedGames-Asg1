using Photon.Voice.Unity;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChatController : MonoBehaviour
{
	private Recorder primaryRecorder;

	StarterAssetsInputs _input;

	private void Start()
	{
		_input = GetComponent<StarterAssetsInputs>();
		primaryRecorder = ReferenceManager.instance.primaryRecorder;

		primaryRecorder.TransmitEnabled = false;
	}

	private void Update()
	{
		PushToTalk();
	}

	private void PushToTalk()
	{
		if(_input.PushToTalk.WasPerformedThisFrame())
		{
			primaryRecorder.TransmitEnabled = true;
		}
		else if (_input.PushToTalk.WasReleasedThisFrame())
		{
			primaryRecorder.TransmitEnabled = false;
		}
	}
}
