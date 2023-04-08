using UnityEngine;

public class PlayerKillCounter : MonoBehaviour
{
	public static PlayerKillCounter Instance;

	KillCounterScript killCounterSlider;

	[HideInInspector] public int e_CurrentKillCounter;
	[SerializeField] public int e_KillCounterMax = 3;
	private int e_KillCountIncrement = 1;
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
		killCounterSlider?.SetSliderCounter(e_CurrentKillCounter);
	}

	public void KillCounterIncrease()
	{
		if (e_CurrentKillCounter < e_KillCounterMax)
		{
			e_CurrentKillCounter += e_KillCountIncrement;
			killCounterSlider?.SetSliderCounter(e_CurrentKillCounter);
			Debug.Log(e_CurrentKillCounter);
		}

		if(e_CurrentKillCounter == e_KillCounterMax)
		{
			player_AbilityUse = true;
		}

	}

	public int GetKills()
	{
		return e_CurrentKillCounter;
	}

	public void SetKillCounterSlider(KillCounterScript slider)
	{
		killCounterSlider = slider;
		killCounterSlider.SetMaxSliderCounter(e_KillCounterMax);
		killCounterSlider.SetSliderCounter(e_CurrentKillCounter);
	}

	private void Update()
	{
		e_CurrentKillCounter = (int)Mathf.Clamp(e_CurrentKillCounter, 0, e_KillCounterMax);
	}

}
