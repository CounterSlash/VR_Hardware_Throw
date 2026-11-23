using System.Collections.Generic;
using UnityEngine;
using VRThrow.Gameplay.Hardware;

namespace VRThrow.Gameplay.ServerClasses
{
    public abstract class ServerBase : MonoBehaviour
    {
        protected abstract IEnumerable<HardwareData> HardwareComponents { get; }

        protected abstract bool TryStoreHardwareData(HardwareData hardwareData);

        public bool TryAddHardwareData(HardwareData hardwareData)
        {
            if (hardwareData == null || !TryStoreHardwareData(hardwareData))
            {
                return false;
            }

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
                IEnumerable<HardwareData> components = HardwareComponents;

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
                IEnumerable<HardwareData> components = HardwareComponents;

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