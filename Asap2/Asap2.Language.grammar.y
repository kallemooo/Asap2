%namespace Asap2
%partial
%parsertype Asap2Parser
%visibility internal
%tokentype Token
%YYLTYPE Location

%union { 
            public decimal d;
            public String s;
            public StringBuilder sb;
            public ALIGNMENT.ALIGNMENT_type alignment_token;
            public ALIGNMENT alignment;
            public DEPOSIT deposit;
            public BYTE_ORDER byte_order;
            public MOD_COMMON mod_common;
            public MODULE module;
            public PROJECT project;
            public HEADER header;
            public MEASUREMENT measurement;
            public ECU_ADDRESS ecu_address;
            public ECU_ADDRESS_EXTENSION ecu_address_ext;
            public IF_DATA if_data;
            public A2ML a2ml;
            public ANNOTATION annotation;
            public ANNOTATION_TEXT annotation_text;
            public ADDR_EPK addr_epk;
            public ARRAY_SIZE array_size;
            public BIT_OPERATION bit_operation;
            public CALIBRATION_ACCESS calibration_access;
            public COMPU_TAB compu_tab;
            public COMPU_VTAB compu_vtab;
            public COMPU_VTAB_RANGE compu_vtab_range;
            public MATRIX_DIM matrix_dim;
            public MEMORY_SEGMENT memory_segment;
            public MEMORY_LAYOUT memory_layout;
            public MOD_PAR mod_par;
            public CALIBRATION_METHOD calibration_method;
            public CALIBRATION_HANDLE calibration_handle;
            public FUNCTION_LIST function_list;
            public MAX_REFRESH max_refresh;
            public SYMBOL_LINK symbol_link;
            public VIRTUAL Virtual;
            public GROUP group;
            public SUB_GROUP sub_group;
            public REF_CHARACTERISTIC ref_characteristic;
            public REF_MEASUREMENT ref_measurement;
            public COMPU_METHOD compu_method;
            public FORMULA formula;
            public CHARACTERISTIC characteristic;
            public List<string> IDENTIFIER_list;
            public AXIS_DESCR axis_descr;
            public FIX_AXIS_PAR_LIST fix_axis_par_list;
            public MONOTONY monotony;
            public AXIS_PTS axis_pts;
            public RECORD_LAYOUT record_layout;
            public FUNCTION function;
            public UNIT unit;
            public USER_RIGHTS user_rights;
            public FRAME frame;
            public VARIANT_CODING variant_coding;
            public VAR_FORBIDDEN_COMB var_forbidden_comb;
            public VAR_CRITERION var_criterion;
            public VAR_CHARACTERISTIC var_characteristic;
            public VAR_ADDRESS var_address;
}

%start Asap2File

%token <d> NUMBER
%token <s> QUOTED_STRING
%token <s> IF_DATA
%token <s> IDENTIFIER

%token <s> A2ML
%token A2ML_VERSION
%token ASAP2_VERSION
%token <alignment_token> ALIGNMENT
%token ADDR_EPK
%token ANNOTATION   
%token ANNOTATION_LABEL
%token ANNOTATION_ORIGIN
%token ANNOTATION_TEXT
%token ARRAY_SIZE
%token AXIS_DESCR
%token AXIS_PTS
%token AXIS_PTS_REF
%token <s> AXIS_PTS_XYZ45
%token <s> AXIS_RESCALE_XYZ45
%token <s> DIST_OP_XYZ45
%token <s> FIX_NO_AXIS_PTS_XYZ45
%token <s> NO_AXIS_PTS_XYZ45
%token <s> NO_RESCALE_XYZ45
%token <s> OFFSET_XYZ45
%token <s> RIP_ADDR_WXYZ45
%token <s> SHIFT_OP_XYZ45
%token <s> SRC_ADDR_XYZ45
%token RESERVED
%token STATIC_RECORD_LAYOUT
%token BIT_MASK
%token BIT_OPERATION
%token COMPARISON_QUANTITY
%token CALIBRATION_ACCESS
%token CALIBRATION_METHOD
%token CALIBRATION_HANDLE
%token CALIBRATION_HANDLE_TEXT
%token COMPU_TAB
%token COMPU_VTAB
%token COMPU_VTAB_RANGE
%token COMPU_METHOD
%token COMPU_TAB_REF
%token COEFFS
%token COEFFS_LINEAR
%token CPU_TYPE
%token CURVE_AXIS_REF
%token CUSTOMER
%token CUSTOMER_NO
%token DEFAULT_VALUE
%token DEFAULT_VALUE_NUMERIC
%token DEPOSIT
%token DISPLAY_IDENTIFIER
%token DISCRETE
%token ECU
%token ECU_CALIBRATION_OFFSET
%token EPK
%token ERROR_MASK
%token EXTENDED_LIMITS
%token FORMULA
%token FORMULA_INV
%token FRAME
%token FRAME_MEASUREMENT
%token FUNCTION
%token FUNCTION_VERSION
%token SUB_FUNCTION
%token IN_MEASUREMENT
%token LOC_MEASUREMENT
%token OUT_MEASUREMENT
%token DEF_CHARACTERISTIC
%token FIX_AXIS_PAR
%token FIX_AXIS_PAR_DIST
%token FIX_AXIS_PAR_LIST
%token FNC_VALUES
%token REF_UNIT
%token UNIT_CONVERSION
%token SI_EXPONENTS
%token IDENTIFICATION
%token RIGHT_SHIFT
%token LEFT_SHIFT
%token SIGN_EXTEND
%token MATRIX_DIM
%token PROJECT
%token GUARD_RAILS
%token HEADER
%token MAX_GRAD
%token MODULE
%token MOD_COMMON
%token MOD_PAR
%token MEMORY_SEGMENT
%token MEMORY_LAYOUT
%token NUMBER_token
%token NO_OF_INTERFACES
%token BYTE_ORDER
%token DEPENDENT_CHARACTERISTIC
%token DATA_SIZE
%token S_REC_LAYOUT
%token VERSION
%token PROJECT_NO
%token PHONE_NO
%token SUPPLIER
%token SYSTEM_CONSTANT
%token STATUS_STRING_REF
%token STEP_SIZE
%token MAP_LIST
%token MEASUREMENT
%token MONOTONY
%token CHARACTERISTIC
%token ECU_ADDRESS
%token ECU_ADDRESS_EXTENSION
%token FORMAT
%token LAYOUT
%token MAX_REFRESH
%token READ_ONLY
%token READ_WRITE
%token PHYS_UNIT
%token FUNCTION_LIST
%token USER
%token REF_MEMORY_SEGMENT
%token SYMBOL_LINK
%token VIRTUAL
%token GROUP
%token SUB_GROUP
%token REF_CHARACTERISTIC
%token REF_MEASUREMENT
%token RECORD_LAYOUT
%token ROOT
%token VIRTUAL_CHARACTERISTIC
%token UNIT
%token USER_RIGHTS
%token REF_GROUP
%token VAR_ADDRESS
%token VAR_CHARACTERISTIC
%token VAR_CRITERION
%token VAR_MEASUREMENT
%token VAR_SELECTION_CHARACTERISTIC
%token VARIANT_CODING
%token VAR_FORBIDDEN_COMB
%token VAR_SEPERATOR
%token VAR_NAMING
%token BEGIN
%token END
%token maxParseToken COMMENT

