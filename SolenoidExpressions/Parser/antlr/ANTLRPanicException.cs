using System;

namespace Solenoid.Expressions.Parser.antlr
{
	/*ANTLR Translator Generator
	* Project led by Terence Parr at http://www.jGuru.com
	* Software rights: http://www.antlr.org/license.html
	*
	* $Id:$
	*/

	//
	// ANTLR C# Code Generator by Micheal Jordan
	//                            Kunle Odutola       : kunle UNDERSCORE odutola AT hotmail DOT com
	//                            Anthony Oguntimehin
	//
	// With many thanks to Eric V. Smith from the ANTLR list.
	//
	
	[Serializable]
	public class ANTLRPanicException : ANTLRException 
	{
		public ANTLRPanicException()
		{
		}

		public ANTLRPanicException(string message) : base(message)
		{
		}

		public ANTLRPanicException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
