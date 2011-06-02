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
using StoneCircle.Persistence;

namespace StoneCircle
{
    /// <summary>
    /// Manages a stage
    /// </summary>

    public class StageManager : ISaveable
    {
        private uint id;

        private Dictionary<String, Stage> stages = new Dictionary<String, Stage>();
        private Dictionary<String, Actor> mainCharacters = new Dictionary<String, Actor>();
        internal Dictionary<String, Actor> MainCharacters { get { return mainCharacters; } }
       
        
        private Stage openStage;
        internal Stage OpenStage { get { return openStage; } }

        private List<String> stateConditions = new List<String>();
        public List<String> StateConditions { get { return stateConditions; } }


        Menu stageTransitionMenu;

        internal ContentManager contentManager;

        internal Texture2D grassTexture;

        private GameManager gameManager;

        public StageManager(uint id)
        {
            this.id = id;
            IdFactory.MoveNextIdPast(id);
        }

        public StageManager(GameManager gameManager)
        {
            this.id = IdFactory.GetNextId();
            this.gameManager = gameManager;
            contentManager = gameManager.ContentManager;

            stageTransitionMenu = new RingMenu(gameManager);
            stageTransitionMenu.AddMenuItem(new ChangeLevelItem("KingsHall", new Vector2(2000, 4000), this, gameManager.UIManager, "Main House"));
            stageTransitionMenu.AddMenuItem(new ChangeLevelItem("Cairn", new Vector2(1000, 1000), this, gameManager.UIManager, "Cairn of Dead Kings"));
            stageTransitionMenu.AddMenuItem(new ChangeLevelItem("Village", new Vector2(500, 500), this, gameManager.UIManager, "Small Village"));



            Character Rhett = new Character("Rhett", "RhettSheet", new Vector2(800, 800), gameManager);
            mainCharacters.Add("Rhett", Rhett);



            // This is the dialogue and set up for a people's Village trial regarding a matter of cattle 
            // ( which represents the socio-economic status of the people involved).
            // 

            Stage Village = new Stage("Village", this);
            stages.Add("Village", Village);
            stages["Village"].AMBStrength = -.1f;


            
            Random random = new Random(55555);
            int boxType, deviation, angle, houseX, houseY;
            for(int i = 0; i< 10; i++)
            {for (int j = 0; j < 19; j++)
            {
                boxType = random.Next(100);
                if ((i == 4 && j < 7) || ( i ==5 && j >3 )||( j-i ==6 && i >2))  { }
                else if (boxType < 1)
                {
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500* new Vector2(i,j) + new Vector2(100, 300)));
           


                } else if(boxType< 2) {
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500* new Vector2(i,j) + new Vector2(300, 100)));
           


                } else if (boxType < 3)
                {
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500* new Vector2(i,j) + new Vector2(300, 300)));
           


                } else  if (boxType < 20)
                {
                    houseX = random.Next(200, 300);
                    houseY = random.Next(200, 300);
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500 * new Vector2(i, j) + new Vector2(houseX, houseY)));
                    deviation = random.Next(150, 200);
                    angle = random.Next(0, 180);
                    stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500 * new Vector2(i, j) + new Vector2(houseX, houseY) + deviation * new Vector2(-(float)Math.Cos(MathHelper.ToRadians(angle)), (float)-Math.Sin(MathHelper.ToRadians(angle))), stages["Village"], gameManager));
                    deviation = random.Next(150, 200);
                    angle = random.Next(0, 180);
                    stages["Village"].addActor("Tree" + i + "_" + j + "b", new Tree(500 * new Vector2(i, j) + new Vector2(houseX, houseY) + deviation * new Vector2(-(float)Math.Cos(MathHelper.ToRadians(angle)), (float)Math.Sin(MathHelper.ToRadians(angle))), stages["Village"], gameManager));
           


                } else  if (boxType < 40)
                {

                    houseX = random.Next(300, 400);
                    houseY = random.Next(300, 400);
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500 * new Vector2(i, j) + new Vector2(houseX, houseY)));
                    deviation = random.Next(150, 200);
                    angle = random.Next(0, 90);
                    stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500 * new Vector2(i, j) + new Vector2(houseX, houseY) + deviation * new Vector2(-(float)Math.Cos(MathHelper.ToRadians(angle)), (float)-Math.Sin(MathHelper.ToRadians(angle))), stages["Village"], gameManager));
           


                } else  if (boxType < 60)
                {
                    houseX = random.Next(300, 400);
                    houseY = random.Next(100, 200);
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500 * new Vector2(i, j) + new Vector2(houseX, houseY)));
                    deviation = random.Next(150, 200);
                    angle = random.Next(0, 75);
                    stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500 * new Vector2(i, j) + new Vector2(houseX, houseY) + deviation * new Vector2(-(float)Math.Cos(MathHelper.ToRadians(angle)), (float)Math.Sin(MathHelper.ToRadians(angle))), stages["Village"], gameManager));
           


                } else  if (boxType < 70)
                {
                    houseX = random.Next(100, 200);
                    houseY = random.Next(100, 200);
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500 * new Vector2(i, j) + new Vector2(houseX, houseY)));
                    deviation = random.Next(150, 200);
                    angle = random.Next(0, 75);
                    stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500 * new Vector2(i, j) + new Vector2(houseX, houseY) + deviation * new Vector2((float)Math.Cos(MathHelper.ToRadians(angle)), (float)Math.Sin(MathHelper.ToRadians(angle))), stages["Village"], gameManager));
           

                } else if (boxType < 100)
                {
                    houseX = random.Next(100, 200);
                    houseY = random.Next(300, 400);
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500* new Vector2(i,j) + new Vector2(houseX, houseY)));
                    deviation = random.Next(150, 200);
                    angle = random.Next(0, 90);
                    
                    
                    stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500* new Vector2(i,j) + new Vector2(houseX, houseY) + deviation * new Vector2 ((float)Math.Cos(MathHelper.ToRadians(angle)),-(float) Math.Sin(MathHelper.ToRadians(angle))), stages["Village"], gameManager));
           


                }  else  if (boxType < 90)
                {
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500* new Vector2(i,j) + new Vector2(100, 300)));
                    stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500* new Vector2(i,j) + new Vector2( 250, 350), stages["Village"], gameManager));
                    


                }  else 
                {
                    stages["Village"].addActor("House" + i + "_" + j, new Shack(gameManager, 500* new Vector2(i,j) + new Vector2(100, 300)));
                     stages["Village"].addActor("Tree" + i + "_" + j, new Tree(500* new Vector2(i,j) + new Vector2( 150, 150), stages["Village"], gameManager));
           


                }
                

            }
          
            }
            for (int i = 0; i < 30; i++)
            {

                Village.AddActor(new Character("VillagerM_" + i, "VillagerM", Vector2.Zero, gameManager), new Vector2(random.Next(200, 4000), random.Next(200, 8000)));
                Village.AddActor(new Character("VillagerF_" + i, "VillagerF", Vector2.Zero, gameManager), new Vector2(random.Next(200, 4000), random.Next(200, 8000)));

            }


            //   stages["Village"].AddTrigger(new Trigger("LevelChange", new TriggerANDCondition(new TriggerNOTCondition( new TriggerPlayerBoxCondition(new CollisionBox(new Vector3( 1000,1000, 0), 2000f, 5000f, 2000f), gameManager.Player)), new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDelivered"))), true, true));
            //   stages["Village"].AddEVENT("LevelChange", new EVENTOpenMenu(stageTransitionMenu, gameManager.UIManager));

            ParallelEVENTGroup VillageOpen = new ParallelEVENTGroup("LevelOpen");
            SerialEVENTGroup VillageIntro = new SerialEVENTGroup("NewGame");
            VillageOpen.AddEVENT(new EVENTStateConditionONEVENT("New Game", VillageIntro, this));

            Character VillagerA = new Character("VillagerA", "VillagerM", new Vector2(800, 950), gameManager);
            Character villagerB = new Character("villagerB", "VillagerM", new Vector2(1100, 1000), gameManager);
            Character Aide = new Character("Aide", "VillagerM", new Vector2(800, 300), gameManager);
            //Village.AddActor(Rhett, new Vector2(2570, 3900));
            Village.AddActor(Aide, new Vector2(2870, 2777));
            Village.AddActor(new SetProp("CenterStone", "SarcenStone2", Vector2.Zero, gameManager), new Vector2(2544, 2831));
            Village.AddActor(VillagerA, new Vector2(2406, 3188));

            Village.AddActor(villagerB, new Vector2(2695, 3206));

            //Aide asks if a decision has been made.
            VillageIntro.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));

            ParallelEVENTGroup AVI1= new ParallelEVENTGroup("VI1");

            SerialEVENTGroup SVI1 = new SerialEVENTGroup("SVI1");
            AVI1.AddEVENT(new EVENTActorEnterStage(Village, "Rhett", this, new Vector2(2800, 7660)));
            AVI1.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2570, 3300), stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("Tell me, Eimar, why are we here again?", Rhett, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("I'm here to settle the cattle dispute between Donal and Marik. You're here because...", gameManager.Player, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("Why are you here?", gameManager.Player, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("I'm here because until my father retires or passes on, I have no cattle of my own to tend.", Rhett, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("' Go out and make a name for yourself Boy. ", Rhett, stages["Village"]));
            
            SVI1.AddEVENT(new EVENTDialogueConfirmed("You misunderstand. Why are you, and by association, me, bothering with this?", Rhett, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("It's petty. Just some pissing contest between a man with a small herd and a man with a smaller herd.", Rhett, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("It's a waste of time. No matter who you give the cow to, they'll be back in a few months fighting over grazing rights.", Rhett, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("Donal's got nearly 8 dozen head.", gameManager.Player, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("Rhett, the King asked me to. I can't turn him down. I'll have to do this someday anyways.", gameManager.Player, stages["Village"]));
            SVI1.AddEVENT(new EVENTDialogueConfirmed("Fine. Let's just get this over with.", Rhett, stages["Village"]));
          //
            AVI1.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(2700, 3300), stages["Village"]));
            AVI1.AddEVENT(SVI1);
            VillageIntro.AddEVENT(AVI1);
            VillageIntro.AddEVENT(new EVENTDialogueConfirmed("Milord, talk to me when you've made your decision.", Aide, stages["Village"]));
            VillageIntro.AddEVENT(new EVENTDialogueConfirmed("All right...", gameManager.Player, stages["Village"]));
            VillageIntro.AddEVENT(new EVENTDialogueConfirmed("Let's do this.", gameManager.Player, stages["Village"]));
            VillageIntro.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            

            Village.AddEVENT(VillageIntro);
            //A Verdict is decided
            stages["Village"].AddTrigger(new Trigger("toVerdict", new TriggerANDCondition(new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDecided")), new TriggerPlayerInteracting(stages["Village"].GetActor("Aide"))), true, false));
            SerialEVENTGroup TalkToAide = new SerialEVENTGroup("toVerdict");
            TalkToAide.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            TalkToAide.AddEVENT(new EVENTDialogueConfirmed("Have you come to a decision milord?", Aide, stages["Village"]));
            RingMenu toVerdict = new RingMenu(gameManager);
            toVerdict.AddMenuItem(new EventItem(stages["Village"], "Verdict", "ThumbsUpIcon", "Yes, I have", gameManager.UIManager));
            toVerdict.AddMenuItem(new CloseMenuButton(gameManager.UIManager));
            TalkToAide.AddEVENT(new EVENTOpenMenu(toVerdict, gameManager.UIManager));

            Village.AddEVENT(TalkToAide);

            SerialEVENTGroup QuestionDonal = new SerialEVENTGroup("QuestionDonal");
            Village.AddTrigger(new Trigger("QuestionDonal", new TriggerANDCondition(new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDecided")), new TriggerPlayerInteracting(stages["Village"].GetActor("VillagerA"))), true, false));
            QuestionDonal.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));

            QuestionDonal.AddEVENT(new EVENTDialogueConfirmed("What is your side of the story?", gameManager.Player, stages["Village"]));
            QuestionDonal.AddEVENT(new EVENTDialogueConfirmed("One of my mare's is pregnant and Marek is claiming that the calf belongs to him!", VillagerA, stages["Village"]));
           QuestionDonal.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            Village.AddEVENT(QuestionDonal);


            SerialEVENTGroup QuestionMarek = new SerialEVENTGroup("QuestionMarek");
            Village.AddTrigger(new Trigger("QuestionMarek", new TriggerANDCondition(new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDecided")), new TriggerPlayerInteracting(stages["Village"].GetActor("villagerB"))), true, false));
           QuestionMarek.AddEVENT(new EVENTDialogueConfirmed("Donal's mare entered into my herd, where she was serviced by my bull.", villagerB, stages["Village"]));
            QuestionMarek.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            Village.AddEVENT(QuestionMarek);

            Village.AddTrigger(new Trigger("HurryUp", new TriggerANDCondition(new TriggerPlayerInteracting(Rhett), new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDelivered"))), true, true));
            Village.AddEVENT("HurryUp", new EVENTDialogueTimed("I can't believe you're actually listening to this case.", Rhett, stages["Village"], 5000f));


            Character positiveVilalger = new Character("PositiveVillager", "VillagerM", new Vector2(800, 950), gameManager);
            Village.AddActor(positiveVilalger, new Vector2(1600, 550));
            SerialEVENTGroup PositiveVillager = new SerialEVENTGroup("PositiveVillager");
            Village.AddTrigger(new Trigger("PositiveVillager", new TriggerANDCondition(new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDecided")), new TriggerPlayerInteracting(stages["Village"].GetActor("PositiveVillager"))), true, false));
            PositiveVillager.AddEVENT(new EVENTDialogueTimed("Villager A should have been watch'n over 'is mare better.", positiveVilalger, Village));
            Village.AddEVENT(PositiveVillager);

            Character Lumpy = new Character("LumpyD", "VillagerM", new Vector2(800, 950), gameManager);
            Village.AddActor(Lumpy, new Vector2(630, 1550));
            SerialEVENTGroup LumpyD = new SerialEVENTGroup("LumpyD");
            Village.AddTrigger(new Trigger("LumpyD", new TriggerANDCondition(new TriggerNOTCondition(new TriggerStateCondition(this, "VerdictDecided")), new TriggerPlayerInteracting(stages["Village"].GetActor("LumpyD"))), true, false));
            LumpyD.AddEVENT(new EVENTDialogueTimed("Marek's already got near a hunnerd head, what's he need one more fer?", Lumpy, Village));
            Village.AddEVENT(LumpyD);



            SerialEVENTGroup Verdict = new SerialEVENTGroup("Verdict");
            Verdict.AddEVENT(new EVENTDialogueConfirmed("I have come to a decision.", gameManager.Player, Village));
            Verdict.AddEVENT(new EVENTDialogueConfirmed("And as the sole representative of Rymar-King, the Ambrosian, you agree to my judgement?", gameManager.Player, Village));
            ParallelEVENTGroup KingsHallrsSayYes = new ParallelEVENTGroup();
            KingsHallrsSayYes.AddEVENT(new EVENTDialogueConfirmed("Aye", VillagerA, Village));
            KingsHallrsSayYes.AddEVENT(new EVENTDialogueConfirmed("Yes ser.", villagerB, Village));
            Verdict.AddEVENT(KingsHallrsSayYes);
            Verdict.AddEVENT(new EVENTDialogueConfirmed("Very well then, here is my decision:", gameManager.Player, Village));

            Village.AddEVENT(Verdict);
            RingMenu VerdictMenu = new RingMenu(gameManager);
            VerdictMenu.AddMenuItem(new EventItem(Village, "VillagerA", "BlankIcon", "Give it to Marek.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Village, "VillagerB", "BlankIcon", "Give it to Donal.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Village, "BothKingsHallrs", "BlankIcon", "Share the calf between the two.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Village, "NeitherKingsHallr", "BlankIcon", "It belongs to neither of you.", gameManager.UIManager));
            Verdict.AddEVENT(new EVENTOpenMenu(VerdictMenu, gameManager.UIManager));

            SerialEVENTGroup VillageEnding = new SerialEVENTGroup("VillageEnding");
            ParallelEVENTGroup CE1 = new ParallelEVENTGroup();
            CE1.AddEVENT(new EVENTDialogueTimed("All right everyone, shows over.", gameManager.Player, Village));
           CE1.AddEVENT(new EVENTMoveActorToPlayer(Rhett, new Vector2(800, 500), Village));
            CE1.AddEVENT(new EVENTMoveActor(VillagerA, new Vector2(1600, 2300), Village)); CE1.AddEVENT(new EVENTMoveActor(villagerB, new Vector2(3000, 5300), Village));
                      
            VillageEnding.AddEVENT(CE1);
            ParallelEVENTGroup CE2 = new ParallelEVENTGroup();
           CE2.AddEVENT(new EVENTActorExitStage(Village, "Aide"));            
           CE2.AddEVENT(new EVENTActorExitStage(Village, "VillagerA"));            
           CE2.AddEVENT(new EVENTActorExitStage(Village, "villagerB"));
           VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("That should stop 'em from bothering you with the small stuff, your mightyfullness.", Rhett, Village), this));
           VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("Don't call me that, Rhett. Besides, they're supposed to bring this stuff to me.", gameManager.Player, Village), this));
           VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("You remember last spring with Mikkel and Jonah. Do you want a repeat performance?", gameManager.Player, Village), this));
           VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("Are you kidding? That was great! Jonah couldn't walk straight for a month!", Rhett, Village), this));
           VillageEnding.AddEVENT(new EVENTDialogueConfirmed("Anyway, now that you're done with this, what say we start tomorrow's festivities a bit early?", Rhett, Village));
           VillageEnding.AddEVENT(new EVENTDialogueConfirmed("I could drink to that.", gameManager.Player, Village));
           VillageEnding.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            RingMenu ExploreMenu = new RingMenu(gameManager);
            ExploreMenu.AddMenuItem(new EventItem(Village, "LeaveVillageWithRhett", true, gameManager.UIManager));
            ExploreMenu.AddMenuItem(new EventItem(Village, "RhettMovesToVillageEntrance", false, gameManager.UIManager));

            SerialEVENTGroup RhettMTVE = new SerialEVENTGroup("RhettMovesToVillageEntrance");
            RhettMTVE.AddEVENT(new EVENTDialogueConfirmed("I need too speak to some people around here first", gameManager.Player, Village));
            RhettMTVE.AddEVENT(new EVENTDialogueConfirmed("Alright, meet me at the village entrance then, I'll be waiting there.", Rhett, Village));
            RhettMTVE.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            RhettMTVE.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2800, 7400), Village));


           

            ParallelEVENTGroup PostCourtLeaveVillage = new ParallelEVENTGroup("LeaveVillageWithRhett");
            SerialEVENTGroup FadeOuteVillage = new SerialEVENTGroup("FadeOut");
            PostCourtLeaveVillage.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            PostCourtLeaveVillage.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2900, 8000), Village));
            PostCourtLeaveVillage.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(2800, 8000), Village));
            FadeOuteVillage.AddEVENT(new EVENTCameraDeactivate(Village));
          FadeOuteVillage.AddEVENT(new EVENTChangeAmbient(Village, new Vector3(1.0f, 1.0f, .4f), .8f, 4000f));
          FadeOuteVillage.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
          FadeOuteVillage.AddEVENT(new EVENTStageChange(this, "KingsHall", new Vector2(2000, 6000)));
          Village.AddEVENT(PostCourtLeaveVillage);



            SerialEVENTGroup villagerA = new SerialEVENTGroup("VillagerA");
            villagerA.AddEVENT(new EVENTDialogueConfirmed("As it stands now, the calf is already in the posession of Donal,", gameManager.Player, Village));
            villagerA.AddEVENT(new EVENTDialogueConfirmed("I see no reason to take it from him.", gameManager.Player, Village));

            villagerA.AddEVENT(KingsHallrsSayYes);
            villagerA.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));
            Village.AddEVENT(villagerA);
            SerialEVENTGroup VillagerB = new SerialEVENTGroup("VillagerB");

            VillagerB.AddEVENT(new EVENTDialogueConfirmed("The calf was conceived on Marek's land, therefore it belongs to him.", gameManager.Player, Village));
            VillagerB.AddEVENT(new EVENTDialogueConfirmed("However; he will take care of the mare in the duration.", gameManager.Player, Village));


            VillagerB.AddEVENT(KingsHallrsSayYes);
            VillagerB.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));
            Village.AddEVENT("VillagerB", VillagerB);
            SerialEVENTGroup BothKingsHallrs = new SerialEVENTGroup("BothKingsHallrs");
            BothKingsHallrs.AddEVENT(new EVENTDialogueConfirmed("Either work out a way to share it by the time it's born, or I'll cut it in two and you can choose sides.", gameManager.Player, Village));
            BothKingsHallrs.AddEVENT(new EVENTStateConditionSet("NeutralDecision", this));

            BothKingsHallrs.AddEVENT(KingsHallrsSayYes);
            BothKingsHallrs.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));

            Village.AddEVENT("BothKingsHallrs", BothKingsHallrs);
            SerialEVENTGroup NeitherKingsHallrs = new SerialEVENTGroup("NeitherKingsHallrs");
            NeitherKingsHallrs.AddEVENT(new EVENTDialogueConfirmed("It seems to me that you've disproved each other's claims thoroughly.", gameManager.Player, Village));
            NeitherKingsHallrs.AddEVENT(new EVENTDialogueConfirmed("You'll both raise the calf, and when it matures, the KingsHall will feast", gameManager.Player, Village));
            NeitherKingsHallrs.AddEVENT(new EVENTStateConditionSet("NeutralDecision", this));
            NeitherKingsHallrs.AddEVENT(KingsHallrsSayYes);
            NeitherKingsHallrs.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));
            Village.AddEVENT("NeitherKingsHallr", NeitherKingsHallrs);
            Village.AddEVENT("VillageEnding", VillageEnding);

            // Character Wraith = new Character("Wraith", "DJ", new Vector2(800, 1000), gameManager);






            // This is the main tent of Maximus, Pilus Prior, of the 3rd Cohort of the _______ Legion.
            // He is first seen here ordering the Wraiths to attack the Kings of NOT-Ireland. The area
            // is mostly in shadow, keeping Maximus obscured until he's revealed later on, (With his cohort in tow)

            ////Shady Area of Bad Guys
            //Stage ShadyArea = new Stage("Shady1", this);
            //ShadyArea.AMBStrength = .8f;
            //ShadyArea.addLight(new LightSource("Light1", new Vector2(500, 1050), 800f, ShadyArea, null));
            //ShadyArea.addLight(new LightSource("Light2", new Vector2(1100, 1050), 800f, ShadyArea, null));

            //Actor Maximus = new Actor("Maximus", "knightm1", new Vector2(800, 800));




            //ShadyArea.AddActor(Maximus, new Vector2(800, 800));
            //ShadyArea.AddActor(Wraith, new Vector2(800, 1000));
            //SerialEVENTGroup ShadyIntro = new SerialEVENTGroup("Introduction");
            //ShadyArea.AddEVENT(ShadyIntro);
            //ParallelEVENTGroup PE1 = new ParallelEVENTGroup("PE1");
            //PE1.AddEVENT(new EVENTCameraDeactivate(gameManager.Camera));
            //PE1.AddEVENT(new EVENTSetCameraLocation(gameManager.Camera, new Vector2(800, 1000)));
            //ShadyIntro.AddEVENT(PE1);
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("Are these reports true?", Maximus, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("They are truth.", Wraith, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("Good, send out your Wraiths. Hit them hard and fast.", Maximus, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("I want their Kings dead and the lands in a panic.", Maximus, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("As you command.", Wraith, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("I want regular reports. I don't want any surprises catching us with our pants down.", Maximus, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("As you command.", Wraith, ShadyArea));
            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("Good. You've got work to do. Get to it.", Maximus, ShadyArea));

            //ShadyIntro.AddEVENT(new EVENTDialogueConfirmed("Dismissed.", Maximus, ShadyArea));
            //ParallelEVENTGroup PE2 = new ParallelEVENTGroup("PE2");
            //PE2.AddEVENT(new EVENTMoveActor(Wraith, new Vector2(800, 1200), Village));
            //PE2.AddEVENT(new EVENTChangeAmbient(Village, new Vector3(1.0f, 1.0f, 1.0f), 1f));
            //PE2.AddEVENT(new EVENTDialogueConfirmed("Hmmm... Knight to E5", Maximus, ShadyArea));
            //ShadyIntro.AddEVENT(PE2);
            //ShadyIntro.AddEVENT(new EVENTStageChange(this, "KingsHall", new Vector2(2000, 4050)));


            //stages.Add("ShadyArea", ShadyArea);


            // This is the house of the king, and the main feasting area for the villages. 
            // It's considered to be owned by the community. Any of the community may come in
            // and impose on the king etc. There are some smaller buildings/ areas for people.                              

            
            stages.Add("KingsHall", new Stage("KingsHall", this));
            stages["KingsHall"].AMBStrength = 1f;
            SetProp CenterStone = new SetProp("CenterStone", "SarcenStone2", new Vector2(2000, 2000), gameManager);
            stages["KingsHall"].addActor("CenterStone", new SetProp("CenterStone", "SarcenStone2", new Vector2(2000, 4000), gameManager));
            stages["KingsHall"].AddTrigger(new Trigger("CenterStone", new TriggerPlayerInteracting(CenterStone), true, false));
            stages["KingsHall"].AddEVENT("CenterStone", new EVENTDialogueTimed("This is the Lairdsstone. One day my name will be engraved here, along with the kings of old.", gameManager.Player, stages["KingsHall"], 5000f));
            for (int i = 0; i < 72; i++) { stages["KingsHall"].addActor("SarcenStone" + i, new SetProp("Sarcen" + i, "SarcenStoneSmall", 2000 * (Vector2.One + Vector2.UnitY) + 1900 * new Vector2((float)Math.Cos(10 * i), 2 * (float)Math.Sin(10 * i)), gameManager)); }

            stages["KingsHall"].addActor("Shack1", new SetProp("Shack1", "Shack", new Vector2(600, 4000), gameManager));
            stages["KingsHall"].addActor("Shack2", new SetProp("Shack2", "Shack", new Vector2(900, 4000), gameManager));
            stages["KingsHall"].addActor("Shack3", new SetProp("Shack3", "Shack", new Vector2(1200, 4000), gameManager));
            stages["KingsHall"].addActor("Shack4", new SetProp("Shack4", "Shack", new Vector2(300, 4000), gameManager));
            stages["KingsHall"].addActor("Shack5", new SetProp("Shack5", "Shack", new Vector2(3700, 4000), gameManager));
            stages["KingsHall"].addActor("Shack6", new SetProp("Shack6", "Shack", new Vector2(2800, 4000), gameManager));
            stages["KingsHall"].addActor("Shack7", new SetProp("Shack7", "Shack", new Vector2(3100, 4000), gameManager));
            stages["KingsHall"].addActor("Shack8", new SetProp("Shack8", "Shack", new Vector2(3400, 4000), gameManager));

            stages["KingsHall"].addActor("Shack11", new SetProp("Shack11", "Shack", new Vector2(700, 3400), gameManager));
            stages["KingsHall"].addActor("Shack12", new SetProp("Shack12", "Shack", new Vector2(1000, 3400), gameManager));
            stages["KingsHall"].addActor("Shack13", new SetProp("Shack13", "Shack", new Vector2(1300, 3400), gameManager));
            stages["KingsHall"].addActor("Shack14", new SetProp("Shack14", "Shack", new Vector2(400, 3400), gameManager));
            stages["KingsHall"].addActor("Shack15", new SetProp("Shack15", "Shack", new Vector2(3600, 3400), gameManager));
            stages["KingsHall"].addActor("Shack16", new SetProp("Shack16", "Shack", new Vector2(2700, 3400), gameManager));
            stages["KingsHall"].addActor("Shack17", new SetProp("Shack17", "Shack", new Vector2(3000, 3400), gameManager));
            stages["KingsHall"].addActor("Shack18", new SetProp("Shack18", "Shack", new Vector2(3300, 3400), gameManager));

            stages["KingsHall"].addActor("Shack21", new SetProp("Shack21", "Shack", new Vector2(700, 4600), gameManager));
            stages["KingsHall"].addActor("Shack22", new SetProp("Shack22", "Shack", new Vector2(1000, 4600), gameManager));
            stages["KingsHall"].addActor("Shack23", new SetProp("Shack23", "Shack", new Vector2(1300, 4600), gameManager));
            stages["KingsHall"].addActor("Shack24", new SetProp("Shack24", "Shack", new Vector2(400, 4600), gameManager));
            stages["KingsHall"].addActor("Shack25", new SetProp("Shack25", "Shack", new Vector2(3600, 4600), gameManager));
            stages["KingsHall"].addActor("Shack26", new SetProp("Shack26", "Shack", new Vector2(2700, 4600), gameManager));
            stages["KingsHall"].addActor("Shack27", new SetProp("Shack27", "Shack", new Vector2(3000, 4600), gameManager));
            stages["KingsHall"].addActor("Shack28", new SetProp("Shack28", "Shack", new Vector2(3300, 4600), gameManager));


            stages["KingsHall"].addActor("Shack31", new SetProp("Shack31", "Shack", new Vector2(900, 5200), gameManager));
            stages["KingsHall"].addActor("Shack32", new SetProp("Shack32", "Shack", new Vector2(1200, 5200), gameManager));
            stages["KingsHall"].addActor("Shack33", new SetProp("Shack33", "Shack", new Vector2(1500, 5200), gameManager));
            stages["KingsHall"].addActor("Shack34", new SetProp("Shack34", "Shack", new Vector2(600, 5200), gameManager));
            stages["KingsHall"].addActor("Shack35", new SetProp("Shack35", "Shack", new Vector2(3400, 5200), gameManager));
            stages["KingsHall"].addActor("Shack36", new SetProp("Shack36", "Shack", new Vector2(2500, 5200), gameManager));
            stages["KingsHall"].addActor("Shack37", new SetProp("Shack37", "Shack", new Vector2(2800, 5200), gameManager));
            stages["KingsHall"].addActor("Shack38", new SetProp("Shack38", "Shack", new Vector2(3100, 5200), gameManager));


            stages["KingsHall"].addActor("Shack41", new SetProp("Shack41", "Shack", new Vector2(900, 2800), gameManager));
            stages["KingsHall"].addActor("Shack42", new SetProp("Shack42", "Shack", new Vector2(1200, 2800), gameManager));
            stages["KingsHall"].addActor("Shack43", new SetProp("Shack43", "Shack", new Vector2(1500, 2800), gameManager));
            stages["KingsHall"].addActor("Shack44", new SetProp("Shack44", "Shack", new Vector2(600, 2800), gameManager));
            stages["KingsHall"].addActor("Shack45", new SetProp("Shack45", "Shack", new Vector2(3400, 2800), gameManager));
            stages["KingsHall"].addActor("Shack46", new SetProp("Shack46", "Shack", new Vector2(2500, 2800), gameManager));
            stages["KingsHall"].addActor("Shack47", new SetProp("Shack47", "Shack", new Vector2(2800, 2800), gameManager));
            stages["KingsHall"].addActor("Shack48", new SetProp("Shack48", "Shack", new Vector2(3100, 2800), gameManager));
            stages["KingsHall"].AMBColor = new Vector3(.5f, .5f, 1f);
            //  Follower Rhett = new Follower("Follower", new Vector2(2000, 4000), stages["KingsHall"], gameManager);


            ParallelEVENTGroup MHI1 = new ParallelEVENTGroup();

            ParallelEVENTGroup MHIinit = new ParallelEVENTGroup();
            SerialEVENTGroup KingsHallIntro = new SerialEVENTGroup("Introduction");
             MHIinit.AddEVENT(new EVENTActorEnterStage(stages["KingsHall"], "Rhett", this, new Vector2(2100, 6000)));
             MHI1.AddEVENT(new EVENTActorEquipItem(stages["KingsHall"], "Rhett", this, new Lantern(Rhett)));
              MHIinit.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
              MHI1.AddEVENT(new EVENTActorEquipItem(stages["KingsHall"], "Player", this, new Lantern(gameManager.Player)));
              MHI1.AddEVENT(new EVENTActorAddItem(gameManager.Player, new Lantern(gameManager.Player)));
          
            
             MHI1.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2050, 3000), stages["KingsHall"]));
            //MHI1.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(2000, 3000), stages["KingsHall"]));
            KingsHallIntro.AddEVENT(MHIinit);
            KingsHallIntro.AddEVENT(MHI1);
            stages["KingsHall"].AddEVENT("Introduction", KingsHallIntro);
            KingsHallIntro.AddEVENT(new EVENTDialogueConfirmed("We're talking.... Oh No! The king's manor is on fire!", Rhett, stages["KingsHall"]));
            KingsHallIntro.AddEVENT(new EVENTCameraDeactivate(stages["KingsHall"]));
           // KingsHallIntro.AddEVENT(new EVENTMoveCamera(stages["KingsHall"], new Vector2(800, 2000), 1000f));
           // KingsHallIntro.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 2000), stages["KingsHall"]));
           // KingsHallIntro.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 1000), stages["KingsHall"]));

            //  stages["KingsHall"].AddTrigger(new Trigger("KingDies", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(1600, 400, 0), new Vector3(2400, 1200, 10)), gameManager.Player), true, true));

            SerialEVENTGroup KingDies = new SerialEVENTGroup("KingDies");
            ParallelEVENTGroup PD1 = new ParallelEVENTGroup();
            // PD1.AddEVENT(new EVENTMoveActor(Wraith, new Vector2(200, 2400), stages["KingsHall"]));
            SerialEVENTGroup KD1A = new SerialEVENTGroup("KD1A");
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Hunh?", gameManager.Player, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("What was that", gameManager.Player, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("What was what?", Rhett, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("I thought I some something fleeing the building.", gameManager.Player, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("You're just jumping at shadows.", Rhett, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueTimed("Sooo...", Rhett, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Looks like the old king is dead. That sucks.", Rhett, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Yea, it does. Rhett I need you to go get the Crone.", gameManager.Player, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Do I HAVE to?You know we don't exaclty see eye to eye.", Rhett, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("I need to be here, otherwise I'd go myself. We NEED her right now, much as you dislike her.", gameManager.Player, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Fine. I'll get her, but you OWE me for this.", Rhett, stages["KingsHall"]));
            KD1A.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 2000), stages["KingsHall"]));
            //KD1A.AddEVENT(new EVENTActorEquipItem(Rhett, null));
            KD1A.AddEVENT(new EVENTChangeAmbient(stages["KingsHall"], new Vector3(1.0f, 1.0f, 1.0f), 0, 10000f));

            PD1.AddEVENT(KD1A);
            Character Elysia = new Character("Elysia", "ElysiaSheet", new Vector2(2000, 3000), gameManager);
            stages["KingsHall"].AddActor(Elysia, new Vector2(3500, 2000));
            PD1.AddEVENT(new EVENTMoveActor(Elysia, new Vector2(2300, 2200), stages["KingsHall"]));
            SerialEVENTGroup KD2A = new SerialEVENTGroup("KD2A");
            KD2A.AddEVENT(new EVENTMoveCamera(stages["KingsHall"], new Vector2(2000, 2000), 3000f));
            KD2A.AddEVENT(new EVENTDramaticPause(3000f));
            KD2A.AddEVENT(new EVENTStateConditionSet("KingsDead", this));

            KingDies.AddEVENT(PD1);
            KingDies.AddEVENT(KD2A);
            stages["KingsHall"].AddEVENT(KingDies);

            // stages["KingsHall"].AddTrigger(new Trigger("MeetElysia",
            //new TriggerANDCondition(new TriggerStateCondition(this, "KingsDead"),
            //new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(1700, 1700, 0), new Vector3(2300, 2300, 10)), gameManager.Player)), true, true));
            SerialEVENTGroup MeetElysia = new SerialEVENTGroup("MeetElysia");
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Elysia? Where's the old lady?", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Hi Eimar, it's nice to see you too.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Sorry Elysia, I need to speak to the Crone, I don't have time to chat.", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueTimed("Eimar, well, the Crone wasn't able to come and sent Elysia to speak in her stead.", Rhett, stages["KingsHall"], 80f));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("I can speak for myself you lummox!", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("My master sent me to attend to the details, she'll be along soon enough.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Now if you don't have anymore questions I need to attend to the body", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Wait, before you go I have some questions.", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Fine, but be quick about it.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("In the manor I saw a strange creature.", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("It was as tall as a man, with dark fur and a gleaming, eyeless skull where it's head should rightly be.", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("With two claws as long as a boys arm at the end of it's upper limbs.", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("I'm telling you, you were jumping at shadows anda  bit too much ale.", Rhett, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("That sounds like a Wendigo, a Spirit of Shadow.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Spirits? You might use that non-sense tofrighten the young, but don't expect us to fall for it.", Rhett, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("You would be wise to just forget about it and chalk it up to bad luck and good beer.", Rhett, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("The Spirits are not to be trifled with!", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Least of all a Spirit of Shadow.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("We must be careful to keep this thing at Bay.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("How can we keep it from attacking the KingsHall?", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("We will be safe enough during the day, Wendigo cannot stand light.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("At night we'll need to light fires around the KingsHall, they fear fire.", Elysia, stages["KingsHall"]));
            //  MeetElysia.AddEVENT(new EVENTDialogueConfirmed("That seems like a lot of work.", , stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Why don't we just get a hunting party together during the day and find it then?", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("You can't just kill Spirits as you would a man!", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Mundane weapons cannot harm them, for they are not of this world.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("So why don't you just cast a spell and smite it with holy fire or something 'O Powerful One.'", Rhett, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Oh, shut up!", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("So how CAN we kill it.", gameManager.Player, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("The Sword of the King has powerful enchantments that allow it to be used against the Otherworldly.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("But only in the hands of the king.", Elysia, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Well, there you go. Problem solved.", Rhett, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Sorry to put it so bluntly, but now that Rymar-King's dead, you're the King now Eimar.", Rhett, stages["KingsHall"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Which means you can now truely wield the 'O Mighty Goblin-Whacker'", Rhett, stages["KingsHall"]));

            stages["KingsHall"].AddEVENT("MeetElysia", MeetElysia);


            // This is the "Cairn of Dead Kings". It's a cremation and funereal mound for the kings of old.
            // There is a Stonehenge esque lintel and pathway facing due West. When the king is cremated
            // at Sunset his spirit leaves his body and passes through the gate into the afterlife, joining the ancestors.
            stages.Add("Cairn of Dead Kings", new Stage("Cairn of Dead Kings", this));

            stages["Cairn of Dead Kings"].AddActor(new Character("KingOfOld1", "Ghost of Old Man", new Vector2(500, 500), gameManager), new Vector2(500, 500));
            SerialEVENTGroup King1Dialogue = new SerialEVENTGroup("King1Dialogue");
            //  King1Dialogue.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            King1Dialogue.AddEVENT(new EVENTDialogueConfirmed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld1"), stages["Cairn of Dead Kings"]));
            King1Dialogue.AddEVENT(new EVENTDialogueTimed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld1"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT(King1Dialogue);
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("King1Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("KingOfOld1")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("KingOfOld2", "Ghost of OldMan", new Vector2(500, 500), gameManager), new Vector2(400, 600));
            SerialEVENTGroup King2Dialogue = new SerialEVENTGroup("King2Dialogue");
            //  King2Dialogue.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            King2Dialogue.AddEVENT(new EVENTDialogueConfirmed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld2"), stages["Cairn of Dead Kings"]));
            King2Dialogue.AddEVENT(new EVENTDialogueTimed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld2"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT(King2Dialogue);
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("King2Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("KingOfOld2")), true, false));


            stages["Cairn of Dead Kings"].AddActor(new Character("KingOfOld3", "Ghost of VillagerM", new Vector2(2500, 500), gameManager), new Vector2(350, 800));
            SerialEVENTGroup King3Dialogue = new SerialEVENTGroup("King3Dialogue");
            //  King3Dialogue.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            King3Dialogue.AddEVENT(new EVENTDialogueConfirmed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld3"), stages["Cairn of Dead Kings"]));
            King3Dialogue.AddEVENT(new EVENTDialogueTimed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld3"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT(King3Dialogue);
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("King3Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("KingOfOld3")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("KingOfOld4", "Ghost of OldMan", new Vector2(500, 500), gameManager), new Vector2(400, 1000));
            SerialEVENTGroup King4Dialogue = new SerialEVENTGroup("King4Dialogue");
            //  King4Dialogue.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            King4Dialogue.AddEVENT(new EVENTDialogueConfirmed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld4"), stages["Cairn of Dead Kings"]));
            King4Dialogue.AddEVENT(new EVENTDialogueTimed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld4"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT(King4Dialogue);
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("King4Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("KingOfOld4")), true, false));


            stages["Cairn of Dead Kings"].AddActor(new Character("KingOfOld5", "Ghost of VillagerM", new Vector2(500, 500), gameManager), new Vector2(500, 1100));
            SerialEVENTGroup King5Dialogue = new SerialEVENTGroup("King5Dialogue");
            //  King5Dialogue.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            King5Dialogue.AddEVENT(new EVENTDialogueConfirmed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld5"), stages["Cairn of Dead Kings"]));
            King5Dialogue.AddEVENT(new EVENTDialogueTimed("You are the manifestation of the people's will. You live for their benefit.", stages["Cairn of Dead Kings"].GetActor("KingOfOld5"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT(King5Dialogue);
            King5Dialogue.AddEVENT(new EVENTActorAddProperty(gameManager.Player, "Ancestor's Blessing"));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("King5Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("KingOfOld5")), true, false));



            ParallelEVENTGroup OTInto = new ParallelEVENTGroup("Warp to OT");
            OTInto.AddEVENT(new EVENTPartialWarpActor(gameManager.Player, -2000 * Vector3.UnitY, stages["Cairn of Dead Kings"]));
            OTInto.AddEVENT(new EVENTSetCameraLocation(stages["Cairn of Dead Kings"], new Vector2(800, 400)));
            OTInto.AddEVENT(new EVENTCameraDeactivate(stages["Cairn of Dead Kings"]));

            stages["Cairn of Dead Kings"].AddEVENT(OTInto);

            stages["Cairn of Dead Kings"].AddEVENT("Warp to Top", new EVENTPartialWarpActor(gameManager.Player, -1500 * Vector3.UnitY, stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT("Warp to Bottom", new EVENTPartialWarpActor(gameManager.Player, 1500 * Vector3.UnitY, stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT("Warp to Left", new EVENTPartialWarpActor(gameManager.Player, -1500 * Vector3.UnitX, stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddEVENT("Warp to Right", new EVENTPartialWarpActor(gameManager.Player, 1500 * Vector3.UnitX, stages["Cairn of Dead Kings"]));

            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Warp to Bottom", new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(1200, 0, 0), 25f, 300f, 1600f), gameManager.Player), true, false));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Warp to Top", new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(1200, 1600, 0), 25f, 300f, 1600f), gameManager.Player), true, false));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Warp to Left", new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(1600, 800, 0), 1600f, 300f, 25f), gameManager.Player), true, false));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Warp to Right", new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(0, 800, 0), 1600f, 300f, 25f), gameManager.Player), true, false));


            stages["Cairn of Dead Kings"].AddActor(new SetProp("GatewayOTF", "StoneGateway", new Vector2(1200, 900), gameManager), new Vector2(1200, 900));
            stages["Cairn of Dead Kings"].AddActor(new SetProp("GatewayOTB", "StoneGateway", new Vector2(1200, 900), gameManager), new Vector2(1220, 700));






            stages["Cairn of Dead Kings"].AddActor(new SetProp("CairnOTF", "StoneGateway", new Vector2(1200, 2900), gameManager), new Vector2(1200, 2900));
            stages["Cairn of Dead Kings"].AddActor(new SetProp("CairnOTB", "StoneGateway", new Vector2(1200, 2900), gameManager), new Vector2(1220, 2700));

            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Warp to OT", new TriggerANDCondition(new TriggerActorHasProperty(gameManager.Player, "Open OT"), new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(1200, 2800, 0), 200f, 300f, 50f), gameManager.Player)), true, false));

            stages["Cairn of Dead Kings"].AddActor(new SetProp("Altar", "Altar", new Vector2(3200, 2900), gameManager), new Vector2(1800, 2800));
            stages["Cairn of Dead Kings"].AddActor(new SetProp("MHCT", "Manholecoverthing", new Vector2(3200, 2900), gameManager), new Vector2(2200, 2800));


            ParallelEVENTGroup WarpOut = new ParallelEVENTGroup("Warp to Cairn");
            WarpOut.AddEVENT(new EVENTPartialWarpActor(gameManager.Player, 2000 * Vector3.UnitY, stages["Cairn of Dead Kings"]));
            WarpOut.AddEVENT(new EVENTSetCameraLocation(stages["Cairn of Dead Kings"], new Vector2(800, 1400)));
            WarpOut.AddEVENT(new EVENTCameraReactivate(stages["Cairn of Dead Kings"]));
            WarpOut.AddEVENT(new EVENTActorRemoveProperty(gameManager.Player, "Open OT"));


            stages["Cairn of Dead Kings"].AddEVENT(WarpOut);
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Warp to Cairn", new TriggerANDCondition(new TriggerActorHasProperty(gameManager.Player, "Ancestor's Blessing"), new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(1200, 800, 0), 200f, 300f, 50f), gameManager.Player)), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Dead King Rymar", "OldMan", new Vector2(3200, 2900), gameManager), new Vector2(1200, 2900));



            stages["Cairn of Dead Kings"].AddActor(new Character("Villager2", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1770, 2700));
            stages["Cairn of Dead Kings"].AddEVENT("Villager2Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager2"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager2Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager2")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager1", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(2185, 2850));
            stages["Cairn of Dead Kings"].AddEVENT("Villager1Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager1"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager1Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager1")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager3", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1860, 2800));
            stages["Cairn of Dead Kings"].AddEVENT("Villager3Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager3"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager3Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager3")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager4", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1880, 2850));
            stages["Cairn of Dead Kings"].AddEVENT("Villager4Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager4"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager4Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager4")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager5", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1800, 2900));
            stages["Cairn of Dead Kings"].AddEVENT("Villager5Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager5"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager5Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager5")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager6", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1850, 2920));
            stages["Cairn of Dead Kings"].AddEVENT("Villager6Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager6"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager6Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager6")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager7", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1840, 2700));
            stages["Cairn of Dead Kings"].AddEVENT("Villager7Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager7"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager7Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager7")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager8", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1890, 2700));
            stages["Cairn of Dead Kings"].AddEVENT("Villager8Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager8"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager8Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager8")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager9", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1890, 2670));
            stages["Cairn of Dead Kings"].AddEVENT("Villager9Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager9"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager9Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager9")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager10", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1890, 2720));
            stages["Cairn of Dead Kings"].AddEVENT("Villager10Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager10"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager10Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager10")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager11", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1890, 2770));
            stages["Cairn of Dead Kings"].AddEVENT("Villager11Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager11"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager11Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager11")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager12", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1940, 2820));
            stages["Cairn of Dead Kings"].AddEVENT("Villager12Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager12"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager12Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager12")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager13", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1930, 2870));
            stages["Cairn of Dead Kings"].AddEVENT("Villager13Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager13"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager13Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager13")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager14", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1840, 2920));
            stages["Cairn of Dead Kings"].AddEVENT("Villager14Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager14"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager14Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager14")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager21", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1985, 2850));
            stages["Cairn of Dead Kings"].AddEVENT("Villager21Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager21"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager21Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager21")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager23", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1960, 2800));
            stages["Cairn of Dead Kings"].AddEVENT("Villager23Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager23"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager23Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager23")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager24", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1980, 2850));
            stages["Cairn of Dead Kings"].AddEVENT("Villager24Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager24"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager24Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager24")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager25", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1900, 2900));
            stages["Cairn of Dead Kings"].AddEVENT("Villager25Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager25"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager25Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager25")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager26", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1950, 2920));
            stages["Cairn of Dead Kings"].AddEVENT("Villager26Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager26"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager26Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager26")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager27", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1940, 2700));
            stages["Cairn of Dead Kings"].AddEVENT("Villager27Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager27"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager27Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager27")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager28", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1990, 2700));
            stages["Cairn of Dead Kings"].AddEVENT("Villager28Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager28"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager28Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager28")), true, false));

            stages["Cairn of Dead Kings"].AddActor(new Character("Villager29", "VillagerM", new Vector2(500, 500), gameManager), new Vector2(1990, 2670));
            stages["Cairn of Dead Kings"].AddEVENT("Villager29Dialogue", new EVENTDialogueTimed("Rest well, Rymar-King. We shall pass your name on throughout the ages.", stages["Cairn of Dead Kings"].GetActor("Villager29"), stages["Cairn of Dead Kings"]));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("Villager29Dialogue", new TriggerPlayerInteracting(stages["Cairn of Dead Kings"].GetActor("Villager29")), true, false));


            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("ExitCairn", new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(2200, 2800, 0), 80f, 2f, 120f), gameManager.Player), true, false));
            stages["Cairn of Dead Kings"].AddTrigger(new Trigger("EnterCairn", new TriggerPlayerBoxCondition(new CollisionBox(new Vector3(2450, 5450, 0), 120f, 200f, 120f), gameManager.Player), true, false));

            SerialEVENTGroup EnterCairn = new SerialEVENTGroup("EnterCairn");
            EnterCairn.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            EnterCairn.AddEVENT(new EVENTWarpActor(gameManager.Player, new Vector3(2350, 2800, 1f), stages["Cairn of Dead Kings"]));
            EnterCairn.AddEVENT(new EVENTMoveCamera(stages["Cairn of Dead Kings"], new Vector2(2750, 1400), 3000f));
            EnterCairn.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            //EnterCairn.AddEVENT(new EVENTSetCameraLocation(stages["Cairn of Dead Kings"], new Vector2(2350, 1400)));
            EnterCairn.AddEVENT(new EVENTCameraReactivate(stages["Cairn of Dead Kings"]));
            //EnterCairn.AddEVENT(new EVENTSetCameraLocation(stages["Cairn of Dead Kings"], new Vector2(2350, 3000)))

            SerialEVENTGroup ExitCairn = new SerialEVENTGroup("ExitCairn");
            ExitCairn.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            ExitCairn.AddEVENT(new EVENTCameraDeactivate(stages["Cairn of Dead Kings"]));
            ExitCairn.AddEVENT(new EVENTWarpActor(gameManager.Player, new Vector3(2450, 5670, 1f), stages["Cairn of Dead Kings"]));
            ExitCairn.AddEVENT(new EVENTMoveCamera(stages["Cairn of Dead Kings"], new Vector2(2450, 2900), 3000f));
            ExitCairn.AddEVENT(new EVENTPlayerReactivate(gameManager.Player));
            ExitCairn.AddEVENT(new EVENTSetCameraLocation(stages["Cairn of Dead Kings"], new Vector2(2450, 2900)));


            stages["Cairn of Dead Kings"].AddEVENT(EnterCairn);

            stages["Cairn of Dead Kings"].AddEVENT(ExitCairn);

            ParallelEVENTGroup CairnIntro = new ParallelEVENTGroup("Introduction");
            CairnIntro.AddEVENT(new EVENTSetCameraLocation(stages["Cairn of Dead Kings"], new Vector2(2400, 3000)));
            CairnIntro.AddEVENT(new EVENTCameraDeactivate(stages["Cairn of Dead Kings"]));
            CairnIntro.AddEVENT(new EVENTActorEnterStage(stages["Cairn of Dead Kings"], "Elysia", this, new Vector2(1660, 2800)));
            CairnIntro.AddEVENT(new EVENTActorAddProperty(gameManager.Player, "Open OT"));
            stages["Cairn of Dead Kings"].AMBStrength = 0f;

            stages["Cairn of Dead Kings"].AddActor(new SetProp("Tree1", "tree3", new Vector2(2300, 5575), gameManager), new Vector2(2365, 5775));
            stages["Cairn of Dead Kings"].AddActor(new SetProp("Tree2", "tree3", new Vector2(2550, 5575), gameManager), new Vector2(2535, 5775));

            stages["Cairn of Dead Kings"].AddEVENT(CairnIntro);


            mainCharacters.Add("Elysia", Elysia);



            // This is the shack in the woods where Elysia lives. It also serves as the entrance to the ritual grounds.
                     
            stages.Add("Forest", new Stage("Forest", this));
            stages["Forest"].adjustRegionCounts(2, 3);
            stages["Forest"].AMBStrength = 0f;
            Random rand = new Random(1123);
            int x, y;
            for (int i = 0; i < 80; i++)
            {

                for (int j = 0; j < 80; j++)
                {
                    x = rand.Next(25, 125);
                    y = rand.Next(25, 125);

                    stages["Forest"].addActor("Tree" + i + "_" + j, new Tree(gameManager, 150 * new Vector2(i, 2 * j) + 4096 * new Vector2(1, 2) + new Vector2(x, y)));

                }
            }



            stages["Forest"].removeActor("Tree6_4");
            stages["Forest"].removeActor("Tree7_4");
            stages["Forest"].removeActor("Tree6_5");
            stages["Forest"].removeActor("Tree7_5");
            stages["Forest"].removeActor("Tree5_4");
            stages["Forest"].removeActor("Tree8_4");
            stages["Forest"].removeActor("Tree9_4");
            stages["Forest"].removeActor("Tree8_5");

            for (int i = 0; i < 5; i++) stages["Forest"].removeActor("Tree4_" + i);
            for (int i = 9; i < 15; i++) stages["Forest"].removeActor("Tree" + i + "_5");

            stages["Forest"].addActor("Elysia's Shack", new SetProp("Elysia's Shack", "Shack", new Vector2(5096, 9215), gameManager));
            stages["Forest"].addActor("Crone's Shack", new SetProp("Crone's Shack", "Shack", new Vector2(5426, 9412), gameManager));

            stages["Forest"].AddTrigger(new Trigger("Elysia", new TriggerPlayerInteracting(stages["Forest"].GetActor("Elysia's Shack")), true, true));


            ParallelEVENTGroup ForestIntro = new ParallelEVENTGroup("Introduction");
            SerialEVENTGroup ConversationWithElysia = new SerialEVENTGroup("Elysia");
            ConversationWithElysia.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("HEY!", gameManager.Player, stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("Wake up in there! It's an emergency.", gameManager.Player, stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDramaticPause(4000f));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("HEY!", gameManager.Player, stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("I'm awake already! Stop banging on my house!", stages["Forest"].GetActor("Elysia's Shack"), stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("Well hurry it up! I don't have all night!", gameManager.Player, stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("I brought you into this world, Rhett, son-of-Arlan. Bang on my house one more time and I'll send you out again!", stages["Forest"].GetActor("Elysia's Shack"), stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("'Brought me into this world?' Do you think I was born yesterday? Everyone knows you've only got a season on me. ", gameManager.Player, stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTActorEnterStage(stages["Forest"], "Elysia", this, new Vector2(5242, 9238)));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("Why are you here?", Elysia, stages["Forest"]));
            ConversationWithElysia.AddEVENT(new EVENTDialogueConfirmed("'Brought me into this world?' Do you think I was born yesterday? Everyone knows you've only got a season on me. ", gameManager.Player, stages["Forest"]));

            stages["Forest"].AddEVENT(ConversationWithElysia);




            stages["Forest"].AMBStrength = .2f;

            x = 0; y = 0;


            bool[,][] forestBox = new bool[20, 20][];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    forestBox[i, j] = new bool[4] { true, true, true, true };
                }
            }

            List<Vector2> adjacentCells = new List<Vector2>();
            List<Vector2> containedCells = new List<Vector2>();
            List<Vector2> tempCells = new List<Vector2>();

            containedCells.Add(new Vector2(14, 19));
            adjacentCells.Add(new Vector2(14, 18));
            adjacentCells.Add(new Vector2(13, 19));
            adjacentCells.Add(new Vector2(15, 19));
            forestBox[17, 19][3] = false;



            while (adjacentCells.Count > 0)
            {
                Vector2 next = adjacentCells[rand.Next(adjacentCells.Count)];
                adjacentCells.Remove(next);
                containedCells.Add(next);
                tempCells.Clear();
                if (containedCells.Contains(new Vector2(next.X - 1, next.Y))) tempCells.Add(new Vector2(next.X - 1, next.Y));
                else if (next.X > 0) adjacentCells.Add(new Vector2(next.X - 1, next.Y));

                if (containedCells.Contains(new Vector2(next.X, next.Y - 1))) tempCells.Add(new Vector2(next.X, next.Y - 1));
                else if (next.Y > 0) adjacentCells.Add(new Vector2(next.X, next.Y - 1));

                if (containedCells.Contains(new Vector2(next.X + 1, next.Y))) tempCells.Add(new Vector2(next.X + 1, next.Y));
                else if (next.X < 19) adjacentCells.Add(new Vector2(next.X + 1, next.Y));

                if (containedCells.Contains(new Vector2(next.X, next.Y + 1))) tempCells.Add(new Vector2(next.X, next.Y + 1));
                else if (next.Y < 19) adjacentCells.Add(new Vector2(next.X, next.Y + 1));

                Vector2 check = next - tempCells[rand.Next(tempCells.Count)];
                if (check == -Vector2.UnitX)
                {
                    forestBox[(int)next.X, (int)next.Y][3] = false;
                    if (next.X < 19) forestBox[(int)next.X + 1, (int)next.Y][1] = false;

                }
                else if (check == Vector2.UnitX)
                {
                    forestBox[(int)next.X, (int)next.Y][1] = false;
                    if (next.X > 0) forestBox[(int)next.X - 1, (int)next.Y][3] = false;

                }
                else if (check == -Vector2.UnitY)
                {
                    forestBox[(int)next.X, (int)next.Y][2] = false;
                    if (next.Y < 19) forestBox[(int)next.X, (int)next.Y + 1][0] = false;

                }
                else if (check == Vector2.UnitY)
                {
                    forestBox[(int)next.X, (int)next.Y][0] = false;
                    if (next.Y > 0) forestBox[(int)next.X, (int)next.Y - 1][2] = false;

                }


            }



            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {

                    for (int k = rand.Next(1, 4); k > 0; k--)
                    {
                        x = rand.Next(50, 350);
                        y = rand.Next(50, 350);

                        stages["Forest"].addActor("Tree" + i + "_" + j + "R" + k, new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(x, y)));


                    }

                    stages["Forest"].addActor("Tree" + i + "_" + j + "NW", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(60, 60)));
                    stages["Forest"].addActor("Tree" + i + "_" + j + "NE", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(340, 61)));
                    stages["Forest"].addActor("Tree" + i + "_" + j + "SW", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(60, 340)));
                    stages["Forest"].addActor("Tree" + i + "_" + j + "SE", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(340, 341)));

                    stages["Forest"].addActor("Tree" + i + "_" + j + "CP", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(400, 400)));

                    if (forestBox[i, j][1])
                    {
                        stages["Forest"].addActor("Tree" + i + "_" + j + "NMW", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(50, 155)));
                        stages["Forest"].addActor("Tree" + i + "_" + j + "NME", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(50, 250)));
                    }
                    if (forestBox[i, j][0])
                    {
                        stages["Forest"].addActor("Tree" + i + "_" + j + "WMN", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(155, 62)));
                        stages["Forest"].addActor("Tree" + i + "_" + j + "WMM", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(250, 59)));
                    }

                    if (forestBox[i, j][2])
                    {
                        stages["Forest"].addActor("Tree" + i + "_" + j + "SWM", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(155, 341)));
                        stages["Forest"].addActor("Tree" + i + "_" + j + "SMM", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(250, 342)));
                        stages["Forest"].addActor("Tree" + i + "_" + j + "SCP", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(200, 400)));
                    }

                    if (forestBox[i, j][3])
                    {
                        stages["Forest"].addActor("Tree" + i + "_" + j + "EMN", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(354, 156)));
                        stages["Forest"].addActor("Tree" + i + "_" + j + "EMM", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(340, 251)));
                        stages["Forest"].addActor("Tree" + i + "_" + j + "EMS", new Tree(gameManager, 400 * new Vector2(i, j) + new Vector2(400, 200)));
                    }


                }



                    }
        
                 
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

        public void LoadContent(ContentManager Cm)
        {
            foreach (Actor A in mainCharacters.Values) A.loadImage(Cm);
           // grassTexture = Cm.Load<Texture2D>("Grass");
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
            SetStage(Next, startingPosition, false);
        }

        public void SetStage(String Next, Vector2 startingPosition, bool movePlayer) {
            if (openStage != null)
            {
                openStage.RemovePlayer();
                openStage.AM.StopSounds();
                openStage.AM.StopSong();
            }

            Stage nextStage = stages[Next];

            if (movePlayer)
            {
                nextStage.addPlayer(gameManager.Player);
            }
            else
            {
                nextStage.addPlayer(gameManager.Player, startingPosition);
            }

            nextStage.input = gameManager.Player.Input;
            nextStage.setCamera();
            nextStage.Load(contentManager);
            nextStage.Initialize();
            nextStage.RunEvent("LevelOpen");
            openStage = nextStage;
        }

        public void SetStage(String nextStage)
        {
            SetStage(nextStage, new Vector2(-150, 600));
        }

        public void Save(BinaryWriter writer, SaveType type, Dictionary<ISaveable, uint> objectTable)
        {
            List<ISaveable> saveableList = new List<ISaveable>();
            foreach (ISaveable saveable in stages.Values)
            {
                saveableList.Add(saveable);
            }

            Saver.SaveSaveableList(saveableList, writer, objectTable);
            writer.Write(objectTable[openStage]);
            writer.Write(objectTable[gameManager.Player]);
            Saver.SaveStringList(stateConditions, writer);
        }

        private StageManagerInflatables inflatables;

        public void Load(BinaryReader reader, SaveType type)
        {
            inflatables = new StageManagerInflatables();

            inflatables.stageIds = Loader.LoadSaveableList(reader);
            inflatables.openStageId = reader.ReadUInt32();
            inflatables.playerId = reader.ReadUInt32();

            if (type == SaveType.FULL)
            {
                stateConditions = Loader.LoadStringList(reader);
            }
            else
            {
                if (stateConditions == null)
                {
                    stateConditions = Loader.LoadStringList(reader);
                }
                else
                {
                    stateConditions.AddRange(Loader.LoadStringList(reader));
                }
            }
        }

        public void Inflate(Dictionary<uint, ISaveable> objectTable)
        {
            if (inflatables != null)
            {
                openStage = (Stage)objectTable[inflatables.openStageId];
                loadingPlayer = (Player)objectTable[inflatables.playerId];
                

                stages = new Dictionary<String, Stage>();
                foreach (uint objectId in inflatables.stageIds)
                {
                    Stage inflatedStage = (Stage)objectTable[objectId];
                    if (inflatedStage != null) {
                        stages.Add(inflatedStage.Id, inflatedStage);
                    }
                }
            }
        }

        internal Player GetLoadingPlayer()
        {
            return this.loadingPlayer;
        }

        public void FinishLoad(GameManager manager)
        {
            this.gameManager = manager;
            contentManager = gameManager.ContentManager;

            SetStage(openStage.Id, manager.Player.Position, true);
        }

        public List<ISaveable> GetSaveableRefs(SaveType type)
        {
            List<ISaveable> refs = new List<ISaveable>();
            foreach (Stage s in stages.Values) {
                refs.Add(s);
            }
            refs.Add(openStage);
            refs.Add(gameManager.Player);
            return refs;
        }

        public uint GetId()
        {
            return this.id;
        }

        private Player loadingPlayer;

        private class StageManagerInflatables
        {
            public List<uint> stageIds;
            public uint openStageId;
            public uint playerId;
        }
    }

}
