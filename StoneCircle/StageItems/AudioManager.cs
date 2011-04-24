using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Media;


namespace StoneCircle
{
    class AudioManager
    {
        Dictionary<String, SoundEffect> effectsLibrary = new Dictionary<String, SoundEffect>();
        List<ActorSoundEffect> currentSounds = new List<ActorSoundEffect>();
        Dictionary<String, Song> BGMList = new Dictionary<String, Song>();
        Song currentBGM;
        AudioListener Player;

        public AudioManager()
        {
            Player = new AudioListener(); 



        }


        public void Initialize()
        {



        }

        public void PlayEffect(String title)
        {   
            effectsLibrary[title].Play();

        }

        public void InstantiateEffect(String title, Actor owner)
        {
            currentSounds.Add(new ActorSoundEffect(effectsLibrary[title], owner, Player, false));

        }

        public void InstantiateEffect(String title, Actor Owner, bool loop)
        {

            currentSounds.Add(new ActorSoundEffect(effectsLibrary[title], Owner, Player, loop));

        }

        private void loadSound(ContentManager CM, String assetName, String callName){

            SoundEffect asset = CM.Load<SoundEffect>(assetName);
           
            effectsLibrary.Add(callName, asset);
            
        }

        private void loadSong(ContentManager CM, String assetName, String callName)
        {

            Song asset = CM.Load<Song>(assetName);
            BGMList.Add(callName, asset);

        }

        public void SetSong(String callName)
        {
            currentBGM = BGMList[callName];
            MediaPlayer.Volume = .2f;
            MediaPlayer.IsRepeating =true;
            MediaPlayer.Play(currentBGM);

        }

        public void StopSong() { MediaPlayer.Stop(); }

        public void PlayBGM()
        {
            MediaPlayer.Play(currentBGM);

        }


        public void PauseSounds()
        {
            foreach( ActorSoundEffect I in currentSounds) I.Pause();

        }

        public void ResumeSounds()
        {
            foreach (ActorSoundEffect I in currentSounds) I.Resume();
        }

        public void StopSounds()
        {
            currentSounds.Clear();
        }

        public void Load(ContentManager CM)
        {
            loadSong(CM, "LXD", "LXD");
            loadSong(CM, "FlowerWaltz", "FlowerWaltz");
            loadSound(CM, "beep", "menuBeep");
            loadSound(CM, "fire", "fire");
            loadSound(CM, "heartbeat", "HeartBeat");
        }

        public void Load(ContentManager CM, List<String> songs, List<String> effects)
        {
        }

        public void Update(Vector3 Location)
        {
            Player.Position = Location;
            foreach (ActorSoundEffect ASE in currentSounds) ASE.Update(Player);


        }

       





        }








    
    class ActorSoundEffect
        {
            SoundEffectInstance SEI;
            AudioEmitter AE;
            Actor owner;


            public ActorSoundEffect(SoundEffect SE, Actor Owner, AudioListener listener, bool loop)
            {
                SEI = SE.CreateInstance();
                AE = new AudioEmitter();
                owner = Owner;
                AE.Position = owner.Location;
                SEI.IsLooped = loop;
                //SEI.Apply3D(listener, AE);
                SEI.Play();
                
            }

            public void Update(AudioListener listener)
            {
                AE.Position = (owner.Location);
                float diff = (listener.Position - AE.Position).LengthSquared();
                if (diff < 1000000) SEI.Volume = (1 - (diff / 1000000))/4;
                 else SEI.Volume = 0;
                
            }

            public void Pause()
            {
                SEI.Pause();
            }

            public void Play()
            {
                SEI.Play();
            }

            public void Stop()
            {
                SEI.Stop();

            }

            public void Resume()
            {
                SEI.Resume();
            }

            public void Dispose()
            {
                SEI.Dispose();
            }
}
}