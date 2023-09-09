using Spectre.Console;

namespace CyberTechBattleBit2.PrettyConsole.Pages;

public class MainPage : ConsolePageTemplate
{
    public MainPage()
    {
        Start();
    }

    //Generate main Table
    //|-----------|-Plyrs-|
    //|Console----|-------|
    //|-----------|-------|
    //|-----------|-------|
    //|-----------|-------|


    public async void Start()
    {
        var table = new Table().Centered();
        AnsiConsole.Live(table).Start(ctx =>
        {
            table.AddColumn("Console");
            table.Columns[0].Width(90);
            ctx.Refresh();
            // Thread.Sleep(1000);

            table.AddColumn("Players");
            table.Columns[1].Width(30);
            ctx.Refresh();

            table.AddColumn("Servers");
            table.Columns[2].Width(30);
            ctx.Refresh();

            for (var i = 0; i < 10; i++) table.AddEmptyRow();


            Thread.Sleep(1000);
        });
    }
}

public class ConsolePageTemplate
{
}