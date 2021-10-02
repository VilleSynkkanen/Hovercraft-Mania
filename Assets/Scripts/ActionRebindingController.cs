using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionRebindingController : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;
    [SerializeField] ActionRebindingUI ui;

    void Start()
    {
        LoadKeybinds();
        ui.UpdateUIElements();
    }

    [Serializable]
    class BindingWrapperClass
    {
        public List<BindingSerializable> bindingList = new List<BindingSerializable>();
    }

    [Serializable]
    private struct BindingSerializable
    {
        public string id;
        public string path;

        public BindingSerializable(string bindingId, string bindingPath)
        {
            id = bindingId;
            path = bindingPath;
        }
    }

    public void Rebind(string actionAndGroup)
    {
        string[] arr = actionAndGroup.Split(':');
        string mapName = "Driving";
        InputActionMap map = actions.FindActionMap(mapName);
        InputAction action = map.FindAction(arr[0]);
        RebindActionStart(action, arr[1]);
    }

    public void RebindActionStart(InputAction inputAction, string group)
    {
        InputActionRebindingExtensions.RebindingOperation rebindOperation = null;
        
        void CleanUp()
        {
            rebindOperation?.Dispose();
            rebindOperation = null;
        }

        int bindingIndex = inputAction.GetBindingIndex(InputBinding.MaskByGroup(group));
        string controlType;
        string cancelType;
        bool keyboard;
        if (bindingIndex == 3)
        {
            controlType = "<Gamepad>";
            cancelType = "<Gamepad>/start";
            keyboard = false;
        }
        else
        {
            controlType = "<keyboard>";
            cancelType = "<Keyboard>/escape";
            keyboard = true;
        }

        rebindOperation = inputAction.PerformInteractiveRebinding(bindingIndex).WithCancelingThrough(cancelType)
            .WithCancelingThrough("<Keyboard>/escape").WithControlsHavingToMatchPath(controlType).OnCancel(operation =>
            {
                CleanUp();
                ui.SetInstructionOverlay(false);
            })
            .OnComplete(operation =>
            {
                CleanUp();
                ui.UpdateSingleElement(inputAction, bindingIndex, inputAction.actionMap.name);
                ui.SetInstructionOverlay(false);
            });

        ui.SetInstructionOverlay(true, keyboard);
        rebindOperation.Start();
    }

    public void SaveKeybinds()
    {
        BindingWrapperClass bindingList = new BindingWrapperClass();
        foreach (var map in actions.actionMaps)
        {
            foreach (var binding in map.bindings)
            {
                if (!string.IsNullOrEmpty(binding.overridePath))
                {
                    bindingList.bindingList.Add(new BindingSerializable(binding.id.ToString(), binding.overridePath));
                }
            }
        }

        PlayerPrefs.SetString("ControlOverrides", JsonUtility.ToJson(bindingList));
        PlayerPrefs.Save();
    }

    public void LoadKeybinds()
    {
        if (PlayerPrefs.HasKey("ControlOverrides"))
        {
            BindingWrapperClass bindingList = JsonUtility.FromJson(PlayerPrefs.GetString("ControlOverrides"), 
                typeof(BindingWrapperClass)) as BindingWrapperClass;

            Dictionary<Guid, string> overrides = new Dictionary<Guid, string>();
            foreach (var item in bindingList.bindingList)
            {
                overrides.Add(new Guid(item.id), item.path);
            }

            foreach (var map in actions.actionMaps)
            {
                var bindings = map.bindings;
                for (var i = 0; i < bindings.Count; ++i)
                {
                    if (overrides.TryGetValue(bindings[i].id, out string overridePath))
                    {
                        map.ApplyBindingOverride(i, new InputBinding { overridePath = overridePath });
                    }
                }
            }
        }
    }

    public void ResetToDefault()
    {
        foreach (var map in actions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        PlayerPrefs.DeleteKey("ControlOverrides");
        ui.UpdateUIElements();
    }
}
