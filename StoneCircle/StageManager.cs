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

    public class StageManager : ISaveable
    {

        private Dictionary<String, Stage> stages = new Dictionary<String, Stage>();
        private Stage openStage;

        private List<String> stateConditions = new List<String>();
        public List<String> StateConditions { get { return stateConditions; } }

        internal ContentManager contentManager;

        private GameManager gameManager;

        public StageManager()
        {
        }

        public StageManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            contentManager = gameManager.ContentManager;

            stages.Add("region1", new Stage("region1", this));
            stages["region1"].addActor("Tree1", new Tree(new Vector2(0, 400), stages["region1"], gameManager));
            stages["region1"].addActor("Tree2", new Tree(new Vector2(100, 400), stages["region1"], gameManager));
            stages["region1"].addActor("Tree3", new Tree(new Vector2(200, 400), stages["region1"], gameManager));
            stages["region1"].addActor("Tree4", new Tree(new Vector2(300, 400), stages["region1"], gameManager));
            stages["region1"].addActor("Tree5", new Tree(new Vector2(400, 400), stages["region1"], gameManager));
            stages["region1"].addActor("Tree6", new Tree(new Vector2(500, 450), stages["region1"], gameManager));
            stages["region1"].addActor("Tree7", new Tree(new Vector2(600, 500), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8", new Tree(new Vector2(750, 550), stages["region1"], gameManager));


            stages["region1"].addActor("Tree1b", new Tree(new Vector2(0, 750), stages["region1"], gameManager));
            stages["region1"].addActor("Tree2b", new Tree(new Vector2(50, 950), stages["region1"], gameManager));
            stages["region1"].addActor("Tree3b", new Tree(new Vector2(200, 950), stages["region1"], gameManager));
            stages["region1"].addActor("Tree4b", new Tree(new Vector2(300, 850), stages["region1"], gameManager));
            stages["region1"].addActor("Tree5b", new Tree(new Vector2(400, 800), stages["region1"], gameManager));
            stages["region1"].addActor("Tree6b", new Tree(new Vector2(500, 850), stages["region1"], gameManager));
            stages["region1"].addActor("Tree7b", new Tree(new Vector2(600, 800), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8b", new Tree(new Vector2(750, 850), stages["region1"], gameManager));


            stages["region1"].addActor("Treea1", new Tree(new Vector2(50, 350), stages["region1"], gameManager));
            stages["region1"].addActor("Tree2a", new Tree(new Vector2(150, 350), stages["region1"], gameManager));
            stages["region1"].addActor("Tree3a", new Tree(new Vector2(250, 350), stages["region1"], gameManager));
            stages["region1"].addActor("Tree4a", new Tree(new Vector2(350, 350), stages["region1"], gameManager));
            stages["region1"].addActor("Tree5a", new Tree(new Vector2(450, 350), stages["region1"], gameManager));
            stages["region1"].addActor("Tree6a", new Tree(new Vector2(500, 400), stages["region1"], gameManager));
            stages["region1"].addActor("Tree7a", new Tree(new Vector2(600, 450), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8a", new Tree(new Vector2(750, 550), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8a2", new Tree(new Vector2(850, 550), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8q", new Tree(new Vector2(950, 550), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8r", new Tree(new Vector2(1050, 550), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8e", new Tree(new Vector2(750, 850), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8c", new Tree(new Vector2(850, 850), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8f", new Tree(new Vector2(950, 850), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8g", new Tree(new Vector2(1050, 875), stages["region1"], gameManager));
            stages["region1"].addActor("Tree8h", new Tree(new Vector2(1150, 850), stages["region1"], gameManager));

            stages["region1"].addActor("Body1", new Actor("Body1", "Body1", new Vector2(50, 500)));
            stages["region1"].addActor("Body2", new Actor("Body2", "Body1", new Vector2(180, 650)));
            stages["region1"].addActor("Body3", new Actor("Body3", "Body1", new Vector2(350, 500)));
            stages["region1"].addActor("Body4", new Actor("Body4", "Body1", new Vector2(550, 650)));

            //ItemActor Lantern = new ItemActor("fireIMG", "Lantern", gameManager);
            //Lantern.AddItem(new Lantern(Lantern));
            // Stages["region1"].AddActor(Lantern, new Vector2( 600, 650));


            stages["region1"].AddTrigger(new Trigger("Bandage", new TriggerANDCondition(new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(600, 400, 0), new Vector3(601, 1200, 1)), gameManager.Player), new TriggerActorHasProperty(gameManager.Player, "Bleeding")), true, true));
            SerialEVENTGroup Bandage = new SerialEVENTGroup("Bandage");
            Bandage.AddEVENT(new EVENTDialogueTimed("I'm losing a lot of blood... I'll need to bandage myself if I don't want to bleed to death", gameManager.Player, stages["region1"]));
            Bandage.AddEVENT(new EVENTDialogueTimed("Hold RT and press Y. Don't move until you've finished bandaging.", gameManager.Player, stages["region1"]));
            Bandage.AddEVENT(new EVENTActorAddItem(gameManager.Player, new Lantern(gameManager.Player)));
            stages["region1"].AddEVENT("Bandage", Bandage);
            stages["region1"].AddTrigger(new Trigger("region2trans", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(300, 1200, 0), new Vector3(2000, 1201, 0)), gameManager.Player), true, false));
            stages["region1"].AddEVENT("region2trans", new EVENTStageChange(this, "region2"));

            //stages["region1"].AddEVENT(new EVENTDialogueTimed("Forest1", "", "Player", "I've got to get out of here... Must... warn... the village", stages["region1"], Lines.LineType.Player));
            //stages["region1"].AddLines(new Lines("Bandage", "Instructions", "Player", "I'm losing a lot of blood... I'll need to bandage myself if I don't want to bleed to death", stages["region1"], Lines.LineType.Player));
            // stages["region1"].AddLines(new Lines("Instructions", "", "Player", "Hold RT and press Y. Don't move until you've finished bandaging.", stages["region1"], Lines.LineType.Player));
            // stages["region1"].AddLines(new Lines("Village", "", "Player", "If I follow this stream to the South I should come across the village of SouthStreamVillage", stages["region1"], Lines.LineType.Player));

            // Stages["region1"].addActor("Fire1", new Fire(new Vector2(-100, 900), Stages["region1"], gameManager));
            stages["region1"].addLight("Light", new Vector2(-100, 600), 1200);


            SerialEVENTGroup testIntroduction = new SerialEVENTGroup("Introduction");
            ParallelEVENTGroup testPar1 = new ParallelEVENTGroup();
            testPar1.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(150, 600), stages["region1"]));
            testPar1.AddEVENT(new EVENTSetCameraLocation(stages["region1"].camera, new Vector2(684, 600)));
            testPar1.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            testIntroduction.AddEVENT(testPar1);
            testIntroduction.AddEVENT(new EVENTDialogueTimed("I've got to get out of here... Must... warn... the village", gameManager.Player, stages["region1"]));
            ParallelEVENTGroup testPar2 = new ParallelEVENTGroup();
            testPar1.AddEVENT(new EVENTCameraDeactivate(stages["region1"].camera));
            testPar2.AddEVENT(new EVENTMoveCamera(stages["region1"].camera, new Vector2(900, 600), 2000f));
            testPar2.AddEVENT(new EVENTScaleCamera(stages["region1"].camera, .8f, 2000f));
            testIntroduction.AddEVENT(testPar2);
            testIntroduction.AddEVENT(new EVENTAcknowledgePause(stages["region1"]));
            testIntroduction.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            // testPar2.AddEVENT(new PerformActionEVENT(gameManager.Player, "Resting"));
            testIntroduction.AddEVENT(new EVENTCameraReactivate(stages["region1"].camera));
            gameManager.Camera.setSubject(gameManager.Player);

            stages["region1"].AddEVENT(testIntroduction);
            // Stages["region1"].AddEVENT(new ParallelEVENTGroup("Intro2", ""));


            stages.Add("region2", new Stage("Region2", this));
            // Stages["region2"].addLight(new ActorLightSource(gameManager.Player, 1200f));
            stages["region2"].AddEVENT("Introduction", new EVENT());
            stages["region2"].AddTrigger(new Trigger("VillageTrans", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(300, 300, 0), new Vector3(2000, 1201, 0)), gameManager.Player), true, false));
            stages["region2"].AddEVENT("VillageTrans", new EVENTStageChange(this, "Village"));

            stages.Add("Village", new Stage("Village", this));
            stages.Add("Cairn", new Stage("Cairn", this));

            stages["Village"].AMBStrength = 0f;
            Actor CenterStone = new Actor("CenterStone", "SarcenStone2", new Vector2(2000, 2000), stages["Village"]);
            stages["Village"].addActor("CenterStone", new Actor("CenterStone", "SarcenStone2", new Vector2(2000, 2000), stages["Village"]));

            stages["Village"].addLight(new LightSource("SarcenGlow", new Vector2(950, 600), 500f, stages["Village"], null));
            stages["Village"].AMBColor = new Vector3(.5f, .5f, 1f);
            Follower annyoingGuy = new Follower("Follower", new Vector2(1400, 600), stages["Village"], gameManager);
            stages["Village"].AddTrigger(new Trigger("DeadGuyItem", new TriggerANDCondition(new TriggerPlayerInteracting(annyoingGuy), new TriggerActorHasProperty(annyoingGuy, "Dead")), true, true));
            stages["Village"].AddTrigger(new Trigger("NotDeadGuyItem", new TriggerANDCondition(new TriggerPlayerInteracting(annyoingGuy), new TriggerActorHasNotProperty(annyoingGuy, "Dead")), true, false));


            ParallelEVENTGroup LootBody = new ParallelEVENTGroup("DeadGuyItem");
            LootBody.AddEVENT(new EVENTActorAddItem(gameManager.Player, new Item("SeveredHand", "ThumbsUpIcon")));
            LootBody.AddEVENT(new EVENTDialogueTimed("Sweet! A Severed hand!", gameManager.Player, stages["Village"]));
            SerialEVENTGroup AnnoyingGuyDialogue = new SerialEVENTGroup("NotDeadGuyItem");

            //   AnnoyingGuyDialogue.AddEVENT(new EVENTDialogue("Why are you following me?", gameManager.Player));
            //  AnnoyingGuyDialogue.AddEVENT(new EVENTDialogue("BoringConversation2", stages["Village"]));
            AnnoyingGuyDialogue.AddEVENT(new EVENTActorAddProperty(annyoingGuy, "Dead"));

            stages["Village"].AddActor(annyoingGuy, new Vector2(1400, 600));

            stages["Village"].AddEVENT(LootBody);
            stages["Village"].AddEVENT(AnnoyingGuyDialogue);
            //  stages["Village"].AddLines(new Lines("BoringConversation", "", "Player", "Why are you following me?", stages["Village"], Lines.LineType.Player));

            for (int i = 0; i < 72; i++) { stages["Village"].addActor("SarcenStone" + i, new Actor("Sarcen" + i, "SarcenStoneSmall", 2000 * Vector2.One + 2000 * new Vector2((float)Math.Cos(10 * i), (float)Math.Sin(10 * i)), stages["Village"])); }

            stages["Village"].addActor("Shack1", new Actor("Shack1", "Shack", new Vector2(600, 2000)));
            stages["Village"].addActor("Shack2", new Actor("Shack2", "Shack", new Vector2(900, 2000)));
            stages["Village"].addActor("Shack3", new Actor("Shack3", "Shack", new Vector2(1200, 2000)));
            stages["Village"].addActor("Shack4", new Actor("Shack4", "Shack", new Vector2(300, 2000)));
            stages["Village"].addActor("Shack5", new Actor("Shack5", "Shack", new Vector2(3700, 2000)));
            stages["Village"].addActor("Shack6", new Actor("Shack6", "Shack", new Vector2(2800, 2000)));
            stages["Village"].addActor("Shack7", new Actor("Shack7", "Shack", new Vector2(3100, 2000)));
            stages["Village"].addActor("Shack8", new Actor("Shack8", "Shack", new Vector2(3400, 2000)));

            stages["Village"].addActor("Shack11", new Actor("Shack11", "Shack", new Vector2(700, 1700)));
            stages["Village"].addActor("Shack12", new Actor("Shack12", "Shack", new Vector2(1000, 1700)));
            stages["Village"].addActor("Shack13", new Actor("Shack13", "Shack", new Vector2(1300, 1700)));
            stages["Village"].addActor("Shack14", new Actor("Shack14", "Shack", new Vector2(400, 1700)));
            stages["Village"].addActor("Shack15", new Actor("Shack15", "Shack", new Vector2(3600, 1700)));
            stages["Village"].addActor("Shack16", new Actor("Shack16", "Shack", new Vector2(2700, 1700)));
            stages["Village"].addActor("Shack17", new Actor("Shack17", "Shack", new Vector2(3000, 1700)));
            stages["Village"].addActor("Shack18", new Actor("Shack18", "Shack", new Vector2(3300, 1700)));

            stages["Village"].addActor("Shack21", new Actor("Shack21", "Shack", new Vector2(700, 2300)));
            stages["Village"].addActor("Shack22", new Actor("Shack22", "Shack", new Vector2(1000, 2300)));
            stages["Village"].addActor("Shack23", new Actor("Shack23", "Shack", new Vector2(1300, 2300)));
            stages["Village"].addActor("Shack24", new Actor("Shack24", "Shack", new Vector2(400, 2300)));
            stages["Village"].addActor("Shack25", new Actor("Shack25", "Shack", new Vector2(3600, 2300)));
            stages["Village"].addActor("Shack26", new Actor("Shack26", "Shack", new Vector2(2700, 2300)));
            stages["Village"].addActor("Shack27", new Actor("Shack27", "Shack", new Vector2(3000, 2300)));
            stages["Village"].addActor("Shack28", new Actor("Shack28", "Shack", new Vector2(3300, 2300)));


            stages["Village"].addActor("Shack31", new Actor("Shack31", "Shack", new Vector2(900, 2600)));
            stages["Village"].addActor("Shack32", new Actor("Shack32", "Shack", new Vector2(1200, 2600)));
            stages["Village"].addActor("Shack33", new Actor("Shack33", "Shack", new Vector2(1500, 2600)));
            stages["Village"].addActor("Shack34", new Actor("Shack34", "Shack", new Vector2(600, 2600)));
            stages["Village"].addActor("Shack35", new Actor("Shack35", "Shack", new Vector2(3400, 2600)));
            stages["Village"].addActor("Shack36", new Actor("Shack36", "Shack", new Vector2(2500, 2600)));
            stages["Village"].addActor("Shack37", new Actor("Shack37", "Shack", new Vector2(2800, 2600)));
            stages["Village"].addActor("Shack38", new Actor("Shack38", "Shack", new Vector2(3100, 2600)));


            stages["Village"].addActor("Shack41", new Actor("Shack41", "Shack", new Vector2(900, 1400)));
            stages["Village"].addActor("Shack42", new Actor("Shack42", "Shack", new Vector2(1200, 1400)));
            stages["Village"].addActor("Shack43", new Actor("Shack43", "Shack", new Vector2(1500, 1400)));
            stages["Village"].addActor("Shack44", new Actor("Shack44", "Shack", new Vector2(600, 1400)));
            stages["Village"].addActor("Shack45", new Actor("Shack45", "Shack", new Vector2(3400, 1400)));
            stages["Village"].addActor("Shack46", new Actor("Shack46", "Shack", new Vector2(2500, 1400)));
            stages["Village"].addActor("Shack47", new Actor("Shack47", "Shack", new Vector2(2800, 1400)));
            stages["Village"].addActor("Shack48", new Actor("Shack48", "Shack", new Vector2(3100, 1400)));
            ParallelEVENTGroup VillageIntro = new ParallelEVENTGroup("Introduction");
            SerialEVENTGroup zoomOut = new SerialEVENTGroup("NotIntroduction");
            zoomOut.AddEVENT(new EVENTSetCameraLocation(stages["Village"].camera, new Vector2(2000, 2000)));
            zoomOut.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            zoomOut.AddEVENT(new EVENTScaleCamera(stages["Village"].camera, .25f, 5000f));
            zoomOut.AddEVENT(new EVENTAcknowledgePause(stages["Village"]));
            zoomOut.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            zoomOut.AddEVENT(new EVENTCameraReactivate(stages["Village"].camera));
            VillageIntro.AddEVENT(zoomOut);
            VillageIntro.AddEVENT(new EVENTChangeAmbient(stages["Village"], new Vector3(1f, 1f, .5f), .5f, 15000f));
            VillageIntro.AddEVENT(new EVENTActorAddItem(gameManager.Player, new Sword(gameManager.Player)));
            stages["Village"].AddEVENT(VillageIntro);
            //Stages["Village"].RunEVENT("Zoom");
            //GM.player.SetAction("FightStance");
            gameManager.Player.StartBleeding();

            stateConditions.Add("Condition1");
            stateConditions.Add("Condition5");
        }

        /// <summary>
        /// Set a condition, so future calls to CheckCondition with the same name will
        /// return true.
        /// </summary>
        /// <param name="condition">Name of the condition to set</param>
        internal void SetCondition(String condition)
        {
            stateConditions.Add(condition);

            /* TODO: notifyTriggers */
        }

        /// <summary>
        /// Unsets a condition, so future calls to CheckCondition with the same name will
        /// return false.
        /// </summary>
        /// <param name="condition">Name of the condition to unset</param>
        internal void UnsetCondition(String condition)
        {
            stateConditions.Remove(condition);

            /* TODO: notifyTriggers */
        }

        /// <summary>
        /// Checks whether a given condition is currently set.
        /// </summary>
        /// <param name="condition">Name of the condition to check</param>
        /// <returns>True if the condition is set; False otherwise</returns>
        /// <see cref="SetCondition"/>
        /// <see cref="UnsetCondition"/>
        internal bool CheckCondition(String condition)
        {
            return stateConditions.Contains(condition);
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

            Stage nextStage = stages[Next];
            nextStage.addPlayer(gameManager.Player, startingPosition);

            nextStage.setCamera();
            nextStage.Load(contentManager);
            nextStage.Initialize();
            nextStage.RunEvent("Introduction");
            openStage = nextStage;
        }

        public void SetStage(String nextStage)
        {
            SetStage(nextStage, new Vector2(-150, 600));
        }


        //        private Dictionary<String, Stage> stages = new Dictionary<String, Stage>();
        // private Stage openStage;

        // private List<String> stateConditions = new List<String>();

        public void FullSave(BinaryWriter writer)
        {
            
            // stow openStage
            if (openStage != null && stages.ContainsValue(openStage))
            {
                writer.Write(openStage.Id);
            }
            else
            {
                writer.Write("");
            }
            // stow stateConditions
            SaveHelper.Save(stateConditions, writer);
        }

        public void IncrementalSave(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Reset(BinaryReader fullSave, BinaryReader incrementalSave)
        {
            String openStageId = fullSave.ReadString();
            stateConditions = SaveHelper.LoadStringList(fullSave);
        }
    }
}
