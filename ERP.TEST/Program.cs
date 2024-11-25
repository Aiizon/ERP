using ERP.DATA.Core;
using ERP.DATA.Databases;

namespace ERP.TEST;
internal class Program
{
    static void Main(string[] args)
    {
        DatabaseTest(args);
    }

    public static void ConsoleTest(string[] args)
    {
        // Initialise la logique de contrôle
        GameControl consoleControl = new ConsoleControl();
        consoleControl.DEBUG = true;
        consoleControl.DisplayDebug("debug", ["mode débogage activé"]);
        consoleControl.LOG = true;
        consoleControl.DisplayDebug("log", ["mode journalisation activé"]);

        // Questions de test
        #region bool

        bool? boolQuestionResult = consoleControl.AskUserBool("bool test")!.Value;
        if (null == boolQuestionResult)
            consoleControl.DisplayDebug("bool test", ["uh oh, erreur !"]);
        if (boolQuestionResult.Value)
            consoleControl.DisplayLog("bool test", ["oui"]);
        else
            consoleControl.DisplayLog("bool test", ["non"]);
        ConsoleControl.Pause(true);

        #endregion

        #region char

        char? charQuestionResult = consoleControl.AskUserChar("char test").Value;
        consoleControl.DisplayLog("char test", [charQuestionResult.ToString()]);
        ConsoleControl.Pause(true);

        #endregion

        #region string

        string? stringQuestionResult = consoleControl.AskUserString("string test");
        if (string.IsNullOrEmpty(stringQuestionResult))
            consoleControl.DisplayDebug("string test", ["uh oh, erreur !"]);
        else
            consoleControl.DisplayLog("string test", [stringQuestionResult]);
        ConsoleControl.Pause(true);

        #endregion

        #region integer

        int? integerQuestionResult = consoleControl.AskUserInteger("integer test");
        if (null == integerQuestionResult)
            consoleControl.DisplayDebug("integer test", ["uh oh, erreur !"]);
        else
            consoleControl.DisplayLog("integer test", [integerQuestionResult.ToString()]);
        ConsoleControl.Pause(true);

        #endregion
    }

    public static void DatabaseTest(string[] args)
    {
        ConsoleControl consoleControl = new ConsoleControl();
        consoleControl.DEBUG = true;
        consoleControl.DisplayDebug("debug", ["mode débogage activé"]);
        consoleControl.LOG = true;
        consoleControl.DisplayDebug("log", ["mode journalisation activé"]);

        // Test de la base de données
        MysqlDatabase database = new MysqlDatabase(new ConnectionBuilder("mysql", "localhost", "select @@version;"), 20, "wt", "kq4DEpdU5b3jsgXT7xbqknxyzhpHFpmM");
    }
}
