using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// <summary>
    /// Unity Input System'den gelen verileri dinler ve ilgili Event Channel'lara iletir.
    /// Hem Gameplay hem de UI inputlarını yöneten merkezi servis.
    /// </summary>
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameplayControl.IPlayerActions
    {
        // Event Channels - Inspector üzerinden atanacak
        [Header("Gameplay Events")]
        public Vector2EventChannelSO moveEventChannel;
        
        public Vector2EventChannelSO targetDirectionChannel;
        
        public VoidEventChannelSO AEventChannel;
        public VoidEventChannelSO BEventChannel;
        public VoidEventChannelSO XEventChannel;
        public VoidEventChannelSO YEventChannel;
        
        public VoidEventChannelSO LBEventChannel;
        public VoidEventChannelSO RBEventChannel;
        public VoidEventChannelSO RTEventChannel;
        public VoidEventChannelSO LTEventChannel;
        
        [Header("Camera Events")]
        public Vector2EventChannelSO cameraInputChannel;
    
        // Unity'nin Generate ettiği C# sınıfı
        private GameplayControl _gameControls;

        private void OnEnable()
        {
            if (_gameControls == null)
            {
                _gameControls = new GameplayControl();
                _gameControls.Player.SetCallbacks(this);
            }
        
            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void EnableGameplayInput()
        {
            Debug.Log("Gameplay Input Enabled");
            _gameControls.Player.Enable();
            // İleride UI map eklediğinde: _gameControls.UI.Disable();
        }

        public void EnableUIInput()
        {
            _gameControls.Player.Disable();
            // İleride UI map eklediğinde: _gameControls.UI.Enable();
        }

        public void DisableAllInput()
        {
            _gameControls.Player.Disable();
        }

        // --- IGameplayActions Interface Implementasyonu ---

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            moveEventChannel.RaiseEvent(moveInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            cameraInputChannel.RaiseEvent(lookInput);
        }

        public void OnX(InputAction.CallbackContext context)
        {
            if (context.performed)
                XEventChannel.RaiseEvent();
        }

        public void OnA(InputAction.CallbackContext context)
        {
            if (context.performed)
                AEventChannel.RaiseEvent();
        }

        public void OnB(InputAction.CallbackContext context)
        {
            if (context.performed)
                BEventChannel.RaiseEvent();
        }

        public void OnY(InputAction.CallbackContext context)
        {
            if (context.performed)
                YEventChannel.RaiseEvent();
        }

        public void OnLB(InputAction.CallbackContext context)
        {
            if (context.performed)
                LBEventChannel.RaiseEvent();
        }

        public void OnRB(InputAction.CallbackContext context)
        {
            if (context.performed)
                RBEventChannel.RaiseEvent();
        }

        public void OnLT(InputAction.CallbackContext context)
        {
            if (context.performed)
                LTEventChannel.RaiseEvent();
        }

        public void OnRT(InputAction.CallbackContext context)
        {
            if (context.performed)
                RTEventChannel.RaiseEvent();
        }

        public void OnDpad(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
                
            Vector2 dpadInput = context.ReadValue<Vector2>();
            if (dpadInput == Vector2.zero)
                return;
            
            targetDirectionChannel.RaiseEvent(dpadInput);
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }
    }
}