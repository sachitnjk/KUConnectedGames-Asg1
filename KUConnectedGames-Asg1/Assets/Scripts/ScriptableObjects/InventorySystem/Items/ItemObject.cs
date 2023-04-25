using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Samples,
	Tapes,
	Notes,
	Key,
	Dogtags
}

[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory System/Items/ItemObject")]
public class ItemObject : ScriptableObject
{

	public GameObject prefab;
	public ItemType type;

	[TextArea(15, 20)]
	public string description;
}
