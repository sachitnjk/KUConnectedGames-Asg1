using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager instance;

	[Header("Script references")]
	[SerializeField] public EnemyWaypointsScript enemyWaypoints;
	[SerializeField] public GateKey gateKeyScript;

	[Header("TMP text field refernces")]
	[SerializeField] public TextMeshProUGUI interactionText;
	[SerializeField] public TextMeshProUGUI gateOpeningText;
	[SerializeField] public TextMeshProUGUI gunCurrentAmmoField;
	[SerializeField] public TextMeshProUGUI gunMaxAmmoField;

	[Header("GameObject References")]
	[SerializeField] public GameObject PrimaryWeapon;
	[SerializeField] public GameObject SecondaryWeapon;
	[SerializeField] public GameObject InventoryPanelUI;
	[SerializeField] public Recorder primaryRecorder;


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
}
