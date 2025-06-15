using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float timeForRespawn;

    [SerializeField] private Vector2 offscreenPos = new Vector2(100, 100);

    BaseLevelManager levelManager;

    private ReachCheckpoint lastCheckPoint;

    private bool shouldRespawn = true;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene s0, Scene s1)
    {
        lastCheckPoint = null;
        setPlayerKinematic(false);
    }

    public void setRespawn(bool pShouldRespawn)
    {
        shouldRespawn = pShouldRespawn;
    }

    public void onDeath()
    {
        levelManager = FindFirstObjectByType<BaseLevelManager>();
        levelManager.OnDeath(this);
        setPlayerKinematic(true);
        transform.position = offscreenPos;
        GetComponent<PlayerController>().resetDeath(); //resets the death effects if they were triggered

        if(shouldRespawn)
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

        if (levelManager != null) // we have ended the level
        {
            if (lastCheckPoint == null)
                transform.position = levelManager.getSpawnPointForPlayer(gameObject);
            else
                transform.position = lastCheckPoint.gameObject.transform.position;
        }

        setPlayerKinematic(false);
    }

    public void setPlayerKinematic(bool p)
    {
        this.GetComponent<Rigidbody2D>().bodyType = (p ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic);

        if (p)
            this.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void setNewCheckpoint(ReachCheckpoint newCheckPoint)
    {
        if (lastCheckPoint && lastCheckPoint.getPriority() > newCheckPoint.getPriority())
            return;

        lastCheckPoint = newCheckPoint;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

}
