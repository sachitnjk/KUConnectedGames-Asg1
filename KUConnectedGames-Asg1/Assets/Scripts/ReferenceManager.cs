using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager instance;

	[SerializeField] public EnemyWaypointsScript enemyWaypoints;

	[SerializeField] public GateKey gateKeyScript;

	[SerializeField] public TextMeshProUGUI interactionText;
	[SerializeField] public TextMeshProUGUI gateOpeningText;

	[SerializeField] public TextMeshProUGUI gunCurrentAmmoField;
	[SerializeField] public TextMeshProUGUI gunMaxAmmoField;


	[SerializeField] public GameObject PrimaryWeapon;
	[SerializeField] public GameObject SecondaryWeapon;


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
