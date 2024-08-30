using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FileData", menuName = "Scriptable/FileData")]
public class FileData : ScriptableObject
{
    public List<CharacterData> Characters;
}
