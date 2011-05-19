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

            stages.Add("Village", new Stage("Village", this));
            stages.Add("Cairn", new Stage("Cairn", this));



            Stage Court = new Stage("Court", this);
            SerialEVENTGroup Intro = new SerialEVENTGroup("Int");
            Character villagerA = new Character("VillagerA", "Actor2", new Vector2(600, 600), gameManager);
            Character villagerB = new Character("VillagerB", "Actor2", new Vector2(1000, 600), gameManager);
            Character Aide = new Character("Aide", "Actor2", new Vector2(800, 300), gameManager);
            Character Rhett = new Character("Rhett", "male_select", new Vector2(800, 800), gameManager);
            mainCharacters.Add("Rhett", Rhett);
            Court.AddActor(Rhett, new Vector2(800, 800));
            Court.AddActor(Aide, new Vector2(800, 300));

            Court.AddActor(villagerA, new Vector2(600, 600));

            Court.AddActor(villagerB, new Vector2(1000, 600));


            //Aide asks if a decision has been made.
            // Intro.AddEVENT(new EVENTPlayerDeactivate(gameManager.Player));
            Intro.AddEVENT(new EVENTDialogueConfirmed("Sir, have you come to a decision?", Aide, Court));
            RingMenu IntroChoice1 = new RingMenu(gameManager);
            IntroChoice1.AddMenuItem(new EventItem(Court, "Verdict", "ThumbsUpIcon", "Yes I have.", gameManager.UIManager));
            IntroChoice1.AddMenuItem(new EventItem(Court, "CaseRecap", "ThumbsDownIcon", "No, not yet.", gameManager.UIManager));
            Intro.AddEVENT(new EVENTOpenMenu(IntroChoice1, gameManager.UIManager));
            Court.AddEVENT(Intro);
            //A Verdict is decided

            RingMenu Questions = new RingMenu(gameManager);
            SerialEVENTGroup CaseRecap = new SerialEVENTGroup("CaseRecap");
            CaseRecap.AddEVENT(new EVENTOpenMenu(Questions, gameManager.UIManager));
            Court.AddEVENT("CaseRecap", CaseRecap);
            Questions.AddMenuItem(new EventItem(Court, "Verdict", "ThumbsUpIcon", "Make your decision", gameManager.UIManager));
            Questions.AddMenuItem(new EventItem(Court, "QuestionB", "BlankIcon", "Question Villager B", gameManager.UIManager));
            Questions.AddMenuItem(new EventItem(Court, "QuestionA", "BlankIcon", "Question VillagerA", gameManager.UIManager));

            SerialEVENTGroup QuestionA = new SerialEVENTGroup("QuestionA");
            QuestionA.AddEVENT(new EVENTDialogueConfirmed("What is your side of the story?", gameManager.Player, Court));
            QuestionA.AddEVENT(new EVENTDialogueConfirmed("One of my mare's is pregnant and Villager B is claiming that the calf belongs to him!", villagerA, Court));
            QuestionA.AddEVENT(new EVENTOpenEvent("CaseRecap", Court));

            SerialEVENTGroup QuestionB = new SerialEVENTGroup("QuestionB");
            QuestionB.AddEVENT(new EVENTDialogueConfirmed("What is your side of the story?", gameManager.Player, Court));
            QuestionB.AddEVENT(new EVENTDialogueConfirmed("Villager A's mare entered into my herd, where she was serviced by my bull. It happened in my herd, it should be my beast.", villagerB, Court));
            QuestionB.AddEVENT(new EVENTDialogueConfirmed("It happened in my herd, it should be my beast.", villagerB, Court));
            QuestionB.AddEVENT(new EVENTOpenEvent("CaseRecap", Court));

            Court.AddEVENT(QuestionA);
            Court.AddEVENT(QuestionB);


            SerialEVENTGroup Verdict = new SerialEVENTGroup("Verdict");
            Verdict.AddEVENT(new EVENTDialogueConfirmed("I have come to a decision.", gameManager.Player, Court));
            Verdict.AddEVENT(new EVENTDialogueConfirmed("And as the sole representative of Rymar-King, the Ambrosian, you agree to my judgement?", gameManager.Player, Court));
            ParallelEVENTGroup VillagersSayYes = new ParallelEVENTGroup();
            VillagersSayYes.AddEVENT(new EVENTDialogueConfirmed("Aye", villagerA, Court));
            VillagersSayYes.AddEVENT(new EVENTDialogueConfirmed("Yes ser.", villagerB, Court));
            Verdict.AddEVENT(VillagersSayYes);
            Verdict.AddEVENT(new EVENTDialogueConfirmed("Very well then, here is my decision:", gameManager.Player, Court));

            Court.AddEVENT(Verdict);
            RingMenu VerdictMenu = new RingMenu(gameManager);
            VerdictMenu.AddMenuItem(new EventItem(Court, "VillagerA", "BlankIcon", "Give it to Villager A.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Court, "VillagerB", "BlankIcon", "Give it to Villager B.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Court, "BothVillagers", "BlankIcon", "Share the calf between the two.", gameManager.UIManager));
            VerdictMenu.AddMenuItem(new EventItem(Court, "NeitherVillager", "BlankIcon", "It belongs to neither of you.", gameManager.UIManager));
            Verdict.AddEVENT(new EVENTOpenMenu(VerdictMenu, gameManager.UIManager));

            SerialEVENTGroup CourtEnding = new SerialEVENTGroup("CourtEnding");
            ParallelEVENTGroup CE1 = new ParallelEVENTGroup();
            CE1.AddEVENT(new EVENTDialogueTimed("All right everyone, shows over.", gameManager.Player, Court));
            CE1.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(800, 500), Court));
            CE1.AddEVENT(new EVENTMoveActor(villagerA, new Vector2(600, 1300), Court)); CE1.AddEVENT(new EVENTMoveActor(villagerB, new Vector2(1000, 1300), Court)); CE1.AddEVENT(new EVENTMoveActor(Aide, new Vector2(800, 100), Court));
            CourtEnding.AddEVENT(CE1);
            //CourtEnding.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(800, 500), Court));
           // CourtEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("That should stop 'em from bothering you with the small stuff, your mightyfullness.", Rhett, Court), this));
           // CourtEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("Don't call me that, Rhett. Besides, they're supposed to bring this stuff to me.", gameManager.Player, Court), this));
           // CourtEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("You remember last spring with Mickel and Jon. Do you want a repeat performance?", gameManager.Player, Court), this));
           // CourtEnding.AddEVENT(new EVENTStateConditionONEVENT("NeutralDecision", new EVENTDialogueConfirmed("Are you kidding? That was the most excitement we'd seen in months!", Rhett, Court), this));
            CourtEnding.AddEVENT(new EVENTDialogueConfirmed("Anyway, now that you're done with this, what say we start tomorrow's festivities a bit early?", Rhett, Court));
            CourtEnding.AddEVENT(new EVENTDialogueConfirmed("I could drink to that.", gameManager.Player, Court));
            ParallelEVENTGroup CE2 = new ParallelEVENTGroup();
            CE2.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(800, 1000), Court));
            // CE2.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(850, 1000), Court));
            CE2.AddEVENT(new EVENTCameraDeactivate(Court));
            CE2.AddEVENT(new EVENTChangeAmbient(Court, new Vector3(1.0f, 1.0f, .4f), .8f, 4000f));
            CourtEnding.AddEVENT(CE2);
            CourtEnding.AddEVENT(new EVENTStageChange(this, "Village", new Vector2(3000, 3000)));


            SerialEVENTGroup VillagerA = new SerialEVENTGroup("VillagerA");
            VillagerA.AddEVENT(new EVENTDialogueConfirmed("As it stands now, the calf is already in the posession of Villager A,", gameManager.Player, Court));
            VillagerA.AddEVENT(new EVENTDialogueConfirmed("I see no reason to take it from him.", gameManager.Player, Court));

            VillagerA.AddEVENT(VillagersSayYes);
            VillagerA.AddEVENT(new EVENTOpenEvent("CourtEnding", Court));
            Court.AddEVENT(VillagerA);
            SerialEVENTGroup VillagerB = new SerialEVENTGroup("VillagerB");

            VillagerB.AddEVENT(new EVENTDialogueConfirmed("The calf was conceived on Villager B's land, therefore it belongs to him.", gameManager.Player, Court));
            VillagerB.AddEVENT(new EVENTDialogueConfirmed("However; he will take care of the mare in the duration.", gameManager.Player, Court));


            VillagerB.AddEVENT(VillagersSayYes);
            VillagerB.AddEVENT(new EVENTOpenEvent("CourtEnding", Court));
            Court.AddEVENT("VillagerB", VillagerB);
            SerialEVENTGroup BothVillagers = new SerialEVENTGroup("BothVillagers");
            BothVillagers.AddEVENT(new EVENTDialogueConfirmed("Either work out a way to share it by the time it's born, or I'll cut it in two and you can choose sides.", gameManager.Player, Court));
            // BothVillagers.AddEVENT(new EVENTDialogueConfirmed("Either work out a way to share it by the time it's born, or I'll cut it in two and you can choose sides.", gameManager.Player, Court));
            BothVillagers.AddEVENT(new EVENTStateConditionSet("NeutralDecision", this));

            BothVillagers.AddEVENT(VillagersSayYes);
            BothVillagers.AddEVENT(new EVENTOpenEvent("CourtEnding", Court));

            Court.AddEVENT("BothVillagers", BothVillagers);
            SerialEVENTGroup NeitherVillagers = new SerialEVENTGroup("NeitherVillagers");
            NeitherVillagers.AddEVENT(new EVENTDialogueConfirmed("It seems to me that you've disproved each other's claims thoroughly.", gameManager.Player, Court));
            NeitherVillagers.AddEVENT(new EVENTDialogueConfirmed("You'll both raise the calf, and when it matures, the village will feast", gameManager.Player, Court));
            NeitherVillagers.AddEVENT(new EVENTStateConditionSet("NeutralDecision", this));
            NeitherVillagers.AddEVENT(VillagersSayYes);
            NeitherVillagers.AddEVENT(new EVENTOpenEvent("CourtEnding", Court));
            Court.AddEVENT("NeitherVillager", NeitherVillagers);
            Court.AddEVENT("CourtEnding", CourtEnding);
            stages.Add("Court", Court);

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
            //PE2.AddEVENT(new EVENTMoveActor(Wraith, new Vector2(800, 1200), Court));
            //PE2.AddEVENT(new EVENTChangeAmbient(Court, new Vector3(1.0f, 1.0f, 1.0f), 1f));
            //PE2.AddEVENT(new EVENTDialogueConfirmed("Hmmm... Knight to E5", Maximus, ShadyArea));
            //ShadyIntro.AddEVENT(PE2);
            //ShadyIntro.AddEVENT(new EVENTStageChange(this, "Village", new Vector2(2000, 4050)));


            //stages.Add("ShadyArea", ShadyArea);



            stages["Village"].AMBStrength = 1f;
            SetProp CenterStone = new SetProp("CenterStone", "SarcenStone2", new Vector2(2000, 2000), gameManager);
            stages["Village"].addActor("CenterStone", new SetProp("CenterStone", "SarcenStone2", new Vector2(2000, 4000), gameManager));
            for (int i = 0; i < 72; i++) { stages["Village"].addActor("SarcenStone" + i, new SetProp("Sarcen" + i, "SarcenStoneSmall", 2000 * (Vector2.One + Vector2.UnitY) + 1900 * new Vector2((float)Math.Cos(10 * i), 2* (float)Math.Sin(10 * i)), gameManager)); }

            stages["Village"].addActor("Shack1", new SetProp("Shack1", "Shack", new Vector2(600, 4000), gameManager));
            stages["Village"].addActor("Shack2", new SetProp("Shack2", "Shack", new Vector2(900, 4000), gameManager));
            stages["Village"].addActor("Shack3", new SetProp("Shack3", "Shack", new Vector2(1200, 4000), gameManager));
            stages["Village"].addActor("Shack4", new SetProp("Shack4", "Shack", new Vector2(300,4000), gameManager));
            stages["Village"].addActor("Shack5", new SetProp("Shack5", "Shack", new Vector2(3700, 4000), gameManager));
            stages["Village"].addActor("Shack6", new SetProp("Shack6", "Shack", new Vector2(2800, 4000), gameManager));
            stages["Village"].addActor("Shack7", new SetProp("Shack7", "Shack", new Vector2(3100, 4000), gameManager));
            stages["Village"].addActor("Shack8", new SetProp("Shack8", "Shack", new Vector2(3400, 4000), gameManager));

            stages["Village"].addActor("Shack11", new SetProp("Shack11", "Shack", new Vector2(700, 3400), gameManager));
            stages["Village"].addActor("Shack12", new SetProp("Shack12", "Shack", new Vector2(1000, 3400), gameManager));
            stages["Village"].addActor("Shack13", new SetProp("Shack13", "Shack", new Vector2(1300, 3400), gameManager));
            stages["Village"].addActor("Shack14", new SetProp("Shack14", "Shack", new Vector2(400, 3400), gameManager));
            stages["Village"].addActor("Shack15", new SetProp("Shack15", "Shack", new Vector2(3600, 3400), gameManager));
            stages["Village"].addActor("Shack16", new SetProp("Shack16", "Shack", new Vector2(2700, 3400), gameManager));
            stages["Village"].addActor("Shack17", new SetProp("Shack17", "Shack", new Vector2(3000, 3400), gameManager));
            stages["Village"].addActor("Shack18", new SetProp("Shack18", "Shack", new Vector2(3300, 3400), gameManager));

            stages["Village"].addActor("Shack21", new SetProp("Shack21", "Shack", new Vector2(700, 4600), gameManager));
            stages["Village"].addActor("Shack22", new SetProp("Shack22", "Shack", new Vector2(1000, 4600), gameManager));
            stages["Village"].addActor("Shack23", new SetProp("Shack23", "Shack", new Vector2(1300, 4600), gameManager));
            stages["Village"].addActor("Shack24", new SetProp("Shack24", "Shack", new Vector2(400, 4600), gameManager));
            stages["Village"].addActor("Shack25", new SetProp("Shack25", "Shack", new Vector2(3600, 4600), gameManager));
            stages["Village"].addActor("Shack26", new SetProp("Shack26", "Shack", new Vector2(2700, 4600), gameManager));
            stages["Village"].addActor("Shack27", new SetProp("Shack27", "Shack", new Vector2(3000, 4600), gameManager));
            stages["Village"].addActor("Shack28", new SetProp("Shack28", "Shack", new Vector2(3300, 4600), gameManager));


            stages["Village"].addActor("Shack31", new SetProp("Shack31", "Shack", new Vector2(900, 5200), gameManager));
            stages["Village"].addActor("Shack32", new SetProp("Shack32", "Shack", new Vector2(1200, 5200), gameManager));
            stages["Village"].addActor("Shack33", new SetProp("Shack33", "Shack", new Vector2(1500, 5200), gameManager));
            stages["Village"].addActor("Shack34", new SetProp("Shack34", "Shack", new Vector2(600, 5200), gameManager));
            stages["Village"].addActor("Shack35", new SetProp("Shack35", "Shack", new Vector2(3400, 5200), gameManager));
            stages["Village"].addActor("Shack36", new SetProp("Shack36", "Shack", new Vector2(2500, 5200), gameManager));
            stages["Village"].addActor("Shack37", new SetProp("Shack37", "Shack", new Vector2(2800, 5200), gameManager));
            stages["Village"].addActor("Shack38", new SetProp("Shack38", "Shack", new Vector2(3100, 5200), gameManager));


            stages["Village"].addActor("Shack41", new SetProp("Shack41", "Shack", new Vector2(900, 2800), gameManager));
            stages["Village"].addActor("Shack42", new SetProp("Shack42", "Shack", new Vector2(1200, 2800), gameManager));
            stages["Village"].addActor("Shack43", new SetProp("Shack43", "Shack", new Vector2(1500, 2800), gameManager));
            stages["Village"].addActor("Shack44", new SetProp("Shack44", "Shack", new Vector2(600, 2800), gameManager));
            stages["Village"].addActor("Shack45", new SetProp("Shack45", "Shack", new Vector2(3400, 2800), gameManager));
            stages["Village"].addActor("Shack46", new SetProp("Shack46", "Shack", new Vector2(2500, 2800), gameManager));
            stages["Village"].addActor("Shack47", new SetProp("Shack47", "Shack", new Vector2(2800, 2800), gameManager));
            stages["Village"].addActor("Shack48", new SetProp("Shack48", "Shack", new Vector2(3100, 2800), gameManager));
            stages["Village"].AMBColor = new Vector3(.5f, .5f, 1f);
            //  Follower Rhett = new Follower("Follower", new Vector2(2000, 4000), stages["Village"], gameManager);


            ParallelEVENTGroup VI1 = new ParallelEVENTGroup();

            ParallelEVENTGroup VIinit = new ParallelEVENTGroup();
            SerialEVENTGroup VillageIntro = new SerialEVENTGroup("Introduction");
            //VI1.AddEVENT(new EVENTActorEquipItem(Rhett, new Lantern(Rhett)));
           // VIinit.AddEVENT(new EVENTActorEnterStage(stages["Village"], "Rhett"));
           // VI1.AddEVENT(new EVENTActorEquipItem(gameManager.Player, new Lantern(gameManager.Player)));
            VI1.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2050, 3000), stages["Village"]));
            //VI1.AddEVENT(new EVENTMoveActor(gameManager.Player, new Vector2(2000, 3000), stages["Village"]));
            VillageIntro.AddEVENT(VIinit);
            VillageIntro.AddEVENT(VI1);
            stages["Village"].AddEVENT(VillageIntro);
            VillageIntro.AddEVENT(new EVENTDialogueConfirmed("We're talking.... Oh No! The king's manor is on fire!", Rhett, stages["Village"]));
            VillageIntro.AddEVENT(new EVENTCameraDeactivate(stages["Village"]));
            VillageIntro.AddEVENT(new EVENTMoveCamera(gameManager.Camera, new Vector2(800, 2000), 1000f));
            VillageIntro.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 2000), stages["Village"]));
            VillageIntro.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 1000), stages["Village"]));

          //  stages["Village"].AddTrigger(new Trigger("KingDies", new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(1600, 400, 0), new Vector3(2400, 1200, 10)), gameManager.Player), true, true));

            SerialEVENTGroup KingDies = new SerialEVENTGroup("KingDies");
            ParallelEVENTGroup PD1 = new ParallelEVENTGroup();
            PD1.AddEVENT(new EVENTMoveActor(Wraith, new Vector2(200, 2400), stages["Village"]));
            SerialEVENTGroup KD1A = new SerialEVENTGroup("KD1A");
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Hunh?", gameManager.Player, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("What was that", gameManager.Player, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("What was what?", Rhett, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("I thought I some something fleeing the building.", gameManager.Player, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("You're just jumping at shadows.", Rhett, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueTimed("Sooo...", Rhett, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Looks like the old king is dead. That sucks.", Rhett, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Yea, it does. Rhett I need you to go get the Crone.", gameManager.Player, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Do I HAVE to?You know we don't exaclty see eye to eye.", Rhett, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("I need to be here, otherwise I'd go myself. We NEED her right now, much as you dislike her.", gameManager.Player, stages["Village"]));
            KD1A.AddEVENT(new EVENTDialogueConfirmed("Fine. I'll get her, but you OWE me for this.", Rhett, stages["Village"]));
            KD1A.AddEVENT(new EVENTMoveActor(Rhett, new Vector2(2200, 2000), stages["Village"]));
           // KD1A.AddEVENT(new EVENTActorEquipItem(Rhett, null));
            KD1A.AddEVENT(new EVENTChangeAmbient(stages["Village"], new Vector3(1.0f, 1.0f, 1.0f), 0, 10000f));

            PD1.AddEVENT(KD1A);
            Character Elysia = new Character("Elysia", "male_select", new Vector2(2000, 3000), gameManager);
            stages["Village"].AddActor(Elysia, new Vector2(3500, 2000));
            PD1.AddEVENT(new EVENTMoveActor(Elysia, new Vector2(2300, 2200), stages["Village"]));
            SerialEVENTGroup KD2A = new SerialEVENTGroup("KD2A");
            KD2A.AddEVENT(new EVENTMoveCamera(gameManager.Camera, new Vector2(2000, 2000), 3000f));
            KD2A.AddEVENT(new EVENTDramaticPause(3000f));
            KD2A.AddEVENT(new EVENTStateConditionSet("KingsDead", this));

            KingDies.AddEVENT(PD1);
            KingDies.AddEVENT(KD2A);
            stages["Village"].AddEVENT(KingDies);

           // stages["Village"].AddTrigger(new Trigger("MeetElysia",
                //new TriggerANDCondition(new TriggerStateCondition(this, "KingsDead"),
                    //new TriggerPlayerBoxCondition(new BoundingBox(new Vector3(1700, 1700, 0), new Vector3(2300, 2300, 10)), gameManager.Player)), true, true));
            SerialEVENTGroup MeetElysia = new SerialEVENTGroup("MeetElysia");
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Elysia? Where's the old lady?", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Hi Eimar, it's nice to see you too.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Sorry Elysia, I need to speak to the Crone, I don't have time to chat.", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueTimed("Eimar, well, the Crone wasn't able to come and sent Elysia to speak in her stead.", Rhett, stages["Village"], 80f));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("I can speak for myself you lummox!", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("My master sent me to attend to the details, she'll be along soon enough.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Now if you don't have anymore questions I need to attend to the body", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Wait, before you go I have some questions.", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Fine, but be quick about it.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("In the manor I saw a strange creature.", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("It was as tall as a man, with dark fur and a gleaming, eyeless skull where it's head should rightly be.", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("With two claws as long as a boys arm at the end of it's upper limbs.", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("I'm telling you, you were jumping at shadows anda  bit too much ale.", Rhett, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("That sounds like a Wendigo, a Spirit of Shadow.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Spirits? You might use that non-sense tofrighten the young, but don't expect us to fall for it.", Rhett, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("You would be wise to just forget about it and chalk it up to bad luck and good beer.", Rhett, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("The Spirits are not to be trifled with!", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Least of all a Spirit of Shadow.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("We must be careful to keep this thing at Bay.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("How can we keep it from attacking the village?", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("We will be safe enough during the day, Wendigo cannot stand light.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("At night we'll need to light fires around the village, they fear fire.", Elysia, stages["Village"]));
            //  MeetElysia.AddEVENT(new EVENTDialogueConfirmed("That seems like a lot of work.", , stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Why don't we just get a hunting party together during the day and find it then?", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("You can't just kill Spirits as you would a man!", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Mundane weapons cannot harm them, for they are not of this world.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("So why don't you just cast a spell and smite it with holy fire or something 'O Powerful One.'", Rhett, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Oh, shut up!", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("So how CAN we kill it.", gameManager.Player, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("The Sword of the King has powerful enchantments that allow it to be used against the Otherworldly.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("But only in the hands of the king.", Elysia, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Well, there you go. Problem solved.", Rhett, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Sorry to put it so bluntly, but now that Rymar-King's dead, you're the King now Eimar.", Rhett, stages["Village"]));
            MeetElysia.AddEVENT(new EVENTDialogueConfirmed("Which means you can now truely wield the 'O Mighty Goblin-Whacker'", Rhett, stages["Village"]));


            stages["Village"].AddEVENT("MeetElysia", MeetElysia);


            mainCharacters.Add("Elysia", Elysia);
            mainCharacters.Add("Wraith", Wraith);



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
