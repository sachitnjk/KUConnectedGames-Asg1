using UnityEngine;
using UnityEngine.VFX;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager instance;

	[SerializeField] public VisualEffect hitImpactVisualEffects;
	[SerializeField] public EnemyWaypointsScript enemyWaypoints;

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
