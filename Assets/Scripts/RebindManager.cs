using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Scripts
{
    /// <summary>
    /// This class is being used to rebind player's input
    /// </summary>
    public class RebindManager : MonoBehaviour
    {
        void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Ensure the manager persists across scenes
                player_input = new();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {
            // Set file path for saving/loading bindings
            saveFilePath = Path.Combine(Application.persistentDataPath, "inputBindings.json");

            // Load saved bindings if available
            if (File.Exists(saveFilePath))
            {
                string savedBindings = File.ReadAllText(saveFilePath);
                player_input.LoadBindingOverridesFromJson(savedBindings);
            }
        }

        /// <summary>
        /// Rebinds given action to the new key binding supplied by listening to most recently pressed key
        /// </summary>
        /// <param name="actionName">the path containing the action map and the action name that is being rebinded </param>
        public void ListenAndRebindControl(string actionName, Action onRebindComplete)
        {
            InputAction changedAction = player_input.FindAction(actionName);
            if (changedAction == null)
            {
                Debug.LogError("There is no such input action");
                return;
            }
            InputSystem.onAnyButtonPress.CallOnce(ctx => {
                string newBinding = ctx.path;
                Debug.Log($"Changing input action {actionName} to {newBinding} using listening");
                changedAction.ApplyBindingOverride(0, newBinding);

                onRebindComplete?.Invoke();
            });
        }

        /// <summary>
        /// Simply rebinds a action to a new freshly-supplied key
        /// </summary>
        /// <param name="actionName">the path containing the action map and the action name that is being rebinded</param>
        /// <param name="newBinding">the path containing the device type and device key name</param>
        public void RebindButton(string actionName, string newBinding)
        {
            InputAction changedAction = player_input.FindAction(actionName);
            if (changedAction == null)
            {
                Debug.LogError("There is no such input action");
                return;
            }
            changedAction.ApplyBindingOverride(0, newBinding);
            Debug.Log($"Input action '{actionName}' was rebound to {newBinding}");
        }

        /// <summary>
        /// Saves the file containing all of the rebindings
        /// </summary>
        public void SaveBindingsOverride()
        {
            string bindingsJson = player_input.SaveBindingOverridesAsJson();
            File.WriteAllText(saveFilePath, bindingsJson);
        }

        /// <summary>
        /// Flushes the rebinding cache i.e cancells all of the rebindings
        /// </summary>
        public void ResetBindingsOverride()
        {
            player_input.RemoveAllBindingOverrides();
        }

        /// <summary>
        /// string containing a system path to a place where all rebinded key's data will be saved
        /// </summary>
        private string saveFilePath;
        /// <summary>
        /// PlayerInput class containing the mapping of all registered key presses to responding event callbacks
        /// </summary>
        public PlayerInput player_input;
        /// <summary>
        /// Singleton's public access point
        /// </summary>
        public static RebindManager Instance { get; private set; }
    }
}