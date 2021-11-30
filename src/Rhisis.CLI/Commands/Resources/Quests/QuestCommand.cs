﻿using McMaster.Extensions.CommandLineUtils;

namespace Rhisis.CLI.Commands.Resources.Quests
{
    [Command("quest", Description = "Manages the database users")]
    [Subcommand(typeof(QuestConverterCommand))]
    public class QuestCommand
    {
        public void OnExecute(CommandLineApplication app) => app.ShowHelp();
    }
}
