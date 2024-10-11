namespace ERP.TEST;
internal class Program
{
    static void Main(string[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Hello, World!");

        bool? questionResult = AskUserBool("Cette question est-elle jolie ?").Value;
        if (null == questionResult)
            Console.WriteLine("woops !");
        else
        {
            if (questionResult.Value)
                Console.WriteLine("Merci ! <3");
            else
                Console.WriteLine(":c");
        }

        Console.ResetColor();
        Console.ReadKey(true);
    }

    public static bool? AskUserBool(string question, bool? defaultResult = null)
    {
        try
        {
            Console.WriteLine(question);

            // Gestion de la chaîne de caractères pour afficher la valeur par défaut si elle existe
            string defaultResultInfo = (null == defaultResult) ? "" : (defaultResult.Value ? "[Y]" : "[N]");

            Console.Write($"[Y]yes | [N]o {defaultResultInfo} > ");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine();

            // Récupération du caractère entré
            char key = char.ToLowerInvariant(input.KeyChar);

            switch (key)
            {
                case 'y':
                    return true;
                case 'n':
                    return false;
                // Si aucun caractère n'est entré, renvoie la valeur par défaut si elle existe ou pose à nouveau la question
                default:
                    if (defaultResult.HasValue)
                        return defaultResult.Value;
                    return AskUserBool(question, defaultResult);
            }
        }
        catch (Exception e)
        {
            // Si une exception est levée au cours du traitement, l'affiche dans la console
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.GetType().FullName);
            Console.WriteLine("Une exception a été levée au cours du traitement : " + e.Message);
            Console.ResetColor();
        }

        if (defaultResult.Value)
            return defaultResult;
        return null;
    }
}
