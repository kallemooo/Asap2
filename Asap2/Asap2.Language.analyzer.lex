%namespace Asap2
%scannertype Asap2Scanner
%visibility internal
%tokentype Token

%option summary, stack, minimize, parser, verbose, persistbuffer, noembedbuffers, unicode, codePage:ISO-8859-1

Identifier      [A-Za-z_][A-Za-z0-9\._]+[\[A-Za-z0-9\]]*
Space           [ \t\u000c]
Decimal         [\-+]?[0-9]*\.?[0-9]+([eE][\-+]?[0-9]+)?
HexNumber       0x[0-9A-Fa-f]+
Eol             (\r?\n)
Alignment       ALIGNMENT\_([A-Za-z0-9_]+)

/* Single and Multiline comments */
CommentStart	\/\*
CommentEnd		\*\/
LineComment		("//"[^\n]*)

%x STATE_STRING
%x STATE_IF_DATA
%x STATE_A2ML
%x ML_COMMENT

%%

/* Scanner body */


{Space}+        /* skip */

\/begin                         { return Make(Token.BEGIN); }
\/end                           { return Make(Token.END); }
A2ML_VERSION                    { return Make(Token.A2ML_VERSION); }
ASAP2_VERSION                   { return Make(Token.ASAP2_VERSION); }
ADDR_EPK                        { return Make(Token.ADDR_EPK); }
{Alignment}                     { return MakeAlignment(); }
ANNOTATION                      { return Make(Token.ANNOTATION); }
ANNOTATION_LABEL                { return Make(Token.ANNOTATION_LABEL); }
ANNOTATION_ORIGIN               { return Make(Token.ANNOTATION_ORIGIN); }
ANNOTATION_TEXT                 { return Make(Token.ANNOTATION_TEXT); }
ARRAY_SIZE                      { return Make(Token.ARRAY_SIZE); }
BIT_MASK                        { return Make(Token.BIT_MASK); }
BIT_OPERATION                   { return Make(Token.BIT_OPERATION); }
CHARACTERISTIC                  { return Make(Token.CHARACTERISTIC); }
CALIBRATION_ACCESS              { return Make(Token.CALIBRATION_ACCESS); }
CALIBRATION_METHOD              { return Make(Token.CALIBRATION_METHOD); }
CALIBRATION_HANDLE              { return Make(Token.CALIBRATION_HANDLE); }
CALIBRATION_HANDLE_TEXT         { return Make(Token.CALIBRATION_HANDLE_TEXT); }
COMPARISON_QUANTITY             { return Make(Token.COMPARISON_QUANTITY); }
COMPU_METHOD                    { return Make(Token.COMPU_METHOD); }
COMPU_TAB_REF                   { return Make(Token.COMPU_TAB_REF); }
COMPU_TAB                       { return Make(Token.COMPU_TAB); }
COMPU_VTAB                      { return Make(Token.COMPU_VTAB); }
COMPU_VTAB_RANGE                { return Make(Token.COMPU_VTAB_RANGE); }
COEFFS                          { return Make(Token.COEFFS); }
COEFFS_LINEAR                   { return Make(Token.COEFFS_LINEAR); }
CPU_TYPE                        { return Make(Token.CPU_TYPE); }
CUSTOMER                        { return Make(Token.CUSTOMER); }
CUSTOMER_NO                     { return Make(Token.CUSTOMER_NO); }
DEPENDENT_CHARACTERISTIC        { return Make(Token.DEPENDENT_CHARACTERISTIC); }
DEFAULT_VALUE                   { return Make(Token.DEFAULT_VALUE); }
DEFAULT_VALUE_NUMERIC           { return Make(Token.DEFAULT_VALUE_NUMERIC); }
DISPLAY_IDENTIFIER              { return Make(Token.DISPLAY_IDENTIFIER); }
DISCRETE                        { return Make(Token.DISCRETE); }
ECU                             { return Make(Token.ECU); }
ECU_CALIBRATION_OFFSET          { return Make(Token.ECU_CALIBRATION_OFFSET); }
ERROR_MASK                      { return Make(Token.ERROR_MASK); }
EXTENDED_LIMITS                 { return Make(Token.EXTENDED_LIMITS); }
FUNCTION_LIST                   { return Make(Token.FUNCTION_LIST); }
EPK                             { return Make(Token.EPK); }
FORMULA                         { return Make(Token.FORMULA); }
FORMULA_INV                     { return Make(Token.FORMULA_INV); }
REF_UNIT                        { return Make(Token.REF_UNIT); }
LAYOUT                          { return Make(Token.LAYOUT); }
MAX_REFRESH                     { return Make(Token.MAX_REFRESH); }
PHYS_UNIT                       { return Make(Token.PHYS_UNIT); }
READ_ONLY                       { return Make(Token.READ_ONLY); }
READ_WRITE                      { return Make(Token.READ_WRITE); }
RIGHT_SHIFT                     { return Make(Token.RIGHT_SHIFT); }
LEFT_SHIFT                      { return Make(Token.LEFT_SHIFT); }
S_REC_LAYOUT                    { return Make(Token.S_REC_LAYOUT); }
SIGN_EXTEND                     { return Make(Token.SIGN_EXTEND); }
ECU_ADDRESS                     { return Make(Token.ECU_ADDRESS); }
ECU_ADDRESS_EXTENSION           { return Make(Token.ECU_ADDRESS_EXTENSION); }
PROJECT                         { return Make(Token.PROJECT); }
HEADER                          { return Make(Token.HEADER); }
MODULE                          { return Make(Token.MODULE); }
MOD_COMMON                      { return Make(Token.MOD_COMMON); }
MEMORY_SEGMENT                  { return Make(Token.MEMORY_SEGMENT); }
MEMORY_LAYOUT                   { return Make(Token.MEMORY_LAYOUT); }
NUMBER                          { return Make(Token.NUMBER_token); }
REF_MEMORY_SEGMENT              { return Make(Token.REF_MEMORY_SEGMENT); }
STEP_SIZE                       { return Make(Token.STEP_SIZE); }
SYMBOL_LINK                     { return Make(Token.SYMBOL_LINK); }
VIRTUAL                         { return Make(Token.VIRTUAL); }
MOD_PAR                         { return Make(Token.MOD_PAR); }
NO_OF_INTERFACES                { return Make(Token.NO_OF_INTERFACES); }
PHONE_NO                        { return Make(Token.PHONE_NO); }
SUPPLIER                        { return Make(Token.SUPPLIER); }
SYSTEM_CONSTANT                 { return Make(Token.SYSTEM_CONSTANT); }
USER                            { return Make(Token.USER); }
DEPOSIT                         { return Make(Token.DEPOSIT); }
BYTE_ORDER                      { return Make(Token.BYTE_ORDER); }
DATA_SIZE                       { return Make(Token.DATA_SIZE); }
VERSION                         { return Make(Token.VERSION); }
PROJECT_NO                      { return Make(Token.PROJECT_NO); }
MEASUREMENT                     { return Make(Token.MEASUREMENT); }
FORMAT                          { return Make(Token.FORMAT); }
MATRIX_DIM                      { return Make(Token.MATRIX_DIM); }
GROUP                           { return Make(Token.GROUP); }
SUB_GROUP                       { return Make(Token.SUB_GROUP); }
REF_CHARACTERISTIC              { return Make(Token.REF_CHARACTERISTIC); }
REF_MEASUREMENT                 { return Make(Token.REF_MEASUREMENT); }
VIRTUAL_CHARACTERISTIC          { return Make(Token.VIRTUAL_CHARACTERISTIC); }
ROOT                            { return Make(Token.ROOT); }
IF_DATA                         { yy_push_state (STATE_IF_DATA); yylval.sb = new StringBuilder(); }
A2ML                            { yy_push_state (STATE_A2ML); yylval.sb = new StringBuilder(); }

