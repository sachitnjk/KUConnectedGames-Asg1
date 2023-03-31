using UnityEngine;
using UnityEngine.VFX;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager instance;

	//[SerializeField] public GameObject hitImpact_Prefab;
	[SerializeField] public EnemyWaypointsScript enemyWaypoints;

	//public VisualEffect hitImpactVisualEffects { get; private set; }

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

		//hitImpactVisualEffects = hitImpact_Prefab.GetComponent<VisualEffect>();
	}
}
