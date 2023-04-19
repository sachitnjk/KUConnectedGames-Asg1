using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
	[SerializeField] Transform recoilFollowPosition;
	[SerializeField] float recoilKickBackAmount;
	[SerializeField] float recoilKickBackSpeed, returnSpeed;

	private float currentRecoilPosition, finalRecoilPosition;

	private void Update()
	{
		currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
		finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, recoilKickBackSpeed * Time.deltaTime);
		recoilFollowPosition.localPosition = new Vector3(0, 0, finalRecoilPosition);

	}

	public void TriggerRecoil()
	{
		currentRecoilPosition += recoilKickBackAmount;
	}
}
