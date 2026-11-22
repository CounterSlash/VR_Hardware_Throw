using TMPro;
using UnityEngine;
using VRThrow.Gameplay.Scoring;

namespace VRThrow.UI.Hud
{
    /// <summary>
    /// Afiseaza scorul pe un Canvas World Space. Este un View "prost": nu calculeaza nimic,
    /// doar formateaza ce primeste de la ScoreManager prin evenimente.
    /// </summary>
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private TextMeshProUGUI totalText;
        [SerializeField] private TextMeshProUGUI lastThrowText;

        // Referinta prin Inspector, nu prin ScoreManager.Instance: ordinea Awake/OnEnable
        // intre obiecte nu e garantata, iar Instance poate fi inca null in OnEnable.
        private void OnEnable()
        {
            if (scoreManager == null || totalText == null || lastThrowText == null)
            {
                Debug.LogError("ScoreView: completeaza ScoreManager, TotalText si LastThrowText in Inspector.", this);
                enabled = false;
                return;
            }
        

            scoreManager.ThrowScored += OnThrowScored;
            scoreManager.ScoreReset += OnScoreReset;
            OnScoreReset();
        }

        private void OnDisable()
        {
            // Daca OnEnable a iesit devreme, nu avem la ce sa ne dezabonam.
            if (scoreManager == null)
            {
                return;
            }

            scoreManager.ThrowScored -= OnThrowScored;
            scoreManager.ScoreReset -= OnScoreReset;
        }

        private void OnThrowScored(ThrowScoreInfo info)
        {
            totalText.text = $"Scor total: {Mathf.RoundToInt(info.TotalScore)}";
            lastThrowText.text =
                $"Distanta: {info.Distance:F1} m | x{info.Multiplier:0.##} = +{Mathf.RoundToInt(info.Points)} pct";
        }

        private void OnScoreReset()
        {
            totalText.text = "Scor total: 0";
            lastThrowText.text = "Arunca un obiect!";
        }
    }
}