{Identifier}                    { return Make(Token.IDENTIFIER); }
{HexNumber}                     { return MakeHexNumber(); }
{Decimal}                       { return MakeNumber(); }

<STATE_IF_DATA> {
    "\/end IF_DATA" { yy_pop_state(); return MakeStringBuilder(Token.IF_DATA); }
    \\.             { yylval.sb.Append(yytext); }
    \r?\n           { yylval.sb.Append("\r\n"); }
    .               { yylval.sb.Append(yytext); }
    <<EOF>>         { ; /* raise an error. */ }
}

<STATE_A2ML> {
    "\/end A2ML"    { yy_pop_state(); return MakeStringBuilder(Token.A2ML); }
    \\.             { yylval.sb.Append(yytext); }
    \r?\n           { yylval.sb.Append("\r\n"); }
    .               { yylval.sb.Append(yytext); }
    <<EOF>>         { ; /* raise an error. */ }
}

\"                  { yy_push_state(STATE_STRING); yylval.sb = new StringBuilder(); }
<STATE_STRING> {
    \"              { yy_pop_state(); return MakeStringBuilder(Token.QUOTED_STRING); }
    \r?\n           { yylval.sb.Append("\r\n"); }
    \\r             { yylval.sb.Append("\r"); }
    \\n             { yylval.sb.Append("\n"); }
    \\t             { yylval.sb.Append("\t"); }
    \\\"            { yylval.sb.Append("\""); }
    \\'             { yylval.sb.Append("\'"); }
    \"\"            { yylval.sb.Append("\""); }
    \\              { yylval.sb.Append("\\"); }
    \\.             { yylval.sb.Append(yytext); }
    .               { yylval.sb.Append(yytext); }
    <<EOF>>         { ; /* raise an error. */ }
}

