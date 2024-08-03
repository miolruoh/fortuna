using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Music : MonoBehaviour
{
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
    [SerializeField] GameObject audioManager;

    private void Start()
    {
        if(GameObject.FindGameObjectsWithTag("Music").Count<GameObject>() <= 0)
        {
            Instantiate(musicPrefab, audioManager.transform);
            DontDestroyOnLoad(audioManager);
            _audioSource = GameObject.FindGameObjectWithTag("MusicAudioSource").GetComponent<AudioSource>();
            gameSoundOn = true;
            musicOn = true;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public static void MuteMusic()
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
}