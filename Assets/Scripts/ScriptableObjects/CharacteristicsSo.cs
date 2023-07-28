using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create CharacteristicsSO", fileName = "CharacteristicsSO", order = 0)]
    public class CharacteristicsSo : ScriptableObject
    {
        public float MoveSpeed;
        public int Hp;
        public float ShootInterval;
        public int Damage;
        
        [EnumToggleButtons]
        public ViewType ViewType;
        [ShowIf("ViewType",ViewType.Player)]
        [Header("For player")]
        public float AngleLerpFactor;
        [ShowIf("ViewType",ViewType.Bot)]
        [Header("For bots")]
        public float MoveDistance;
        [ShowIf("ViewType",ViewType.Bot)]
        public float TimeOfImmobility;
        [ShowIf("ViewType",ViewType.Bot)]
        public int inContactDamage;
    }

    public enum ViewType 
    {
        Bot,
        Player
    }
}