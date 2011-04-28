using System;                                                                          
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using UserMenus;

namespace StoneCircle
{
    /// <summary>
    /// Manages a stage
    /// </summary>
    [Serializable]
    public class StageManager
    {

        internal Dictionary<String, Stage> Stages = new Dictionary<String, Stage>();
        internal Stage openStage;

        internal List<String>StateConditions = new List<String>();

        [NonSerialized] internal ContentManager contentManager;
        [NonSerialized] private GameManager gameManager;

        public StageManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            contentManager = gameManager.ContentManager;

            Stages.Add("region1", new Stage("region1", this));
            Stages["region1"].addActor("Tree1", new Tree(new Vector2(0, 400), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree2", new Tree(new Vector2(100, 400), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree3", new Tree(new Vector2(200, 400), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree4", new Tree(new Vector2(300, 400), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree5", new Tree(new Vector2(400, 400), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree6", new Tree(new Vector2(500, 450), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree7", new Tree(new Vector2(600, 500), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8", new Tree(new Vector2(750, 550), Stages["region1"], gameManager));


            Stages["region1"].addActor("Tree1b", new Tree(new Vector2(0, 750), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree2b", new Tree(new Vector2(50, 950), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree3b", new Tree(new Vector2(200, 950), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree4b", new Tree(new Vector2(300, 850), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree5b", new Tree(new Vector2(400, 800), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree6b", new Tree(new Vector2(500, 850), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree7b", new Tree(new Vector2(600, 800), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8b", new Tree(new Vector2(750, 850), Stages["region1"], gameManager));


            Stages["region1"].addActor("Treea1", new Tree(new Vector2(50, 350), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree2a", new Tree(new Vector2(150, 350), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree3a", new Tree(new Vector2(250, 350), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree4a", new Tree(new Vector2(350, 350), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree5a", new Tree(new Vector2(450, 350), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree6a", new Tree(new Vector2(500, 400), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree7a", new Tree(new Vector2(600, 450), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8a", new Tree(new Vector2(750, 550), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8a2", new Tree(new Vector2(850, 550), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8q", new Tree(new Vector2(950, 550), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8r", new Tree(new Vector2(1050, 550), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8e", new Tree(new Vector2(750, 850), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8c", new Tree(new Vector2(850, 850), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8f", new Tree(new Vector2(950, 850), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8g", new Tree(new Vector2(1050, 875), Stages["region1"], gameManager));
            Stages["region1"].addActor("Tree8h", new Tree(new Vector2(1150, 850), Stages["region1"], gameManager));

            Stages["region1"].addActor("Body1", new Actor("Body1", "Body1", new Vector2(50, 500)));
            Stages["region1"].addActor("Body2", new Actor("Body2", "Body1", new Vector2(180, 650)));
            Stages["region1"].addActor("Body3", new Actor("Body3", "Body1", new Vector2(350, 500)));
            Stages["region1"].addActor("Body4", new Actor("Body4", "Body1", new Vector2(550, 650)));

            Stages["region1"].AddTrigger( new Trigger("Bandage",new TriggerPlayerBoxCondition(new BoundingBox( new Vector3(600, 400, 0), new Vector3(601, 1200, 1)), gameManager.Player), true, true));
            
            Stages["region1"].AddEvent("Bandage", new DialogueEvent("Bandage", Stages["region1"]));
            Stages["region1"].AddTrigger( new Trigger("region2trans", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(300,1200,0), new Vector3(2000,1201,0)), gameManager.Player), true, false));
            Stages["region1"].AddEvent("region2trans", new StageChangeEvent(this, "region2"));
     
            Stages["region1"].AddLines(new Lines("Forest1", "", "Player", "I've got to get out of here... Must... warn... the village", Stages["region1"], Lines.LineType.Player));
            Stages["region1"].AddLines(new Lines("Bandage", "Instructions", "Player", "I'm losing a lot of blood... I'll need to bandage myself if I don't want to bleed to death", Stages["region1"], Lines.LineType.Player));
            Stages["region1"].AddLines(new Lines("Instructions", "", "Player", "Hold RT and press Y. Don't move until you've finished bandaging.", Stages["region1"], Lines.LineType.Player));
            Stages["region1"].AddLines(new Lines("Village", "", "Player", "If I follow this stream to the South I should come across the village of SouthStreamVillage", Stages["region1"], Lines.LineType.Player));

            Stages["region1"].addActor("Fire1", new Fire(new Vector2(-100, 900), Stages["region1"], gameManager));
            Stages["region1"].addLight("Light", new Vector2(-100, 600), 1200);

            
            SerialEventGroup testIntroduction = new SerialEventGroup("Introduction");
            ParallelEventGroup testPar1 = new ParallelEventGroup();
            testPar1.AddEvent(new MoveActorEvent(gameManager.Player, new Vector2(150, 600), Stages["region1"]));
            testPar1.AddEvent(new SetCameraEvent(Stages["region1"].camera, new Vector2(684, 600)));
            testPar1.AddEvent( new PlayerDeactivateEvent(gameManager.Player));
            testIntroduction.AddEvent(testPar1);
            testIntroduction.AddEvent(new DialogueEvent("Forest1", Stages["region1"]));
            ParallelEventGroup testPar2 = new ParallelEventGroup();
            testPar1.AddEvent(new CameraDeactivateEvent(Stages["region1"].camera));
            testPar2.AddEvent(new MoveCameraEvent(Stages["region1"].camera, new Vector2(900, 600), 2000f));
            testPar2.AddEvent(new ScaleCameraEvent(Stages["region1"].camera, .8f, 2000f));
            testIntroduction.AddEvent(testPar2);
            testIntroduction.AddEvent(new AcknowledgePauseEvent(Stages["region1"]));
            testIntroduction.AddEvent(new PlayerReactivateEvent(gameManager.Player));
           // testPar2.AddEvent(new PerformActionEvent(gameManager.Player, "Resting"));
            testIntroduction.AddEvent(new CameraReactivateEvent(Stages["region1"].camera));


            Stages["region1"].AddEvent(testIntroduction);
           // Stages["region1"].AddEvent(new ParallelEventGroup("Intro2", ""));

            
           Stages.Add("region2", new Stage("Region2", this));
           Stages["region2"].addLight(new ActorLightSource(gameManager.Player, 1200f));
            Stages["region2"].AddEvent("Introduction", new Event());
            Stages["region2"].AddTrigger(new Trigger("VillageTrans", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(300, 300, 0), new Vector3(2000, 1201, 0)), gameManager.Player), true, false));
            Stages["region2"].AddEvent("VillageTrans", new StageChangeEvent(this, "Village"));

            Stages.Add("Village", new Stage("Village", this));
            Stages.Add("Cairn", new Stage("Cairn", this));
            
            Stages["Village"].AMBStrength = .2f;
            Stages["Village"].addActor("CenterStone", new Actor("CenterStone", "SarcenStone2", new Vector2(2000, 2000), Stages["Village"]));
            Stages["Village"].addLight(new LightSource("SarcenGlow", new Vector2(950, 600), 500f, Stages["Village"], null));
            Stages["Village"].AMBColor = new Vector3(.5f, .5f, 1f);
            Stages["Village"].addActor("Follower", new Follower("Follower", new Vector2(1400, 600), Stages["Village"], gameManager));
            
            for (int i = 0; i < 72; i++){Stages["Village"].addActor("SarcenStone" + i, new Actor("Sarcen" + i, "SarcenStoneSmall", 2000 * Vector2.One + 2000 * new Vector2((float)Math.Cos(10 * i), (float)Math.Sin(10 * i)), Stages["Village"]));}

            Stages["Village"].addActor("Shack1", new Actor("Shack1", "Shack", new Vector2(600, 2000)));
            Stages["Village"].addActor("Shack2", new Actor("Shack2", "Shack", new Vector2(900, 2000)));
            Stages["Village"].addActor("Shack3", new Actor("Shack3", "Shack", new Vector2(1200, 2000)));
            Stages["Village"].addActor("Shack4", new Actor("Shack4", "Shack", new Vector2(300, 2000)));
            Stages["Village"].addActor("Shack5", new Actor("Shack5", "Shack", new Vector2(3700, 2000)));
            Stages["Village"].addActor("Shack6", new Actor("Shack6", "Shack", new Vector2(2800, 2000)));
            Stages["Village"].addActor("Shack7", new Actor("Shack7", "Shack", new Vector2(3100, 2000)));
            Stages["Village"].addActor("Shack8", new Actor("Shack8", "Shack", new Vector2(3400, 2000)));

            Stages["Village"].addActor("Shack11", new Actor("Shack11", "Shack", new Vector2(700, 1700)));
            Stages["Village"].addActor("Shack12", new Actor("Shack12", "Shack", new Vector2(1000, 1700)));
            Stages["Village"].addActor("Shack13", new Actor("Shack13", "Shack", new Vector2(1300, 1700)));
            Stages["Village"].addActor("Shack14", new Actor("Shack14", "Shack", new Vector2(400, 1700)));
            Stages["Village"].addActor("Shack15", new Actor("Shack15", "Shack", new Vector2(3600, 1700)));
            Stages["Village"].addActor("Shack16", new Actor("Shack16", "Shack", new Vector2(2700, 1700)));
            Stages["Village"].addActor("Shack17", new Actor("Shack17", "Shack", new Vector2(3000, 1700)));
            Stages["Village"].addActor("Shack18", new Actor("Shack18", "Shack", new Vector2(3300, 1700)));

            Stages["Village"].addActor("Shack21", new Actor("Shack21", "Shack", new Vector2(700, 2300)));
            Stages["Village"].addActor("Shack22", new Actor("Shack22", "Shack", new Vector2(1000, 2300)));
            Stages["Village"].addActor("Shack23", new Actor("Shack23", "Shack", new Vector2(1300, 2300)));
            Stages["Village"].addActor("Shack24", new Actor("Shack24", "Shack", new Vector2(400, 2300)));
            Stages["Village"].addActor("Shack25", new Actor("Shack25", "Shack", new Vector2(3600, 2300)));
            Stages["Village"].addActor("Shack26", new Actor("Shack26", "Shack", new Vector2(2700, 2300)));
            Stages["Village"].addActor("Shack27", new Actor("Shack27", "Shack", new Vector2(3000, 2300)));
            Stages["Village"].addActor("Shack28", new Actor("Shack28", "Shack", new Vector2(3300, 2300)));


            Stages["Village"].addActor("Shack31", new Actor("Shack31", "Shack", new Vector2(900, 2600)));
            Stages["Village"].addActor("Shack32", new Actor("Shack32", "Shack", new Vector2(1200, 2600)));
            Stages["Village"].addActor("Shack33", new Actor("Shack33", "Shack", new Vector2(1500, 2600)));
            Stages["Village"].addActor("Shack34", new Actor("Shack34", "Shack", new Vector2(600, 2600)));
            Stages["Village"].addActor("Shack35", new Actor("Shack35", "Shack", new Vector2(3400, 2600)));
            Stages["Village"].addActor("Shack36", new Actor("Shack36", "Shack", new Vector2(2500, 2600)));
            Stages["Village"].addActor("Shack37", new Actor("Shack37", "Shack", new Vector2(2800, 2600)));
            Stages["Village"].addActor("Shack38", new Actor("Shack38", "Shack", new Vector2(3100, 2600)));


            Stages["Village"].addActor("Shack41", new Actor("Shack41", "Shack", new Vector2(900, 1400)));
            Stages["Village"].addActor("Shack42", new Actor("Shack42", "Shack", new Vector2(1200, 1400)));
            Stages["Village"].addActor("Shack43", new Actor("Shack43", "Shack", new Vector2(1500, 1400)));
            Stages["Village"].addActor("Shack44", new Actor("Shack44", "Shack", new Vector2(600, 1400)));
            Stages["Village"].addActor("Shack45", new Actor("Shack45", "Shack", new Vector2(3400, 1400)));
            Stages["Village"].addActor("Shack46", new Actor("Shack46", "Shack", new Vector2(2500, 1400)));
            Stages["Village"].addActor("Shack47", new Actor("Shack47", "Shack", new Vector2(2800, 1400)));
            Stages["Village"].addActor("Shack48", new Actor("Shack48", "Shack", new Vector2(3100, 1400)));

            ParallelEventGroup zoomOut = new ParallelEventGroup("Introduction");
            zoomOut.AddEvent(new ScaleCameraEvent(Stages["Village"].camera, .25f, 5000f));
            zoomOut.AddEvent(new SetCameraEvent(Stages["Village"].camera, new Vector2(2000, 2000)));
            zoomOut.AddEvent(new AcknowledgePauseEvent(Stages["Village"]));
            zoomOut.AddEvent(new ChangeAmbient(Stages["Village"], new Vector3(1f, 1f, .5f), 0f, 5000f));
            Stages["Village"].AddEvent(zoomOut);
            //Stages["Village"].RunEvent("Zoom");
            //GM.player.SetAction("FightStance");
            gameManager.Player.StartBleeding();
        }




        public void Update(GameTime T)
        {
            openStage.Update(T);

        }

        public void Draw(GraphicsDevice device, SpriteBatch batch, RenderTarget2D shadeTemp)
        {   
            openStage.Draw(device, batch, shadeTemp);

        }

        public void Initialize()
        {
            
         

        }

        public void SetStage(String Next, Vector2 startingPosition)
        {
            if (openStage != null)
            {
                openStage.RemovePlayer();
                openStage.AM.StopSounds();
                openStage.AM.StopSong();
            }
            Stage nextStage = Stages[Next];
            nextStage.addPlayer(gameManager.Player, startingPosition);
          
            nextStage.setCamera(); 
            nextStage.Load(contentManager);
            nextStage.Initialize();
            nextStage.RunEvent("Introduction");
            openStage = nextStage;
        }

        public void SetStage(String nextStage)
        {
            SetStage(nextStage, new Vector2(-150,600));

        }
    }
}
