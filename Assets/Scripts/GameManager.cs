using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڵ� �Ծ�
/// 1. Ŭ����, �ż��� �� �빮��
/// 2. �������� ī�� �⺻������ ī�� ǥ���
/// - private �϶��� �տ� _
/// - public �϶��� �빮�� ����
/// - ����) bool Ÿ�Կ� ���ؼ��� is �Ǵ� b�� �����ؼ� �밡�������� ǥ���Ѵ�
/// 3. ������Ƽ�� �빮�� ����
/// </summary>

/*
 * GameManager
 * # Lobby : �κ� ��ҵ� ����
 * # World : �� ���� ��ҵ� ����
 * # UIManager : UI ����
 */

namespace Framework
{
    using UI;
    /// <summary>
    /// �ۻ�� ����
    /// </summary>
    public class GameManager : MonoBahaviourSingleton<GameManager>
    {
        [SerializeField] Initializer _initializer;

        public static UIManager UIManager;

        public static Lobby Lobby;
        
        public static PlayerInfoMation PlayerInfomation;

        private void Awake()
        {

            PlayerInfomation = new PlayerInfoMation();

            Lobby = new Lobby();
            Lobby.Canvas = _initializer.MainCanvas;
            Lobby.EventSystem = _initializer.EventSystem;
            Lobby.lobbyUI = GameObject.Find("LobbyUI").GetComponent<LobbyUI>();
            Lobby.Init();
            UIManager = new UIManager();
            UIManager.Canvas = _initializer.MainCanvas;
            UIManager.EventSystem = _initializer.EventSystem;
            
            
            
           

            DontDestroyOnLoad(UIManager.Canvas);
            DontDestroyOnLoad(UIManager.EventSystem);
            DontDestroyOnLoad(Lobby.Canvas);
            DontDestroyOnLoad(Lobby.EventSystem);
            DontDestroyOnLoad(gameObject);

            //UIManager.CreateLobbyMainUIButton(OnCreateLobbyButton);
        }

        
    }
}
