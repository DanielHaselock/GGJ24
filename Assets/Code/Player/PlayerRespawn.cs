using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float timeForRespawn;

    [SerializeField] private Vector2 offscreenPos = new Vector2(100, 100);

    BaseLevelManager levelManager; //Maybe change this

    private ReachCheckpoint lastCheckPoint;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene s0, Scene s1)
    {
        lastCheckPoint = null;
    }

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

        if(lastCheckPoint == null)
            transform.position = levelManager.getSpawnPointForPlayer(gameObject);
        else
            transform.position = lastCheckPoint.gameObject.transform.position;

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
