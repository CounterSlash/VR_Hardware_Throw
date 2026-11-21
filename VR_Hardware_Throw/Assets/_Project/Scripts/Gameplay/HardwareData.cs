using UnityEngine;

namespace VRThrow.Gameplay.Hardware
{
    /// <summary>
    /// Date de design pentru un tip de hardware (ProLiant, Storinator etc.).
    /// Sunt ScriptableObject ca sa adaugam un obiect nou fara sa scriem cod nou,
    /// iar designul (multiplicatori, masa) sa se poata ajusta din Inspector.
    /// Asset-urile se pun in Assets/_Project/Data/Hardware/.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Hardware_", menuName = "VRThrow/Hardware Data")]
    public class HardwareData : ScriptableObject
    {
        [SerializeField] private string displayName = "Hardware";

        [Tooltip("Pondere de scor: scorul aruncarii = distanta * aceasta valoare.")]
        [SerializeField, Min(0.1f)] private float scoreMultiplier = 1f;

        [Tooltip("Masa fizica aplicata pe Rigidbody. Obiectele grele sunt mai greu de aruncat, deci merita un multiplier mai mare.")]
        [SerializeField, Min(0.1f)] private float massKg = 2f;

        // Expunem doar getter-e: datele sunt read-only la runtime, ca sa nu
        // modificam din greseala un asset partajat intre toate instantele.
        public string DisplayName => displayName;
        public float ScoreMultiplier => scoreMultiplier;
        public float MassKg => massKg;
    }
}
