//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 3.4.1.9004
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// $ANTLR 3.4.1.9004 TrussPreProcessor.g 2014-06-20 08:55:06

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 219
// Unreachable code detected.
#pragma warning disable 162
// Missing XML comment for publicly visible type or member 'Type_or_Member'
#pragma warning disable 1591
// CLS compliance checking will not be performed on 'type' because it is not visible from outside this assembly.
#pragma warning disable 3019


using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Misc;

namespace  Truss.Compiler.PreProcessor 
{
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "3.4.1.9004")]
[System.CLSCompliant(false)]
 internal  partial class TrussPreProcessorLexer : Antlr.Runtime.Lexer
{
	public const int EOF=-1;
	public const int DIGIT=4;
	public const int FIRST_LETTER=5;
	public const int IDENTIFIER=6;
	public const int KW_DEFINE=7;
	public const int KW_ELIF=8;
	public const int KW_ELSE=9;
	public const int KW_ENDIF=10;
	public const int KW_FALSE=11;
	public const int KW_IF=12;
	public const int KW_TRUE=13;
	public const int KW_UNDEF=14;
	public const int LETTER=15;
	public const int NEXT_LETTER=16;
	public const int OP_AMPERSAND_AMPERSAND=17;
	public const int OP_BAR_BAR=18;
	public const int OP_EQUALS_EQUALS=19;
	public const int OP_EXCLAMATION=20;
	public const int OP_EXCLAMATION_EQUALS=21;
	public const int OP_PAREN_CLOSE=22;
	public const int OP_PAREN_OPEN=23;
	public const int WS=24;

    // delegates
    // delegators

	public TrussPreProcessorLexer()
	{
		OnCreated();
	}

	public TrussPreProcessorLexer(ICharStream input )
		: this(input, new RecognizerSharedState())
	{
	}

