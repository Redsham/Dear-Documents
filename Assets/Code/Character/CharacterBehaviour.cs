using Character.Animations;
using Gameplay.Persons.Data;
using UnityEngine;

namespace Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        public Person            Person   { get; set; }
        public CharacterAnimator Animator => m_Animator;
    
        [SerializeField] private CharacterAnimator m_Animator;
        

        private void OnValidate()
        {
            m_Animator ??= GetComponentInChildren<CharacterAnimator>();
        }
    }
}