%type <deposit>             deposit
%type <byte_order>          byte_order
%type <a2ml>                a2ml
%type <addr_epk>            addr_epk
%type <alignment>           alignment
%type <annotation>          annotation
%type <annotation>          annotation_data
%type <annotation_text>     annotation_text
%type <annotation_text>     annotation_text_data
%type <array_size>          array_size
%type <axis_descr>          axis_descr
%type <axis_pts>            axis_pts
%type <axis_pts>            axis_pts_data
%type <bit_operation>       bit_operation
%type <bit_operation>       bit_operation_data
%type <characteristic>      characteristic
%type <characteristic>      characteristic_data
%type <calibration_access>  calibration_access
%type <calibration_method>  calibration_method
%type <calibration_method>  calibration_method_data
%type <calibration_handle>  calibration_handle
%type <calibration_handle>  calibration_handle_data
%type <compu_method>        compu_method
%type <compu_method>        compu_method_data
%type <compu_tab>           compu_tab
%type <compu_tab>           compu_tab_data
%type <compu_vtab>          compu_vtab
%type <compu_vtab>          compu_vtab_data
%type <compu_vtab_range>    compu_vtab_range
%type <compu_vtab_range>    compu_vtab_range_data
%type <matrix_dim>          matrix_dim
%type <ecu_address>         ecu_address
%type <ecu_address_ext>     ecu_address_extension
%type <fix_axis_par_list>   fix_axis_par_list
%type <frame>               frame
%type <frame>               frame_data
%type <function>            function
%type <function>            function_data
%type <function_list>       function_list
%type <function_list>       function_list_data
%type <formula>             formula
%type <mod_common>          mod_common
%type <mod_common>          mod_common_data
%type <mod_par>             mod_par
%type <mod_par>             mod_par_data
%type <module>              module
%type <module>              module_data
%type <monotony>            monotony
%type <memory_segment>      memory_segment
%type <memory_segment>      memory_segment_data
%type <memory_layout>       memory_layout
%type <memory_layout>       memory_layout_data
%type <project>             project
%type <project>             project_data
%type <header>              header
%type <header>              header_data
%type <measurement>         measurement
%type <measurement>         measurement_data
%type <max_refresh>         max_refresh
%type <symbol_link>         symbol_link
%type <if_data>             if_data
%type <var_address>         var_address
%type <var_characteristic> var_characteristic
%type <var_criterion>       var_criterion
%type <variant_coding>      variant_coding
%type <variant_coding>      variant_coding_data
%type <var_forbidden_comb>  var_forbidden_comb
%type <Virtual>             Virtual
%type <Virtual>             Virtual_data
%type <group>               group
%type <group>               group_data
%type <sub_group>           sub_group
%type <sub_group>           sub_group_data
%type <ref_characteristic>  ref_characteristic
%type <ref_characteristic>  ref_characteristic_data
%type <ref_measurement>     ref_measurement
%type <ref_measurement>     ref_measurement_data
%type <record_layout>       record_layout
%type <record_layout>       record_layout_data
%type <unit>                unit
%type <unit>                unit_data
%type <user_rights>         user_rights
%type <user_rights>         user_rights_data
%type <s>                   default_value
%type <d>                   default_value_numeric
%type <IDENTIFIER_list>     IDENTIFIER_list

%%

Asap2File
    : /* Start of the file */
    | Asap2File project {
        Asap2File.elements.Add($2);
    }
    | Asap2File ASAP2_VERSION NUMBER NUMBER {
        Asap2File.elements.Add(new ASAP2_VERSION(@2, (uint)$3, (uint)$4));
    }
    | Asap2File A2ML_VERSION NUMBER NUMBER {
        Asap2File.elements.Add(new A2ML_VERSION(@2, (uint)$3, (uint)$4));
    }
    ;

IDENTIFIER_list
    : /* generic IDENTIFIER list handler */ {
        $$ = new List<string>();
    }
    | IDENTIFIER_list IDENTIFIER {
        $$ = $1;
        $$.Add($2);
    }
    ;

a2ml                        : BEGIN A2ML {
                                $$ = new A2ML(@$, $2);
                            }
                            ;

addr_epk                    : ADDR_EPK NUMBER {
                                $$ = new ADDR_EPK(@$, (UInt64)$2);
                            }
                            ;

alignment                   : ALIGNMENT NUMBER {
                                $$ = new ALIGNMENT(@$, $1, (uint)$2);
                            }
                            ;

annotation                  : BEGIN ANNOTATION annotation_data END ANNOTATION {
                                $$ = $3;
                            }
                            ;

annotation_data             : /* empty */ {
                                $$ = new ANNOTATION(@$);
                            }
                            | annotation_data ANNOTATION_LABEL QUOTED_STRING {
                                $$ = $1;
                                $$.annotation_label = new ANNOTATION_LABEL(@2, $3);
                            }
                            | annotation_data ANNOTATION_ORIGIN QUOTED_STRING {
                                $$ = $1;
                                $$.annotation_origin = new ANNOTATION_ORIGIN(@2, $3);
                            }
                            | annotation_data annotation_text {
                                $$ = $1;
                                $$.annotation_text = $2;
                            }
                            ;

annotation_text             : BEGIN ANNOTATION_TEXT annotation_text_data END ANNOTATION_TEXT {
                                $$ = $3;
                            }
                            ;

annotation_text_data        : /* empty */ {
                                $$ = new ANNOTATION_TEXT(@$);
                            }
                            | annotation_text_data QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add($2);
                            }
                            ;


array_size                  : ARRAY_SIZE NUMBER {
                                $$ = new ARRAY_SIZE(@1, (ulong)$2);
                            }
                            ;

axis_descr
    : IDENTIFIER IDENTIFIER IDENTIFIER NUMBER NUMBER NUMBER{
        $$ = new AXIS_DESCR(@$, attribute: (AXIS_DESCR.Attribute)EnumToStringOrAbort(typeof(AXIS_DESCR.Attribute), $1), InputQuantity: $2, Conversion: $3, MaxAxisPoints: (UInt64)$4, LowerLimit: $5, UpperLimit: $6);
    }
    |  axis_descr annotation {
        $$ = $1;
        $$.annotation.Add($2);
    }
    |  axis_descr AXIS_PTS_REF IDENTIFIER {
        $$ = $1;
        $$.axis_pts_ref = $3;
    }
    |  axis_descr byte_order {
        $$ = $1;
        $$.byte_order = $2;
    }
    |  axis_descr CURVE_AXIS_REF IDENTIFIER {
        $$ = $1;
        $$.curve_axis_ref = $3;
    }
    |  axis_descr deposit {
        $$ = $1;
        $$.deposit = $2;
    }
    |  axis_descr EXTENDED_LIMITS NUMBER NUMBER {
        $$ = $1;
        $$.extended_limits = new EXTENDED_LIMITS(@2, $3, $4);
    }
    |  axis_descr FIX_AXIS_PAR NUMBER NUMBER NUMBER {
        $$ = $1;
        $$.fix_axis_par = new FIX_AXIS_PAR(@2, (Int64)$3, (Int64)$4, (UInt64)$5);
    }
    |  axis_descr FIX_AXIS_PAR_DIST NUMBER NUMBER NUMBER {
        $$ = $1;
        $$.fix_axis_par_dist = new FIX_AXIS_PAR_DIST(@2, (Int64)$3, (Int64)$4, (UInt64)$5);
    }
    |  axis_descr BEGIN FIX_AXIS_PAR_LIST fix_axis_par_list END FIX_AXIS_PAR_LIST {
        $$ = $1;
        $$.fix_axis_par_list = $4;
    }
    |  axis_descr FORMAT QUOTED_STRING {
        $$ = $1;
        $$.format = $3;
    }
    |  axis_descr MAX_GRAD NUMBER {
        $$ = $1;
        $$.max_grad = $3;
    }
    |  axis_descr monotony {
        $$ = $1;
        $$.monotony = $2;
    }
    |  axis_descr PHYS_UNIT QUOTED_STRING {
        $$ = $1;
        $$.phys_unit = $3;
    }
    |  axis_descr READ_ONLY {
        $$ = $1;
        $$.read_only = new READ_ONLY(@2);
    }
    |  axis_descr STEP_SIZE NUMBER {
        $$ = $1;
        $$.step_size = $3;
    }
    ;

