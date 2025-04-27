using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

public static class SoundManager
{
    private static Dictionary<string, SoundEffect> soundEffects = new();
    private static Dictionary<string, Song> songs = new();

    public static void LoadSoundEffect(ContentManager content, string name)
    {
        soundEffects[name] = content.Load<SoundEffect>(name);
    }

    public static void LoadSong(ContentManager content, string name)
    {
        songs[name] = content.Load<Song>(name);
    }

    public static void PlaySoundEffect(string name, float volume = 1f, float pitch = 0f, float pan = 0f)
    {
        if (soundEffects.TryGetValue(name, out var sound))
            sound.Play(volume, pitch, pan);
    }

    public static void PlaySong(string name, bool isRepeating = true)
    {
        if (songs.TryGetValue(name, out var song))
        {
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Play(song);
        }
    }

    public static void StopSong()
    {
        MediaPlayer.Stop();
    }
}