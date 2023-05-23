%namespace Asap2
%scannertype Asap2Scanner
%visibility internal
%tokentype Token

%option summary, stack, minimize, parser, verbose, persistbuffer, noembedbuffers, unicode, codePage:ISO-8859-1

%{
    // User code is all now in Asap2.Scanner.cs
%}


Identifier              [A-Za-z_][A-Za-z0-9_\.\[\]]*
Space                   [ \t\u000c]
Decimal                 [\-+]?[0-9]*\.?[0-9]+([\.]?[eE][\-+]?[0-9]+)?
HexNumber               0x[0-9A-Fa-f]+
Eol                     (\r?\n)
Alignment               ALIGNMENT_[A-Za-z0-9_]+
XYZ45                   [XYZ45]
WXYZ45                  [WXYZ45]

IncFile                 .*

/* Single and Multiline comments */
CommentStart	\/\*
CommentEnd		\*\/
LineComment		("//"[^\n]*)

%x STATE_STRING
%x STATE_IF_DATA
%x STATE_A2ML
%x ML_COMMENT
%x STATE_INCL

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
AXIS_DESCR                      { return Make(Token.AXIS_DESCR); }
AXIS_PTS                        { return Make(Token.AXIS_PTS); }
AXIS_PTS_REF                    { return Make(Token.AXIS_PTS_REF); }
AXIS_PTS_{XYZ45}                { return Make(Token.AXIS_PTS_XYZ45); }
AXIS_RESCALE_{XYZ45}            { return Make(Token.AXIS_RESCALE_XYZ45); }
DIST_OP_{XYZ45}                 { return Make(Token.DIST_OP_XYZ45); }
FIX_NO_AXIS_PTS_{XYZ45}         { return Make(Token.FIX_NO_AXIS_PTS_XYZ45); }
NO_AXIS_PTS_{XYZ45}             { return Make(Token.NO_AXIS_PTS_XYZ45); }
NO_RESCALE_{XYZ45}              { return Make(Token.NO_RESCALE_XYZ45); }
OFFSET_{XYZ45}                  { return Make(Token.OFFSET_XYZ45); }
RIP_ADDR_{WXYZ45}               { return Make(Token.RIP_ADDR_WXYZ45); }
SHIFT_OP_{XYZ45}                { return Make(Token.SHIFT_OP_XYZ45); }
SRC_ADDR_{XYZ45}                { return Make(Token.SRC_ADDR_XYZ45); }
STATIC_RECORD_LAYOUT            { return Make(Token.STATIC_RECORD_LAYOUT); }
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
CURVE_AXIS_REF                  { return Make(Token.CURVE_AXIS_REF); }
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
FNC_VALUES                      { return Make(Token.FNC_VALUES); }
FIX_AXIS_PAR                    { return Make(Token.FIX_AXIS_PAR); }
FIX_AXIS_PAR_DIST               { return Make(Token.FIX_AXIS_PAR_DIST); }
FIX_AXIS_PAR_LIST               { return Make(Token.FIX_AXIS_PAR_LIST); }
EPK                             { return Make(Token.EPK); }
FORMULA                         { return Make(Token.FORMULA); }
FORMULA_INV                     { return Make(Token.FORMULA_INV); }
FUNCTION                        { return Make(Token.FUNCTION); }
FUNCTION_VERSION                { return Make(Token.FUNCTION_VERSION); }
SUB_FUNCTION                    { return Make(Token.SUB_FUNCTION); }
DEF_CHARACTERISTIC              { return Make(Token.DEF_CHARACTERISTIC); }
IN_MEASUREMENT                  { return Make(Token.IN_MEASUREMENT); }
LOC_MEASUREMENT                 { return Make(Token.LOC_MEASUREMENT); }
OUT_MEASUREMENT                 { return Make(Token.OUT_MEASUREMENT); }
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
GUARD_RAILS                     { return Make(Token.GUARD_RAILS); }
HEADER                          { return Make(Token.HEADER); }
IDENTIFICATION                  { return Make(Token.IDENTIFICATION); }
MAP_LIST                        { return Make(Token.MAP_LIST); }
MAX_GRAD                        { return Make(Token.MAX_GRAD); }
MODULE                          { return Make(Token.MODULE); }
MOD_COMMON                      { return Make(Token.MOD_COMMON); }
MONOTONY                        { return Make(Token.MONOTONY); }
MEMORY_SEGMENT                  { return Make(Token.MEMORY_SEGMENT); }
MEMORY_LAYOUT                   { return Make(Token.MEMORY_LAYOUT); }
NUMBER                          { return Make(Token.NUMBER_token); }
REF_MEMORY_SEGMENT              { return Make(Token.REF_MEMORY_SEGMENT); }
RECORD_LAYOUT                   { return Make(Token.RECORD_LAYOUT); }
STATUS_STRING_REF               { return Make(Token.STATUS_STRING_REF); }
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
FRAME_MEASUREMENT               { return Make(Token.FRAME_MEASUREMENT); }
FRAME                           { return Make(Token.FRAME); }
MATRIX_DIM                      { return Make(Token.MATRIX_DIM); }
GROUP                           { return Make(Token.GROUP); }
SUB_GROUP                       { return Make(Token.SUB_GROUP); }
REF_CHARACTERISTIC              { return Make(Token.REF_CHARACTERISTIC); }
REF_MEASUREMENT                 { return Make(Token.REF_MEASUREMENT); }
VIRTUAL_CHARACTERISTIC          { return Make(Token.VIRTUAL_CHARACTERISTIC); }
ROOT                            { return Make(Token.ROOT); }
UNIT                            { return Make(Token.UNIT); }
USER_RIGHTS                     { return Make(Token.USER_RIGHTS); }
REF_GROUP                       { return Make(Token.REF_GROUP); }
UNIT_CONVERSION                 { return Make(Token.UNIT_CONVERSION); }
SI_EXPONENTS                    { return Make(Token.SI_EXPONENTS); }
VAR_ADDRESS                     { return Make(Token.VAR_ADDRESS); }
VAR_CHARACTERISTIC              { return Make(Token.VAR_CHARACTERISTIC); }
VAR_CRITERION                   { return Make(Token.VAR_CRITERION); }
VAR_MEASUREMENT                 { return Make(Token.VAR_MEASUREMENT); }
VAR_SELECTION_CHARACTERISTIC    { return Make(Token.VAR_SELECTION_CHARACTERISTIC); }
VARIANT_CODING                  { return Make(Token.VARIANT_CODING); }
VAR_FORBIDDEN_COMB              { return Make(Token.VAR_FORBIDDEN_COMB); }
VAR_SEPERATOR                   { return Make(Token.VAR_SEPERATOR); }
VAR_NAMING                      { return Make(Token.VAR_NAMING); }
IF_DATA                         { yy_push_state (STATE_IF_DATA); yylval.sb = new StringBuilder(); }
A2ML                            { yy_push_state (STATE_A2ML); yylval.sb = new StringBuilder(); }

"\/include"                     { yy_push_state(STATE_INCL); }
\"                              { yy_push_state(STATE_STRING); yylval.sb = new StringBuilder(); }
{Identifier}                    { return Make(Token.IDENTIFIER); }
{HexNumber}                     { return MakeHexNumber(); }
{Decimal}                       { return MakeNumber(); }

<STATE_INCL>{
    {Eol}           { yy_pop_state(); TryInclude(null); }
    [ \t]               /* skip whitespace */
    [^ \t]{IncFile} { yy_pop_state(); TryInclude(yytext); }
}

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

<STATE_STRING> {
    \"              { yy_pop_state(); return MakeStringBuilder(Token.QUOTED_STRING); }
    \r?\n           { yylval.sb.Append("\r\n"); }
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

// User code is all now in Asap2.Scanner.cs