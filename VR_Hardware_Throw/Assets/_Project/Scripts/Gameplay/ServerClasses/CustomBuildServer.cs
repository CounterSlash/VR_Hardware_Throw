using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using _Project.Scripts.Gameplay.HardwareTypes;
using VRThrow.Gameplay.Hardware;

namespace VRThrow.Gameplay.ServerClasses
{
    public sealed class CustomBuildServer : ServerBase
    {
        [Header("Maximum Component Counts")]
        [SerializeField, Min(0)] private int maxCPUComponents = 1;
        [SerializeField, Min(0)] private int maxRAMComponents = 1;
        [SerializeField, Min(0)] private int maxGPUComponents = 1;
        [SerializeField, Min(0)] private int maxStorageComponents = 1;
        [SerializeField, Min(0)] private int maxExtraComponents = 1;

        [Header("Hardware Components")]
        [SerializeField] private List<HardwareData> CPUComponents = new List<HardwareData>();
        [SerializeField] private List<HardwareData> RAMComponents = new List<HardwareData>();
        [SerializeField] private List<HardwareData> GPUComponents = new List<HardwareData>();
        [SerializeField] private List<HardwareData> StorageComponents = new List<HardwareData>();
        [SerializeField] private List<HardwareData> ExtraComponents = new List<HardwareData>();

        [Header("Component Positions (Local)")]
        [SerializeField] private List<ComponentPosition> CPUComponentPositions = new List<ComponentPosition>();
        [SerializeField] private List<ComponentPosition> RAMComponentPositions = new List<ComponentPosition>();
        [SerializeField] private List<ComponentPosition> GPUComponentPositions = new List<ComponentPosition>();
        [SerializeField] private List<ComponentPosition> StorageComponentPositions = new List<ComponentPosition>();
        [SerializeField] private List<ComponentPosition> ExtraComponentPositions = new List<ComponentPosition>();

        protected override IEnumerable<HardwareData> HardwareComponents
        {
            get
            {
                if (CPUComponents != null)
                {
                    foreach (HardwareData component in CPUComponents)
                    {
                        yield return component;
                    }
                }

                if (RAMComponents != null)
                {
                    foreach (HardwareData component in RAMComponents)
                    {
                        yield return component;
                    }
                }

                if (GPUComponents != null)
                {
                    foreach (HardwareData component in GPUComponents)
                    {
                        yield return component;
                    }
                }

                if (StorageComponents != null)
                {
                    foreach (HardwareData component in StorageComponents)
                    {
                        yield return component;
                    }
                }

                if (ExtraComponents != null)
                {
                    foreach (HardwareData component in ExtraComponents)
                    {
                        yield return component;
                    }
                }
            }
        }

        protected override bool TryStoreHardwareData(HardwareData hardwareData, out Vector3 localPosition,
            out Quaternion localRotation)
        {
            switch (hardwareData.HardwareType)
            {
                case HardwareTypes.CPU:
                    return TryAddToComponents(CPUComponents, CPUComponentPositions, maxCPUComponents, hardwareData, out localPosition, out localRotation);
                case HardwareTypes.RAM:
                    return TryAddToComponents(RAMComponents, RAMComponentPositions, maxRAMComponents, hardwareData, out localPosition, out localRotation);
                case HardwareTypes.GPU:
                    return TryAddToComponents(GPUComponents, GPUComponentPositions, maxGPUComponents, hardwareData, out localPosition, out localRotation);
                case HardwareTypes.Storage:
                    return TryAddToComponents(StorageComponents, StorageComponentPositions, maxStorageComponents, hardwareData, out localPosition, out localRotation);
                case HardwareTypes.Extra:
                    return TryAddToComponents(ExtraComponents, ExtraComponentPositions, maxExtraComponents, hardwareData, out localPosition, out localRotation);
                case HardwareTypes.Unspecified:
                    localPosition = Vector3.zero;
                    localRotation = Quaternion.identity;
                    return false;
                default:
                    localPosition = Vector3.zero;
                    localRotation = Quaternion.identity;
                    return false;
            }
        }

