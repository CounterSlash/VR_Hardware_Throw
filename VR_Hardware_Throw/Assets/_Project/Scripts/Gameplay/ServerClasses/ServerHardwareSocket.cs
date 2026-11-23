using UnityEngine;

namespace VRThrow.Gameplay.ServerClasses
{
    [RequireComponent(typeof(Collider))]
    public sealed class ServerHardwareSocket : MonoBehaviour
    {
        [SerializeField] private CustomBuildServer server;

        private void Awake()
        {
            if (server == null)
            {
                server = GetComponentInParent<CustomBuildServer>();
            }

            Collider socketCollider = GetComponent<Collider>();
            if (!socketCollider.isTrigger)
            {
                Debug.LogError($"{name}: ServerHardwareSocket requires its Collider to have Is Trigger enabled.", this);
            }

            if (server == null)
            {
                Debug.LogError($"{name}: No CustomBuildServer is assigned or found in the parent hierarchy.", this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            TryInstallPart(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryInstallPart(other);
        }

        private void TryInstallPart(Collider other)
        {
            if (server == null)
            {
                return;
            }

            HardwarePart part = other.GetComponentInParent<HardwarePart>();
            if (part != null)
            {
                part.TryInstall(server);
            }
        }
    }
}
