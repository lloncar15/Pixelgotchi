using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GimGim.Player
{
    /// <summary>
    /// Provides programmatic access to <see cref="InputActionAsset" />, <see cref="InputActionMap" />, <see cref="InputAction" /> and <see cref="InputControlScheme" /> instances defined in asset "Assets/InputSystem_Actions.inputactions".
    /// </summary>
    /// <remarks>
    /// This class is source generated and any manual edits will be discarded if the associated asset is reimported or modified.
    /// </remarks>
    /// <example>
    /// <code>
    /// using namespace UnityEngine;
    /// using UnityEngine.InputSystem;
    ///
    /// // Example of using an InputActionMap named "Player" from a UnityEngine.MonoBehaviour implementing callback interface.
    /// public class Example : MonoBehaviour, MyActions.IPlayerActions
    /// {
    ///     private MyActions_Actions m_Actions;                  // Source code representation of asset.
    ///     private MyActions_Actions.PlayerActions m_Player;     // Source code representation of action map.
    ///
    ///     void Awake()
    ///     {
    ///         m_Actions = new MyActions_Actions();              // Create asset object.
    ///         m_Player = m_Actions.Player;                      // Extract action map object.
    ///         m_Player.AddCallbacks(this);                      // Register callback interface IPlayerActions.
    ///     }
    ///
    ///     void OnDestroy()
    ///     {
    ///         m_Actions.Dispose();                              // Destroy asset object.
    ///     }
    ///
    ///     void OnEnable()
    ///     {
    ///         m_Player.Enable();                                // Enable all actions within map.
    ///     }
    ///
    ///     void OnDisable()
    ///     {
    ///         m_Player.Disable();                               // Disable all actions within map.
    ///     }
    ///
    ///     #region Interface implementation of MyActions.IPlayerActions
    ///
    ///     // Invoked when "Move" action is either started, performed or canceled.
    ///     public void OnMove(InputAction.CallbackContext context)
    ///     {
    ///         Debug.Log($"OnMove: {context.ReadValue&lt;Vector2&gt;()}");
    ///     }
    ///
    ///     // Invoked when "Attack" action is either started, performed or canceled.
    ///     public void OnAttack(InputAction.CallbackContext context)
    ///     {
    ///         Debug.Log($"OnAttack: {context.ReadValue&lt;float&gt;()}");
    ///     }
    ///
    ///     #endregion
    /// }
    /// </code>
    /// </example>
    public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
    {
        /// <summary>
        /// Provides access to the underlying asset instance.
        /// </summary>
        public InputActionAsset asset { get; }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public @PlayerInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem_Actions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""0aa7e279-566c-4dd1-9e11-4fbaed60eeb1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""135750ef-c18c-4faa-acda-6e27cb077dfd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""0231dccb-bf91-4b7d-895d-f6be704dff31"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""95432f6a-04d7-4506-8d0b-84d10c38cf01"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CancelInteraction"",
                    ""type"": ""Button"",
                    ""id"": ""530d1685-ca4a-4399-ac75-33184fdb7587"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e76a330e-c1a7-48f6-90dc-afde14c7c5a4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Choice1"",
                    ""type"": ""Button"",
                    ""id"": ""ba093571-d5b1-486d-b5a0-66e5f9277010"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Choice2"",
                    ""type"": ""Button"",
                    ""id"": ""2e54e0b1-7b92-420c-9d2d-0e7c2b5b340c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1d71083c-43f7-4bcc-b738-a59c2f2c847a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""f928fdc2-0475-4e16-b76a-0e1c53ca7f84"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b30dff95-e358-4dc5-ae5e-ae8898d401c7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2407921b-8666-4283-80e2-e316b8aeef09"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""57563bee-0f91-4473-ad07-78e4a734f1cb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""73947354-1e6a-48bd-ae57-901fa86f138c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fb182322-7eb5-4c54-a7c1-038414e35bc6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""26158183-ef7b-4330-8b7f-91745c9d39e5"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""edc60501-ab39-42ff-b08d-afa857069de6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""24317184-9e26-4477-83bf-0ad451775a3d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""78ca5a9c-7e03-44cd-9fd5-b17f6b4e37e8"",
                    ""path"": ""<XRController>/{Primary2DAxis}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6e6f007-7875-41d7-b6bc-568d3eb8e70e"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b4a4d2a-85f2-498c-80cc-8f447a0a2f56"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24045026-f572-4eae-8f53-2bd45f3ef304"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d76bdd62-7948-4e08-a290-cb9c31f3ef44"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Touch"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""569d11e6-1bd2-485f-a4f8-c82c5e931a46"",
                    ""path"": ""<Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76535099-b64c-4ca8-8f3c-c83c160f56b5"",
                    ""path"": ""<XRController>/{PrimaryAction}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4fbc081-7b4d-40fc-9a15-982411816753"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b39ec46-1133-4ced-a789-bd22a3aa11f4"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4979c30a-c9d7-4b7a-9825-87e611cdd5a9"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45343762-f4c3-4733-8b55-df79aa9805a8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CancelInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb46b9f8-01e1-46ec-8dcf-866ec3061bf9"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CancelInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bb7efd5-4a6e-4892-aa76-705f65e585da"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bda7c004-99ae-46e7-933c-eb8037015040"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choice1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b229429e-e760-4322-8d46-a28cf1728ef5"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choice2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""272f6d14-89ba-496f-b7ff-215263d3219f"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c95b2375-e6d9-4b88-9c4c-c5e76515df4b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""7607c7b6-cd76-4816-beef-bd0341cfe950"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""15cef263-9014-4fd5-94d9-4e4a6234a6ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""32b35790-4ed0-4e9a-aa41-69ac6d629449"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3c7022bf-7922-4f7c-a998-c437916075ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""44b200b1-1557-4083-816c-b22cbdf77ddf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dad70c86-b58c-4b17-88ad-f5e53adf419e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0489e84a-4833-4c40-bfae-cea84b696689"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""24908448-c609-4bc3-a128-ea258674378a"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9caa3d8a-6b2f-4e8e-8bad-6ede561bd9be"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""809f371f-c5e2-4e7a-83a1-d867598f40dd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9144cbe6-05e1-4687-a6d7-24f99d23dd81"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2db08d65-c5fb-421b-983f-c71163608d67"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""58748904-2ea9-4a80-8579-b500e6a76df8"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ba04515-75aa-45de-966d-393d9bbd1c14"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""712e721c-bdfb-4b23-a86c-a0d9fcfea921"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcd248ae-a788-4676-a12e-f4d81205600b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1f04d9bc-c50b-41a1-bfcc-afb75475ec20"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fb8277d4-c5cd-4663-9dc7-ee3f0b506d90"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""e25d9774-381c-4a61-b47c-7b6b299ad9f9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3db53b26-6601-41be-9887-63ac74e79d19"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0cb3e13e-3d90-4178-8ae6-d9c5501d653f"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0392d399-f6dd-4c82-8062-c1e9c0d34835"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""942a66d9-d42f-43d6-8d70-ecb4ba5363bc"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ff527021-f211-4c02-933e-5976594c46ed"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""563fbfdd-0f09-408d-aa75-8642c4f08ef0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb480147-c587-4a33-85ed-eb0ab9942c43"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2bf42165-60bc-42ca-8072-8c13ab40239b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85d264ad-e0a0-4565-b7ff-1a37edde51ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""74214943-c580-44e4-98eb-ad7eebe17902"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cea9b045-a000-445b-95b8-0c171af70a3b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8607c725-d935-4808-84b1-8354e29bab63"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4cda81dc-9edd-4e03-9d7c-a71a14345d0b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9e92bb26-7e3b-4ec4-b06b-3c8f8e498ddc"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Gamepad;Touch;Joystick;XR"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82627dcc-3b13-4ba9-841d-e4b746d6553e"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Gamepad;Touch;Joystick;XR"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c52c8e0b-8179-41d3-b8a1-d149033bbe86"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1394cbc-336e-44ce-9ea8-6007ed6193f7"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5693e57a-238a-46ed-b5ae-e64e6e574302"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4faf7dc9-b979-4210-aa8c-e808e1ef89f5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d66d5ba-88d7-48e6-b1cd-198bbfef7ace"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47c2a644-3ebc-4dae-a106-589b7ca75b59"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb9e6b34-44bf-4381-ac63-5aa15d19f677"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38c99815-14ea-4617-8627-164d27641299"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c191405-5738-4d4b-a523-c6a301dbf754"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24066f69-da47-44f3-a07e-0015fb02eb2e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7236c0d9-6ca3-47cf-a6ee-a97f5b59ea77"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23e01e3a-f935-4948-8d8b-9bcac77714fb"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
            m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
            m_Player_CancelInteraction = m_Player.FindAction("CancelInteraction", throwIfNotFound: true);
            m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
            m_Player_Choice1 = m_Player.FindAction("Choice1", throwIfNotFound: true);
            m_Player_Choice2 = m_Player.FindAction("Choice2", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
            m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
            m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
            m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
            m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
            m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
            m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
            m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
            m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
            m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        }

        ~@PlayerInputActions()
        {
            UnityEngine.Debug.Assert(!m_Player.enabled, "This will cause a leak and performance issues, PlayerInputActions.Player.Disable() has not been called.");
            UnityEngine.Debug.Assert(!m_UI.enabled, "This will cause a leak and performance issues, PlayerInputActions.UI.Disable() has not been called.");
        }

        /// <summary>
        /// Destroys this asset and all associated <see cref="InputAction"/> instances.
        /// </summary>
        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindingMask" />
        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.devices" />
        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.controlSchemes" />
        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Contains(InputAction)" />
        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.GetEnumerator()" />
        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Enable()" />
        public void Enable()
        {
            asset.Enable();
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.Disable()" />
        public void Disable()
        {
            asset.Disable();
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.bindings" />
        public IEnumerable<InputBinding> bindings => asset.bindings;

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindAction(string, bool)" />
        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        /// <inheritdoc cref="UnityEngine.InputSystem.InputActionAsset.FindBinding(InputBinding, out InputAction)" />
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Player
        private readonly InputActionMap m_Player;
        private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Attack;
        private readonly InputAction m_Player_Interact;
        private readonly InputAction m_Player_CancelInteraction;
        private readonly InputAction m_Player_Pause;
        private readonly InputAction m_Player_Choice1;
        private readonly InputAction m_Player_Choice2;
        /// <summary>
        /// Provides access to input actions defined in input action map "Player".
        /// </summary>
        public struct PlayerActions
        {
            private @PlayerInputActions m_Wrapper;

            /// <summary>
            /// Construct a new instance of the input action map wrapper class.
            /// </summary>
            public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
            /// <summary>
            /// Provides access to the underlying input action "Player/Move".
            /// </summary>
            public InputAction @Move => m_Wrapper.m_Player_Move;
            /// <summary>
            /// Provides access to the underlying input action "Player/Attack".
            /// </summary>
            public InputAction @Attack => m_Wrapper.m_Player_Attack;
            /// <summary>
            /// Provides access to the underlying input action "Player/Interact".
            /// </summary>
            public InputAction @Interact => m_Wrapper.m_Player_Interact;
            /// <summary>
            /// Provides access to the underlying input action "Player/CancelInteraction".
            /// </summary>
            public InputAction @CancelInteraction => m_Wrapper.m_Player_CancelInteraction;
            /// <summary>
            /// Provides access to the underlying input action "Player/Pause".
            /// </summary>
            public InputAction @Pause => m_Wrapper.m_Player_Pause;
            /// <summary>
            /// Provides access to the underlying input action "Player/Choice1".
            /// </summary>
            public InputAction @Choice1 => m_Wrapper.m_Player_Choice1;
            /// <summary>
            /// Provides access to the underlying input action "Player/Choice2".
            /// </summary>
            public InputAction @Choice2 => m_Wrapper.m_Player_Choice2;
            /// <summary>
            /// Provides access to the underlying input action map instance.
            /// </summary>
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
            public void Enable() { Get().Enable(); }
            /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
            public void Disable() { Get().Disable(); }
            /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
            public bool enabled => Get().enabled;
            /// <summary>
            /// Implicitly converts an <see ref="PlayerActions" /> to an <see ref="InputActionMap" /> instance.
            /// </summary>
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            /// <summary>
            /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
            /// </summary>
            /// <param name="instance">Callback instance.</param>
            /// <remarks>
            /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
            /// </remarks>
            /// <seealso cref="PlayerActions" />
            public void AddCallbacks(IPlayerActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @CancelInteraction.started += instance.OnCancelInteraction;
                @CancelInteraction.performed += instance.OnCancelInteraction;
                @CancelInteraction.canceled += instance.OnCancelInteraction;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Choice1.started += instance.OnChoice1;
                @Choice1.performed += instance.OnChoice1;
                @Choice1.canceled += instance.OnChoice1;
                @Choice2.started += instance.OnChoice2;
                @Choice2.performed += instance.OnChoice2;
                @Choice2.canceled += instance.OnChoice2;
            }

            /// <summary>
            /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
            /// </summary>
            /// <remarks>
            /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
            /// </remarks>
            /// <seealso cref="PlayerActions" />
            private void UnregisterCallbacks(IPlayerActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Attack.started -= instance.OnAttack;
                @Attack.performed -= instance.OnAttack;
                @Attack.canceled -= instance.OnAttack;
                @Interact.started -= instance.OnInteract;
                @Interact.performed -= instance.OnInteract;
                @Interact.canceled -= instance.OnInteract;
                @CancelInteraction.started -= instance.OnCancelInteraction;
                @CancelInteraction.performed -= instance.OnCancelInteraction;
                @CancelInteraction.canceled -= instance.OnCancelInteraction;
                @Pause.started -= instance.OnPause;
                @Pause.performed -= instance.OnPause;
                @Pause.canceled -= instance.OnPause;
                @Choice1.started -= instance.OnChoice1;
                @Choice1.performed -= instance.OnChoice1;
                @Choice1.canceled -= instance.OnChoice1;
                @Choice2.started -= instance.OnChoice2;
                @Choice2.performed -= instance.OnChoice2;
                @Choice2.canceled -= instance.OnChoice2;
            }

            /// <summary>
            /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="PlayerActions.UnregisterCallbacks(IPlayerActions)" />.
            /// </summary>
            /// <seealso cref="PlayerActions.UnregisterCallbacks(IPlayerActions)" />
            public void RemoveCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            /// <summary>
            /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
            /// </summary>
            /// <remarks>
            /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
            /// </remarks>
            /// <seealso cref="PlayerActions.AddCallbacks(IPlayerActions)" />
            /// <seealso cref="PlayerActions.RemoveCallbacks(IPlayerActions)" />
            /// <seealso cref="PlayerActions.UnregisterCallbacks(IPlayerActions)" />
            public void SetCallbacks(IPlayerActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        /// <summary>
        /// Provides a new <see cref="PlayerActions" /> instance referencing this action map.
        /// </summary>
        public PlayerActions @Player => new PlayerActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
        private readonly InputAction m_UI_Navigate;
        private readonly InputAction m_UI_Submit;
        private readonly InputAction m_UI_Cancel;
        private readonly InputAction m_UI_Point;
        private readonly InputAction m_UI_Click;
        private readonly InputAction m_UI_RightClick;
        private readonly InputAction m_UI_MiddleClick;
        private readonly InputAction m_UI_ScrollWheel;
        private readonly InputAction m_UI_TrackedDevicePosition;
        private readonly InputAction m_UI_TrackedDeviceOrientation;
        /// <summary>
        /// Provides access to input actions defined in input action map "UI".
        /// </summary>
        public struct UIActions
        {
            private @PlayerInputActions m_Wrapper;

            /// <summary>
            /// Construct a new instance of the input action map wrapper class.
            /// </summary>
            public UIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
            /// <summary>
            /// Provides access to the underlying input action "UI/Navigate".
            /// </summary>
            public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
            /// <summary>
            /// Provides access to the underlying input action "UI/Submit".
            /// </summary>
            public InputAction @Submit => m_Wrapper.m_UI_Submit;
            /// <summary>
            /// Provides access to the underlying input action "UI/Cancel".
            /// </summary>
            public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
            /// <summary>
            /// Provides access to the underlying input action "UI/Point".
            /// </summary>
            public InputAction @Point => m_Wrapper.m_UI_Point;
            /// <summary>
            /// Provides access to the underlying input action "UI/Click".
            /// </summary>
            public InputAction @Click => m_Wrapper.m_UI_Click;
            /// <summary>
            /// Provides access to the underlying input action "UI/RightClick".
            /// </summary>
            public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
            /// <summary>
            /// Provides access to the underlying input action "UI/MiddleClick".
            /// </summary>
            public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
            /// <summary>
            /// Provides access to the underlying input action "UI/ScrollWheel".
            /// </summary>
            public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
            /// <summary>
            /// Provides access to the underlying input action "UI/TrackedDevicePosition".
            /// </summary>
            public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
            /// <summary>
            /// Provides access to the underlying input action "UI/TrackedDeviceOrientation".
            /// </summary>
            public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
            /// <summary>
            /// Provides access to the underlying input action map instance.
            /// </summary>
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Enable()" />
            public void Enable() { Get().Enable(); }
            /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.Disable()" />
            public void Disable() { Get().Disable(); }
            /// <inheritdoc cref="UnityEngine.InputSystem.InputActionMap.enabled" />
            public bool enabled => Get().enabled;
            /// <summary>
            /// Implicitly converts an <see ref="UIActions" /> to an <see ref="InputActionMap" /> instance.
            /// </summary>
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            /// <summary>
            /// Adds <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
            /// </summary>
            /// <param name="instance">Callback instance.</param>
            /// <remarks>
            /// If <paramref name="instance" /> is <c>null</c> or <paramref name="instance"/> have already been added this method does nothing.
            /// </remarks>
            /// <seealso cref="UIActions" />
            public void AddCallbacks(IUIActions instance)
            {
                if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
            }

            /// <summary>
            /// Removes <see cref="InputAction.started"/>, <see cref="InputAction.performed"/> and <see cref="InputAction.canceled"/> callbacks provided via <param cref="instance" /> on all input actions contained in this map.
            /// </summary>
            /// <remarks>
            /// Calling this method when <paramref name="instance" /> have not previously been registered has no side-effects.
            /// </remarks>
            /// <seealso cref="UIActions" />
            private void UnregisterCallbacks(IUIActions instance)
            {
                @Navigate.started -= instance.OnNavigate;
                @Navigate.performed -= instance.OnNavigate;
                @Navigate.canceled -= instance.OnNavigate;
                @Submit.started -= instance.OnSubmit;
                @Submit.performed -= instance.OnSubmit;
                @Submit.canceled -= instance.OnSubmit;
                @Cancel.started -= instance.OnCancel;
                @Cancel.performed -= instance.OnCancel;
                @Cancel.canceled -= instance.OnCancel;
                @Point.started -= instance.OnPoint;
                @Point.performed -= instance.OnPoint;
                @Point.canceled -= instance.OnPoint;
                @Click.started -= instance.OnClick;
                @Click.performed -= instance.OnClick;
                @Click.canceled -= instance.OnClick;
                @RightClick.started -= instance.OnRightClick;
                @RightClick.performed -= instance.OnRightClick;
                @RightClick.canceled -= instance.OnRightClick;
                @MiddleClick.started -= instance.OnMiddleClick;
                @MiddleClick.performed -= instance.OnMiddleClick;
                @MiddleClick.canceled -= instance.OnMiddleClick;
                @ScrollWheel.started -= instance.OnScrollWheel;
                @ScrollWheel.performed -= instance.OnScrollWheel;
                @ScrollWheel.canceled -= instance.OnScrollWheel;
                @TrackedDevicePosition.started -= instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started -= instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= instance.OnTrackedDeviceOrientation;
            }

            /// <summary>
            /// Unregisters <param cref="instance" /> and unregisters all input action callbacks via <see cref="UIActions.UnregisterCallbacks(IUIActions)" />.
            /// </summary>
            /// <seealso cref="UIActions.UnregisterCallbacks(IUIActions)" />
            public void RemoveCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            /// <summary>
            /// Replaces all existing callback instances and previously registered input action callbacks associated with them with callbacks provided via <param cref="instance" />.
            /// </summary>
            /// <remarks>
            /// If <paramref name="instance" /> is <c>null</c>, calling this method will only unregister all existing callbacks but not register any new callbacks.
            /// </remarks>
            /// <seealso cref="UIActions.AddCallbacks(IUIActions)" />
            /// <seealso cref="UIActions.RemoveCallbacks(IUIActions)" />
            /// <seealso cref="UIActions.UnregisterCallbacks(IUIActions)" />
            public void SetCallbacks(IUIActions instance)
            {
                foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        /// <summary>
        /// Provides a new <see cref="UIActions" /> instance referencing this action map.
        /// </summary>
        public UIActions @UI => new UIActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        /// <summary>
        /// Provides access to the input control scheme.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputControlScheme" />
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        /// <summary>
        /// Provides access to the input control scheme.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputControlScheme" />
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_TouchSchemeIndex = -1;
        /// <summary>
        /// Provides access to the input control scheme.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputControlScheme" />
        public InputControlScheme TouchScheme
        {
            get
            {
                if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
                return asset.controlSchemes[m_TouchSchemeIndex];
            }
        }
        private int m_JoystickSchemeIndex = -1;
        /// <summary>
        /// Provides access to the input control scheme.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputControlScheme" />
        public InputControlScheme JoystickScheme
        {
            get
            {
                if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
                return asset.controlSchemes[m_JoystickSchemeIndex];
            }
        }
        private int m_XRSchemeIndex = -1;
        /// <summary>
        /// Provides access to the input control scheme.
        /// </summary>
        /// <seealso cref="UnityEngine.InputSystem.InputControlScheme" />
        public InputControlScheme XRScheme
        {
            get
            {
                if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
                return asset.controlSchemes[m_XRSchemeIndex];
            }
        }
        /// <summary>
        /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "Player" which allows adding and removing callbacks.
        /// </summary>
        /// <seealso cref="PlayerActions.AddCallbacks(IPlayerActions)" />
        /// <seealso cref="PlayerActions.RemoveCallbacks(IPlayerActions)" />
        public interface IPlayerActions
        {
            /// <summary>
            /// Method invoked when associated input action "Move" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnMove(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Attack" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnAttack(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Interact" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnInteract(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "CancelInteraction" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnCancelInteraction(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Pause" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnPause(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Choice1" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnChoice1(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Choice2" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnChoice2(InputAction.CallbackContext context);
        }
        /// <summary>
        /// Interface to implement callback methods for all input action callbacks associated with input actions defined by "UI" which allows adding and removing callbacks.
        /// </summary>
        /// <seealso cref="UIActions.AddCallbacks(IUIActions)" />
        /// <seealso cref="UIActions.RemoveCallbacks(IUIActions)" />
        public interface IUIActions
        {
            /// <summary>
            /// Method invoked when associated input action "Navigate" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnNavigate(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Submit" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnSubmit(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Cancel" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnCancel(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Point" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnPoint(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "Click" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnClick(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "RightClick" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnRightClick(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "MiddleClick" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnMiddleClick(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "ScrollWheel" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnScrollWheel(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "TrackedDevicePosition" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnTrackedDevicePosition(InputAction.CallbackContext context);
            /// <summary>
            /// Method invoked when associated input action "TrackedDeviceOrientation" is either <see cref="UnityEngine.InputSystem.InputAction.started" />, <see cref="UnityEngine.InputSystem.InputAction.performed" /> or <see cref="UnityEngine.InputSystem.InputAction.canceled" />.
            /// </summary>
            /// <seealso cref="UnityEngine.InputSystem.InputAction.started" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.performed" />
            /// <seealso cref="UnityEngine.InputSystem.InputAction.canceled" />
            void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        }
    }
}
