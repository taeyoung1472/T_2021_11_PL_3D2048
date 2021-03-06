using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NameManager : MonoBehaviour
{
    [Header("Name")]
    [SerializeField]
    private InputField nameInput;
    //[SerializeField]
    //private InputField changeNameInput;

    [SerializeField]
    private int nameLimit;

    private IEnumerator changeCoroutine;
    private Text namePlaceholder;

    private string nickName;
    private bool isShaking;
    private float dotweenDuration = 0.5f;

    [SerializeField] private GameObject nameInputHold;

    #region 이벤트
    private void Start()
    {
        if (GameManager.Instance.UserInfo.name == null)
        {
            nameInputHold.SetActive(true);
        }
        GameManager.Instance.AdManager.ToggleBannerAd(true);
        namePlaceholder = nameInput.placeholder.GetComponent<Text>();
        nameInput.characterLimit = nameLimit;
        changeCoroutine = ChangePlaceholder();
        StartCoroutine(changeCoroutine);
    }
    #endregion

    #region OnClick
    public void OnClickStart()
    {
        nickName = nameInput.text;
        //Debug.Log("함수 밖");
        if (isShaking) return;
        nickName = nickName.Trim();
        nickName = nickName.Replace("\n", "").Replace("\r", "").Replace("ㅤ", "");
        if (nickName.Length <= 0)
        {
            if (isShaking) return;
            isShaking = true;
            StartCoroutine(ShakeText());
        }
        GameManager.Instance.UserInfo.name = nickName;
        GameManager.Instance.EndInpuut();
        GameManager.Instance.SaveUser();
        print(nickName);
    }
    #endregion

    private IEnumerator ShakeText()
    {
        if (changeCoroutine != null)
        {
            Debug.Log(changeCoroutine);

            StopCoroutine(changeCoroutine);
        }
        Debug.Log("함수 안");

        nameInput.text = "";
        namePlaceholder.color = Color.red;
        namePlaceholder.text = "Wrong!";

        namePlaceholder.transform.DOShakePosition(dotweenDuration, 15f, 20, 45f);

        yield return new WaitForSeconds(dotweenDuration);

        namePlaceholder.color = Color.gray;
        namePlaceholder.text = "Input NickName";

        StartCoroutine(changeCoroutine);
        isShaking = false;
    }
    private IEnumerator ChangePlaceholder()
    {
        char comma = '.';
        string commas = "";
        while (true)
        {
            commas = ".";
            for (int i = 1; i <= 3; i++)
            {
                yield return new WaitWhile(() => nameInput.isFocused);
                namePlaceholder.text = "Input NickName" + commas;
                commas += comma;
                yield return new WaitForSeconds(0.6f);
            }
        }
    }
}
