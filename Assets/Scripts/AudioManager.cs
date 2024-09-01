using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sFXAudioSource;
    [SerializeField] private AudioMixer audioMixer;
    private bool gameSoundOn;
    private bool musicOn;
    [SerializeField] private Sprite gameSoundOnIcon;
    [SerializeField] private Sprite gameSoundOffIcon;
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;
    private Button musicButton;
    private Button gameSoundButton;
    [SerializeField] private AudioClip clickSFX;
    private readonly float mute = -80f;
    private readonly float unmute = 0f;
    // Set all the sfx volume values
    private static readonly float clickVolume = 0.2f;
    public static float ClickVolume
    {
        get {return clickVolume;}
    }
    private static readonly float bonkVolume = 1.5f;
    public static float BonkVolume
    {
        get {return bonkVolume;}
    }
    private static readonly float bonkNailVolume = 0.035f;
    public static float BonkNailVolume
    {
        get {return bonkNailVolume;}
    }
    private static readonly float newHighScoreSFXVolume = 0.2f;
    public static float NewHighScoreSFXVolume
    {
        get {return newHighScoreSFXVolume;}
    }
    private static readonly float rollingBallVolume = 0.5f;
    public static float RollingBallVolume
    {
        get {return rollingBallVolume;}
    }
    // Set instance to this class
    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
    }
    // Set what needed for audio
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
    // Create sound effects when needed
    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // spawn in gameobject
        AudioSource audioSource = Instantiate(_sFXAudioSource, spawnTransform.position, Quaternion.identity);

        // assign the audioclip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        // get length of sfx clip
        float clipLength = audioSource.clip.length;

        // destroy clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
    // Set volume of sound effects
    private void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", level);
    }
    // Set volume of music
    private void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", level);
    }
    // Checks if the scene have music volume button
    private void CheckMusicToggleExistence(Button button)
    {
        if(button != null)
            {
                musicButton = button;
                if(musicOn)
                {
                    musicButton.GetComponent<Image>().sprite = musicOnIcon;
                }
                else
                {
                    musicButton.GetComponent<Image>().sprite = musicOffIcon;
                }
                musicButton.onClick.AddListener(OnClickMusic);
            }
    }
    // Checks if the scene have game sound volume button
    private void CheckGameSoundToggleExistence(Button button)
    {
        if(button != null)
        {
            gameSoundButton = button;
            if(gameSoundOn)
            {
                gameSoundButton.GetComponent<Image>().sprite = gameSoundOnIcon;
            }
            else
            {
                gameSoundButton.GetComponent<Image>().sprite = gameSoundOffIcon;
            }
            gameSoundButton.onClick.AddListener(OnClickGameSound);
        }
    }
    //Change sound effect icon when clicking it to mute/unmute
    public void OnClickGameSound()
    {
        if(gameSoundOn)
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOffIcon;
            SetSFXVolume(mute);
            gameSoundOn = false;
        }
        else
        {
            gameSoundButton.GetComponent<Image>().sprite = gameSoundOnIcon;
            SetSFXVolume(unmute);
            instance.PlaySFXClip(clickSFX, transform, clickVolume);
            gameSoundOn = true;
        }
    }
    //Change music icon when clicking it to mute/unmute
    public void OnClickMusic()
    {
        
        if(musicOn)
        {
            musicButton.GetComponent<Image>().sprite = musicOffIcon;
            SetMusicVolume(mute);
            musicOn = false;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOnIcon;
            SetMusicVolume(unmute);
            instance.PlaySFXClip(clickSFX, transform, clickVolume);
            musicOn = true;
        }
    }
    // start event
    private void OnEnable() 
    {
        GameMenu.checkIfMusicButtonExists += CheckMusicToggleExistence;
        GameMenu.checkIfSFXButtonExists += CheckGameSoundToggleExistence;
        SettingsControl.checkIfMusicButtonExists += CheckMusicToggleExistence;
        SettingsControl.checkIfSFXButtonExists += CheckGameSoundToggleExistence;
    }
    // end event
    private void OnDisable()
    {
        GameMenu.checkIfMusicButtonExists -= CheckMusicToggleExistence;
        GameMenu.checkIfSFXButtonExists -= CheckGameSoundToggleExistence;
        SettingsControl.checkIfMusicButtonExists -= CheckMusicToggleExistence;
        SettingsControl.checkIfSFXButtonExists -= CheckGameSoundToggleExistence;
    }
}