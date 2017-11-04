using System;
using ActiproSoftware.SyntaxEditor;
using ActiproSoftware.SyntaxEditor.Addons.Dynamic;

namespace EngineManager {

	/// <summary>
	/// Provides an implementation of a <c>JSON</c> syntax language that can perform automatic outlining.
	/// </summary>
	public class JsonDynamicSyntaxLanguage : DynamicOutliningSyntaxLanguage {
		
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// This constructor is for designer use only and should never be called by your code.
		/// </summary>
		public JsonDynamicSyntaxLanguage() : base() {}

		/// <summary>
		/// Initializes a new instance of the <c>JsonDynamicSyntaxLanguage</c> class. 
		/// </summary>
		/// <param name="key">The key of the language.</param>
		/// <param name="secure">Whether the language is secure.</param>
		public JsonDynamicSyntaxLanguage(string key, bool secure) : base(key, secure) {}

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
		public override void GetTokenOutliningAction(TokenStream tokenStream, ref string outliningKey, ref OutliningNodeAction tokenAction) {
			// Get the token
			IToken token = tokenStream.Peek();

			// See if the token starts or ends an outlining node
			switch (token.Key) {
				case "OpenCurlyBraceToken":
					outliningKey = "CodeBlock";
					tokenAction = OutliningNodeAction.Start;
					break;
				case "CloseCurlyBraceToken":
					outliningKey = "CodeBlock";
					tokenAction = OutliningNodeAction.End;
					break;
			}
		}
		
	}
}
