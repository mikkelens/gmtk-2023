using Sirenix.OdinInspector;
using UnityEngine;

public class AnimatedGuardScript : MonoBehaviour
{
    [field: SerializeField] public Sprite NormalSprite { get; private set; }
    [field: SerializeField] public Sprite SusSprite { get; private set; }
    [field: SerializeField] public Sprite MegaSusSprite { get; private set; }

    [field: SerializeField, Required] public SpriteRenderer GuardSpriteRenderer { get; private set; }
}