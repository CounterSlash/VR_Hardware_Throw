using System.Collections.Generic;
using UnityEngine;
using VRThrow.Gameplay.Hardware;

namespace VRThrow.Gameplay.ServerClasses
{
    public sealed class CustomBuildServer : ServerBase
    {
        [Header("Hardware Components")]
        [Tooltip("Drag HardwareData assets here (for example RAM, CPU, and storage).")]
        [SerializeField]
        private List<HardwareData> hardwareComponents = new List<HardwareData>();

        protected override List<HardwareData> HardwareComponents => hardwareComponents;
    }
}