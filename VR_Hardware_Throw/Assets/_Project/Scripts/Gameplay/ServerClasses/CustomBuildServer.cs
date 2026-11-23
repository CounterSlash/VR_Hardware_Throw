using System.Collections.Generic;
using UnityEngine;
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

        protected override bool TryStoreHardwareData(HardwareData hardwareData)
        {
            switch (hardwareData.HardwareType)
            {
                case HardwareTypes.CPU:
                    return TryAddToComponents(CPUComponents, maxCPUComponents, hardwareData);
                case HardwareTypes.RAM:
                    return TryAddToComponents(RAMComponents, maxRAMComponents, hardwareData);
                case HardwareTypes.GPU:
                    return TryAddToComponents(GPUComponents, maxGPUComponents, hardwareData);
                case HardwareTypes.Storage:
                    return TryAddToComponents(StorageComponents, maxStorageComponents, hardwareData);
                case HardwareTypes.Extra:
                    return TryAddToComponents(ExtraComponents, maxExtraComponents, hardwareData);
                case HardwareTypes.Unspecified:
                    return false;
                default:
                    return false;
            }
        }

        private static bool TryAddToComponents(List<HardwareData> components, int maximum, HardwareData hardwareData)
        {
            if (components == null)
            {
                return false;
            }

            int componentCount = 0;
            foreach (HardwareData component in components)
            {
                if (component != null)
                {
                    componentCount++;
                }
            }

            if (componentCount >= maximum)
            {
                return false;
            }

            components.Add(hardwareData);
            return true;
        }
    }
}
