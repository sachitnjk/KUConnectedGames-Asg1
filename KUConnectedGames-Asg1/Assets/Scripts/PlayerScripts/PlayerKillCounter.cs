using UnityEngine;

public class PlayerKillCounter : MonoBehaviour
{
	public static PlayerKillCounter Instance;

	public int e_CurrentKillCounter;
	[SerializeField] private int e_KillCounterMax = 3;
	[SerializeField] private int e_KillCountIncrement = 1;
	bool player_AbilityUse = false;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log("Singleton coming up againm");
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
	}

	public void KillCounterIncrease()
	{
		e_CurrentKillCounter += e_KillCountIncrement;
		Debug.Log(e_CurrentKillCounter);

		if(e_CurrentKillCounter >= e_KillCounterMax)
		{
			KillCounterFull();
			KillCounterReset();
		}
	}

	public void KillCounterFull()
	{
		player_AbilityUse = true;
		Debug.Log(player_AbilityUse);
	}

	private void Update()
	{
		e_CurrentKillCounter = (int)Mathf.Clamp(e_CurrentKillCounter, 0, e_KillCounterMax);
	}

}