	public TrussPreProcessorLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state)
	{

		OnCreated();
	}
	public override string GrammarFileName { get { return "TrussPreProcessor.g"; } }


	partial void OnCreated();
	partial void EnterRule(string ruleName, int ruleIndex);
	partial void LeaveRule(string ruleName, int ruleIndex);

	partial void EnterRule_KW_DEFINE();
	partial void LeaveRule_KW_DEFINE();

	// $ANTLR start "KW_DEFINE"
	[GrammarRule("KW_DEFINE")]
	private void mKW_DEFINE()
	{
		EnterRule_KW_DEFINE();
		EnterRule("KW_DEFINE", 1);
		TraceIn("KW_DEFINE", 1);
		try
		{
			int _type = KW_DEFINE;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:139:11: ( 'define' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:139:13: 'define'
			{
			DebugLocation(139, 13);
			Match("define"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_DEFINE", 1);
			LeaveRule("KW_DEFINE", 1);
			LeaveRule_KW_DEFINE();
		}
	}
	// $ANTLR end "KW_DEFINE"

	partial void EnterRule_KW_UNDEF();
	partial void LeaveRule_KW_UNDEF();

	// $ANTLR start "KW_UNDEF"
	[GrammarRule("KW_UNDEF")]
	private void mKW_UNDEF()
	{
		EnterRule_KW_UNDEF();
		EnterRule("KW_UNDEF", 2);
		TraceIn("KW_UNDEF", 2);
		try
		{
			int _type = KW_UNDEF;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:140:10: ( 'undef' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:140:12: 'undef'
			{
			DebugLocation(140, 12);
			Match("undef"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_UNDEF", 2);
			LeaveRule("KW_UNDEF", 2);
			LeaveRule_KW_UNDEF();
		}
	}
	// $ANTLR end "KW_UNDEF"

	partial void EnterRule_KW_TRUE();
	partial void LeaveRule_KW_TRUE();

	// $ANTLR start "KW_TRUE"
	[GrammarRule("KW_TRUE")]
	private void mKW_TRUE()
	{
		EnterRule_KW_TRUE();
		EnterRule("KW_TRUE", 3);
		TraceIn("KW_TRUE", 3);
		try
		{
			int _type = KW_TRUE;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:141:9: ( 'true' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:141:11: 'true'
			{
			DebugLocation(141, 11);
			Match("true"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_TRUE", 3);
			LeaveRule("KW_TRUE", 3);
			LeaveRule_KW_TRUE();
		}
	}
	// $ANTLR end "KW_TRUE"

	partial void EnterRule_KW_FALSE();
	partial void LeaveRule_KW_FALSE();

	// $ANTLR start "KW_FALSE"
	[GrammarRule("KW_FALSE")]
	private void mKW_FALSE()
	{
		EnterRule_KW_FALSE();
		EnterRule("KW_FALSE", 4);
		TraceIn("KW_FALSE", 4);
		try
		{
			int _type = KW_FALSE;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:142:10: ( 'false' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:142:12: 'false'
			{
			DebugLocation(142, 12);
			Match("false"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_FALSE", 4);
			LeaveRule("KW_FALSE", 4);
			LeaveRule_KW_FALSE();
		}
	}
	// $ANTLR end "KW_FALSE"

	partial void EnterRule_KW_IF();
	partial void LeaveRule_KW_IF();

	// $ANTLR start "KW_IF"
	[GrammarRule("KW_IF")]
	private void mKW_IF()
	{
		EnterRule_KW_IF();
		EnterRule("KW_IF", 5);
		TraceIn("KW_IF", 5);
		try
		{
			int _type = KW_IF;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:143:7: ( 'if' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:143:9: 'if'
			{
			DebugLocation(143, 9);
			Match("if"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_IF", 5);
			LeaveRule("KW_IF", 5);
			LeaveRule_KW_IF();
		}
	}
	// $ANTLR end "KW_IF"

	partial void EnterRule_KW_ELIF();
	partial void LeaveRule_KW_ELIF();

	// $ANTLR start "KW_ELIF"
	[GrammarRule("KW_ELIF")]
	private void mKW_ELIF()
	{
		EnterRule_KW_ELIF();
		EnterRule("KW_ELIF", 6);
		TraceIn("KW_ELIF", 6);
		try
		{
			int _type = KW_ELIF;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:144:9: ( 'elif' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:144:11: 'elif'
			{
			DebugLocation(144, 11);
			Match("elif"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_ELIF", 6);
			LeaveRule("KW_ELIF", 6);
			LeaveRule_KW_ELIF();
		}
	}
	// $ANTLR end "KW_ELIF"

	partial void EnterRule_KW_ELSE();
	partial void LeaveRule_KW_ELSE();

	// $ANTLR start "KW_ELSE"
	[GrammarRule("KW_ELSE")]
	private void mKW_ELSE()
	{
		EnterRule_KW_ELSE();
		EnterRule("KW_ELSE", 7);
		TraceIn("KW_ELSE", 7);
		try
		{
			int _type = KW_ELSE;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:145:9: ( 'else' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:145:11: 'else'
			{
			DebugLocation(145, 11);
			Match("else"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_ELSE", 7);
			LeaveRule("KW_ELSE", 7);
			LeaveRule_KW_ELSE();
		}
	}
	// $ANTLR end "KW_ELSE"

	partial void EnterRule_KW_ENDIF();
	partial void LeaveRule_KW_ENDIF();

	// $ANTLR start "KW_ENDIF"
	[GrammarRule("KW_ENDIF")]
	private void mKW_ENDIF()
	{
		EnterRule_KW_ENDIF();
		EnterRule("KW_ENDIF", 8);
		TraceIn("KW_ENDIF", 8);
		try
		{
			int _type = KW_ENDIF;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:146:10: ( 'endif' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:146:12: 'endif'
			{
			DebugLocation(146, 12);
			Match("endif"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("KW_ENDIF", 8);
			LeaveRule("KW_ENDIF", 8);
			LeaveRule_KW_ENDIF();
		}
	}
	// $ANTLR end "KW_ENDIF"

	partial void EnterRule_OP_AMPERSAND_AMPERSAND();
	partial void LeaveRule_OP_AMPERSAND_AMPERSAND();

	// $ANTLR start "OP_AMPERSAND_AMPERSAND"
	[GrammarRule("OP_AMPERSAND_AMPERSAND")]
	private void mOP_AMPERSAND_AMPERSAND()
	{
		EnterRule_OP_AMPERSAND_AMPERSAND();
		EnterRule("OP_AMPERSAND_AMPERSAND", 9);
		TraceIn("OP_AMPERSAND_AMPERSAND", 9);
		try
		{
			int _type = OP_AMPERSAND_AMPERSAND;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:148:24: ( '&&' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:148:26: '&&'
			{
			DebugLocation(148, 26);
			Match("&&"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_AMPERSAND_AMPERSAND", 9);
			LeaveRule("OP_AMPERSAND_AMPERSAND", 9);
			LeaveRule_OP_AMPERSAND_AMPERSAND();
		}
	}
	// $ANTLR end "OP_AMPERSAND_AMPERSAND"

	partial void EnterRule_OP_BAR_BAR();
	partial void LeaveRule_OP_BAR_BAR();

	// $ANTLR start "OP_BAR_BAR"
	[GrammarRule("OP_BAR_BAR")]
	private void mOP_BAR_BAR()
	{
		EnterRule_OP_BAR_BAR();
		EnterRule("OP_BAR_BAR", 10);
		TraceIn("OP_BAR_BAR", 10);
		try
		{
			int _type = OP_BAR_BAR;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:149:12: ( '||' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:149:14: '||'
			{
			DebugLocation(149, 14);
			Match("||"); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_BAR_BAR", 10);
			LeaveRule("OP_BAR_BAR", 10);
			LeaveRule_OP_BAR_BAR();
		}
	}
	// $ANTLR end "OP_BAR_BAR"

	partial void EnterRule_OP_EXCLAMATION();
	partial void LeaveRule_OP_EXCLAMATION();

	// $ANTLR start "OP_EXCLAMATION"
	[GrammarRule("OP_EXCLAMATION")]
	private void mOP_EXCLAMATION()
	{
		EnterRule_OP_EXCLAMATION();
		EnterRule("OP_EXCLAMATION", 11);
		TraceIn("OP_EXCLAMATION", 11);
		try
		{
			int _type = OP_EXCLAMATION;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:150:16: ( '!' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:150:18: '!'
			{
			DebugLocation(150, 18);
			Match('!'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_EXCLAMATION", 11);
			LeaveRule("OP_EXCLAMATION", 11);
			LeaveRule_OP_EXCLAMATION();
		}
	}
	// $ANTLR end "OP_EXCLAMATION"

	partial void EnterRule_OP_PAREN_OPEN();
	partial void LeaveRule_OP_PAREN_OPEN();

	// $ANTLR start "OP_PAREN_OPEN"
	[GrammarRule("OP_PAREN_OPEN")]
	private void mOP_PAREN_OPEN()
	{
		EnterRule_OP_PAREN_OPEN();
		EnterRule("OP_PAREN_OPEN", 12);
		TraceIn("OP_PAREN_OPEN", 12);
		try
		{
			int _type = OP_PAREN_OPEN;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:151:15: ( '(' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:151:17: '('
			{
			DebugLocation(151, 17);
			Match('('); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_PAREN_OPEN", 12);
			LeaveRule("OP_PAREN_OPEN", 12);
			LeaveRule_OP_PAREN_OPEN();
		}
	}
	// $ANTLR end "OP_PAREN_OPEN"

	partial void EnterRule_OP_PAREN_CLOSE();
	partial void LeaveRule_OP_PAREN_CLOSE();

	// $ANTLR start "OP_PAREN_CLOSE"
	[GrammarRule("OP_PAREN_CLOSE")]
	private void mOP_PAREN_CLOSE()
	{
		EnterRule_OP_PAREN_CLOSE();
		EnterRule("OP_PAREN_CLOSE", 13);
		TraceIn("OP_PAREN_CLOSE", 13);
		try
		{
			int _type = OP_PAREN_CLOSE;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:152:16: ( ')' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:152:18: ')'
			{
			DebugLocation(152, 18);
			Match(')'); 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_PAREN_CLOSE", 13);
			LeaveRule("OP_PAREN_CLOSE", 13);
			LeaveRule_OP_PAREN_CLOSE();
		}
	}
	// $ANTLR end "OP_PAREN_CLOSE"

	partial void EnterRule_OP_EXCLAMATION_EQUALS();
	partial void LeaveRule_OP_EXCLAMATION_EQUALS();

	// $ANTLR start "OP_EXCLAMATION_EQUALS"
	[GrammarRule("OP_EXCLAMATION_EQUALS")]
	private void mOP_EXCLAMATION_EQUALS()
	{
		EnterRule_OP_EXCLAMATION_EQUALS();
		EnterRule("OP_EXCLAMATION_EQUALS", 14);
		TraceIn("OP_EXCLAMATION_EQUALS", 14);
		try
		{
			int _type = OP_EXCLAMATION_EQUALS;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:153:23: ( '!=' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:153:25: '!='
			{
			DebugLocation(153, 25);
			Match("!="); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_EXCLAMATION_EQUALS", 14);
			LeaveRule("OP_EXCLAMATION_EQUALS", 14);
			LeaveRule_OP_EXCLAMATION_EQUALS();
		}
	}
	// $ANTLR end "OP_EXCLAMATION_EQUALS"

	partial void EnterRule_OP_EQUALS_EQUALS();
	partial void LeaveRule_OP_EQUALS_EQUALS();

	// $ANTLR start "OP_EQUALS_EQUALS"
	[GrammarRule("OP_EQUALS_EQUALS")]
	private void mOP_EQUALS_EQUALS()
	{
		EnterRule_OP_EQUALS_EQUALS();
		EnterRule("OP_EQUALS_EQUALS", 15);
		TraceIn("OP_EQUALS_EQUALS", 15);
		try
		{
			int _type = OP_EQUALS_EQUALS;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:154:18: ( '==' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:154:20: '=='
			{
			DebugLocation(154, 20);
			Match("=="); 


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("OP_EQUALS_EQUALS", 15);
			LeaveRule("OP_EQUALS_EQUALS", 15);
			LeaveRule_OP_EQUALS_EQUALS();
		}
	}
	// $ANTLR end "OP_EQUALS_EQUALS"

	partial void EnterRule_WS();
	partial void LeaveRule_WS();

	// $ANTLR start "WS"
	[GrammarRule("WS")]
	private void mWS()
	{
		EnterRule_WS();
		EnterRule("WS", 16);
		TraceIn("WS", 16);
		try
		{
			int _type = WS;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:157:5: ( ( ' ' | '\\t' | '\\u000C' ) )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:158:9: ( ' ' | '\\t' | '\\u000C' )
			{
			DebugLocation(158, 9);
			if (input.LA(1)=='\t'||input.LA(1)=='\f'||input.LA(1)==' ')
			{
				input.Consume();
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;
			}

			DebugLocation(159, 9);
			 _channel = Hidden; 

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("WS", 16);
			LeaveRule("WS", 16);
			LeaveRule_WS();
		}
	}
	// $ANTLR end "WS"

	partial void EnterRule_DIGIT();
	partial void LeaveRule_DIGIT();

	// $ANTLR start "DIGIT"
	[GrammarRule("DIGIT")]
	private void mDIGIT()
	{
		EnterRule_DIGIT();
		EnterRule("DIGIT", 17);
		TraceIn("DIGIT", 17);
		try
		{
			// TrussPreProcessor.g:164:16: ( '0' .. '9' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:
			{
			DebugLocation(164, 16);
			if ((input.LA(1)>='0' && input.LA(1)<='9'))
			{
				input.Consume();
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;
			}


			}

		}
		finally
		{
			TraceOut("DIGIT", 17);
			LeaveRule("DIGIT", 17);
			LeaveRule_DIGIT();
		}
	}
	// $ANTLR end "DIGIT"

	partial void EnterRule_LETTER();
	partial void LeaveRule_LETTER();

	// $ANTLR start "LETTER"
	[GrammarRule("LETTER")]
	private void mLETTER()
	{
		EnterRule_LETTER();
		EnterRule("LETTER", 18);
		TraceIn("LETTER", 18);
		try
		{
			// TrussPreProcessor.g:165:17: ( 'a' .. 'z' | 'A' .. 'Z' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:
			{
			DebugLocation(165, 17);
			if ((input.LA(1)>='A' && input.LA(1)<='Z')||(input.LA(1)>='a' && input.LA(1)<='z'))
			{
				input.Consume();
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;
			}


			}

		}
		finally
		{
			TraceOut("LETTER", 18);
			LeaveRule("LETTER", 18);
			LeaveRule_LETTER();
		}
	}
	// $ANTLR end "LETTER"

	partial void EnterRule_FIRST_LETTER();
	partial void LeaveRule_FIRST_LETTER();

	// $ANTLR start "FIRST_LETTER"
	[GrammarRule("FIRST_LETTER")]
	private void mFIRST_LETTER()
	{
		EnterRule_FIRST_LETTER();
		EnterRule("FIRST_LETTER", 19);
		TraceIn("FIRST_LETTER", 19);
		try
		{
			// TrussPreProcessor.g:166:23: ( LETTER | '_' )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:
			{
			DebugLocation(166, 23);
			if ((input.LA(1)>='A' && input.LA(1)<='Z')||input.LA(1)=='_'||(input.LA(1)>='a' && input.LA(1)<='z'))
			{
				input.Consume();
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;
			}


			}

		}
		finally
		{
			TraceOut("FIRST_LETTER", 19);
			LeaveRule("FIRST_LETTER", 19);
			LeaveRule_FIRST_LETTER();
		}
	}
	// $ANTLR end "FIRST_LETTER"

	partial void EnterRule_NEXT_LETTER();
	partial void LeaveRule_NEXT_LETTER();

	// $ANTLR start "NEXT_LETTER"
	[GrammarRule("NEXT_LETTER")]
	private void mNEXT_LETTER()
	{
		EnterRule_NEXT_LETTER();
		EnterRule("NEXT_LETTER", 20);
		TraceIn("NEXT_LETTER", 20);
		try
		{
			// TrussPreProcessor.g:167:22: ( FIRST_LETTER | DIGIT )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:
			{
			DebugLocation(167, 22);
			if ((input.LA(1)>='0' && input.LA(1)<='9')||(input.LA(1)>='A' && input.LA(1)<='Z')||input.LA(1)=='_'||(input.LA(1)>='a' && input.LA(1)<='z'))
			{
				input.Consume();
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				Recover(mse);
				throw mse;
			}


			}

		}
		finally
		{
			TraceOut("NEXT_LETTER", 20);
			LeaveRule("NEXT_LETTER", 20);
			LeaveRule_NEXT_LETTER();
		}
	}
	// $ANTLR end "NEXT_LETTER"

	partial void EnterRule_IDENTIFIER();
	partial void LeaveRule_IDENTIFIER();

	// $ANTLR start "IDENTIFIER"
	[GrammarRule("IDENTIFIER")]
	private void mIDENTIFIER()
	{
		EnterRule_IDENTIFIER();
		EnterRule("IDENTIFIER", 21);
		TraceIn("IDENTIFIER", 21);
		try
		{
			int _type = IDENTIFIER;
			int _channel = DefaultTokenChannel;
			// TrussPreProcessor.g:169:12: ( FIRST_LETTER ( NEXT_LETTER )* )
			DebugEnterAlt(1);
			// TrussPreProcessor.g:169:14: FIRST_LETTER ( NEXT_LETTER )*
			{
			DebugLocation(169, 14);
			mFIRST_LETTER(); 
			DebugLocation(169, 27);
			// TrussPreProcessor.g:169:27: ( NEXT_LETTER )*
			try { DebugEnterSubRule(1);
			while (true)
			{
				int alt1=2;
				try { DebugEnterDecision(1, false);
				int LA1_1 = input.LA(1);

				if (((LA1_1>='0' && LA1_1<='9')||(LA1_1>='A' && LA1_1<='Z')||LA1_1=='_'||(LA1_1>='a' && LA1_1<='z')))
				{
					alt1 = 1;
				}


				} finally { DebugExitDecision(1); }
				switch ( alt1 )
				{
				case 1:
					DebugEnterAlt(1);
					// TrussPreProcessor.g:
					{
					DebugLocation(169, 27);
					input.Consume();


					}
					break;

				default:
					goto loop1;
				}
			}

			loop1:
				;

			} finally { DebugExitSubRule(1); }


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
			TraceOut("IDENTIFIER", 21);
			LeaveRule("IDENTIFIER", 21);
			LeaveRule_IDENTIFIER();
		}
	}
	// $ANTLR end "IDENTIFIER"

	public override void mTokens()
	{
		// TrussPreProcessor.g:1:8: ( KW_DEFINE | KW_UNDEF | KW_TRUE | KW_FALSE | KW_IF | KW_ELIF | KW_ELSE | KW_ENDIF | OP_AMPERSAND_AMPERSAND | OP_BAR_BAR | OP_EXCLAMATION | OP_PAREN_OPEN | OP_PAREN_CLOSE | OP_EXCLAMATION_EQUALS | OP_EQUALS_EQUALS | WS | IDENTIFIER )
		int alt2=17;
		try { DebugEnterDecision(2, false);
		switch (input.LA(1))
		{
		case 'd':
			{
			int LA2_2 = input.LA(2);

			if ((LA2_2=='e'))
			{
				int LA2_3 = input.LA(3);

				if ((LA2_3=='f'))
				{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='i'))
					{
						int LA2_5 = input.LA(5);

						if ((LA2_5=='n'))
						{
							int LA2_6 = input.LA(6);

							if ((LA2_6=='e'))
							{
								int LA2_7 = input.LA(7);

								if (((LA2_7>='0' && LA2_7<='9')||(LA2_7>='A' && LA2_7<='Z')||LA2_7=='_'||(LA2_7>='a' && LA2_7<='z')))
								{
									alt2 = 17;
								}
								else
								{
									alt2 = 1;
								}
							}
							else
							{
								alt2 = 17;
							}
						}
						else
						{
							alt2 = 17;
						}
					}
					else
					{
						alt2 = 17;
					}
				}
				else
				{
					alt2 = 17;
				}
			}
			else
			{
				alt2 = 17;
			}
			}
			break;
		case 'u':
			{
			int LA2_2 = input.LA(2);

			if ((LA2_2=='n'))
			{
				int LA2_3 = input.LA(3);

				if ((LA2_3=='d'))
				{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='e'))
					{
						int LA2_5 = input.LA(5);

						if ((LA2_5=='f'))
						{
							int LA2_6 = input.LA(6);

							if (((LA2_6>='0' && LA2_6<='9')||(LA2_6>='A' && LA2_6<='Z')||LA2_6=='_'||(LA2_6>='a' && LA2_6<='z')))
							{
								alt2 = 17;
							}
							else
							{
								alt2 = 2;
							}
						}
						else
						{
							alt2 = 17;
						}
					}
					else
					{
						alt2 = 17;
					}
				}
				else
				{
					alt2 = 17;
				}
			}
			else
			{
				alt2 = 17;
			}
			}
			break;
		case 't':
			{
			int LA2_2 = input.LA(2);

			if ((LA2_2=='r'))
			{
				int LA2_3 = input.LA(3);

				if ((LA2_3=='u'))
				{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='e'))
					{
						int LA2_5 = input.LA(5);

						if (((LA2_5>='0' && LA2_5<='9')||(LA2_5>='A' && LA2_5<='Z')||LA2_5=='_'||(LA2_5>='a' && LA2_5<='z')))
						{
							alt2 = 17;
						}
						else
						{
							alt2 = 3;
						}
					}
					else
					{
						alt2 = 17;
					}
				}
				else
				{
					alt2 = 17;
				}
			}
			else
			{
				alt2 = 17;
			}
			}
			break;
		case 'f':
			{
			int LA2_2 = input.LA(2);

			if ((LA2_2=='a'))
			{
				int LA2_3 = input.LA(3);

				if ((LA2_3=='l'))
				{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='s'))
					{
						int LA2_5 = input.LA(5);

						if ((LA2_5=='e'))
						{
							int LA2_6 = input.LA(6);

							if (((LA2_6>='0' && LA2_6<='9')||(LA2_6>='A' && LA2_6<='Z')||LA2_6=='_'||(LA2_6>='a' && LA2_6<='z')))
							{
								alt2 = 17;
							}
							else
							{
								alt2 = 4;
							}
						}
						else
						{
							alt2 = 17;
						}
					}
					else
					{
						alt2 = 17;
					}
				}
				else
				{
					alt2 = 17;
				}
			}
			else
			{
				alt2 = 17;
			}
			}
			break;
		case 'i':
			{
			int LA2_2 = input.LA(2);

			if ((LA2_2=='f'))
			{
				int LA2_3 = input.LA(3);

				if (((LA2_3>='0' && LA2_3<='9')||(LA2_3>='A' && LA2_3<='Z')||LA2_3=='_'||(LA2_3>='a' && LA2_3<='z')))
				{
					alt2 = 17;
				}
				else
				{
					alt2 = 5;
				}
			}
			else
			{
				alt2 = 17;
			}
			}
			break;
		case 'e':
			{
			switch (input.LA(2))
			{
			case 'l':
				{
				switch (input.LA(3))
				{
				case 'i':
					{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='f'))
					{
						int LA2_5 = input.LA(5);

						if (((LA2_5>='0' && LA2_5<='9')||(LA2_5>='A' && LA2_5<='Z')||LA2_5=='_'||(LA2_5>='a' && LA2_5<='z')))
						{
							alt2 = 17;
						}
						else
						{
							alt2 = 6;
						}
					}
					else
					{
						alt2 = 17;
					}
					}
					break;
				case 's':
					{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='e'))
					{
						int LA2_5 = input.LA(5);

						if (((LA2_5>='0' && LA2_5<='9')||(LA2_5>='A' && LA2_5<='Z')||LA2_5=='_'||(LA2_5>='a' && LA2_5<='z')))
						{
							alt2 = 17;
						}
						else
						{
							alt2 = 7;
						}
					}
					else
					{
						alt2 = 17;
					}
					}
					break;
				default:
					alt2=17;
					break;

				}

				}
				break;
			case 'n':
				{
				int LA2_3 = input.LA(3);

				if ((LA2_3=='d'))
				{
					int LA2_4 = input.LA(4);

					if ((LA2_4=='i'))
					{
						int LA2_5 = input.LA(5);

						if ((LA2_5=='f'))
						{
							int LA2_6 = input.LA(6);

							if (((LA2_6>='0' && LA2_6<='9')||(LA2_6>='A' && LA2_6<='Z')||LA2_6=='_'||(LA2_6>='a' && LA2_6<='z')))
							{
								alt2 = 17;
							}
							else
							{
								alt2 = 8;
							}
						}
						else
						{
							alt2 = 17;
						}
					}
					else
					{
						alt2 = 17;
					}
				}
				else
				{
					alt2 = 17;
				}
				}
				break;
			default:
				alt2=17;
				break;

			}

			}
			break;
		case '&':
			{
			alt2 = 9;
			}
			break;
		case '|':
			{
			alt2 = 10;
			}
			break;
		case '!':
			{
			int LA2_2 = input.LA(2);

			if ((LA2_2=='='))
			{
				alt2 = 14;
			}
			else
			{
				alt2 = 11;
			}
			}
			break;
		case '(':
			{
			alt2 = 12;
			}
			break;
		case ')':
			{
			alt2 = 13;
			}
			break;
		case '=':
			{
			alt2 = 15;
			}
			break;
		case '\t':
		case '\f':
		case ' ':
			{
			alt2 = 16;
			}
			break;
		case 'A':
		case 'B':
		case 'C':
		case 'D':
		case 'E':
		case 'F':
		case 'G':
		case 'H':
		case 'I':
		case 'J':
		case 'K':
		case 'L':
		case 'M':
		case 'N':
		case 'O':
		case 'P':
		case 'Q':
		case 'R':
		case 'S':
		case 'T':
		case 'U':
		case 'V':
		case 'W':
		case 'X':
		case 'Y':
		case 'Z':
		case '_':
		case 'a':
		case 'b':
		case 'c':
		case 'g':
		case 'h':
		case 'j':
		case 'k':
		case 'l':
		case 'm':
		case 'n':
		case 'o':
		case 'p':
		case 'q':
		case 'r':
		case 's':
		case 'v':
		case 'w':
		case 'x':
		case 'y':
		case 'z':
			{
			alt2 = 17;
			}
			break;
		default:
			{
				NoViableAltException nvae = new NoViableAltException("", 2, 0, input, 1);
				DebugRecognitionException(nvae);
				throw nvae;
			}
		}

		} finally { DebugExitDecision(2); }
		switch (alt2)
		{
		case 1:
			DebugEnterAlt(1);
			// TrussPreProcessor.g:1:10: KW_DEFINE
			{
			DebugLocation(1, 10);
			mKW_DEFINE(); 

			}
			break;
		case 2:
			DebugEnterAlt(2);
			// TrussPreProcessor.g:1:20: KW_UNDEF
			{
			DebugLocation(1, 20);
			mKW_UNDEF(); 

			}
			break;
		case 3:
			DebugEnterAlt(3);
			// TrussPreProcessor.g:1:29: KW_TRUE
			{
			DebugLocation(1, 29);
			mKW_TRUE(); 

			}
			break;
		case 4:
			DebugEnterAlt(4);
			// TrussPreProcessor.g:1:37: KW_FALSE
			{
			DebugLocation(1, 37);
			mKW_FALSE(); 

			}
			break;
		case 5:
			DebugEnterAlt(5);
			// TrussPreProcessor.g:1:46: KW_IF
			{
			DebugLocation(1, 46);
			mKW_IF(); 

			}
			break;
		case 6:
			DebugEnterAlt(6);
			// TrussPreProcessor.g:1:52: KW_ELIF
			{
			DebugLocation(1, 52);
			mKW_ELIF(); 

			}
			break;
		case 7:
			DebugEnterAlt(7);
			// TrussPreProcessor.g:1:60: KW_ELSE
			{
			DebugLocation(1, 60);
			mKW_ELSE(); 

			}
			break;
		case 8:
			DebugEnterAlt(8);
			// TrussPreProcessor.g:1:68: KW_ENDIF
			{
			DebugLocation(1, 68);
			mKW_ENDIF(); 

			}
			break;
		case 9:
			DebugEnterAlt(9);
			// TrussPreProcessor.g:1:77: OP_AMPERSAND_AMPERSAND
			{
			DebugLocation(1, 77);
			mOP_AMPERSAND_AMPERSAND(); 

			}
			break;
		case 10:
			DebugEnterAlt(10);
			// TrussPreProcessor.g:1:100: OP_BAR_BAR
			{
			DebugLocation(1, 100);
			mOP_BAR_BAR(); 

			}
			break;
		case 11:
			DebugEnterAlt(11);
			// TrussPreProcessor.g:1:111: OP_EXCLAMATION
			{
			DebugLocation(1, 111);
			mOP_EXCLAMATION(); 

			}
			break;
		case 12:
			DebugEnterAlt(12);
			// TrussPreProcessor.g:1:126: OP_PAREN_OPEN
			{
			DebugLocation(1, 126);
			mOP_PAREN_OPEN(); 

			}
			break;
		case 13:
			DebugEnterAlt(13);
			// TrussPreProcessor.g:1:140: OP_PAREN_CLOSE
			{
			DebugLocation(1, 140);
			mOP_PAREN_CLOSE(); 

			}
			break;
		case 14:
			DebugEnterAlt(14);
			// TrussPreProcessor.g:1:155: OP_EXCLAMATION_EQUALS
			{
			DebugLocation(1, 155);
			mOP_EXCLAMATION_EQUALS(); 

			}
			break;
		case 15:
			DebugEnterAlt(15);
			// TrussPreProcessor.g:1:177: OP_EQUALS_EQUALS
			{
			DebugLocation(1, 177);
			mOP_EQUALS_EQUALS(); 

			}
			break;
		case 16:
			DebugEnterAlt(16);
			// TrussPreProcessor.g:1:194: WS
			{
			DebugLocation(1, 194);
			mWS(); 

			}
			break;
		case 17:
			DebugEnterAlt(17);
			// TrussPreProcessor.g:1:197: IDENTIFIER
			{
			DebugLocation(1, 197);
			mIDENTIFIER(); 

			}
			break;

		}

	}


	#region DFA

	protected override void InitDFAs()
	{
		base.InitDFAs();
	}

 
	#endregion

}

} // namespace  Truss.Compiler.PreProcessor 