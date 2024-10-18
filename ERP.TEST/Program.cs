namespace ERP.TEST;
internal class Program
{
    static void Main(string[] args)
    {
        // Initialise les couleurs de la console
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Hello, World!");

        bool? boolQuestionResult = AskUserBool("Cette question est-elle jolie ?").Value;
        if (null == boolQuestionResult)
            Console.WriteLine("woops !");
        else
        {
            if (boolQuestionResult.Value)
                Console.WriteLine("Merci ! <3");
            else
                Console.WriteLine(":c");
        }

        string stringQuestionResult = AskUserString("Quel est ton mot favoris ?");
        if (string.IsNullOrEmpty(stringQuestionResult))
            Console.WriteLine("woops !");
        else
            Console.WriteLine($"Super, ton mot favoris est {stringQuestionResult} !");

        int? integerQuestionResult = AskUserInteger("Quel est ton nombre favoris ?");
        if (null == integerQuestionResult)
            Console.WriteLine("woops !");
        else
            Console.WriteLine($"Super, ton nombre favoris est {integerQuestionResult} !");

        // Réinitialisation des paramètres de la console pour la suite du code
        Console.ResetColor();
        Console.ReadKey(true);
    }

    /// <summary>
    /// Pose une question avec une réponse vraie / fausse
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
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
            PrintError(e);
        }

        if (defaultResult.Value)
            return defaultResult;
        return null;
    }

    /// <summary>
    /// Pose une question avec une réponse sous la forme d'une chaîne de caractères
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
    public static string AskUserString(string question, string defaultResult = "")
    {
        try
        {
            Console.WriteLine(question);

            // Gestion de la chaîne de caractères pour afficher la valeur par défaut si elle existe
            string defaultResultInfo = (string.IsNullOrEmpty(defaultResult)) ? $"[{defaultResult}]" : "";

            Console.Write($"{defaultResultInfo} > ");
            string input = Console.ReadLine();
            Console.WriteLine();

            // Si la chaîne est vide, renvoie le résultat par défaut ou repose la question
            if (string.IsNullOrEmpty(input))
                return string.IsNullOrEmpty(defaultResult) ? defaultResult : AskUserString(question, defaultResult);

            return input;
        }
        catch (Exception e)
        {
            PrintError(e);
        }

        return null;
    }

    /// <summary>
    /// Pose une question avec une réponse sous la forme d'un nombre
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
    public static int? AskUserInteger(string question, int? defaultResult = null)
    {
        try
        {
            Console.WriteLine(question);

            // Gestion de la chaîne de caractères pour afficher la valeur par défaut si elle existe
            string defaultResultInfo = (null == defaultResult) ? "" : $"[{defaultResult}]";

            Console.Write($"{defaultResultInfo} > ");
            string input = Console.ReadLine();
            Console.WriteLine();

            // Si la chaîne est vide, renvoie le résultat par défaut ou repose la question
            if (string.IsNullOrEmpty(input))
                return defaultResult.HasValue ? defaultResult.Value : AskUserInteger(question, defaultResult);

            // Si la chaîne n'est pas un nombre, repose la question
            if (!int.TryParse(input, out int result))
                return AskUserInteger(question, defaultResult);

            return result;
        }
        catch (Exception e)
        {
            PrintError(e);
        }

        return null;
    }

    /// <summary>
    /// Affiche une erreur dans la console
    /// </summary>
    /// <param name="e">Exception</param>
    public static void PrintError(Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.GetType().FullName);
        Console.WriteLine("Une exception a été levée au cours du traitement : " + e.Message);
        Console.ResetColor();
    }
}
