using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    [SerializeField] private GameObject[] podiumPrefabs;

    [SerializeField] private float endSceneTimer = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerManager.Instance.UpdateScoreOrder();
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            //Find the proper podium
            var podium = Instantiate(podiumPrefabs[i], transform);
            //Do the animations based on the placement of the player
            Transform playerDummyTransform = podium.transform.Find("PlayerDummy");
            GameObject playerDummy = playerDummyTransform?.gameObject;
            if (playerDummy != null)
            {
                //Get playersprite
                var player = PlayerManager.Instance.players[i];
                Texture2D palette = player.GetComponent<PlayerCustomization>().palette;
                Material material = player.GetComponent<PlayerCustomization>().material;

                material.SetTexture("_GradientTexture", palette);

                SpriteRenderer spriteRenderer = playerDummy.GetComponent<SpriteRenderer>();
                Animator dummyAnimator = playerDummy.GetComponent<Animator>();
                spriteRenderer.material = material;

                dummyAnimator.runtimeAnimatorController = player.GetComponent<Animator>().runtimeAnimatorController;

                //Display which player they were
                Transform winBalloonTransform = podium.transform.Find("WinBalloon");
                GameObject winBalloon = winBalloonTransform?.gameObject;
                Animator winBalloonAnimator = winBalloon.GetComponent<Animator>();

                switch (i)
                {
                    case 0:
                        dummyAnimator.SetTrigger("Cheer");
                        winBalloonAnimator.SetTrigger("player_1");
                        break;
                    case 1:
                        if (PlayerManager.Instance.players.Count == 2) dummyAnimator.SetTrigger("Lose");
                        else if (PlayerManager.Instance.players.Count == 3) dummyAnimator.SetTrigger("Cry");
                        else dummyAnimator.SetTrigger("Angry");
                        winBalloonAnimator.SetTrigger("player_2");
                        break;
                    case 2:
                        if (PlayerManager.Instance.players.Count == 3) dummyAnimator.SetTrigger("Lose");
                        else dummyAnimator.SetTrigger("Cry");
                        winBalloonAnimator.SetTrigger("player_3");
                        break;
                    case 3:
                        dummyAnimator.SetTrigger("Lose");
                        winBalloonAnimator.SetTrigger("player_4");
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Debug.LogError("Cannot find PlayerDummy");
            }

            Transform playerNumberTransform = podium.transform.Find("PlayerNumber");
            GameObject playerNumber = playerNumberTransform?.gameObject;
            if (playerNumber != null) playerNumber.GetComponent<TextMeshProUGUI>().text = "~P" + (PlayerManager.Instance.players[i].PlayerIndex + 1) + "~";
            else Debug.LogError("Cannot find PlayerNumber");

            //Display the name of the player
            Transform playerNameTransform = podium.transform.Find("PlayerName");
            GameObject playerName = playerNameTransform?.gameObject;
            if (playerName != null) playerName.GetComponent<TextMeshProUGUI>().text = PlayerManager.Instance.players[i].PlayerName;
            else Debug.LogError("Cannot find PlayerName");
            //Display the final score of the player
            Transform playerScoreTransform = podium.transform.Find("PlayerScore");
            GameObject playerScore = playerScoreTransform?.gameObject;
            if (playerScore != null) playerScore.GetComponent<TextMeshProUGUI>().text = "$" + PlayerManager.Instance.players[i].score;
            else Debug.LogError("Cannot find PlayerScore");
        }

        StartCoroutine(EndSceneTimerCoroutine(endSceneTimer));
        
    }

    private IEnumerator EndSceneTimerCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        GameManager.Instance.GoToMainMenu();
    }
}
