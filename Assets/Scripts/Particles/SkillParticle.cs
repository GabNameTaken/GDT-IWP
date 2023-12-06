using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticle : MonoBehaviour
{
    ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particleSystem.Play();
        StartCoroutine(Duration(particleSystem.main.duration));
    }

    IEnumerator Duration(float duration)
    {
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }

    public void ManualPlay(float duration)
    {
        particleSystem.Play();
        StartCoroutine(Duration(duration));
    }
}
