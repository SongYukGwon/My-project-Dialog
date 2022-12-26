using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DB : ScriptableObject
{
	public List<TalkType> Talk; // Replace 'EntityType' to an actual type that is serializable.
	public List<SelectType> Select; // Replace 'EntityType' to an actual type that is serializable.
}
