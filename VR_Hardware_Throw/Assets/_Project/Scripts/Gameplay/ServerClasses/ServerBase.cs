using System.Collections.Generic;
using UnityEngine;
using VRThrow.Gameplay.Hardware;

namespace VRThrow.Gameplay.ServerClasses
{
    public abstract class ServerBase : MonoBehaviour
    {
        protected abstract List<HardwareData> HardwareComponents { get; }

        public bool TryAddHardwareData(HardwareData hardwareData)
        {
            if (hardwareData == null || HardwareComponents == null)
            {
                return false;
            }

            HardwareComponents.Add(hardwareData);

            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null && TotalMassKg > 0f)
            {
                body.mass = TotalMassKg;
            }

            return true;
        }

        public float ScoreMultiplier
        {
            get
            {
                float multiplier = 1f;
                List<HardwareData> components = HardwareComponents;

                if (components == null)
                {
                    return multiplier;
                }

                foreach (HardwareData component in components)
                {
                    if (component != null)
                    {
                        multiplier += component.ScoreMultiplier;
                    }
                }

                return multiplier;
            }
        }

        public float TotalMassKg
        {
            get
            {
                float totalMassKg = 0f;
                List<HardwareData> components = HardwareComponents;

                if (components == null)
                {
                    return totalMassKg;
                }

                foreach (HardwareData component in components)
                {
                    if (component != null)
                    {
                        totalMassKg += component.MassKg;
                    }
                }

                return totalMassKg;
            }
        }
    }
}