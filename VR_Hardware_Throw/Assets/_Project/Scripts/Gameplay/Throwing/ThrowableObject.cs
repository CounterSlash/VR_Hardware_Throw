using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRI 3.x. Pe XRI 2.x sterge aceasta linie.
using VRThrow.Gameplay.Scoring;
using VRThrow.Gameplay.ServerClasses;
using System.Collections.Generic;

namespace VRThrow.Gameplay.Throwing
{
    /// <summary>
    /// Detecteaza aruncarea unui obiect si raporteaza aterizarea catre ScoreManager.
    /// Nu calculeaza scorul: doar observa fizica si trimite faptele mai departe.
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
    public class ThrowableObject : MonoBehaviour
    {
        private ServerBase server;

        [Header("Detectie aterizare")]
        [Tooltip("Layer-ele pe care aterizarea conteaza (ex. 'ScoringSurface'). Layer in loc de tag: nu depinde de un string care poate fi scris gresit.")]
        [SerializeField] private LayerMask scoringSurfaces;

        [Tooltip("Timp minim in aer pentru ca aruncarea sa conteze. Elimina obiectele scapate din mana la picioare.")]
        [SerializeField, Min(0f)] private float minAirTime = 0.2f;

        private XRGrabInteractable grabInteractable;
        private bool isInFlight;
        private float releaseTime;

        // Modificatori acumulati in timpul zborului curent. Se golesc la prindere si la aterizare.
        private readonly HashSet<UnityEngine.Object> appliedModifierSources = new HashSet<UnityEngine.Object>();
        private float bonusPoints;
        private float multiplierBonus;

        private float Multiplier => server != null ? server.ScoreMultiplier : 1f;

        private void Awake()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            server = GetComponent<ServerBase>();

            if (server == null || server.TotalMassKg <= 0f)
            {
                return;
            }


            GetComponent<Rigidbody>().mass = server.TotalMassKg;
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        private void OnDisable()
        {
            // Dezabonarea simetrica previne scurgerile si apelurile pe obiecte dezactivate.
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            // Daca jucatorul reia obiectul din aer sau de pe jos, aruncarea anterioara se anuleaza.
            isInFlight = false;
            ClearModifiers();
        }

        private void OnReleased(SelectExitEventArgs args)
        {

            //Debug.Log($"[Throw] released. stillSelected={grabInteractable.isSelected}, canceled={args.isCanceled}", this);

            // Cu doua maini, selectExited se declanseaza si cand o singura mana da drumul.
            // Obiectul e "aruncat" abia cand nu il mai tine nimeni.
            if (grabInteractable.isSelected)
            {
                return;
            }

            // Selectia anulata de sistem (obiect dezactivat, distrus) nu e o aruncare.
            if (args.isCanceled)
            {
                return;
            }

            isInFlight = true;
            releaseTime = Time.time;

            //Debug.Log("[Throw] in flight", this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log($"[Throw] hit {collision.gameObject.name} (layer {LayerMask.LayerToName(collision.gameObject.layer)}), inFlight={isInFlight}, scoringSurface={IsScoringSurface(collision.gameObject)}", this);

            if (!isInFlight || !IsScoringSurface(collision.gameObject))
            {
                return;
            }

            // Primul impact cu suprafata de scor inchide aruncarea, indiferent daca e valida.
            // Un singur punct de iesire evita dubla contorizare la ricosee.
            isInFlight = false;

            //float airTime = Time.time - releaseTime;
            //Debug.Log($"[Throw] airTime={airTime:F2}s, manager={(ScoreManager.Instance != null)}", this);

            // Preluam modificatorii si golim imediat, ca sa nu ramana agatati pe iesirile timpurii de mai jos.
            float pointsFromModifiers = bonusPoints;
            float multiplierFromModifiers = multiplierBonus;
            ClearModifiers();


            if (Time.time - releaseTime < minAirTime || ScoreManager.Instance == null)
            {
                return;
            }

            // Punctul de contact, nu centrul obiectului: un server mare ar primi
            // altfel distanta masurata de la mijlocul lui, nu de unde atinge pista.
            Vector3 landingPoint = collision.GetContact(0).point;
            ScoreManager.Instance.RegisterLanding(
                landingPoint,
                Multiplier,
                pointsFromModifiers,
                multiplierFromModifiers);
        }

        private bool IsScoringSurface(GameObject other)
        {
            return (scoringSurfaces.value & (1 << other.layer)) != 0;
        }

        /// <summary>
        /// Inregistreaza efectul unui obstacol asupra aruncarii curente.
        /// Returneaza false daca obiectul nu e in zbor sau sursa a fost deja aplicata
        /// (evita dubla penalizare cand obiectul ricoseaza intr-o bariera).
        /// </summary>
        public bool TryApplyModifier(UnityEngine.Object source, float pointsDelta, float multiplierDelta)
        {
            if (!isInFlight)
            {
                return false;
            }

            if (!appliedModifierSources.Add(source))
            {
                return false;
            }

            bonusPoints += pointsDelta;
            multiplierBonus += multiplierDelta;
            return true;
        }

        private void ClearModifiers()
        {
            appliedModifierSources.Clear();
            bonusPoints = 0f;
            multiplierBonus = 0f;
        }
    }

    
}