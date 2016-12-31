%namespace Asap2
%scannertype Asap2Scanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers, unicode, codePage:utf-8

Identifier      [A-Za-z_][A-Za-z0-9\._]+[\[A-Za-z0-9\]]*
Space           [ \t]
Decimal         [\-+]?[0-9]*\.?[0-9]+([eE][\-+]?[0-9]+)?
HexNumber       0x[0-9A-Fa-f]+
Eol             (\r?\n)
Alignment       ALIGNMENT\_([A-Za-z0-9_]+)

/* Single and Multiline comments */
CommentStart	\/\*
CommentEnd		\*\/
LineComment		"//".*

%x STATE_STRING
%x STATE_IF_DATA
%x STATE_A2ML
%x CMMT     // Inside a multi line comment.
%x CMMT2    // Inside a single line comment.

%{
%}

%%

/* Scanner body */


{Space}+        /* skip */

\/begin                         { return (int)Token.BEGIN; }
\/end                           { return (int)Token.END; }
A2ML_VERSION                    { return (int)Token.A2ML_VERSION; }
ASAP2_VERSION                   { return (int)Token.ASAP2_VERSION; }
ADDR_EPK                        { return (int)Token.ADDR_EPK; }
{Alignment}                     { GetAlignment(); return (int)Token.ALIGNMENT; }
ANNOTATION                      { return (int)Token.ANNOTATION; }
ANNOTATION_LABEL                { return (int)Token.ANNOTATION_LABEL; }
ANNOTATION_ORIGIN               { return (int)Token.ANNOTATION_ORIGIN; }
ANNOTATION_TEXT                 { return (int)Token.ANNOTATION_TEXT; }
ARRAY_SIZE                      { return (int)Token.ARRAY_SIZE; }
BIT_MASK                        { return (int)Token.BIT_MASK; }
BIT_OPERATION                   { return (int)Token.BIT_OPERATION; }
CALIBRATION_ACCESS              { return (int)Token.CALIBRATION_ACCESS; }
CALIBRATION_METHOD              { return (int)Token.CALIBRATION_METHOD; }
CALIBRATION_HANDLE              { return (int)Token.CALIBRATION_HANDLE; }
CALIBRATION_HANDLE_TEXT         { return (int)Token.CALIBRATION_HANDLE_TEXT; }
COMPU_METHOD                    { return (int)Token.COMPU_METHOD; }
COMPU_TAB_REF                   { return (int)Token.COMPU_TAB_REF; }
COMPU_TAB                       { return (int)Token.COMPU_TAB; }
COMPU_VTAB                      { return (int)Token.COMPU_VTAB; }
COMPU_VTAB_RANGE                { return (int)Token.COMPU_VTAB_RANGE; }
COEFFS                          { return (int)Token.COEFFS; }
COEFFS_LINEAR                   { return (int)Token.COEFFS_LINEAR; }
CPU_TYPE                        { return (int)Token.CPU_TYPE; }
CUSTOMER                        { return (int)Token.CUSTOMER; }
CUSTOMER_NO                     { return (int)Token.CUSTOMER_NO; }
DEFAULT_VALUE                   { return (int)Token.DEFAULT_VALUE; }
DEFAULT_VALUE_NUMERIC           { return (int)Token.DEFAULT_VALUE_NUMERIC; }
DISPLAY_IDENTIFIER              { return (int)Token.DISPLAY_IDENTIFIER; }
DISCRETE                        { return (int)Token.DISCRETE; }
ECU                             { return (int)Token.ECU; }
ECU_CALIBRATION_OFFSET          { return (int)Token.ECU_CALIBRATION_OFFSET; }
ERROR_MASK                      { return (int)Token.ERROR_MASK; }
FUNCTION_LIST                   { return (int)Token.FUNCTION_LIST; }
EPK                             { return (int)Token.EPK; }
FORMULA                         { return (int)Token.FORMULA; }
FORMULA_INV                     { return (int)Token.FORMULA_INV; }
REF_UNIT                        { return (int)Token.REF_UNIT; }
LAYOUT                          { return (int)Token.LAYOUT; }
MAX_REFRESH                     { return (int)Token.MAX_REFRESH; }
PHYS_UNIT                       { return (int)Token.PHYS_UNIT; }
READ_WRITE                      { return (int)Token.READ_WRITE; }
RIGHT_SHIFT                     { return (int)Token.RIGHT_SHIFT; }
LEFT_SHIFT                      { return (int)Token.LEFT_SHIFT; }
S_REC_LAYOUT                    { return (int)Token.S_REC_LAYOUT; }
SIGN_EXTEND                     { return (int)Token.SIGN_EXTEND; }
ECU_ADDRESS                     { return (int)Token.ECU_ADDRESS; }
ECU_ADDRESS_EXTENSION           { return (int)Token.ECU_ADDRESS_EXTENSION; }
PROJECT                         { return (int)Token.PROJECT; }
HEADER                          { return (int)Token.HEADER; }
MODULE                          { return (int)Token.MODULE; }
MOD_COMMON                      { return (int)Token.MOD_COMMON; }
MEMORY_SEGMENT                  { return (int)Token.MEMORY_SEGMENT; }
MEMORY_LAYOUT                   { return (int)Token.MEMORY_LAYOUT; }
REF_MEMORY_SEGMENT              { return (int)Token.REF_MEMORY_SEGMENT; }
SYMBOL_LINK                     { return (int)Token.SYMBOL_LINK; }
VIRTUAL                         { return (int)Token.VIRTUAL; }
MOD_PAR                         { return (int)Token.MOD_PAR; }
NO_OF_INTERFACES                { return (int)Token.NO_OF_INTERFACES; }
PHONE_NO                        { return (int)Token.PHONE_NO; }
SUPPLIER                        { return (int)Token.SUPPLIER; }
SYSTEM_CONSTANT                 { return (int)Token.SYSTEM_CONSTANT; }
USER                            { return (int)Token.USER; }
DEPOSIT                         { return (int)Token.DEPOSIT; }
BYTE_ORDER                      { return (int)Token.BYTE_ORDER; }
DATA_SIZE                       { return (int)Token.DATA_SIZE; }
VERSION                         { return (int)Token.VERSION; }
PROJECT_NO                      { return (int)Token.PROJECT_NO; }
MEASUREMENT                     { return (int)Token.MEASUREMENT; }
CHARACTERISTIC                  { return (int)Token.CHARACTERISTIC; }
FORMAT                          { return (int)Token.FORMAT; }
MATRIX_DIM                      { return (int)Token.MATRIX_DIM; }
GROUP                           { return (int)Token.GROUP; }
SUB_GROUP                       { return (int)Token.SUB_GROUP; }
REF_CHARACTERISTIC              { return (int)Token.REF_CHARACTERISTIC; }
REF_MEASUREMENT                 { return (int)Token.REF_MEASUREMENT; }
ROOT                            { return (int)Token.ROOT; }

