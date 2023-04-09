using UnityEngine;
using UnityEngine.VFX;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager instance;

	[SerializeField] public EnemyWaypointsScript enemyWaypoints;
	[SerializeField] public GateKey gateKeyScript;


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
