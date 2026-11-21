namespace VRThrow.Core
{
    /// <summary>
    /// Regula de scor, izolata de Unity. Fiind C# pur, se poate testa in EditMode
    /// fara sa pornesti scena. Cand adaugam si timpul in formula (vezi User Story Map),
    /// modificam DOAR aceasta clasa, nu MonoBehaviour-urile din joc.
    /// </summary>
    public static class ScoreCalculator
    {
        public static float Calculate(float distanceMeters, float multiplier)
        {
            // Date invalide nu trebuie sa produca scor negativ sau infinit.
            if (distanceMeters <= 0f || multiplier <= 0f)
            {
                return 0f;
            }

            return distanceMeters * multiplier;
        }
    }
}
