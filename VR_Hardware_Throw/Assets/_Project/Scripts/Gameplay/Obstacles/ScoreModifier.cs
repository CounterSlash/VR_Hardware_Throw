using UnityEngine;
using UnityEngine.Events;
using VRThrow.Gameplay.Throwing;
using VRThrow.Gameplay.Scoring;
using VRThrow.Gameplay;


namespace VRThrow.Gameplay.Obstacles
{
    /// <summary>
    /// Modifica scorul unei aruncari cand obiectul aruncat atinge acest collider.
    /// Functioneaza pe trigger (inel, zona bonus) si pe collider solid (bariera, tinta).
    /// Nu calculeaza scorul: doar depune modificatori pe ThrowableObject, iar
    /// ScoreManager ii aplica la aterizare.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ScoreModifier : MonoBehaviour
    {
        [Header("Efect")]
        [Tooltip("Puncte adaugate (pozitiv) sau scazute (negativ) din scorul aruncarii.")]
        [SerializeField] private float pointsDelta;

        [Tooltip("Modificare relativa a multiplicatorului: +0.5 inseamna x1.5, -0.5 inseamna x0.5.")]
        [SerializeField] private float multiplierDelta;

        [Header("Conditii")]
        [Tooltip("Efectul se aplica doar daca obiectul trece in directia setata (ex. prin inel, nu pe dos).")]
        [SerializeField] private bool requireEntryDirection;

        [Tooltip("Directia valida de trecere, in spatiul local al acestui obiect.")]
        [SerializeField] private Vector3 localEntryDirection = Vector3.forward;

        [Header("Dupa aplicare")]
        [Tooltip("Obiect dezactivat dupa aplicare (ex. tinta care dispare). Lasa gol ca sa ramana.")]
        [SerializeField] private GameObject disableOnApplied;

        [Tooltip("Feedback: sunet, VFX, popup. Se declanseaza o singura data per aruncare.")]
        [SerializeField] private UnityEvent onApplied;

        private void OnTriggerEnter(Collider other)
        {
            TryApply(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            TryApply(collision.collider);
        }

        private void TryApply(Collider other)
        {
            // Serverul are collidere pe copii: cautam radacina care contine ThrowableObject.
            ThrowableObject throwable = other.GetComponentInParent<ThrowableObject>();
            if (throwable == null)
            {
                return;
            }

            if (requireEntryDirection && !IsMovingInEntryDirection(other))
            {
                return;
            }

            // ThrowableObject decide daca efectul conteaza (doar in zbor, o data per sursa).
            if (!throwable.TryApplyModifier(this, pointsDelta, multiplierDelta))
            {
                return;
            }

            onApplied?.Invoke();

            if (disableOnApplied != null)
            {
                disableOnApplied.SetActive(false);
            }
        }

        private bool IsMovingInEntryDirection(Collider other)
        {
            Rigidbody body = other.attachedRigidbody;
            if (body == null)
            {
                return false;
            }

#if UNITY_6000_0_OR_NEWER
            Vector3 velocity = body.linearVelocity;
#else
            Vector3 velocity = body.velocity;
#endif

            Vector3 worldEntry = transform.TransformDirection(localEntryDirection.normalized);
            return Vector3.Dot(velocity, worldEntry) > 0f;
        }
    }
}