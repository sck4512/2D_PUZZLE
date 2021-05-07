// GENERATED AUTOMATICALLY FROM 'Assets/MyInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MyInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MyInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MyInput"",
    ""maps"": [
        {
            ""name"": ""MyController"",
            ""id"": ""b45561f3-2f17-4d79-80b4-9536d69060f1"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""1a267f78-8882-468a-b69e-d4f40c04097b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1a8898f0-f6e5-4611-a43e-6c3dc965c8bf"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // MyController
        m_MyController = asset.FindActionMap("MyController", throwIfNotFound: true);
        m_MyController_Click = m_MyController.FindAction("Click", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // MyController
    private readonly InputActionMap m_MyController;
    private IMyControllerActions m_MyControllerActionsCallbackInterface;
    private readonly InputAction m_MyController_Click;
    public struct MyControllerActions
    {
        private @MyInput m_Wrapper;
        public MyControllerActions(@MyInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_MyController_Click;
        public InputActionMap Get() { return m_Wrapper.m_MyController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MyControllerActions set) { return set.Get(); }
        public void SetCallbacks(IMyControllerActions instance)
        {
            if (m_Wrapper.m_MyControllerActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_MyControllerActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_MyControllerActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_MyControllerActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_MyControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public MyControllerActions @MyController => new MyControllerActions(this);
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IMyControllerActions
    {
        void OnClick(InputAction.CallbackContext context);
    }
}
