using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillableBallInteractable : BallInteractable
{
    public UnityEvent OnDeath;
    public UnityEvent OnResurrection;
    protected bool isDead = false;
    [SerializeField]
    private float life;
    [SerializeField]
    private float lifeAtStart;
    private bool isDeadOnLastCheckPoint;
    private float lifeOnLastCheckPoint;
    // Start is called before the first frame update

    protected override void Initialise()
    {
        base.Initialise();
        isDead = false;
        life = lifeAtStart;
    }
    protected override void RecordCurrentState(Transform t)
    {
        base.RecordCurrentState(t);
        lifeOnLastCheckPoint = life;
        isDeadOnLastCheckPoint = isDead;
    }
    protected override void RevertToPreviousCheckPoint()
    {
        base.RevertToPreviousCheckPoint();
        life = lifeOnLastCheckPoint;
        isDead = isDeadOnLastCheckPoint;
        if (isDead)
        {
            // Die();
        }
        else
        {
            OnResurrection.Invoke();
        }
    }
    protected override void Interact()
    {
       // base.Interact();
        if (life > 0)
        {
            Debug.Log("Interact Life>0");
            life -= LightManager.Instance.DecreaseLight();
            base.Interact();
        }
        else
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        OnDeath.Invoke();
        isDead = true;
    }

}
