using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconActiveEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private ItemObject item;
	[SerializeField] private TextMeshProUGUI descriptionText;
	[SerializeField] private GameObject descriptionPanel;

	private void Start()
	{
		descriptionText.text = item.description;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		descriptionPanel.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData) 
	{
		descriptionPanel.SetActive(false);
	}
}