{Identifier}    { yylval.s = yytext; return (int)Token.IDENTIFIER; }
{HexNumber}     {
    yylval.s = yytext;
    var tmp = yytext;
    if (tmp.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
    {
        tmp = tmp.Substring(2);
    }
    yylval.d = long.Parse(tmp, NumberStyles.HexNumber);
    return (int)Token.NUMBER;
}

{Decimal}       {
    yylval.s = yytext;
    decimal.TryParse(yytext, NumberStyles.Float, CultureInfo.InvariantCulture, out yylval.d);
    return (int)Token.NUMBER;
}

"\/begin IF_DATA"   { yy_push_state (STATE_IF_DATA); yylval.sb = new StringBuilder(); }
<STATE_IF_DATA> {
    "\/end IF_DATA" { yy_pop_state(); yylval.s = yylval.sb.ToString(); return (int)Token.IF_DATA; }
    \\.             { yylval.sb.Append(yytext); }
    \r?\n           { yylval.sb.Append("\r\n"); }
    .               { yylval.sb.Append(yytext); }
    <<EOF>>         { ; /* raise an error. */ }
}

"\/begin A2ML"      { yy_push_state (STATE_A2ML); yylval.sb = new StringBuilder(); }
<STATE_A2ML> {
    "\/end A2ML"    { yy_pop_state(); yylval.s = yylval.sb.ToString(); return (int)Token.A2ML; }
    \\.             { yylval.sb.Append(yytext); }
    \r?\n           { yylval.sb.Append("\r\n"); }
    .               { yylval.sb.Append(yytext); }
    <<EOF>>         { ; /* raise an error. */ }
}

\"                  { yy_push_state(STATE_STRING); yylval.sb = new StringBuilder(); }
<STATE_STRING> {
    \"              { yy_pop_state(); yylval.s = yylval.sb.ToString(); return (int)Token.QUOTED_STRING; }
    \r?\n           { yylval.sb.Append("\r\n"); }
    \\r             { yylval.sb.Append("\r"); }
    \\n             { yylval.sb.Append("\n"); }
    \\t             { yylval.sb.Append("\t"); }
    \\\"            { yylval.sb.Append("\""); }
    \"\"            { yylval.sb.Append("\""); }
    \\              { yylval.sb.Append("\\"); }
    \\.             { yylval.sb.Append(yytext); }
    .               { yylval.sb.Append(yytext); }
    <<EOF>>         { ; /* raise an error. */ }
}

/* Single line comment handler. */
{LineComment}+      { yy_push_state(CMMT2); }
<CMMT2>{
    {Eol}           { yy_pop_state(); }
}

/* Move to a 'comment' state on seeing comments. */
{CommentStart}      {  yy_push_state(CMMT); }

/* Inside a block comment. */
<CMMT>{
    [^*\n]+         /* eat up '*'s not followed by '/'s */
    "*"             /* eat anything that's not a '*' */
    {CommentEnd}    { yy_pop_state(); }
    <<EOF>>         { ; /* raise an error. */ }
}

%%