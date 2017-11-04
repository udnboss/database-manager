using System;
using ActiproSoftware.SyntaxEditor;
using ActiproSoftware.SyntaxEditor.Addons.Dynamic;

namespace EngineManager {

	/// <summary>
	/// Provides an implementation of a <c>XML</c> syntax language that can perform automatic outlining.
	/// </summary>
	public class XmlDynamicSyntaxLanguage : DynamicOutliningSyntaxLanguage {
		
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// This constructor is for designer use only and should never be called by your code.
		/// </summary>
		public XmlDynamicSyntaxLanguage() : base() {}

		/// <summary>
		/// Initializes a new instance of the <c>XmlDynamicSyntaxLanguage</c> class. 
		/// </summary>
		/// <param name="key">The key of the language.</param>
		/// <param name="secure">Whether the language is secure.</param>
		public XmlDynamicSyntaxLanguage(string key, bool secure) : base(key, secure) {}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// NON-PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Completes the element tag at the specified offset.
		/// </summary>
		/// <param name="syntaxEditor">The <see cref="SyntaxEditor"/> to examine.</param>
		/// <param name="offset">The offset at which to base the context.</param>
		private void CompleteElementTag(SyntaxEditor syntaxEditor, int offset) {
			string elementName = null;

			// Ensure the previous token is a > and not a />
			TextStream stream = syntaxEditor.Document.GetTextStream(offset);
			if (!stream.GoToPreviousToken())
				return;
			if ((stream.Token.Key != "StartTagEndToken") || (offset - stream.Offset != 1))
				return;

			// Search backward for a start element name
			bool exitLoop = false;
			while (stream.GoToPreviousToken()) {
				switch (stream.Token.Key) {
					case "StartTagNameToken":
						elementName = stream.TokenText.Trim();
						exitLoop = true;
						break;
					case "StartTagStartToken":
					case "EndTagEndToken":
						return;
				}
				if (exitLoop)
					break;
			}

			// Quit if no element name was found
			if (elementName == null)
				return;

			// Search forward to ensure that the next element is not an end element for the same element
			stream.Offset = offset;
			exitLoop = false;
			while (!stream.IsAtDocumentEnd) {
				switch (stream.Token.Key) {
					case "EndTagDefaultToken":
						if (elementName == stream.TokenText.Trim())
							return;
						else
							exitLoop = true;
						break;
					case "StartTagStartToken":
					case "EndTagEndToken":
						exitLoop = true;
						break;
				}
				if (exitLoop)
					break;
				stream.GoToNextToken();
			}

			// Insert the end element text
			syntaxEditor.SelectedView.InsertSurroundingText(DocumentModificationType.AutoComplete, null, "</" + elementName + ">");
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Returns token parsing information for automatic outlining that determines if the current <see cref="IToken"/>
		/// in the <see cref="TokenStream"/> starts or ends an outlining node.
		/// </summary>
		/// <param name="tokenStream">A <see cref="TokenStream"/> that is positioned at the <see cref="IToken"/> requiring outlining data.</param>
		/// <param name="outliningKey">Returns the outlining node key to assign.  A <see langword="null"/> should be returned if the token doesn't start or end a node.</param>
		/// <param name="tokenAction">Returns the <see cref="OutliningNodeAction"/> to take for the token.</param>
		public override void GetTokenOutliningAction(TokenStream tokenStream, ref string outliningKey, ref OutliningNodeAction tokenAction) {}

		/// <summary>
		/// Occurs after a <see cref="Trigger"/> is activated
		/// for a <see cref="SyntaxEditor"/> that has a <see cref="Document"/> using this language.
		/// </summary>
		/// <param name="syntaxEditor">The <see cref="SyntaxEditor"/> that will raise the event.</param>
		/// <param name="e">An <c>TriggerEventArgs</c> that contains the event data.</param>
		protected override void OnSyntaxEditorTriggerActivated(SyntaxEditor syntaxEditor, TriggerEventArgs e) {
			switch (e.Trigger.Key) {
				case "TagAutoCompleteTrigger": {
					if (!syntaxEditor.SelectedView.Selection.IsReadOnly) {
						// Complete an element tag if appropriate
						this.CompleteElementTag(syntaxEditor, syntaxEditor.Caret.Offset);
					}
					break;
				}
			}
		}

	}
}
