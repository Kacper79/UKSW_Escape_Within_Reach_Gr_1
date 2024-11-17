using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int currentMainQuestNumber = 0;
}
