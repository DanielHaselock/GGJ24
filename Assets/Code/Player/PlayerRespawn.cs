using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float timeForRespawn;

    [SerializeField] private Vector2 offscreenPos = new Vector2(100, 100);

    BaseLevelManager levelManager; //Maybe change this

    public void onDeath()
    {
        levelManager = FindFirstObjectByType<BaseLevelManager>();
        setPlayerKinematic(true);
        transform.position = offscreenPos;
        startRespawnPlayer();
    }

    public void startRespawnPlayer()
    {
        StartCoroutine(respawnPlayer());
    }

    public void stopRespawnPlayer()
    {
        StopAllCoroutines();
    }

    IEnumerator respawnPlayer()
    {
        yield return new WaitForSeconds(timeForRespawn);
        transform.position = levelManager.getSpawnPointForPlayer(gameObject);
        setPlayerKinematic(false);
    }

    public void setPlayerKinematic(bool p)
    {
        this.GetComponent<Rigidbody2D>().bodyType = (p ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic);

        if (p)
            this.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

}
