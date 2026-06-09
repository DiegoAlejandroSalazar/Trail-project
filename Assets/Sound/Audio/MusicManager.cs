using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MusicManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
	if(instance == null)
	{
	instance = this;
	}
    }
	public void PlaySoundFXClip(AudioClip audioClip, Transform spaceTransform, float volume)
	{
		
		AudioSource audioSource = Instantiate(soundFXObject, spaceTransform.position, Quaternion.identity);
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		audioSource.Play();
		float clipLength = audioSource.clip.length;
		Destroy(audioSource.gameObject, clipLength);

	}
}
