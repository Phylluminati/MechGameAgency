using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using CardRunner;

namespace CardRunner{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public string cardDescriptor;
    public CardType cardType;
    public int cost;
    public int dmg;
    public List<Effect> applyEffect;
    public DamageType damageType;
    public List<int> effectSeverity;

    public enum CardType
        {
            PHY,
            HACK,
            MULT,
            RUST,
            NULL
        }
    public enum Effect
        {
            MELT,
            DRAIN,
            STUN,
            AAA,
            SELFDESTRUCT,
            RECURSIVE,
            SELFSWITCH,
            ENEMYSWITCH,
            DRAW,
            DOUBLEDMG,
            DOUBLEDMGHACK,
            DOUBLEDMGPHY,
            SELFHARM,
            DRAWPHYIFSHIELD,
            DRAWHACKIFPHY,
            PILEDRIVER,
            ODAY,
            
            NON,
            ENEMYATTACK

        }
    public enum DamageType
        {
            HACK,
            PHY,
            RUST
        }
}

}