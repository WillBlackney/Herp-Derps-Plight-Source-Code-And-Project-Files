using UnityEngine;
using UnityEngine.Events;

namespace Assets.HeroEditor.Common.CommonScripts
{
    /// <summary>
    /// Can be used to handle animation state events (Enter/Update/Exit).
    /// </summary>
    public class StateHandler : StateMachineBehaviour
    {
        public string Name;
        public UnityEvent StateEnter;
        public UnityEvent StateUpdate;
        public UnityEvent StateExit;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateEnter?.Invoke();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateUpdate?.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateExit?.Invoke();
        }
    }
}