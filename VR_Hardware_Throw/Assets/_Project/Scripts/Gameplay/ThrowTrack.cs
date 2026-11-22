using UnityEngine;

namespace VRThrow.Gameplay.Scoring
{
    /// <summary>
    /// Se pune pe obiectul "StartLine" din scena. Axa Z (forward) a obiectului
    /// reprezinta directia pistei.
    ///
    /// Referinta catre linia de start NU poate sta pe prefab-ul obiectului aruncat:
    /// un prefab nu poate serializa referinte catre obiecte din scena, iar obiectele
    /// instantiate la runtime ar ramane cu campul gol. De aceea o tine scena.
    /// </summary>
    public class ThrowTrack : MonoBehaviour
    {
        public float GetForwardDistance(Vector3 worldPosition)
        {
            Vector3 toPoint = worldPosition - transform.position;

            // Proiectam directia pe plan orizontal: o linie de start usor inclinata
            // nu trebuie sa influenteze distanta.
            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

            // Produsul scalar cu un vector orizontal ignora deja componenta Y,
            // deci nu mai e nevoie sa o anulam manual pe toPoint.
            // Ignora si deviatia stanga/dreapta: conteaza doar distanta inainte.
            float forwardDistance = Vector3.Dot(toPoint, forward);

            // Aterizarea in spatele liniei de start nu da scor negativ.
            return Mathf.Max(0f, forwardDistance);
        }
    }
}
