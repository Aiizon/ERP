namespace ERP.TEST;
internal class Program
{
    static void Main(string[] args)
    {
        // Initialise la logique de contrôle
        GameControl consoleControl = new ConsoleControl();

        // Initialise les couleurs de la console
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Hello World!");

        #region bool
        bool? boolQuestionResult = consoleControl.AskUserBool("Cette question est-elle jolie ?")!.Value;
        if (boolQuestionResult.Value)
            Console.WriteLine("Merci ! <3");
        else
            Console.WriteLine(":c");
        ConsoleControl.Pause(true);
        #endregion

        #region char
        char? charQuestionResult = consoleControl.AskUserChar("Par quelle lettre ton prénom commence-t-il ?").Value;
        Console.WriteLine($"Ton prénom commence par la lettre {charQuestionResult}, super !");
        ConsoleControl.Pause(true);
        #endregion

        #region string
        string? stringQuestionResult = consoleControl.AskUserString("Quel est ton mot favoris ?");
        if (string.IsNullOrEmpty(stringQuestionResult))
            Console.WriteLine("woops !");
        else
            Console.WriteLine($"Super, ton mot favoris est {stringQuestionResult} !");
        ConsoleControl.Pause(true);
        #endregion

        #region integer
        int? integerQuestionResult = consoleControl.AskUserInteger("Quel est ton nombre favoris ?");
        if (null == integerQuestionResult)
            Console.WriteLine("woops !");
        else
            Console.WriteLine($"Super, ton nombre favoris est {integerQuestionResult} !");
        ConsoleControl.Pause(true);
        #endregion

        // Réinitialisation des paramètres de la console pour la suite du code
        Console.ResetColor();
    }
}
