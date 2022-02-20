using DotNetDBTools.DeployTool;
using DotNetDBTools.DeployTool.Commands;
using McMaster.Extensions.CommandLineUtils;

CommandLineApplication app = new();
app.HelpOption(inherited: true);
OptionsBuilder optionsBuilder = new();

app.Command("register", cmd =>
{
    cmd.Description = "Register database specified by connection string as DNDBT.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> csOption = optionsBuilder.AddCsOption(cmd);
    cmd.OnExecute(() =>
    {
        new RegisterCommand().Execute(
            dbmsOption.ParsedValue,
            csOption.ParsedValue);
    });
});

app.Command("unregister", cmd =>
{
    cmd.Description = "Unregister database specified by connection string as DNDBT.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> csOption = optionsBuilder.AddCsOption(cmd);
    cmd.OnExecute(() =>
    {
        new UnregisterCommand().Execute(
            dbmsOption.ParsedValue,
            csOption.ParsedValue);
    });
});

app.Command("publish", cmd =>
{
    cmd.Description = "Update database specified by connection string to dbAssembly.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> asmOption = optionsBuilder.AddAsmOption(cmd);
    CommandOption<string> csOption = optionsBuilder.AddCsOption(cmd);
    CommandOption<bool> lossOption = optionsBuilder.AddLossOption(cmd);
    cmd.OnExecute(() =>
    {
        new PublishCommand().Execute(
            dbmsOption.ParsedValue,
            asmOption.ParsedValue,
            csOption.ParsedValue,
            lossOption.ParsedValue);
    });
});

app.Command("scriptupdate", cmd =>
{
    cmd.Description = "Create script for updating database specified by connection string to dbAssembly.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> asmOption = optionsBuilder.AddAsmOption(cmd);
    CommandOption<string> csOption = optionsBuilder.AddCsOption(cmd);
    CommandOption<string> outOption = optionsBuilder.AddOutOption(cmd);
    CommandOption<bool> lossOption = optionsBuilder.AddLossOption(cmd);
    cmd.OnExecute(() =>
    {
        new ScriptUpdateCommand().Execute(
            dbmsOption.ParsedValue,
            asmOption.ParsedValue,
            csOption.ParsedValue,
            outOption.ParsedValue,
            lossOption.ParsedValue);
    });
});

app.Command("scriptnew", cmd =>
{
    cmd.Description = "Create script for updating empty database to dbAssembly.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> asmOption = optionsBuilder.AddAsmOption(cmd);
    CommandOption<string> outOption = optionsBuilder.AddOutOption(cmd);
    CommandOption<bool> ddlonlyOption = optionsBuilder.AddDdlonlyOption(cmd);
    cmd.OnExecute(() =>
    {
        new ScriptNewCommand().Execute(
            dbmsOption.ParsedValue,
            asmOption.ParsedValue,
            outOption.ParsedValue,
            ddlonlyOption.ParsedValue);
    });
});

app.Command("scriptasmdiff", cmd =>
{
    cmd.Description = "Create script for updating database with state of old dbAssembly to state of new dbAssembly.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> newasmOption = optionsBuilder.AddNewasmOption(cmd);
    CommandOption<string> oldasmOption = optionsBuilder.AddOldasmOption(cmd);
    CommandOption<string> outOption = optionsBuilder.AddOutOption(cmd);
    CommandOption<bool> lossOption = optionsBuilder.AddLossOption(cmd);
    CommandOption<bool> ddlonlyOption = optionsBuilder.AddDdlonlyOption(cmd);
    cmd.OnExecute(() =>
    {
        new ScriptsAsmDiffCommand().Execute(
            dbmsOption.ParsedValue,
            newasmOption.ParsedValue,
            oldasmOption.ParsedValue,
            outOption.ParsedValue,
            lossOption.ParsedValue,
            ddlonlyOption.ParsedValue);
    });
});

app.Command("definition", cmd =>
{
    cmd.Description = "Generate definition of database specified by connection string to specified output directory.";
    CommandOption<Dbms> dbmsOption = optionsBuilder.AddDbmsOption(cmd);
    CommandOption<string> csOption = optionsBuilder.AddCsOption(cmd);
    CommandOption<string> outOption = optionsBuilder.AddOutOption(cmd);
    cmd.OnExecute(() =>
    {
        new DefinitionCommand().Execute(
            dbmsOption.ParsedValue,
            csOption.ParsedValue,
            outOption.ParsedValue);
    });
});

app.OnExecute(() =>
{
    Console.WriteLine("Specify a subcommand");
    app.ShowHelp();
    return 1;
});

return app.Execute(args);
