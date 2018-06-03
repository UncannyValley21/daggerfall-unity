﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2018 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using FullSerializer;

namespace DaggerfallWorkshop.Game.Questing.Actions
{
    /// <summary>
    /// Places a Person to a random building in their home town within current region.
    /// </summary>
    public class CreateNpc : ActionTemplate
    {
        Symbol npcSymbol;

        public override string Pattern
        {
            get { return @"create npc (?<anNPC>[a-zA-Z0-9_.-]+)"; }
        }

        public CreateNpc(Quest parentQuest)
            : base(parentQuest)
        {
        }

        public override IQuestAction CreateNew(string source, Quest parentQuest)
        {
            // Source must match pattern
            Match match = Test(source);
            if (!match.Success)
                return null;

            // Factory new action
            CreateNpc action = new CreateNpc(parentQuest);
            action.npcSymbol = new Symbol(match.Groups["anNPC"].Value);

            return action;
        }

        public override void Update(Task caller)
        {
            base.Update(caller);

            // Attempt to get Person resource
            Person person = ParentQuest.GetPerson(npcSymbol);
            if (person == null)
            {
                SetComplete();
                throw new Exception(string.Format("Could not find Person resource symbol {0}", npcSymbol));
            }

            // Just output some debug text for now
            QuestMachine.LogFormat("TODO (Debug Only): Created NPC {0} in {1}/{2} for building {3}", person.DisplayName, person.HomeRegionName, person.HomeTownName, person.HomeBuildingName);

            SetComplete();
        }

        #region Serialization

        [fsObject("v1")]
        public struct SaveData_v1
        {
            public Symbol npcSymbol;
        }

        public override object GetSaveData()
        {
            SaveData_v1 data = new SaveData_v1();
            data.npcSymbol = npcSymbol;

            return data;
        }

        public override void RestoreSaveData(object dataIn)
        {
            SaveData_v1 data = (SaveData_v1)dataIn;
            if (dataIn == null)
                return;

            npcSymbol = data.npcSymbol;
        }

        #endregion
    }
}