/* Single line comment handler. */
{LineComment}       { return Make(Token.COMMENT); }

/* Move to a 'comment' state on seeing comments. */
{CommentStart}      { yy_push_state(ML_COMMENT); return Make(Token.COMMENT); }

/* Inside a block comment. */
<ML_COMMENT>{
    [^*\n]+         { return Make(Token.COMMENT); }
    "*"             { return Make(Token.COMMENT); }
    {CommentEnd}    { yy_pop_state(); return Make(Token.COMMENT); }
    <<EOF>>         { ; /* raise an error. */ }
}

%%
public int MakeNumber()
{
    yylval.s = yytext;
    decimal.TryParse(yytext, NumberStyles.Float, CultureInfo.InvariantCulture, out yylval.d);
    yylloc = new QUT.Gppg.LexLocation(yyline,yycol,yyline,yycol + yyleng);
    return (int)Token.NUMBER;
}

public int MakeHexNumber()
{
    yylval.s = yytext;
    var tmp = yytext;
    if (tmp.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
    {
        tmp = tmp.Substring(2);
    }
    yylval.d = long.Parse(tmp, NumberStyles.HexNumber);
    yylloc = new QUT.Gppg.LexLocation(yyline,yycol,yyline,yycol + yyleng);
    return (int)Token.NUMBER;
}

public int MakeAlignment()
{
    yylval.s = yytext;
    try
    {
        yylval.alignment_token = (ALIGNMENT.ALIGNMENT_type)Enum.Parse(typeof(ALIGNMENT.ALIGNMENT_type), yytext);        
    }
    catch (ArgumentException)
    {
        throw new Exception("Unknown ALIGNMENT type: " + yytext);
    }
    yylloc = new QUT.Gppg.LexLocation(yyline,yycol,yyline,yycol + yyleng);
    return (int)Token.ALIGNMENT;
}

public int MakeStringBuilder(Token token)
{
    yylval.s = yylval.sb.ToString();
    yylloc = new QUT.Gppg.LexLocation(yyline,yycol,yyline,yycol + yyleng);
    return (int)token;
}

public int Make(Token token)
{
    yylval.s = yytext;
    yylloc = new QUT.Gppg.LexLocation(yyline,yycol,yyline,yycol + yyleng);
    return (int)token;
}
