namespace Anvil.Unity.Editor.UI
{
    /// <summary>
    /// The result of a <see cref="EditorInputDialog"/> being shown.
    /// </summary>
    public class EditorInputDialogResult
    {
        /// <summary>
        /// Represents the action the user took with the dialog
        /// </summary>
        public enum ResultAction
        {
            //Ok or the affirmative action
            Ok,
            //Cancel or the negative action
            Cancel
        }

        /// <summary>
        /// The <see cref="ResultAction"/> the user took with the dialog
        /// </summary>
        public readonly ResultAction Result;
        /// <summary>
        /// The text entered into the input box.
        /// NOTE: This will be string.Empty if the user took the <see cref="ResultAction.Cancel"/> route.
        /// </summary>
        public readonly string ResultText;
        
        internal EditorInputDialogResult(ResultAction result, string resultText)
        {
            Result = result;
            ResultText = Result == ResultAction.Ok ? resultText : string.Empty;
        }
    }
}
