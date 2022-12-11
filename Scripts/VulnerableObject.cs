using UnityEngine;

public class VulnerableObject : MonoBehaviour
{
    #region Protected Fields

    protected float health;
    protected float damage;
    protected float speed;
    protected float mana;

    #endregion

    #region Virtual Methods

    public virtual float Health => this.health;
    public virtual void SubHealth(float damage) { this.health -= damage; }
    public virtual void AddHealth(float health) { this.health += health; }
    public virtual void SetSpeedRatio(float speedRatio) { this.speed *= speedRatio; }
    public virtual void SetDamageRatio(float damageRatio) { this.damage *= damageRatio; }
    public virtual void AddMana(float mana) { this.mana += mana; }
    public virtual void SubMana(float mana) { this.mana -= mana; }

    #endregion
}
