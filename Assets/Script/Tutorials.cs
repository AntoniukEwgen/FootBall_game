using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Chat
{
    public GameObject chatObject;
    public TMP_Text chatText;
    public string message;
}

public class Tutorials : MonoBehaviour
{
    public GameObject Tutorial;
    public Chat[] Chats;
    public TMP_Text lastChatText;
    public float pulseDuration = 1f;
    public float pulseScale = 1.1f;
    private int currentChat = 0;
    private bool isTextComplete = false;

    [SerializeField] private GameObject gameResult;
    [SerializeField] private Button startGame;
    [SerializeField] private GameManager gameManager;

    void Awake()
    {
        gameManager.isPaused= true;
        Time.timeScale = 0;
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            Tutorial.SetActive(false);
            gameResult.SetActive(true);
        }
        else
        {
            Tutorial.SetActive(true);
            gameResult.SetActive(false);
        }
    }

    void Start()
    {
        startGame.onClick.AddListener(ToggleGameStart);

        ShowText(Chats[currentChat]);
    }

    void Update()
    {
        if (!Tutorial.activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTextComplete)
            {
                Chats[currentChat].chatObject.SetActive(false);
                if (currentChat + 1 < Chats.Length)
                {
                    currentChat++;
                    ShowText(Chats[currentChat]);
                }
                else
                {
                    PlayerPrefs.SetInt("TutorialCompleted", 1);
                    PlayerPrefs.Save();
                    Tutorial.SetActive(false);
                    gameResult.SetActive(true);
                }
            }
            else
            {
                StopAllCoroutines();
                Chats[currentChat].chatText.text = Chats[currentChat].message;
                isTextComplete = true;
            }
        }
    }

    public void ToggleGameStart()
    {
        gameManager.isPaused = false;
        Time.timeScale = 1;
        gameResult.SetActive(false);
    }

    void ShowText(Chat chat)
    {
        chat.chatObject.SetActive(true);
        chat.chatText.text = "";
        if (currentChat == Chats.Length - 1)
        {
            lastChatText.text = "Tap screen to play";
        }
        else
        {
            lastChatText.text = "Tap screen to continue";
        }
        Pulse(lastChatText.transform);
        StartCoroutine(TypeText(chat));
    }

    void Pulse(Transform target)
    {
        DOTween.To(() => target.localScale, x => target.localScale = x, Vector3.one * pulseScale, pulseDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }


    IEnumerator TypeText(Chat chat)
    {
        isTextComplete = false;
        foreach (char letter in chat.message.ToCharArray())
        {
            chat.chatText.text += letter;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        isTextComplete = true;
    }
}
