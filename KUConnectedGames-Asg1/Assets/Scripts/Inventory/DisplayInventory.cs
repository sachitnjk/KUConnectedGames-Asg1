using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
	public InventoryObject inventory;

	public int x_Start;
	public int y_Start;
	public int y_SpaceBetweenItems;
	public int x_SpaceBetweenItems;
	public int numberOfColumns;

	Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

	private void Start()
	{
		CreateDisplay();
	}

	private void Update()
	{
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		for (int i = 0; i < inventory.Container.Count; i++)
		{
			if (itemsDisplayed.ContainsKey(inventory.Container[i]))
			{
				itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
			}
			else
			{
				var obj = Instantiate(inventory.Container[i].Item.prefab, Vector3.zero, Quaternion.identity, transform);
				obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
				obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
				itemsDisplayed.Add(inventory.Container[i], obj);
			}
		}
	}

	public void CreateDisplay()
	{
		for (int i = 0; i < inventory.Container.Count; i++)
		{
			var obj = Instantiate(inventory.Container[i].Item.prefab, Vector3.zero, Quaternion.identity, transform);
			obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
			obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
			itemsDisplayed.Add(inventory.Container[i], obj);
		}
	}

	public Vector3 GetPosition(int i)
	{
		return new Vector3(x_Start + (x_SpaceBetweenItems * (i % numberOfColumns)), y_Start + (-y_SpaceBetweenItems * (i / numberOfColumns)), 0f);
	}
}
