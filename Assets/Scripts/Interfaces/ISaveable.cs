using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISaveable
    {
        public void Save(ref SaveData saveData);
        public void Load(SaveData saveData);

        public void Register()
        {
            SaveManager.Instance.saveablesGO.Add(this);
        }
    }
}