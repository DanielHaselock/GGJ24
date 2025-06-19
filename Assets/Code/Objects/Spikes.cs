using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : GenericObstacle
{
    [SerializeField] private float cycle_time = 1.875f;
    [SerializeField] private bool already_hidden;

    protected override void Start()
    {
        base.Start();
        if (already_hidden == true)
        {
            m_animator.Play("spikes_return");
        }
        else if (already_hidden == false)
        {
            m_animator.Play("spikes_emerge");
        }
        StartCoroutine(ActiveInactiveLoop());
    }

    private IEnumerator ActiveInactiveLoop() { 
        while (true)
        {
            if (already_hidden == true)
            {
                yield return new WaitForSeconds(cycle_time);
                m_animator.SetTrigger("Emerge");

                yield return new WaitForSeconds(cycle_time);
                m_animator.SetTrigger("Return");
            }
            else if (already_hidden == false)
            {
                yield return new WaitForSeconds(cycle_time);
                m_animator.SetTrigger("Return");

                yield return new WaitForSeconds(cycle_time);
                m_animator.SetTrigger("Emerge");
            }
        }
    }
}