axis_pts
    : BEGIN AXIS_PTS axis_pts_data END AXIS_PTS {
        $$ = $3;
    }
    ;

axis_pts_data
    : IDENTIFIER QUOTED_STRING NUMBER IDENTIFIER IDENTIFIER NUMBER IDENTIFIER NUMBER NUMBER NUMBER {
        $$ = new AXIS_PTS(location: @$, Name: $1, LongIdentifier: $2, Address: (UInt64)$3, InputQuantity: $4, Deposit: $5, MaxDiff: $6, Conversion: $7, MaxAxisPoints: (UInt64)$8, LowerLimit: $9, UpperLimit: $10);
    }
    | axis_pts_data annotation {
        $$ = $1;
        $$.annotation.Add($2);
    }
    | axis_pts_data byte_order {
        $$ = $1;
        $$.byte_order = $2;
    }
    | axis_pts_data calibration_access {
        $$ = $1;
        $$.calibration_access = $2;
    }
    | axis_pts_data deposit {
        $$ = $1;
        $$.deposit = $2;
    }
    | axis_pts_data DISPLAY_IDENTIFIER IDENTIFIER {
        $$ = $1;
        $$.display_identifier = $3;
    }
    | axis_pts_data ecu_address_extension {
        $$ = $1;
        $$.ecu_address_extension = $2;
    }
    | axis_pts_data EXTENDED_LIMITS NUMBER NUMBER {
        $$ = $1;
        $$.extended_limits = new EXTENDED_LIMITS(@2, $3, $4);
    }
    | axis_pts_data FORMAT QUOTED_STRING {
        $$ = $1;
        $$.format = $3;
    }
    | axis_pts_data function_list {
        $$ = $1;
        $$.function_list = $2;
    }
    | axis_pts_data GUARD_RAILS {
        $$ = $1;
        $$.guard_rails = new GUARD_RAILS(@2);
    }
    | axis_pts if_data {
        $$ = $1;
        $$.if_data.Add($2);
    }
    | axis_pts_data monotony {
        $$ = $1;
        $$.monotony = $2;
    }
    | axis_pts_data PHYS_UNIT QUOTED_STRING {
        $$ = $1;
        $$.phys_unit = $3;
    }
    | axis_pts_data READ_ONLY {
        $$ = $1;
        $$.read_only = new READ_ONLY(@2);
    }
    | axis_pts_data REF_MEMORY_SEGMENT IDENTIFIER {
        $$ = $1;
        $$.ref_memory_segment = $3;
    }
    | axis_pts_data STEP_SIZE NUMBER {
        $$ = $1;
        $$.step_size = $3;
    }
    | axis_pts_data symbol_link {
        $$ = $1;
        $$.symbol_link = $2;
    }
    ;

fix_axis_par_list
    : /* empty */ {
        $$ = new FIX_AXIS_PAR_LIST(@$);
    }
    | fix_axis_par_list NUMBER {
        $$ = $1;
        $$.AxisPts_Values.Add($2);
    }
    ;

bit_operation               : BEGIN BIT_OPERATION bit_operation_data END BIT_OPERATION {
                                $$ = $3;
                            }
                            ;

bit_operation_data          : /* empty */ {
                                $$ = new BIT_OPERATION(@$);
                            }
                            | bit_operation_data RIGHT_SHIFT NUMBER {
                                $$ = $1;
                                $$.right_shift = new RIGHT_SHIFT(@2, (ulong)$3);
                            }
                            | bit_operation_data LEFT_SHIFT NUMBER  {
                                $$ = $1;
                                $$.left_shift = new LEFT_SHIFT(@2, (ulong)$3);
                            }
                            | bit_operation_data SIGN_EXTEND  {
                                $$ = $1;
                                $$.sign_extend = new SIGN_EXTEND(@2);
                            }
                            ;


calibration_access          : CALIBRATION_ACCESS IDENTIFIER {
                                $$ = new CALIBRATION_ACCESS(@1, (CALIBRATION_ACCESS.CALIBRATION_ACCESS_type)EnumToStringOrAbort(typeof(CALIBRATION_ACCESS.CALIBRATION_ACCESS_type), $2));
                            }
                            ;

                            
calibration_method          : BEGIN CALIBRATION_METHOD calibration_method_data END CALIBRATION_METHOD {
                                $$ = $3;
                            }
                            ;

calibration_method_data     : QUOTED_STRING NUMBER {
                                $$ = new CALIBRATION_METHOD(@$, $1, (ulong)$2);
                            }
                            | calibration_method_data calibration_handle {
                                $$ = $1;
                                $$.calibration_handle = $2;
                            }
                            ;

calibration_handle          : BEGIN CALIBRATION_HANDLE calibration_handle_data END CALIBRATION_HANDLE {
                                $$ = $3;
                            }
                            ;

calibration_handle_data     : NUMBER {
                                $$ = new CALIBRATION_HANDLE(@$);
                                $$.Handles.Add((Int64)$1);
                            }
                            | calibration_handle_data NUMBER {
                                $$ = $1;
                                $$.Handles.Add((Int64)$2);
                            }
                            | calibration_handle_data CALIBRATION_HANDLE_TEXT QUOTED_STRING {
                                $$ = $1;
                                $$.text = $3;
                            }
                            ;

compu_method                : BEGIN COMPU_METHOD compu_method_data END COMPU_METHOD {
                                $$ = $3;
                            }
                            ;

