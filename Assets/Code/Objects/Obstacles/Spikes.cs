using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : GenericObstacle
{

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ActiveInactiveLoop());
    }

    private IEnumerator ActiveInactiveLoop() { 
        while (true)
        {
            yield return new WaitForSeconds(1.875f);
            m_animator.SetTrigger("Return");

            yield return new WaitForSeconds(1.875f);
            m_animator.SetTrigger("Emerge");
        }
    }
}
