using UnityEngine;
using UnityEngine.Events;

namespace Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts
{
    public class StateHandler : StateMachineBehaviour
    {
        public string Name;
        public UnityEvent StateEnterEvent;
        public UnityEvent StateUpdateEvent;
        public UnityEvent StateExit;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateEnterEvent?.Invoke();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateUpdateEvent?.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateExit?.Invoke();
        }
    }
}