compu_method_data           : IDENTIFIER QUOTED_STRING IDENTIFIER QUOTED_STRING QUOTED_STRING {
                                ConversionType conversionType;  
                                conversionType = (ConversionType)EnumToStringOrAbort(typeof(ConversionType), $3);
                                $$ = new COMPU_METHOD(location: @$, Name: $1, LongIdentifier: $2, conversionType: conversionType, Format: $4, Unit: $5);
                            }
                            | compu_method_data COEFFS NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER {
                                $$ = $1;
                                $$.coeffs = new COEFFS(location: @2, a: $3, b: $4, c: $5, d: $6, e: $7, f: $8);
                            }
                            | compu_method_data COEFFS_LINEAR NUMBER NUMBER {
                                $$ = $1;
                                $$.coeffs_linear = new COEFFS_LINEAR(location: @2, a: $3, b: $4);
                            }
                            | compu_method_data COMPU_TAB_REF IDENTIFIER {
                                $$ = $1;
                                $$.compu_tab_ref = $3;
                            }
                            | compu_method_data formula {
                                $$ = $1;
                                $$.formula = $2;
                            }
                            | compu_method_data REF_UNIT IDENTIFIER {
                                $$ = $1;
                                $$.ref_unit = $3;
                            }
                            | compu_method_data STATUS_STRING_REF IDENTIFIER {
                                $$ = $1;
                                $$.status_string_ref = $3;
                            }
                            ;

formula                     : BEGIN FORMULA QUOTED_STRING END FORMULA {
                                $$ = new FORMULA(@$, $3);
                            }
                            | BEGIN FORMULA QUOTED_STRING FORMULA_INV QUOTED_STRING END FORMULA {
                                $$ = new FORMULA(@$, $3);
                                $$.formula_inv = $5;
                            }
                            ;

characteristic
    : BEGIN CHARACTERISTIC characteristic_data END CHARACTERISTIC {
        $$ = $3;
    }
    ;

characteristic_data
    : IDENTIFIER QUOTED_STRING IDENTIFIER NUMBER IDENTIFIER NUMBER IDENTIFIER NUMBER NUMBER {
        CHARACTERISTIC.Type type = (CHARACTERISTIC.Type)EnumToStringOrAbort(typeof(CHARACTERISTIC.Type), $3);
        $$ = new CHARACTERISTIC(location: @$, Name: $1, LongIdentifier: $2, type: type, Address: (UInt64)$4, Deposit: $5, MaxDiff: $6, Conversion: $7, LowerLimit: $8, UpperLimit: $9);
    }
    |  characteristic_data annotation {
        $$ = $1;
        $$.annotation.Add($2);
    }
    |  characteristic_data BEGIN AXIS_DESCR axis_descr END AXIS_DESCR {
        $$ = $1;
        $$.axis_descr.Add($4);
    }
    |  characteristic_data BIT_MASK NUMBER {
        $$ = $1;
        $$.bit_mask = (UInt64)$3;
    }
    |  characteristic_data byte_order {
        $$ = $1;
        $$.byte_order = $2;
    }
    |  characteristic_data COMPARISON_QUANTITY IDENTIFIER {
        $$ = $1;
        $$.comparison_quantity = $3;
    }
    |  characteristic_data calibration_access {
        $$ = $1;
        $$.calibration_access = $2;
    }
    |  characteristic_data BEGIN DEPENDENT_CHARACTERISTIC QUOTED_STRING IDENTIFIER_list END DEPENDENT_CHARACTERISTIC {
        $$ = $1;
        $$.dependent_characteristic = new DEPENDENT_CHARACTERISTIC(@2, $4);
        $$.dependent_characteristic.Characteristic = $5;
    }
    |  characteristic_data DISCRETE {
        $$ = $1;
        $$.discrete = new DISCRETE(@2);
    }
    |  characteristic_data DISPLAY_IDENTIFIER IDENTIFIER {
        $$ = $1;
        $$.display_identifier = $3;
    }
    |  characteristic_data ecu_address_extension {
        $$ = $1;
        $$.ecu_address_extension = $2;
    }
    |  characteristic_data FORMAT QUOTED_STRING {
        $$ = $1;
        $$.format = $3;
    }
    |  characteristic_data EXTENDED_LIMITS NUMBER NUMBER {
        $$ = $1;
        $$.extended_limits = new EXTENDED_LIMITS(@2, $3, $4);
    }
    |  characteristic_data function_list {
        $$ = $1;
        $$.function_list = $2;
    }
    |  characteristic_data GUARD_RAILS {
        $$ = $1;
        $$.guard_rails = new GUARD_RAILS(@2);
    }
    | characteristic_data if_data {
        $$ = $1;
        $$.if_data.Add($2);
    }
    |  characteristic_data BEGIN MAP_LIST IDENTIFIER_list END MAP_LIST {
        $$ = $1;
        $$.map_list = new MAP_LIST(@2);
        $$.map_list.MapList = $4;
    }
    |  characteristic_data matrix_dim {
        $$ = $1;
        $$.matrix_dim = $2;
    }
    |  characteristic_data max_refresh {
        $$ = $1;
        $$.max_refresh = $2;
    }
    |  characteristic_data NUMBER_token NUMBER {
        $$ = $1;
        $$.number = (UInt64)$3;
    }
    |  characteristic_data PHYS_UNIT QUOTED_STRING {
        $$ = $1;
        $$.phys_unit = $3;
    }
    |  characteristic_data READ_ONLY {
        $$ = $1;
        $$.read_only = new READ_ONLY(@2);
    }
    |  characteristic_data REF_MEMORY_SEGMENT IDENTIFIER {
        $$ = $1;
        $$.ref_memory_segment = $3;
    }
    |  characteristic_data STEP_SIZE NUMBER {
        $$ = $1;
        $$.step_size = $3;
    }
    |  characteristic_data symbol_link {
        $$ = $1;
        $$.symbol_link = $2;
    }
    |  characteristic_data BEGIN VIRTUAL_CHARACTERISTIC QUOTED_STRING IDENTIFIER_list END VIRTUAL_CHARACTERISTIC {
        $$ = $1;
        $$.virtual_characteristic = new VIRTUAL_CHARACTERISTIC(@2, $4);
        $$.virtual_characteristic.Characteristic = $5;
    }
    ;

compu_tab                   : BEGIN COMPU_TAB compu_tab_data END COMPU_TAB {
                                $$ = $3;
                            }
                            ;

compu_tab_data              : IDENTIFIER QUOTED_STRING IDENTIFIER NUMBER {
                                ConversionType conversionType = (ConversionType)EnumToStringOrAbort(typeof(ConversionType), $3);
                                $$ = new COMPU_TAB(location: @$, Name: $1, LongIdentifier: $2, conversionType: conversionType, NumberValuePairs: (uint)$4);
                            }
                            | compu_tab_data NUMBER NUMBER {
                                $$ = $1;
                                $$.data.Add(new COMPU_TAB_DATA(@2, $2, $3));
                            }
                            | compu_tab_data default_value {
                                $$ = $1;
                                $$.default_value = $2;
                            }
                            | compu_tab_data default_value_numeric {
                                $$ = $1;
                                $$.default_value_numeric = $2;
                            }
                            ;


compu_vtab                  : BEGIN COMPU_VTAB compu_vtab_data END COMPU_VTAB {
                                $$ = $3;
                            }
                            ;

compu_vtab_data             : IDENTIFIER QUOTED_STRING IDENTIFIER NUMBER {
                                $$ = new COMPU_VTAB(@$, Name: $1, LongIdentifier: $2, NumberValuePairs: (uint)$4);
                                if ($3 != $$.ConversionType)
                                {
                                    Scanner.yyerror("Parser warning: Unknown COMPU_VTAB ConversionType: " + $3 + " expecting: " + $$.ConversionType);
                                }
                            }
                            | compu_vtab_data NUMBER QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add(new COMPU_VTAB_DATA(@2, $2, $3));
                            }
                            | compu_vtab_data default_value {
                                $$ = $1;
                                $$.default_value = $2;
                            }
                            ;

