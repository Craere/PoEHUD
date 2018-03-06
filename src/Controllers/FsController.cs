using PoeHUD.Framework;
using PoeHUD.Poe.FilesInMemory;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PoeHUD.Controllers
{
    public class FsController
    {
        public readonly BaseItemTypes BaseItemTypes;
        public readonly ItemClasses itemClasses;
        public readonly ModsDat Mods;
        public readonly StatsDat Stats;
        public readonly TagsDat Tags;
        public readonly WorldAreas WorldAreas;
        public readonly PassiveSkills PassiveSkills;
        public readonly LabyrinthTrials LabyrinthTrials;
        public readonly Quests Quests;
        public readonly QuestStates QuestStates;
        public readonly BestiaryCapturableMonsters BestiaryCapturableMonsters;
        public readonly BestiaryRecipes BestiaryRecipes;
        public readonly BestiaryRecipeComponents BestiaryRecipeComponents;
        public readonly BestiaryGroups BestiaryGroups;
        public readonly BestiaryFamilies BestiaryFamilies;
        public readonly BestiaryGenuses BestiaryGenuses;
        public readonly MonsterVarieties MonsterVarieties;

        private readonly Dictionary<string, long> files;
        private readonly Memory mem;

        //Debug for DeveloperTool
        private BestiaryCapturableMonsters BestiaryCapturableMonsters_Debug => BestiaryCapturableMonsters;
        private BestiaryRecipes BestiaryRecipes_Debug => BestiaryRecipes;
        private BestiaryRecipeComponents BestiaryRecipeComponents_Debug => BestiaryRecipeComponents;
        private BestiaryGroups BestiaryGroups_Debug => BestiaryGroups;
        private BestiaryFamilies BestiaryFamilies_Debug => BestiaryFamilies;
        private BestiaryGenuses BestiaryGenuses_Debug => BestiaryGenuses;
        private MonsterVarieties MonsterVarieties_Debug => MonsterVarieties;


        public FsController(Memory mem)
        {
            this.mem = mem;
            files = GetAllFiles();
            itemClasses = new ItemClasses();
            BaseItemTypes = new BaseItemTypes(mem, FindFile("Data/BaseItemTypes.dat"));
            Tags = new TagsDat(mem, FindFile("Data/Tags.dat"));
            Stats = new StatsDat(mem, FindFile("Data/Stats.dat"));
            Mods = new ModsDat(mem, FindFile("Data/Mods.dat"), Stats, Tags);
            WorldAreas = new WorldAreas(mem, FindFile("Data/WorldAreas.dat"));
            PassiveSkills = new PassiveSkills(mem, FindFile("Data/PassiveSkills.dat"));
            LabyrinthTrials = new LabyrinthTrials(mem, FindFile("Data/LabyrinthTrials.dat"));
            Quests = new Quests(mem, FindFile("Data/Quest.dat"));
            QuestStates = new QuestStates(mem, FindFile("Data/QuestStates.dat"));
            BestiaryCapturableMonsters = new BestiaryCapturableMonsters(mem, FindFile("Data/BestiaryCapturableMonsters.dat"));
            BestiaryRecipes = new BestiaryRecipes(mem, FindFile("Data/BestiaryRecipes.dat"));
            BestiaryRecipeComponents = new BestiaryRecipeComponents(mem, FindFile("Data/BestiaryRecipeComponent.dat"));
            BestiaryGroups = new BestiaryGroups(mem, FindFile("Data/BestiaryGroups.dat"));
            BestiaryFamilies = new BestiaryFamilies(mem, FindFile("Data/BestiaryFamilies.dat"));
            BestiaryGenuses = new BestiaryGenuses(mem, FindFile("Data/BestiaryGenus.dat"));
            MonsterVarieties = new MonsterVarieties(mem, FindFile("Data/MonsterVarieties.dat"));
        }

        public Dictionary<string, long> GetAllFiles()
        {
            var fileList = new Dictionary<string, long>();
            long fileRoot = mem.AddressOfProcess + mem.offsets.FileRoot;
            long start = mem.ReadLong(fileRoot + 0x8);

            for (long CurrFile = mem.ReadLong(start); CurrFile != start && CurrFile != 0; CurrFile = mem.ReadLong(CurrFile))
            {
                 var str = mem.ReadStringU(mem.ReadLong(CurrFile + 0x10), 512);

                if (!fileList.ContainsKey(str))
                {
                    fileList.Add(str, mem.ReadLong(CurrFile + 0x18));
                }
            }
            return fileList;
        }

        public long FindFile(string name)
        {
            try
            {
                return files[name];
            }
            catch (KeyNotFoundException)
            {
                const string MESSAGE_FORMAT = "Couldn't find the file in memory: {0}\nTry to restart the game.";
                MessageBox.Show(string.Format(MESSAGE_FORMAT, name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            return 0;
        }
    }
}