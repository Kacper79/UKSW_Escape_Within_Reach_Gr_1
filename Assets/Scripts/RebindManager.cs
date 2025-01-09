using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Scripts
{
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

        public void ListenAndRebindControl(string actionName)
        {
            InputAction changedAction = player_input.FindAction(actionName);
            if (changedAction != null)
            {
                Debug.LogError("There is no such input action");
                return;
            }
            InputSystem.onAnyButtonPress.CallOnce(ctx => {
                string newBinding = ctx.path;
                Debug.Log($"Changing input action {actionName} to {newBinding} using listening");
                changedAction.ApplyBindingOverride(0, newBinding);
            });
        }

        public void RebindButton(string actionName, string newBinding)
        {
            InputAction changedAction = player_input.FindAction(actionName);
            if (changedAction != null)
            {
                Debug.LogError("There is no such input action");
                return;
            }
            changedAction.ApplyBindingOverride(0, newBinding);
            Debug.Log($"Input action '{actionName}' was rebound to {newBinding}");
        }

        public void SaveBindingsOverride()
        {
            string bindingsJson = player_input.SaveBindingOverridesAsJson();
            File.WriteAllText(saveFilePath, bindingsJson);
        }

        public void ResetBindingsOverride()
        {
            player_input.RemoveAllBindingOverrides();
        }

        private string saveFilePath;
        public PlayerInput player_input;
        public static RebindManager Instance { get; private set; }
    }
}