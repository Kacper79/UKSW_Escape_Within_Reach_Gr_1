using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    /// <summary>
    /// Interface used to implement load/save functionality in the game
    /// Each class willing to implement load/save functionality needs to implement this interface
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// This is function that mutates save data (passed by reference in order to modify argument) in order to save game state.
        /// All the serializable data in the class implementing this interface should be passed and saved here.
        /// </summary>
        /// <param name="saveData">SaveData struct containing all of the save data that can be mutated </param>
        public void Save(ref SaveData saveData);

        /// <summary>
        /// This is function that loads the serializable data of the class implementing this interface.
        /// This function is called to mutate serializable data in the class implementing this interface when the save is being loaded.
        /// </summary>
        /// <param name="saveData">SaveData struct containing all of the save data that can be loaded from</param>
        public void Load(SaveData saveData);
    }
}