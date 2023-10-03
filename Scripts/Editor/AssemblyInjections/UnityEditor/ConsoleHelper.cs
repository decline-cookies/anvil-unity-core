using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Anvil.Unity.Editor.Debugging
{
    public static class ConsoleHelper
    {
        // TODO: #117
        // Should one wish to extend this functionality, adding this as a custom button overlaid on the Console Window
        // is probably helpful and better than a tools menu.
        // Additional features would be the ability to keep track of which error we are on and be able to go next/back
        // as well as first and last. 
        
        //Had to figure this out from old Unity decompiled code. Not sure where this is defined today sadly.
        //But it's unlikely to ever change as so much would have to be changed in the Unity Editor to accomodate.
        private const int ERROR_FLAG = 512;
        
        [MenuItem("Anvil/Debug/Skip to First Error")]
        internal static void SkipToFirstError()
        {
            //If we're not showing errors in the console window, we can't really scroll to it can we?
            if ((LogEntries.consoleFlags & ERROR_FLAG) == 0)
            {
                //Let the user decide...
                if (EditorUtility.DisplayDialog(
                    "Incorrect Configuration",
                    "Errors are not being show in the Console currently. Do you want them to be enabled and try again?",
                    "Enable",
                    "Cancel"))
                {
                    LogEntries.SetConsoleFlag(ERROR_FLAG, true);
                }
                else
                {
                    return;
                }
            }
            
            int errorCount = 0;
            int warningCount = 0;
            int defaultCount = 0;
            LogEntries.GetCountsByType(ref errorCount, ref warningCount, ref defaultCount);

            //No point doing any work if there aren't any errors to scroll to.
            if (errorCount == 0)
            {
                return;
            }

            Type consoleWindowType = typeof(ConsoleWindow);
            ConsoleWindow consoleWindowInstance = (ConsoleWindow)consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            ListViewState listViewState = (ListViewState)consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(consoleWindowInstance);

            int numLogEntries = LogEntries.GetCount();
            LogEntry outputLogEntry = new LogEntry();

            LogEntries.StartGettingEntries();
            for (int i = 0; i < numLogEntries; ++i)
            {
                LogEntries.GetEntryInternal(i, outputLogEntry);

                if (!IsError(outputLogEntry.mode))
                {
                    continue;
                }

                Vector2 scrollPos = listViewState.scrollPos;
                scrollPos.y = i * listViewState.rowHeight;
                listViewState.scrollPos = scrollPos;
                ConsoleWindow.ShowConsoleRow(i);
                break;
            }
            LogEntries.EndGettingEntries();
        }

        private static bool IsError(int mode)
        {
            return (mode
                    & ((int)ConsoleWindow.Mode.Fatal
                        | (int)ConsoleWindow.Mode.Assert
                        | (int)ConsoleWindow.Mode.Error
                        | (int)ConsoleWindow.Mode.ScriptCompileError
                        | (int)ConsoleWindow.Mode.ScriptingError
                        | (int)ConsoleWindow.Mode.AssetImportError
                        | (int)ConsoleWindow.Mode.ScriptingAssertion
                        | (int)ConsoleWindow.Mode.ScriptingException
                        | (int)ConsoleWindow.Mode.ReportBug
                        | (int)ConsoleWindow.Mode.StickyError))
                != 0;
        }
    }
}