compu_vtab_range            : BEGIN COMPU_VTAB_RANGE compu_vtab_range_data END COMPU_VTAB_RANGE {
                                $$ = $3;
                            }
                            ;

compu_vtab_range_data       : IDENTIFIER QUOTED_STRING NUMBER {
                                $$ = new COMPU_VTAB_RANGE(location: @$, Name: $1, LongIdentifier: $2, NumberValueTriples: (uint)$3);
                            }
                            | compu_vtab_range_data NUMBER NUMBER QUOTED_STRING {
                                $$ = $1;
                                $$.data.Add(new COMPU_VTAB_RANGE_DATA(@2, $2, $3, $4));
                            }
                            | compu_vtab_range_data default_value {
                                $$ = $1;
                                $$.default_value = $2;
                            }
                            ;

default_value               : DEFAULT_VALUE QUOTED_STRING {
                                $$ = $2;
                            }
                            ;

default_value_numeric       : DEFAULT_VALUE_NUMERIC NUMBER {
                                $$ = $2;
                            }
                            ;
project                     : BEGIN PROJECT project_data END PROJECT {
                                $$ = $3;
                            }
                            ;



project_data    :   IDENTIFIER QUOTED_STRING {
                    $$ = new PROJECT(@$);
                    $$.name           = $1;
                    $$.LongIdentifier = $2;
                }
                | project_data header {
                    $$ = $1;
                    $$.header = $2;
                }
                | project_data module {
                    $$ = $1;
                    try
                    {
                        $$.modules.Add($2.name, $2);
                    }
                    catch (ArgumentException)
                    {
                        Scanner.yyerror(String.Format("Syntax error: Duplicate MODULE with name '{0}' found", $2.name));
                        YYAbort();
                    }
                }
                ;

header          :   BEGIN HEADER header_data END HEADER {
                        $$ = $3;
                    }
                ;
                
header_data     : QUOTED_STRING {
                    $$ = new HEADER(@$);
                    $$.longIdentifier = $1;
                }
                | header_data VERSION QUOTED_STRING {
                    $$ = $1;
                    $$.version = $3;
                }
                | header_data PROJECT_NO IDENTIFIER {
                    $$ = $1;
                    $$.project_no = $3;
                }
                ;

module          :   BEGIN MODULE module_data END MODULE {                   
                    $$ = $3;
                }
                ;

