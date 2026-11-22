using System;
using UnityEngine;
using VRThrow.Core;

namespace VRThrow.Gameplay.Scoring
{
    /// <summary>Rezultatul unei aruncari, trimis catre oricine asculta (UI, audio, analytics).</summary>
    public readonly struct ThrowScoreInfo
    {
        public readonly float Distance;
        public readonly float Multiplier;
        public readonly float Points;
        public readonly float TotalScore;

        public ThrowScoreInfo(float distance, float multiplier, float points, float totalScore)
        {
            Distance = distance;
            Multiplier = multiplier;
            Points = points;
            TotalScore = totalScore;
        }
    }

    /// <summary>
    /// Tine scorul total al sesiunii. Nu cunoaste UI-ul: publica evenimente,
    /// iar afisarea se ocupa de ScoreView. Asa putem schimba ecranul (sau adauga
    /// sunet la scor) fara sa atingem logica.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [SerializeField] private ThrowTrack track;

        public float TotalScore { get; private set; }

        public event Action<ThrowScoreInfo> ThrowScored;
        public event Action ScoreReset;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                // Distrugem doar componenta, nu tot GameObject-ul: pe el pot sta
                // si alte componente care nu au legatura cu singleton-ul.
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            // Curatam referinta statica, altfel ramane "moarta" la schimbarea scenei.
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void RegisterLanding(Vector3 landingPosition, float multiplier)
        {
            if (track == null)
            {
                Debug.LogError("ScoreManager: lipseste referinta catre ThrowTrack (StartLine).", this);
                return;
            }

            float distance = track.GetForwardDistance(landingPosition);
            float points = ScoreCalculator.Calculate(distance, multiplier);
            TotalScore += points;

            ThrowScored?.Invoke(new ThrowScoreInfo(distance, multiplier, points, TotalScore));
        }

        /// <summary>Apelat la retry de nivel sau la intoarcerea in meniu.</summary>
        public void ResetScore()
        {
            TotalScore = 0f;
            ScoreReset?.Invoke();
        }
    }
}
