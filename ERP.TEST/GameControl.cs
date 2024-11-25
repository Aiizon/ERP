namespace ERP.TEST;

public abstract class GameControl
{
    public const bool DEBUG               = false;
    public const bool LOG                 = false;
    public const ConsoleColor COLOR_DEBUG = ConsoleColor.Magenta;
    public const ConsoleColor CONSOLE_LOG = ConsoleColor.Yellow;

    #region input
    /// <summary>
    /// Pose une question avec une réponse vraie / fausse
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
    public abstract bool? AskUserBool(string question, bool? defaultResult = null);

    /// <summary>
    /// Pose une question avec une réponse sous la forme d'u caractère
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
    public abstract char? AskUserChar(string question, char? defaultResult = null);

    /// <summary>
    /// Pose une question avec une réponse sous la forme d'une chaîne de caractères
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
    public abstract string? AskUserString(string question, string? defaultResult = "");

    /// <summary>
    /// Pose une question avec une réponse sous la forme d'un nombre
    /// </summary>
    /// <param name="question">Question</param>
    /// <param name="defaultResult">Résultat par défaut</param>
    /// <returns>Réponse</returns>
    public abstract int? AskUserInteger(string question, int? defaultResult = null);
    #endregion

    #region output
    /// <summary>
    /// Affiche une erreur dans la console
    /// </summary>
    /// <param name="e">Exception</param>
    public abstract void PrintError(Exception e);

    /// <summary>
    /// Affiche un message multiligne avec titre
    /// </summary>
    /// <param name="title">Titre</param>
    /// <param name="messages">Corps du message</param>
    /// <param name="titleColor">Couleur du titre</param>
    /// <param name="messageColor">Couleur du message</param>
    public abstract void Display(string title, IEnumerable<string> messages, ConsoleColor titleColor = ConsoleColor.Green, ConsoleColor messageColor = ConsoleColor.Gray);

    // ???????????
    /// <summary>
    /// Affiche un message sur une ligne avec titre
    /// Utilise GameControl.Display()
    /// </summary>
    /// <param name="title">Titre</param>
    /// <param name="message">Corps du message</param>
    /// <param name="titleColor">Couleur du titre</param>
    /// <param name="messageColor">Couleur du message</param>
    public virtual void Display(string title, string message, ConsoleColor titleColor = ConsoleColor.Green, ConsoleColor messageColor = ConsoleColor.Gray)
    {
        Display(title, new List<string>([message]), titleColor, messageColor);
    }

    /// <summary>
    /// Affiche un message de débogage avec titre
    /// </summary>
    /// <param name="title">Titre</param>
    /// <param name="messages">Corps du message</param>
    public virtual void DisplayDebug(string title, IEnumerable<string> messages)
    {
        if (DEBUG)
            Display(title, messages, COLOR_DEBUG);
    }

    /// <summary>
    /// Affiche un message de journalisation avec titre
    /// </summary>
    /// <param name="title">Titre</param>
    /// <param name="messages">Corps du message</param>
    public virtual void DisplayLog(string title, IEnumerable<string> messages)
    {
        if (LOG)
            Display(title, messages, CONSOLE_LOG);
    }
    #endregion
}

public class ConsoleControl : GameControl
{
    #region input
    public override bool? AskUserBool(string question, bool? defaultResult = null)
    {
        try
        {
            Display(question, "");

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

        if (defaultResult != null && defaultResult.Value)
            return defaultResult;
        return null;
    }

    public override char? AskUserChar(string question, char? defaultResult = null)
    {
        try
        {
            Console.WriteLine(question);

            // Gestion de la chaîne de caractères pour afficher la valeur par défaut si elle existe
            string defaultResultInfo = (null == defaultResult) ? "" : $"[{defaultResult}]";

            Console.Write($"{defaultResultInfo} > ");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine();

            // Récupération du caractère entré
            char key = input.KeyChar;

            // Si le caractère n'est pas un retour à la ligne ou une tabulation, renvoie le résultat - sinon, repose la question
            return (key != '\r' && key != '\n' && key != '\t') ? key : AskUserChar(question, defaultResult);
        }
        catch (Exception e)
        {
            PrintError(e);
        }

        return null;
    }

    public override string? AskUserString(string question, string? defaultResult = "")
    {
        try
        {
            Console.WriteLine(question);

            // Gestion de la chaîne de caractères pour afficher la valeur par défaut si elle existe
            string defaultResultInfo = (string.IsNullOrEmpty(defaultResult)) ? $"[{defaultResult}]" : "";

            Console.Write($"{defaultResultInfo} > ");
            string? input = Console.ReadLine();
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

    public override int? AskUserInteger(string question, int? defaultResult = null)
    {
        try
        {
            Console.WriteLine(question);

            // Gestion de la chaîne de caractères pour afficher la valeur par défaut si elle existe
            string defaultResultInfo = (null == defaultResult) ? "" : $"[{defaultResult}]";

            Console.Write($"{defaultResultInfo} > ");
            string? input = Console.ReadLine();
            Console.WriteLine();

            // Si la chaîne est vide, renvoie le résultat par défaut ou repose la question
            if (string.IsNullOrEmpty(input))
                return defaultResult ?? AskUserInteger(question, defaultResult);

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

    public override void PrintError(Exception e)
    {
        string title = e.GetType().FullName;
        string message = e.Message;
        if (DEBUG)
        {
            DisplayDebug(title, [message]);
        } else if (LOG)
        {
            DisplayLog(title, [message]);
        }
    }
    #endregion

    #region output
    public static void Pause(bool doClear = false)
    {
        Console.WriteLine("...");
        Console.ReadKey(true);
        if (doClear)
            Console.Clear();
    }

    public override void Display(string title, IEnumerable<string> messages, ConsoleColor titleColor, ConsoleColor messageColor)
    {
        // Affiche le titre s'il existe
        if (!string.IsNullOrEmpty(title))
        {
            Console.ForegroundColor = titleColor;
            Console.WriteLine(title);
        }

        // Vérifie que la IEnumerablee de message ne soit pas vide
        if (null != messages && messages.Any())
        {
            // Affiche tous les messages
            Console.ForegroundColor = messageColor;
            foreach (string message in messages)
                Console.WriteLine(message);
        }

        Console.ResetColor();
    }
    #endregion
}