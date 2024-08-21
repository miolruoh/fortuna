using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sFXAudioSource;
    private bool gameSoundOn;
    private bool musicOn;
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
        if(GameObject.FindGameObjectsWithTag("MusicAudioSource").Count<GameObject>() <= 0)
        {
            Instantiate(_musicAudioSource, instance.transform);
            DontDestroyOnLoad(instance);
            gameSoundOn = true;
            musicOn = true; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume, bool loop)
    {
        // spawn in gameobject
        AudioSource audioSource = Instantiate(_sFXAudioSource, spawnTransform.position, Quaternion.identity);

        // assign the audioclip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        // assign loop
        audioSource.loop = loop;

        //play sound
        audioSource.Play();

        // get length of sfx clip
        float clipLength = audioSource.clip.length;

        // destroy clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }



    private void MuteToggleMusic()
    {
        if(musicOn)
        {
            GameObject.FindObjectOfType<AudioSource>().mute = true;
        }
        else
        {
            GameObject.FindObjectOfType<AudioSource>().mute = false;
        }
    }
    private void MuteToggleSFX()
    {
        if(gameSoundOn)
        {
            _musicAudioSource.mute = true;
        }
        else
        {
            _musicAudioSource.mute = false;
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
        if(gameSoundOn)
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOffIcon;
            MuteToggleSFX();
            gameSoundOn = false;
        }
        else
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOnIcon;
            MuteToggleSFX();
            gameSoundOn = true;
        }
    }
    public void OnClickMusic()
    {
        
        if(musicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            MuteToggleMusic();
            musicOn = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
            MuteToggleMusic();
            musicOn = true;
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