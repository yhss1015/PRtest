using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip itemGet;
    public AudioClip slash;
    public AudioClip slashAttack;

    public static SoundManager Instance;
    private AudioSource soundSource;

    private void Awake()
    {
        // Singleton 패턴을 통한 유일한 인스턴스 보장
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지됨
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스는 삭제
        }

        // AudioSource 초기화는 Awake에서 하는 것이 더 안전
        soundSource = GetComponent<AudioSource>();
    }

    public void itemGetSound()
    {
        soundSource.PlayOneShot(itemGet);
    }

    public void slashSound()
    {
        soundSource.PlayOneShot(slash);
    }

    public void slashAttackSound()
    {
        soundSource.PlayOneShot(slashAttack);
    }
}
