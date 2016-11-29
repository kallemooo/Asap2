%namespace Asap2
%scannertype Asap2Scanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Identifier      [A-Za-z][A-Za-z0-9_]+
Space           [ \t]
Number          [\-]?[0-9]+
HexNumber		0x[0-9A-Fa-f]+
Eol             (\r\n?|\n)

%x STATE_STRING
%x COMMENT
%x foo

%{
public int line_num = 1;
public int chars_num = 1;
int comment_caller;
%}

%%

{Eol}	   ++line_num;

/* Scanner body */

/* //[^\r\n]*((\r\n)|<<EOF>>) */

{Space}+		/* skip */

\/begin         { return (int)Token.BEGIN; }
\/end           { return (int)Token.END; }
ASAP2_VERSION   { return (int)Token.ASAP2_VERSION; }
PROJECT			{ return (int)Token.PROJECT; }
HEADER			{ return (int)Token.HEADER; }


{Identifier}    { yylval.s = yytext; return (int)Token.IDENTIFIER; }
{Number}		{ GetNumber(); return (int)Token.NUMBER; }
{HexNumber}		{ GetHexNumber(); return (int)Token.HEXNUMBER; }


\"              { BEGIN(STATE_STRING); yylval.s = ""; }
<STATE_STRING> {
	\"  { BEGIN(INITIAL); return (int)Token.QUOTED_STRING; }
	\\. { yylval.s += yytext; }
	.   { yylval.s += yytext; }
}


/* Comment handler. */
"/*"		{comment_caller = INITIAL;    BEGIN(COMMENT);      }

<foo>"/*"	{
			comment_caller = foo;
			BEGIN(COMMENT);
			}

<COMMENT>[^*\n]*	   /* eat anything that's not a '*' */
<COMMENT>"*"+[^*/\n]*  /* eat up '*'s not followed by '/'s */
<COMMENT>\n		   ++line_num;
<COMMENT>"*"+"/"	   BEGIN(comment_caller);

%%