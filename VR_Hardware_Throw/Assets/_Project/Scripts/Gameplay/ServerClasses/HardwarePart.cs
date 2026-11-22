using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using VRThrow.Gameplay.Hardware;
using VRThrow.Gameplay.Throwing;

namespace VRThrow.Gameplay.ServerClasses
{
    [RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
    public sealed class HardwarePart : MonoBehaviour
    {
        [SerializeField] private HardwareData hardwareData;

        private bool isInstalled;

        public HardwareData Data => hardwareData;

        public bool TryInstall(CustomBuildServer server)
        {
            if (isInstalled || server == null || hardwareData == null)
            {
                return false;
            }

            XRGrabInteractable grabInteractable = GetComponentInParent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                return false;
            }

            Rigidbody body = GetComponentInParent<Rigidbody>();
            ThrowableObject throwable = GetComponentInParent<ThrowableObject>();
            if (!server.TryAddHardwareData(hardwareData))
            {
                return false;
            }

            transform.SetParent(server.transform, true);

            if (body != null && transform == body.transform)
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                body.isKinematic = true;
                body.detectCollisions = false;
            }

            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
            }

            if (throwable != null)
            {
                throwable.enabled = false;
            }

            isInstalled = true;
            return true;
        }

        private void OnValidate()
        {
            if (hardwareData == null)
            {
                Debug.LogWarning($"{name}: Assign a HardwareData asset to this hardware part.", this);
            }
        }
    }
}
