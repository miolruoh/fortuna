using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static AudioSource _audioSource;
    private static bool gameSoundOn;
    public static bool GameSoundOn
    {
        get {return gameSoundOn;}
        set 
        {
            gameSoundOn = value;
        }
    }
    private static bool musicOn;
    public static bool MusicOn
    {
        get {return musicOn;}
        set 
        {
            musicOn = value;
        }
    }
    [SerializeField] GameObject musicPrefab;
    public Sprite gameSoundOnIcon;
    public Sprite gameSoundOffIcon;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;
    private Button musicButton;
    private Button gameSoundButton;

    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if(GameObject.FindGameObjectsWithTag("Music").Count<GameObject>() <= 0)
        {
            Instantiate(musicPrefab, instance.transform);
            DontDestroyOnLoad(instance);
            _audioSource = GameObject.FindGameObjectWithTag("MusicAudioSource").GetComponent<AudioSource>();
            gameSoundOn = true;
            musicOn = true;            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void MuteToggleMusic()
    {
        if(MusicOn)
        {
            _audioSource.mute = true;
        }
        else
        {
            _audioSource.mute = false;
        }
    }

    private void MuteToggleSFX()
    {
        if(GameSoundOn)
        {
            _audioSource.mute = true;
        }
        else
        {
            _audioSource.mute = false;
        }
    }

    public void CheckMusicToggleExistence(Button button)
    {
        if(button != null)
            {
                musicButton = button;
                musicButton.onClick.AddListener(OnClickMusic);
            }
    }
    public void CheckGameSoundToggleExistence(Button button)
    {
        if(button != null)
        {
            gameSoundButton = button;
            gameSoundButton.onClick.AddListener(OnClickGameSound);
        }
    }


    public void OnClickGameSound()
    {
        if(GameSoundOn)
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOffIcon;
            MuteToggleSFX();
            GameSoundOn = false;
        }
        else
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOnIcon;
            MuteToggleSFX();
            GameSoundOn = true;
        }
    }
    public void OnClickMusic()
    {
        
        if(MusicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            MuteToggleMusic();
            MusicOn = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
            MuteToggleMusic();
            MusicOn = true;
        }
    }

    private void OnEnable() 
    {
        GameMenu.checkIfMusicButtonExists += CheckMusicToggleExistence;
        GameMenu.checkIfSFXButtonExists += CheckGameSoundToggleExistence;
        SettingsControl.checkIfMusicButtonExists += CheckMusicToggleExistence;
        SettingsControl.checkIfSFXButtonExists += CheckGameSoundToggleExistence;
    }

    private void OnDisable()
    {
        GameMenu.checkIfMusicButtonExists -= CheckMusicToggleExistence;
        GameMenu.checkIfSFXButtonExists -= CheckGameSoundToggleExistence;
        SettingsControl.checkIfMusicButtonExists -= CheckMusicToggleExistence;
        SettingsControl.checkIfSFXButtonExists -= CheckGameSoundToggleExistence;
    }
}