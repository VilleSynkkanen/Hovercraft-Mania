using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ActionRebindingUI : MonoBehaviour
{
    // TMProUGUI and actionnames lists correspond to each other
    [SerializeField] TextMeshProUGUI[] keyboard;
    [SerializeField] TextMeshProUGUI[] gamepad;
    [SerializeField] InputActionAsset inputs;
    [SerializeField] string[] actionNames;
    [SerializeField] string[] groupNames;
    [SerializeField] GameObject instructionOverlay;
    [SerializeField] TextMeshProUGUI instructions;

    List<TextMeshProUGUI[]> texts = new List<TextMeshProUGUI[]>();

    private void Awake()
    {
        texts.Add(keyboard);
        texts.Add(gamepad);
    }

    public void UpdateUIElements()
    {
        InputActionMap map = inputs.FindActionMap("Driving");
        for (int j = 0; j < groupNames.Length; j++)
        {
            for (int i = 0; i < actionNames.Length; i++)
            {
                if(texts[j].Length > i)
                {
                    InputAction action = map.FindAction(actionNames[i]);
                    int bindingIndex = action.GetBindingIndex(InputBinding.MaskByGroup(groupNames[j]));
                    texts[j][i].text = action.GetBindingDisplayString(bindingIndex);
                }
            }
        }
    }

    public void UpdateSingleElement(InputAction action, int bindingIndex, string actionMap)
    {
        InputActionMap map = inputs.FindActionMap(actionMap);

        for (int i = 0; i < actionNames.Length; i++)
        {
            if (action == map.FindAction(actionNames[i]))
                texts[bindingIndex][i].text = action.GetBindingDisplayString(bindingIndex);
        }
    }

    public void SetInstructionOverlay(bool active, bool keyboard = true)
    {
        instructionOverlay.SetActive(active);
        if(active && keyboard)
        {
            instructions.text = "PRESS A KEY YOU WANT TO BIND TO\nPRESS ESC TO CANCEL REBINDING";
        }
        else if(active && !keyboard)
        {
            instructions.text = "PRESS A BUTTON YOU WANT TO BIND TO\nPRESS START OR ESC TO CANCEL REBINDING";
        }
    }
}
