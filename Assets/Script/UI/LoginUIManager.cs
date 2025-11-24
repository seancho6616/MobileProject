using UnityEngine;
using TMPro; // ★ TMP 필수
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIManager : MonoBehaviour
{
    [Header("Panels (패널 오브젝트)")]
    public GameObject loginPanel;   // Login_Panel 오브젝트
    public GameObject signinPanel;  // Signin_Panel 오브젝트

    [Header("Login UI")]
    public TMP_InputField loginIdInput;
    public TMP_InputField loginPwInput; 
    
    public Button loginButton;
    public Button goToRegisterButton;

    [Header("Register UI")]
    public TMP_InputField regIdInput;   
    public TMP_InputField regPwInput;
    public TMP_InputField regNickInput;
    
    public Button registerSubmitButton;

    void Start()
    {
        // 초기 화면 설정 (로그인 창만 보이고, 회원가입 창은 숨김)
        loginPanel.SetActive(true);
        signinPanel.SetActive(false);

        // 버튼 기능 연결
        loginButton.onClick.AddListener(OnLoginClick);
        goToRegisterButton.onClick.AddListener(ShowRegisterPanel);
        registerSubmitButton.onClick.AddListener(OnRegisterSubmitClick);
    }

    // 패널 전환 로그인 <-> 회원가입
    void ShowRegisterPanel()
    {
        // 로그인 창 숨기고, 회원가입 창 보여주기
        loginPanel.SetActive(false);
        signinPanel.SetActive(true);
        
        // 가입 창 열 때 입력 칸 초기화
        regIdInput.text = "";
        regPwInput.text = "";
        regNickInput.text = "";
    }

    void ShowLoginPanel()
    {
        // 회원가입 창 숨기고, 로그인 창 보여주기
        signinPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    // 로그인 요청
    void OnLoginClick()
    {
        string id = loginIdInput.text;
        string pw = loginPwInput.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            Debug.Log("로그인 실패, 아이디와 비밀번호를 입력하세요.");
            return;
        }

        StartCoroutine(NetworkManager.Instance.Login(id, pw, (isSuccess) =>
        {
            if (isSuccess)
            {
                Debug.Log("로그인 성공, 게임 씬으로 이동");
                SceneManager.LoadScene("StartScene"); 
            }
        }));
    }

    // 회원가입 요청
    void OnRegisterSubmitClick()
    {
        string id = regIdInput.text;
        string pw = regPwInput.text;
        string nick = regNickInput.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(nick))
        {
            Debug.Log("회원가입 실패, 모든 정보를 입력해주세요.");
            return;
        }

        StartCoroutine(NetworkManager.Instance.Register(id, pw, nick, (isSuccess) =>
        {
            if (isSuccess)
            {
                Debug.Log("가입 성공, 로그인 화면으로 돌아갑니다.");
                ShowLoginPanel(); 
            }
            else
            {
                Debug.Log("가입 실패, 중복을 확인하세요.");
            }
        }));
    }
}