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
        StartCoroutine(Duration());
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(particleSystem.main.duration);

        Destroy(gameObject);
    }
}
