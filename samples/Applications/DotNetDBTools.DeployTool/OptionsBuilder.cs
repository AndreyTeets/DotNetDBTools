using McMaster.Extensions.CommandLineUtils;

namespace DotNetDBTools.DeployTool;

internal class OptionsBuilder
{
    public CommandOption<Dbms> AddDbmsOption(CommandLineApplication app)
    {
        CommandOption<Dbms> option = app.Option<Dbms>(
            "--dbms <DBMS>",
            "Required. Targeted DBMS kind.",
            CommandOptionType.SingleValue);

        option.IsRequired().Accepts().Enum<Dbms>(ignoreCase: true);
        return option;
    }

    public CommandOption<string> AddAsmOption(CommandLineApplication app)
    {
        CommandOption<string> option = app.Option<string>(
            "--asm <AssemblyPath>",
            "Required. Path to dbAssembly file.",
            CommandOptionType.SingleValue);

        option.IsRequired();
        return option;
    }

    public CommandOption<string> AddNewasmOption(CommandLineApplication app)
    {
        CommandOption<string> option = app.Option<string>(
            "--newasm <NewAssemblyPath>",
            "Required. Path to new dbAssembly file.",
            CommandOptionType.SingleValue);

        option.IsRequired();
        return option;
    }

    public CommandOption<string> AddOldasmOption(CommandLineApplication app)
    {
        CommandOption<string> option = app.Option<string>(
            "--oldasm <OldAssemblyPath>",
            "Required. Path to old dbAssembly file.",
            CommandOptionType.SingleValue);

        option.IsRequired();
        return option;
    }

    public CommandOption<string> AddCsOption(CommandLineApplication app)
    {
        CommandOption<string> option = app.Option<string>(
            "--cs <ConnectionString>",
            "Required. Connection string.",
            CommandOptionType.SingleValue);

        option.IsRequired();
        return option;
    }

    public CommandOption<string> AddOutOption(CommandLineApplication app)
    {
        CommandOption<string> option = app.Option<string>(
            "--out <OutputPath>",
            "Required. Output path.",
            CommandOptionType.SingleValue);

        option.IsRequired();
        return option;
    }

    public CommandOption<bool> AddLossOption(CommandLineApplication app)
    {
        CommandOption<bool> option = app.Option<bool>(
            "--loss",
            "Optional. Allow data loss during update.",
            CommandOptionType.SingleOrNoValue);

        return option;
    }

    public CommandOption<bool> AddDdlonlyOption(CommandLineApplication app)
    {
        CommandOption<bool> option = app.Option<bool>(
            "--ddlonly",
            "Optional. Include only DDL changes into script (and skip DNDBT system info changes).",
            CommandOptionType.SingleOrNoValue);

        return option;
    }
}

