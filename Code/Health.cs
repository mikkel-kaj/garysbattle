
using Sandbox;

public sealed class Health : Component, Component.IDamageable   
{
    [Property] private int MaxHealth { get; set; } = 100;
    public int CurrentHealth { get; private set; }

    protected override void OnStart()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage( int amount )
    {
        Log.Info( $"Took {amount} damage, HP: {CurrentHealth}/{MaxHealth}" );
        CurrentHealth -= amount;
        if ( CurrentHealth <= 0 )
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject.Destroy();
    }

    public void OnDamage( in DamageInfo damage )
    {
	    TakeDamage( (int) damage.Damage );
    }
}