        private void OnValidate()
        {
            CPUComponentPositions ??= new List<ComponentPosition>();
            RAMComponentPositions ??= new List<ComponentPosition>();
            GPUComponentPositions ??= new List<ComponentPosition>();
            StorageComponentPositions ??= new List<ComponentPosition>();
            ExtraComponentPositions ??= new List<ComponentPosition>();

            EnsurePositionCount(CPUComponentPositions, maxCPUComponents);
            EnsurePositionCount(RAMComponentPositions, maxRAMComponents);
            EnsurePositionCount(GPUComponentPositions, maxGPUComponents);
            EnsurePositionCount(StorageComponentPositions, maxStorageComponents);
            EnsurePositionCount(ExtraComponentPositions, maxExtraComponents);
        }

        private void OnDrawGizmosSelected()
        {
            DrawComponentPositions(CPUComponentPositions, CPUComponents, Color.red, "CPU");
            DrawComponentPositions(RAMComponentPositions, RAMComponents, Color.green, "RAM");
            DrawComponentPositions(GPUComponentPositions, GPUComponents, Color.blue, "GPU");
            DrawComponentPositions(StorageComponentPositions, StorageComponents, Color.yellow, "Storage");
            DrawComponentPositions(ExtraComponentPositions, ExtraComponents, Color.magenta, "Extra");
        }

        private void DrawComponentPositions(List<ComponentPosition> positions, List<HardwareData> components,
            Color color, string componentType)
        {
            if (positions == null)
            {
                return;
            }

            Gizmos.color = color;
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 worldPosition = transform.TransformPoint(positions[i].LocalPosition);
                Quaternion worldRotation = transform.rotation * positions[i].LocalRotation;
                Gizmos.DrawWireSphere(worldPosition, 0.08f);
                DrawRotationAxis(worldPosition, worldRotation, Vector3.right, Color.red);
                DrawRotationAxis(worldPosition, worldRotation, Vector3.up, Color.green);
                DrawRotationAxis(worldPosition, worldRotation, Vector3.forward, Color.blue);

#if UNITY_EDITOR
                bool occupied = components != null && i < components.Count && components[i] != null;
                Handles.Label(worldPosition, $"{componentType} [{i}] {(occupied ? "Occupied" : "Available")}");
#endif
            }
        }

        private static void DrawRotationAxis(Vector3 origin, Quaternion rotation, Vector3 axis, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(origin, origin + rotation * axis * 0.25f);
        }

        private static bool TryAddToComponents(List<HardwareData> components, List<ComponentPosition> positions,
            int maximum, HardwareData hardwareData, out Vector3 localPosition, out Quaternion localRotation)
        {
            localPosition = Vector3.zero;
            localRotation = Quaternion.identity;
            if (components == null || positions == null)
            {
                return false;
            }

            for (int slotIndex = 0; slotIndex < maximum; slotIndex++)
            {
                if (slotIndex < components.Count && components[slotIndex] != null)
                {
                    continue;
                }

                if (slotIndex >= positions.Count)
                {
                    return false;
                }

                if (slotIndex < components.Count)
                {
                    components[slotIndex] = hardwareData;
                }
                else
                {
                    components.Add(hardwareData);
                }

                localPosition = positions[slotIndex].LocalPosition;
                localRotation = positions[slotIndex].LocalRotation;
                return true;
            }

            return false;
        }

        private static void EnsurePositionCount(List<ComponentPosition> positions, int requiredCount)
        {
            requiredCount = Mathf.Max(0, requiredCount);

            while (positions.Count < requiredCount)
            {
                positions.Add(default);
            }

            if (positions.Count > requiredCount)
            {
                positions.RemoveRange(requiredCount, positions.Count - requiredCount);
            }
        }
    }
}
