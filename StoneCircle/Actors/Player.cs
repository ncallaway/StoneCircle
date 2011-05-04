using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using UserMenus;

namespace StoneCircle
{
   class Player : Actor
    {
        public InputController Input;
       
        
       // These variables represent the location and position of the actor. 


        public Player(String Id, String asset_name, Vector2 starting, Stage Parent, InputController Input, GameManager gameManager) : base(gameManager)
        {
            asset_Name = "PlayerSheet";
            speed = 150;
            this.Input = Input;
            learnAction(new Interact());
            Location = new Vector3( starting.X, starting.Y, 0);
            name = Id;
            parent = Parent;
            inventoryMenu = new Inventory(this, gameManager);
            inventoryOpen = false;
            
           
            ImageHeight = 75;
            ImageWidth = 40;
            // properties.Add("LegInjury");
            
        }

        public Player(String Id, String asset_name, Vector2 starting, GameManager gameManager, InputController Input)
            : base(gameManager)
        {
            asset_Name = "PlayerSheet";
            speed = 125;
            this.Input = Input;
            learnAction(new Interact());
            Location = new Vector3(starting.X, starting.Y, 0);
            name = Id;
            inventoryMenu = new Inventory(this, gameManager);
            ImageHeight = 75;
            ImageWidth = 40;
            
            inventoryOpen = false;
            learnAction(new FightStance());
            learnAction(new Dash());
            learnAction(new DashJump());
            learnAction(new BandageSelf(this));
            
        
          
            currentFatigue = 100;
        }

     

        public override void loadImage(ContentManager theContentManager)
        {
            base.loadImage(theContentManager);
            inventoryMenu.Load(theContentManager);
        }

        protected override void heartBeat()
        {
            parent.AM.PlayEffect("HeartBeat");
            base.heartBeat();
        }

        public override String ChooseAction(GameTime t, Dictionary<String, Actor>.ValueCollection targets)
        {
            Input.Update();

           Location.Z = Math.Max(Location.Z - 2, 0);
            if( Input.LStickPosition().LengthSquared() >.25f)Facing = Input.LStickPosition();
            if (Input.IsRightTriggerHeld())
            {
                if (Input.IsAButtonNewlyPressed() && current_Action.AvailableHigh.AButton != null) return current_Action.AvailableHigh.AButton;
                if (Input.IsBButtonNewlyPressed() && current_Action.AvailableHigh.BButton != null) return (current_Action.AvailableHigh.BButton);
                if (Input.IsXButtonNewlyPressed() && current_Action.AvailableHigh.XButton != null) return (current_Action.AvailableHigh.XButton);
                if (Input.IsYButtonNewlyPressed() && current_Action.AvailableHigh.YButton != null) return (current_Action.AvailableHigh.YButton);
                if (Input.LStickPosition().Length() > .2f && current_Action.AvailableHigh.LStickAction != null) return current_Action.AvailableHigh.LStickAction;
                if (current_Action.AvailableHigh.NoButton != null) return current_Action.AvailableHigh.NoButton;
            
            }
            else {
                if (Input.IsAButtonNewlyPressed() && current_Action.AvailableLow.AButton != null) return current_Action.AvailableLow.AButton;
                if (Input.IsBButtonNewlyPressed() && current_Action.AvailableLow.BButton != null) return (current_Action.AvailableLow.BButton);
                if (Input.IsXButtonNewlyPressed() && current_Action.AvailableLow.XButton != null) return (current_Action.AvailableLow.XButton);
                if (Input.IsYButtonNewlyPressed() && current_Action.AvailableLow.YButton != null) return (current_Action.AvailableLow.YButton);
                if (Input.LStickPosition().Length() > .2f && current_Action.AvailableLow.LStickAction != null) return current_Action.AvailableLow.LStickAction;
                if (current_Action.AvailableLow.NoButton != null) return current_Action.AvailableLow.NoButton;
            }
            return "";
        }


        public override void Update(GameTime t, Dictionary<String,Actor>.ValueCollection targets) // Updates position, vestigial remnants of player update. 
        {
            if (Bleeding && currentLife > 0)
            {
                currentBeatTime -= t.ElapsedGameTime.Milliseconds;
                if (currentBeatTime < 0)
                {
                    heartBeat();
                    currentBeatTime = currentBeatTimer;
                }
            }

            if (currentLife <= 0) SetAction("Dead");

            if (currentFatigue <= 0) SetAction("Unconcious");
            if (currentFatigue > totalFatigue) currentFatigue = totalFatigue;
            currentFatigue += current_Action.Fatigue;

            if (Input.IsRightBumperNewlyPressed()) { gameManager.UIManager.OpenMenu(inventoryMenu); if(currentItem!=null) currentItem.OnUnequipItem(); }

            current_Action.Update(t, targets);
            SetAction(ChooseAction(t, targets));
        }

        public override void Draw(SpriteBatch theSpriteBatch, Vector2 camera_pos, float camera_scale, float intensity, SpriteFont font)
        {
            if(inventoryOpen) inventoryMenu.Draw(theSpriteBatch);
           base.Draw(theSpriteBatch, camera_pos, camera_scale, intensity, font);
        }

        public void CloseInventory()
        {   
            gameManager.UIManager.CloseMenu();
            currentItem.OnEquipItem();
        }

        public virtual void EmptyHands() { if(currentItem!=null) currentItem.OnUnequipItem(); currentItem = null; }


        public void AddInventoryItem(Item item) { inventoryMenu.AddMenuItem(new InventoryItem(item)); }

              
    }
    }


                                 