module_data :   IDENTIFIER QUOTED_STRING {
                    $$ = new MODULE(@$);
                    $$.name = $1;
                    $$.LongIdentifier = $2;
                }
                | module_data mod_common {
                    $$ = $1;
                        $$.elements.Add($2);
                }
                | module_data measurement {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data if_data {
                    $$ = $1;
                    $$.elements.Add($2);
                }
                | module_data a2ml {
                    $$ = $1;
                        $$.elements.Add($2);
                }
                | module_data compu_method {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data compu_tab {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data compu_vtab {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data compu_vtab_range {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data group {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data mod_par {
                    $$ = $1;
                    $$.elements.Add($2);
                }
                | module_data characteristic {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data axis_pts {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data record_layout {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data function {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data unit {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data user_rights {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data frame {
                    $$ = $1;
                    try
                    {
                        $$.AddElement($2);
                    }
                    catch (ValidationErrorException e)
                    {
                        Scanner.yyerror(e.ToString());
                    }
                }
                | module_data variant_coding {
                    $$ = $1;
                    $$.elements.Add($2);
                }
                ;

if_data         : BEGIN IF_DATA {
                    $$ = new IF_DATA(@$, $2);
                }
                ;

mod_common      : BEGIN MOD_COMMON mod_common_data END MOD_COMMON {
                    $$ = $3;
                }
                ;

mod_common_data :  QUOTED_STRING {
                    $$ = new MOD_COMMON(@1, $1);
                }
                |  mod_common_data deposit {
                    $$ = $1;
                    $$.deposit    = $2;
                }
                |  mod_common_data byte_order {
                    $$ = $1;
                    $$.byte_order = $2;
                }
                |  mod_common_data DATA_SIZE NUMBER {
                    $$ = $1;
                    $$.data_size  = (UInt64)$3;
                }
                |  mod_common_data S_REC_LAYOUT IDENTIFIER {
                    $$ = $1;
                    $$.s_rec_layout  = $3;
                }
                |  mod_common_data alignment {
                    $$ = $1;
                    try
                    {
                        $$.alignments.Add($2.name, $2);
                    }
                    catch (ArgumentException)
                    {
                        Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2.name));
                    }
                }
                ;

mod_par         : BEGIN MOD_PAR mod_par_data END MOD_PAR {
                    $$ = $3;
                }
                ;

mod_par_data :  QUOTED_STRING {
                    $$ = new MOD_PAR(@$, $1);
                }
                |  mod_par_data addr_epk {
                    $$ = $1;
                    $$.addr_epk.Add($2);
                }
                |  mod_par_data calibration_method {
                    $$ = $1;
                    $$.calibration_method.Add($2);
                }
                |  mod_par_data CPU_TYPE QUOTED_STRING {
                    $$ = $1;
                    $$.cpu_type = $3;
                }
                |  mod_par_data CUSTOMER QUOTED_STRING {
                    $$ = $1;
                    $$.customer = $3;
                }
                |  mod_par_data CUSTOMER_NO QUOTED_STRING {
                    $$ = $1;
                    $$.customer_no = $3;
                }
                |  mod_par_data ECU QUOTED_STRING {
                    $$ = $1;
                    $$.ecu = $3;
                }
                |  mod_par_data ECU_CALIBRATION_OFFSET NUMBER {
                    $$ = $1;
                    $$.ecu_calibration_offset = (Int64)$3;
                }
                |  mod_par_data EPK QUOTED_STRING {
                    $$ = $1;
                    $$.epk = $3;
                }
                |  mod_par_data memory_layout {
                    $$ = $1;
                    $$.memory_layout.Add($2);
                }
                |  mod_par_data memory_segment {
                    $$ = $1;
                    try
                    {
                        $$.memory_segment.Add($2.Name, $2);
                    }
                    catch (ArgumentException)
                    {
                        Scanner.yyerror(String.Format("Warning: Duplicate MEMORY_SEGMENT with name '{0}' found, ignoring", $2.Name));
                    }
                }
                |  mod_par_data NO_OF_INTERFACES NUMBER {
                    $$ = $1;
                    $$.no_of_interfaces = (UInt64)$3;
                }
                |  mod_par_data PHONE_NO QUOTED_STRING {
                    $$ = $1;
                    $$.phone_no = $3;
                }
                |  mod_par_data SUPPLIER QUOTED_STRING {
                    $$ = $1;
                    $$.supplier = $3;
                }
                |  mod_par_data SYSTEM_CONSTANT QUOTED_STRING QUOTED_STRING {
                    $$ = $1;
                    try
                    {
                        $$.system_constants.Add($3, new SYSTEM_CONSTANT(@2, $3, $4));
                    }
                    catch (ArgumentException)
                    {
                        Scanner.yyerror(String.Format("Warning: Duplicate SYSTEM_CONSTANT with name '{0}' found, ignoring", $3));
                    }
                }
                |  mod_par_data USER QUOTED_STRING {
                    $$ = $1;
                    $$.user = $3;
                }
                |  mod_par_data VERSION QUOTED_STRING {
                    $$ = $1;
                    $$.version = $3;
                }
                ;

matrix_dim      : MATRIX_DIM NUMBER NUMBER NUMBER {
                    $$ = new MATRIX_DIM(@$, (uint)$2, (uint)$3, (uint)$4);
                }
                ;

measurement     : BEGIN MEASUREMENT measurement_data END MEASUREMENT {
                    $$ = $3;
                }
                ;

measurement_data :  IDENTIFIER QUOTED_STRING IDENTIFIER IDENTIFIER NUMBER NUMBER NUMBER NUMBER {
                    $$ = new MEASUREMENT(@$, $1, $2, (DataType)EnumToStringOrAbort(typeof(DataType), $3), $4, (uint)$5, $6, $7, $8);
                }
                |  measurement_data annotation {
                    $$ = $1;
                    $$.annotation.Add($2);
                }
                |  measurement_data array_size {
                    $$ = $1;
                    $$.array_size = $2;
                }
                |  measurement_data BIT_MASK NUMBER {
                    $$ = $1;
                    $$.bit_mask = (UInt64)$3;
                }
                |  measurement_data bit_operation {
                    $$ = $1;
                    $$.bit_operation = $2;
                }
                |  measurement_data byte_order {
                    $$ = $1;
                    $$.byte_order = $2;
                }
                |  measurement_data DISCRETE {
                    $$ = $1;
                    $$.discrete = new DISCRETE(@2);
                }
                |  measurement_data DISPLAY_IDENTIFIER IDENTIFIER {
                    $$ = $1;
                    $$.display_identifier = $3;
                }
                |  measurement_data ecu_address {
                    $$ = $1;
                    $$.ecu_address = $2;
                }
                |  measurement_data ecu_address_extension {
                    $$ = $1;
                    $$.ecu_address_extension = $2;
                }
                |  measurement_data ERROR_MASK NUMBER {
                    $$ = $1;
                    $$.error_mask = (UInt64)$3;
                }
                |  measurement_data FORMAT QUOTED_STRING {
                    $$ = $1;
                    $$.format = $3;
                }
                |  measurement_data function_list {
                    $$ = $1;
                    $$.function_list = $2;
                }
                |  measurement_data LAYOUT IDENTIFIER {
                    $$ = $1;
                    $$.layout = (MEASUREMENT.LAYOUT)EnumToStringOrAbort(typeof(MEASUREMENT.LAYOUT), $3);
                }
                |  measurement_data matrix_dim {
                    $$ = $1;
                    $$.matrix_dim = $2;
                }
                |  measurement_data max_refresh {
                    $$ = $1;
                    $$.max_refresh = $2;
                }
                |  measurement_data PHYS_UNIT QUOTED_STRING {
                    $$ = $1;
                    $$.phys_unit = $3;
                }
                |  measurement_data READ_WRITE {
                    $$ = $1;
                    $$.read_write = new READ_WRITE(@2);
                }
                |  measurement_data REF_MEMORY_SEGMENT IDENTIFIER {
                    $$ = $1;
                    $$.ref_memory_segment = $3;
                }
                |  measurement_data symbol_link {
                    $$ = $1;
                    $$.symbol_link = $2;
                }
                |  measurement_data Virtual {
                    $$ = $1;
                    $$.Virtual = $2;
                }
                | measurement_data if_data {
                    $$ = $1;
                    $$.if_data.Add($2);
                }
                ;

max_refresh     : MAX_REFRESH NUMBER NUMBER {
                    $$ = new MAX_REFRESH(@$, (UInt64)$2, (UInt64)$3);
                }
                ;

monotony
    : MONOTONY IDENTIFIER {
        $$ = new MONOTONY(@$, (MONOTONY.MONOTONY_type)EnumToStringOrAbort(typeof(MONOTONY.MONOTONY_type), $2));
    }
    ;

symbol_link     : SYMBOL_LINK QUOTED_STRING NUMBER {
                    $$ = new SYMBOL_LINK(@$, $2, (UInt64)$3);
                }
                ;

function
    : BEGIN FUNCTION function_data END FUNCTION {
        $$ = $3;
    }
    ;

function_data
    : IDENTIFIER QUOTED_STRING {
        $$ = new FUNCTION(@$, $1, $2);
    }
    |  function_data annotation {
        $$ = $1;
        $$.annotation.Add($2);
    }
    |  function_data FUNCTION_VERSION QUOTED_STRING {
        $$ = $1;
        $$.function_version = $3;
    }
    | function_data BEGIN DEF_CHARACTERISTIC IDENTIFIER_list END DEF_CHARACTERISTIC {
        $$ = $1;
        $$.def_characteristic = new DEF_CHARACTERISTIC(@$);
        $$.def_characteristic.def_characteristics = $4;
    }
    | function_data if_data {
        $$ = $1;
        $$.if_data.Add($2);
    }
    | function_data BEGIN IN_MEASUREMENT IDENTIFIER_list END IN_MEASUREMENT {
        $$ = $1;
        $$.in_measurement = new IN_MEASUREMENT(@$);
        $$.in_measurement.measurements = $4;
    }
    | function_data BEGIN LOC_MEASUREMENT IDENTIFIER_list END LOC_MEASUREMENT {
        $$ = $1;
        $$.loc_measurement = new LOC_MEASUREMENT(@$);
        $$.loc_measurement.measurements = $4;
    }
    | function_data BEGIN OUT_MEASUREMENT IDENTIFIER_list END OUT_MEASUREMENT {
        $$ = $1;
        $$.out_measurement = new OUT_MEASUREMENT(@$);
        $$.out_measurement.measurements = $4;
    }
    | function_data ref_characteristic {
        $$ = $1;
        $$.ref_characteristic = $2;
    }
    | function_data BEGIN SUB_FUNCTION IDENTIFIER_list END SUB_FUNCTION {
        $$ = $1;
        $$.sub_function = new SUB_FUNCTION(@$);
        $$.sub_function.sub_functions = $4;
    }
    ;

function_list  : BEGIN FUNCTION_LIST function_list_data END FUNCTION_LIST {
                    $$ = $3;
                }
                ;

function_list_data  : /* start */ {
                    $$ = new FUNCTION_LIST(@$);
                }
                |  function_list_data IDENTIFIER {
                    $$ = $1;
                    $$.functions.Add($2);
                }
                ;

Virtual         : BEGIN VIRTUAL Virtual_data END VIRTUAL {
                    $$ = $3;
                }
                ;

Virtual_data   : /* start */  {
                    $$ = new VIRTUAL(@$);
                }
                |  Virtual_data IDENTIFIER {
                    $$ = $1;
                    $$.MeasuringChannel.Add($2);
                }
                ;

memory_segment  : BEGIN MEMORY_SEGMENT memory_segment_data END MEMORY_SEGMENT {
                    $$ = $3;
                }
                ;

memory_segment_data : IDENTIFIER QUOTED_STRING IDENTIFIER IDENTIFIER IDENTIFIER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER {
                    MEMORY_SEGMENT.PrgType PrgType = (MEMORY_SEGMENT.PrgType)EnumToStringOrAbort(typeof(MEMORY_SEGMENT.PrgType), $3);
                    MEMORY_SEGMENT.MemoryType MemoryType = (MEMORY_SEGMENT.MemoryType)EnumToStringOrAbort(typeof(MEMORY_SEGMENT.MemoryType), $4);
                    MEMORY_SEGMENT.Attribute Attribute = (MEMORY_SEGMENT.Attribute)EnumToStringOrAbort(typeof(MEMORY_SEGMENT.Attribute), $5);
                    $$ = new MEMORY_SEGMENT(@$, $1, $2, PrgType, MemoryType, Attribute, (UInt64)$6, (UInt64)$7, (Int64)$8, (Int64)$9, (Int64)$10, (Int64)$11, (Int64)$12);
                }
                |  memory_segment_data if_data {
                    $$ = $1;
                    $$.if_data.Add($2);
                }
                ;

memory_layout       : BEGIN MEMORY_LAYOUT memory_layout_data END MEMORY_LAYOUT {
                        $$ = $3;
                    }
                    ;

memory_layout_data  : IDENTIFIER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER {
                    MEMORY_LAYOUT.PrgType PrgType = (MEMORY_LAYOUT.PrgType)EnumToStringOrAbort(typeof(MEMORY_LAYOUT.PrgType), $1);
                    $$ = new MEMORY_LAYOUT(@$, PrgType, (UInt64)$2, (UInt64)$3, (Int64)$4, (Int64)$5, (Int64)$6, (Int64)$7, (Int64)$8);
                }
                |  memory_layout_data if_data {
                    $$ = $1;
                    $$.if_data.Add($2);
                }
                ;

deposit         : DEPOSIT IDENTIFIER {
                    DEPOSIT.DEPOSIT_type type = (DEPOSIT.DEPOSIT_type)EnumToStringOrAbort(typeof(DEPOSIT.DEPOSIT_type), $2);
                    $$ = new DEPOSIT(@$, type);
                }
                ;

byte_order      : BYTE_ORDER IDENTIFIER {
                    BYTE_ORDER.BYTE_ORDER_type order = (BYTE_ORDER.BYTE_ORDER_type)EnumToStringOrAbort(typeof(BYTE_ORDER.BYTE_ORDER_type), $2);
                    $$ = new BYTE_ORDER(@$, order);
                }
                ;

ecu_address                 : ECU_ADDRESS NUMBER {
                                $$ = new ECU_ADDRESS(@$, (UInt64)$2);
                            }
                            ;

ecu_address_extension       : ECU_ADDRESS_EXTENSION NUMBER {
                                $$ = new ECU_ADDRESS_EXTENSION(@$, (UInt64)$2);
                            }
                            ;

group                       : BEGIN GROUP group_data END GROUP {
                                $$ = $3;
                            }
                            ;

group_data                  : IDENTIFIER QUOTED_STRING {
                                $$ = new GROUP(@$, $1, $2);
                            }
                            |  group_data annotation {
                                $$ = $1;
                                $$.annotation.Add($2);
                            }
                            | group_data if_data {
                                $$ = $1;
                                $$.if_data.Add($2);
                            }
                            |  group_data function_list {
                                $$ = $1;
                                $$.function_list = $2;
                            }
                            |  group_data ref_characteristic {
                                $$ = $1;
                                $$.ref_characteristic = $2;
                            }
                            |  group_data ref_measurement {
                                $$ = $1;
                                $$.ref_measurement = $2;
                            }
                            |  group_data ROOT {
                                $$ = $1;
                                $$.root = new ROOT(@2);
                            }
                            |  group_data sub_group {
                                $$ = $1;
                                $$.sub_group = $2;
                            }
                            ;

ref_characteristic          : BEGIN REF_CHARACTERISTIC ref_characteristic_data END REF_CHARACTERISTIC {
                                $$ = $3;
                            }
                            ;

ref_characteristic_data     : /* start */  {
                                $$ = new REF_CHARACTERISTIC(@$);
                            }
                            |  ref_characteristic_data IDENTIFIER {
                                $$ = $1;
                                $$.reference.Add($2);
                            }
                            ;

ref_measurement             : BEGIN REF_MEASUREMENT ref_measurement_data END REF_MEASUREMENT {
                                $$ = $3;
                            }
                            ;

ref_measurement_data        : /* start */  {
                                $$ = new REF_MEASUREMENT(@$);
                            }
                            |  ref_measurement_data IDENTIFIER {
                                $$ = $1;
                                $$.reference.Add($2);
                            }
                            ;
record_layout
    : BEGIN RECORD_LAYOUT record_layout_data END RECORD_LAYOUT {
         $$ = $3;
    }
    ;

record_layout_data
    : IDENTIFIER {
        $$ = new RECORD_LAYOUT(@$, $1);
    }
    | record_layout_data alignment {
        $$ = $1;
        try
        {
            $$.alignments.Add($2.name, $2);
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2.name));
        }
    }
    | record_layout_data AXIS_PTS_XYZ45 NUMBER IDENTIFIER IDENTIFIER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.axis_pts_xyz45.Add($2, new AXIS_PTS_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4), indexIncr: (IndexOrder)EnumToStringOrAbort(typeof(IndexOrder), $5), addrType: (AddrType)EnumToStringOrAbort(typeof(AddrType), $6)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data AXIS_RESCALE_XYZ45 NUMBER IDENTIFIER NUMBER IDENTIFIER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.axis_rescale_xyz45.Add($2, new AXIS_RESCALE_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4), MaxNoOfRescalePairs: (UInt64)$5, indexIncr: (IndexOrder)EnumToStringOrAbort(typeof(IndexOrder), $6), addrType: (AddrType)EnumToStringOrAbort(typeof(AddrType), $7)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data DIST_OP_XYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.dist_op_xyz45.Add($2, new DIST_OP_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data FIX_NO_AXIS_PTS_XYZ45 NUMBER {
        $$ = $1;
        try
        {
            $$.fix_no_axis_pts_xyz45.Add($2, new FIX_NO_AXIS_PTS_XYZ45(location: @$, Name: $2, NumberOfAxisPoints: (UInt64)$3));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data FNC_VALUES NUMBER IDENTIFIER IDENTIFIER IDENTIFIER{
        $$ = $1;
        $$.fnc_values = new FNC_VALUES(location: @$, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4), indexMode: (FNC_VALUES.IndexMode)EnumToStringOrAbort(typeof(FNC_VALUES.IndexMode), $5), addrType: (AddrType)EnumToStringOrAbort(typeof(AddrType), $6));
    }
    | record_layout_data IDENTIFICATION NUMBER IDENTIFIER {
        $$ = $1;
        $$.identification = new IDENTIFICATION(location: @$, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4));
    }
    | record_layout_data NO_AXIS_PTS_XYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.no_axis_pts_xyz45.Add($2, new NO_AXIS_PTS_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data NO_RESCALE_XYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.no_rescale_xyz45.Add($2, new NO_RESCALE_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data OFFSET_XYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.offset_xyz45.Add($2, new OFFSET_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data RESERVED NUMBER IDENTIFIER {
        $$ = $1;
        $$.reserved = new RESERVED(location: @$, Position: (UInt64)$3, dataSize: (DataSize)EnumToStringOrAbort(typeof(DataSize), $4));
    }
    | record_layout_data RIP_ADDR_WXYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.rip_addr_wxyz45.Add($2, new RIP_ADDR_WXYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data SHIFT_OP_XYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.shift_op_xyz45.Add($2, new SHIFT_OP_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data SRC_ADDR_XYZ45 NUMBER IDENTIFIER {
        $$ = $1;
        try
        {
            $$.src_addr_xyz45.Add($2, new SRC_ADDR_XYZ45(location: @$, Name: $2, Position: (UInt64)$3, dataType: (DataType)EnumToStringOrAbort(typeof(DataType), $4)));
        }
        catch (ArgumentException)
        {
            Scanner.yyerror(String.Format("Warning: Duplicate '{0}' found, ignoring", $2));
        }
    }
    | record_layout_data STATIC_RECORD_LAYOUT {
        $$ = $1;
        $$.static_record_layout = new STATIC_RECORD_LAYOUT(@$);
    }
    ;

sub_group                   : BEGIN SUB_GROUP sub_group_data END SUB_GROUP {
                                $$ = $3;
                            }
                            ;

sub_group_data              : /* start */  {
                                $$ = new SUB_GROUP(@$);
                            }
                            |  sub_group_data IDENTIFIER {
                                $$ = $1;
                                $$.groups.Add($2);
                            }
                            ;

unit
    : BEGIN UNIT unit_data END UNIT {
        $$ = $3;
    }
    ;

unit_data
    : IDENTIFIER QUOTED_STRING QUOTED_STRING IDENTIFIER {
        $$ = new UNIT(location: @$, Name: $1, LongIdentifier: $2, Display: $3, type: (UNIT.Type)EnumToStringOrAbort(typeof(UNIT.Type), $4));
    }
    |  unit_data REF_UNIT IDENTIFIER {
        $$ = $1;
        $$.ref_unit = $3;
    }
    |  unit_data SI_EXPONENTS NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER NUMBER {
        $$ = $1;
        $$.si_exponents = new SI_EXPONENTS(@$, (Int64)$3, (Int64)$4, (Int64)$5, (Int64)$6, (Int64)$7, (Int64)$8, (Int64)$9);
    }
    |  unit_data UNIT_CONVERSION NUMBER NUMBER {
        $$ = $1;
        $$.unit_conversion = new UNIT_CONVERSION(@$, $3, $4);
    }
    ;

user_rights
    : BEGIN USER_RIGHTS user_rights_data END USER_RIGHTS {
        $$ = $3;
    }
    ;

user_rights_data
    : IDENTIFIER {
        $$ = new USER_RIGHTS(@$, $1);
    }
    |  user_rights_data BEGIN REF_GROUP IDENTIFIER_list END REF_GROUP {
        var ref_group = new REF_GROUP(@$);
        ref_group.reference = $4;
        $$ = $1;
        $$.ref_group.Add(ref_group);
    }
    |  user_rights_data READ_ONLY {
        $$ = $1;
        $$.read_only = new READ_ONLY(@$);
    }
    ;

frame
    : BEGIN FRAME frame_data END FRAME {
        $$ = $3;
    }
    ;

frame_data
    : IDENTIFIER QUOTED_STRING NUMBER NUMBER {
        $$ = new FRAME(@$, $1, $2, (UInt64)$3, (UInt64)$4);
    }
    |  frame_data FRAME_MEASUREMENT IDENTIFIER_list {
        $$ = $1;
        $$.frame_measurement = $3;
    }
    | frame_data if_data {
        $$ = $1;
        $$.if_data.Add($2);
    }
    ;

variant_coding
    : BEGIN VARIANT_CODING variant_coding_data END VARIANT_CODING {
        $$ = $3;
    }
    ;

variant_coding_data
    : /* empty */ {
        $$ = new VARIANT_CODING(@$);
    }
    | variant_coding_data BEGIN VAR_CHARACTERISTIC var_characteristic END VAR_CHARACTERISTIC {
        $$ = $1;
        $$.var_characteristic.Add($4);
    }
    | variant_coding_data BEGIN VAR_CRITERION var_criterion END VAR_CRITERION {
        $$ = $1;
        $$.var_criterion.Add($4);
    }
    | variant_coding_data BEGIN VAR_FORBIDDEN_COMB var_forbidden_comb END VAR_FORBIDDEN_COMB {
        $$ = $1;
        $$.forbidden_combinations.Add($4);
    }
    | variant_coding_data VAR_SEPERATOR QUOTED_STRING {
        $$ = $1;
        $$.var_seperator = $3;
    }
    | variant_coding_data VAR_NAMING IDENTIFIER {
        $$ = $1;
        $$.var_naming = (VARIANT_CODING.VAR_NAMING)EnumToStringOrAbort(typeof(VARIANT_CODING.VAR_NAMING), $3);
    }
    ;

var_forbidden_comb
    : /* empty */ {
        $$ = new VAR_FORBIDDEN_COMB(@$);
    }
    | var_forbidden_comb IDENTIFIER IDENTIFIER {
        $$ = $1;
        $$.combinations.Add(new VAR_FORBIDDEN_COMB.Combo($2, $3));
    }
    ;

var_criterion
    : IDENTIFIER QUOTED_STRING {
        $$ = new VAR_CRITERION(@$, $1, $2);
    }
    |  var_criterion IDENTIFIER {
        $$ = $1;
        $$.Idents.Add($2);
    }
    | var_criterion VAR_MEASUREMENT IDENTIFIER {
        $$ = $1;
        $$.var_measurement = new VAR_MEASUREMENT(@$, $3);
    }
    | var_criterion VAR_SELECTION_CHARACTERISTIC IDENTIFIER {
        $$ = $1;
        $$.var_selection_characteristic = new VAR_SELECTION_CHARACTERISTIC(@$, $3);
    }
    ;

var_characteristic
    : IDENTIFIER {
        $$ = new VAR_CHARACTERISTIC(@$, $1);
    }
    |  var_characteristic IDENTIFIER {
        $$ = $1;
        $$.CriterionNames.Add($2);
    }
    | var_characteristic BEGIN VAR_ADDRESS var_address END VAR_ADDRESS {
        $$ = $1;
        $$.var_address = $4;
    }
    ;

var_address
    : /* Start of VAR_ADDRESS */ {
        $$ = new VAR_ADDRESS(@$);
    }
    |  var_address NUMBER {
        $$ = $1;
        $$.Addresses.Add((UInt64)$2);
    }
    ;
%%

private object EnumToStringOrAbort(Type type, string strIn)
{
    try
    {
       return Enum.Parse(type, strIn);
    }
    catch (ArgumentException e)
    {
        StringBuilder values = new StringBuilder();
        string[] myArray = Enum.GetNames(type);
        foreach(var item in myArray)
        {
            if (values.Length > 0)
            {
                values.Append(", ");
            }
            values.Append(item);
        }
        Scanner.yyerror(String.Format("Syntax error: Unknown '{0}' enum value '{1}' expecting one of '{2}'", type.ToString(), strIn, values.ToString()));
        YYAbort();
        throw e;
    }
}
