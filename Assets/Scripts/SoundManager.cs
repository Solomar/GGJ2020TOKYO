using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public struct MusicNote
{
    public int pitch;
    public int octave;
    public int duration;
    public Dictionary<float, MusicNote> nextNotes;
}

public class SoundManager : MonoBehaviour
{ 
    private static SoundManager instance;
    private List<GameObject> audioSources = new List<GameObject>();
    private static Dictionary<string, List<List<MusicNote>>> tracks = new Dictionary<string, List<List<MusicNote>>>();
    private static AudioSource musicSource;
    private int bpm = 0;
    private string currentTrack = "";

    public string CurrentTrack { get { return currentTrack; } }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject sound = new GameObject("SoundManager");
                DontDestroyOnLoad(sound);
                SoundManager s = sound.AddComponent<SoundManager>();
                instance = s;

                musicSource = sound.AddComponent<AudioSource>();
            }
            return instance;
        }
    }

    private static void FillTracks()
    {
        FillTrack("Audio");
    }

    private static void FillTrack(string trackName)
    {
        List<List<MusicNote>> notes = new List<List<MusicNote>>();
        tracks.Add(trackName, notes);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(Resources.Load<TextAsset>(trackName).text);
        ReadNodes(doc.DocumentElement.ChildNodes, trackName);
    }

    private static void ReadNodes(XmlNodeList nodes, string trackName)
    {
        foreach (XmlNode node in nodes)
        {
            if (node.Name == "note")
            {
                bool isChord = false;
                int pitch = 0;
                int octave = 0;
                int duration = 0;
                int track = 0;
                string pos = "";
                foreach (XmlNode c in node.ChildNodes)
                {
                    if (c.Name == "chord") isChord = true;
                    if (c.Name == "rest") pitch = 0;
                    if (c.Name == "pitch")
                    {
                        pos = node.Attributes["default-x"].InnerText;
                        foreach (XmlNode c2 in c.ChildNodes)
                        {
                            if (c2.Name == "step")
                            {
                                string step = c2.InnerText;
                                if (step == "A") pitch = 2;
                                if (step == "B") pitch = 4;
                                if (step == "C") pitch = 6;
                                if (step == "D") pitch = 8;
                                if (step == "E") pitch = 10;
                                if (step == "F") pitch = 12;
                                if (step == "G") pitch = 14;
                            }
                            if (c2.Name == "octave") octave = int.Parse(c2.InnerText);
                        }
                    }
                    if (c.Name == "duration") duration = int.Parse(c.InnerText);
                    if (c.Name == "staff") track = int.Parse(c.InnerText);
                }

                if (!isChord)
                {
                    MusicNote mn = new MusicNote();
                    mn.duration = duration;
                    mn.pitch = pitch;
                    mn.octave = octave;
                    if (track == 0) track = 1;
                    if (tracks[trackName].Count < track) tracks[trackName].Add(new List<MusicNote>());
                    tracks[trackName][track - 1].Add(mn);
                }
            }
            else if (node.HasChildNodes)
            {
                ReadNodes(node.ChildNodes, trackName);
            }
        }
    }

    public AudioSource GetAudioSource()
    {
        foreach (GameObject audioSource in audioSources)
        {
            if (audioSource.GetComponent<AudioSource>().isPlaying == false) return audioSource.GetComponent<AudioSource>();
        }

        GameObject source = new GameObject();
        source.transform.parent = transform;
        AudioSource aus = source.AddComponent<AudioSource>();
        audioSources.Add(source);

        return aus;
    }

    public float PlaySoundPitched(string path)
    {
        AudioSource aus = GetAudioSource();
        aus.clip = Resources.Load<AudioClip>(path);
        if (aus.clip != null)
        {
            aus.pitch = Random.Range(0.5f, 1.5f);
            aus.volume = 0.5f;
            aus.Play();
            aus.gameObject.name = aus.clip.name;
            return aus.clip.length;
        }
        else
        {
            Debug.Log("Missing clip " + path);
            return 0f;
        }
    }

    public float PlaySound(string path)
    {
        AudioSource aus = GetAudioSource();
        aus.clip = Resources.Load<AudioClip>(path);
        aus.pitch = 1f;
        aus.Play();
        aus.gameObject.name = aus.clip.name;
        return aus.clip.length;
    }

    public void PlayMusic(string track)
    {
        if (currentTrack == track && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else if (currentTrack == track) return;

        currentTrack = track;
        musicSource.clip = Resources.Load<AudioClip>(track);
        musicSource.volume = 0.03f;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopAudio()
    {
        foreach (GameObject audioSource in audioSources)
        {
            if (audioSource.GetComponent<AudioSource>().isPlaying) audioSource.GetComponent<AudioSource>().Stop();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySound(string name, float volume)
    {
        AudioClip ac = Resources.Load<AudioClip>(name);
        GameObject go = new GameObject();
        AudioSource aus = go.AddComponent<AudioSource>();
        aus.clip = ac;
        aus.volume = volume;
        aus.Play();
        Destroy(go, aus.clip.length);
    }
}
