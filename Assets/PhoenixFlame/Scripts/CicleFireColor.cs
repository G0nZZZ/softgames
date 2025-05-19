using UnityEngine;

namespace PhoenixFlame
{
    [RequireComponent(typeof(Animator))]
    public class CycleFireColor : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int NextColorTrigger = Animator.StringToHash("TriggerColor");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // This method is called by the UI Button
        public void NextColor()
        {
            if (_animator != null)
            {
                Debug.Log("NextColorTrigger");
                _animator.SetTrigger(NextColorTrigger);
            }
        }
    }
}
