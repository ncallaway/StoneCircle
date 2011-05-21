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
            stageTransitionMenu.AddMenuItem(new ChangeLevelItem("MainHouse", new Vector2(2000, 4000), this, gameManager.UIManager, "Main House"));
            stageTransitionMenu.AddMenuItem(new ChangeLevelItem("Cairn", new Vector2(1000, 1000), this, gameManager.UIManager, "Cairn of Dead Kings"));
            stageTransitionMenu.AddMenuItem(new ChangeLevelItem("Village", new Vector2(500, 500), this, gameManager.UIManager, "Small Village"));
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


            // stages["region1"].AddTrigger(new Trigger("Bandage", new TriggerANDCondition(new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(600, 400, 0), new Vector3(601, 1200, 1)), gameManager.Player), new TriggerActorHasProperty(gameManager.Player, "Bleeding")), true, true));
            SerialEVENTGroup Bandage = new SerialEVENTGroup("Bandage");
            Bandage.AddEVENT(new EVENTDialogueTimed("I'm losing a lot of blood... I'll need to bandage myself if I don't want to bleed to death", gameManager.Player, stages["region1"]));
            Bandage.AddEVENT(new EVENTDialogueTimed("Hold RT and press Y. Don't move until you've finished bandaging.", gameManager.Player, stages["region1"]));
            Bandage.AddEVENT(new EVENTActorAddItem(gameManager.Player, new Lantern(gameManager.Player)));
            stages["region1"].AddEVENT("Bandage", Bandage);
            //  stages["region1"].AddTrigger(new Trigger("region2trans", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(300, 1200, 0), new Vector3(2000, 1201, 0)), gameManager.Player), true, false));
            stages["region1"].AddEVENT("region2trans", new EVENTStageChange(this, "region2", new Vector2(100, 100)));

            stages["region1"].addLight("Light", new Vector2(-100, 600), 1200);


            Character Rhett = new Character("Rhett", "male_select", new Vector2(800, 800), gameManager);
            mainCharacters.Add("Rhett", Rhett);



            // This is the dialogue and set up for a people's Village trial regarding a matter of cattle 
            // ( which represents the socio-economic status of the people involved).
            // 
            Stage Village = new Stage("Village", this);
            SerialEVENTGroup Intro = new SerialEVENTGroup("Int");
            Character MainHouserA = new Character("MainHouserA", "Actor2", new Vector2(600, 600), gameManager);
            Character MainHouserB = new Character("MainHouserB", "Actor2", new Vector2(1000, 600), gameManager);
            Character Aide = new Character("Aide", "Actor2", new Vector2(800, 300), gameManager);
            Village.AddActor(Rhett, new Vector2(800, 800));
            Village.AddActor(Aide, new Vector2(800, 300));

            Village.AddActor(MainHouserA, new Vector2(600, 600));

            Village.AddActor(MainHouserB, new Vector2(1000, 600));


            stages.Add("MainHouse", new Stage("MainHouse", this));
            //Aide asks if a decision has been made.
            // Intro.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            Intro.AddEVENT(new EVENTDialogueConfirmed("Sir, have you come to a decision?", Aide, Village));
            RingMenu IntroChoice1 = new RingMenu(gameManager);
            IntroChoice1.AddMenuItem(new EventItem(Village, "Verdict", "ThumbsUpIcon", "Yes I have.", gameManager.UIManager));
            IntroChoice1.AddMenuItem(new EventItem(Village, "CaseRecap", "ThumbsDownIcon", "No, not yet.", gameManager.UIManager));
            Intro.AddEVENT(new EVENTOpenMenu(IntroChoice1, gameManager.UIManager));
            Village.AddEVENT(Intro);
            //A Verdict is decided

            RingMenu Questions = new RingMenu(gameManager);
            SerialEVENTGroup CaseRecap = new SerialEVENTGroup("CaseRecap");
            CaseRecap.AddEVENT(new EVENTOpenMenu(Questions, gameManager.UIManager));
            Village.AddEVENT("CaseRecap", CaseRecap);
            Questions.AddMenuItem(new EventItem(Village, "Verdict", "ThumbsUpIcon", "Make your decision", gameManager.UIManager));
            Questions.AddMenuItem(new EventItem(Village, "QuestionB", "BlankIcon", "Question MainHouser B", gameManager.UIManager));
            Questions.AddMenuItem(new EventItem(Village, "QuestionA", "BlankIcon", "Question MainHouserA", gameManager.UIManager));

            SerialEVENTGroup QuestionA = new SerialEVENTGroup("QuestionA");
            QuestionA.AddEVENT(new EVENTDialogueConfirmed("What is your side of the story?", gameManager.Player, Village));
            QuestionA.AddEVENT(new EVENTDialogueConfirmed("One of my mare's is pregnant and MainHouser B is claiming that the calf belongs to him!", MainHouserA, Village));
            QuestionA.AddEVENT(new EVENTOpenEvent("CaseRecap", Village));

            SerialEVENTGroup QuestionB = new SerialEVENTGroup("QuestionB");
            QuestionB.AddEVENT(new EVENTDialogueConfirmed("What is your side of the story?", gameManager.Player, Village));
            QuestionB.AddEVENT(new EVENTDialogueConfirmed("MainHouser A's mare entered into my herd, where she was serviced by my bull. It happened in my herd, it should be my beast.", MainHouserB, Village));
            QuestionB.AddEVENT(new EVENTDialogueConfirmed("It happened in my herd, it should be my beast.", MainHouserB, Village));
            QuestionB.AddEVENT(new EVENTOpenEvent("CaseRecap", Village));

            Village.AddEVENT(QuestionA);
            Village.AddEVENT(QuestionB);


            SerialEVENTGroup Verdict = new SerialEVENTGroup("Verdict");
            Verdict.AddEVENT(new EVENTDialogueConfirmed("I have come to a decision.", gameManager.Player, Village));
            Verdict.AddEVENT(new EVENTDialogueConfirmed("And as the sole representative of Rymar-King, the Ambrosian, you agree to my judgement?", gameManager.Player, Village));
            ParallelEVENTGroup MainHousersSayYes = new ParallelEVENTGroup();
            MainHousersSayYes.AddEVENT(new EVENTDialogueConfirmed("Aye", MainHouserA, Village));
            MainHousersSayYes.AddEVENT(new EVENTDialogueConfirmed("Yes ser.", MainHouserB, Village));
            Verdict.AddEVENT(MainHousersSayYes);
            Verdict.AddEVENT(new EVENTDialogueConfirmed("Very well then, here is my decision:", gameManager.Player, Village));

            Village.AddEVENT(Verdict);
            RingMenu VerdictMenu = new RingMenu(gameManager);
            VerdictMenu.AddMenuItem(new EventItem(Village, "MainHouserA", "BlankIcon", "Give it to MainHouser A.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Village, "MainHouserB", "BlankIcon", "Give it to MainHouser B.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Village, "BothMainHousers", "BlankIcon", "Share the calf between the two.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Village, "NeitherMainHouser", "BlankIcon", "It belongs to neither of you.", gameManager.UIManager));
            Verdict.AddEVENT(new EVENTOpenMenu(VerdictMenu, gameManager.UIManager));

            SerialEVENTGroup VillageEnding = new SerialEVENTGroup("VillageEnding");
            ParallelEVENTGroup CE1 = new ParallelEVENTGroup();
            CE1.AddEVENT(new EVENTDialogueTimed("All right everyone, shows over.", gameManager.Player, Village));
            CE1.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(800, 500), Village));
            CE1.AddEVENT(new EVENTMoveActor(MainHouserA, new Vector2(600, 1300), Village)); CE1.AddEVENT(new EVENTMoveActor(MainHouserB, new Vector2(1000, 1300), Village)); CE1.AddEVENT(new EVENTMoveActor(Aide, new Vector2(800, 100), Village));
            VillageEnding.AddEVENT(CE1);
            VillageEnding.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(800, 500), Village));
             VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("That should stop 'em from bothering you with the small stuff, your mightyfullness.", Rhett, Village), this));
             VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("Don't call me that, Rhett. Besides, they're supposed to bring this stuff to me.", gameManager.Player, Village), this));
             VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("You remember last spring with Mickel and Jon. Do you want a repeat performance?", gameManager.Player, Village), this));
             VillageEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("Are you kidding? That was the most excitement we'd seen in months!", Rhett, Village), this));
            VillageEnding.AddEVENT(new EVENTDialogueConfirmed("Anyway, now that you're done with this, what say we start tomorrow's festivities a bit early?", Rhett, Village));
            VillageEnding.AddEVENT(new EVENTDialogueConfirmed("I could drink to that.", gameManager.Player, Village));
            ParallelEVENTGroup CE2 = new ParallelEVENTGroup();
            CE2.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(800, 1000), Village));
            // CE2.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(850, 1000), Village));
            CE2.AddEVENT(new EVENTCameraDeactivate(Village));
            CE2.AddEVENT(new EVENTChangeAmbient(Village, new Vector3(1.0f, 1.0f, .4f), .8f, 4000f));
            VillageEnding.AddEVENT(CE2);
            VillageEnding.AddEVENT(new EVENTStageChange(this, "MainHouse", new Vector2(3000, 3000)));


            SerialEVENTGroup VillagerA = new SerialEVENTGroup("VillagerA");
            VillagerA.AddEVENT(new EVENTDialogueConfirmed("As it stands now, the calf is already in the posession of MainHouser A,", gameManager.Player, Village));
            VillagerA.AddEVENT(new EVENTDialogueConfirmed("I see no reason to take it from him.", gameManager.Player, Village));

            VillagerA.AddEVENT(MainHousersSayYes);
            VillagerA.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));
            Village.AddEVENT(VillagerA);
            SerialEVENTGroup VillagerB = new SerialEVENTGroup("VillagerB");

            VillagerB.AddEVENT(new EVENTDialogueConfirmed("The calf was conceived on MainHouser B's land, therefore it belongs to him.", gameManager.Player, Village));
            VillagerB.AddEVENT(new EVENTDialogueConfirmed("However; he will take care of the mare in the duration.", gameManager.Player, Village));


            VillagerB.AddEVENT(MainHousersSayYes);
            VillagerB.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));
            Village.AddEVENT("VillagerB", VillagerB);
            SerialEVENTGroup BothMainHousers = new SerialEVENTGroup("BothMainHousers");
            BothMainHousers.AddEVENT(new EVENTDialogueConfirmed("Either work out a way to share it by the time it's born, or I'll cut it in two and you can choose sides.", gameManager.Player, Village));
            // BothMainHousers.AddEVENT(new EVENTDialogueConfirmed("Either work out a way to share it by the time it's born, or I'll cut it in two and you can choose sides.", gameManager.Player, Village));
            BothMainHousers.AddEVENT(new EVENTStateConditionSet("NeutralDecision", this));

            BothMainHousers.AddEVENT(MainHousersSayYes);
            BothMainHousers.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));

            Village.AddEVENT("BothMainHousers", BothMainHousers);
            SerialEVENTGroup NeitherMainHousers = new SerialEVENTGroup("NeitherMainHousers");
            NeitherMainHousers.AddEVENT(new EVENTDialogueConfirmed("It seems to me that you've disproved each other's claims thoroughly.", gameManager.Player, Village));
            NeitherMainHousers.AddEVENT(new EVENTDialogueConfirmed("You'll both raise the calf, and when it matures, the MainHouse will feast", gameManager.Player, Village));
            NeitherMainHousers.AddEVENT(new EVENTStateConditionSet("NeutralDecision", this));
            NeitherMainHousers.AddEVENT(MainHousersSayYes);
            NeitherMainHousers.AddEVENT(new EVENTOpenEvent("VillageEnding", Village));
            Village.AddEVENT("NeitherMainHouser", NeitherMainHousers);
            Village.AddEVENT("VillageEnding", VillageEnding);
            stages.Add("Village", Village);


            // This is the main tent of Maximus, Pilus Prior, of the 3rd Cohort of the _______ Legion.
            // He is first seen here ordering the Wraiths to attack the Kings of NOT-Ireland. The area
            // is mostly in shadow, keeping Maximus obscured until he's revealed later on, (With his cohort in tow)

            ////Shady Area of Bad Guys
            //Stage ShadyArea = new Stage("Shady1", this);
            //ShadyArea.AMBStrength = .8f;
            //ShadyArea.addLight(new LightSource("Light1", new Vector2(500, 1050), 800f, ShadyArea, null));
            //ShadyArea.addLight(new LightSource("Light2", new Vector2(1100, 1050), 800f, ShadyArea, null));

            //Actor Maximus = new Actor("Maximus", "knightm1", new Vector2(800, 800));
            Character Wraith = new Character("Wraith", "DJ", new Vector2(800, 1000), gameManager);
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
            //ShadyIntro.AddEVENT(new EVENTStageChange(this, "MainHouse", new Vector2(2000, 4050)));


            //stages.Add("ShadyArea", ShadyArea);


            // This is the house of the king, and the main feasting area for the villages. 
            // It's considered to be owned by the community. Any of the community may come in
            // and impose on the king etc. There are some smaller buildings/ areas for people. 
            stages["MainHouse"].AMBStrength = 1f;
            SetProp CenterStone = new SetProp("CenterStone", "SarcenStone2", new Vector2(2000, 2000), gameManager);
            stages["MainHouse"].addActor("CenterStone", new SetProp("CenterStone", "SarcenStone2", new Vector2(2000, 4000), gameManager));
            for (int i = 0; i < 72; i++) { stages["MainHouse"].addActor("SarcenStone" + i, new SetProp("Sarcen" + i, "SarcenStoneSmall", 2000 * (Vector2.One + Vector2.UnitY) + 1900 * new Vector2((float)Math.Cos(10 * i), 2 * (float)Math.Sin(10 * i)), gameManager)); }

            stages["MainHouse"].addActor("Shack1", new SetProp("Shack1", "Shack", new Vector2(600, 4000), gameManager));
            stages["MainHouse"].addActor("Shack2", new SetProp("Shack2", "Shack", new Vector2(900, 4000), gameManager));
            stages["MainHouse"].addActor("Shack3", new SetProp("Shack3", "Shack", new Vector2(1200, 4000), gameManager));
            stages["MainHouse"].addActor("Shack4", new SetProp("Shack4", "Shack", new Vector2(300, 4000), gameManager));
            stages["MainHouse"].addActor("Shack5", new SetProp("Shack5", "Shack", new Vector2(3700, 4000), gameManager));
            stages["MainHouse"].addActor("Shack6", new SetProp("Shack6", "Shack", new Vector2(2800, 4000), gameManager));
            stages["MainHouse"].addActor("Shack7", new SetProp("Shack7", "Shack", new Vector2(3100, 4000), gameManager));
            stages["MainHouse"].addActor("Shack8", new SetProp("Shack8", "Shack", new Vector2(3400, 4000), gameManager));

            stages["MainHouse"].addActor("Shack11", new SetProp("Shack11", "Shack", new Vector2(700, 3400), gameManager));
            stages["MainHouse"].addActor("Shack12", new SetProp("Shack12", "Shack", new Vector2(1000, 3400), gameManager));
            stages["MainHouse"].addActor("Shack13", new SetProp("Shack13", "Shack", new Vector2(1300, 3400), gameManager));
            stages["MainHouse"].addActor("Shack14", new SetProp("Shack14", "Shack", new Vector2(400, 3400), gameManager));
            stages["MainHouse"].addActor("Shack15", new SetProp("Shack15", "Shack", new Vector2(3600, 3400), gameManager));
            stages["MainHouse"].addActor("Shack16", new SetProp("Shack16", "Shack", new Vector2(2700, 3400), gameManager));
            stages["MainHouse"].addActor("Shack17", new SetProp("Shack17", "Shack", new Vector2(3000, 3400), gameManager));
            stages["MainHouse"].addActor("Shack18", new SetProp("Shack18", "Shack", new Vector2(3300, 3400), gameManager));

            stages["MainHouse"].addActor("Shack21", new SetProp("Shack21", "Shack", new Vector2(700, 4600), gameManager));
            stages["MainHouse"].addActor("Shack22", new SetProp("Shack22", "Shack", new Vector2(1000, 4600), gameManager));
            stages["MainHouse"].addActor("Shack23", new SetProp("Shack23", "Shack", new Vector2(1300, 4600), gameManager));
            stages["MainHouse"].addActor("Shack24", new SetProp("Shack24", "Shack", new Vector2(400, 4600), gameManager));
            stages["MainHouse"].addActor("Shack25", new SetProp("Shack25", "Shack", new Vector2(3600, 4600), gameManager));
            stages["MainHouse"].addActor("Shack26", new SetProp("Shack26", "Shack", new Vector2(2700, 4600), gameManager));
            stages["MainHouse"].addActor("Shack27", new SetProp("Shack27", "Shack", new Vector2(3000, 4600), gameManager));
            stages["MainHouse"].addActor("Shack28", new SetProp("Shack28", "Shack", new Vector2(3300, 4600), gameManager));


            stages["MainHouse"].addActor("Shack31", new SetProp("Shack31", "Shack", new Vector2(900, 5200), gameManager));
            stages["MainHouse"].addActor("Shack32", new SetProp("Shack32", "Shack", new Vector2(1200, 5200), gameManager));
            stages["MainHouse"].addActor("Shack33", new SetProp("Shack33", "Shack", new Vector2(1500, 5200), gameManager));
            stages["MainHouse"].addActor("Shack34", new SetProp("Shack34", "Shack", new Vector2(600, 5200), gameManager));
            stages["MainHouse"].addActor("Shack35", new SetProp("Shack35", "Shack", new Vector2(3400, 5200), gameManager));
            stages["MainHouse"].addActor("Shack36", new SetProp("Shack36", "Shack", new Vector2(2500, 5200), gameManager));
            stages["MainHouse"].addActor("Shack37", new SetProp("Shack37", "Shack", new Vector2(2800, 5200), gameManager));
            stages["MainHouse"].addActor("Shack38", new SetProp("Shack38", "Shack", new Vector2(3100, 5200), gameManager));


            stages["MainHouse"].addActor("Shack41", new SetProp("Shack41", "Shack", new Vector2(900, 2800), gameManager));
            stages["MainHouse"].addActor("Shack42", new SetProp("Shack42", "Shack", new Vector2(1200, 2800), gameManager));
            stages["MainHouse"].addActor("Shack43", new SetProp("Shack43", "Shack", new Vector2(1500, 2800), gameManager));
            stages["MainHouse"].addActor("Shack44", new SetProp("Shack44", "Shack", new Vector2(600, 2800), gameManager));
            stages["MainHouse"].addActor("Shack45", new SetProp("Shack45", "Shack", new Vector2(3400, 2800), gameManager));
            stages["MainHouse"].addActor("Shack46", new SetProp("Shack46", "Shack", new Vector2(2500, 2800), gameManager));
            stages["MainHouse"].addActor("Shack47", new SetProp("Shack47", "Shack", new Vector2(2800, 2800), gameManager));
            stages["MainHouse"].addActor("Shack48", new SetProp("Shack48", "Shack", new Vector2(3100, 2800), gameManager));
            stages["MainHouse"].AMBColor = new Vector3(.5f, .5f, 1f);
            //  Follower Rhett = new Follower("Follower", new Vector2(2000, 4000), stages["MainHouse"], gameManager);


            ParallelEVENTGroup VI1 = new ParallelEVENTGroup();

            ParallelEVENTGroup VIinit = new ParallelEVENTGroup();
            SerialEVENTGroup MainHouseIntro = new SerialEVENTGroup("Introduction");
            //VI1.AddEVENT(new EVENTActorEquipItem(Rhett, new Lantern(Rhett)));
            // VIinit.AddEVENT(new EVENTActorEnterStage(stages["MainHouse"], "Rhett"));
            // VI1.AddEVENT(new EVENTActorEquipItem(gameManager.Player, new Lantern(gameManager.Player)));
            VI1.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2050, 3000), stages["MainHouse"]));
            //VI1.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(2000, 3000), stages["MainHouse"]));
            MainHouseIntro.AddEVENT(VIinit);
            MainHouseIntro.AddEVENT(VI1);
            stages["MainHouse"].AddEVENT(MainHouseIntro);
            MainHouseIntro.AddEVENT(new EVENTDialogueConfirmed("We're talking.... Oh No! The king's manor is on fire!", Rhett, stages["MainHouse"]));
            MainHouseIntro.AddEVENT(new EVENTCameraDeactivate(stages["MainHouse"]));
            MainHouseIntro.AddEVENT(new EVENTMoveCamera(gameManager.Camera, new Vector2(800, 2000), 1000f));
            MainHouseIntro.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 2000), stages["MainHouse"]));
            MainHouseIntro.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 1000), stages["MainHouse"]));

            //  stages["MainHouse"].AddTrigger(new Trigger("KingDies", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(1600, 400, 0), new Vector3(2400, 1200, 10)), gameManager.Player), true, true));

            SerialEVENTGroup KingDies = new SerialEVENTGroup("KingDies");
            ParallelEVENTGroup PD1 = new ParallelEVENTGroup();
            PD1.AddEVENT(new EVENTMoveActor(Wraith, new Vector2(200, 2400), stages["MainHouse"]));
            SerialEVENTGroup KD1A = new SerialEVENTGroup("KD1A");
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Hunh?", gameManager.Player, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("What was that", gameManager.Player, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("What was what?", Rhett, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("I thought I some something fleeing the building.", gameManager.Player, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("You're just jumping at shadows.", Rhett, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueTimed("Sooo...", Rhett, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Looks like the old king is dead. That sucks.", Rhett, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Yea, it does. Rhett I need you to go get the Crone.", gameManager.Player, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Do I HAVE to?You know we don't exaclty see eye to eye.", Rhett, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("I need to be here, otherwise I'd go myself. We NEED her right now, much as you dislike her.", gameManager.Player, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Fine. I'll get her, but you OWE me for this.", Rhett, stages["MainHouse"]));
            KD1A.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 2000), stages["MainHouse"]));
            // KD1A.AddEVENT(new EVENTActorEquipItem(Rhett, null));
            KD1A.AddEVENT(new EVENTChangeAmbient(stages["MainHouse"], new Vector3(1.0f, 1.0f, 1.0f), 0, 10000f));

            PD1.AddEVENT(KD1A);
            Character Elysia = new Character("Elysia", "male_select", new Vector2(2000, 3000), gameManager);
            stages["MainHouse"].AddActor(Elysia, new Vector2(3500, 2000));
            PD1.AddEVENT(new EVENTMoveActor(Elysia, new Vector2(2300, 2200), stages["MainHouse"]));
            SerialEVENTGroup KD2A = new SerialEVENTGroup("KD2A");
            KD2A.AddEVENT(new EVENTMoveCamera(gameManager.Camera, new Vector2(2000, 2000), 3000f));
            KD2A.AddEVENT(new EVENTDramaticPause(3000f));
            KD2A.AddEVENT(new EVENTStateConditionSet("KingsDead", this));

            KingDies.AddEVENT(PD1);
            KingDies.AddEVENT(KD2A);
            stages["MainHouse"].AddEVENT(KingDies);

            // stages["MainHouse"].AddTrigger(new Trigger("MeetElysia",
            //new TriggerANDCondition(new TriggerStateCondition(this, "KingsDead"),
            //new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(1700, 1700, 0), new Vector3(2300, 2300, 10)), gameManager.Player)), true, true));
            SerialEVENTGroup MeetElysia = new SerialEVENTGroup("MeetElysia");
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Elysia? Where's the old lady?", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Hi Eimar, it's nice to see you too.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Sorry Elysia, I need to speak to the Crone, I don't have time to chat.", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueTimed("Eimar, well, the Crone wasn't able to come and sent Elysia to speak in her stead.", Rhett, stages["MainHouse"], 80f));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("I can speak for myself you lummox!", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("My master sent me to attend to the details, she'll be along soon enough.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Now if you don't have anymore questions I need to attend to the body", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Wait, before you go I have some questions.", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Fine, but be quick about it.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("In the manor I saw a strange creature.", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("It was as tall as a man, with dark fur and a gleaming, eyeless skull where it's head should rightly be.", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("With two claws as long as a boys arm at the end of it's upper limbs.", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("I'm telling you, you were jumping at shadows anda  bit too much ale.", Rhett, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("That sounds like a Wendigo, a Spirit of Shadow.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Spirits? You might use that non-sense tofrighten the young, but don't expect us to fall for it.", Rhett, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("You would be wise to just forget about it and chalk it up to bad luck and good beer.", Rhett, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("The Spirits are not to be trifled with!", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Least of all a Spirit of Shadow.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("We must be careful to keep this thing at Bay.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("How can we keep it from attacking the MainHouse?", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("We will be safe enough during the day, Wendigo cannot stand light.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("At night we'll need to light fires around the MainHouse, they fear fire.", Elysia, stages["MainHouse"]));
            //  MeetElysia.AddEVENT(new EVENTDialogueConfirmed("That seems like a lot of work.", , stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Why don't we just get a hunting party together during the day and find it then?", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("You can't just kill Spirits as you would a man!", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Mundane weapons cannot harm them, for they are not of this world.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("So why don't you just cast a spell and smite it with holy fire or something 'O Powerful One.'", Rhett, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Oh, shut up!", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("So how CAN we kill it.", gameManager.Player, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("The Sword of the King has powerful enchantments that allow it to be used against the Otherworldly.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("But only in the hands of the king.", Elysia, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Well, there you go. Problem solved.", Rhett, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Sorry to put it so bluntly, but now that Rymar-King's dead, you're the King now Eimar.", Rhett, stages["MainHouse"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Which means you can now truely wield the 'O Mighty Goblin-Whacker'", Rhett, stages["MainHouse"]));

            stages["MainHouse"].AddEVENT("MeetElysia", MeetElysia);


            // This is the "Cairn of Dead Kings". It's a creamation and funereal mound for the kings of old.
            // There is a Stonehenge esque lintel and pathway facing due West. When the king is cremated
            // at Sunset his spirit leaves his body and passes through the gate into the afterlife, joining the ancestors.

            stages.Add("CairnTop", new Stage("CairnTop", this));





            mainCharacters.Add("Elysia", Elysia);
            mainCharacters.Add("Wraith", Wraith);


            // This is one of the smaller villages of Ambrosia
            // It has a couple houses etc.  


            stages["Village"].addActor("House1", new SetProp("House1", "Shack", new Vector2(600, 600), gameManager));
            stages["Village"].addActor("House2", new SetProp("House2", "Shack", new Vector2(400, 900), gameManager));

            stages["Village"].addActor("House3", new SetProp("House3", "Shack", new Vector2(1400, 500), gameManager));
            stages["Village"].addActor("House4", new SetProp("House4", "Shack", new Vector2(800, 1800), gameManager));
            stages["Village"].addActor("House5", new SetProp("House5", "Shack", new Vector2(1400, 1200), gameManager));
            stages["Village"].addActor("House6", new SetProp("House6", "Shack", new Vector2(1700, 900), gameManager));


            stages["Village"].addActor("Tree1", new Tree(new Vector2(550, 300), stages["Village"], gameManager));
            stages["Village"].addActor("Tree2", new Tree(new Vector2(550, 300), stages["Village"], gameManager));
            stages["Village"].addActor("Tree3", new Tree(new Vector2(500, 500), stages["Village"], gameManager));
            stages["Village"].addActor("Tree4", new Tree(new Vector2(450, 600), stages["Village"], gameManager));
            stages["Village"].addActor("Tree5", new Tree(new Vector2(600, 200), stages["Village"], gameManager));
            stages["Village"].addActor("Tree6", new Tree(new Vector2(675, 300), stages["Village"], gameManager));
            stages["Village"].addActor("Tree7", new Tree(new Vector2(770, 100), stages["Village"], gameManager));
            stages["Village"].addActor("Tree8", new Tree(new Vector2(900, 300), stages["Village"], gameManager));
            stages["Village"].addActor("Tree9", new Tree(new Vector2(1550, 300), stages["Village"], gameManager));
            stages["Village"].addActor("Tree10", new Tree(new Vector2(220, 700), stages["Village"], gameManager));
            stages["Village"].addActor("Tree11", new Tree(new Vector2(245, 1180), stages["Village"], gameManager));
            stages["Village"].addActor("Tree12", new Tree(new Vector2(235, 910), stages["Village"], gameManager));
            stages["Village"].addActor("Tree13", new Tree(new Vector2(245, 1050), stages["Village"], gameManager));
            stages["Village"].addActor("Tree14", new Tree(new Vector2(395, 1450), stages["Village"], gameManager));
            stages["Village"].addActor("Tree15", new Tree(new Vector2(1245, 1700), stages["Village"], gameManager));
            stages["Village"].addActor("Tree16", new Tree(new Vector2(1275, 1730), stages["Village"], gameManager));
            stages["Village"].addActor("Tree17", new Tree(new Vector2(1045, 370), stages["Village"], gameManager));
            stages["Village"].addActor("Tree18", new Tree(new Vector2(1845, 1480), stages["Village"], gameManager));
            stages["Village"].addActor("Tree19", new Tree(new Vector2(805, 1430), stages["Village"], gameManager));

            stages["Village"].addActor("Tree20", new Tree(new Vector2(145, 380), stages["Village"], gameManager));
            stages["Village"].addActor("Tree21", new Tree(new Vector2(145, 580), stages["Village"], gameManager));
            stages["Village"].addActor("Tree22", new Tree(new Vector2(145, 780), stages["Village"], gameManager));
            stages["Village"].addActor("Tree23", new Tree(new Vector2(145, 980), stages["Village"], gameManager));
            stages["Village"].addActor("Tree24", new Tree(new Vector2(145, 1180), stages["Village"], gameManager));
            stages["Village"].addActor("Tree25", new Tree(new Vector2(145, 1380), stages["Village"], gameManager));
            stages["Village"].addActor("Tree26", new Tree(new Vector2(145, 1580), stages["Village"], gameManager));
            stages["Village"].addActor("Tree27", new Tree(new Vector2(145, 1780), stages["Village"], gameManager));
            stages["Village"].addActor("Tree28", new Tree(new Vector2(145, 1980), stages["Village"], gameManager));
            stages["Village"].addActor("Tree29", new Tree(new Vector2(145, 2180), stages["Village"], gameManager));

            stages["Village"].addActor("Tree30", new Tree(new Vector2(95, 480), stages["Village"], gameManager));
            stages["Village"].addActor("Tree31", new Tree(new Vector2(95, 680), stages["Village"], gameManager));
            stages["Village"].addActor("Tree32", new Tree(new Vector2(95, 880), stages["Village"], gameManager));
            stages["Village"].addActor("Tree33", new Tree(new Vector2(95, 1080), stages["Village"], gameManager));
            stages["Village"].addActor("Tree34", new Tree(new Vector2(95, 1280), stages["Village"], gameManager));
            stages["Village"].addActor("Tree35", new Tree(new Vector2(95, 1480), stages["Village"], gameManager));
            stages["Village"].addActor("Tree36", new Tree(new Vector2(95, 1680), stages["Village"], gameManager));
            stages["Village"].addActor("Tree37", new Tree(new Vector2(95, 1880), stages["Village"], gameManager));
            stages["Village"].addActor("Tree38", new Tree(new Vector2(95, 2080), stages["Village"], gameManager));
            stages["Village"].addActor("Tree39", new Tree(new Vector2(95, 2280), stages["Village"], gameManager));

            stages["Village"].addActor("Tree40", new Tree(new Vector2(45, 381), stages["Village"], gameManager));
            stages["Village"].addActor("Tree41", new Tree(new Vector2(45, 581), stages["Village"], gameManager));
            stages["Village"].addActor("Tree42", new Tree(new Vector2(45, 781), stages["Village"], gameManager));
            stages["Village"].addActor("Tree43", new Tree(new Vector2(45, 981), stages["Village"], gameManager));
            stages["Village"].addActor("Tree44", new Tree(new Vector2(45, 1181), stages["Village"], gameManager));
            stages["Village"].addActor("Tree45", new Tree(new Vector2(45, 1381), stages["Village"], gameManager));
            stages["Village"].addActor("Tree46", new Tree(new Vector2(45, 1581), stages["Village"], gameManager));
            stages["Village"].addActor("Tree47", new Tree(new Vector2(45, 1781), stages["Village"], gameManager));
            stages["Village"].addActor("Tree48", new Tree(new Vector2(45, 1981), stages["Village"], gameManager));
            stages["Village"].addActor("Tree49", new Tree(new Vector2(45, 2181), stages["Village"], gameManager));

            stages["Village"].AddTrigger(new Trigger("LevelChange",new TriggerNOTCondition( new TriggerPlayerBoxCondition(new CollisionBox(new Vector3( 1000,1000, 0), 2000f, 5000f, 2000f), gameManager.Player)), true, true));
            stages["Village"].AddEVENT("LevelChange", new EVENTOpenMenu(stageTransitionMenu, gameManager.UIManager));
            
            
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
            grassTexture = Cm.Load<Texture2D>("Grass");
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
