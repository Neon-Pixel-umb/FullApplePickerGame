using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] backgroundMusicTracks; // Array to hold multiple background music tracks
    public AudioSource gameOverMusic; // AudioSource for the game over music
    public AudioSource catchAppleSound;
    public AudioSource hitStickSound;
    public AudioSource extraLifeSound;
    public AudioSource gameOverSound;

    private AudioSource currentTrack;
    private bool isGameOver = false; // Flag to indicate game over state

    private void Awake()
    {
        // Ensure there's only one instance of the AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Play a random background music track at the start
        PlayRandomBackgroundMusic();
    }

    private void Update()
    {
        // Only play random background music if the game is not over
        if (!isGameOver && currentTrack != null && !currentTrack.isPlaying)
        {
            PlayRandomBackgroundMusic();
        }
    }

    public void PlayRandomBackgroundMusic()
    {
        if (backgroundMusicTracks.Length > 0)
        {
            // Choose a random track from the array that is not the current track
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, backgroundMusicTracks.Length);
            } while (backgroundMusicTracks[randomIndex] == currentTrack);

            // Stop the current track if it is playing
            if (currentTrack != null)
            {
                currentTrack.Stop();
            }

            // Play the new random track
            currentTrack = backgroundMusicTracks[randomIndex];
            currentTrack.loop = false; // Ensure background music does not loop
            currentTrack.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (currentTrack != null && currentTrack.isPlaying)
        {
            currentTrack.Stop();
        }
    }

    public void PlayGameOverMusic()
    {
        StopBackgroundMusic(); // Stop any playing background music
        isGameOver = true; // Set game over state to true
        gameOverMusic.loop = true; // Ensure game over music loops
        gameOverMusic.Play(); // Play the game over music
    }

    public void StopAllMusic()
    {
        // Stop all music
        StopBackgroundMusic();
        if (gameOverMusic.isPlaying)
        {
            gameOverMusic.Stop();
        }
        isGameOver = false; // Reset the game over state
    }

    public void PlayCatchAppleSound()
    {
        catchAppleSound.Play();
    }

    public void PlayHitStickSound()
    {
        hitStickSound.Play();
    }

    public void PlayExtraLifeSound()
    {
        extraLifeSound.Play();
    }

    public void PlayGameOverSound()
    {
        gameOverSound.Play();
    }
}
