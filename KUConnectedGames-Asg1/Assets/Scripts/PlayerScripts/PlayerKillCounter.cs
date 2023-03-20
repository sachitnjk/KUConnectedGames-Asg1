using UnityEngine;

public class PlayerKillCounter : MonoBehaviour
{
	public static PlayerKillCounter Instance;

	[HideInInspector] public int e_CurrentKillCounter;
	[SerializeField] private int e_KillCounterMax = 3;
	[SerializeField] private int e_KillCountIncrement = 1;
	public bool player_AbilityUse = false;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	private void Start()
	{
		KillCounterReset();
	}

	public void KillCounterReset()
	{
		e_CurrentKillCounter = 0;
		player_AbilityUse = false;
	}

	public void KillCounterIncrease()
	{
		if (e_CurrentKillCounter < e_KillCounterMax)
		{
			e_CurrentKillCounter += e_KillCountIncrement;
			Debug.Log(e_CurrentKillCounter);
		}

		if(e_CurrentKillCounter == e_KillCounterMax)
		{
			player_AbilityUse = true;
		}

	}

	private void Update()
	{
		e_CurrentKillCounter = (int)Mathf.Clamp(e_CurrentKillCounter, 0, e_KillCounterMax);
	}

}
