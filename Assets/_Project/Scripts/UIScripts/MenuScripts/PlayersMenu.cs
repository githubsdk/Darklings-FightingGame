using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersMenu : BaseMenu
{
    [SerializeField] private CharacterMenu _characterMenu = default;
    [SerializeField] private RectTransform[] _playerIcons = default;
    [SerializeField] private RectTransform[] _playerGroups = default;
    [SerializeField] private GameObject _cpuTextRight = default;
    [SerializeField] private GameObject _cpuTextLeft = default;
    [SerializeField] private PromptsInput _prompts = default;
    [SerializeField] private BaseMenu[] _childMenues = default;
    private Audio _audio;
    private readonly float _left = -375.0f;
    private readonly float _right = 375.0f;
    private readonly float _center = 0.0f;
    public GameObject CpuTextRight { get { return _cpuTextRight; } private set { } }
    public GameObject CpuTextLeft { get { return _cpuTextLeft; } private set { } }
    public RectTransform[] PlayerGroups { get { return _playerGroups; } set { } }


    void Awake()
    {
        _audio = GetComponent<Audio>();
    }

    private void UpdateVisiblePlayers(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        for (int i = 0; i < _playerIcons.Length; i++)
            _playerIcons[i].GetComponent<PlayerIcon>().SetController();
    }


    public bool IsOnRight()
    {
        for (int i = 0; i < _playerIcons.Length; i++)
            if (_playerIcons[i].anchoredPosition.x == _right)
                return true;
        return false;
    }

    public bool IsOnLeft()
    {
        for (int i = 0; i < _playerIcons.Length; i++)
            if (_playerIcons[i].anchoredPosition.x == _left)
                return true;
        return false;
    }

    public void OpenOtherMenu()
    {
        if (_playerIcons[0].anchoredPosition.x != _center || _playerIcons[1].anchoredPosition.x != _center || _playerIcons[2].anchoredPosition.x != _center)
        {
            _audio.Sound("Pressed").Play();
            if (_playerIcons[0].parent == _playerGroups[2])
            {
                SceneSettings.ControllerTwoScheme = _playerIcons[0].GetComponent<PlayerIcon>().PlayerInput.currentControlScheme;
                SceneSettings.ControllerTwo = _playerIcons[0].GetComponent<PlayerIcon>().PlayerInput.devices[0];
            }
            else if (_playerIcons[1].parent == _playerGroups[2])
            {
                SceneSettings.ControllerTwoScheme = _playerIcons[1].GetComponent<PlayerIcon>().PlayerInput.currentControlScheme;
                SceneSettings.ControllerTwo = _playerIcons[1].GetComponent<PlayerIcon>().PlayerInput.devices[0];
            }
            else if (_playerIcons[2].parent == _playerGroups[2])
            {
                SceneSettings.ControllerTwoScheme = _playerIcons[2].GetComponent<PlayerIcon>().PlayerInput.currentControlScheme;
                SceneSettings.ControllerTwo = _playerIcons[2].GetComponent<PlayerIcon>().PlayerInput.devices[0];
            }
            else
                SceneSettings.ControllerTwo = null;
            if (_playerIcons[0].parent == _playerGroups[0])
            {
                SceneSettings.ControllerOneScheme = _playerIcons[0].GetComponent<PlayerIcon>().PlayerInput.currentControlScheme;
                SceneSettings.ControllerOne = _playerIcons[0].GetComponent<PlayerIcon>().PlayerInput.devices[0];
            }
            else if (_playerIcons[1].parent == _playerGroups[0])
            {
                SceneSettings.ControllerOneScheme = _playerIcons[1].GetComponent<PlayerIcon>().PlayerInput.currentControlScheme;
                SceneSettings.ControllerOne = _playerIcons[1].GetComponent<PlayerIcon>().PlayerInput.devices[0];
            }
            else if (_playerIcons[2].parent == _playerGroups[0])
            {
                SceneSettings.ControllerOneScheme = _playerIcons[2].GetComponent<PlayerIcon>().PlayerInput.currentControlScheme;
                SceneSettings.ControllerOne = _playerIcons[2].GetComponent<PlayerIcon>().PlayerInput.devices[0];
            }
            else
                SceneSettings.ControllerOne = null;
            gameObject.SetActive(false);
            for (int i = 0; i < _playerIcons.Length; i++)
            {
                _playerIcons[i].GetComponent<PlayerIcon>().Center();
                _playerIcons[i].gameObject.SetActive(false);
            }
            _characterMenu.Show();
        }
    }

    public bool ArePlayerIconsLeft()
    {
        for (int i = 0; i < _playerIcons.Length; i++)
            if (_playerIcons[i].anchoredPosition.x != _left)
                return false;
        return true;
    }

    public bool ArePlayerIconsRight()
    {
        for (int i = 0; i < _playerIcons.Length; i++)
            if (_playerIcons[i].anchoredPosition.x != _right)
                return false;
        return true;
    }

    public void OpenKeyboardCoOp()
    {
        _audio.Sound("Pressed").Play();
        SceneSettings.ControllerTwo = _playerIcons[0].GetComponent<PlayerIcon>().PlayerInput.devices[0];
        SceneSettings.ControllerOne = _playerIcons[0].GetComponent<PlayerIcon>().PlayerInput.devices[0];
        gameObject.SetActive(false);
        _characterMenu.Show();
    }

    void OnDisable()
    {
        _cpuTextLeft.SetActive(true);
        _cpuTextRight.SetActive(true);
        InputSystem.onDeviceChange -= UpdateVisiblePlayers;
    }

    private void OnEnable()
    {
        _prompts.gameObject.SetActive(true);
        InputSystem.onDeviceChange += UpdateVisiblePlayers;
        UpdateVisiblePlayers(null, default);
    }
}
